// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Util.Ui.Sources.Spa.StaticFiles;

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Clase que representa un middleware para manejar la p�gina predeterminada de una aplicaci�n SPA (Single Page Application).
/// </summary>
internal sealed class SpaDefaultPageMiddleware
{
    /// <summary>
    /// Adjunta el middleware de una aplicaci�n de una sola p�gina (SPA) al constructor de la aplicaci�n.
    /// </summary>
    /// <param name="spaBuilder">El constructor de la SPA que contiene la configuraci�n y el constructor de la aplicaci�n.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="spaBuilder"/> es nulo.</exception>
    /// <remarks>
    /// Este m�todo reescribe todas las solicitudes a la p�gina predeterminada especificada en las opciones de la SPA.
    /// Si hay un Endpoint presente, se omite la reescritura y se contin�a con el siguiente middleware.
    /// Adem�s, sirve la p�gina predeterminada como un archivo est�tico, permitiendo la posibilidad de 
    /// alojar m�ltiples SPAs con distintas p�ginas predeterminadas mediante la anulaci�n del proveedor de archivos.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    public static void Attach(ISpaBuilder spaBuilder)
    {
        ArgumentNullException.ThrowIfNull(spaBuilder);

        var app = spaBuilder.ApplicationBuilder;
        var options = spaBuilder.Options;

        // Rewrite all requests to the default page
        app.Use((context, next) =>
        {
            // If we have an Endpoint, then this is a deferred match - just noop.
            if (context.GetEndpoint() != null)
            {
                return next(context);
            }

            context.Request.Path = options.DefaultPage;
            return next(context);
        });

        // Serve it as a static file
        // Developers who need to host more than one SPA with distinct default pages can
        // override the file provider
        app.UseSpaStaticFilesInternal(
            options.DefaultPageStaticFileOptions ?? new StaticFileOptions(),
            allowFallbackOnServingWebRootFiles: true);
    }
}