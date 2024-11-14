using Microsoft.Extensions.Options;
using Util.Reflections;

namespace Util.Scheduling; 

/// <summary>
/// Clase que gestiona la programación de tareas utilizando Hangfire.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SchedulerManagerBase"/> y proporciona funcionalidades específicas para la programación de trabajos en segundo plano.
/// </remarks>
public class HangfireSchedulerManager : SchedulerManagerBase {
    private readonly BackgroundJobServerOptions _serverOptions;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HangfireSchedulerManager"/>.
    /// </summary>
    /// <param name="scopeFactory">La fábrica de ámbitos que proporciona acceso a los servicios de la aplicación.</param>
    /// <param name="typeFinder">El buscador de tipos que permite localizar tipos en la aplicación.</param>
    /// <param name="serverOptions">Las opciones del servidor de trabajos en segundo plano.</param>
    public HangfireSchedulerManager( IServiceScopeFactory scopeFactory, ITypeFinder typeFinder,
        IOptions<BackgroundJobServerOptions> serverOptions ) : base( scopeFactory, typeFinder ) {
        _serverOptions = serverOptions.Value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega un trabajo de forma asíncrona.
    /// </summary>
    /// <param name="job">El trabajo que se va a agregar. No puede ser null.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con el resultado que indica el estado del trabajo agregado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="job"/> es null.</exception>
    /// <remarks>
    /// Este método configura el trabajo antes de programarlo. 
    /// Si el trabajo proporcionado es null, el método devolverá null.
    /// </remarks>
    /// <seealso cref="IJob"/>
    /// <seealso cref="JobBase"/>
    public override Task<string> AddJobAsync( IJob job ) {
        if ( job == null )
            return null;
        job.Config();
        var result = ScheduleJob( job as JobBase );
        return Task.FromResult( result );
    }

    /// <summary>
    /// Programa un trabajo para su ejecución, ya sea de forma recurrente o en un momento específico.
    /// </summary>
    /// <param name="job">El trabajo a programar, que debe ser una instancia de <see cref="JobBase"/>.</param>
    /// <returns>
    /// Un identificador único del trabajo programado si se ha programado correctamente; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Si el trabajo es nulo, el método devolverá <c>null</c>. Si el trabajo tiene un trigger con una expresión cron válida, 
    /// se programará como un trabajo recurrente. Si el trigger tiene un retraso especificado, 
    /// se programará como un trabajo diferido. De lo contrario, se encolará como un trabajo en segundo plano.
    /// </remarks>
    /// <seealso cref="JobBase"/>
    /// <seealso cref="RecurringJob"/>
    /// <seealso cref="BackgroundJob"/>
    private string ScheduleJob( JobBase job ) {
        if ( job == null )
            return null;
        var jobInfo = job.GetJobInfo();
        var trigger = job.GetTrigger();
        if ( trigger.GetCron().IsEmpty() == false ) {
            var id = jobInfo.GetId();
            if ( id.IsEmpty() )
                id = job.GetType().FullName;
            RecurringJob.AddOrUpdate( id, jobInfo.GetQueue(), () => job.Execute( job.Data ), trigger.GetCron() );
            return id;
        }
        return trigger.GetDelay() == null ? BackgroundJob.Enqueue( () => job.Execute( job.Data ) ) : BackgroundJob.Schedule( () => job.Execute( job.Data ), trigger.GetDelay().SafeValue() );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el programador de tareas de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene el programador de tareas <see cref="IScheduler"/>.
    /// </returns>
    /// <remarks>
    /// Este método crea una instancia de <see cref="HangfireScheduler"/> utilizando las opciones del servidor.
    /// </remarks>
    public override Task<IScheduler> GetSchedulerAsync() {
        return Task.FromResult( (IScheduler)new HangfireScheduler(_serverOptions) );
    }
}