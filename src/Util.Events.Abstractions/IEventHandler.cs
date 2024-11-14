namespace Util.Events; 

/// <summary>
/// Define un manejador de eventos que puede ser implementado por clases que deseen manejar eventos específicos.
/// </summary>
public interface IEventHandler {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve <c>true</c> si la funcionalidad está habilitada; de lo contrario, devuelve <c>false</c>.
    /// </remarks>
    /// <value>
    /// <c>true</c> si está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    bool Enabled { get; }
}

/// <summary>
/// Interfaz genérica que define un manejador de eventos.
/// </summary>
/// <typeparam name="TEvent">El tipo de evento que este manejador puede procesar. Debe implementar la interfaz <see cref="IEvent"/>.</typeparam>
/// <remarks>
/// Esta interfaz permite la implementación de manejadores de eventos que pueden recibir un tipo específico de evento.
/// </remarks>
public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent {
    /// <summary>
    /// Maneja un evento de tipo <typeparamref name="TEvent"/> de manera asíncrona.
    /// </summary>
    /// <param name="event">El evento que se va a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <typeparam name="TEvent">El tipo del evento que se está manejando.</typeparam>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método debe ser implementado para realizar la lógica necesaria al recibir un evento.
    /// Asegúrese de manejar adecuadamente las excepciones y el estado de cancelación.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task HandleAsync( TEvent @event, CancellationToken cancellationToken ) ;
}