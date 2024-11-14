namespace Util.Events;

/// <summary>
/// Representa un evento de integración en el sistema.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de la interfaz <see cref="IEvent"/> y se utiliza para definir eventos
/// que son relevantes para la integración entre diferentes sistemas o componentes.
/// </remarks>
public interface IIntegrationEvent : IEvent {
    /// <summary>
    /// Obtiene el identificador del evento.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador del evento.
    /// </value>
    string EventId { get; }
    /// <summary>
    /// Obtiene la fecha y hora del evento.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa el momento en que ocurrió el evento.
    /// </value>
    DateTime EventTime { get; }
}