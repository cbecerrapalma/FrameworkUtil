namespace Util.Microservices.Polly;

/// <summary>
/// Clase que implementa un manejador de políticas de reintento utilizando Polly.
/// </summary>
/// <remarks>
/// Esta clase se encarga de definir y aplicar políticas de reintento para manejar fallos en las operaciones.
/// Utiliza la biblioteca Polly para facilitar la implementación de estas políticas.
/// </remarks>
public class PollyRetryPolicyHandler : IRetryPolicyHandler {
    private readonly int? _count;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PollyRetryPolicyHandler"/>.
    /// </summary>
    /// <param name="count">El número de reintentos a realizar. Si es nulo, se utilizará un valor predeterminado.</param>
    public PollyRetryPolicyHandler( int? count = null ) {
        _count = count;
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja el resultado de una acción y devuelve una política de reintento.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <param name="action">Una función que toma un resultado de tipo <typeparamref name="TResult"/> y devuelve un valor booleano que indica si se debe reintentar.</param>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento configurada.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza la biblioteca Polly para crear una política de reintento basada en el resultado de la acción proporcionada.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    public IRetryPolicy<TResult> HandleResult<TResult>( Func<TResult, bool> action ) {
        return new PollyRetryPolicy<TResult>( Policy.HandleResult( action ), _count );
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja excepciones de un tipo específico y devuelve una política de reintento.
    /// </summary>
    /// <typeparam name="TException">El tipo de excepción que se manejará. Debe ser una subclase de <see cref="System.Exception"/>.</typeparam>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy"/> que define la política de reintento para el tipo de excepción especificado.
    /// </returns>
    /// <remarks>
    /// Esta función utiliza la biblioteca Polly para crear una política de reintento que se activará cuando se produzca una excepción del tipo especificado.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    /// <seealso cref="PollyRetryPolicy"/>
    public IRetryPolicy HandleException<TException>() where TException : Exception {
        return new PollyRetryPolicy( Policy.Handle<TException>(), _count );
    }
}