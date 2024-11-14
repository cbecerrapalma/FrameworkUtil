// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Util.Ui.Sources.Spa.Npm;
using Util.Ui.Sources.Spa.Prerendering;
using Util.Ui.Sources.Spa.Util;

namespace Util.Ui.Sources.Spa.AngularCli;

/// <summary>
/// Clase que implementa la interfaz <see cref="ISpaPrerendererBuilder"/>.
/// Proporciona funcionalidades para construir y configurar un prerenderizador de aplicaciones de una sola p�gina (SPA) utilizando Angular CLI.
/// </summary>
public class AngularCliBuilder : ISpaPrerendererBuilder
{
    private static readonly TimeSpan RegexMatchTimeout = TimeSpan.FromSeconds(5); // This is a development-time only feature, so a very long timeout is fine

    private readonly string _scriptName;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AngularCliBuilder"/>.
    /// </summary>
    /// <param name="npmScript">El nombre del script de npm que se va a ejecutar. No puede ser nulo o vac�o.</param>
    /// <exception cref="ArgumentException">Se lanza cuando <paramref name="npmScript"/> es nulo o vac�o.</exception>
    public AngularCliBuilder(string npmScript)
    {
        if (string.IsNullOrEmpty(npmScript))
        {
            throw new ArgumentException("No puede ser nulo ni estar vac�o.", nameof(npmScript));
        }

        _scriptName = npmScript;
    }

    /// <inheritdoc />
    /// <summary>
    /// Construye la aplicaci�n SPA utilizando el comando del administrador de paquetes especificado.
    /// </summary>
    /// <param name="spaBuilder">El constructor de la aplicaci�n SPA que contiene las opciones de configuraci�n.</param>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la propiedad <see cref="SpaOptions.SourcePath"/> est� vac�a o es nula.
    /// </exception>
    /// <remarks>
    /// Este m�todo inicia un script de Node.js para la construcci�n de la aplicaci�n Angular en modo de observaci�n.
    /// Se asegura de que el script se ejecute correctamente y maneja las excepciones que pueden ocurrir durante su ejecuci�n.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    /// <seealso cref="SpaOptions"/>
    /// <seealso cref="NodeScriptRunner"/>
    public async Task Build(ISpaBuilder spaBuilder)
    {
        var pkgManagerCommand = spaBuilder.Options.PackageManagerCommand;
        var sourcePath = spaBuilder.Options.SourcePath;
        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new InvalidOperationException($"Para usar {nameof(AngularCliBuilder)}, debes proporcionar un valor no vac�o para la propiedad {nameof(SpaOptions.SourcePath)} de {nameof(SpaOptions)}.");
        }

        var appBuilder = spaBuilder.ApplicationBuilder;
        var applicationStoppingToken = appBuilder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
        var logger = LoggerFinder.GetOrCreateLogger(
            appBuilder,
            nameof(AngularCliBuilder));
        var diagnosticSource = appBuilder.ApplicationServices.GetRequiredService<DiagnosticSource>();
        var scriptRunner = new NodeScriptRunner(
            sourcePath,
            _scriptName,
            "--watch",
            null,
            pkgManagerCommand,
            diagnosticSource,
            applicationStoppingToken);
        scriptRunner.AttachToLogger(logger);

        using var stdOutReader = new EventedStreamStringReader(scriptRunner.StdOut);
        using var stdErrReader = new EventedStreamStringReader(scriptRunner.StdErr);
        try
        {
            await scriptRunner.StdOut.WaitForMatch(["Date"]);
        }
        catch (EndOfStreamException ex)
        {
            throw new InvalidOperationException(
                $"El script '{_scriptName}' del {pkgManagerCommand} sali� sin indicar �xito.\n" +
                $"La salida fue: {stdOutReader.ReadAsString()}\n" +
                $"La salida de error fue: {stdErrReader.ReadAsString()}", ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new InvalidOperationException(
                $"El script de {pkgManagerCommand} '{_scriptName}' se agot� sin indicar �xito. " +
                $"La salida fue: {stdOutReader.ReadAsString()}\n" +
                $"La salida de error fue: {stdErrReader.ReadAsString()}", ex);
        }
    }
}