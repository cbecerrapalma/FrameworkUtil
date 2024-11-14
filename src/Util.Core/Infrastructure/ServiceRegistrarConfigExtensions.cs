namespace Util.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios de dependencia en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada con el registrador de servicios de dependencia habilitado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de <see cref="ServiceRegistrarConfig"/> que permite habilitar el 
    /// <see cref="DependencyServiceRegistrar"/> para su uso en la aplicación.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="DependencyServiceRegistrar"/>
    public static ServiceRegistrarConfig EnableDependencyServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( DependencyServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de dependencia en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración actualizada después de desactivar el registrador de servicios de dependencia.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de <see cref="ServiceRegistrarConfig"/> y permite desactivar 
    /// el <see cref="DependencyServiceRegistrar"/> para evitar que se registre en el contenedor de servicios.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="DependencyServiceRegistrar"/>
    public static ServiceRegistrarConfig DisableDependencyServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( DependencyServiceRegistrar.ServiceName );
        return config;
    }
}