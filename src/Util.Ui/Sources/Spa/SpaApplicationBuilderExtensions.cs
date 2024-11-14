// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Util.Ui.Sources.Spa;

/// <summary>
/// Proporciona métodos de extensión para configurar aplicaciones SPA (Single Page Application).
/// </summary>
public static class SpaApplicationBuilderExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="IApplicationBuilder"/> para configurar una aplicación de una sola página (SPA).
    /// </summary>
    /// <param name="app">La instancia de <see cref="IApplicationBuilder"/> que se está configurando.</param>
    /// <param name="configuration">Una acción que permite configurar el <see cref="ISpaBuilder"/> para la aplicación.</param>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="configuration"/> es nulo.</exception>
    /// <remarks>
    /// Este método permite establecer las opciones y la configuración necesarias para que la aplicación funcione como una SPA.
    /// Se utiliza un <see cref="DefaultSpaBuilder"/> para facilitar la configuración de la SPA.
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