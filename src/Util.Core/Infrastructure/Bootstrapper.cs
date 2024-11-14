using Util.Helpers;
using Util.Reflections;

namespace Util.Infrastructure; 

/// <summary>
/// Clase responsable de la inicialización y configuración de la aplicación.
/// </summary>
public class Bootstrapper {
    private readonly IHostBuilder _hostBuilder;
    private readonly IAssemblyFinder _assemblyFinder;
    private readonly ITypeFinder _typeFinder;
    private readonly List<Action> _serviceActions;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Bootstrapper"/>.
    /// </summary>
    /// <param name="hostBuilder">El constructor de host que se utilizará para configurar el entorno de la aplicación.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="hostBuilder"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor configura el buscador de ensamblados y el buscador de tipos,
    /// así como una lista de acciones de servicio que se pueden ejecutar durante el arranque.
    /// </remarks>
    public Bootstrapper( IHostBuilder hostBuilder ) {
        _hostBuilder = hostBuilder ?? throw new ArgumentNullException( nameof( hostBuilder ) );
        _assemblyFinder = new AppDomainAssemblyFinder { AssemblySkipPattern = BootstrapperConfig.AssemblySkipPattern };
        _typeFinder = new AppDomainTypeFinder( _assemblyFinder );
        _serviceActions = new List<Action>();
    }

    /// <summary>
    /// Inicia el proceso de configuración y ejecución de servicios.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de llamar a los métodos necesarios para configurar
    /// los servicios, resolver el registrador de servicios y ejecutar las acciones
    /// de los servicios. Debe ser sobreescrito en clases derivadas si se requiere
    /// una implementación específica.
    /// </remarks>
    public virtual void Start() {
        ConfigureServices();
        ResolveServiceRegistrar();
        ExecuteServiceActions();
    }

    /// <summary>
    /// Configura los servicios necesarios para la aplicación.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de establecer la configuración inicial de los servicios
    /// utilizando el contenedor de inyección de dependencias. Se añaden servicios
    /// singleton para el buscador de ensamblados y el buscador de tipos.
    /// </remarks>
    protected virtual void ConfigureServices()  
    {  
        _hostBuilder.ConfigureServices((context, services) =>  
        {  
            Util.Helpers.Config.SetConfiguration(context.Configuration);  
            services.TryAddSingleton(_assemblyFinder);  
            services.TryAddSingleton(_typeFinder);  
        });  
    }

    /// <summary>
    /// Resuelve y registra instancias de servicios que implementan la interfaz <see cref="IServiceRegistrar"/>.
    /// </summary>
    /// <remarks>
    /// Este método busca todos los tipos que implementan <see cref="IServiceRegistrar"/>, crea instancias de ellos,
    /// filtra aquellos que están habilitados y los ordena según su identificador de orden.
    /// Luego, registra cada instancia en el contexto de servicio.
    /// </remarks>
    /// <param name="_hostBuilder">El constructor del host que se utiliza para la configuración del servicio.</param>
    /// <param name="_assemblyFinder">El buscador de ensamblados que se utiliza para localizar tipos.</param>
    /// <param name="_typeFinder">El buscador de tipos que se utiliza para encontrar implementaciones de <see cref="IServiceRegistrar"/>.</param>
    /// <param name="_serviceActions">La colección donde se almacenan las acciones de registro de servicio.</param>
    /// <seealso cref="IServiceRegistrar"/>
    /// <seealso cref="ServiceContext"/>
    protected virtual void ResolveServiceRegistrar() {
        var types = _typeFinder.Find<IServiceRegistrar>();
        var instances = types.Select(type => Reflection.CreateInstance<IServiceRegistrar>(type)).Where(t => t.Enabled).OrderBy(t => t.OrderId).ToList();
        var context = new ServiceContext(_hostBuilder, _assemblyFinder, _typeFinder);
        instances.ForEach(t => _serviceActions.Add(t.Register(context)));
    }

    /// <summary>
    /// Ejecuta las acciones de servicio definidas en la colección <c>_serviceActions</c>.
    /// </summary>
    /// <remarks>
    /// Este método itera sobre cada acción de servicio y la invoca si no es nula.
    /// </remarks>
    protected virtual void ExecuteServiceActions()
    {
        _serviceActions.ForEach(action => action?.Invoke());
    }
}