namespace Util.Microservices.Events;

/// <summary>
/// Representa un registro de reintentos de suscripción.
/// </summary>
public class SubscriptionRetryLog {
    /// <summary>
    /// Representa un número entero.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar y acceder a un número entero.
    /// </remarks>
    /// <value>
    /// Un valor entero que puede ser leído y escrito.
    /// </value>
    public int Number { get; set; }
    /// <summary>
    /// Obtiene o establece el tiempo de reintento.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo. Si es nulo, significa que no hay un tiempo de reintento programado.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa el tiempo de reintento, o <c>null</c> si no está establecido.
    /// </value>
    public DateTime? RetryTime { get; set; }
    /// <summary>
    /// Obtiene o establece el mensaje asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje.
    /// </value>
    public string Message { get; set; }
}