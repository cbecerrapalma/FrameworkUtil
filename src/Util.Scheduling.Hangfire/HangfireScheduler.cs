namespace Util.Scheduling; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IScheduler"/> para programar tareas utilizando Hangfire.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar la programación y ejecución de trabajos en segundo plano 
/// utilizando la biblioteca Hangfire. Permite definir trabajos recurrentes y únicos, así como 
/// manejar la configuración de la cola de trabajos.
/// </remarks>
public class HangfireScheduler : IScheduler {
    private BackgroundJobServer _jobServer;
    private readonly BackgroundJobServerOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HangfireScheduler"/>.
    /// </summary>
    /// <param name="options">Las opciones de configuración para el servidor de trabajos en segundo plano.</param>
    public HangfireScheduler( BackgroundJobServerOptions options ) {
        _options = options;
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia el servidor de trabajos en segundo plano.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica de inicio del servidor de trabajos.</returns>
    /// <remarks>
    /// Este método configura y arranca el servidor de trabajos en segundo plano utilizando las opciones especificadas.
    /// </remarks>
    /// <seealso cref="BackgroundJobServer"/>
    public virtual Task StartAsync( CancellationToken cancellationToken = default ) {
        _jobServer = new BackgroundJobServer( _options );
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Detiene de manera asíncrona el servidor de trabajos.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token que puede ser utilizado para cancelar la operación de detención.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de detención.
    /// </returns>
    /// <remarks>
    /// Este método libera los recursos utilizados por el servidor de trabajos y asegura que
    /// se complete la detención de manera adecuada. Si se recibe una solicitud de cancelación
    /// a través del <paramref name="cancellationToken"/>, se debe manejar adecuadamente.
    /// </remarks>
    public virtual Task StopAsync( CancellationToken cancellationToken = default ) {
        _jobServer.Dispose();
        return Task.CompletedTask;
    }
}