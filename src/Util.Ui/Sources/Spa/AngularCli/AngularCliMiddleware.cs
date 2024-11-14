// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Util.Ui.Razor;
using Util.Ui.Sources.Spa.Npm;
using Util.Ui.Sources.Spa.Proxying;
using Util.Ui.Sources.Spa.Util;

namespace Util.Ui.Sources.Spa.AngularCli;

/// <summary>
/// Clase que proporciona middleware para integrar Angular CLI en una aplicaci�n ASP.NET.
/// </summary>
internal static class AngularCliMiddleware
{
    private const string LogCategoryName = "Microsoft.AspNetCore.SpaServices";

    /// <summary>
    /// Adjunta un servidor de desarrollo Angular a la canalizaci�n de middleware.
    /// </summary>
    /// <param name="spaBuilder">El objeto <see cref="ISpaBuilder"/> que configura la aplicaci�n SPA.</param>
    /// <param name="scriptName">El nombre del script que se utilizar� para iniciar el servidor Angular.</param>
    /// <exception cref="ArgumentException">Se lanza si <paramref name="spaBuilder"/> o <paramref name="scriptName"/> son nulos o vac�os.</exception>
    /// <remarks>
    /// Este m�todo verifica que las propiedades necesarias est�n configuradas en <paramref name="spaBuilder"/>.
    /// Si el puerto del servidor de desarrollo no est� especificado, se busca un puerto disponible.
    /// Luego, se inicia el servidor Angular CLI y se conecta a la canalizaci�n de middleware.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    /// <seealso cref="TcpPortFinder"/>
    /// <seealso cref="RazorWatchService"/>
    public static void Attach(ISpaBuilder spaBuilder, string scriptName)
    {
        var pkgManagerCommand = spaBuilder.Options.PackageManagerCommand;
        var sourcePath = spaBuilder.Options.SourcePath;
        var devServerPort = spaBuilder.Options.DevServerPort;
        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new ArgumentException("La propiedad 'SourcePath' no puede ser nula ni estar vac�a.", nameof(spaBuilder));
        }

        if (string.IsNullOrEmpty(scriptName))
        {
            throw new ArgumentException("No puede ser nulo o vac�o.", nameof(scriptName));
        }

        if (devServerPort == default)
        {
            devServerPort = TcpPortFinder.FindAvailablePort();
        }

