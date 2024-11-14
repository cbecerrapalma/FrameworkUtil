namespace Util.Events; 

public record IntegrationEvent : IIntegrationEvent {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del evento.
    /// </summary>
    /// <remarks>
    /// Este identificador es de solo lectura y se establece durante la inicialización del objeto.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador único del evento.
    /// </value>
    public string EventId { get; init; }

    /// <inheritdoc />
    /// <summary>
    /// Representa la hora en que ocurrió un evento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se inicializa al momento de la creación del objeto.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que indica el momento específico del evento.
    /// </value>
    public DateTime EventTime { get; init; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IntegrationEvent"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor genera un nuevo identificador único para el evento y establece la hora actual
    /// utilizando el método proporcionado por la clase <see cref="Util.Helpers.Time"/>.
    /// </remarks>
    public IntegrationEvent() {
        EventId = Guid.NewGuid().ToString();
        EventTime = Util.Helpers.Time.Now;
    }
}