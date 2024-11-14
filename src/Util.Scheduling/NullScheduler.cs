namespace Util.Scheduling; 

/// <summary>
/// Representa un programador nulo que no realiza ninguna acción.
/// Esta clase implementa la interfaz <see cref="IScheduler"/>.
/// </summary>
public class NullScheduler : IScheduler {
    public static readonly IScheduler Instance = new NullScheduler();

    /// <summary>
    /// Inicia la tarea de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que se completa inmediatamente.</returns>
    public Task StartAsync( CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Pausa la ejecución de la tarea de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación de pausa completada.
    /// </returns>
    public Task PauseAsync() {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Reanuda la tarea de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. 
    /// Esta tarea se completa inmediatamente.
    /// </returns>
    public Task ResumeAsync() {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Detiene el proceso asíncrono.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de detención. 
    /// La tarea se completa inmediatamente ya que no se realiza ninguna operación adicional.
    /// </returns>
    public Task StopAsync( CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }
}