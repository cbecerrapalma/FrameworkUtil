using Util.Infrastructure;

namespace Util.ObjectMapping.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registro de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios de AutoMapper en la configuración del registrador de servicios.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    /// <remarks>
    /// Este método es una extensión que permite habilitar fácilmente el registrador de servicios de AutoMapper
    /// en la configuración existente, facilitando la integración de AutoMapper en la aplicación.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="AutoMapperServiceRegistrar"/>
    public static ServiceRegistrarConfig EnableAutoMapperServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( AutoMapperServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de AutoMapper en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada después de desactivar AutoMapper.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de <see cref="ServiceRegistrarConfig"/> y permite desactivar 
    /// el registrador de servicios de AutoMapper de manera fluida.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    public static ServiceRegistrarConfig DisableAutoMapperServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( AutoMapperServiceRegistrar.ServiceName );
        return config;
    }
}