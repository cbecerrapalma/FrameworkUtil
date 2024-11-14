namespace Util.Microservices; 

/// <summary>
/// Define una política de reintentos que se puede aplicar a operaciones que pueden fallar.
/// </summary>
public interface IRetryPolicy {
    /// <summary>
    /// Representa una política de reintento que intentará ejecutar una operación indefinidamente.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy"/> que implementa la política de reintentos indefinidos.
    /// </returns>
    /// <remarks>
    /// Esta política es útil en escenarios donde se desea que una operación continúe intentando ejecutarse hasta que tenga éxito,
    /// sin un límite predefinido de intentos. Sin embargo, se debe tener cuidado al usar esta política, ya que puede llevar a
    /// un bucle infinito si la operación nunca tiene éxito.
    /// </remarks>
    IRetryPolicy Forever();
    /// <summary>
    /// Obtiene una política de reintento que define el comportamiento de espera.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IRetryPolicy"/> que representa la política de reintento.
    /// </returns>
    /// <remarks>
    /// Esta función es útil para implementar lógica de reintento en operaciones que pueden fallar,
    /// permitiendo especificar cómo y cuándo se deben realizar los reintentos.
    /// </remarks>
    IRetryPolicy Wait();
    /// <summary>
    /// Proporciona una política de reintento que espera un período de tiempo determinado antes de volver a intentar una operación.
    /// </summary>
    /// <param name="sleepDurationProvider">Función que determina la duración de la espera antes de cada reintento, basada en el número de intentos realizados.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy"/> que implementa la lógica de reintento con espera.</returns>
    /// <remarks>
    /// Esta función permite personalizar el tiempo de espera entre intentos, lo que puede ser útil para manejar situaciones de sobrecarga o errores temporales.
    /// </remarks>
    IRetryPolicy Wait( Func<int, TimeSpan> sleepDurationProvider );
    /// <summary>
    /// Define un método que se invoca cuando se produce un intento fallido.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al producirse un error, que recibe como parámetros la excepción lanzada y el número de intento actual.</param>
    /// <returns>Una instancia de <see cref="IRetryPolicy"/> que representa la política de reintento configurada.</returns>
    /// <remarks>
    /// Este método permite personalizar el comportamiento en caso de errores durante la ejecución de operaciones que pueden requerir reintentos.
    /// </remarks>
    /// <seealso cref="IRetryPolicy"/>
    IRetryPolicy OnRetry( Action<Exception, int> action );
    /// <summary>
    /// Ejecuta la acción especificada.
    /// </summary>
    /// <param name="action">La acción que se debe ejecutar.</param>
    /// <remarks>
    /// Este método permite ejecutar una acción que no devuelve un valor. 
    /// Asegúrese de que la acción no sea nula antes de llamar a este método.
    /// </remarks>
    void Execute( Action action );
    /// <summary>
    /// Ejecuta de manera asíncrona una acción proporcionada.
    /// </summary>
    /// <param name="action">La acción asíncrona que se desea ejecutar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite ejecutar una función asíncrona y espera a que se complete.
    /// Es útil para encapsular la lógica de ejecución asíncrona en un solo lugar.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="action"/> es null.</exception>
    /// <seealso cref="Task"/>
    Task ExecuteAsync( Func<Task> action );
}