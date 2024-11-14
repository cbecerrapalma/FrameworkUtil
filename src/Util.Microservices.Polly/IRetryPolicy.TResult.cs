namespace Util.Microservices; 

/// <summary>
/// Define un contrato para una política de reintentos que puede ser aplicada a operaciones que retornan un resultado de tipo <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TResult">El tipo de resultado que retorna la operación que se está intentando ejecutar.</typeparam>
public interface IRetryPolicy<TResult> {
    /// <summary>
    /// Crea una política de reintento que intentará indefinidamente ejecutar una operación.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la operación que se está intentando ejecutar.</typeparam>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento indefinido.</returns>
    /// <remarks>
    /// Esta política es útil en situaciones donde se desea seguir intentando una operación hasta que tenga éxito,
    /// sin un límite en la cantidad de intentos. Sin embargo, se debe tener cuidado al usar esta política,
    /// ya que puede llevar a un bucle infinito si la operación nunca tiene éxito.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    IRetryPolicy<TResult> Forever();
    /// <summary>
    /// Obtiene una política de reintento que espera un período de tiempo antes de realizar un nuevo intento.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento.
    /// </returns>
    /// <remarks>
    /// Esta función es útil en escenarios donde se desea implementar un mecanismo de reintento con un retraso
    /// entre cada intento, lo que puede ayudar a manejar situaciones temporales como fallos de red o
    /// servicios no disponibles.
    /// </remarks>
    IRetryPolicy<TResult> Wait();
    /// <summary>
    /// Proporciona un método para definir una política de reintento que espera un período de tiempo determinado 
    /// antes de cada nuevo intento, utilizando una función que determina la duración de la espera.
    /// </summary>
    /// <param name="sleepDurationProvider">Una función que recibe el número de intento actual y devuelve 
    /// un <see cref="TimeSpan"/> que representa la duración de la espera antes del siguiente intento.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que implementa la política de reintento.</returns>
    /// <remarks>
    /// Este método permite personalizar la duración de la espera entre reintentos, lo que puede ser útil 
    /// para manejar situaciones en las que se requiere un tiempo de espera variable basado en el número 
    /// de intentos realizados.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    IRetryPolicy<TResult> Wait( Func<int, TimeSpan> sleepDurationProvider );
    /// <summary>
    /// Define una acción que se ejecuta cada vez que se produce un intento de reintento.
    /// </summary>
    /// <param name="action">La acción que se ejecutará en cada reintento, que recibe el resultado del intento anterior y el número de reintentos realizados.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintentos configurada.</returns>
    /// <remarks>
    /// Esta función permite personalizar el comportamiento durante los reintentos, como registrar información o realizar acciones específicas.
    /// </remarks>
    IRetryPolicy<TResult> OnRetry( Action<DelegateResult<TResult>, int> action );
    /// <summary>
    /// Ejecuta una acción y devuelve un resultado de tipo <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="action">La acción que se va a ejecutar, la cual debe devolver un resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>El resultado de la acción ejecutada.</returns>
    /// <typeparam name="TResult">El tipo del resultado que devuelve la acción.</typeparam>
    /// <remarks>
    /// Este método es útil para encapsular la ejecución de una acción que puede devolver un resultado,
    /// permitiendo manejar la lógica de ejecución de manera más estructurada.
    /// </remarks>
    /// <seealso cref="Func{TResult}"/>
    TResult Execute( Func<TResult> action );
    /// <summary>
    /// Ejecuta de manera asíncrona una acción que devuelve un resultado de tipo <typeparamref name="TResult"/>.
    /// </summary>
    /// <param name="action">Una función que representa la acción asíncrona a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el resultado de tipo <typeparamref name="TResult"/>.</returns>
    /// <remarks>
    /// Este método permite ejecutar una acción asíncrona y manejar su resultado de manera eficiente.
    /// Asegúrese de que la función proporcionada sea adecuada para su uso en un contexto asíncrono.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción asíncrona.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<TResult> ExecuteAsync( Func<Task<TResult>> action );
}