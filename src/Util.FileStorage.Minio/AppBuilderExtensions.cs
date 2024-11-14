using Util.Configs;

namespace Util.FileStorage.Minio;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega el servicio Minio al contenedor de aplicaciones.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con el servicio Minio agregado.</returns>
    public static IAppBuilder AddMinio( this IAppBuilder builder ) {
        return builder.AddMinio( null );
    }

    /// <summary>
    /// Agrega el servicio de Minio al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el servicio de Minio.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de Minio.</param>
    /// <returns>El mismo objeto <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método verifica que el <paramref name="builder"/> no sea nulo y configura los servicios necesarios para 
    /// utilizar Minio como un sistema de almacenamiento de archivos. Si se proporciona <paramref name="setupAction"/>, 
    /// se aplicará para configurar las opciones de Minio.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    public static IAppBuilder AddMinio( this IAppBuilder builder, Action<MinioOptions> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            if ( setupAction != null )
                services.Configure( setupAction );
            services.TryAddTransient<IFileStore, MinioFileStore>();
        } );
        return builder;
    }
}