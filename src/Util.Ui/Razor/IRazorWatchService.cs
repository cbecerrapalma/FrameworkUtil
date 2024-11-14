namespace Util.Ui.Razor;

/// <summary>
/// Define un servicio para observar cambios en archivos Razor.
/// </summary>
public interface IRazorWatchService {
    /// <summary>
    /// Inicia de manera asíncrona una tarea que puede ser cancelada.
    /// </summary>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto <see cref="Task"/> que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite iniciar una tarea que puede ser interrumpida mediante el uso del token de cancelación proporcionado.
    /// Asegúrese de manejar adecuadamente la cancelación en el código que llama a este método.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task StartAsync( CancellationToken cancellationToken );
    /// <summary>
    /// Detiene de manera asíncrona la tarea actual.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de detención.</returns>
    /// <remarks>
    /// Este método debe ser implementado para liberar recursos y realizar cualquier limpieza necesaria antes de que la tarea se detenga.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si la operación es cancelada a través del <paramref name="cancellationToken"/>.
    /// </exception>
    Task StopAsync( CancellationToken cancellationToken );
}