using Util.Helpers;

namespace Util.Scheduling; 

/// <summary>
/// Clase base abstracta que representa un trabajo que implementa la interfaz <see cref="IJob"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona una estructura común para todos los trabajos que se derivan de ella.
/// Las clases que heredan de <see cref="JobBase"/> deben implementar los métodos definidos en la interfaz <see cref="IJob"/>.
/// </remarks>
public abstract class JobBase : IJob {
    private IJobInfo _jobInfo;
    private IJobTrigger _trigger;
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena de texto.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Representa los datos asociados a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar cualquier tipo de dato, ya que es de tipo <see cref="object"/>.
    /// </remarks>
    /// <value>
    /// Un objeto que representa los datos. Puede ser nulo si no se ha asignado ningún valor.
    /// </value>
    public object Data { get; set; }

    /// <summary>
    /// Configura la información del trabajo y el disparador para Hangfire.
    /// </summary>
    /// <remarks>
    /// Este método inicializa las instancias de <see cref="HangfireJobInfo"/> y <see cref="HangfireTrigger"/> 
    /// y llama a métodos adicionales para configurar sus propiedades.
    /// </remarks>
    public void Config() {
        _jobInfo = new HangfireJobInfo();
        _trigger = new HangfireTrigger();
        ConfigId( _jobInfo );
        ConfigDetail( _jobInfo );
        ConfigTrigger( _trigger );
    }

    /// <summary>
    /// Configura el identificador del trabajo.
    /// </summary>
    /// <param name="job">El objeto <see cref="IJobInfo"/> que representa el trabajo al que se le asignará el identificador.</param>
    /// <remarks>
    /// Este método verifica si el identificador está vacío antes de asignarlo al trabajo.
    /// Si el identificador está vacío, no se realiza ninguna acción.
    /// </remarks>
    protected virtual void ConfigId( IJobInfo job ) {
        if ( Id.IsEmpty() )
            return;
        job.Id( Id );
    }

    /// <summary>
    /// Configura los detalles de un trabajo específico.
    /// </summary>
    /// <param name="job">La información del trabajo que se va a configurar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación específica.
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
    /// Ejecuta una tarea de manera asíncrona utilizando un contexto de ejecución de Hangfire.
    /// </summary>
    /// <param name="data">Los datos que se utilizarán durante la ejecución de la tarea.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método crea un nuevo alcance de servicio utilizando el <see cref="Ioc.ServiceScopeFactory"/> 
    /// y luego llama a otro método <see cref="Execute(HangfireExecutionContext)"/> 
    /// pasando un contexto de ejecución de Hangfire que incluye el proveedor de servicios y los datos.
    /// </remarks>
    public virtual async Task Execute( object data ) {
        using var scope = Ioc.ServiceScopeFactory.CreateScope();
        await Execute( new HangfireExecutionContext( scope.ServiceProvider, data ) );
    }

    /// <summary>
    /// Método abstracto que se encarga de ejecutar una tarea en el contexto de Hangfire.
    /// </summary>
    /// <param name="context">El contexto de ejecución de Hangfire que proporciona información sobre la tarea que se está ejecutando.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para definir la lógica específica de la tarea.
    /// </remarks>
    protected abstract Task Execute( HangfireExecutionContext context );

    /// <summary>
    /// Obtiene la información del trabajo.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IJobInfo"/> que contiene la información del trabajo.
    /// </returns>
    public IJobInfo GetJobInfo() {
        return _jobInfo;
    }

    /// <summary>
    /// Obtiene el desencadenador asociado.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IJobTrigger"/> que representa el desencadenador.
    /// </returns>
    public IJobTrigger GetTrigger() {
        return _trigger;
    }
}