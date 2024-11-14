namespace Util.Dependency; 

/// <summary>
/// Representa un contenedor que puede almacenar elementos.
/// </summary>
public class Container {
    private readonly ServiceCollection _services;
    private IServiceProvider _provider;

    public static readonly Container Instance = new();

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Container"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una nueva colección de servicios utilizando <see cref="ServiceCollection"/>.
    /// </remarks>
    public Container() {
        _services = new ServiceCollection();
    }

    /// <summary>
    /// Obtiene la colección de servicios.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ServiceCollection"/> que contiene todos los servicios registrados.
    /// </returns>
    public ServiceCollection GetServices() {
        return _services;
    }

    /// <summary>
    /// Obtiene el proveedor de servicios.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IServiceProvider"/> que representa el proveedor de servicios.
    /// </returns>
    public IServiceProvider GetServiceProvider() {
        return _provider ??= _services.BuildServiceProvider();
    }

    /// <summary>
    /// Obtiene una instancia del servicio del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del servicio que se desea obtener.</typeparam>
    /// <returns>Una instancia del servicio del tipo especificado.</returns>
    /// <remarks>
    /// Este método es un envoltorio que llama a otro método de obtención de servicios
    /// pasando el tipo de servicio como parámetro.
    /// </remarks>
    /// <seealso cref="GetService{T}(Type)"/>
    public T GetService<T>() {
        return GetService<T>( typeof(T) );
    }

    /// <summary>
    /// Obtiene un servicio del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del servicio que se desea obtener.</typeparam>
    /// <param name="type">El tipo del servicio que se está buscando.</param>
    /// <returns>Una instancia del servicio del tipo especificado, o el valor predeterminado si no se encuentra.</returns>
    /// <remarks>
    /// Este método intenta recuperar un servicio basado en el tipo proporcionado.
    /// Si el servicio no se encuentra, se devuelve el valor predeterminado para el tipo T.
    /// </remarks>
    /// <seealso cref="GetService(Type)"/>
    public T GetService<T>( Type type ) {
        var service = GetService( type );
        if ( service == null )
            return default;
        return (T)service;
    }

    /// <summary>
    /// Obtiene un servicio del contenedor de servicios basado en el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo del servicio que se desea obtener.</param>
    /// <returns>Una instancia del servicio solicitado, o null si no se encuentra el servicio.</returns>
    public object GetService( Type type ) {
        var provider = GetServiceProvider();
        return provider.GetService( type );
    }

    /// <summary>
    /// Limpia todos los servicios registrados y restablece el proveedor a null.
    /// </summary>
    /// <remarks>
    /// Este método elimina todas las instancias de servicios almacenados en la colección 
    /// y asegura que el proveedor no apunte a ninguna instancia, liberando así los recursos 
    /// asociados.
    /// </remarks>
    public void Clear() {
        _services.Clear();
        _provider = null;
    }
}