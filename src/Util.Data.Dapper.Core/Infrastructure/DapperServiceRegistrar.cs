using Util.Data.Dapper.TypeHandlers;
using Util.Infrastructure;

namespace Util.Data.Dapper.Infrastructure;

/// <summary>
/// Clase que implementa la interfaz <see cref="IServiceRegistrar"/> 
/// para registrar servicios relacionados con Dapper en el contenedor de inyección de dependencias.
/// </summary>
public class DapperServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de Dapper.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre del servicio.
    /// </value>
    public static string ServiceName => "Util.Data.Dapper.Infrastructure.DapperServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador del pedido.
    /// </summary>
    /// <value>
    /// El identificador del pedido, que es un número entero.
    /// </value>
    public int OrderId => 810;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina consultando la configuración del registrador de servicios
    /// utilizando el nombre del servicio especificado.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled( ServiceName );

    /// <summary>
    /// Registra un manejador de tipo personalizado para propiedades adicionales en el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que se utiliza para el registro del manejador.</param>
    /// <returns>Devuelve null, ya que este método no tiene un resultado que retornar.</returns>
    public Action Register( ServiceContext serviceContext ) {
        SqlMapper.AddTypeHandler( new ExtraPropertiesTypeHandler() );
        return null;
    }
}