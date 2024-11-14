namespace Util.Events; 

/// <summary>
/// Representa un bus de eventos local que permite la 
public class LocalEventBus : ILocalEventBus {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalEventBus"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    public LocalEventBus( IServiceProvider serviceProvider ) {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException( nameof( serviceProvider ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa la interfaz <see cref="IEvent"/>.</typeparam>
    /// <param name="event">El evento que se va a 
    public async Task PublishAsync<TEvent>( TEvent @event,CancellationToken cancellationToken = default ) where TEvent : IEvent {
        cancellationToken.ThrowIfCancellationRequested();
        if ( @event == null )
            return;
        await PublishLocalEventAsync( @event, cancellationToken );
    }

    /// <summary>
    /// Publica un evento local de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa la interfaz <see cref="IEvent"/>.</typeparam>
    /// <param name="@event">La instancia del evento que se va a 
    private async Task PublishLocalEventAsync<TEvent>( TEvent @event, CancellationToken cancellationToken ) where TEvent : IEvent {
        var eventType = @event.GetType();
        var handlers = GetEventHandlers( eventType );
        if ( handlers == null )
            return;
        foreach ( var handler in handlers.Where( t => t is { Enabled: true } ).OrderBy( t => t.Order ) ) {
            var method = typeof( IEventHandler<> ).MakeGenericType( eventType ).GetMethod( "HandleAsync", new[] { eventType,typeof( CancellationToken ) } );
            if ( method == null )
                return;
            var result = method.Invoke( handler, new object[] { @event, cancellationToken } );
            if ( result == null )
                return;
            await (Task)result;
        }
    }

    /// <summary>
    /// Obtiene los controladores de eventos locales para un tipo de evento específico.
    /// </summary>
    /// <param name="eventType">El tipo de evento para el cual se buscan los controladores.</param>
    /// <returns>
    /// Una colección de controladores de eventos locales que implementan <see cref="ILocalEventHandler"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el proveedor de servicios para resolver los controladores de eventos 
    /// que están registrados para el tipo de evento especificado. Si no se encuentran controladores, 
    /// el método no devuelve ningún resultado.
    /// </remarks>
    /// <seealso cref="IEventHandler{T}"/>
    /// <seealso cref="ILocalEventHandler"/>
    private IEnumerable<ILocalEventHandler> GetEventHandlers( Type eventType ) {
        var handlerType = typeof( IEventHandler<> ).MakeGenericType( eventType );
        var serviceType = typeof( IEnumerable<> ).MakeGenericType( handlerType );
        var handlers = _serviceProvider.GetService( serviceType );
        if ( handlers is not IEnumerable<IEventHandler> eventHandlers )
            yield break;
        foreach ( var handler in eventHandlers )
            yield return handler as ILocalEventHandler;
    }
}