namespace Util;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="WebApplicationBuilder"/>.
/// </summary>
public static class WebApplicationBuilderExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="WebApplicationBuilder"/> para crear una instancia de <see cref="IAppBuilder"/>.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="WebApplicationBuilder"/> que se va a extender.</param>
    /// <returns>Una instancia de <see cref="IAppBuilder"/> que se ha creado a partir del <paramref name="builder"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    /// <remarks>
    /// Este método permite a los desarrolladores utilizar un <see cref="WebApplicationBuilder"/> para construir aplicaciones
    /// de manera más flexible mediante la creación de un <see cref="IAppBuilder"/>.
    /// </remarks>
    public static IAppBuilder AsBuild( this WebApplicationBuilder builder ) {
        builder.CheckNull( nameof( builder ) );
        return new AppBuilder( builder.Host );
    }
}