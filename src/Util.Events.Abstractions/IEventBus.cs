namespace Util.Events; 

/// <summary>
/// Define una interfaz para el bus de eventos, que permite la 
public interface IEventBus : ITransientDependency {
    /// <summary>
    /// Publica un evento de tipo <typeparamref name="TEvent"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa la interfaz <see cref="IEvent"/>.</typeparam>
    /// <param name="@event">El evento que se va a 
    Task PublishAsync<TEvent>( TEvent @event,CancellationToken cancellationToken = default ) where TEvent : IEvent;
}