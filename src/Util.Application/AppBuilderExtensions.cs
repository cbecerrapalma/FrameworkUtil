using Util.Applications.Locks;
using Util.Configs;

namespace Util.Applications;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un servicio de bloqueo al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <returns>El mismo objeto <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método verifica que el <paramref name="builder"/> no sea nulo y luego configura
    /// el contenedor de servicios para que incluya una implementación por defecto de <see cref="ILock"/>.
    /// </remarks>
    /// <seealso cref="ILock"/>
    /// <seealso cref="DefaultLock"/>
    public static IAppBuilder AddLock( this IAppBuilder builder ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.TryAddTransient<ILock, DefaultLock>();
        } );
        return builder;
    }
}