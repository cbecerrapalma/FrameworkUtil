namespace Util.Microservices.Dapr;

/// <summary>
/// Proporciona métodos de extensión para crear instancias de clientes de microservicios.
/// </summary>
public static class MicroserviceClientFactoryExtensions {
    /// <summary>
    /// Establece el identificador de la aplicación en la fábrica de clientes de microservicios.
    /// </summary>
    /// <param name="source">La instancia de la fábrica de clientes de microservicios.</param>
    /// <param name="appId">El identificador de la aplicación que se va a establecer.</param>
    /// <returns>La instancia original de la fábrica de clientes de microservicios.</returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar el identificador de la aplicación 
    /// solo si la fuente es una instancia de <see cref="DaprMicroserviceClientFactory"/>.
    /// </remarks>
    /// <seealso cref="DaprMicroserviceClientFactory"/>
    public static IMicroserviceClientFactory AppId( this IMicroserviceClientFactory source,string appId ) {
        if ( source is DaprMicroserviceClientFactory factory )
            factory.AppId( appId );
        return source;
    }

    /// <summary>
    /// Establece el puerto HTTP de Dapr en la fábrica de clientes de microservicios.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IMicroserviceClientFactory"/> que se está extendiendo.</param>
    /// <param name="daprHttpPort">El puerto HTTP que se desea establecer para Dapr.</param>
    /// <returns>
    /// La instancia original de <see cref="IMicroserviceClientFactory"/> con el puerto HTTP de Dapr configurado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite configurar el puerto HTTP de Dapr
    /// en una fábrica de clientes de microservicios que implementa <see cref="DaprMicroserviceClientFactory"/>.
    /// </remarks>
    /// <seealso cref="IMicroserviceClientFactory"/>
    /// <seealso cref="DaprMicroserviceClientFactory"/>
    public static IMicroserviceClientFactory DaprHttpPort( this IMicroserviceClientFactory source, int daprHttpPort ) {
        if ( source is DaprMicroserviceClientFactory factory )
            factory.DaprHttpPort( daprHttpPort );
        return source;
    }

    /// <summary>
    /// Establece el puerto gRPC de Dapr en la fábrica de clientes de microservicios.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IMicroserviceClientFactory"/> que se va a modificar.</param>
    /// <param name="daprGrpcPort">El puerto gRPC que se desea establecer para Dapr.</param>
    /// <returns>La instancia modificada de <see cref="IMicroserviceClientFactory"/>.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IMicroserviceClientFactory"/> 
    /// permitiendo configurar el puerto gRPC de Dapr si la instancia es de tipo <see cref="DaprMicroserviceClientFactory"/>.
    /// </remarks>
    /// <seealso cref="IMicroserviceClientFactory"/>
    /// <seealso cref="DaprMicroserviceClientFactory"/>
    public static IMicroserviceClientFactory DaprGrpcPort( this IMicroserviceClientFactory source, int daprGrpcPort ) {
        if ( source is DaprMicroserviceClientFactory factory )
            factory.DaprGrpcPort( daprGrpcPort );
        return source;
    }
}