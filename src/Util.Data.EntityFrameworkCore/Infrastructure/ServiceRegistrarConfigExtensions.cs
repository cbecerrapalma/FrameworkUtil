using Util.Infrastructure;

namespace Util.Data.EntityFrameworkCore.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios de Entity Framework en la configuración del registrador de servicios.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    /// <remarks>
    /// Este método es una extensión que permite habilitar el registrador de servicios de Entity Framework 
    /// de manera fluida, permitiendo encadenar otras configuraciones si es necesario.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="EntityFrameworkServiceRegistrar"/>
    public static ServiceRegistrarConfig EnableEntityFrameworkServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( EntityFrameworkServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de Entity Framework en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración modificada después de desactivar el registrador de servicios de Entity Framework.</returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="ServiceRegistrarConfig"/> 
    /// y permite desactivar el servicio de Entity Framework de manera sencilla.
    /// </remarks>
    public static ServiceRegistrarConfig DisableEntityFrameworkServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( EntityFrameworkServiceRegistrar.ServiceName );
        return config;
    }
}