namespace Util.Logging; 

/// <summary>
/// Clase que implementa la interfaz <see cref="ILogFactory"/> para la creación de instancias de registro.
/// </summary>
/// <remarks>
/// Esta clase es responsable de proporcionar métodos para crear objetos de registro que pueden ser utilizados
/// para registrar información, advertencias y errores en la aplicación.
/// </remarks>
public class LogFactory : ILogFactory {
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LogFactory"/>.
    /// </summary>
    /// <param name="factory">La fábrica de registradores que se utilizará para crear instancias de <see cref="ILogger"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="factory"/> es <c>null</c>.</exception>
    public LogFactory( ILoggerFactory factory ) {
        _loggerFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Crea una instancia de registro de log para la categoría especificada.
    /// </summary>
    /// <param name="categoryName">El nombre de la categoría para la cual se desea crear el log.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el log creado.</returns>
    public ILog CreateLog( string categoryName ) {
        var logger = _loggerFactory.CreateLogger( categoryName );
        var wrapper = new LoggerWrapper( logger );
        return new Log( wrapper );
    }

    /// <summary>
    /// Crea una instancia de registro (log) para el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo para el cual se desea crear el registro.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el registro creado.</returns>
    /// <remarks>
    /// Este método utiliza una fábrica de loggers para crear un logger específico 
    /// para el tipo proporcionado y lo envuelve en un objeto <see cref="LoggerWrapper"/> 
    /// antes de devolver una nueva instancia de <see cref="Log"/>.
    /// </remarks>
    public ILog CreateLog( Type type ) {
        var logger = _loggerFactory.CreateLogger( type );
        var wrapper = new LoggerWrapper( logger );
        return new Log( wrapper );
    }
}