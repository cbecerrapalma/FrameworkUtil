using Util.Configs;
using Util.Tenants.Resolvers;

namespace Util.Tenants;

/// <summary>
/// Proporciona métodos de extensión para la configuración de aplicaciones.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un inquilino (tenant) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará el inquilino.</param>
    /// <returns>El constructor de la aplicación actualizado con el inquilino agregado.</returns>
    public static IAppBuilder AddTenant( this IAppBuilder builder ) {
        return builder.AddTenant( null );
    }

    /// <summary>
    /// Agrega un servicio de gestión de inquilinos al contenedor de servicios.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones del inquilino.</param>
    /// <returns>El mismo objeto <see cref="IAppBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite establecer opciones específicas para la gestión de inquilinos
    /// y registrar los servicios necesarios en el contenedor de servicios.
    /// </remarks>
    /// <seealso cref="TenantOptions"/>
    /// <seealso cref="ITenantResolver"/>
    /// <seealso cref="DefaultTenantResolver"/>
    public static IAppBuilder AddTenant( this IAppBuilder builder, Action<TenantOptions> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        var options = new TenantOptions {
            IsEnabled = true
        };
        setupAction?.Invoke( options );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.TryAddSingleton<ITenantResolver, DefaultTenantResolver>();
            services.TryAddSingleton<IOptions<TenantOptions>>( new OptionsWrapper<TenantOptions>( options ) );
        } );
        return builder;
    }
}