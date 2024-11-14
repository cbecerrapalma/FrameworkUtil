using Util.Dependency;
using Util.Reflections;

namespace Util.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios de dependencia.
/// </summary>
/// <remarks>
/// Esta clase se encarga de registrar los servicios necesarios para la inyección de dependencias
/// en la aplicación, facilitando la gestión de las instancias de los servicios.
/// </remarks>
public class DependencyServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de infraestructura de registro de dependencias.
    /// </summary>
    /// <value>
    /// El nombre del servicio como una cadena.
    /// </value>
    public static string ServiceName => "Util.Infrastructure.DependencyServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador de la orden.
    /// </summary>
    /// <value>
    /// El identificador de la orden, que es un valor entero fijo de 100.
    /// </value>
    public int OrderId => 100;

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
    /// Registra las dependencias en el contenedor de servicios utilizando el contexto de servicio proporcionado.
    /// </summary>
    /// <param name="serviceContext">El contexto de servicio que contiene información sobre el host y el localizador de tipos.</param>
    /// <returns>Una acción que configura los servicios en el contenedor de servicios.</returns>
    /// <remarks>
    /// Este método registra tres tipos de dependencias: 
    /// <list type="bullet">
    /// <item><description>ISingletonDependency como Singleton.</description></item>
    /// <item><description>IScopeDependency como Scoped.</description></item>
    /// <item><description>ITransientDependency como Transient.</description></item>
    /// </list>
    /// Se utiliza el método <see cref="RegisterDependency{T}"/> para realizar el registro de cada dependencia con su respectivo ciclo de vida.
    /// </remarks>
    public Action Register( ServiceContext serviceContext ) {
        return () => {
            serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
                RegisterDependency<ISingletonDependency>( services, serviceContext.TypeFinder, ServiceLifetime.Singleton );
                RegisterDependency<IScopeDependency>( services, serviceContext.TypeFinder, ServiceLifetime.Scoped );
                RegisterDependency<ITransientDependency>( services, serviceContext.TypeFinder, ServiceLifetime.Transient );
            } );
        };
    }

    /// <summary>
    /// Registra las dependencias en el contenedor de servicios.
    /// </summary>
    /// <typeparam name="TDependencyInterface">La interfaz de la dependencia que se va a registrar.</typeparam>
    /// <param name="services">La colección de servicios donde se registrarán las dependencias.</param>
    /// <param name="finder">El objeto que se utiliza para encontrar tipos.</param>
    /// <param name="lifetime">El tiempo de vida del servicio que se va a registrar.</param>
    /// <remarks>
    /// Este método busca todos los tipos que implementan la interfaz especificada,
    /// filtra los tipos encontrados y los registra en la colección de servicios
    /// con el tiempo de vida especificado.
    /// </remarks>
    private void RegisterDependency<TDependencyInterface>( IServiceCollection services, ITypeFinder finder, ServiceLifetime lifetime ) {
        var types = GetTypes<TDependencyInterface>( finder );
        var result = FilterTypes( types );
        foreach ( var item in result )
            RegisterType( services, item.Item1, item.Item2, lifetime );
    }

    /// <summary>
    /// Obtiene una lista de tuplas que contienen tipos de interfaz y sus correspondientes tipos de clase 
    /// que implementan la interfaz especificada.
    /// </summary>
    /// <typeparam name="TDependencyInterface">El tipo de interfaz que se busca.</typeparam>
    /// <param name="finder">Una instancia de <see cref="ITypeFinder"/> que se utiliza para encontrar los tipos de clase.</param>
    /// <returns>
    /// Una lista de tuplas, donde cada tupla contiene un tipo de interfaz y el tipo de clase que lo implementa.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="ITypeFinder"/> para localizar todos los tipos de clase que implementan
    /// la interfaz especificada por el tipo <typeparamref name="TDependencyInterface"/>. Luego, para cada tipo de clase
    /// encontrado, se obtienen los tipos de interfaz que implementa y se añaden a la lista de resultados.
    /// </remarks>
    private List<(Type, Type)> GetTypes<TDependencyInterface>( ITypeFinder finder ) {
        var result = new List<(Type, Type)>();
        var classTypes = finder.Find<TDependencyInterface>();
        foreach ( var classType in classTypes ) {
            var interfaceTypes = Util.Helpers.Reflection.GetInterfaceTypes( classType, typeof( TDependencyInterface ) );
            interfaceTypes.ForEach( interfaceType => result.Add( (interfaceType, classType) ) );
        }
        return result;
    }

    /// <summary>
    /// Filtra una lista de tuplas de tipos, agrupando por el primer tipo de cada tupla.
    /// </summary>
    /// <param name="types">Una lista de tuplas donde cada tupla contiene dos tipos.</param>
    /// <returns>Una lista de tuplas filtradas, donde se han agrupado los tipos por el primer elemento de la tupla.</returns>
    /// <remarks>
    /// Si un grupo de tipos tiene solo un elemento, se agrega directamente al resultado.
    /// Si un grupo tiene múltiples elementos, se utiliza un método auxiliar para determinar qué tipo agregar basado en una prioridad.
    /// </remarks>
    private List<(Type, Type)> FilterTypes( List<(Type, Type)> types ) {
        var result = new List<(Type, Type)>();
        foreach ( var group in types.GroupBy( t => t.Item1 ) ) {
            if ( group.Count() == 1 ) {
                result.Add( group.First() );
                continue;
            }
            result.Add( GetTypesByPriority( group ) );
        }
        return result;
    }

    /// <summary>
    /// Obtiene los tipos asociados a un grupo, seleccionando el tipo con la mayor prioridad.
    /// </summary>
    /// <param name="group">Un grupo de elementos que contiene tuplas de tipos.</param>
    /// <returns>
    /// Una tupla que contiene el tipo clave del grupo y el tipo con la mayor prioridad.
    /// </returns>
    /// <remarks>
    /// Este método recorre los elementos del grupo y utiliza la función <see cref="GetPriority"/> 
    /// para determinar la prioridad de cada tipo. Solo se considera el tipo con la mayor prioridad.
    /// </remarks>
    private (Type, Type) GetTypesByPriority( IGrouping<Type, (Type, Type)> group ) {
        int? currentPriority = null;
        Type classType = null;
        foreach ( var item in group ) {
            var priority = GetPriority( item.Item2 );
            if ( currentPriority == null || priority > currentPriority ) {
                currentPriority = priority;
                classType = item.Item2;
            }
        }
        return ( group.Key, classType );
    }

    /// <summary>
    /// Obtiene la prioridad de un tipo basado en el atributo <see cref="IocAttribute"/>.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener la prioridad.</param>
    /// <returns>La prioridad del tipo si el atributo <see cref="IocAttribute"/> está presente; de lo contrario, devuelve 0.</returns>
    /// <remarks>
    /// Este método busca el atributo <see cref="IocAttribute"/> en el tipo proporcionado y devuelve su valor de prioridad.
    /// Si el atributo no está presente, se asume que la prioridad es 0.
    /// </remarks>
    /// <seealso cref="IocAttribute"/>
    private int GetPriority( Type type ) {
        var attribute = type.GetCustomAttribute<IocAttribute>();
        if ( attribute == null )
            return 0;
        return attribute.Priority;
    }

    /// <summary>
    /// Registra un tipo de servicio en la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el tipo.</param>
    /// <param name="interfaceType">El tipo de la interfaz que se está registrando.</param>
    /// <param name="classType">El tipo de la clase que implementa la interfaz.</param>
    /// <param name="lifetime">El tiempo de vida del servicio, que determina cómo se gestiona la instancia del servicio.</param>
    private void RegisterType( IServiceCollection services, Type interfaceType, Type classType, ServiceLifetime lifetime ) {
        services.TryAdd( new ServiceDescriptor( interfaceType, classType, lifetime ) );
    }
}