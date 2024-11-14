using Util.Configs;

namespace Util.Events;

/// <summary>
/// Proporciona métodos de extensión para la configuración de aplicaciones.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="IAppBuilder"/> para agregar MediatR al contenedor de servicios.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <returns>La misma instancia de <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método configura MediatR para que escanee los ensamblados en busca de manejadores de solicitudes y eventos.
    /// Además, se registra <see cref="ILocalEventBus"/> con una implementación de <see cref="MediatREventBus"/>.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="ILocalEventBus"/>
    /// <seealso cref="MediatREventBus"/>
    public static IAppBuilder AddMediatR( this IAppBuilder builder ) {
        builder.CheckNull( nameof( builder ) );
        MediatROptions.IsScan = true;
        builder.Host.ConfigureServices( ( context, services ) => {
            services.TryAddTransient<ILocalEventBus, MediatREventBus>();
        } );
        return builder;
    }

    /// <summary>
    /// Extensión para agregar MediatR al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El builder de la aplicación que se está configurando.</param>
    /// <param name="setupAction">Acción para configurar el servicio de MediatR.</param>
    /// <returns>El mismo builder para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite la integración de MediatR en la aplicación, registrando
    /// el bus de eventos local y configurando los servicios necesarios para su uso.
    /// </remarks>
    /// <seealso cref="MediatRServiceConfiguration"/>
    /// <seealso cref="ILocalEventBus"/>
    public static IAppBuilder AddMediatR( this IAppBuilder builder, Action<MediatRServiceConfiguration> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.TryAddTransient<ILocalEventBus, MediatREventBus>();
            services.AddMediatR( setupAction );
        } );
        return builder;
    }
}