using Util.Events;
using Util.Http;

namespace Util.Microservices; 

/// <summary>
/// Define un cliente para interactuar con microservicios.
/// </summary>
public interface IMicroserviceClient {
    /// <summary>
    /// Obtiene una instancia de <see cref="IHttpClient"/> que permite realizar solicitudes HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a un cliente HTTP configurado,
    /// el cual puede ser utilizado para realizar operaciones de red.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IHttpClient"/> que se puede utilizar para enviar solicitudes y recibir respuestas.
    /// </returns>
    IHttpClient HttpClient { get; }
    /// <summary>
    /// Obtiene la instancia de <see cref="IServiceInvocation"/> asociada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a la implementación del servicio de invocación,
    /// que puede ser utilizada para realizar llamadas a métodos de servicio.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IServiceInvocation"/> que representa el servicio de invocación.
    /// </returns>
    IServiceInvocation ServiceInvocation { get; }
    /// <summary>
    /// Obtiene la instancia del bus de eventos de integración.
    /// </summary>
    /// <value>
    /// Una implementación de <see cref="IIntegrationEventBus"/> que permite la 
    IIntegrationEventBus IntegrationEventBus { get; }
    /// <summary>
    /// Obtiene la instancia del administrador de estado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al objeto que gestiona el estado de la aplicación,
    /// permitiendo la recuperación y almacenamiento de datos de estado entre las solicitudes.
    /// </remarks>
    /// <value>
    /// Una instancia que implementa la interfaz <see cref="IStateManager"/>.
    /// </value>
    IStateManager StateManager { get; }
}