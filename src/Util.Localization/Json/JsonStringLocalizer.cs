using Util.Localization.Base;

namespace Util.Localization.Json;

/// <summary>
/// Proporciona localización de cadenas utilizando un archivo JSON como fuente de datos.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="StringLocalizerBase"/> y se encarga de cargar las cadenas localizadas desde un archivo JSON.
/// </remarks>
public class JsonStringLocalizer : StringLocalizerBase {
    private readonly IPathResolver _pathResolver;
    private readonly string _resourcesRootPath;
    private readonly string _resourcesBaseName;
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>> _resourcesCache;
    private readonly string _searchedLocation;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JsonStringLocalizer"/>.
    /// </summary>
    /// <param name="pathResolver">El resolutor de rutas utilizado para localizar los recursos.</param>
    /// <param name="resourcesRootPath">La ruta raíz donde se encuentran los recursos.</param>
    /// <param name="resourcesBaseName">El nombre base de los recursos que se utilizarán para la localización.</param>
    /// <param name="logger">El registrador utilizado para registrar eventos y errores. Si es <c>null</c>, se utilizará un registrador nulo.</param>
    /// <param name="cache">La caché en memoria utilizada para almacenar los recursos localizados.</param>
    /// <param name="options">Las opciones de localización en formato JSON.</param>
    /// <remarks>
    /// Este constructor configura el localizador de cadenas JSON con las rutas y opciones especificadas.
    /// Se inicializa un diccionario concurrente para almacenar en caché los recursos localizados.
    /// </remarks>
    public JsonStringLocalizer( IPathResolver pathResolver, string resourcesRootPath, string resourcesBaseName, ILogger logger, 
        IMemoryCache cache, IOptions<JsonLocalizationOptions> options ) 
        : base( cache, resourcesBaseName, options ) {
        _pathResolver = pathResolver;
        _resourcesRootPath = resourcesRootPath;
        _resourcesBaseName = resourcesBaseName;
        _logger = logger ?? NullLogger.Instance;
        _resourcesCache = new ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>>();
        _searchedLocation = $"{resourcesRootPath}.{resourcesBaseName}";
    }

    /// <summary>
    /// Obtiene una cadena localizada basada en la cultura y el nombre proporcionados.
    /// </summary>
    /// <param name="culture">La información de cultura que se utilizará para la localización.</param>
    /// <param name="name">El nombre de la cadena que se desea localizar.</param>
    /// <returns>
    /// Un objeto <see cref="LocalizedString"/> que contiene la cadena localizada. 
    /// Si no se encuentra un valor, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método busca primero el valor en los recursos especificados por el nombre base de recursos.
    /// Si no se encuentra, intenta obtener el valor de los recursos predeterminados.
    /// Si el valor encontrado está vacío, se devuelve un <see cref="LocalizedString"/> con una cadena vacía.
    /// </remarks>
    protected override LocalizedString GetLocalizedString( CultureInfo culture, string name ) {
        var value = GetValue( culture, name, _resourcesBaseName ) ?? GetValue( culture, name, null );
        if ( value.IsEmpty() )
            return new LocalizedString( name, string.Empty, true, null );
        return new LocalizedString( name, value, false, _searchedLocation );
    }

    /// <summary>
    /// Obtiene el valor de un recurso específico basado en la cultura, el nombre y el tipo.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para buscar el recurso.</param>
    /// <param name="name">El nombre del recurso que se desea obtener.</param>
    /// <param name="type">El tipo de recurso que se está buscando.</param>
    /// <returns>
    /// El valor del recurso correspondiente al nombre especificado, o null si no se encuentra.
    /// </returns>
    /// <remarks>
    /// Este método busca en la caché de recursos y registra la ubicación de búsqueda.
    /// Si no se encuentran recursos para la cultura y el tipo especificados, se devuelve null.
    /// </remarks>
    protected override string GetValue( CultureInfo culture, string name, string type ) {
        var resources = GetResourcesByCache( culture, type );
        if ( resources == null )
            return null;
        var resource = resources.SingleOrDefault( t => t.Key == name );
        var result = resource.Value;
        _logger.SearchedLocation( name, _searchedLocation, culture );
        return result;
    }

