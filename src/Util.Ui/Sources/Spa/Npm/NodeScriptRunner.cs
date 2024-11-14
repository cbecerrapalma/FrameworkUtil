// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Util.Ui.Sources.Spa.Util;

// This is under the NodeServices namespace because post 2.1 it will be moved to that package
namespace Util.Ui.Sources.Spa.Npm;

/// <summary>
/// Clase que se encarga de ejecutar scripts de nodos.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IDisposable"/> para liberar recursos no administrados.
/// </remarks>
internal sealed class NodeScriptRunner : IDisposable
{
    private Process _npmProcess;
    /// <summary>
    /// Obtiene una instancia de <see cref="EventedStreamReader"/> que representa la salida estándar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a la salida estándar del sistema, facilitando la lectura de datos
    /// que se envían a la consola o a otros flujos de salida estándar.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="EventedStreamReader"/> que permite leer desde la salida estándar.
    /// </returns>
    public EventedStreamReader StdOut { get; }
    /// <summary>
    /// Obtiene una instancia de <see cref="EventedStreamReader"/> que representa la salida de error estándar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite leer los datos de error generados por el proceso en ejecución.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="EventedStreamReader"/> que permite la lectura de la salida de error estándar.
    /// </returns>
    public EventedStreamReader StdErr { get; }

