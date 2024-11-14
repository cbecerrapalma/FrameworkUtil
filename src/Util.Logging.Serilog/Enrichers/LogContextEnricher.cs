using Util.Helpers;

namespace Util.Logging.Serilog.Enrichers; 

/// <summary>
/// Clase que enriquece los eventos de registro con información adicional.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ILogEventEnricher"/> y permite agregar datos contextuales a los eventos de registro,
/// lo que puede ser útil para el análisis y la depuración.
/// </remarks>
public class LogContextEnricher : ILogEventEnricher {
    /// <summary>
    /// Enriquecen un evento de registro con información contextual adicional.
    /// </summary>
    /// <param name="logEvent">El evento de registro que se va a enriquecer.</param>
    /// <param name="propertyFactory">La fábrica de propiedades para crear propiedades adicionales en el evento de registro.</param>
    /// <remarks>
    /// Este método obtiene el contexto actual a través de un accessor de contexto.
    /// Si el contexto o el evento de registro son nulos, el método no realiza ninguna acción.
    /// Se eliminan propiedades existentes del evento de registro y se añaden nuevas propiedades
    /// relacionadas con la duración, el identificador de traza, el identificador de usuario,
    /// la aplicación, el entorno y otros datos relevantes.
    /// </remarks>
    public void Enrich( LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        var accessor = Ioc.Create<ILogContextAccessor>();
        if ( accessor == null )
            return;
        var context = accessor.Context;
        if ( context == null )
            return;
        if ( logEvent == null )
            return;
        if ( propertyFactory == null )
            return;
        RemoveProperties( logEvent );
        AddDuration( context,logEvent, propertyFactory );
        AddTraceId( context, logEvent, propertyFactory );
        AddUserId( context, logEvent, propertyFactory );
        AddApplication( context, logEvent, propertyFactory );
        AddEnvironment( context, logEvent, propertyFactory );
        AddData( context, logEvent, propertyFactory );
    }

    /// <summary>
    /// Elimina propiedades específicas de un evento de registro.
    /// </summary>
    /// <param name="logEvent">El evento de registro del cual se eliminarán las propiedades.</param>
    /// <remarks>
    /// Este método busca y elimina las propiedades "ActionId", "ActionName", "RequestId", "RequestPath" y "ConnectionId"
    /// del objeto <paramref name="logEvent"/> si están presentes.
    /// </remarks>
    private void RemoveProperties( LogEvent logEvent ) {
        logEvent.RemovePropertyIfPresent( "ActionId" );
        logEvent.RemovePropertyIfPresent( "ActionName" );
        logEvent.RemovePropertyIfPresent( "RequestId" );
        logEvent.RemovePropertyIfPresent( "RequestPath" );
        logEvent.RemovePropertyIfPresent( "ConnectionId" );
    }

    /// <summary>
    /// Agrega la duración del evento de registro al contexto de registro.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene la información sobre el evento.</param>
    /// <param name="logEvent">El evento de registro al que se le añadirá la duración.</param>
    /// <param name="propertyFactory">La fábrica de propiedades para crear la propiedad de duración.</param>
    /// <remarks>
    /// Este método verifica si el contexto tiene un cronómetro activo. Si no es así, no realiza ninguna acción.
    /// Si el cronómetro está activo, se crea una propiedad que representa la duración transcurrida y se añade al evento de registro.
    /// </remarks>
    private void AddDuration( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context?.Stopwatch == null )
            return;
        var property = propertyFactory.CreateProperty( "Duration", context.Stopwatch.Elapsed.Description() );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Agrega el identificador de traza al evento de registro si está disponible en el contexto.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene el identificador de traza.</param>
    /// <param name="logEvent">El evento de registro al que se le agregará el identificador de traza.</param>
    /// <param name="propertyFactory">La fábrica de propiedades para crear la propiedad del identificador de traza.</param>
    /// <remarks>
    /// Este método verifica si el contexto es nulo o si el identificador de traza está vacío.
    /// Si no es así, crea una propiedad con el identificador de traza y la agrega o actualiza en el evento de registro.
    /// </remarks>
    private void AddTraceId( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context == null || context.TraceId.IsEmpty() )
            return;
        var property = propertyFactory.CreateProperty( "TraceId", context.TraceId );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Agrega el identificador de usuario al evento de registro si está disponible.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene información sobre el usuario.</param>
    /// <param name="logEvent">El evento de registro al cual se le añadirá el identificador de usuario.</param>
    /// <param name="propertyFactory">La fábrica de propiedades para crear la propiedad del identificador de usuario.</param>
    /// <remarks>
    /// Este método verifica si el contexto es nulo o si el identificador de usuario está vacío. 
    /// Si alguna de estas condiciones se cumple, el método no realiza ninguna acción.
    /// De lo contrario, crea una propiedad con el nombre "UserId" y el valor del identificador de usuario, 
    /// y la añade o actualiza en el evento de registro.
    /// </remarks>
    private void AddUserId( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context == null || context.UserId.IsEmpty() )
            return;
        var property = propertyFactory.CreateProperty( "UserId", context.UserId );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Agrega información de la aplicación al evento de registro.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene información sobre la aplicación.</param>
    /// <param name="logEvent">El evento de registro al que se le añadirá la propiedad de la aplicación.</param>
    /// <param name="propertyFactory">La fábrica de propiedades de eventos de registro utilizada para crear la propiedad de la aplicación.</param>
    /// <remarks>
    /// Este método verifica si el contexto es nulo o si la aplicación está vacía antes de intentar agregar la propiedad.
    /// Si el contexto es válido, se crea una propiedad con el nombre "Application" y se añade al evento de registro.
    /// </remarks>
    private void AddApplication( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context == null || context.Application.IsEmpty() )
            return;
        var property = propertyFactory.CreateProperty( "Application", context.Application );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Agrega información sobre el entorno al evento de registro si el contexto y el entorno son válidos.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene información sobre el entorno.</param>
    /// <param name="logEvent">El evento de registro al que se le añadirá la propiedad del entorno.</param>
    /// <param name="propertyFactory">La fábrica de propiedades de eventos de registro utilizada para crear la propiedad del entorno.</param>
    private void AddEnvironment( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context == null || context.Environment.IsEmpty() )
            return;
        var property = propertyFactory.CreateProperty( "Environment", context.Environment );
        logEvent.AddOrUpdateProperty( property );
    }

    /// <summary>
    /// Agrega datos del contexto de registro al evento de registro especificado.
    /// </summary>
    /// <param name="context">El contexto de registro que contiene los datos a agregar.</param>
    /// <param name="logEvent">El evento de registro al que se agregarán los datos.</param>
    /// <param name="propertyFactory">La fábrica de propiedades para crear propiedades de registro.</param>
    /// <remarks>
    /// Este método verifica si el contexto de registro contiene datos. Si no hay datos, no realiza ninguna acción.
    /// Si hay datos, itera a través de cada elemento en el contexto y crea una propiedad de registro utilizando la fábrica de propiedades.
    /// Luego, agrega o actualiza la propiedad en el evento de registro.
    /// </remarks>
    private void AddData( LogContext context, LogEvent logEvent, ILogEventPropertyFactory propertyFactory ) {
        if ( context?.Data == null || context.Data.Count == 0 )
            return;
        foreach ( var item in context.Data ) {
            var property = propertyFactory.CreateProperty( item.Key, item.Value );
            logEvent.AddOrUpdateProperty( property );
        }
    }
}