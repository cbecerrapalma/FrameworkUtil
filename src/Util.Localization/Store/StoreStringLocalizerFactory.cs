namespace Util.Localization.Store;

/// <summary>
/// Clase que implementa la interfaz <see cref="IStringLocalizerFactory"/> 
/// para crear instancias de localizadores de cadenas.
/// </summary>
/// <remarks>
/// Esta clase es responsable de la creación de localizadores de cadenas 
/// que permiten la traducción de textos en la aplicación, facilitando 
/// la internacionalización y localización.
/// </remarks>
public class StoreStringLocalizerFactory : IStringLocalizerFactory {
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILocalizedStore _store;
    private readonly IMemoryCache _cache;
    private readonly IOptions<LocalizationOptions> _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StoreStringLocalizerFactory"/>.
    /// </summary>
    /// <param name="options">Las opciones de localización que se utilizarán para la configuración.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de <see cref="ILogger"/>.</param>
    /// <param name="store">La tienda de localización que proporciona los recursos localizados.</param>
    /// <param name="cache">El caché en memoria utilizado para almacenar los recursos localizados.</param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="loggerFactory"/>, <paramref name="store"/> o <paramref name="cache"/> son nulos.
    /// </exception>
    public StoreStringLocalizerFactory( IOptions<LocalizationOptions> options, ILoggerFactory loggerFactory, ILocalizedStore store, IMemoryCache cache ) {
        _options = options;
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException( nameof( loggerFactory ) );
        _store = store ?? throw new ArgumentNullException( nameof( store ) );
        _cache = cache ?? throw new ArgumentNullException( nameof( cache ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IStringLocalizer"/> utilizando el tipo de fuente de recursos especificado.
    /// </summary>
    /// <param name="resourceSource">
    /// El tipo que actúa como fuente de recursos para la localización. No puede ser nulo.
    /// </param>
    /// <returns>
    /// Una instancia de <see cref="IStringLocalizer"/> que se utiliza para la localización de cadenas.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="resourceSource"/> es nulo.
    /// </exception>
    /// <remarks>
    /// Este método utiliza el atributo <see cref="LocalizedTypeAttribute"/> para obtener el tipo de recurso
    /// asociado al tipo de fuente de recursos proporcionado.
    /// </remarks>
    /// <seealso cref="IStringLocalizer"/>
    /// <seealso cref="StoreStringLocalizer"/>
    public IStringLocalizer Create( Type resourceSource ) {
        resourceSource.CheckNull( nameof( resourceSource ) );
        var type = LocalizedTypeAttribute.GetResourceType( resourceSource );
        return new StoreStringLocalizer( _loggerFactory.CreateLogger<StoreStringLocalizer>(), _cache, _store, type, _options );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IStringLocalizer"/> utilizando el nombre base y la ubicación especificados.
    /// </summary>
    /// <param name="baseName">El nombre base que se utilizará para la localización.</param>
    /// <param name="location">La ubicación donde se encuentran los recursos de localización.</param>
    /// <returns>Una instancia de <see cref="IStringLocalizer"/> configurada con los parámetros proporcionados.</returns>
    /// <remarks>
    /// Este método utiliza un logger, un caché y un almacén para crear el localizador de cadenas.
    /// </remarks>
    /// <seealso cref="IStringLocalizer"/>
    public IStringLocalizer Create( string baseName, string location ) {
        return new StoreStringLocalizer( _loggerFactory.CreateLogger<StoreStringLocalizer>(), _cache, _store, baseName, _options );
    }
}