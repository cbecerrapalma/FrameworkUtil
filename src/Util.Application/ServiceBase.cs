namespace Util.Applications; 

/// <summary>
/// Clase base abstracta para servicios que implementan la interfaz <see cref="IService"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación común para los servicios, 
/// permitiendo que las clases derivadas compartan funcionalidad común.
/// </remarks>
public abstract class ServiceBase : IService {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor obtiene instancias de los servicios necesarios a través del <paramref name="serviceProvider"/>.
    /// Si alguno de los servicios solicitados no está disponible, se utilizará una instancia nula correspondiente.
    /// </remarks>
    protected ServiceBase(IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Session = serviceProvider.GetService<ISession>() ?? NullSession.Instance;
        IntegrationEventBus = serviceProvider.GetService<IIntegrationEventBus>() ?? NullIntegrationEventBus.Instance;
        var logFactory = serviceProvider.GetService<ILogFactory>();
        Log = logFactory?.CreateLog(GetType()) ?? NullLog.Instance;
        L = serviceProvider.GetService<IStringLocalizer>() ?? NullStringLocalizer.Instance;
    }

    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Este miembro es de solo lectura y proporciona acceso a los servicios 
    /// registrados en el contenedor de inyección de dependencias.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IServiceProvider"/> que permite 
    /// resolver servicios en el contexto actual.
    /// </value>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Obtiene o establece la sesión actual.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="ISession"/> que representa la sesión actual.
    /// </value>
    protected ISession Session { get; set; }

    /// <summary>
    /// Obtiene la instancia del bus de eventos de integración.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al bus de eventos que se utiliza para 
    protected IIntegrationEventBus IntegrationEventBus { get; }

    /// <summary>
    /// Obtiene el registro de log asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un objeto que implementa la interfaz <see cref="ILog"/>.
    /// Se utiliza para registrar información, advertencias y errores durante la ejecución de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ILog"/>.
    /// </value>
    protected ILog Log { get; }

    /// <summary>
    /// Obtiene o establece el localizador de cadenas para la localización de texto.
    /// </summary>
    /// <remarks>
    /// Este localizador se utiliza para traducir cadenas de texto en la aplicación,
    /// permitiendo soportar múltiples idiomas y culturas.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IStringLocalizer"/>.
    /// </value>
    protected IStringLocalizer L { get; set; }
}