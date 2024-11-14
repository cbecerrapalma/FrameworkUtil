using Util.Configs;
using SerilogLog = Serilog.Log;

namespace Util.Logging.Serilog;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="AppBuilder"/>.
/// </summary>
public static class AppBuilderExtensions {
    /// <summary>
    /// Agrega el registro de Serilog al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación donde se añadirá Serilog.</param>
    /// <returns>El mismo constructor de la aplicación con Serilog agregado.</returns>
    public static IAppBuilder AddSerilog( this IAppBuilder builder ) {
        return builder.AddSerilog( false );
    }

    /// <summary>
    /// Agrega el registro de Serilog al constructor de la aplicación.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación al que se le añadirá Serilog.</param>
    /// <param name="isClearProviders">Indica si se deben limpiar los proveedores de registro existentes.</param>
    /// <returns>El mismo constructor de la aplicación con Serilog agregado.</returns>
    public static IAppBuilder AddSerilog( this IAppBuilder builder, bool isClearProviders ) {
        return builder.AddSerilog( options => {
            options.IsClearProviders = isClearProviders;
        } );
    }

    /// <summary>
    /// Agrega la configuración de Serilog al constructor de aplicaciones.
    /// </summary>
    /// <param name="builder">El constructor de aplicaciones donde se añadirá Serilog.</param>
    /// <param name="appName">El nombre de la aplicación que se utilizará en la configuración de Serilog.</param>
    /// <returns>El mismo constructor de aplicaciones con Serilog configurado.</returns>
    /// <remarks>
    /// Este método permite personalizar la configuración de Serilog al proporcionar el nombre de la aplicación.
    /// </remarks>
    /// <seealso cref="IAppBuilder"/>
    public static IAppBuilder AddSerilog( this IAppBuilder builder, string appName ) {
        return builder.AddSerilog( options => {
            options.Application = appName;
        } );
    }

    /// <summary>
    /// Agrega la configuración de Serilog al pipeline de la aplicación.
    /// </summary>
    /// <param name="builder">El <see cref="IAppBuilder"/> al que se le añadirá la configuración de Serilog.</param>
    /// <param name="setupAction">Una acción que permite configurar las opciones de registro mediante <see cref="LogOptions"/>.</param>
    /// <returns>El <see cref="IAppBuilder"/> modificado con la configuración de Serilog.</returns>
    /// <remarks>
    /// Este método permite personalizar la configuración de logging utilizando las opciones definidas en <see cref="LogOptions"/>.
    /// Se pueden establecer propiedades adicionales y configurar el comportamiento de los proveedores de logging.
    /// </remarks>
    /// <seealso cref="LogOptions"/>
    /// <seealso cref="ILogFactory"/>
    /// <seealso cref="ILog{T}"/>
    /// <seealso cref="ILogger"/>
    public static IAppBuilder AddSerilog( this IAppBuilder builder, Action<LogOptions> setupAction ) {
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
                SerilogLog.Logger = new LoggerConfiguration()
                    .Enrich.WithProperty( "Application", options.Application )
                    .Enrich.FromLogContext()
                    .Enrich.WithLogLevel()
                    .Enrich.WithLogContext()
                    .ReadFrom.Configuration( configuration )
                    .ConfigLogLevel( configuration )
                    .CreateLogger();
                loggingBuilder.AddSerilog();
            } );
        } );
        return builder;
    }
}