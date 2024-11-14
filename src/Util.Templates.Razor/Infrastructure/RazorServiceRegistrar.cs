using Util.Infrastructure;
using Util.Reflections;
using Util.Templates.Razor;
using Util.Templates.Razor.Filters;

namespace Util.Templates.Infrastructure; 

/// <summary>
/// Clase que se encarga de registrar servicios relacionados con Razor.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IServiceRegistrar"/> y proporciona
/// la funcionalidad necesaria para registrar servicios en el contenedor de inyección de dependencias.
/// </remarks>
public class RazorServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de registro de Razor.
    /// </summary>
    /// <value>
    /// El nombre del servicio como una cadena.
    /// </value>
    public static string ServiceName => "Util.Templates.Infrastructure.RazorServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador del pedido.
    /// </summary>
    /// <value>El identificador del pedido, que es un entero constante con el valor 610.</value>
    public int OrderId => 610;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina consultando la configuración del registrador de servicios
    /// mediante el nombre del servicio especificado.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled( ServiceName );

    /// <summary>
    /// Registra los servicios y configuraciones necesarias en el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene la configuración y los servicios.</param>
    /// <returns>Devuelve una acción que puede ser utilizada para realizar operaciones adicionales, actualmente retorna null.</returns>
    /// <remarks>
    /// Este método configura los servicios mediante el HostBuilder y registra las referencias de ensamblado y filtros necesarios.
    /// </remarks>
    public Action Register( ServiceContext serviceContext ) {
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            RegisterDependency( services );
        } );
        RegisterAssemblyReference( serviceContext.TypeFinder );
        RegisterFilters();
        return null;
    }

    /// <summary>
    /// Registra las dependencias necesarias para el motor de plantillas Razor.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las dependencias.</param>
    private void RegisterDependency( IServiceCollection services ) {
        services.AddSingleton<IRazorEngine, RazorEngine>();
        services.AddSingleton<ITemplateEngine, RazorTemplateEngine>();
        services.AddSingleton<IRazorTemplateEngine, RazorTemplateEngine>();
    }

    /// <summary>
    /// Registra referencias de ensamblado en el motor de plantillas Razor.
    /// </summary>
    /// <param name="finder">Una instancia de <see cref="ITypeFinder"/> que se utiliza para encontrar ensamblados.</param>
    /// <remarks>
    /// Este método agrega una referencia al ensamblado "System.Collections" y, si el <paramref name="finder"/> 
    /// es del tipo <see cref="AppDomainTypeFinder"/>, también agrega referencias a todos los ensamblados 
    /// encontrados por el <paramref name="finder"/>.
    /// </remarks>
    private void RegisterAssemblyReference( ITypeFinder finder ) {
        RazorTemplateEngine.AddAssemblyReference( "System.Collections" );
        if( !( finder is AppDomainTypeFinder typeFinder ) )
            return;
        typeFinder.GetAssemblies().ForEach( RazorTemplateEngine.AddAssemblyReference );
    }

    /// <summary>
    /// Registra los filtros necesarios en el motor de plantillas Razor.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de añadir instancias de filtros personalizados al motor de plantillas Razor,
    /// permitiendo su uso en las plantillas para procesar modelos y vistas parciales de manera asíncrona.
    /// </remarks>
    private void RegisterFilters() {
        RazorTemplateEngine.AddFilter( new ModelFilter() );
        RazorTemplateEngine.AddFilter( new PartialAsyncFilter() );
    }
}