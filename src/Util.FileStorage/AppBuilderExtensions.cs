using Util.Configs;
using Util.FileStorage.Local;

namespace Util.FileStorage;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un almacenamiento de archivos local al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le añadirá el almacenamiento de archivos local.</param>
    /// <returns>El mismo constructor de aplicaciones con el almacenamiento de archivos local agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar un almacenamiento de archivos local sin especificar parámetros adicionales.
    /// </remarks>
    public static IAppBuilder AddLocalFileStore( this IAppBuilder builder ) {
        return builder.AddLocalFileStore( null );
    }

    /// <summary>
    /// Agrega un almacenamiento de archivos local al contenedor de servicios.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que configura las opciones del almacenamiento local.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite la configuración del almacenamiento de archivos local mediante una acción que recibe un objeto de tipo <see cref="LocalStoreOptions"/>.
    /// Si se proporciona una acción de configuración, se aplicará a las opciones del almacenamiento local.
    /// Además, se registran las implementaciones de <see cref="IFileStore"/> y <see cref="ILocalFileStore"/> como servicios transitorios.
    /// </remarks>
    /// <seealso cref="LocalStoreOptions"/>
    /// <seealso cref="IFileStore"/>
    /// <seealso cref="ILocalFileStore"/>
    public static IAppBuilder AddLocalFileStore( this IAppBuilder builder, Action<LocalStoreOptions> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            if( setupAction != null )
                services.Configure( setupAction );
            services.TryAddTransient<IFileStore, LocalFileStore>();
            services.TryAddTransient<ILocalFileStore, LocalFileStore>();
        } );
        return builder;
    }
}