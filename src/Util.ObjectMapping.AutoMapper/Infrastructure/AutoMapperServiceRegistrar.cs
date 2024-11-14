using Util.Helpers;
using Util.Infrastructure;

namespace Util.ObjectMapping.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios para AutoMapper.
/// </summary>
/// <remarks>
/// Esta clase se encarga de registrar los perfiles de AutoMapper en el contenedor de servicios.
/// </remarks>
public class AutoMapperServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de mapeo de objetos.
    /// </summary>
    /// <remarks>
    /// Este servicio se utiliza para registrar configuraciones de AutoMapper en la infraestructura de la aplicación.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el nombre del servicio.
    /// </returns>
    public static string ServiceName => "Util.ObjectMapping.Infrastructure.AutoMapperServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador del pedido.
    /// </summary>
    /// <value>
    /// El identificador del pedido, que es un valor entero fijo de 300.
    /// </value>
    public int OrderId => 300;

    /// <summary>
    /// Obtiene un valor que indica si el servicio está habilitado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el servicio está habilitado; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina consultando la configuración del registrador de servicios 
    /// mediante el nombre del servicio especificado.
    /// </remarks>
    public bool Enabled => ServiceRegistrarConfig.IsEnabled( ServiceName );

    /// <summary>
    /// Registra la configuración del mapeador de objetos utilizando el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene información sobre los tipos y la configuración del host.</param>
    /// <returns>Devuelve null, ya que este método no tiene un valor de retorno significativo.</returns>
    /// <remarks>
    /// Este método busca todas las implementaciones de <see cref="IAutoMapperConfig"/> utilizando el 
    /// <see cref="TypeFinder"/> del contexto del servicio. Luego, crea instancias de estas configuraciones 
    /// y las aplica a una expresión de configuración de mapeo. Finalmente, se establece el mapeador 
    /// de objetos y se registra como un servicio singleton en el contenedor de servicios.
    /// </remarks>
    /// <seealso cref="IAutoMapperConfig"/>
    /// <seealso cref="MapperConfigurationExpression"/>
    /// <seealso cref="ObjectMapper"/>
    /// <seealso cref="ObjectMapperExtensions"/>
    public Action Register( ServiceContext serviceContext ) {
        var types = serviceContext.TypeFinder.Find<IAutoMapperConfig>();
        var instances = types.Select( type => Reflection.CreateInstance<IAutoMapperConfig>( type ) ).ToList();
        var expression = new MapperConfigurationExpression();
        instances.ForEach( t => t.Config( expression ) );
        var mapper = new ObjectMapper( expression );
        ObjectMapperExtensions.SetMapper( mapper );
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            services.AddSingleton<IObjectMapper>( mapper );
        } );
        return null;
    }
}