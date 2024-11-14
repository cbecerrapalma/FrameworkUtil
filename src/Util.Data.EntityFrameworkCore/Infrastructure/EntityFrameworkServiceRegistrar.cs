using Util.Data.EntityFrameworkCore.Filters;

namespace Util.Data.EntityFrameworkCore.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios para Entity Framework.
/// </summary>
/// <remarks>
/// Esta clase se encarga de registrar los servicios necesarios para el funcionamiento de Entity Framework 
/// dentro de la aplicación, permitiendo la inyección de dependencias y facilitando el acceso a la base de datos.
/// </remarks>
public class EntityFrameworkServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// Este servicio se utiliza para registrar la infraestructura de Entity Framework en la aplicación.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el nombre del servicio.
    /// </returns>
    public static string ServiceName => "Util.Data.EntityFrameworkCore.Infrastructure.EntityFrameworkServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador de la orden.
    /// </summary>
    /// <value>
    /// El identificador de la orden, que es un entero constante con valor 710.
    /// </value>
    public int OrderId => 710;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina a través de la configuración del registrador de servicios,
    /// verificando si el servicio especificado por <see cref="ServiceName"/> está habilitado.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled( ServiceName );

    /// <summary>
    /// Registra los tipos de filtros en el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio donde se registran los filtros.</param>
    /// <returns>Devuelve null, ya que este método no tiene un resultado significativo.</returns>
    public Action Register( ServiceContext serviceContext ) {
        FilterManager.AddFilterType<IDelete>();
        FilterManager.AddFilterType<ITenant>();
        return null;
    }
}