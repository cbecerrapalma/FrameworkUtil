using Util.Microservices.Dapr.Events;
using Util.Microservices.Dapr.ServiceInvocations;
using Util.Microservices.Dapr.StateManagements;

namespace Util.Microservices.Dapr; 

/// <summary>
/// Representa un cliente para interactuar con microservicios utilizando Dapr.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IMicroserviceClient"/> y proporciona métodos para 
/// realizar llamadas a microservicios a través de Dapr, facilitando la comunicación entre 
/// diferentes componentes de una arquitectura de microservicios.
/// </remarks>
public class DaprMicroserviceClient : IMicroserviceClient {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprMicroserviceClient"/>.
    /// </summary>
    /// <param name="client">Una instancia de <see cref="DaprClient"/> utilizada para interactuar con Dapr.</param>
    /// <param name="httpClient">Una instancia de <see cref="HttpClient"/> para realizar solicitudes HTTP.</param>
    /// <param name="options">Opciones de configuración de Dapr, encapsuladas en <see cref="IOptions{DaprOptions}"/>.</param>
    /// <param name="appId">El identificador de la aplicación Dapr.</param>
    /// <param name="loggerFactory">Una fábrica de registros para crear instancias de <see cref="ILogger"/>.</param>
    /// <param name="serviceProvider">Un proveedor de servicios para resolver dependencias.</param>
    /// <remarks>
    /// Este constructor configura los servicios necesarios para la integración con Dapr, incluyendo
    /// la invocación de servicios, la gestión de estados y la 
    public DaprMicroserviceClient( DaprClient client, HttpClient httpClient, IOptions<DaprOptions> options, string appId, ILoggerFactory loggerFactory, IServiceProvider serviceProvider ) {
        HttpClient = new HttpClientService().SetHttpClient( httpClient );
        ServiceInvocation = new DaprServiceInvocation( client, options, loggerFactory ).Service( appId );
        IntegrationEventBus = new DaprIntegrationEventBus( client, options, loggerFactory, serviceProvider );
        var keyGenerator = serviceProvider.GetService<IKeyGenerator>();
        StateManager = new DaprStateManager( client, options, loggerFactory, keyGenerator );
    }

    /// <summary>
    /// Obtiene la instancia de <see cref="IHttpClient"/> utilizada para realizar solicitudes HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un cliente HTTP que puede ser utilizado para 
    /// enviar solicitudes y recibir respuestas de un recurso identificado por una URI.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IHttpClient"/> que permite realizar operaciones de 
    /// comunicación HTTP.
    /// </value>
    public IHttpClient HttpClient { get; }

    /// <summary>
    /// Obtiene la instancia de <see cref="IServiceInvocation"/> utilizada para invocar servicios.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la implementación de la interfaz <see cref="IServiceInvocation"/> 
    /// que permite realizar llamadas a servicios de manera abstracta, facilitando la interacción con 
    /// diferentes servicios sin necesidad de conocer su implementación específica.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IServiceInvocation"/> que representa el servicio de invocación.
    /// </value>
    public IServiceInvocation ServiceInvocation { get; }

    /// <summary>
    /// Obtiene la instancia del bus de eventos de integración.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al bus de eventos que se utiliza para 
    public IIntegrationEventBus IntegrationEventBus { get; }

    /// <summary>
    /// Obtiene la instancia del administrador de estado.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IStateManager"/>.
    /// </value>
    public IStateManager StateManager { get; }
}