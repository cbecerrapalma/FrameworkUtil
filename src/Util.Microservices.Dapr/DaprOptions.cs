namespace Util.Microservices.Dapr; 

/// <summary>
/// Representa las opciones de configuración para Dapr.
/// </summary>
public class DaprOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece las opciones predeterminadas para la invocación de servicios y la 
    public DaprOptions() {
        ServiceInvocation = new ServiceInvocationOptions();
        Pubsub = new PubsubOptions();
    }

    /// <summary>
    /// Obtiene o establece el identificador de la aplicación.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador único de la aplicación.
    /// </value>
    public string AppId { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del almacén de secretos.
    /// </summary>
    /// <value>
    /// El nombre del almacén de secretos.
    /// </value>
    public string SecretStoreName { get; set; }
    /// <summary>
    /// Representa las opciones de invocación de servicio.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite configurar diferentes parámetros relacionados con la invocación de servicios.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo <see cref="ServiceInvocationOptions"/> que contiene las opciones de invocación.
    /// </value>
    public ServiceInvocationOptions ServiceInvocation { get; set; }
    /// <summary>
    /// Obtiene o establece las opciones de Pub/Sub.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="PubsubOptions"/> que contiene la configuración de Pub/Sub.
    /// </value>
    public PubsubOptions Pubsub { get; set; }
}