namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten registrar servicios en el contenedor de inyección de dependencias.
/// </remarks>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Obtiene la configuración de la colección de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se busca la instancia de configuración.</param>
    /// <returns>La instancia de <see cref="IConfiguration"/> que se encuentra en la colección de servicios.</returns>
    /// <remarks>
    /// Este método extiende <see cref="IServiceCollection"/> para facilitar el acceso a la configuración
    /// registrada en el contenedor de servicios.
    /// </remarks>
    /// <seealso cref="IServiceCollection"/>
    /// <seealso cref="IConfiguration"/>
    public static IConfiguration GetConfiguration( this IServiceCollection services ) {
        return services.GetSingletonInstance<IConfiguration>();
    }

    /// <summary>
    /// Obtiene la instancia singleton de un servicio registrado en la colección de servicios.
    /// </summary>
    /// <typeparam name="T">El tipo del servicio que se desea obtener.</typeparam>
    /// <param name="services">La colección de servicios donde se busca el servicio.</param>
    /// <returns>
    /// La instancia del servicio de tipo <typeparamref name="T"/> si está registrada como singleton; de lo contrario, <c>default</c>.
    /// </returns>
    /// <remarks>
    /// Este método busca en la colección de servicios un servicio que coincida con el tipo especificado y que esté registrado con un ciclo de vida singleton.
    /// Si se encuentra el servicio, se devuelve la instancia correspondiente. Si no se encuentra, se devuelve el valor predeterminado para el tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <seealso cref="IServiceCollection"/>
    public static T GetSingletonInstance<T>( this IServiceCollection services ) {
        var descriptor = services.FirstOrDefault( t => t.ServiceType == typeof( T ) && t.Lifetime == ServiceLifetime.Singleton );
        if( descriptor == null )
            return default;
        if( descriptor.ImplementationInstance != null )
            return (T)descriptor.ImplementationInstance;
        if( descriptor.ImplementationFactory != null )
            return (T)descriptor.ImplementationFactory.Invoke( null );
        return default;
    }
}