    /// <summary>
    /// Obtiene los recursos desde la caché según la cultura y el nombre base de los recursos.
    /// </summary>
    /// <param name="culture">La cultura para la cual se desean obtener los recursos.</param>
    /// <param name="resourcesBaseName">El nombre base de los recursos que se desean obtener.</param>
    /// <returns>
    /// Una colección de pares clave-valor que representan los recursos obtenidos.
    /// </returns>
    /// <remarks>
    /// Si los recursos ya están en la caché, se devolverán directamente desde allí.
    /// De lo contrario, se cargarán utilizando el método <see cref="GetResources(CultureInfo, string)"/>.
    /// </remarks>
    protected virtual IEnumerable<KeyValuePair<string, string>> GetResourcesByCache(CultureInfo culture, string resourcesBaseName) {
        return _resourcesCache.GetOrAdd($"{culture.Name}_{resourcesBaseName}", _ => GetResources(culture, resourcesBaseName));
    }

    /// <summary>
    /// Obtiene los recursos de un archivo JSON basado en el nombre de los recursos y la cultura especificada.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para localizar el archivo de recursos.</param>
    /// <param name="resourcesBaseName">El nombre base de los recursos que se desean cargar.</param>
    /// <returns>
    /// Una colección de pares clave-valor que representan los recursos cargados desde el archivo JSON,
    /// o <c>null</c> si no se encuentra el archivo de recursos.
    /// </returns>
    /// <remarks>
    /// Este método intenta localizar un archivo JSON de recursos en la ruta especificada. Si el archivo no se encuentra,
    /// intenta buscar un archivo alternativo utilizando el nombre de recursos modificado.
    /// </remarks>
    /// <seealso cref="ConfigurationBuilder"/>
    protected virtual IEnumerable<KeyValuePair<string, string>> GetResources( CultureInfo culture, string resourcesBaseName ) {
        var path = _pathResolver.GetJsonResourcePath( _resourcesRootPath, resourcesBaseName, culture );
        if ( File.Exists( path ) == false ) {
            if ( resourcesBaseName.IsEmpty() )
                return null;
            path = _pathResolver.GetJsonResourcePath( _resourcesRootPath, resourcesBaseName.Replace( '.', Path.DirectorySeparatorChar ), culture );
        }
        if ( File.Exists( path ) == false )
            return null;
        var builder = new ConfigurationBuilder()
            .AddJsonFile( path, optional: false, reloadOnChange: false );
        var config = builder.Build();
        return config.AsEnumerable();
    }

    /// <summary>
    /// Obtiene todas las cadenas localizadas para las culturas de interfaz de usuario actuales.
    /// </summary>
    /// <param name="includeParentCultures">Indica si se deben incluir las culturas padre en la búsqueda de cadenas localizadas.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método recorre las culturas de interfaz de usuario actuales y obtiene las cadenas localizadas
    /// correspondientes a cada cultura. Si <paramref name="includeParentCultures"/> es falso, el método
    /// devolverá las cadenas localizadas de la cultura actual y no buscará en las culturas padre.
    /// </remarks>
    /// <seealso cref="LocalizedString"/>
    public override IEnumerable<LocalizedString> GetAllStrings( bool includeParentCultures ) {
        var result = new List<LocalizedString>();
        var cultures = Util.Helpers.Culture.GetCurrentUICultures();
        foreach ( var culture in cultures ) {
            var resources = ToLocalizedStrings( GetResourcesByCache( culture, _resourcesBaseName ) );
            result.AddRange( resources );
            if ( includeParentCultures == false )
                return result;
        }
        return result;
    }

    /// <summary>
    /// Convierte una colección de pares clave-valor en una colección de cadenas localizadas.
    /// </summary>
    /// <param name="resources">Una colección de pares clave-valor donde la clave es el identificador del recurso y el valor es la cadena localizada.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método crea una nueva lista de <see cref="LocalizedString"/> a partir de los recursos proporcionados.
    /// Cada par clave-valor se utiliza para instanciar un nuevo objeto <see cref="LocalizedString"/>.
    /// </remarks>
    protected virtual IEnumerable<LocalizedString> ToLocalizedStrings(IEnumerable<KeyValuePair<string, string>> resources)
    {
        var result = new List<LocalizedString>();
        foreach (var resource in resources)
            result.Add(new LocalizedString(resource.Key, resource.Value, false, _searchedLocation));
        return result;
    }
}