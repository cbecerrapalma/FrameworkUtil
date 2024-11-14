namespace Util.Events; 

/// <summary>
/// Proporciona métodos de extensión para la funcionalidad del bus de eventos.
/// </summary>
public static class EventBusExtensions {
    /// <summary>
    /// Publica de manera asíncrona una colección de eventos en el bus de eventos especificado.
    /// </summary>
    /// <param name="eventBus">El bus de eventos donde se 
    public static async Task PublishAsync( this IEventBus eventBus, IEnumerable<IEvent> events, CancellationToken cancellationToken = default ) {
        eventBus.CheckNull( nameof( eventBus ) );
        if( events == null )
            return;
        foreach( var @event in events ) {
            await eventBus.PublishAsync( @event, cancellationToken );
        }
    }
}