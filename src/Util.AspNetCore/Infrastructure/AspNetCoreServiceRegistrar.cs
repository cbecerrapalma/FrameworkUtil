using Util.Helpers;
using Util.Http;
using Util.SystemTextJson;

namespace Util.Infrastructure; 

/// <summary>
/// Clase que implementa el registro de servicios para una aplicación ASP.NET Core.
/// </summary>
/// <remarks>
/// Esta clase es responsable de registrar los servicios necesarios en el contenedor de inyección de dependencias
/// de ASP.NET Core, permitiendo que otros componentes de la aplicación puedan utilizar estos servicios.
/// </remarks>
public class AspNetCoreServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de infraestructura de ASP.NET Core.
    /// </summary>
    /// <value>
    /// El nombre del servicio, que es "Util.Infrastructure.AspNetCoreServiceRegistrar".
    /// </value>
    public static string ServiceName => "Util.Infrastructure.AspNetCoreServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador de la orden.
    /// </summary>
    /// <value>
    /// El identificador de la orden, que es un valor entero fijo de 200.
    /// </value>
    public int OrderId => 200;

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
    /// Registra los servicios necesarios en el contexto del servicio.
    /// </summary>
    /// <param name="serviceContext">El contexto del servicio que contiene la configuración del host.</param>
    /// <returns>Devuelve una acción que representa la operación de registro.</returns>
    /// <remarks>
    /// Este método configura los servicios necesarios para la aplicación, incluyendo el acceso al contexto HTTP,
    /// la adición de un cliente HTTP, y la configuración de opciones JSON.
    /// </remarks>
    public Action Register( ServiceContext serviceContext ) {
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            RegisterHttpContextAccessor( services );
            services.AddHttpClient();
            RegisterServiceLocator();
            RegisterHttpClient( services );
            ConfigJsonOptions( services );
        } );
        return null;
    }

    /// <summary>
    /// Registra el acceso al contexto HTTP en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el acceso al contexto HTTP.</param>
    /// <remarks>
    /// Este método crea una instancia de <see cref="HttpContextAccessor"/> y la registra como un servicio singleton.
    /// También asigna la instancia a la propiedad estática <see cref="Web.HttpContextAccessor"/> para su uso global.
    /// </remarks>
    private void RegisterHttpContextAccessor( IServiceCollection services ) {
        var httpContextAccessor = new HttpContextAccessor();
        services.TryAddSingleton<IHttpContextAccessor>( httpContextAccessor );
        Web.HttpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Registra el localizador de servicios en el contenedor de inversión de control (IoC).
    /// </summary>
    /// <remarks>
    /// Este método establece una acción que proporciona el proveedor de servicios de la aplicación web.
    /// Se utiliza para resolver las dependencias de los servicios registrados en el contenedor IoC.
    /// </remarks>
    private void RegisterServiceLocator() {
        Ioc.SetServiceProviderAction( () => Web.ServiceProvider );
    }

    /// <summary>
    /// Registra el servicio <see cref="IHttpClient"/> en el contenedor de servicios.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrará el cliente HTTP.</param>
    private void RegisterHttpClient( IServiceCollection services ) {
        services.TryAddSingleton<IHttpClient, HttpClientService>();
    }

    /// <summary>
    /// Configura las opciones de JSON para la aplicación.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registran las opciones de JSON.</param>
    /// <remarks>
    /// Este método agrega convertidores personalizados para manejar la serialización y deserialización de objetos 
    /// <see cref="DateTime"/> y <see cref="Nullable{DateTime}"/> en formato JSON.
    /// </remarks>
    private void ConfigJsonOptions( IServiceCollection services ) {
        services.Configure( ( Microsoft.AspNetCore.Mvc.JsonOptions options ) => {
            options.JsonSerializerOptions.Converters.Add( new DateTimeJsonConverter() );
            options.JsonSerializerOptions.Converters.Add( new NullableDateTimeJsonConverter() );
        } );
    }
}