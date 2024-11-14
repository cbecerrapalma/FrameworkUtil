using Util.Aop;

namespace Util.Microservices.Dapr.Events.Filters;

/// <summary>
/// Filtro de excepciones para registrar errores relacionados con suscripciones.
/// </summary>
/// <remarks>
/// Este filtro se utiliza para capturar y registrar excepciones que ocurren durante el procesamiento de solicitudes
/// relacionadas con suscripciones, permitiendo una mejor gestión y diagnóstico de errores.
/// </remarks>
public class SubscriptionErrorLogFilterAttribute : ExceptionFilterAttribute
{
    /// <summary>
    /// Maneja las excepciones que ocurren durante el procesamiento de eventos de suscripción.
    /// </summary>
    /// <param name="context">El contexto de la excepción que contiene información sobre la solicitud y la excepción lanzada.</param>
    /// <remarks>
    /// Este método se invoca cuando se produce una excepción en el proceso de suscripción de eventos.
    /// Registra la excepción utilizando el servicio de registro adecuado, diferenciando entre advertencias y errores.
    /// </remarks>
    public override void OnException(ExceptionContext context)
    {
        if (context == null)
            return;
        var log = context.HttpContext.RequestServices.GetService<ILogger<SubscriptionErrorLogFilterAttribute>>();
        var exception = context.Exception.GetRawException();
        if (exception is Warning warning)
        {
            log.LogWarning(warning, $"Integración de suscripción de eventos fallida:{exception.Message}");
            return;
        }
        log.LogError(exception, $"Integración de suscripción de eventos fallida:{exception.Message}");
    }
}