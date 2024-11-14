namespace Util.Helpers; 

/// <summary>
/// Clase estática que proporciona métodos para la inversión de control (IoC).
/// </summary>
public static class Ioc {
    private static readonly Util.Dependency.Container _container = Util.Dependency.Container.Instance;
    private static Func<IServiceProvider> _getServiceProviderAction;

    /// <summary>
    /// Obtiene o establece la fábrica de ámbitos de servicio.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite crear un ámbito de servicio que se puede utilizar para resolver servicios
    /// dentro de un contexto específico, facilitando la gestión del ciclo de vida de los servicios.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="IServiceScopeFactory"/> que se utiliza para crear ámbitos de servicio.
    /// </value>
    public static IServiceScopeFactory ServiceScopeFactory { get; set; }

    /// <summary>
    /// Crea una nueva instancia del contenedor de dependencias.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="Util.Dependency.Container"/> que representa el contenedor de dependencias.
    /// </returns>
    public static Util.Dependency.Container CreateContainer() {
        return new Util.Dependency.Container();
    }

    /// <summary>
    /// Obtiene los servicios registrados en el contenedor de inyección de dependencias.
    /// </summary>
    /// <returns>
    /// Una colección de servicios disponibles en el contenedor.
    /// </returns>
    public static IServiceCollection GetServices() {
        return _container.GetServices();
    }

    /// <summary>
    /// Establece una acción que proporciona un <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="action">Una función que devuelve una instancia de <see cref="IServiceProvider"/>.</param>
    /// <remarks>
    /// Este método permite configurar una acción personalizada que se utilizará para obtener el proveedor de servicios.
    /// </remarks>
    /// <seealso cref="IServiceProvider"/>
    public static void SetServiceProviderAction( Func<IServiceProvider> action ) {
        _getServiceProviderAction = action;
    }

    /// <summary>
    /// Obtiene el proveedor de servicios.
    /// </summary>
    /// <returns>
    /// Un <see cref="IServiceProvider"/> que representa el proveedor de servicios actual.
    /// Si el proveedor de servicios no está disponible, se obtiene del contenedor.
    /// </returns>
    public static IServiceProvider GetServiceProvider() {
        var provider = _getServiceProviderAction?.Invoke();
        if ( provider != null )
            return provider;
        return _container.GetServiceProvider();
    }

    /// <summary>
    /// Crea una instancia de un tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de la instancia que se desea crear.</typeparam>
    /// <returns>Una nueva instancia del tipo especificado.</returns>
    /// <remarks>
    /// Este método utiliza el tipo genérico <typeparamref name="T"/> para crear una nueva instancia.
    /// Se asume que el tipo tiene un constructor sin parámetros.
    /// </remarks>
    /// <seealso cref="Create{T}(Type)"/>
    public static T Create<T>() {
        return Create<T>( typeof( T ) );
    }

    /// <summary>
    /// Crea una instancia del tipo especificado y la devuelve como el tipo genérico T.
    /// </summary>
    /// <typeparam name="T">El tipo al que se convertirá la instancia creada.</typeparam>
    /// <param name="type">El tipo del cual se desea crear una instancia.</param>
    /// <returns>Una instancia del tipo T si se pudo crear, de lo contrario, el valor predeterminado de T.</returns>
    /// <remarks>
    /// Este método intenta crear una instancia del tipo proporcionado. Si la creación falla y el resultado es nulo,
    /// se devuelve el valor predeterminado para el tipo T.
    /// </remarks>
    /// <seealso cref="System.Activator.CreateInstance(Type)"/>
    public static T Create<T>( Type type ) {
        var service = Create( type );
        if( service == null )
            return default;
        return (T)service;
    }

    /// <summary>
    /// Crea una instancia del tipo especificado utilizando el proveedor de servicios.
    /// </summary>
    /// <param name="type">El tipo de objeto que se desea crear. No puede ser null.</param>
    /// <returns>
    /// Una instancia del tipo especificado, o null si el tipo es null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un proveedor de servicios para obtener una instancia del tipo solicitado.
    /// Si el tipo es null, el método devolverá null sin intentar crear una instancia.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el tipo es null.</exception>
    public static object Create( Type type ) {
        if( type == null )
            return null;
        var provider = GetServiceProvider();
        return provider.GetService( type );
    }

    /// <summary>
    /// Crea una nueva lista de tipo generico <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que contendrá la lista.</typeparam>
    /// <returns>
    /// Una nueva instancia de <see cref="List{T}"/> que contiene elementos del tipo <typeparamref name="T"/>.
    /// </returns>
    public static List<T> CreateList<T>() {
        return CreateList<T>( typeof( T ) );
    }

    /// <summary>
    /// Crea una lista de un tipo específico.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos de la lista.</typeparam>
    /// <param name="type">El tipo que se utilizará para crear la lista.</param>
    /// <returns>Una lista de tipo <typeparamref name="T"/>. Si no se puede crear la lista, se devuelve una lista vacía.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para crear una instancia de un tipo que implementa <see cref="IEnumerable{T}"/> 
    /// y convierte el resultado en una lista del tipo especificado.
    /// </remarks>
    /// <seealso cref="IEnumerable{T}"/>
    public static List<T> CreateList<T>( Type type ) {
        Type serviceType = typeof( IEnumerable<> ).MakeGenericType( type );
        var result = Create( serviceType );
        if( result == null )
            return new List<T>();
        return ( (IEnumerable<T>)result ).ToList();
    }

    /// <summary>
    /// Crea un nuevo ámbito de servicio (scope) utilizando el proveedor de servicios actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IServiceScope"/> que representa el nuevo ámbito de servicio.
    /// </returns>
    public static IServiceScope CreateScope() {
        var provider = GetServiceProvider();
        return provider.CreateScope();
    }

    /// <summary>
    /// Limpia todos los elementos del contenedor.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos almacenados en el contenedor, 
    /// dejándolo vacío. Es útil para reiniciar el estado del contenedor 
    /// sin necesidad de crear una nueva instancia.
    /// </remarks>
    public static void Clear() {
        _container.Clear();
    }
}