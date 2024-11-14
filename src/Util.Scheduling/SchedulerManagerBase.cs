using Util.Helpers;
using Util.Reflections;

namespace Util.Scheduling; 

/// <summary>
/// Clase base abstracta para la gestión de programadores.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación básica para los gestores de programadores,
/// permitiendo la creación de programadores específicos que hereden de esta clase.
/// </remarks>
public abstract class SchedulerManagerBase : ISchedulerManager {
    private readonly ITypeFinder _typeFinder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SchedulerManagerBase"/>.
    /// </summary>
    /// <param name="scopeFactory">La fábrica de ámbitos de servicio que se utilizará para crear instancias de servicios.</param>
    /// <param name="typeFinder">El objeto que se utiliza para encontrar tipos en la aplicación.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="scopeFactory"/> o <paramref name="typeFinder"/> son nulos.</exception>
    protected SchedulerManagerBase( IServiceScopeFactory scopeFactory, ITypeFinder typeFinder ) {
        Ioc.ServiceScopeFactory = scopeFactory ?? throw new ArgumentNullException( nameof( scopeFactory ) );
        _typeFinder = typeFinder ?? throw new ArgumentNullException( nameof( typeFinder ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega un nuevo trabajo de tipo <typeparamref name="TJob"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TJob">El tipo del trabajo que se va a agregar. Debe implementar la interfaz <see cref="IJob"/> y tener un constructor sin parámetros.</typeparam>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado de tipo <see cref="string"/> que contiene el identificador del trabajo agregado.
    /// </returns>
    /// <remarks>
    /// Este método permite la creación y adición de trabajos de forma genérica, facilitando la reutilización de código y la implementación de diferentes tipos de trabajos.
    /// </remarks>
    /// <seealso cref="IJob"/>
    public virtual async Task<string> AddJobAsync<TJob>() where TJob : IJob, new() {
        return await AddJobAsync( new TJob() );
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega un trabajo de forma asíncrona.
    /// </summary>
    /// <param name="job">El trabajo que se desea agregar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que representa el estado del trabajo agregado.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    /// <seealso cref="IJob"/>
    public abstract Task<string> AddJobAsync( IJob job );

    /// <inheritdoc />
    /// <summary>
    /// Escanea y registra trabajos de tipo <see cref="IJob"/> y <see cref="IScan"/>.
    /// </summary>
    /// <param name="isScanAllJobs">Indica si se deben escanear todos los trabajos disponibles. Si es verdadero, se incluirán todos los trabajos de tipo <see cref="IJob"/>.</param>
    /// <remarks>
    /// Este método busca todos los tipos que implementan la interfaz <see cref="IScan"/> y, si se especifica, también busca trabajos que implementan la interfaz <see cref="IJob"/>. 
    /// Los tipos encontrados se procesan para crear instancias de trabajos y se añaden de manera asíncrona.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de escaneo y registro de trabajos.
    /// </returns>
    /// <seealso cref="IJob"/>
    /// <seealso cref="IScan"/>
    public virtual async Task ScanJobsAsync( bool isScanAllJobs = true ) {
        var types = _typeFinder.Find<IScan>() ?? new List<Type>();
        if ( isScanAllJobs ) {
            types.AddRange( _typeFinder.Find<IJob>() );
            types = types.DistinctBy( t => t.FullName ).ToList();
        }
        foreach ( var type in types ) {
            var job = Reflection.CreateInstance<IJob>( type );
            if ( job == null )
                continue;
            await AddJobAsync( job );
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una instancia del programador de tareas de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una instancia de <see cref="IScheduler"/>.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    /// <seealso cref="IScheduler"/>
    public abstract Task<IScheduler> GetSchedulerAsync();
}