namespace Util.Microservices.Dapr; 

/// <summary>
/// Representa las opciones de configuración para el sistema de 
public class PubsubOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PubsubOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece los valores predeterminados para las opciones de Pub/Sub.
    /// </remarks>
    public PubsubOptions() {
        EventLogStoreName = "event-state-store";
        ImportHeaderKeys = new List<string> {
            "Authorization",
            "x-correlation-id"
        };
        EnableEventLog = true;
        MaxRetry = 3;
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si el registro de eventos está habilitado.
    /// </summary>
    /// <remarks>
    /// Si este valor es verdadero, se registrarán los eventos en el registro de eventos del sistema.
    /// De lo contrario, no se registrarán eventos.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el registro de eventos está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    public bool EnableEventLog { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del almacén de registros de eventos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para identificar el almacén donde se almacenan los registros de eventos.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre del almacén de registros de eventos.
    /// </value>
    public string EventLogStoreName { get; set; }

    /// <summary>
    /// Obtiene o establece una lista de claves de encabezado para la importación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar las claves que se utilizarán al importar datos,
    /// permitiendo la configuración dinámica de los encabezados según las necesidades del proceso de importación.
    /// </remarks>
    /// <value>
    /// Una lista de cadenas que representa las claves de encabezado.
    /// </value>
    public IList<string> ImportHeaderKeys { get; set; }

    /// <summary>
    /// Obtiene o establece el número máximo de reintentos.
    /// </summary>
    /// <remarks>
    /// Este valor determina cuántas veces se intentará realizar una operación antes de considerar que ha fallado.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número máximo de reintentos permitidos.
    /// </value>
    public int MaxRetry { get; set; }
}