using Util.Configs;

namespace Util.Scheduling;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega Hangfire al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El builder de la aplicación donde se agregará Hangfire.</param>
    /// <param name="setupAction">Acción para configurar la configuración global de Hangfire.</param>
    /// <returns>El builder de la aplicación con Hangfire agregado.</returns>
    public static IAppBuilder AddHangfire( this IAppBuilder builder, Action<IGlobalConfiguration> setupAction ) {
        return builder.AddHangfire( setupAction, true );
    }

    /// <summary>
    /// Agrega Hangfire al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Una acción que permite configurar la globalización de Hangfire.</param>
    /// <param name="serverSetupAction">Una acción que permite configurar las opciones del servidor de trabajos en segundo plano.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> con Hangfire agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar Hangfire con opciones de configuración predeterminadas.
    /// </remarks>
    public static IAppBuilder AddHangfire( this IAppBuilder builder, Action<IGlobalConfiguration> setupAction, Action<BackgroundJobServerOptions> serverSetupAction ) {
        return builder.AddHangfire( setupAction, serverSetupAction, true );
    }

    /// <summary>
    /// Agrega Hangfire al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación.</param>
    /// <param name="setupAction">Acción para configurar la configuración global de Hangfire.</param>
    /// <param name="isScanJobs">Indica si se deben escanear los trabajos.</param>
    /// <returns>El constructor de la aplicación con Hangfire agregado.</returns>
    public static IAppBuilder AddHangfire( this IAppBuilder builder, Action<IGlobalConfiguration> setupAction, bool isScanJobs ) {
        return builder.AddHangfire( setupAction, null, isScanJobs );
    }

    /// <summary>
    /// Agrega la configuración de Hangfire al contenedor de servicios de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="setupAction">Acción para configurar la configuración global de Hangfire.</param>
    /// <param name="serverSetupAction">Acción para configurar las opciones del servidor de trabajos en segundo plano.</param>
    /// <param name="isScanJobs">Indica si se deben escanear los trabajos existentes.</param>
    /// <returns>
    /// El objeto <see cref="IAppBuilder"/> modificado con la configuración de Hangfire.
    /// </returns>
    /// <remarks>
    /// Este método permite integrar Hangfire en la aplicación, configurando los servicios necesarios
    /// para el funcionamiento de los trabajos en segundo plano y la programación de tareas.
    /// </remarks>
    /// <seealso cref="IGlobalConfiguration"/>
    /// <seealso cref="BackgroundJobServerOptions"/>
    public static IAppBuilder AddHangfire( this IAppBuilder builder, Action<IGlobalConfiguration> setupAction, Action<BackgroundJobServerOptions> serverSetupAction, bool isScanJobs ) {
        builder.CheckNull( nameof( builder ) );
        builder.Host.ConfigureServices( ( context, services ) => {
            AddHangfire( services, setupAction, serverSetupAction );
            AddSchedulerBuilder( services );
            AddSchedulerOptions( services, isScanJobs );
            AddHostedService( services );
        } );
        return builder;
    }

    /// <summary>
    /// Agrega la configuración de Hangfire a los servicios de la aplicación.
    /// </summary>
    /// <param name="services">La colección de servicios donde se agregará la configuración de Hangfire.</param>
    /// <param name="setupAction">Acción que permite configurar la global de Hangfire.</param>
    /// <param name="serverSetupAction">Acción opcional que permite configurar las opciones del servidor de trabajos en segundo plano.</param>
    /// <remarks>
    /// Este método es útil para inicializar Hangfire en una aplicación ASP.NET Core,
    /// permitiendo la configuración de la base de datos y otras opciones globales.
    /// </remarks>
    /// <seealso cref="IGlobalConfiguration"/>
    /// <seealso cref="BackgroundJobServerOptions"/>
    private static void AddHangfire( IServiceCollection services, Action<IGlobalConfiguration> setupAction, Action<BackgroundJobServerOptions> serverSetupAction ) {
        services.AddHangfire( setupAction );
        if ( serverSetupAction != null )
            services.Configure( serverSetupAction );
    }

    /// <summary>
    /// Agrega el constructor del programador a la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el programador.</param>
    private static void AddSchedulerBuilder( IServiceCollection services ) {
        services.TryAddSingleton<ISchedulerManager, HangfireSchedulerManager>();
    }

    /// <summary>
    /// Configura las opciones del programador en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las opciones del programador.</param>
    /// <param name="isScanJobs">Indica si el programador debe escanear trabajos.</param>
    private static void AddSchedulerOptions( IServiceCollection services, bool isScanJobs ) {
        void Action( SchedulerOptions t ) => t.IsScanJobs = isScanJobs;
        services.Configure( (Action<SchedulerOptions>)Action );
    }

    /// <summary>
    /// Agrega un servicio hospedado a la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se agregará el servicio hospedado.</param>
    private static void AddHostedService( IServiceCollection services ) {
        services.AddHostedService<HostService>();
    }
}