    private static readonly Regex AnsiColorRegex = new Regex("\x001b\\[[0-9;]*m", RegexOptions.None, TimeSpan.FromSeconds(1));

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NodeScriptRunner"/>.
    /// </summary>
    /// <param name="workingDirectory">El directorio de trabajo donde se ejecutará el script.</param>
    /// <param name="scriptName">El nombre del script que se desea ejecutar.</param>
    /// <param name="arguments">Los argumentos que se pasarán al script.</param>
    /// <param name="envVars">Un diccionario de variables de entorno que se establecerán para el proceso.</param>
    /// <param name="pkgManagerCommand">El comando del gestor de paquetes a utilizar.</param>
    /// <param name="diagnosticSource">La fuente de diagnóstico para registrar eventos.</param>
    /// <param name="applicationStoppingToken">Un token que se puede utilizar para cancelar la operación si la aplicación se detiene.</param>
    /// <exception cref="ArgumentException">Se lanza si <paramref name="workingDirectory"/>, <paramref name="scriptName"/> o <paramref name="pkgManagerCommand"/> son nulos o están vacíos.</exception>
    /// <remarks>
    /// Este constructor configura la información del proceso y lanza el proceso del script de Node.js utilizando el gestor de paquetes especificado.
    /// Dependiendo del sistema operativo, puede que se necesite invocar el comando a través de "cmd" en Windows.
    /// </remarks>
    /// <seealso cref="NodeScriptRunner"/>
    public NodeScriptRunner(string workingDirectory, string scriptName, string arguments, IDictionary<string, string> envVars, string pkgManagerCommand, DiagnosticSource diagnosticSource, CancellationToken applicationStoppingToken)
    {
        if (string.IsNullOrEmpty(workingDirectory))
        {
            throw new ArgumentException("No puede ser nulo o vacío.", nameof(workingDirectory));
        }

        if (string.IsNullOrEmpty(scriptName))
        {
            throw new ArgumentException("No puede ser nulo ni estar vacío.", nameof(scriptName));
        }

        if (string.IsNullOrEmpty(pkgManagerCommand))
        {
            throw new ArgumentException("No puede ser nulo ni estar vacío.", nameof(pkgManagerCommand));
        }

        var exeToRun = pkgManagerCommand;
        var completeArguments = $"run {scriptName} -- {arguments ?? string.Empty}";
        if (OperatingSystem.IsWindows())
        {
            // On Windows, the node executable is a .cmd file, so it can't be executed
            // directly (except with UseShellExecute=true, but that's no good, because
            // it prevents capturing stdio). So we need to invoke it via "cmd /c".
            exeToRun = "cmd";
            completeArguments = $"/c {pkgManagerCommand} {completeArguments}";
        }

        var processStartInfo = new ProcessStartInfo(exeToRun)
        {
            Arguments = completeArguments,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = workingDirectory
        };

        if (envVars != null)
        {
            foreach (var keyValuePair in envVars)
            {
                processStartInfo.Environment[keyValuePair.Key] = keyValuePair.Value;
            }
        }

        _npmProcess = LaunchNodeProcess(processStartInfo, pkgManagerCommand);
        StdOut = new EventedStreamReader(_npmProcess.StandardOutput);
        StdErr = new EventedStreamReader(_npmProcess.StandardError);

        applicationStoppingToken.Register(((IDisposable)this).Dispose);

        if (diagnosticSource.IsEnabled("Microsoft.AspNetCore.NodeServices.Npm.NpmStarted"))
        {
            WriteDiagnosticEvent(
                diagnosticSource,
                "Microsoft.AspNetCore.NodeServices.Npm.NpmStarted",
                new
                {
                    processStartInfo = processStartInfo,
                    process = _npmProcess
                });
        }

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026",
            Justification = "Los valores que se pasan a Write tienen las propiedades de uso común preservadas con DynamicDependency.")]
        static void WriteDiagnosticEvent<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(DiagnosticSource diagnosticSource, string name, TValue value)
            => diagnosticSource.Write(name, value);
    }

    /// <summary>
    /// Adjunta un registrador (logger) al sistema de salida estándar y de error.
    /// </summary>
    /// <param name="logger">El registrador que se utilizará para registrar la información y advertencias.</param>
    /// <remarks>
    /// Este método se suscribe a los eventos de recepción de líneas de salida estándar y de error.
    /// Cuando se recibe una línea completa en la salida estándar, se registra como información si el registrador está habilitado para el nivel de información.
    /// Las líneas en la salida de error se registran como advertencias.
    /// Además, si se reciben fragmentos de salida que no contienen nuevas líneas, se imprimen directamente en la consola.
    /// </remarks>
    /// <seealso cref="ILogger"/>
    public void AttachToLogger(ILogger logger)
    {
        // When the node task emits complete lines, pass them through to the real logger
        StdOut.OnReceivedLine += line =>
        {
            if (!string.IsNullOrWhiteSpace(line) && logger.IsEnabled(LogLevel.Information))
            {
                // Node tasks commonly emit ANSI colors, but it wouldn't make sense to forward
                // those to loggers (because a logger isn't necessarily any kind of terminal)
                logger.LogInformation(StripAnsiColors(line));
            }
        };

        StdErr.OnReceivedLine += line =>
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                logger.LogWarning(StripAnsiColors(line));
            }
        };

        // But when it emits incomplete lines, assume this is progress information and
        // hence just pass it through to StdOut regardless of logger config.
        StdErr.OnReceivedChunk += chunk =>
        {
            Debug.Assert(chunk.Array != null);

            var containsNewline = Array.IndexOf(
                chunk.Array, '\n', chunk.Offset, chunk.Count) >= 0;
            if (!containsNewline)
            {
                Console.Write(chunk.Array, chunk.Offset, chunk.Count);
            }
        };
    }

    /// <summary>
    /// Elimina los códigos de color ANSI de una cadena de texto.
    /// </summary>
    /// <param name="line">La cadena de texto de la cual se eliminarán los códigos de color ANSI.</param>
    /// <returns>Una nueva cadena de texto sin los códigos de color ANSI.</returns>
    private static string StripAnsiColors(string line)
        => AnsiColorRegex.Replace(line, string.Empty);

    /// <summary>
    /// Inicia un proceso de nodo utilizando la información de inicio proporcionada.
    /// </summary>
    /// <param name="startInfo">La información de inicio del proceso que se va a lanzar.</param>
    /// <param name="commandName">El nombre del comando que se está intentando ejecutar.</param>
    /// <returns>
    /// Un objeto <see cref="Process"/> que representa el proceso iniciado.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si no se puede iniciar el proceso. La excepción incluye detalles sobre cómo resolver el problema.
    /// </exception>
    /// <remarks>
    /// Este método establece la propiedad <c>EnableRaisingEvents</c> en <c>true</c> para permitir la recepción de eventos de proceso.
    /// </remarks>
    private static Process LaunchNodeProcess(ProcessStartInfo startInfo, string commandName)
    {
        try
        {
            var process = Process.Start(startInfo)!;

            // See equivalent comment in OutOfProcessNodeInstance.cs for why
            process.EnableRaisingEvents = true;

            return process;
        }
        catch (Exception ex)
        {
            var message = $"No se pudo iniciar '{commandName}'. Para resolver esto:.\n\n"
                        + $"[1] Asegúrate de que '{commandName}' esté instalado y se pueda encontrar en uno de los directorios del PATH.\n"
                        + $"    La variable de entorno PATH actual es: {Environment.GetEnvironmentVariable("PATH")}\n"
                        + "    Asegúrate de que el ejecutable esté en uno de esos directorios o actualiza tu PATH.\n\n"
                        + "[2] Consulta la InnerException para obtener más detalles sobre la causa.";
            throw new InvalidOperationException(message, ex);
        }
    }

    /// <summary>
    /// Libera los recursos utilizados por la instancia de la clase.
    /// </summary>
    /// <remarks>
    /// Este método forma parte de la implementación de la interfaz <see cref="IDisposable"/>.
    /// Se asegura de que el proceso de npm se termine correctamente si está en ejecución.
    /// </remarks>
    /// <returns>
    /// Este método no devuelve ningún valor.
    /// </returns>
    void IDisposable.Dispose()
    {
        if (_npmProcess != null && !_npmProcess.HasExited)
        {
            _npmProcess.Kill(entireProcessTree: true);
            _npmProcess = null;
        }
    }
}