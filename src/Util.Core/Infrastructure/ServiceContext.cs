using Util.Reflections;

namespace Util.Infrastructure;

/// <summary>
/// Representa el contexto de un servicio, que puede contener información relevante 
/// para la ejecución de operaciones dentro de un servicio.
/// </summary>
public class ServiceContext {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ServiceContext"/>.
    /// </summary>
    /// <param name="hostBuilder">El constructor de host que se utilizará para configurar el servicio.</param>
    /// <param name="assemblyFinder">El buscador de ensamblados que se utilizará para localizar ensamblados.</param>
    /// <param name="typeFinder">El buscador de tipos que se utilizará para localizar tipos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si alguno de los parámetros es nulo.</exception>
    public ServiceContext( IHostBuilder hostBuilder, IAssemblyFinder assemblyFinder, ITypeFinder typeFinder ) {
        HostBuilder = hostBuilder ?? throw new ArgumentNullException( nameof( hostBuilder ) );
        AssemblyFinder = assemblyFinder ?? throw new ArgumentNullException( nameof( assemblyFinder ) );
        TypeFinder = typeFinder ?? throw new ArgumentNullException( nameof( typeFinder ) );
    }

    /// <summary>
    /// Obtiene el constructor de host que se utiliza para configurar y crear una instancia de <see cref="IHost"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor permite definir los servicios, configuraciones y otros aspectos del host
    /// que se utilizarán en la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IHostBuilder"/>.
    /// </value>
    public IHostBuilder HostBuilder { get; }

    /// <summary>
    /// Obtiene el buscador de ensamblados.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IAssemblyFinder"/> 
    /// que permite buscar y localizar ensamblados en la aplicación.
    /// </value>
    public IAssemblyFinder AssemblyFinder { get; }

    /// <summary>
    /// Obtiene una instancia de <see cref="ITypeFinder"/> que se utiliza para buscar tipos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un buscador de tipos que puede ser utilizado para localizar
    /// tipos en el ensamblado actual o en otros ensamblados según la configuración.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ITypeFinder"/>.
    /// </value>
    public ITypeFinder TypeFinder { get; }
}