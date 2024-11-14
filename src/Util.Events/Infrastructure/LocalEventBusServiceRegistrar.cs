using Util.Infrastructure;
using Util.Reflections;

namespace Util.Events.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios para el bus de eventos local.
/// </summary>
/// <remarks>
/// Esta clase se encarga de registrar los servicios necesarios para el funcionamiento del bus de eventos en el contexto local.
/// </remarks>
public class LocalEventBusServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de registro del bus de eventos local.
    /// </summary>
    /// <remarks>
    /// Este servicio se utiliza para registrar eventos en la infraestructura de eventos local.
    /// </remarks>
    /// <returns>
    /// Un string que representa el nombre del servicio.
    /// </returns>
    public static string ServiceName => "Util.Events.Infrastructure.LocalEventBusServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador de la orden.
    /// </summary>
    /// <value>
    /// El identificador de la orden, que es un número entero.
    /// </value>
    public int OrderId => 510;

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
    /// Registra los servicios y controladores de eventos en el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene la configuración del host.</param>
    /// <returns>Devuelve null, ya que este método no tiene un valor de retorno significativo.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para la aplicación utilizando el 
    /// <see cref="IServiceCollection"/> proporcionado por el <paramref name="serviceContext"/>.
    /// </remarks>
    public Action Register( ServiceContext serviceContext ) {
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            RegisterDependency( services );
            RegisterEventHandlers( services, serviceContext.TypeFinder );
        } );
        return null;
    }

    /// <summary>
    /// Registra las dependencias necesarias en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán las dependencias.</param>
    private void RegisterDependency( IServiceCollection services ) {
        services.TryAddTransient<ILocalEventBus, LocalEventBus>();
    }

    /// <summary>
    /// Registra los controladores de eventos en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán los controladores de eventos.</param>
    /// <param name="finder">Un objeto que permite encontrar tipos en el ensamblado.</param>
    /// <remarks>
    /// Este método busca todos los tipos que implementan la interfaz genérica <see cref="IEventHandler{T}"/> 
    /// y los registra en el contenedor de servicios como servicios de tipo <see cref="ServiceLifetime.Scoped"/>.
    /// </remarks>
    private void RegisterEventHandlers( IServiceCollection services, ITypeFinder finder ) {
        Type handlerType = typeof(IEventHandler<>);
        var handlerTypes = GetTypes( finder,handlerType );
        foreach( var handler in handlerTypes ) {
            var serviceTypes = handler.FindInterfaces( ( filter, criteria ) => criteria != null && filter.IsGenericType && ( (Type)criteria ).IsAssignableFrom( filter.GetGenericTypeDefinition() ), handlerType );
            serviceTypes.ToList().ForEach( serviceType => services.AddScoped( serviceType, handler ) );
        }
    }

    /// <summary>
    /// Obtiene un arreglo de tipos que coinciden con el tipo especificado utilizando un buscador de tipos.
    /// </summary>
    /// <param name="finder">El buscador de tipos que se utilizará para encontrar los tipos.</param>
    /// <param name="type">El tipo para el cual se desean encontrar coincidencias.</param>
    /// <returns>Un arreglo de tipos que coinciden con el tipo especificado.</returns>
    private Type[] GetTypes( ITypeFinder finder,Type type ) {
        return finder.Find( type ).ToArray();
    }
}