        // Start Angular CLI and attach to middleware pipeline
        var appBuilder = spaBuilder.ApplicationBuilder;
        var applicationStoppingToken = appBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
        var logger = LoggerFinder.GetOrCreateLogger(appBuilder, LogCategoryName);
        var diagnosticSource = appBuilder.ApplicationServices.GetRequiredService<DiagnosticSource>();
        var angularCliServerInfoTask = GetUrl(devServerPort, applicationStoppingToken);
        spaBuilder.UseProxyToSpaDevelopmentServer(() =>
        {
            var timeout = spaBuilder.Options.StartupTimeout;
            return angularCliServerInfoTask.WithTimeout(timeout,
                $"El proceso de Angular CLI no comenz� a escuchar solicitudes. " +
                $"dentro del per�odo de tiempo de {timeout.TotalSeconds} segundos. " +
                $"Verifica la salida del registro para obtener informaci�n sobre errores.");
        });
        Task.Factory.StartNew(async () =>
        {
            while (true)
            {
                if (RazorWatchService.IsStartComplete == false)
                {
                    await Task.Delay(500, applicationStoppingToken);
                    continue;
                }
                await StartAngularCliServerAsync(sourcePath, scriptName, pkgManagerCommand, devServerPort, logger, diagnosticSource, applicationStoppingToken);
                return;
            }
        }, applicationStoppingToken);
    }

    /// <summary>
    /// Obtiene la URL del servidor Angular CLI en ejecuci�n.
    /// </summary>
    /// <param name="portNumber">El n�mero de puerto en el que se est� ejecutando el servidor Angular CLI.</param>
    /// <param name="applicationStoppingToken">Token de cancelaci�n que se utiliza para notificar cuando la aplicaci�n est� deteni�ndose.</param>
    /// <returns>
    /// Una tarea que representa la operaci�n as�ncrona, con un valor que contiene la URI del servidor Angular CLI.
    /// </returns>
    /// <remarks>
    /// Este m�todo espera hasta que el servidor Angular CLI acepte solicitudes antes de devolver la URI.
    /// </remarks>
    private static async Task<Uri> GetUrl(int portNumber, CancellationToken applicationStoppingToken)
    {
        var uri = new Uri($"http://localhost:{portNumber}");
        await WaitForAngularCliServerToAcceptRequests(uri, applicationStoppingToken);
        return uri;
    }

    /// <summary>
    /// Inicia el servidor Angular CLI de manera as�ncrona.
    /// </summary>
    /// <param name="sourcePath">La ruta de origen donde se encuentra el proyecto Angular.</param>
    /// <param name="scriptName">El nombre del script que se utilizar� para iniciar el servidor.</param>
    /// <param name="pkgManagerCommand">El comando del gestor de paquetes que se utilizar� para ejecutar el script.</param>
    /// <param name="portNumber">El n�mero de puerto en el que se ejecutar� el servidor.</param>
    /// <param name="logger">El registrador que se utilizar� para registrar informaci�n y errores.</param>
    /// <param name="diagnosticSource">La fuente de diagn�stico que se utilizar� para la supervisi�n.</param>
    /// <param name="applicationStoppingToken">El token de cancelaci�n que se utilizar� para detener la aplicaci�n.</param>
    /// <returns>Una tarea que representa la operaci�n as�ncrona.</returns>
    /// <remarks>
    /// Este m�todo espera a que el servidor Angular CLI indique que est� escuchando solicitudes.
    /// Si el script sale sin indicar que est� escuchando, se lanzar� una excepci�n.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza cuando el script de Angular CLI sale sin indicar que est� escuchando.
    /// </exception>
    private static async Task StartAngularCliServerAsync(string sourcePath, string scriptName, string pkgManagerCommand, int portNumber, ILogger logger, DiagnosticSource diagnosticSource, CancellationToken applicationStoppingToken)
    {
        Console.WriteLine($"dbug: Angular es un marco de trabajo para construir aplicaciones web. http://localhost:{portNumber}");
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation($"Iniciando @angular/cli en el puerto {portNumber}...");
        }

        var scriptRunner = new NodeScriptRunner(
            sourcePath, scriptName, $"--port {portNumber}", null, pkgManagerCommand, diagnosticSource, applicationStoppingToken);
        scriptRunner.AttachToLogger(logger);

        bool openBrowserLine;
        using var stdErrReader = new EventedStreamStringReader(scriptRunner.StdErr);
        try
        {
            openBrowserLine = await scriptRunner.StdOut.WaitForMatch(["Modo de visualizaci�n habilitado.", "Desarrollo en vivo de Angular"]);
        }
        catch (EndOfStreamException ex)
        {
            throw new InvalidOperationException(
                $"El script '{scriptName}' de {pkgManagerCommand} sali� sin indicar que el " +
                $"Angular CLI estaba escuchando solicitudes. La salida de error fue: " +
                $"{stdErrReader.ReadAsString()}", ex);
        }
    }

    /// <summary>
    /// Espera hasta que el servidor Angular CLI acepte solicitudes.
    /// </summary>
    /// <param name="cliServerUri">La URI del servidor Angular CLI que se est� esperando.</param>
    /// <param name="applicationStoppingToken">Token de cancelaci�n que se utiliza para detener la operaci�n si es necesario.</param>
    /// <remarks>
    /// Este m�todo realiza solicitudes peri�dicas al servidor Angular CLI hasta que recibe una respuesta con contenido no vac�o o se alcanza un l�mite de intentos.
    /// Si el servidor no est� disponible o no responde, el m�todo ignorar� las excepciones y continuar� intentando.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operaci�n as�ncrona.
    /// </returns>
    private static async Task WaitForAngularCliServerToAcceptRequests(Uri cliServerUri, CancellationToken applicationStoppingToken)
    {
        using var client = new HttpClient();
        var i = 0;
        while (true)
        {
            try
            {
                i++;
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, cliServerUri), applicationStoppingToken);
                var content = await response.Content.ReadAsStringAsync(applicationStoppingToken);
                if (content.IsEmpty() == false)
                {
                    return;
                }
                await Task.Delay(300, applicationStoppingToken);
                if (i > 1000)
                {
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}