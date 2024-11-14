namespace Util.Microservices.Dapr.Events; 

/// <summary>
/// Representa un evento en la nube con datos de tipo genérico.
/// </summary>
/// <typeparam name="TData">El tipo de los datos asociados al evento.</typeparam>
public class CloudEvent<TData> : CloudEvent {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CloudEvent"/>.
    /// </summary>
    /// <param name="id">Identificador único del evento. Si está vacío, se generará un nuevo GUID.</param>
    /// <param name="data">Los datos asociados al evento.</param>
    /// <typeparam name="TData">El tipo de los datos que se asocian al evento.</typeparam>
    /// <remarks>
    /// El tipo de contenido de los datos se establece como "application/cloudevents+json".
    /// Además, se inicializa un diccionario para los encabezados del evento.
    /// </remarks>
    public CloudEvent( string id, TData data ) {
        Id = id.IsEmpty() ? Guid.NewGuid().ToString() : id;
        Data = data;
        DataContentType = "application/cloudevents+json";
        Headers = new Dictionary<string, string>();
    }

    /// <summary>
    /// Representa el identificador único de un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar el ID de un objeto en formato de cadena.
    /// </remarks>
    /// <returns>
    /// Un valor de tipo <see cref="string"/> que representa el identificador.
    /// </returns>
    [JsonPropertyName( "id" )]
    public string Id { get; }

    /// <summary>
    /// Representa la propiedad que contiene los datos de tipo genérico.
    /// </summary>
    /// <typeparam name="TData">El tipo de los datos que se almacenan en la propiedad <c>Data</c>.</typeparam>
    /// <value>
    /// Un objeto de tipo <typeparamref name="TData"/> que representa los datos.
    /// </value>
    [JsonPropertyName( "data" )]
    public TData Data { get; set; }

    /// <summary>
    /// Representa un conjunto de encabezados en formato clave-valor.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar información adicional que puede ser necesaria para la 
    /// comunicación entre servicios o para la configuración de solicitudes y respuestas.
    /// </remarks>
    /// <returns>
    /// Un diccionario que contiene los encabezados, donde la clave es el nombre del encabezado y el 
    /// valor es su contenido asociado.
    /// </returns>
    [JsonPropertyName( "headers" )]
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Representa el tipo de contenido de los datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para especificar el tipo de contenido de los datos que se están manejando.
    /// </remarks>
    /// <returns>
    /// Un string que indica el tipo de contenido de los datos.
    /// </returns>
    [JsonPropertyName( "datacontenttype" )]
    public string DataContentType { get; init; }

    /// <summary>
    /// Obtiene los datos almacenados como un tipo genérico.
    /// </summary>
    /// <typeparam name="T">El tipo de datos que se desea obtener.</typeparam>
    /// <returns>
    /// Un objeto del tipo especificado <typeparamref name="T"/> que representa los datos almacenados.
    /// </returns>
    /// <remarks>
    /// Este método realiza un casting de los datos a un tipo genérico. Asegúrese de que el tipo especificado
    /// sea compatible con el tipo de datos almacenados para evitar excepciones de tiempo de ejecución.
    /// </remarks>
    public T GetData<T>() {
        return (T)(object)Data;
    }
}