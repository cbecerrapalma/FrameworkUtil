using Util.Applications.Filters;
using Util.AspNetCore;
using Util.Helpers;
using Util.Logging;
using Util.Properties;
using Util.Sessions;
using Util.SystemTextJson;

namespace Util.Applications.Controllers;

/// <summary>
/// Controlador API para manejar las operaciones relacionadas con el recurso especificado.
/// </summary>
/// <remarks>
/// Este controlador proporciona métodos para realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) 
/// sobre el recurso. Se espera que los métodos devuelvan respuestas adecuadas en formato JSON.
/// </remarks>
[ApiController]
[Route( "api/[controller]" )]
[ExceptionHandler( Order = 1 )]
[ErrorLogFilter( Order = 2 )]
public abstract class WebApiControllerBase : ControllerBase {
    /// <summary>
    /// Obtiene la instancia de la sesión de usuario actual.
    /// </summary>
    /// <value>
    /// La instancia de <see cref="ISession"/> que representa la sesión de usuario.
    /// </value>
    /// <remarks>
    /// Esta propiedad es virtual y puede ser sobreescrita en clases derivadas.
    /// </remarks>
    protected virtual ISession Session => UserSession.Instance;

    /// <summary>
    /// Obtiene una instancia de registro (log) para el tipo actual.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que representa el registro para el tipo actual.
    /// Si ocurre un error al crear el registro, se devuelve una instancia de <see cref="NullLog"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar un comportamiento
    /// personalizado en la creación del registro.
    /// </remarks>
    protected virtual ILog GetLog() {
        try {
            var logFactory = Ioc.Create<ILogFactory>();
            return logFactory.CreateLog(GetType());
        }
        catch {
            return NullLog.Instance;
        }
    }

    /// <summary>
    /// Devuelve un resultado exitoso con los datos y mensaje especificados.
    /// </summary>
    /// <param name="data">Los datos que se incluirán en el resultado. Puede ser nulo.</param>
    /// <param name="message">El mensaje que se devolverá. Si es nulo, se utilizará un mensaje predeterminado.</param>
    /// <param name="statusCode">El código de estado HTTP que se devolverá. Por defecto es 200.</param>
    /// <returns>Un objeto <see cref="IActionResult"/> que representa el resultado exitoso.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas.
    /// </remarks>
    protected virtual IActionResult Success(dynamic data = null, string message = null, int? statusCode = 200) {
        message ??= R.Success;
        return GetResult(StateCode.Ok, message, data, statusCode);
    }

    /// <summary>
    /// Obtiene un resultado basado en los parámetros proporcionados.
    /// </summary>
    /// <param name="code">El código que representa el resultado.</param>
    /// <param name="message">El mensaje asociado al resultado.</param>
    /// <param name="data">Datos adicionales que se incluirán en el resultado.</param>
    /// <param name="httpStatusCode">El código de estado HTTP opcional que se utilizará.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener una instancia de <see cref="IResultFactory"/> desde los servicios de la solicitud HTTP.
    /// Si no se encuentra, se crea un nuevo resultado utilizando el constructor de <see cref="Result"/>.
    /// </remarks>
    private IActionResult GetResult( string code, string message, dynamic data, int? httpStatusCode ) {
        var options = GetJsonSerializerOptions();
        var resultFactory = HttpContext.RequestServices.GetService<IResultFactory>();
        if ( resultFactory == null )
            return new Result( code, message, data, httpStatusCode, options );
        return resultFactory.CreateResult( code, message, data, httpStatusCode, options );
    }

    /// <summary>
    /// Obtiene las opciones de serialización JSON para la respuesta.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> configurado con políticas de nombrado, codificación y conversores personalizados.
    /// </returns>
    /// <remarks>
    /// Si se encuentra un <see cref="IJsonSerializerOptionsFactory"/> en el contenedor de servicios, se utilizará para crear las opciones.
    /// De lo contrario, se retornarán opciones predeterminadas que incluyen políticas específicas para el nombrado de propiedades,
    /// codificación de caracteres y condiciones de ignorar propiedades.
    /// </remarks>
    private JsonSerializerOptions GetJsonSerializerOptions() {
        var factory = HttpContext.RequestServices.GetService<IJsonSerializerOptionsFactory>();
        if( factory != null )
            return factory.CreateOptions();
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }

    /// <summary>
    /// Devuelve un resultado que indica un fallo en la operación.
    /// </summary>
    /// <param name="message">El mensaje que describe el fallo.</param>
    /// <param name="statusCode">El código de estado HTTP que se debe devolver. Por defecto es 200.</param>
    /// <returns>Un objeto <see cref="IActionResult"/> que representa el resultado del fallo.</returns>
    protected virtual IActionResult Fail(string message, int? statusCode = 200) {
        return GetResult(StateCode.Fail, message, null, statusCode);
    }

    /// <summary>
    /// Devuelve un resultado de archivo a partir de un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a devolver como archivo.</param>
    /// <returns>
    /// Un objeto <see cref="FileStreamResult"/> que representa el flujo de datos como un archivo.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para enviar un flujo de bytes al cliente con el tipo de contenido
    /// "application/octet-stream", lo que indica que se trata de un archivo binario.
    /// </remarks>
    protected IActionResult GetStreamResult(Stream stream) {
        return new FileStreamResult(stream, "application/octet-stream");
    }

    /// <summary>
    /// Obtiene un resultado de flujo a partir de un arreglo de bytes.
    /// </summary>
    /// <param name="stream">El arreglo de bytes que se convertirá en un flujo de memoria.</param>
    /// <returns>Un objeto <see cref="IActionResult"/> que representa el resultado del flujo.</returns>
    protected IActionResult GetStreamResult(byte[] stream)
    {
        return GetStreamResult(new MemoryStream(stream));
    }
}