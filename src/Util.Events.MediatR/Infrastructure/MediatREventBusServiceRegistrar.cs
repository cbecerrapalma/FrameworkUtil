namespace Util.Events.Infrastructure;

/// <summary>
/// Clase que implementa el registro de servicios para el bus de eventos MediatR.
/// </summary>
/// <remarks>
/// Esta clase se encarga de registrar los servicios necesarios para el funcionamiento
/// del bus de eventos MediatR en la aplicación. Implementa la interfaz <see cref="IServiceRegistrar"/>.
/// </remarks>
public class MediatREventBusServiceRegistrar : IServiceRegistrar {
    /// <summary>
    /// Obtiene el nombre del servicio de eventos local utilizado en la infraestructura de MediatR.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre del servicio.
    /// </value>
    public static string ServiceName => "Util.Events.MediatR.Infrastructure.LocalEventBusServiceRegistrar";

    /// <summary>
    /// Obtiene el identificador del pedido.
    /// </summary>
    /// <value>
    /// El identificador del pedido, que es un valor entero fijo de 511.
    /// </value>
    public int OrderId => 511;

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
    /// Registra los servicios necesarios en el contexto de servicio proporcionado.
    /// </summary>
    /// <param name="serviceContext">El contexto de servicio que contiene la configuración del host.</param>
    /// <returns>Devuelve null, ya que este método no tiene un valor de retorno significativo.</returns>
    public Action Register( ServiceContext serviceContext ) {
        serviceContext.HostBuilder.ConfigureServices( ( context, services ) => {
            RegisterMediatR( services, serviceContext.AssemblyFinder );
        } );
        return null;
    }

    /// <summary>
    /// Registra los servicios de MediatR en el contenedor de inyección de dependencias.
    /// </summary>
    /// <param name="services">La colección de servicios donde se registrarán los servicios de MediatR.</param>
    /// <param name="finder">Una instancia de <see cref="IAssemblyFinder"/> que se utiliza para encontrar ensamblados.</param>
    /// <remarks>
    /// Este método solo registrará los servicios si la opción <c>MediatROptions.IsScan</c> es verdadera.
    /// </remarks>
    private void RegisterMediatR( IServiceCollection services, IAssemblyFinder finder ) {
        if ( MediatROptions.IsScan == false )
            return;
        services.AddMediatR( t => t.RegisterServicesFromAssemblies( finder.Find().ToArray() ) );
    }
}