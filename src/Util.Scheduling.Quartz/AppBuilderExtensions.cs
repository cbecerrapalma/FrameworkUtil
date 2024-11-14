using Util.Configs;

namespace Util.Scheduling;

/// <summary>
/// Contiene métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega el servicio Quartz al contenedor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones donde se agregará el servicio Quartz.</param>
    /// <param name="isScanJobs">Indica si se deben escanear los trabajos automáticamente.</param>
    /// <returns>El mismo constructor de aplicaciones con el servicio Quartz agregado.</returns>
    public static IAppBuilder AddQuartz( this IAppBuilder builder, bool isScanJobs = true ) {
        return builder.AddQuartz( null, isScanJobs );
    }

    /// <summary>
    /// Agrega el servicio Quartz al contenedor de servicios.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar el <see cref="IServiceCollectionQuartzConfigurator"/>.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con el servicio Quartz agregado.</returns>
    public static IAppBuilder AddQuartz( this IAppBuilder builder, Action<IServiceCollectionQuartzConfigurator> setupAction ) {
        return builder.AddQuartz( setupAction, true );
    }

    /// <summary>
    /// Agrega la configuración de Quartz al contenedor de servicios.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar los servicios de Quartz.</param>
    /// <param name="isScanJobs">Un valor booleano que indica si se deben escanear los trabajos.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con la configuración de Quartz aplicada.</returns>
    /// <remarks>
    /// Este método se utiliza para integrar Quartz en una aplicación, configurando los servicios necesarios
    /// y añadiendo el servicio hospedado para la ejecución de trabajos programados.
    /// </remarks>
    /// <seealso cref="IServiceCollectionQuartzConfigurator"/>
    public static IAppBuilder AddQuartz( this IAppBuilder builder, Action<IServiceCollectionQuartzConfigurator> setupAction, bool isScanJobs ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            AddQuartz( services, setupAction );
            AddSchedulerBuilder( services );
            AddSchedulerOptions( services, isScanJobs );
            AddHostedService( services );
        } );
        return builder;
    }

    /// <summary>
    /// Agrega la configuración de Quartz al contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán los servicios de Quartz.</param>
    /// <param name="setupAction">Una acción opcional que permite configurar Quartz adicionalmente.</param>
    /// <remarks>
    /// Si <paramref name="setupAction"/> es nulo, se registrará Quartz con la configuración predeterminada.
    /// De lo contrario, se aplicará la configuración proporcionada en <paramref name="setupAction"/>.
    /// </remarks>
    private static void AddQuartz( IServiceCollection services, Action<IServiceCollectionQuartzConfigurator> setupAction ) {
        if ( setupAction == null ) {
            services.AddQuartz( options => {
                options.UseMicrosoftDependencyInjectionJobFactory();
            } );
            return;
        }
        services.AddQuartz( options => {
            options.UseMicrosoftDependencyInjectionJobFactory();
            setupAction( options );
        } );
    }

    /// <summary>
    /// Agrega el constructor de un programador a la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el programador.</param>
    private static void AddSchedulerBuilder( IServiceCollection services ) {
        services.TryAddSingleton<ISchedulerManager, QuartzSchedulerManager>();
    }

    /// <summary>
    /// Configura las opciones del programador (Scheduler) en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las opciones del programador.</param>
    /// <param name="isScanJobs">Indica si el programador debe escanear trabajos.</param>
    private static void AddSchedulerOptions( IServiceCollection services, bool isScanJobs ) {
        void Action( SchedulerOptions t ) => t.IsScanJobs = isScanJobs;
        services.Configure( (Action<SchedulerOptions>)Action );
    }

    /// <summary>
    /// Agrega un servicio alojado a la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el servicio alojado.</param>
    /// <remarks>
    /// Este método utiliza la interfaz <see cref="IServiceCollection"/> para registrar
    /// el servicio <see cref="HostService"/> como un servicio alojado.
    /// </remarks>
    private static void AddHostedService( IServiceCollection services ) {
        services.AddHostedService<HostService>();
    }
}