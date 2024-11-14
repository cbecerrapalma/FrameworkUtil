namespace Util.Localization.Json;

/// <summary>
/// Proporciona una implementación de <see cref="IStringLocalizerFactory"/> 
/// que crea instancias de <see cref="IStringLocalizer"/> para la localización de cadenas 
/// utilizando archivos JSON como fuente de datos.
/// </summary>
public class JsonStringLocalizerFactory : IStringLocalizerFactory {
    private readonly string _rootPath;
    private readonly IPathResolver _pathResolver;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMemoryCache _cache;
    private readonly IOptions<JsonLocalizationOptions> _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JsonStringLocalizerFactory"/>.
    /// </summary>
    /// <param name="options">Opciones de localización en formato JSON.</param>
    /// <param name="pathResolver">Resolver de rutas para localizar los archivos de recursos.</param>
    /// <param name="loggerFactory">Fábrica de registros para generar logs.</param>
    /// <param name="cache">Cache opcional para almacenar los recursos localizados.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="pathResolver"/> o <paramref name="loggerFactory"/> son nulos.</exception>
    public JsonStringLocalizerFactory( IOptions<JsonLocalizationOptions> options, IPathResolver pathResolver, ILoggerFactory loggerFactory, IMemoryCache cache = null ) {
        _options = options;
        _rootPath = options.Value.ResourcesPath;
        _pathResolver = pathResolver ?? throw new ArgumentNullException( nameof( pathResolver ) );
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException( nameof(loggerFactory) );
        _cache = cache;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IStringLocalizer"/> utilizando el tipo de recurso especificado.
    /// </summary>
    /// <param name="resourceSource">El tipo que se utilizará como fuente de recursos.</param>
    /// <returns>Una instancia de <see cref="IStringLocalizer"/> que se puede utilizar para la localización de cadenas.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="resourceSource"/> es nulo.</exception>
    /// <remarks>
    /// Este método utiliza un resolvedor de rutas para determinar la ruta raíz de los recursos y el nombre base de los recursos
    /// a partir del ensamblado del tipo proporcionado. Luego, crea una nueva instancia de <see cref="JsonStringLocalizer"/>.
    /// </remarks>
    /// <seealso cref="IStringLocalizer"/>
    /// <seealso cref="JsonStringLocalizer"/>
    public IStringLocalizer Create( Type resourceSource ) {
        resourceSource.CheckNull( nameof( resourceSource ) );
        var assembly = resourceSource.Assembly;
        var rootPath = _pathResolver.GetResourcesRootPath( assembly, _rootPath );
        var baseName = _pathResolver.GetResourcesBaseName( assembly, resourceSource.FullName );
        return new JsonStringLocalizer( _pathResolver, rootPath, baseName, _loggerFactory.CreateLogger<JsonStringLocalizer>(), _cache, _options );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IStringLocalizer"/> utilizando el nombre base y la ubicación especificados.
    /// </summary>
    /// <param name="baseName">El nombre base para la localización de cadenas.</param>
    /// <param name="location">La ubicación del ensamblado que contiene los recursos de localización. Si es <c>null</c>, se utiliza el nombre del ensamblado actual.</param>
    /// <returns>Una instancia de <see cref="IStringLocalizer"/> que se puede utilizar para obtener cadenas localizadas.</returns>
    /// <remarks>
    /// Este método carga el ensamblado especificado por <paramref name="location"/> y obtiene la ruta raíz de los recursos de localización 
    /// utilizando el <see cref="_pathResolver"/>. Luego, crea y devuelve una nueva instancia de <see cref="JsonStringLocalizer"/>.
    /// </remarks>
    /// <seealso cref="IStringLocalizer"/>
    /// <seealso cref="JsonStringLocalizer"/>
    public IStringLocalizer Create( string baseName, string location ) {
        location ??= new AssemblyName( GetType().Assembly.FullName ).Name;
        var assemblyName = new AssemblyName( location );
        var assembly = Assembly.Load( assemblyName );
        var rootPath = _pathResolver.GetResourcesRootPath( assembly, _rootPath );
        return new JsonStringLocalizer( _pathResolver, rootPath, baseName, _loggerFactory.CreateLogger<JsonStringLocalizer>(), _cache, _options );
    }
}