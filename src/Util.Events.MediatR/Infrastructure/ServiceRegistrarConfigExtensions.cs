namespace Util.Events.Infrastructure;

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios para el bus de eventos MediatR.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios modificada con el registrador de MediatR habilitado.
    /// </returns>
    public static ServiceRegistrarConfig EnableMediatREventBusServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( MediatREventBusServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de MediatR para el bus de eventos.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    public static ServiceRegistrarConfig DisableMediatREventBusServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( MediatREventBusServiceRegistrar.ServiceName );
        return config;
    }
}