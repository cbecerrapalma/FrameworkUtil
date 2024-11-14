using Util.Infrastructure;

namespace Util.Templates.Infrastructure; 

/// <summary>
/// Proporciona métodos de extensión para la configuración del registrador de servicios.
/// </summary>
public static class ServiceRegistrarConfigExtensions {
    /// <summary>
    /// Habilita el registrador de servicios de Razor en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>La configuración del registrador de servicios modificada.</returns>
    public static ServiceRegistrarConfig EnableRazorServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Enable( RazorServiceRegistrar.ServiceName );
        return config;
    }

    /// <summary>
    /// Desactiva el registrador de servicios de Razor en la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración del registrador de servicios que se va a modificar.</param>
    /// <returns>
    /// La configuración del registrador de servicios actualizada, con el registrador de servicios de Razor desactivado.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="ServiceRegistrarConfig"/> 
    /// y permite desactivar el servicio de Razor de manera fluida.
    /// </remarks>
    public static ServiceRegistrarConfig DisableRazorServiceRegistrar( this ServiceRegistrarConfig config ) {
        ServiceRegistrarConfig.Disable( RazorServiceRegistrar.ServiceName );
        return config;
    }
}