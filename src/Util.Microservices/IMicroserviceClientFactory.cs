namespace Util.Microservices;

/// <summary>
/// Interfaz que define un contrato para la creación de clientes de microservicios.
/// </summary>
/// <remarks>
/// Esta interfaz es parte de la infraestructura de microservicios y permite la creación de instancias de clientes
/// que se comunican con otros microservicios. Implementaciones de esta interfaz deben ser registradas como
/// dependencias transitorias.
/// </remarks>
public interface IMicroserviceClientFactory : ITransientDependency {
    /// <summary>
    /// Crea una instancia de un cliente de microservicio.
    /// </summary>
    /// <returns>
    /// Una implementación de <see cref="IMicroserviceClient"/> que permite la interacción con el microservicio.
    /// </returns>
    /// <remarks>
    /// Este método es útil para inicializar un cliente que se puede utilizar para realizar llamadas a un microservicio específico.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la creación del cliente.
    /// </remarks>
    IMicroserviceClient Create();
}