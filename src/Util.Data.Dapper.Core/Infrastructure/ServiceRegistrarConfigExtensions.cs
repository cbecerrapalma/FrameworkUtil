using Util.Infrastructure;

namespace Util.Data.Dapper.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registro de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios Dapper en la configuración del registrador de servicios.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    /// <remarks>
    /// Este método es una extensión que permite habilitar el registrador de servicios Dapper
    /// de manera fluida, facilitando la configuración de servicios en la aplicación.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="DapperServiceRegistrar"/>
    public static ServiceRegistrarConfig EnableDapperServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( DapperServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios Dapper en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada con Dapper desactivado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite desactivar de manera sencilla el registrador de servicios Dapper
    /// sin necesidad de modificar la configuración de forma manual.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    /// <seealso cref="DapperServiceRegistrar"/>
    public static ServiceRegistrarConfig DisableDapperServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( DapperServiceRegistrar.ServiceName );
        return config;
    }
}