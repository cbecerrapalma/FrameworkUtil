namespace Util.Microservices.Events;

/// <summary>
/// Representa un registro de suscripción que almacena información sobre las suscripciones de los usuarios.
/// </summary>
public class SubscriptionLog {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SubscriptionLog"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de registros de reintento de suscripción.
    /// </remarks>
    public SubscriptionLog() {
        RetryLogs = new List<SubscriptionRetryLog>();
    }

    /// <summary>
    /// Obtiene o establece el identificador de la aplicación.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador de la aplicación.
    /// </value>
    public string AppId { get; set; }
    /// <summary>
    /// Obtiene o establece la URL de la ruta.
    /// </summary>
    /// <value>
    /// La URL de la ruta como una cadena.
    /// </value>
    public string RouteUrl { get; set; }
    /// <summary>
    /// Obtiene o establece el número de reintentos permitidos.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que no se especifica un límite de reintentos.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de reintentos, o null si no se ha establecido.
    /// </value>
    public int? RetryCount { get; set; }
    /// <summary>
    /// Representa el estado de la suscripción.
    /// </summary>
    /// <value>
    /// Un valor de la enumeración <see cref="SubscriptionState"/> que indica el estado actual de la suscripción.
    /// </value>
    public SubscriptionState State { get; set; }
    /// <summary>
    /// Obtiene o establece la fecha y hora de la suscripción.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa el momento en que se realizó la suscripción.
    /// </value>
    public DateTime SubscriptionTime { get; set; }
    /// <summary>
    /// Obtiene o establece la fecha y hora de la última modificación.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora en que se realizó la última modificación.
    /// </value>
    public DateTime LastModificationTime { get; set; }
    /// <summary>
    /// Obtiene o establece la lista de registros de reintentos de suscripción.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena los registros que contienen información sobre los reintentos realizados 
    /// para las suscripciones, permitiendo un seguimiento detallado de los intentos fallidos y exitosos.
    /// </remarks>
    /// <value>
    /// Una lista de objetos <see cref="SubscriptionRetryLog"/> que representan los registros de reintentos.
    /// </value>
    public List<SubscriptionRetryLog> RetryLogs { get; set; }
}