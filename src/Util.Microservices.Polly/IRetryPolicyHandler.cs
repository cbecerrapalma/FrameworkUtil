namespace Util.Microservices;

/// <summary>
/// Define un contrato para manejar políticas de reintento.
/// </summary>
public interface IRetryPolicyHandler {
    /// <summary>
    /// Maneja el resultado de una acción y permite aplicar una política de reintento.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <param name="action">Una función que toma un resultado de tipo <typeparamref name="TResult"/> y devuelve un valor booleano que indica si la acción fue exitosa.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento aplicada al resultado de la acción.</returns>
    /// <remarks>
    /// Este método es útil para manejar resultados que pueden requerir reintentos en caso de fallos.
    /// Se puede utilizar en escenarios donde se necesite validar el resultado de una operación y decidir si se debe reintentar o no.
    /// </remarks>
    IRetryPolicy<TResult> HandleResult<TResult>( Func<TResult, bool> action );
    /// <summary>
    /// Define un método para manejar excepciones de un tipo específico.
    /// </summary>
    /// <typeparam name="TException">El tipo de excepción que se va a manejar. Debe ser una subclase de <see cref="Exception"/>.</typeparam>
    /// <returns>Una instancia de <see cref="IRetryPolicy"/> que define la política de reintento para la excepción manejada.</returns>
    /// <remarks>
    /// Este método permite implementar una lógica personalizada para manejar excepciones específicas,
    /// facilitando la creación de políticas de reintento que se adapten a diferentes tipos de errores.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    IRetryPolicy HandleException<TException>() where TException : Exception;
}