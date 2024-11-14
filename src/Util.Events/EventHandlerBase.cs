namespace Util.Events; 

/// <summary>
/// Clase base abstracta para manejar eventos de tipo específico.
/// </summary>
/// <typeparam name="TEvent">El tipo de evento que esta clase manejará. Debe implementar la interfaz <see cref="IEvent"/>.</typeparam>
public abstract class EventHandlerBase<TEvent> : IEventHandler<TEvent>, ILocalEventHandler where TEvent : IEvent {
    /// <summary>
    /// Maneja de manera asíncrona el evento especificado.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que se va a manejar.</typeparam>
    /// <param name="@event">El evento que se va a procesar.</param>
    /// <param name="cancellationToken">Token de cancelación para permitir la cancelación de la operación.</param>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    public abstract Task HandleAsync( TEvent @event, CancellationToken cancellationToken );

    /// <summary>
    /// Obtiene el valor del orden.
    /// </summary>
    /// <remarks>
    /// Este valor es un entero que representa el orden de un elemento.
    /// Por defecto, el valor es 0.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el orden.
    /// </returns>
    public virtual int Order => 0;

    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>true</c>, lo que significa que la funcionalidad está habilitada.
    /// </value>
    /// <remarks>
    /// Esta propiedad es virtual y puede ser sobreescrita en una clase derivada para proporcionar una lógica diferente.
    /// </remarks>
    public virtual bool Enabled => true;
}