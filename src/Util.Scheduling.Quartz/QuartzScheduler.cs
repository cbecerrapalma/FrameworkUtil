namespace Util.Scheduling; 

/// <summary>
/// Representa un programador que utiliza Quartz para gestionar y ejecutar trabajos programados.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IScheduler"/> y proporciona métodos para 
/// programar, pausar, reanudar y eliminar trabajos.
/// </remarks>
public class QuartzScheduler : IScheduler {
    private readonly Quartz.IScheduler _scheduler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QuartzScheduler"/>.
    /// </summary>
    /// <param name="scheduler">
    /// Una instancia de <see cref="Quartz.IScheduler"/> que se utilizará para programar tareas.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si el parámetro <paramref name="scheduler"/> es <c>null</c>.
    /// </exception>
    public QuartzScheduler( Quartz.IScheduler scheduler ) {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia el programador de tareas de forma asíncrona si aún no ha sido iniciado.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de inicio del programador.</returns>
    /// <remarks>
    /// Este método verifica si el programador ya ha sido iniciado antes de intentar iniciarlo.
    /// Si el programador ya está en ejecución, el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="StopAsync"/>
    public async Task StartAsync( CancellationToken cancellationToken = default ) {
        if ( _scheduler.IsStarted )
            return;
        await _scheduler.Start( cancellationToken );
    }

    /// <summary>
    /// Pausa todas las tareas programadas en el planificador.
    /// </summary>
    /// <remarks>
    /// Este método es asíncrono y permite pausar la ejecución de todas las tareas que están actualmente programadas.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de pausa. 
    /// El resultado de la tarea no tiene valor, ya que es un método de tipo <see cref="Task"/>.
    /// </returns>
    public async Task PauseAsync() {
        await _scheduler.PauseAll();
    }

    /// <summary>
    /// Reanuda todas las tareas programadas en el planificador.
    /// </summary>
    /// <remarks>
    /// Este método llama al método <see cref="_scheduler.ResumeAll"/> para reanudar todas las tareas que han sido pausadas.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de reanudar las tareas. El valor devuelto es un <see cref="Task"/> que se completa cuando todas las tareas han sido reanudadas.
    /// </returns>
    public async Task ResumeAsync() {
        await _scheduler.ResumeAll();
    }

    /// <inheritdoc />
    /// <summary>
    /// Detiene el programador de tareas de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de detención del programador.</returns>
    /// <remarks>
    /// Este método verifica si el programador ya está apagado antes de intentar detenerlo.
    /// Si el programador está en estado de apagado, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="StartAsync(CancellationToken)"/>
    public async Task StopAsync( CancellationToken cancellationToken = default ) {
        if ( _scheduler.IsShutdown )
            return;
        await _scheduler.Shutdown( true, cancellationToken );
    }
}