namespace Util.Microservices.Events;

/// <summary>
/// Representa un registro de eventos de integración nulo.
/// Esta clase hereda de <see cref="IntegrationEventLog"/> y no implementa
/// ninguna funcionalidad adicional.
/// </summary>
public class NullIntegrationEventLog : IntegrationEventLog {
    public static readonly IntegrationEventLog Instance = new NullIntegrationEventLog();
}

/// <summary>
/// Representa un registro de eventos de integración específico para un tipo de objeto.
/// Esta clase hereda de <see cref="IntegrationEventLog{T}"/> y se utiliza para gestionar el registro de eventos de integración sin un tipo específico.
/// </summary>
public class IntegrationEventLog : IntegrationEventLog<object> {
}

/// <summary>
/// Representa un registro de evento de integración que almacena información sobre un evento específico.
/// </summary>
/// <typeparam name="TValue">El tipo de valor asociado con el evento de integración.</typeparam>
public class IntegrationEventLog<TValue> : StateBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IntegrationEventLog"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de <see cref="SubscriptionLog"/> 
    /// para almacenar los registros de suscripción relacionados con el evento de integración.
    /// </remarks>
    public IntegrationEventLog() {
        SubscriptionLogs = new List<SubscriptionLog>();
    }

    /// <summary>
    /// Obtiene o establece el identificador de la aplicación.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador único de la aplicación.
    /// </value>
    public string AppId { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador del usuario.
    /// </summary>
    /// <value>
    /// El identificador del usuario como una cadena.
    /// </value>
    public string UserId { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del pub/sub.
    /// </summary>
    /// <value>
    /// El nombre del pub/sub.
    /// </value>
    public string PubsubName { get; set; }
    /// <summary>
    /// Obtiene o establece el tema asociado.
    /// </summary>
    /// <value>
    /// Una cadena que representa el tema.
    /// </value>
    public string Topic { get; set; }
    /// <summary>
    /// Obtiene o establece el valor asociado.
    /// </summary>
    /// <value>
    /// El valor de tipo <typeparamref name="TValue"/> que está asociado.
    /// </value>
    /// <typeparam name="TValue">El tipo del valor asociado.</typeparam>
    public TValue Value { get; set; }
    /// <summary>
    /// Obtiene o establece el estado del evento.
    /// </summary>
    /// <value>
    /// Un objeto de tipo <see cref="EventState"/> que representa el estado actual del evento.
    /// </value>
    public EventState State { get; set; }
    /// <summary>
    /// Obtiene o establece la fecha y hora de 
    public DateTime PublishTime { get; set; }
    /// <summary>
    /// Obtiene o establece la fecha y hora de la última modificación.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora en que se realizó la última modificación.
    /// </value>
    public DateTime LastModificationTime { get; set; }
    /// <summary>
    /// Obtiene o establece la lista de registros de suscripción.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una colección de objetos <see cref="SubscriptionLog"/> que representan los registros de suscripción.
    /// </remarks>
    /// <returns>
    /// Una lista de <see cref="SubscriptionLog"/> que contiene los registros de suscripción.
    /// </returns>
    public List<SubscriptionLog> SubscriptionLogs { get; set; }

    /// <summary>
    /// Obtiene el valor almacenado como un tipo específico.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir el valor almacenado.</typeparam>
    /// <returns>El valor almacenado convertido al tipo especificado.</returns>
    /// <remarks>
    /// Este método realiza una conversión de tipo de un objeto almacenado en la propiedad 'Value'.
    /// Asegúrese de que el tipo especificado sea compatible con el tipo del valor almacenado,
    /// de lo contrario, se producirá una excepción de tiempo de ejecución.
    /// </remarks>
    public T GetValue<T>() {
        return (T)(object)Value;
    }
}