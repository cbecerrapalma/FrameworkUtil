namespace Util.Scheduling; 

/// <summary>
/// Clase que representa un servicio hospedado que se ejecuta en segundo plano.
/// Implementa la interfaz <see cref="IHostedService"/>.
/// </summary>
public class HostService : IHostedService {
    private readonly ISchedulerManager _manager;
    private readonly SchedulerOptions _options;
    private IScheduler _scheduler;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HostService"/>.
    /// </summary>
    /// <param name="manager">La instancia de <see cref="ISchedulerManager"/> que se utilizará para gestionar los programadores.</param>
    /// <param name="options">Las opciones de configuración para el programador, encapsuladas en <see cref="IOptions{SchedulerOptions}"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="manager"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Si <paramref name="options"/> es <c>null</c>, se inicializa con una nueva instancia de <see cref="SchedulerOptions"/>.
    /// </remarks>
    public HostService( ISchedulerManager manager, IOptions<SchedulerOptions> options ) {
        _manager = manager ?? throw new ArgumentNullException( nameof( manager ) );
        _options = options?.Value ?? new SchedulerOptions();
        _scheduler = NullScheduler.Instance;
    }

    /// <summary>
    /// Inicia el proceso de escaneo de trabajos y arranca el programador.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método primero escanea los trabajos según la configuración especificada en las opciones.
    /// Luego, obtiene una instancia del programador y lo inicia.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    /// <seealso cref="ScanJobsAsync(bool)"/>
    /// <seealso cref="GetSchedulerAsync"/>
    /// <seealso cref="StartAsync(CancellationToken)"/>
    public async Task StartAsync( CancellationToken cancellationToken ) {
        await _manager.ScanJobsAsync( _options.IsScanJobs );
        _scheduler = await _manager.GetSchedulerAsync();
        await _scheduler.StartAsync( cancellationToken );
    }

    /// <summary>
    /// Detiene de manera asíncrona el programador.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de detención del programador.
    /// </returns>
    /// <remarks>
    /// Este método llama al método <see cref="_scheduler.StopAsync(CancellationToken)"/> 
    /// para realizar la detención del programador.
    /// </remarks>
    public async Task StopAsync( CancellationToken cancellationToken ) {
        await _scheduler.StopAsync(cancellationToken);
    }
}