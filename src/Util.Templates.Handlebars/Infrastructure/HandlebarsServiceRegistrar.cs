using Util.Infrastructure;
using Util.Templates.HandlebarsDotNet;

namespace Util.Templates.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios para Handlebars.
/// </summary>
/// <remarks>
/// Esta clase es responsable de registrar los servicios necesarios para el funcionamiento de Handlebars
/// en la aplicación. Implementa la interfaz <see cref="IServiceRegistrar"/>.
/// </remarks>
public class HandlebarsServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de plantillas de Handlebars.
    /// </summary>
    /// <remarks>
    /// Este servicio se utiliza para registrar y gestionar plantillas de Handlebars en la infraestructura de la aplicación.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el nombre del servicio.
    /// </returns>
    public static string ServiceName => "Util.Templates.Infrastructure.HandlebarsServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador del pedido.
    /// </summary>
    /// <value>
    /// El identificador del pedido, que es un valor entero fijo de 620.
    /// </value>
    public int OrderId => 620;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina a través de la configuración del registrador de servicios,
    /// utilizando el nombre del servicio actual.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled( ServiceName );

    /// <summary>
    /// Registra los servicios necesarios para el motor de plantillas Handlebars.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene la configuración del host.</param>
    /// <returns>
    /// Un objeto <see cref="Action"/> que representa la acción de registro. 
    /// En este caso, siempre devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método configura los servicios de inyección de dependencias para utilizar 
    /// el motor de plantillas Handlebars como una implementación de <see cref="ITemplateEngine"/> 
    /// y <see cref="IHandlebarsTemplateEngine"/>.
    /// </remarks>
    public Action Register( ServiceContext serviceContext ) {
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            services.AddSingleton<ITemplateEngine, HandlebarsTemplateEngine>();
            services.AddSingleton<IHandlebarsTemplateEngine, HandlebarsTemplateEngine>();
        } );
        return null;
    }
}