namespace Util.Microservices.Polly;

/// <summary>
/// Representa una política de reintento vacía que no realiza ningún reintento.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IRetryPolicy"/> y no lleva a cabo ninguna acción de reintento,
/// lo que significa que cualquier operación que utilice esta política se ejecutará una sola vez sin reintentos.
/// </remarks>
public class EmptyRetryPolicy : IRetryPolicy {
    public static readonly IRetryPolicy Instance = new EmptyRetryPolicy();

    /// <inheritdoc />
    /// <summary>
    /// Devuelve una instancia de la política de reintento que se ejecutará indefinidamente.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy"/> que representa la política de reintento sin límite.
    /// </returns>
    /// <remarks>
    /// Esta política no tiene un límite en el número de reintentos, lo que significa que intentará
    /// ejecutar la operación de forma continua hasta que tenga éxito o se detenga manualmente.
    /// </remarks>
    public IRetryPolicy Forever() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Devuelve la instancia actual de la política de reintento.
    /// </summary>
    /// <returns>
    /// La instancia de <see cref="IRetryPolicy"/> que representa la política de reintento actual.
    /// </returns>
    /// <remarks>
    /// Este método permite que el llamador espere hasta que se complete la operación de reintento.
    /// </remarks>
    public IRetryPolicy Wait() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Proporciona un mecanismo para esperar un período de tiempo determinado 
    /// basado en una función que define la duración de la espera.
    /// </summary>
    /// <param name="sleepDurationProvider">
    /// Una función que toma un entero (número de intentos) y devuelve 
    /// un <see cref="TimeSpan"/> que representa la duración de la espera 
    /// antes de intentar nuevamente.
    /// </param>
    /// <returns>
    /// Un objeto que implementa <see cref="IRetryPolicy"/> que permite 
    /// realizar reintentos según la política definida.
    /// </returns>
    /// <remarks>
    /// Este método permite personalizar la duración de la espera entre 
    /// intentos, lo que puede ser útil en escenarios donde se desea 
    /// implementar una lógica de reintentos con tiempos de espera 
    /// variables.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    public IRetryPolicy Wait( Func<int, TimeSpan> sleepDurationProvider ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Se invoca cuando se produce un intento de reintento debido a un fallo.
    /// </summary>
    /// <param name="action">La acción que se ejecutará en cada intento fallido, que recibe como parámetros la excepción que ocurrió y el número de intento actual.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IRetryPolicy"/>.</returns>
    /// <remarks>
    /// Esta implementación permite definir un comportamiento personalizado para manejar los errores que ocurren durante la ejecución de una operación.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    public IRetryPolicy OnRetry( Action<Exception, int> action ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción proporcionada si no es nula.
    /// </summary>
    /// <param name="action">La acción que se desea ejecutar.</param>
    /// <remarks>
    /// Este método verifica si la acción es nula antes de invocarla para evitar excepciones.
    /// </remarks>
    public void Execute( Action action ) {
        action?.Invoke();
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción asincrónica proporcionada como un delegado.
    /// </summary>
    /// <param name="action">La acción asincrónica que se va a ejecutar.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="action"/> es nulo, no se realizará ninguna acción.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    public async Task ExecuteAsync( Func<Task> action ) {
        if ( action == null )
            return;
        await action();
    }
}

/// <summary>
/// Representa una política de reintento vacía que no realiza ningún intento de reintento.
/// </summary>
/// <typeparam name="TResult">El tipo de resultado que se espera de la operación.</typeparam>
public class EmptyRetryPolicy<TResult> : IRetryPolicy<TResult> {
    public static readonly IRetryPolicy<TResult> Instance = new EmptyRetryPolicy<TResult>();

    /// <inheritdoc />
    /// <summary>
    /// Devuelve una política de reintento que intentará indefinidamente.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy{TResult}"/> que representa la política de reintento indefinido.
    /// </returns>
    /// <remarks>
    /// Esta política es útil en situaciones donde se desea seguir intentando una operación hasta que tenga éxito,
    /// sin un límite en la cantidad de intentos.
    /// </remarks>
    public IRetryPolicy<TResult> Forever() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Espera y devuelve la política de reintento actual.
    /// </summary>
    /// <returns>
    /// La política de reintento actual.
    /// </returns>
    /// <remarks>
    /// Este método permite que la ejecución se detenga temporalmente antes de intentar una nueva operación,
    /// siguiendo la lógica definida en la política de reintento.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    public IRetryPolicy<TResult> Wait() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Proporciona un mecanismo para esperar un período de tiempo determinado antes de reintentar una operación.
    /// </summary>
    /// <param name="sleepDurationProvider">Una función que toma un entero (número de reintentos) y devuelve un <see cref="TimeSpan"/> que representa la duración de la espera.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IRetryPolicy{TResult}"/> que permite aplicar la política de reintento.
    /// </returns>
    /// <remarks>
    /// Este método es útil para definir la lógica de espera entre reintentos en caso de que una operación falle.
    /// </remarks>
    /// <seealso cref="IRetryPolicy{TResult}"/>
    public IRetryPolicy<TResult> Wait( Func<int, TimeSpan> sleepDurationProvider ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cada vez que se produzca un reintento.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe el resultado del intento anterior y el número de reintento.</param>
    /// <returns>El objeto actual de política de reintentos.</returns>
    /// <remarks>
    /// Esta función permite personalizar el comportamiento en caso de reintentos, 
    /// lo que puede ser útil para registrar información o realizar acciones específicas 
    /// en función del resultado de los intentos anteriores.
    /// </remarks>
    public IRetryPolicy<TResult> OnRetry( Action<DelegateResult<TResult>, int> action ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción proporcionada y devuelve el resultado.
    /// </summary>
    /// <param name="action">La función que se va a ejecutar. Puede ser nula.</param>
    /// <returns>El resultado de la acción ejecutada, o el valor predeterminado de <typeparamref name="TResult"/> si la acción es nula.</returns>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <remarks>
    /// Este método permite ejecutar una función de forma segura, manejando el caso en que la función proporcionada sea nula.
    /// </remarks>
    /// <seealso cref="Func{TResult}"/>
    public TResult Execute( Func<TResult> action ) {
        return action == null ? default : action();
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción asincrónica y devuelve el resultado.
    /// </summary>
    /// <param name="action">La acción asincrónica a ejecutar, que devuelve un resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>El resultado de la acción asincrónica, o el valor predeterminado de <typeparamref name="TResult"/> si la acción es nula.</returns>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la acción.</typeparam>
    /// <remarks>
    /// Este método permite ejecutar una función asincrónica que puede ser nula. Si la acción es nula, se devolverá el valor predeterminado del tipo de resultado.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    public async Task<TResult> ExecuteAsync( Func<Task<TResult>> action ) {
        return action == null ? default : await action();
    }
}