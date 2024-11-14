using Microsoft.AspNetCore.Http.Extensions;

namespace Util.Microservices.Dapr.Events.Filters;

/// <summary>
/// Atributo que filtra las acciones en función de las suscripciones.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para aplicar lógica de filtrado a las acciones de un controlador,
/// permitiendo que solo se ejecuten aquellas que cumplen con los criterios de suscripción.
/// </remarks>
public class SubscriptionFilterAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Método que se ejecuta antes y después de la ejecución de una acción, 
    /// permitiendo la manipulación de la solicitud de eventos de integración.
    /// </summary>
    /// <param name="context">El contexto de la acción que se está ejecutando.</param>
    /// <param name="next">El delegado que representa la siguiente acción en la cadena de ejecución.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método verifica si la solicitud es de tipo "application/cloudevents+json" 
    /// y maneja la lógica de suscripción a eventos de integración. 
    /// Se registran errores y se realizan acciones antes y después de la ejecución de la acción.
    /// </remarks>
    /// <exception cref="Exception">Se lanza si ocurre un error durante la ejecución del filtro.</exception>
    /// <seealso cref="ILogger{T}"/>
    /// <seealso cref="IIntegrationEventManager"/>
    /// <seealso cref="IPubsubCallback"/>
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context == null)
            return;
        if (next == null)
            return;
        if (context.HttpContext.Request.ContentType != "application/cloudevents+json")
            return;
        var log = context.HttpContext.RequestServices.GetService<ILogger<SubscriptionFilterAttribute>>() ?? NullLogger<SubscriptionFilterAttribute>.Instance;
        try
        {
            var body = await GetBodyJsonElement(context);
            var routeUrl = context.HttpContext.Request.GetDisplayUrl();
            var eventId = GetEventId(body);
            if (eventId.IsEmpty())
            {
                log.LogError("Integración de evento identificador vacío, cuerpo:{@body}, rutaUrl:{routeUrl}", body, routeUrl);
                return;
            }
            log.LogTrace("Preparándose para procesar la suscripción de eventos integrados, eventId={@eventId}, routeUrl={routeUrl}", eventId, routeUrl);
            var manager = context.HttpContext.RequestServices.GetRequiredService<IIntegrationEventManager>();
            var eventLog = await GetEventLog(manager, eventId);
            if (eventLog == null)
            {
                log.LogError("Integración de evento vacío, eventId={@eventId}, cuerpo:{@body}, rutaUrl:{routeUrl}", eventId, body, routeUrl);
                return;
            }
            if (manager.CanSubscription(eventLog) == false)
            {
                log.LogDebug("La suscripción a eventos integrados no requiere procesamiento, eventId={@eventId}, body:{@body}, routeUrl={routeUrl}", eventId, body, routeUrl);
                if (manager.IsSubscriptionSuccess(eventLog))
                    context.Result = PubsubResult.Success;
                return;
            }
            var callback = context.HttpContext.RequestServices.GetRequiredService<IPubsubCallback>();
            await callback.OnSubscriptionBefore(eventLog, routeUrl);
            var executedContext = await next();
            OnActionExecuted(executedContext);
            if (executedContext.Result is PubsubResult result)
            {
                await callback.OnSubscriptionAfter(eventId, result == PubsubResult.Success, result.Message);
                return;
            }
            if (executedContext.Exception != null)
            {
                await callback.OnSubscriptionAfter(eventId, false, Warning.GetMessage(executedContext.Exception));
                return;
            }
        }
        catch (Exception exception)
        {
            log.LogError(exception, "Integración de la suscripción de eventos, el procesamiento del filtro falló.");
            throw;
        }
    }

    /// <summary>
    /// Obtiene un elemento JSON del cuerpo de la solicitud.
    /// </summary>
    /// <param name="context">El contexto de ejecución de la acción que contiene información sobre la solicitud HTTP actual.</param>
    /// <returns>
    /// Un <see cref="JsonElement"/> que representa el cuerpo de la solicitud en formato JSON.
    /// </returns>
    /// <remarks>
    /// Este método lee el cuerpo de la solicitud de forma asíncrona y lo convierte en un elemento JSON utilizando las opciones de serialización proporcionadas por el cliente Dapr, si está disponible.
    /// </remarks>
    /// <seealso cref="ActionExecutingContext"/>
    /// <seealso cref="DaprClient"/>
    /// <seealso cref="Util.Helpers.Json.ToObjectAsync{T}"/>
    protected async Task<JsonElement> GetBodyJsonElement(ActionExecutingContext context)
    {
        var body = await GetBody(context);
        var client = context.HttpContext.RequestServices.GetService<DaprClient>();
        return await Util.Helpers.Json.ToObjectAsync<JsonElement>(body, client?.JsonSerializerOptions);
    }

    /// <summary>
    /// Obtiene el cuerpo de la solicitud HTTP como un arreglo de bytes.
    /// </summary>
    /// <param name="context">El contexto de la acción que contiene la solicitud HTTP.</param>
    /// <returns>Un arreglo de bytes que representa el cuerpo de la solicitud.</returns>
    /// <remarks>
    /// Este método habilita el almacenamiento en búfer de la solicitud para permitir la lectura del cuerpo más de una vez.
    /// Se utiliza de forma asíncrona para mejorar el rendimiento y la capacidad de respuesta de la aplicación.
    /// </remarks>
    protected async Task<byte[]> GetBody(ActionExecutingContext context)
    {
        context.HttpContext.Request.EnableBuffering();
        return await Util.Helpers.File.ReadToBytesAsync(context.HttpContext.Request.Body);
    }

    /// <summary>
    /// Obtiene el identificador del evento desde un elemento JSON.
    /// </summary>
    /// <param name="body">El elemento JSON que contiene la propiedad "eventId".</param>
    /// <returns>El identificador del evento como una cadena, o null si no se encuentra la propiedad.</returns>
    protected string GetEventId(JsonElement body)
    {
        return body.TryGetProperty("eventId", out var eventId) ? eventId.Deserialize<string>() : null;
    }

    /// <summary>
    /// Obtiene el registro de eventos de integración asociado a un identificador de evento específico.
    /// </summary>
    /// <param name="manager">El administrador de eventos de integración que se utilizará para recuperar el registro.</param>
    /// <param name="eventId">El identificador del evento que se desea obtener.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro del evento de integración, 
    /// o null si no se encuentra el registro después de 100 intentos.
    /// </returns>
    /// <remarks>
    /// Este método intenta recuperar el registro del evento hasta 100 veces, 
    /// con un retraso de 100 milisegundos entre cada intento. 
    /// Si el registro no se encuentra después de 100 intentos, se devuelve null.
    /// </remarks>
    protected async Task<IntegrationEventLog> GetEventLog(IIntegrationEventManager manager, string eventId)
    {
        for (var i = 0; i < 100; i++)
        {
            var eventLog = await manager.GetAsync(eventId);
            if (eventLog != null)
                return eventLog;
            await Task.Delay(100);
        }
        return null;
    }
}