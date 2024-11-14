using Util.Infrastructure;

namespace Util.Templates.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios de Handlebars en la configuración del registrador de servicios.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios a la que se le aplicará la habilitación.</param>
    /// <returns>La configuración del registrador de servicios actualizada.</returns>
    public static ServiceRegistrarConfig EnableHandlebarsServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( HandlebarsServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de Handlebars.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión que permite desactivar el registrador de servicios de Handlebars
    /// en la configuración proporcionada.
    /// </remarks>
    /// <seealso cref="ServiceRegistrarConfig"/>
    public static ServiceRegistrarConfig DisableHandlebarsServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( HandlebarsServiceRegistrar.ServiceName );
        return config;
    }
}