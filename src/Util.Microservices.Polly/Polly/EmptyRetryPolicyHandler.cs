namespace Util.Microservices.Polly;

/// <summary>
/// Clase que implementa un manejador de políticas de reintento vacío.
/// </summary>
/// <remarks>
/// Esta clase no realiza ninguna acción de reintento y se utiliza como un marcador de posición
/// en situaciones donde no se desea aplicar ninguna política de reintento.
/// </remarks>
public class EmptyRetryPolicyHandler : IRetryPolicyHandler {
    public static readonly IRetryPolicyHandler Instance = new EmptyRetryPolicyHandler();

    /// <inheritdoc />
    /// <summary>
    /// Maneja el resultado de una acción y devuelve una política de reintento vacía.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <param name="action">Una función que toma un resultado de tipo <typeparamref name="TResult"/> y devuelve un valor booleano.</param>
    /// <returns>
    /// Una instancia de <see cref="EmptyRetryPolicy{TResult}"/> que representa una política de reintento vacía.
    /// </returns>
    /// <remarks>
    /// Esta implementación no realiza ningún reintento y simplemente devuelve una política vacía.
    /// </remarks>
    public IRetryPolicy<TResult> HandleResult<TResult>( Func<TResult, bool> action ) {
        return EmptyRetryPolicy<TResult>.Instance;
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja excepciones del tipo especificado y devuelve una política de reintento.
    /// </summary>
    /// <typeparam name="TException">El tipo de excepción que se va a manejar. Debe ser una subclase de <see cref="System.Exception"/>.</typeparam>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy"/> que representa la política de reintento a aplicar.
    /// En este caso, se devuelve una instancia de <see cref="EmptyRetryPolicy"/> que no realiza reintentos.
    /// </returns>
    /// <remarks>
    /// Este método es útil para definir comportamientos específicos al manejar excepciones en operaciones que pueden requerir reintentos.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    /// <seealso cref="EmptyRetryPolicy"/>
    public IRetryPolicy HandleException<TException>() where TException : Exception {
        return EmptyRetryPolicy.Instance;
    }
}