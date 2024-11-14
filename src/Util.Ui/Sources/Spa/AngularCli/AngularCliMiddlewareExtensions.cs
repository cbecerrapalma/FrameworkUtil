// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa.AngularCli;

/// <summary>
/// Proporciona m�todos de extensi�n para configurar el middleware de Angular CLI.
/// </summary>
public static class AngularCliMiddlewareExtensions
{
    /// <summary>
    /// Configura el servidor Angular CLI para la aplicaci�n de una SPA (Single Page Application).
    /// </summary>
    /// <param name="spaBuilder">El objeto <see cref="ISpaBuilder"/> que se utiliza para construir la SPA.</param>
    /// <param name="npmScript">El nombre del script de npm que se utilizar� para iniciar el servidor Angular CLI.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="spaBuilder"/> es nulo.</exception>
    /// <exception cref="InvalidOperationException">Se lanza si la propiedad <see cref="SpaOptions.SourcePath"/> es nula o vac�a.</exception>
    /// <remarks>
    /// Este m�todo adjunta el middleware de Angular CLI al <paramref name="spaBuilder"/>. 
    /// Aseg�rese de que la propiedad <see cref="SpaOptions.SourcePath"/> est� configurada correctamente 
    /// antes de llamar a este m�todo.
    /// </remarks>
    public static void UseAngularCliServer(
        this ISpaBuilder spaBuilder,
        string npmScript)
    {
        ArgumentNullException.ThrowIfNull(spaBuilder);
        var spaOptions = spaBuilder.Options;
        if (string.IsNullOrEmpty(spaOptions.SourcePath))
        {
            throw new InvalidOperationException($"Para usar {nameof(UseAngularCliServer)}, debes proporcionar un valor no vac�o para la propiedad {nameof(SpaOptions.SourcePath)} de {nameof(SpaOptions)}.");
        }
        AngularCliMiddleware.Attach(spaBuilder, npmScript);
    }
}