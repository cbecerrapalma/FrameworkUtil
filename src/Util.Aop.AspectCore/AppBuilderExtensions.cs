using Util.Configs;

namespace Util.Aop;

/// <summary>
/// Proporciona métodos de extensión para la configuración de la aplicación.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega la funcionalidad de AOP (Programación Orientada a Aspectos) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> al que se le añadirá la funcionalidad AOP.</param>
    /// <returns>
    /// Devuelve el mismo objeto <see cref="IAppBuilder"/> con la funcionalidad AOP añadida.
    /// </returns>
    public static IAppBuilder AddAop( this IAppBuilder builder ) {
        return builder.AddAop( false );
    }

    /// <summary>
    /// Agrega la funcionalidad de AOP (Programación Orientada a Aspectos) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le añadirá la funcionalidad AOP.</param>
    /// <param name="isEnableIAopProxy">Indica si se debe habilitar el proxy de AOP.</param>
    /// <returns>El constructor de la aplicación con la funcionalidad AOP añadida.</returns>
    public static IAppBuilder AddAop( this IAppBuilder builder,bool isEnableIAopProxy ) {
        return builder.AddAop( null, isEnableIAopProxy );
    }

    /// <summary>
    /// Agrega la funcionalidad de AOP (Programación Orientada a Aspectos) al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones al que se le agregará AOP.</param>
    /// <param name="setupAction">Una acción que permite configurar los aspectos.</param>
    /// <returns>
    /// El mismo constructor de aplicaciones con AOP agregado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar AOP sin especificar el uso de configuración adicional.
    /// </remarks>
    /// <seealso cref="AddAop(IAppBuilder, Action{IAspectConfiguration})"/>
    public static IAppBuilder AddAop( this IAppBuilder builder, Action<IAspectConfiguration> setupAction ) {
        return builder.AddAop( setupAction, false );
    }

    /// <summary>
    /// Agrega la configuración de AOP (Programación Orientada a Aspectos) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le añadirá la configuración de AOP.</param>
    /// <param name="setupAction">Una acción que permite configurar los aspectos.</param>
    /// <param name="isEnableIAopProxy">Indica si se debe habilitar el proxy de AOP.</param>
    /// <returns>El constructor de la aplicación con la configuración de AOP aplicada.</returns>
    private static IAppBuilder AddAop( this IAppBuilder builder, Action<IAspectConfiguration> setupAction, bool isEnableIAopProxy ) {
        return builder.AddAop( setupAction, t => t.IsEnableIAopProxy = isEnableIAopProxy );
    }

    /// <summary>
    /// Agrega la configuración de AOP (Programación Orientada a Aspectos) al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se añadirá la configuración de AOP.</param>
    /// <param name="setupAction">Acción que configura los aspectos mediante un objeto de configuración de aspecto.</param>
    /// <param name="action">Acción que permite establecer opciones adicionales para AOP.</param>
    /// <returns>El constructor de la aplicación con la configuración de AOP aplicada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad del constructor de la aplicación para incluir 
    /// la capacidad de utilizar AOP mediante la configuración de aspectos y opciones específicas.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    /// <seealso cref="IAspectConfiguration"/>
    /// <seealso cref="AopOptions"/>
    private static IAppBuilder AddAop( this IAppBuilder builder, Action<IAspectConfiguration> setupAction, Action<AopOptions> action ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.UseServiceProviderFactory( new DynamicProxyServiceProviderFactory() );
        builder.Host.ConfigureServices( ( context, services ) => {
            var options = new AopOptions();
            action?.Invoke( options );
            ConfigureDynamicProxy( services, setupAction, options.IsEnableParameterAspect,options.IsEnableIAopProxy );
            RegisterAspectScoped( services );
        } );
        return builder;
    }

    /// <summary>
    /// Configura el proxy dinámico para los servicios especificados.
    /// </summary>
    /// <param name="services">La colección de servicios donde se configurará el proxy dinámico.</param>
    /// <param name="setupAction">Una acción que permite configurar aspectos específicos del proxy.</param>
    /// <param name="isEnableParameterAspect">Indica si se debe habilitar el aspecto de parámetros.</param>
    /// <param name="isEnableIAopProxy">Indica si se debe habilitar el proxy AOP.</param>
    /// <remarks>
    /// Este método permite personalizar la configuración del proxy dinámico, 
    /// incluyendo la posibilidad de habilitar aspectos de parámetros y 
    /// definir predicados para determinar qué tipos no deben ser proxy.
    /// </remarks>
    /// <seealso cref="IAspectConfiguration"/>
    private static void ConfigureDynamicProxy( IServiceCollection services, Action<IAspectConfiguration> setupAction,bool isEnableParameterAspect, bool isEnableIAopProxy ) {
        services.ConfigureDynamicProxy( config => {
            if ( setupAction == null ) {
                config.NonAspectPredicates.Add( t => !IsProxy( t.DeclaringType, isEnableIAopProxy ) );
                if( isEnableParameterAspect )
                    config.EnableParameterAspect();
                return;
            }
            setupAction.Invoke( config );
        } );
    }

    /// <summary>
    /// Determina si el tipo especificado es un proxy basado en la interfaz de AOP y la configuración de habilitación.
    /// </summary>
    /// <param name="type">El tipo que se va a evaluar.</param>
    /// <param name="isEnableIAopProxy">Indica si la habilitación de IAopProxy está activada.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo es un proxy; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo es nulo y si la habilitación de IAopProxy está desactivada.
    /// Si está desactivada, se comprueba si el tipo no contiene la cadena "Xunit.DependencyInjection.ITestOutputHelperAccessor".
    /// Si la habilitación está activada, se evalúan las interfaces implementadas por el tipo para determinar si incluye la interfaz <see cref="IAopProxy"/>.
    /// </remarks>
    /// <seealso cref="IAopProxy"/>
    private static bool IsProxy( Type type, bool isEnableIAopProxy ) {
        if ( type == null )
            return false;
        if ( isEnableIAopProxy == false ) {
            return type.SafeString().Contains( "Xunit.DependencyInjection.ITestOutputHelperAccessor" ) == false;
        }
        var interfaces = type.GetInterfaces();
        if ( interfaces == null || interfaces.Length == 0 )
            return false;
        foreach ( var item in interfaces ) {
            if ( item == typeof( IAopProxy ) )
                return true;
        }
        return false;
    }

    /// <summary>
    /// Registra los servicios relacionados con aspectos en el contenedor de inyección de dependencias.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán los aspectos.</param>
    private static void RegisterAspectScoped( IServiceCollection services ) {
        services.AddScoped<IAspectScheduler, ScopeAspectScheduler>();
        services.AddScoped<IAspectBuilderFactory, ScopeAspectBuilderFactory>();
        services.AddScoped<IAspectContextFactory, ScopeAspectContextFactory>();
    }
}