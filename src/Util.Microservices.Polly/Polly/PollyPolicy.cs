using Util.Dependency;

namespace Util.Microservices.Polly;

/// <summary>
/// Representa una clase que utiliza Inversión de Control (IoC) para gestionar la creación y el ciclo de vida de los objetos.
/// </summary>
/// <remarks>
/// Esta clase permite la inyección de dependencias, facilitando la gestión de las instancias de los objetos
/// y promoviendo un diseño más limpio y mantenible.
/// </remarks>
/// <param name="id">El identificador único de la instancia que se va a gestionar.</param>
/// <seealso cref="OtroComponente"/>
[Ioc(1)]
public class PollyPolicy : IPolicy {
    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un manejador de políticas de reintento.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="IRetryPolicyHandler"/> que maneja las políticas de reintento.
    /// </returns>
    /// <seealso cref="IRetryPolicyHandler"/>
    /// <remarks>
    /// Este método utiliza la biblioteca Polly para implementar la lógica de reintento.
    /// </remarks>
    public IRetryPolicyHandler Retry() {
        return new PollyRetryPolicyHandler();
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un manejador de políticas de reintento.
    /// </summary>
    /// <param name="count">El número de reintentos que se realizarán en caso de fallo.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicyHandler"/> que implementa la política de reintento.</returns>
    /// <seealso cref="IRetryPolicyHandler"/>
    /// <remarks>
    /// Este método utiliza la biblioteca Polly para gestionar las políticas de reintento.
    /// </remarks>
    public IRetryPolicyHandler Retry( int count ) {
        return new PollyRetryPolicyHandler( count );
    }
}