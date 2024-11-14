namespace Util.Microservices.Polly;

/// <summary>
/// Representa una política vacía que implementa la interfaz <see cref="IPolicy"/>.
/// Esta clase puede ser utilizada como un marcador o para representar la ausencia de una política específica.
/// </summary>
public class EmptyPolicy : IPolicy {
    public static readonly IPolicy Instance = new EmptyPolicy();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una instancia de un manejador de políticas de reintento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicyHandler"/> que representa una política de reintento vacía.
    /// </returns>
    /// <remarks>
    /// Este método proporciona un manejador de políticas de reintento que no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="IRetryPolicyHandler"/>
    public IRetryPolicyHandler Retry() {
        return EmptyRetryPolicyHandler.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Implementa un mecanismo de reintento.
    /// </summary>
    /// <param name="count">El número de intentos que se realizarán.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicyHandler"/> que representa una política de reintento vacía.</returns>
    /// <remarks>
    /// Esta implementación siempre devuelve una política de reintento vacía, lo que significa que no se realizarán reintentos.
    /// </remarks>
    /// <seealso cref="IRetryPolicyHandler"/>
    public IRetryPolicyHandler Retry( int count ) {
        return EmptyRetryPolicyHandler.Instance;
    }
}