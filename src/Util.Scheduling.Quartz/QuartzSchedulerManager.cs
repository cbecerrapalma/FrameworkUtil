using Util.Reflections;

namespace Util.Scheduling; 

/// <summary>
/// Clase que gestiona la programación de tareas utilizando Quartz.
/// Hereda de <see cref="SchedulerManagerBase"/>.
/// </summary>
public class QuartzSchedulerManager : SchedulerManagerBase {
    private readonly ISchedulerFactory _factory;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QuartzSchedulerManager"/>.
    /// </summary>
    /// <param name="scopeFactory">La fábrica de ámbito utilizada para crear instancias de servicio.</param>
    /// <param name="typeFinder">El buscador de tipos utilizado para localizar tipos en la aplicación.</param>
    /// <param name="factory">La fábrica de programadores de Quartz que se utilizará para crear instancias de programadores.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="factory"/> es <c>null</c>.</exception>
    public QuartzSchedulerManager( IServiceScopeFactory scopeFactory, ITypeFinder typeFinder, ISchedulerFactory factory )
        : base( scopeFactory, typeFinder ) {
        _factory = factory ?? throw new ArgumentNullException( nameof( factory ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega un trabajo al programador de tareas de forma asíncrona.
    /// </summary>
    /// <param name="job">El trabajo que se desea agregar al programador. No puede ser nulo.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <see langword="string"/> que es nulo.</returns>
    /// <remarks>
    /// Este método configura el trabajo y lo programa utilizando el programador obtenido de la fábrica.
    /// Si el trabajo proporcionado es nulo, el método devuelve nulo sin realizar ninguna operación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="job"/> es nulo.</exception>
    public override async Task<string> AddJobAsync( IJob job ) {
        if ( job == null )
            return null;
        job.Config();
        var scheduler = await _factory.GetScheduler();
        var detail = job.GetDetail();
        await scheduler.ScheduleJob( detail, job.GetTrigger() );
        return null;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una instancia del programador (scheduler) de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una instancia de <see cref="IScheduler"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una fábrica para obtener el programador y lo envuelve en una instancia de <see cref="QuartzScheduler"/>.
    /// </remarks>
    public override async Task<IScheduler> GetSchedulerAsync() {
        var scheduler = await _factory.GetScheduler();
        return new QuartzScheduler( scheduler );
    }
}