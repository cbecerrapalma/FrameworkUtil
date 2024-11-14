using Util.Infrastructure;

namespace Util.Applications.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registro de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios para Web API en la configuración del registrador de servicios.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="ServiceRegistrarConfig"/> 
    /// para permitir la habilitación del registrador de servicios específico para Web API.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="WebApiServiceRegistrar"/>
    public static ServiceRegistrarConfig EnableWebApiServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( WebApiServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios para la API web.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada.
    /// </returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="ServiceRegistrarConfig"/> 
    /// y permite desactivar el registrador de servicios específico para la API web.
    /// </remarks>
    public static ServiceRegistrarConfig DisableWebApiServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( WebApiServiceRegistrar.ServiceName );
        return config;
    }
}