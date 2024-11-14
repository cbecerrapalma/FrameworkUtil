using Util.Helpers;

namespace Util.Scheduling; 

/// <summary>
/// Clase base abstracta para la implementación de trabajos en Quartz.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IJob"/> de Quartz, 
/// proporcionando una estructura común para los trabajos que se ejecutarán 
/// en el planificador de Quartz.
/// </remarks>
public abstract class JobBase : IJob, Quartz.IJob {
    public const string DataKey = "Util_Job_Data";
    private IJobDetail _jobDetail;
    private ITrigger _trigger;

    /// <summary>
    /// Obtiene o establece el objeto de datos asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a un objeto de datos que puede ser de cualquier tipo.
    /// </remarks>
    /// <value>
    /// Un objeto que representa los datos. Puede ser nulo si no se ha establecido ningún valor.
    /// </value>
    public object Data { get; set; }

    /// <summary>
    /// Configura los detalles del trabajo y el disparador para la ejecución de tareas programadas.
    /// </summary>
    /// <remarks>
    /// Este método inicializa un objeto de tipo <see cref="QuartzJobInfo"/> y un objeto de tipo <see cref="QuartzTrigger"/>.
    /// Luego, llama a los métodos <see cref="ConfigDetail(QuartzJobInfo)"/> y <see cref="ConfigTrigger(QuartzTrigger)"/> 
    /// para configurar los detalles del trabajo y el disparador, respectivamente. Finalmente, crea el detalle del trabajo 
    /// y el disparador a partir de las configuraciones realizadas.
    /// </remarks>
    public void Config() {
        var jobInfo = new QuartzJobInfo();
        var trigger = new QuartzTrigger();
        ConfigDetail( jobInfo );
        ConfigTrigger( trigger );
        _jobDetail = CreateJobDetail( jobInfo );
        _trigger = trigger.ToTrigger();
        Config( _jobDetail, _trigger );
    }

    /// <summary>
    /// Configura los detalles de un trabajo específico.
    /// </summary>
    /// <param name="job">El objeto que contiene la información del trabajo a configurar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// </remarks>
    protected virtual void ConfigDetail( IJobInfo job ) {
    }

    /// <summary>
    /// Configura el disparador de trabajo especificado.
    /// </summary>
    /// <param name="trigger">El disparador de trabajo que se va a configurar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una configuración específica del disparador.
    /// </remarks>
    protected virtual void ConfigTrigger( IJobTrigger trigger ) {
    }

    /// <summary>
    /// Crea un detalle de trabajo basado en la información proporcionada por el objeto <paramref name="job"/>.
    /// </summary>
    /// <param name="job">El objeto que contiene la información del trabajo a crear.</param>
    /// <returns>Un objeto <see cref="IJobDetail"/> que representa el trabajo creado.</returns>
    /// <remarks>
    /// Este método utiliza el constructor de <see cref="JobBuilder"/> para configurar el trabajo 
    /// con un identificador único basado en el nombre y grupo del trabajo. Si hay datos adicionales 
    /// disponibles, se serializan a formato JSON y se añaden al trabajo.
    /// </remarks>
    private IJobDetail CreateJobDetail( IJobInfo job ) {
        var builder = JobBuilder.Create( GetType() )
            .WithIdentity( job.GetName(), job.GetGroup() );
        if ( Data != null )
            builder.UsingJobData( DataKey, Util.Helpers.Json.ToJson( Data ) );
        return builder.Build();
    }

    /// <summary>
    /// Configura los detalles del trabajo y el disparador.
    /// </summary>
    /// <param name="jobDetail">Los detalles del trabajo que se va a configurar.</param>
    /// <param name="trigger">El disparador que se va a configurar.</param>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban para proporcionar una configuración específica.
    /// </remarks>
    protected virtual void Config( IJobDetail jobDetail, ITrigger trigger ) {
    }

    /// <summary>
    /// Ejecuta la tarea programada utilizando el contexto de ejecución de Quartz.
    /// </summary>
    /// <param name="context">El contexto de ejecución del trabajo que contiene información sobre la ejecución actual.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método crea un nuevo ámbito de servicio utilizando el <see cref="IServiceScopeFactory"/> 
    /// y luego llama a otro método de ejecución pasando un contexto específico de Quartz.
    /// </remarks>
    public async Task Execute( IJobExecutionContext context ) {
        using var scope = Ioc.ServiceScopeFactory.CreateScope();
        await Execute( new QuartzExecutionContext( scope.ServiceProvider, context ) );
    }

    /// <summary>
    /// Método abstracto que se encarga de ejecutar una tarea en el contexto de Quartz.
    /// </summary>
    /// <param name="context">El contexto de ejecución de Quartz que proporciona información sobre la tarea a ejecutar.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para definir la lógica específica de la tarea.
    /// </remarks>
    /// <seealso cref="QuartzExecutionContext"/>
    protected abstract Task Execute( QuartzExecutionContext context );

    /// <summary>
    /// Obtiene los detalles del trabajo.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IJobDetail"/> que representa los detalles del trabajo.
    /// </returns>
    public IJobDetail GetJobDetail() {
        return _jobDetail;
    }

    /// <summary>
    /// Obtiene el disparador asociado.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ITrigger"/> que representa el disparador.
    /// </returns>
    public ITrigger GetTrigger() {
        return _trigger;
    }
}