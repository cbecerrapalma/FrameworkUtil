namespace Util.Logging.Serilog; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador.
/// </summary>
public static class LoggerConfigurationExtensions {
    /// <summary>
    /// Configura los niveles de registro para la configuración de logging especificada.
    /// </summary>
    /// <param name="source">La configuración del logger a modificar.</param>
    /// <param name="configuration">La configuración que contiene los niveles de log.</param>
    /// <returns>La configuración del logger modificada con los niveles de log establecidos.</returns>
    /// <remarks>
    /// Este método lee la sección "Logging:LogLevel" de la configuración proporcionada
    /// y ajusta los niveles de log en la configuración del logger en consecuencia.
    /// Se establece un nivel de log predeterminado y se pueden sobreescribir niveles específicos
    /// para diferentes categorías de log.
    /// </remarks>
    /// <seealso cref="LoggerConfiguration"/>
    /// <seealso cref="IConfiguration"/>
    public static LoggerConfiguration ConfigLogLevel( this LoggerConfiguration source, IConfiguration configuration ) {
        source.CheckNull( nameof( source ) );
        configuration.CheckNull( nameof( configuration ) );
        var section = configuration.GetSection( "Logging:LogLevel" );
        foreach ( var item in section.GetChildren() ) {
            if ( item.Key == "Default" ) {
                source.MinimumLevel.ControlledBy( new LoggingLevelSwitch( GetLogLevel( item.Value ) ) );
                continue;
            }
            source.MinimumLevel.Override( item.Key, GetLogLevel( item.Value ) );
        }
        return source;
    }

    /// <summary>
    /// Obtiene el nivel de registro correspondiente a una cadena de texto que representa el nivel de log.
    /// </summary>
    /// <param name="logLevel">Una cadena que representa el nivel de log deseado. Puede ser "TRACE", "DEBUG", "INFORMATION", "ERROR", "CRITICAL" o "NONE".</param>
    /// <returns>Devuelve un valor de <see cref="LogEventLevel"/> que corresponde al nivel de log especificado.</returns>
    /// <remarks>
    /// Si el nivel de log proporcionado no coincide con ninguno de los valores esperados, se devolverá el nivel de advertencia por defecto.
    /// </remarks>
    private static LogEventLevel GetLogLevel( string logLevel ) {
        switch ( logLevel.ToUpperInvariant() ) {
            case "TRACE":
                return LogEventLevel.Verbose;
            case "DEBUG":
                return LogEventLevel.Debug;
            case "INFORMATION":
                return LogEventLevel.Information;
            case "ERROR":
                return LogEventLevel.Error;
            case "CRITICAL":
                return LogEventLevel.Fatal;
            case "NONE":
                return LogEventLevel.Fatal;
            default:
                return LogEventLevel.Warning;
        }
    }
}