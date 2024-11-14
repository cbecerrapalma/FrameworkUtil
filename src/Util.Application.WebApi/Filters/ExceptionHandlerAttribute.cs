using Util.Aop;
using Util.Helpers;
using Util.Exceptions;
using Util.AspNetCore;
using Util.SystemTextJson;

namespace Util.Applications.Filters; 

/// <summary>
/// Atributo que maneja excepciones en un controlador de ASP.NET.
/// </summary>
/// <remarks>
/// Este atributo permite interceptar excepciones lanzadas por las acciones del controlador
/// y realizar un manejo personalizado, como el registro de errores o la modificación de la respuesta.
/// </remarks>
public class ExceptionHandlerAttribute : ExceptionFilterAttribute {
    /// <summary>
    /// Maneja las excepciones que ocurren durante la ejecución de una acción.
    /// </summary>
    /// <param name="context">El contexto de la excepción que contiene información sobre la excepción y el estado actual de la solicitud.</param>
    /// <remarks>
    /// Este método establece que la excepción ha sido manejada, obtiene un mensaje de error basado en la excepción, 
    /// localiza el mensaje, determina un código de error y un código de estado HTTP, y finalmente establece el resultado 
    /// que se devolverá al cliente.
    /// </remarks>
    /// <seealso cref="ExceptionContext"/>
    /// <seealso cref="GetPrompt(Exception, bool)"/>
    /// <seealso cref="GetLocalizedMessages(ExceptionContext, string)"/>
    /// <seealso cref="GetResult(ExceptionContext, StateCode, string, int)"/>
    public override void OnException( ExceptionContext context ) {
        context.ExceptionHandled = true;
        var message = context.Exception.GetPrompt( Web.Environment.IsProduction() );
        message = GetLocalizedMessages( context, message );
        var errorCode = context.Exception.GetErrorCode() ?? StateCode.Fail;
        var httpStatusCode = context.Exception.GetHttpStatusCode() ?? 200;
        context.Result = GetResult( context, errorCode, message, httpStatusCode );
    }

    /// <summary>
    /// Obtiene un mensaje localizado basado en el contexto de la excepción y un mensaje proporcionado.
    /// </summary>
    /// <param name="context">El contexto de la excepción que contiene información sobre la excepción lanzada.</param>
    /// <param name="message">El mensaje que se desea localizar.</param>
    /// <returns>El mensaje localizado si se encuentra, de lo contrario, devuelve el mensaje original.</returns>
    /// <remarks>
    /// Este método verifica si la excepción es de tipo <see cref="Warning"/> y si se debe localizar el mensaje.
    /// Si las opciones de localización están habilitadas, intenta obtener un localizador de cadenas para localizar el mensaje.
    /// Si no se encuentra un mensaje localizado, se devuelve el mensaje original.
    /// </remarks>
    protected virtual string GetLocalizedMessages(ExceptionContext context, string message) {
        var exception = context.Exception.GetRawException();
        if (exception is not Warning warning)
            return message;
        if (warning.IsLocalization == false)
            return message;
        if (warning.IsLocalization == null) {
            var localizationOptions = context.HttpContext.RequestServices.GetService<IOptions<Util.Localization.LocalizationOptions>>();
            if (localizationOptions.Value.IsLocalizeWarning == false)
                return message;
        }
        var stringLocalizerFactory = context.HttpContext.RequestServices.GetService<IStringLocalizerFactory>();
        if (stringLocalizerFactory == null)
            return message;
        var stringLocalizer = stringLocalizerFactory.Create("Warning", null);
        var localizedString = stringLocalizer[message];
        if (localizedString.ResourceNotFound == false)
            return localizedString.Value;
        stringLocalizer = context.HttpContext.RequestServices.GetService<IStringLocalizer>();
        if (stringLocalizer == null)
            return message;
        return stringLocalizer[message];
    }

    /// <summary>
    /// Obtiene un resultado de acción basado en el contexto de excepción proporcionado.
    /// </summary>
    /// <param name="context">El contexto de excepción que contiene información sobre la solicitud actual.</param>
    /// <param name="code">El código que representa el resultado de la acción.</param>
    /// <param name="message">El mensaje que describe el resultado de la acción.</param>
    /// <param name="httpStatusCode">El código de estado HTTP que se debe devolver. Puede ser nulo.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la acción.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener una instancia de <see cref="IResultFactory"/> desde los servicios de la solicitud.
    /// Si no se encuentra, se crea un nuevo resultado utilizando el código, el mensaje y el código de estado HTTP proporcionados.
    /// </remarks>
    protected virtual IActionResult GetResult(ExceptionContext context, string code, string message, int? httpStatusCode)
    {
        var options = GetJsonSerializerOptions(context);
        var resultFactory = context.HttpContext.RequestServices.GetService<IResultFactory>();
        if (resultFactory == null)
            return new Result(code, message, null, httpStatusCode, options);
        return resultFactory.CreateResult(code, message, null, httpStatusCode, options);
    }
    /// <summary>
    /// Obtiene las opciones de serialización JSON para el contexto de excepción proporcionado.
    /// </summary>
    /// <param name="context">El contexto de excepción que contiene información sobre la solicitud HTTP actual.</param>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> que contiene las configuraciones de serialización JSON.
    /// Si se encuentra un <see cref="IJsonSerializerOptionsFactory"/>, se utilizarán sus opciones; 
    /// de lo contrario, se devolverán opciones predeterminadas.
    /// </returns>
    /// <remarks>
    /// Las opciones predeterminadas incluyen:
    /// <list type="bullet">
    /// <item>
    /// <description>Uso de la política de nombres de propiedad en formato camelCase.</description>
    /// </item>
    /// <item>
    /// <description>Codificación de caracteres Unicode.</description>
    /// </item>
    /// <item>
    /// <description>Ignorar propiedades con valores nulos durante la serialización.</description>
    /// </item>
    /// <item>
    /// <description>Conversores personalizados para tipos específicos como DateTime y long.</description>
    /// </item>
    /// </list>
    /// </remarks>
    private JsonSerializerOptions GetJsonSerializerOptions( ExceptionContext context ) {
        var factory = context.HttpContext.RequestServices.GetService<IJsonSerializerOptionsFactory>();
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
}