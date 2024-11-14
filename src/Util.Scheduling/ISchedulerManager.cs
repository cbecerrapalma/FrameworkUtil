namespace Util.Scheduling; 

/// <summary>
/// Interfaz que define las operaciones para la gestión de programadores.
/// </summary>
public interface ISchedulerManager {
    /// <summary>
    /// Agrega un nuevo trabajo de tipo <typeparamref name="TJob"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TJob">El tipo del trabajo que se va a agregar. Debe implementar la interfaz <see cref="IJob"/> y tener un constructor público sin parámetros.</typeparam>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que representa el identificador del trabajo agregado.</returns>
    /// <remarks>
    /// Este método crea una instancia del trabajo especificado y lo agrega a la cola de trabajos para su procesamiento.
    /// Asegúrese de que el tipo de trabajo cumpla con los requisitos de la interfaz <see cref="IJob"/>.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Se lanza si el tipo de trabajo no cumple con los requisitos especificados.</exception>
    Task<string> AddJobAsync<TJob>() where TJob : IJob, new();
    /// <summary>
    /// Agrega un nuevo trabajo de forma asíncrona.
    /// </summary>
    /// <param name="job">El trabajo que se desea agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el identificador del trabajo agregado.</returns>
    /// <remarks>
    /// Este método permite agregar un trabajo a la cola de trabajos para su procesamiento. 
    /// Asegúrese de que el trabajo proporcionado cumpla con los requisitos necesarios 
    /// antes de llamarlo.
    /// </remarks>
    /// <seealso cref="IJob"/>
    Task<string> AddJobAsync( IJob job );
    /// <summary>
    /// Inicia un escaneo de trabajos de manera asíncrona.
    /// </summary>
    /// <param name="isScanAllJobs">Indica si se deben escanear todos los trabajos. El valor por defecto es <c>true</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona de escaneo de trabajos.</returns>
    /// <remarks>
    /// Este método permite realizar un escaneo de trabajos en función del parámetro proporcionado.
    /// Si <paramref name="isScanAllJobs"/> es <c>true</c>, se escanearán todos los trabajos disponibles.
    /// De lo contrario, se escanearán solo los trabajos que cumplen con ciertos criterios.
    /// </remarks>
    /// <seealso cref="Job"/>
    /// <seealso cref="ScanJobResult"/>
    Task ScanJobsAsync( bool isScanAllJobs = true );
    /// <summary>
    /// Obtiene una instancia del programador de tareas (scheduler) de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene el programador de tareas solicitado.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder a un programador de tareas que puede ser utilizado para programar y ejecutar tareas de manera eficiente.
    /// </remarks>
    /// <seealso cref="IScheduler"/>
    Task<IScheduler> GetSchedulerAsync();
}