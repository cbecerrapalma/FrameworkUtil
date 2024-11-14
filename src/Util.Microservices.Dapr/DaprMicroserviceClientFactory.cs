namespace Util.Microservices.Dapr; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IMicroserviceClientFactory"/> 
/// para crear instancias de clientes de microservicios utilizando Dapr.
/// </summary>
public class DaprMicroserviceClientFactory : IMicroserviceClientFactory {
    private readonly DaprClient _client;
    private string _appId;
    private int _daprHttpPort;
    private int _daprGrpcPort;
    private readonly IOptions<DaprOptions> _options;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprMicroserviceClientFactory"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr que se utilizará para las operaciones de microservicio.</param>
    /// <param name="options">Las opciones de configuración para Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de <see cref="ILogger"/>.</param>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    public DaprMicroserviceClientFactory( DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory, IServiceProvider serviceProvider ) {
        _client = client;
        _options = options;
        _loggerFactory = loggerFactory;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Establece el identificador de la aplicación.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que se va a establecer.</param>
    public void AppId( string appId ) {
        _appId = appId;
    }

    /// <summary>
    /// Establece el puerto HTTP de Dapr.
    /// </summary>
    /// <param name="daprHttpPort">El puerto HTTP que se desea establecer para Dapr.</param>
    public void DaprHttpPort( int daprHttpPort ) {
        _daprHttpPort = daprHttpPort;
    }

    /// <summary>
    /// Establece el puerto gRPC de Dapr.
    /// </summary>
    /// <param name="daprGrpcPort">El número de puerto gRPC que se asignará a Dapr.</param>
    public void DaprGrpcPort( int daprGrpcPort ) {
        _daprGrpcPort = daprGrpcPort;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un cliente de microservicio.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IMicroserviceClient"/>.
    /// </returns>
    /// <remarks>
    /// Este método inicializa un cliente Dapr y un cliente HTTP, y luego crea una instancia de 
    /// <see cref="DaprMicroserviceClient"/> utilizando estos clientes junto con las opciones, 
    /// el identificador de la aplicación, el fabricante de registros y el proveedor de servicios 
    /// especificados.
    /// </remarks>
    public virtual IMicroserviceClient Create() {
        var client = CreateDaprClient();
        var httpClient = CreateHttpClient();
        return new DaprMicroserviceClient( client, httpClient, _options, _appId, _loggerFactory, _serviceProvider );
    }

    /// <summary>
    /// Crea una instancia de <see cref="DaprClient"/> utilizando los puntos finales HTTP y gRPC configurados.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="DaprClient"/>. Si ambos puntos finales están vacíos y el cliente ya está creado, se devuelve el cliente existente.
    /// </returns>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobrescriban para proporcionar una implementación personalizada.
    /// </remarks>
    /// <seealso cref="DaprClient"/>
    protected virtual DaprClient CreateDaprClient() {
        var httpEndpoint = GetDaprHttpEndpoint();
        var grpcEndpoint = GetDaprGrpcEndpoint();
        if( httpEndpoint.IsEmpty() && grpcEndpoint.IsEmpty() && _client != null )
            return _client;
        var builder = new DaprClientBuilder();
        if ( httpEndpoint.IsEmpty() == false ) 
            builder.UseHttpEndpoint( httpEndpoint );
        if ( grpcEndpoint.IsEmpty() == false ) 
            builder.UseGrpcEndpoint( grpcEndpoint );
        return builder.Build();
    }
    /// <summary>
    /// Obtiene el endpoint HTTP de Dapr.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el puerto HTTP de Dapr ha sido configurado. 
    /// Si no se ha configurado, intenta obtenerlo de las variables de entorno. 
    /// Si el puerto es válido, devuelve la URL del endpoint; de lo contrario, devuelve null.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la URL del endpoint HTTP de Dapr, 
    /// o null si el puerto no está configurado correctamente.
    /// </returns>
    protected virtual string GetDaprHttpEndpoint() 
    { 
        if (_daprHttpPort == 0) 
            _daprHttpPort = Util.Helpers.Environment.GetEnvironmentVariable<int>("DAPR_HTTP_ENDPOINT"); 
        return _daprHttpPort > 0 ? $"http://localhost:{_daprHttpPort}" : null; 
    }

    /// <summary>
    /// Obtiene el endpoint gRPC de Dapr.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el puerto gRPC de Dapr ha sido configurado. Si no se ha configurado, 
    /// intenta obtenerlo de las variables de entorno. Si el puerto es válido, se devuelve la URL 
    /// del endpoint; de lo contrario, se devuelve null.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la URL del endpoint gRPC de Dapr, o null si el puerto no es válido.
    /// </returns>
    protected virtual string GetDaprGrpcEndpoint() {
        if (_daprGrpcPort == 0)
            _daprGrpcPort = Util.Helpers.Environment.GetEnvironmentVariable<int>("DAPR_GRPC_ENDPOINT");
        return _daprGrpcPort > 0 ? $"http://localhost:{_daprGrpcPort}" : null;
    }

    /// <summary>
    /// Crea una instancia de <see cref="HttpClient"/> para invocar servicios Dapr.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpClient"/> configurado para invocar servicios Dapr.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el identificador de la aplicación (_appId) está vacío.
    /// Si está vacío, se crea un cliente HTTP predeterminado. Si no, se obtiene el 
    /// punto final HTTP de Dapr y se utiliza para crear el cliente HTTP con el 
    /// identificador de la aplicación y el punto final correspondiente.
    /// </remarks>
    /// <seealso cref="DaprClient"/>
    protected virtual HttpClient CreateHttpClient() {
        if ( _appId.IsEmpty() )
            return DaprClient.CreateInvokeHttpClient();
        var endpoint = GetDaprHttpEndpoint();
        return endpoint.IsEmpty() ? DaprClient.CreateInvokeHttpClient( _appId ) : DaprClient.CreateInvokeHttpClient( _appId, endpoint );
    }
}