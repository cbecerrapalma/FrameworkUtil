namespace Util;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega un control de acceso (ACL) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le agregará el ACL.</param>
    /// <returns>El constructor de la aplicación modificado con el ACL agregado.</returns>
    /// <seealso cref="DefaultPermissionManager"/>
    public static IAppBuilder AddAcl( this IAppBuilder builder ) {
        return builder.AddAcl<DefaultPermissionManager>();
    }

    /// <summary>
    /// Agrega la funcionalidad de Control de Acceso (ACL) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará la funcionalidad ACL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de ACL.</param>
    /// <returns>El constructor de la aplicación con la funcionalidad ACL añadida.</returns>
    public static IAppBuilder AddAcl( this IAppBuilder builder, Action<AclOptions> setupAction ) {
        return builder.AddAcl<DefaultPermissionManager>( setupAction );
    }

    /// <summary>
    /// Agrega un middleware de control de acceso a la aplicación.
    /// </summary>
    /// <typeparam name="TPermissionManager">
    /// El tipo de administrador de permisos que se utilizará. Debe ser una clase que implemente la interfaz <see cref="IPermissionManager"/>.
    /// </typeparam>
    /// <param name="builder">
    /// La instancia de <see cref="IAppBuilder"/> sobre la cual se agrega el middleware.
    /// </param>
    /// <returns>
    /// La instancia de <see cref="IAppBuilder"/> con el middleware de control de acceso agregado.
    /// </returns>
    /// <remarks>
    /// Este método permite configurar un sistema de control de acceso utilizando un administrador de permisos específico.
    /// </remarks>
    /// <seealso cref="IPermissionManager"/>
    /// <seealso cref="AclMiddlewareResultHandler"/>
    public static IAppBuilder AddAcl<TPermissionManager>( this IAppBuilder builder ) where TPermissionManager : class, IPermissionManager {
        return builder.AddAcl<TPermissionManager, AclMiddlewareResultHandler>();
    }

    /// <summary>
    /// Agrega la funcionalidad de Control de Acceso (ACL) al pipeline de la aplicación.
    /// </summary>
    /// <typeparam name="TPermissionManager">El tipo de administrador de permisos que se utilizará.</typeparam>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le agregará la funcionalidad ACL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de ACL.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con la funcionalidad ACL agregada.</returns>
    /// <remarks>
    /// Este método permite extender el pipeline de la aplicación con un middleware que gestiona el acceso basado en permisos.
    /// </remarks>
    /// <seealso cref="AclOptions"/>
    /// <seealso cref="IPermissionManager"/>
    public static IAppBuilder AddAcl<TPermissionManager>( this IAppBuilder builder, Action<AclOptions> setupAction ) where TPermissionManager : class, IPermissionManager {
        return builder.AddAcl<TPermissionManager, AclMiddlewareResultHandler>( setupAction );
    }

    /// <summary>
    /// Agrega un control de acceso basado en permisos a la configuración de la aplicación.
    /// </summary>
    /// <typeparam name="TPermissionManager">El tipo de administrador de permisos que implementa <see cref="IPermissionManager"/>.</typeparam>
    /// <typeparam name="TAuthorizationMiddlewareResultHandler">El tipo de manejador de resultados de middleware de autorización que implementa <see cref="IAuthorizationMiddlewareResultHandler"/>.</typeparam>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> a la que se le agrega el control de acceso.</param>
    /// <returns>La instancia de <see cref="IAppBuilder"/> con el control de acceso agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar el control de acceso sin un parámetro adicional.
    /// </remarks>
    /// <seealso cref="AddAcl{TPermissionManager, TAuthorizationMiddlewareResultHandler}(IAppBuilder, object)"/>
    public static IAppBuilder AddAcl<TPermissionManager, TAuthorizationMiddlewareResultHandler>( this IAppBuilder builder )
        where TPermissionManager : class, IPermissionManager
        where TAuthorizationMiddlewareResultHandler : class, IAuthorizationMiddlewareResultHandler {
        return builder.AddAcl<TPermissionManager, TAuthorizationMiddlewareResultHandler>( null );
    }

    /// <summary>
    /// Agrega la configuración de control de acceso a la aplicación.
    /// </summary>
    /// <typeparam name="TPermissionManager">El tipo de administrador de permisos que se utilizará.</typeparam>
    /// <typeparam name="TAuthorizationMiddlewareResultHandler">El tipo de manejador de resultados del middleware de autorización.</typeparam>
    /// <param name="builder">El constructor de la aplicación donde se añadirá la configuración de ACL.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de ACL.</param>
    /// <returns>El constructor de la aplicación con la configuración de ACL añadida.</returns>
    /// <remarks>
    /// Este método permite configurar el control de acceso en la aplicación utilizando un administrador de permisos y un manejador de resultados de autorización específicos.
    /// Se registran los servicios necesarios para la autorización y se aplica la configuración proporcionada a través de <paramref name="setupAction"/>.
    /// </remarks>
    /// <seealso cref="IAuthorizationHandler"/>
    /// <seealso cref="IAuthorizationPolicyProvider"/>
    /// <seealso cref="IPermissionManager"/>
    public static IAppBuilder AddAcl<TPermissionManager, TAuthorizationMiddlewareResultHandler>( this IAppBuilder builder, Action<AclOptions> setupAction )
        where TPermissionManager : class, IPermissionManager
        where TAuthorizationMiddlewareResultHandler : class, IAuthorizationMiddlewareResultHandler {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.AddSingleton<IAuthorizationHandler, AclHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, AclPolicyProvider>();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, TAuthorizationMiddlewareResultHandler>();
            services.AddSingleton<IPermissionManager, TPermissionManager>();
            if ( setupAction != null )
                services.Configure( setupAction );
        } );
        return builder;
    }
}