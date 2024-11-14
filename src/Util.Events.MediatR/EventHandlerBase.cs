namespace Util.Events;

/// <summary>
/// Clase base abstracta para manejar eventos de tipo <typeparamref name="TEvent"/>.
/// Implementa la interfaz <see cref="INotificationHandler{T}"/> para gestionar notificaciones.
/// </summary>
/// <typeparam name="TEvent">El tipo de evento que esta clase manejará. Debe implementar las interfaces <see cref="IEvent"/> e <see cref="INotification"/>.</typeparam>
public abstract class EventHandlerBase<TEvent> : INotificationHandler<TEvent> where TEvent : IEvent, INotification {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>true</c>, lo que significa que la funcionalidad está habilitada.
    /// </value>
    /// <remarks>
    /// Esta propiedad es virtual, lo que permite que las clases derivadas puedan sobreescribirla si es necesario.
    /// </remarks>
    public virtual bool Enabled => true;

    /// <summary>
    /// Maneja el evento especificado de manera asíncrona.
    /// </summary>
    /// <param name="event">El evento que se va a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método verifica si el manejo de eventos está habilitado antes de proceder a manejar el evento.
    /// Si el manejo de eventos está deshabilitado, el método termina sin realizar ninguna acción.
    /// </remarks>
    public async Task Handle( TEvent @event, CancellationToken cancellationToken ) {
        if ( Enabled == false )
            return;
        await HandleAsync( @event, cancellationToken );
    }

    /// <summary>
    /// Maneja de forma asíncrona un evento específico.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que se va a manejar.</typeparam>
    /// <param name="event">El evento que se debe procesar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para proporcionar la lógica de manejo del evento.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public abstract Task HandleAsync( TEvent @event, CancellationToken cancellationToken );
}