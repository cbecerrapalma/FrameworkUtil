using Util.SystemTextJson;

namespace Util.Applications; 

/// <summary>
/// Representa el resultado de una operación en formato JSON.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="JsonResult"/> y se utiliza para encapsular la respuesta JSON 
/// que se enviará al cliente. Permite personalizar el comportamiento del resultado JSON 
/// según las necesidades de la aplicación.
/// </remarks>
public class Result : JsonResult {
    /// <summary>
    /// Obtiene el código asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un valor de tipo string que representa el código.
    /// </remarks>
    /// <value>
    /// Un string que contiene el código.
    /// </value>
    public string Code { get; }
    /// <summary>
    /// Obtiene el mensaje asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje.
    /// </value>
    public string Message { get; }
    /// <summary>
    /// Obtiene los datos dinámicos asociados a esta instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a un conjunto de datos que pueden variar en tipo y estructura,
    /// lo que proporciona flexibilidad en el manejo de información.
    /// </remarks>
    /// <value>
    /// Un objeto dinámico que representa los datos asociados.
    /// </value>
    public dynamic Data { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Result"/>.
    /// </summary>
    /// <param name="code">El código que representa el resultado.</param>
    /// <param name="message">Un mensaje que describe el resultado.</param>
    /// <param name="data">Datos adicionales relacionados con el resultado. Este parámetro es opcional.</param>
    /// <param name="httpStatusCode">El código de estado HTTP asociado con el resultado. Este parámetro es opcional.</param>
    /// <param name="options">Opciones de serialización JSON. Este parámetro es opcional.</param>
    /// <remarks>
    /// Esta clase se utiliza para encapsular el resultado de una operación, incluyendo un código, un mensaje, 
    /// datos adicionales y un código de estado HTTP.
    /// </remarks>
    public Result( string code, string message, dynamic data = null, int? httpStatusCode = null, JsonSerializerOptions options = null ) : base( null ) {
        Code = code;
        Message = message;
        Data = data;
        SerializerSettings = GetOptions( options );
        StatusCode = httpStatusCode;
    }

    /// <summary>
    /// Obtiene las opciones de serialización JSON. Si se proporcionan opciones, se devuelven esas opciones; de lo contrario, se crean nuevas opciones con configuraciones predeterminadas.
    /// </summary>
    /// <param name="options">Las opciones de serialización JSON proporcionadas. Si es <c>null</c>, se generarán nuevas opciones.</param>
    /// <returns>Las opciones de serialización JSON a utilizar.</returns>
    /// <remarks>
    /// Las opciones predeterminadas incluyen:
    /// <list type="bullet">
    /// <item>Uso de la política de nomenclatura en formato camelCase.</item>
    /// <item>Codificación de caracteres Unicode.</item>
    /// <item>Ignorar propiedades con valores nulos durante la serialización.</item>
    /// <item>Conversores personalizados para manejar fechas y fechas nulas.</item>
    /// </list>
    /// </remarks>
    private JsonSerializerOptions GetOptions( JsonSerializerOptions options ) {
        if ( options != null )
            return options;
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter()
            }
        };
    }

    /// <summary>
    /// Ejecuta el resultado de la acción de forma asíncrona.
    /// </summary>
    /// <param name="context">El contexto de la acción que contiene información sobre la solicitud y la respuesta.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método crea un objeto anónimo que contiene el código, el mensaje y los datos, 
    /// y luego llama al método base para completar la ejecución del resultado.
    /// </remarks>
    public override Task ExecuteResultAsync( ActionContext context ) {
        Value = new {
            Code,
            Message,
            Data
        };
        return base.ExecuteResultAsync( context );
    }
}