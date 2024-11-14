namespace Util.Events;

/// <summary>
/// Clase que implementa un bus de eventos local utilizando MediatR.
/// </summary>
/// <remarks>
/// Esta clase es responsable de la gestión y distribución de eventos dentro de la aplicación,
/// permitiendo la comunicación entre diferentes componentes de manera desacoplada.
/// </remarks>
public class MediatREventBus : ILocalEventBus {
    private readonly IMediator _publisher;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MediatREventBus"/>.
    /// </summary>
    /// <param name="publisher">Una instancia de <see cref="IMediator"/> que se utilizará para 
    public MediatREventBus( IMediator publisher ) {
        _publisher = publisher ?? throw new ArgumentNullException( nameof( publisher ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa la interfaz <see cref="IEvent"/>.</typeparam>
    /// <param name="@event">El evento que se va a 
    public async Task PublishAsync<TEvent>( TEvent @event,CancellationToken cancellationToken = default ) where TEvent : IEvent {
        cancellationToken.ThrowIfCancellationRequested();
        if ( @event == null )
            return;
        await _publisher.Publish( @event, cancellationToken );
    }
}