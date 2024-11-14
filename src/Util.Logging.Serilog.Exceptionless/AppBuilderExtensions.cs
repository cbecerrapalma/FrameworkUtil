using Util.Configs;
using SerilogLog = Serilog.Log;

namespace Util.Logging.Serilog;

/// <summary>
/// Proporciona métodos de extensión para configurar aplicaciones.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega el servicio Exceptionless al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">La instancia de <see cref="IAppBuilder"/> a la que se le agregará Exceptionless.</param>
    /// <param name="isClearProviders">Indica si se deben limpiar los proveedores existentes antes de agregar Exceptionless.</param>
    /// <returns>La instancia de <see cref="IAppBuilder"/> con Exceptionless agregado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite agregar Exceptionless sin especificar un parámetro adicional.
    /// </remarks>
    /// <seealso cref="AddExceptionless(IAppBuilder, bool)"/>
    public static IAppBuilder AddExceptionless( this IAppBuilder builder, bool isClearProviders = false ) {
        return builder.AddExceptionless( null, isClearProviders );
    }

    /// <summary>
    /// Agrega el servicio Exceptionless al contenedor de aplicaciones.
    /// </summary>
    /// <param name="builder">El contenedor de aplicaciones donde se agrega el servicio.</param>
    /// <param name="appName">El nombre de la aplicación que se registrará en Exceptionless.</param>
    /// <returns>El contenedor de aplicaciones actualizado con el servicio Exceptionless agregado.</returns>
    public static IAppBuilder AddExceptionless( this IAppBuilder builder, string appName ) {
        return builder.AddExceptionless( null, appName );
    }

    /// <summary>
    /// Agrega la funcionalidad de Exceptionless al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="configAction">Una acción que permite configurar la instancia de <see cref="ExceptionlessConfiguration"/>.</param>
    /// <param name="isClearProviders">Indica si se deben limpiar los proveedores existentes antes de agregar Exceptionless.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado con la configuración de Exceptionless.</returns>
    public static IAppBuilder AddExceptionless( this IAppBuilder builder, Action<ExceptionlessConfiguration> configAction, bool isClearProviders = false ) {
        return builder.AddExceptionless( configAction, t => t.IsClearProviders = isClearProviders );
    }

    /// <summary>
    /// Agrega la configuración de Exceptionless al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se agregará Exceptionless.</param>
    /// <param name="configAction">Acción para configurar la instancia de <see cref="ExceptionlessConfiguration"/>.</param>
    /// <param name="appName">El nombre de la aplicación que se registrará en Exceptionless.</param>
    /// <returns>El <see cref="IAppBuilder"/> actualizado con la configuración de Exceptionless.</returns>
    /// <remarks>
    /// Este método permite personalizar la configuración de Exceptionless y establecer el nombre de la aplicación
    /// que se utilizará para el seguimiento de excepciones.
    /// </remarks>
    /// <seealso cref="ExceptionlessConfiguration"/>
    public static IAppBuilder AddExceptionless( this IAppBuilder builder, Action<ExceptionlessConfiguration> configAction, string appName ) {
        return builder.AddExceptionless( configAction, t => t.Application = appName );
    }

    /// <summary>
    /// Agrega el soporte para Exceptionless al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="IAppBuilder"/> que se está configurando.</param>
    /// <param name="configAction">Una acción que permite configurar la instancia de <see cref="ExceptionlessConfiguration"/>.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de registro a través de <see cref="LogOptions"/>.</param>
    /// <returns>El objeto <see cref="IAppBuilder"/> modificado para incluir el soporte de Exceptionless.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para el registro de excepciones y eventos utilizando Exceptionless.
    /// Se asegura de que los proveedores de registro sean configurados adecuadamente según las opciones especificadas.
    /// </remarks>
    /// <seealso cref="ExceptionlessConfiguration"/>
    /// <seealso cref="LogOptions"/>
    /// <seealso cref="ILogFactory"/>
    /// <seealso cref="ILog"/>
    public static IAppBuilder AddExceptionless( this IAppBuilder builder, Action<ExceptionlessConfiguration> configAction, Action<LogOptions> setupAction ) {
        builder.CheckNull( nameof( builder ) );
        var options = new LogOptions();
        setupAction?.Invoke( options );
        builder.Host.ConfigureServices( ( context, services ) => {
            services.AddSingleton<ILogFactory, LogFactory>();
            services.AddTransient( typeof( ILog<> ), typeof( Log<> ) );
            services.AddTransient( typeof( ILog ), t => t.GetService<ILogFactory>()?.CreateLog( "default" ) ?? NullLog.Instance );
            var configuration = context.Configuration;
            services.AddLogging( loggingBuilder => {
                if ( options.IsClearProviders )
                    loggingBuilder.ClearProviders();
                ConfigExceptionless( configAction, configuration );
                SerilogLog.Logger = new LoggerConfiguration()
                    .Enrich.WithProperty( "Application", options.Application )
                    .Enrich.FromLogContext()
                    .Enrich.WithLogLevel()
                    .Enrich.WithLogContext()
                    .WriteTo.Exceptionless()
                    .ReadFrom.Configuration( configuration )
                    .ConfigLogLevel( configuration )
                    .CreateLogger();
                loggingBuilder.AddSerilog();
            } );
        } );
        return builder;
    }

    /// <summary>
    /// Configura el cliente Exceptionless utilizando la acción de configuración proporcionada 
    /// o lee la configuración desde el objeto IConfiguration.
    /// </summary>
    /// <param name="configAction">Acción que configura el objeto ExceptionlessConfiguration.</param>
    /// <param name="configuration">Objeto que contiene la configuración a ser leída.</param>
    /// <remarks>
    /// Este método inicializa el cliente Exceptionless y aplica la configuración 
    /// especificada. Si no se proporciona una acción de configuración, se utiliza 
    /// la configuración predeterminada del objeto IConfiguration.
    /// </remarks>
    private static void ConfigExceptionless( Action<ExceptionlessConfiguration> configAction, IConfiguration configuration ) {
        ExceptionlessClient.Default.Startup();
        if ( configAction != null ) {
            configAction( ExceptionlessClient.Default.Configuration );
            ConfigLogLevel( configuration, ExceptionlessClient.Default.Configuration );
            return;
        }
        ExceptionlessClient.Default.Configuration.ReadFromConfiguration( configuration );
        ConfigLogLevel( configuration, ExceptionlessClient.Default.Configuration );
    }

    /// <summary>
    /// Configura el nivel de registro para la aplicación utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="configuration">La configuración de la aplicación que contiene los niveles de registro.</param>
    /// <param name="options">Las opciones de configuración de Exceptionless que se actualizarán con los niveles de registro.</param>
    /// <remarks>
    /// Este método itera a través de la sección de configuración de niveles de registro y agrega cada nivel a las opciones de Exceptionless.
    /// Se asigna un nivel de registro por defecto si se encuentra en la configuración.
    /// </remarks>
    private static void ConfigLogLevel( IConfiguration configuration, ExceptionlessConfiguration options ) {
        var section = configuration.GetSection( "Logging:LogLevel" );
        foreach ( var item in section.GetChildren() ) {
            if ( item.Key == "Default" ) {
                options.Settings.Add( "@@log:*", GetLogLevel( item.Value ) );
                continue;
            }
            options.Settings.Add( $"@@log:{item.Key}*", GetLogLevel( item.Value ) );
        }
    }

    /// <summary>
    /// Obtiene el nivel de registro correspondiente a una cadena de texto dada.
    /// </summary>
    /// <param name="logLevel">El nivel de registro en formato de texto que se desea convertir.</param>
    /// <returns>
    /// Devuelve el nivel de registro correspondiente en formato estándar. 
    /// Si el nivel de registro no es reconocido, se devuelve "Warn".
    /// </returns>
    /// <remarks>
    /// Este método convierte los niveles de registro comunes a sus representaciones estándar.
    /// Los niveles aceptados son: TRACE, DEBUG, INFORMATION, ERROR, CRITICAL, NONE.
    /// </remarks>
    private static string GetLogLevel( string logLevel ) {
        switch ( logLevel.ToUpperInvariant() ) {
            case "TRACE":
                return "Trace";
            case "DEBUG":
                return "Debug";
            case "INFORMATION":
                return "Info";
            case "ERROR":
                return "Error";
            case "CRITICAL":
                return "Fatal";
            case "NONE":
                return "Off";
            default:
                return "Warn";
        }
    }
}