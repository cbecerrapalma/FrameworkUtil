namespace Util.Logging.Serilog.Enrichers; 

/// <summary>
/// Clase que enriquece los eventos de registro con información sobre el nivel de registro.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ILogEventEnricher"/> y se utiliza para agregar
/// información adicional a los eventos de registro, específicamente el nivel de registro.
/// </remarks>
public class LogLevelEnricher : ILogEventEnricher {
    /// <summary>
    /// Enriquecen un evento de registro añadiendo o actualizando una propiedad que indica el nivel de registro.
    /// </summary>
    /// <param name="logEvent">El evento de registro que se va a enriquecer.</param>
    /// <param name="propertyFactory">La fábrica de propiedades utilizada para crear la propiedad del nivel de registro.</param>
    public void Enrich( LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        var property = propertyFactory.CreateProperty( "LogLevel", GetLogLevel( logEvent.Level ) );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Obtiene el nivel de registro correspondiente a un nivel de evento de registro.
    /// </summary>
    /// <param name="logLevel">El nivel de evento de registro que se desea convertir a una cadena.</param>
    /// <returns>
    /// Una cadena que representa el nivel de registro. 
    /// Retorna <c>null</c> si el nivel de evento de registro no es reconocido.
    /// </returns>
    private string GetLogLevel( LogEventLevel logLevel ) {
        switch ( logLevel ) {
            case LogEventLevel.Verbose:
                return "Trace";
            case LogEventLevel.Debug:
                return "Debug";
            case LogEventLevel.Information:
                return "Information";
            case LogEventLevel.Warning:
                return "Warning";
            case LogEventLevel.Error:
                return "Error";
            case LogEventLevel.Fatal:
                return "Critical";
            default:
                return null;
        }
    }
}