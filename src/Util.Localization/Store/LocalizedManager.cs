using Util.Localization.Caching;

namespace Util.Localization.Store;

/// <summary>
/// Clase que implementa la interfaz <see cref="ILocalizedManager"/> 
/// para gestionar la localización de recursos en la aplicación.
/// </summary>
public class LocalizedManager : ILocalizedManager {
    private readonly ILocalizedStore _store;
    private readonly IMemoryCache _cache;
    private readonly LocalizationOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalizedManager"/>.
    /// </summary>
    /// <param name="store">La tienda de localización que proporciona los recursos localizados.</param>
    /// <param name="cache">El caché en memoria que se utilizará para almacenar los recursos localizados.</param>
    /// <param name="options">Las opciones de localización que configuran el comportamiento del administrador de localización.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="store"/> o <paramref name="cache"/> son nulos.</exception>
    /// <remarks>
    /// Este constructor asegura que los parámetros necesarios para la inicialización de la clase no sean nulos,
    /// proporcionando una instancia de <see cref="LocalizationOptions"/> por defecto si <paramref name="options"/> es nulo.
    /// </remarks>
    public LocalizedManager( ILocalizedStore store, IMemoryCache cache, IOptions<LocalizationOptions> options ) {
        _store = store ?? throw new ArgumentNullException( nameof( store ) );
        _cache = cache ?? throw new ArgumentNullException( nameof( cache ) );
        _options = options?.Value ?? new LocalizationOptions();
    }

    /// <inheritdoc />
    /// <summary>
    /// Carga todos los recursos para cada cultura disponible.
    /// </summary>
    /// <remarks>
    /// Este método itera a través de todas las culturas obtenidas y llama al método 
    /// <see cref="LoadAllResources(string)"/> para cargar los recursos específicos de cada cultura.
    /// </remarks>
    public virtual void LoadAllResources() {
        var cultures = GetCultures();
        foreach ( var culture in cultures )
            LoadAllResources( culture );
    }

    /// <summary>
    /// Obtiene una lista de nombres de culturas disponibles, incluyendo la cultura de la interfaz de usuario actual.
    /// </summary>
    /// <returns>
    /// Una lista de cadenas que representan los nombres de las culturas disponibles, sin duplicados.
    /// </returns>
    /// <remarks>
    /// Este método primero agrega la cultura actual de la interfaz de usuario a la lista de resultados.
    /// Luego, recupera las culturas disponibles desde un almacén y las añade a la lista si hay alguna.
    /// Finalmente, se asegura de que la lista no contenga elementos duplicados antes de devolverla.
    /// </remarks>
    protected virtual List<string> GetCultures() {
        var result = new List<string> {
            Util.Helpers.Culture.GetCurrentUICultureName()
        };
        var cultures = _store.GetCultures();
        if (cultures is { Count: > 0 })
            result.AddRange(cultures);
        return result.Distinct().ToList();
    }

    /// <inheritdoc />
    /// <summary>
    /// Carga todos los recursos para una cultura específica.
    /// </summary>
    /// <param name="culture">La cultura para la cual se deben cargar los recursos.</param>
    /// <remarks>
    /// Este método verifica si la cultura proporcionada está vacía. Si es así, no realiza ninguna acción.
    /// Luego, obtiene los tipos de recursos disponibles en el almacén. Si no hay tipos disponibles,
    /// se llama a sí mismo con la cultura y un valor nulo.
    /// Si hay tipos disponibles, itera sobre cada tipo y carga los recursos correspondientes.
    /// </remarks>
    /// <seealso cref="LoadAllResources(string, Type)"/>
    /// <seealso cref="_store"/>
    public virtual void LoadAllResources( string culture ) {
        if ( culture.IsEmpty() )
            return;
        var types = _store.GetTypes();
        if ( types == null || types.Count == 0 ) {
            LoadAllResources( culture, null );
            return;
        }
        foreach ( var type in types )
            LoadAllResources( culture, type );
    }

    /// <inheritdoc />
    /// <summary>
    /// Carga todos los recursos para una cultura y tipo específicos.
    /// </summary>
    /// <param name="culture">La cultura para la cual se cargarán los recursos.</param>
    /// <param name="type">El tipo de recursos que se cargarán.</param>
    /// <remarks>
    /// Este método verifica si la cultura está vacía antes de intentar cargar los recursos.
    /// Si no se encuentran recursos para la cultura y tipo especificados, no se realiza ninguna acción.
    /// Cada recurso encontrado se carga en la caché de recursos.
    /// </remarks>
    /// <seealso cref="LoadResourceCache(string, string, string, string)"/>
    public virtual void LoadAllResources( string culture, string type ) {
        if ( culture.IsEmpty() )
            return;
        var resources = _store.GetResources( culture, type );
        if ( resources == null )
            return;
        foreach ( var resource in resources )
            LoadResourceCache( culture, type, resource.Key, resource.Value );
    }

    /// <summary>
    /// Carga un recurso en la caché utilizando la cultura, el tipo, el nombre y el valor especificados.
    /// </summary>
    /// <param name="culture">La cultura para la cual se está cargando el recurso.</param>
    /// <param name="type">El tipo de recurso que se está almacenando en la caché.</param>
    /// <param name="name">El nombre del recurso que se está almacenando.</param>
    /// <param name="value">El valor del recurso que se está almacenando.</param>
    /// <remarks>
    /// Este método crea una clave de caché utilizando el método <see cref="CacheHelper.GetCacheKey"/> 
    /// y almacena un objeto <see cref="LocalizedString"/> en la caché con una duración determinada.
    /// </remarks>
    protected virtual void LoadResourceCache( string culture, string type, string name, string value ) {
        var cacheKey = CacheHelper.GetCacheKey( culture, type, name );
        var localizedString = new LocalizedString( name, value, false, null );
        _cache.Set( cacheKey, localizedString, TimeSpan.FromSeconds( CacheHelper.GetExpiration( _options ) ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Carga todos los recursos de un tipo específico para cada cultura disponible.
    /// </summary>
    /// <param name="type">El tipo de recurso que se desea cargar.</param>
    /// <remarks>
    /// Este método itera a través de todas las culturas obtenidas mediante el método <see cref="GetCultures"/> 
    /// y llama al método <see cref="LoadAllResources"/> para cargar los recursos de la cultura actual.
    /// </remarks>
    /// <seealso cref="GetCultures"/>
    /// <seealso cref="LoadAllResources(string, string)"/>
    public void LoadResourcesByType( string type ) {
        var cultures = GetCultures();
        foreach ( var culture in cultures )
            LoadAllResources( culture, type );
    }
}