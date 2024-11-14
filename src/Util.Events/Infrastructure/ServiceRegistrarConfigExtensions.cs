using Util.Infrastructure;

namespace Util.Events.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registro de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios para el bus de eventos local.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios modificada.</returns>
    public static ServiceRegistrarConfig EnableLocalEventBusServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( LocalEventBusServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios para el bus de eventos local.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="ServiceRegistrarConfig"/> 
    /// y permite desactivar el registrador de servicios específico para el bus de eventos local.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    public static ServiceRegistrarConfig DisableLocalEventBusServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( LocalEventBusServiceRegistrar.ServiceName );
        return config;
    }
}