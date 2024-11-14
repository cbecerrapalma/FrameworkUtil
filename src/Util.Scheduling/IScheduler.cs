namespace Util.Scheduling; 

/// <summary>
/// Define una interfaz para un programador que gestiona tareas o eventos.
/// </summary>
public interface IScheduler {
    /// <summary>
    /// Inicia de manera asíncrona una tarea.
    /// </summary>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite iniciar una tarea que puede ser cancelada mediante el token proporcionado.
    /// Si la tarea se cancela, se lanzará una excepción <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task StartAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Detiene de manera asíncrona la operación actual.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de detención.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado para liberar recursos y detener cualquier proceso en ejecución
    /// de manera ordenada. Si se recibe un token de cancelación, se debe hacer un esfuerzo por
    /// detener la operación lo más pronto posible.
    /// </remarks>
    /// <seealso cref="StartAsync(CancellationToken)"/>
    Task StopAsync( CancellationToken cancellationToken = default );
}