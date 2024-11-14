using Util.Localization;
using Util.Logging;
using Util.Sessions;

namespace Util.Domain.Services; 

/// <summary>
/// Clase base abstracta para servicios de dominio.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación base para los servicios de dominio,
/// permitiendo la reutilización de código común y la definición de comportamientos
/// que serán compartidos por todos los servicios de dominio derivados.
/// </remarks>
public abstract class DomainServiceBase : IDomainService {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainServiceBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para obtener las dependencias necesarias.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor se encarga de inicializar las propiedades del servicio, incluyendo la sesión, el registro y la localización de cadenas.
    /// Si el <paramref name="serviceProvider"/> no proporciona una implementación para <see cref="ISession"/>, se utilizará <see cref="NullSession.Instance"/>.
    /// De manera similar, si no se proporciona un <see cref="ILogFactory"/>, se utilizará <see cref="NullLog.Instance"/>.
    /// Para la localización de cadenas, se utilizará <see cref="NullStringLocalizer.Instance"/> si no se encuentra una implementación.
    /// </remarks>
    protected DomainServiceBase( IServiceProvider serviceProvider ) {
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException( nameof( serviceProvider ) );
        Session = serviceProvider.GetService<ISession>() ?? NullSession.Instance;
        var logFactory = serviceProvider.GetService<ILogFactory>();
        Log = logFactory?.CreateLog( GetType() ) ?? NullLog.Instance;
        L = serviceProvider.GetService<IStringLocalizer>() ?? NullStringLocalizer.Instance;
    }

    /// <summary>
    /// Obtiene el proveedor de servicios utilizado para la inyección de dependencias.
    /// </summary>
    /// <remarks>
    /// Este miembro es protegido y solo puede ser accedido desde la clase que lo contiene 
    /// o desde clases derivadas. Proporciona acceso a las instancias de servicios registrados 
    /// en el contenedor de inyección de dependencias.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa <see cref="IServiceProvider"/> que permite resolver 
    /// instancias de servicios.
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Obtiene o establece la sesión actual.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="ISession"/> que representa la sesión activa.
    /// </value>
    protected ISession Session { get; set; }

    /// <summary>
    /// Obtiene el registro de log asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una instancia de un objeto que implementa la interfaz <see cref="ILog"/>.
    /// Se utiliza para registrar información, advertencias y errores en el sistema.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ILog"/>.
    /// </value>
    protected ILog Log { get; }

    /// <summary>
    /// Obtiene o establece el localizador de cadenas utilizado para la localización de textos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a un objeto que proporciona cadenas localizadas 
    /// en función de la cultura actual del usuario. Es útil para la internacionalización 
    /// de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IStringLocalizer"/>.
    /// </value>
    protected IStringLocalizer L { get; set; }
}