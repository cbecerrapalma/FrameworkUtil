using Microsoft.AspNetCore.Http;
using Util.Applications.Logging;
using Util.AspNetCore;
using Util.Infrastructure;
using Util.Logging;
using Util.SystemTextJson;

namespace Util.Applications.Infrastructure;

/// <summary>
/// Clase que se encarga de registrar servicios para una API web.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IServiceRegistrar"/> y proporciona 
/// la funcionalidad necesaria para registrar los servicios requeridos por la API.
/// </remarks>
public class WebApiServiceRegistrar : IServiceRegistrar
{
    /// <summary>
    /// Obtiene el nombre del servicio de la infraestructura de la aplicación web.
    /// </summary>
    /// <value>
    /// El nombre del servicio como una cadena.
    /// </value>
    public static string ServiceName => "Util.Applications.Infrastructure.WebApiServiceRegistrar";

    /// <summary>
    /// Representa el identificador único de un pedido.
    /// </summary>
    /// <value>
    /// El identificador del pedido, que es un número entero.
    /// </value>
    public int OrderId => 1210;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina llamando al método <see cref="ServiceRegistrarConfig.IsEnabled(string)"/> 
    /// con el nombre del servicio especificado por <see cref="ServiceName"/>.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled(ServiceName);

    /// <summary>
    /// Registra los servicios necesarios en el contenedor de servicios.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene la configuración del host.</param>
    /// <returns>
    /// Un objeto <see cref="Action"/> que representa la acción de registro. 
    /// En este caso, siempre devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método configura los servicios requeridos para el registro de contexto de logs 
    /// y otros filtros de inicio necesarios para la aplicación.
    /// </remarks>
    public Action Register(ServiceContext serviceContext)
    {
        serviceContext.HostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<ILogContextAccessor, LogContextAccessor>();
            services.AddSingleton<IStartupFilter, LogContextStartupFilter>();
            ConfigApiBehaviorOptions(services);
        });
        return null;
    }

    /// <summary>
    /// Configura las opciones del comportamiento de la API.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registran las configuraciones.</param>
    /// <remarks>
    /// Este método establece un comportamiento personalizado para manejar respuestas de estado de modelo inválido.
    /// Se registra un registro de errores cuando el estado del modelo no es válido y se devuelve un mensaje localizado.
    /// </remarks>
    /// <seealso cref="ApiBehaviorOptions"/>
    private void ConfigApiBehaviorOptions(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = false;
            var builtInFactory = options.InvalidModelStateResponseFactory;
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var log = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<WebApiServiceRegistrar>>();
                if (actionContext.ModelState.IsValid)
                    return builtInFactory(actionContext);
                var error = GetModelError(actionContext);
                log.LogError(error.Exception, "Error de vinculación de modelo, ErrorMessage: {ErrorMessage}", error.ErrorMessage);
                var message = GetLocalizedMessages(actionContext.HttpContext, error.ErrorMessage);
                return GetResult(actionContext.HttpContext, StateCode.Fail, message, 200);
            };
        });
    }
    /// <summary>
    /// Obtiene el primer error de modelo encontrado en el contexto de acción.
    /// </summary>
    /// <param name="actionContext">El contexto de acción que contiene el estado del modelo.</param>
    /// <returns>
    /// Un objeto <see cref="ModelError"/> que representa el primer error encontrado, 
    /// o <c>null</c> si no hay errores en el estado del modelo.
    /// </returns>

    private ModelError GetModelError(ActionContext actionContext)
    {
        foreach (var state in actionContext.ModelState)
        {
            foreach (var error in state.Value.Errors)
                return error;
        }
        return null;
    }
    /// <summary>
    /// Obtiene un mensaje localizado a partir del contexto HTTP y un mensaje dado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene los servicios necesarios para la localización.</param>
    /// <param name="message">El mensaje que se desea localizar.</param>
    /// <returns>El mensaje localizado si se encuentra; de lo contrario, se devuelve el mensaje original.</returns>
    /// <remarks>
    /// Este método intenta obtener un localizador de cadenas a partir del contexto HTTP.
    /// Si no se encuentra un localizador de cadenas, se devuelve el mensaje original.
    /// Primero, intenta localizar el mensaje utilizando un localizador específico para el ensamblado actual.
    /// Si no se encuentra el mensaje, intenta obtener un localizador de cadenas genérico.
    /// </remarks>
    /// <seealso cref="IStringLocalizerFactory"/>
    /// <seealso cref="IStringLocalizer"/>
    protected virtual string GetLocalizedMessages(HttpContext context, string message)
    {
        var stringLocalizerFactory = context.RequestServices.GetService<IStringLocalizerFactory>();
        if (stringLocalizerFactory == null)
            return message;
        var assemblyName = new AssemblyName(GetType().Assembly.FullName);
        var stringLocalizer = stringLocalizerFactory.Create("Warning", assemblyName.Name);
        var localizedString = stringLocalizer[message];
        if (localizedString.ResourceNotFound == false)
            return localizedString.Value;
        stringLocalizer = context.RequestServices.GetService<IStringLocalizer>();
        if (stringLocalizer == null)
            return message;
        return stringLocalizer[message];
    }

    /// <summary>
    /// Obtiene un resultado basado en el contexto HTTP, un código, un mensaje y un código de estado HTTP opcional.
    /// </summary>
    /// <param name="context">El contexto HTTP actual que contiene información sobre la solicitud y la respuesta.</param>
    /// <param name="code">El código que se incluirá en el resultado.</param>
    /// <param name="message">El mensaje que se incluirá en el resultado.</param>
    /// <param name="httpStatusCode">El código de estado HTTP opcional que se asignará al resultado.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay una fábrica de resultados disponible en el contexto de la solicitud.
    /// Si no se encuentra una fábrica, se crea un resultado utilizando el constructor de la clase <see cref="Result"/>.
    /// De lo contrario, se utiliza la fábrica para crear el resultado.
    /// </remarks>
    protected virtual IActionResult GetResult(HttpContext context, string code, string message, int? httpStatusCode)
    {
        var options = GetJsonSerializerOptions(context);
        var resultFactory = context.RequestServices.GetService<IResultFactory>();
        if (resultFactory == null)
            return new Result(code, message, null, httpStatusCode, options);
        return resultFactory.CreateResult(code, message, null, httpStatusCode, options);
    }

    /// <summary>
    /// Obtiene las opciones de serialización JSON para el contexto HTTP actual.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene los servicios requeridos.</param>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> configurado según el contexto.
    /// Si se encuentra un <see cref="IJsonSerializerOptionsFactory"/>, se utilizará para crear las opciones.
    /// De lo contrario, se devolverán opciones predeterminadas con una política de nomenclatura en minúsculas y
    /// configuraciones específicas para la serialización de fechas y números.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para asegurar que la serialización JSON se realice de manera consistente
    /// en toda la aplicación, utilizando configuraciones adecuadas según el contexto de la solicitud.
    /// </remarks>
    /// <seealso cref="IJsonSerializerOptionsFactory"/>
    /// <seealso cref="JsonSerializerOptions"/>
    /// <seealso cref="JsonNamingPolicy"/>
    /// <seealso cref="JavaScriptEncoder"/>
    /// <seealso cref="JsonIgnoreCondition"/>

    private JsonSerializerOptions GetJsonSerializerOptions(HttpContext context)
    {
        var factory = context.RequestServices.GetService<IJsonSerializerOptionsFactory>();
        if (factory != null)
            return factory.CreateOptions();
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
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