// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Proporciona m�todos de extensi�n para configurar aplicaciones SPA (Single Page Application).
/// </summary>
public static class SpaApplicationBuilderExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="IApplicationBuilder"/> para configurar una aplicaci�n de una sola p�gina (SPA).
    /// </summary>
    /// <param name="app">La instancia de <see cref="IApplicationBuilder"/> que se est� configurando.</param>
    /// <param name="configuration">Una acci�n que permite configurar el <see cref="ISpaBuilder"/> para la aplicaci�n.</param>
    /// <exception cref="ArgumentNullException">Se lanza si el par�metro <paramref name="configuration"/> es nulo.</exception>
    /// <remarks>
    /// Este m�todo permite establecer las opciones y la configuraci�n necesarias para que la aplicaci�n funcione como una SPA.
    /// Se utiliza un <see cref="DefaultSpaBuilder"/> para facilitar la configuraci�n de la SPA.
    /// </remarks>
    /// <seealso cref="ISpaBuilder"/>
    /// <seealso cref="SpaOptions"/>
    /// <seealso cref="SpaDefaultPageMiddleware"/>
    public static void UseAngular( this IApplicationBuilder app, Action<ISpaBuilder> configuration ) {
        ArgumentNullException.ThrowIfNull( configuration );
        var optionsProvider = app.ApplicationServices.GetService<IOptions<SpaOptions>>()!;
        var options = new SpaOptions( optionsProvider.Value );
        var spaBuilder = new DefaultSpaBuilder( app, options );
        configuration.Invoke( spaBuilder );
        SpaDefaultPageMiddleware.Attach( spaBuilder );
    }
}