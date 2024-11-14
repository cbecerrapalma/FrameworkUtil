using Microsoft.AspNetCore.Builder;
using Util.Microservices.Dapr.Events;

namespace Util.Microservices.Dapr;

/// <summary>
/// Proporciona métodos de extensión para agregar middleware de encabezados de eventos en la nube.
/// </summary>
public static class CloudEventHeadersMiddlewareExtensions {
    /// <summary>
    /// Extensión para configurar el middleware que maneja los encabezados de eventos en la nube.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación que se está configurando.</param>
    /// <returns>El mismo constructor de la aplicación con el middleware agregado.</returns>
    /// <remarks>
    /// Este método permite agregar el middleware <see cref="CloudEventHeadersMiddleware"/> 
    /// a la tubería de procesamiento de solicitudes de la aplicación.
    /// </remarks>
    public static IApplicationBuilder UseCloudEventHeaders( this IApplicationBuilder builder ) {
        return builder.UseMiddleware<CloudEventHeadersMiddleware>();
    }
}