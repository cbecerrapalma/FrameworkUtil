namespace Util.Scheduling; 

/// <summary>
/// Representa el contexto de ejecución para las tareas programadas con Quartz.
/// </summary>
/// <remarks>
/// Esta clase contiene información relevante sobre el estado y los parámetros de la ejecución
/// de una tarea programada. Se utiliza para pasar datos entre diferentes componentes del sistema
/// durante la ejecución de trabajos en Quartz.
/// </remarks>
public class QuartzExecutionContext {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QuartzExecutionContext"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="context">El contexto de ejecución del trabajo de Quartz.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> o <paramref name="context"/> son nulos.</exception>
    public QuartzExecutionContext( IServiceProvider serviceProvider, IJobExecutionContext context ) {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException( nameof( serviceProvider ) );
        Context = context ?? throw new ArgumentNullException( nameof( context ) );
    }

    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Este proveedor de servicios se utiliza para resolver dependencias en la aplicación.
    /// </remarks>
    /// <returns>
    /// Un objeto que implementa <see cref="IServiceProvider"/> que permite la resolución de servicios.
    /// </returns>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Obtiene el contexto de ejecución del trabajo.
    /// </summary>
    /// <value>
    /// Un objeto que implementa <see cref="IJobExecutionContext"/> que proporciona información sobre la ejecución del trabajo actual.
    /// </value>
    public IJobExecutionContext Context { get; }

    /// <summary>
    /// Obtiene una instancia del servicio especificado por el tipo generico T.
    /// </summary>
    /// <typeparam name="T">El tipo del servicio que se desea obtener.</typeparam>
    /// <returns>
    /// Una instancia del servicio solicitado, o null si el servicio no está registrado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el ServiceProvider para resolver la dependencia del servicio.
    /// Asegúrese de que el servicio esté registrado en el contenedor de servicios antes de llamarlo.
    /// </remarks>
    /// <seealso cref="ServiceProvider"/>
    public T GetService<T>() {
        return ServiceProvider.GetService<T>();
    }

    /// <summary>
    /// Obtiene datos del contexto en formato JSON y los deserializa en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se deserializarán los datos.</typeparam>
    /// <returns>
    /// Un objeto del tipo <typeparamref name="T"/> que representa los datos deserializados.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clave <c>JobBase.DataKey</c> para obtener la cadena JSON del mapa de datos del trabajo.
    /// Asegúrese de que el JSON sea compatible con el tipo <typeparamref name="T"/> para evitar excepciones durante la deserialización.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Json.ToObject{T}(string)"/>
    public T GetData<T>() {
        var json = Context.JobDetail.JobDataMap.GetString( JobBase.DataKey );
        return Util.Helpers.Json.ToObject<T>( json );
    }
}