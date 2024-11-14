using Util.Localization.Base;

namespace Util.Localization.Store;

/// <summary>
/// Representa un localizador de cadenas que utiliza un almacenamiento específico para la localización de cadenas.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="StringLocalizerBase"/> y proporciona funcionalidades para la 
/// localización de cadenas en una aplicación, permitiendo la recuperación de cadenas localizadas 
/// desde un almacenamiento definido.
/// </remarks>
public class StoreStringLocalizer : StringLocalizerBase {
    private readonly ILogger _logger;
    private readonly ILocalizedStore _store;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StoreStringLocalizer"/>.
    /// </summary>
    /// <param name="logger">El registrador utilizado para registrar información y errores.</param>
    /// <param name="cache">La caché en memoria utilizada para almacenar localizaciones.</param>
    /// <param name="store">La tienda de localización que proporciona las cadenas localizadas.</param>
    /// <param name="type">El tipo de localización que se está utilizando.</param>
    /// <param name="options">Las opciones de localización que configuran el comportamiento del localizador.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="store"/> es <c>null</c>.</exception>
    public StoreStringLocalizer( ILogger logger, IMemoryCache cache, ILocalizedStore store, string type, IOptions<LocalizationOptions> options ) : base( cache, type, options ) {
        _logger = logger ?? NullLogger.Instance;
        _store = store ?? throw new ArgumentNullException( nameof( store ) );
    }

    /// <summary>
    /// Obtiene una cadena localizada formateada según el nombre y los argumentos proporcionados.
    /// </summary>
    /// <param name="name">El nombre de la cadena localizada que se desea obtener.</param>
    /// <param name="arguments">Los argumentos que se utilizarán para formatear la cadena localizada.</param>
    /// <returns>
    /// Una <see cref="LocalizedString"/> que contiene la cadena localizada formateada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la cultura de la interfaz de usuario actual para recuperar la cadena localizada
    /// desde la caché y luego la formatea con los argumentos proporcionados.
    /// </remarks>
    protected override LocalizedString GetResult(string name, params object[] arguments) {
        var culture = Util.Helpers.Culture.GetCurrentUICulture();
        var result = GetLocalizedStringByCache(culture, name);
        return FormatResult(result, name, arguments);
    }

    /// <summary>
    /// Obtiene un valor del almacén basado en la cultura, el nombre y el tipo especificados.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para obtener el valor.</param>
    /// <param name="name">El nombre del valor que se desea obtener.</param>
    /// <param name="type">El tipo del valor que se desea obtener.</param>
    /// <returns>
    /// El valor correspondiente al nombre y tipo especificados en la cultura dada.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que utiliza un almacén de valores y registra la búsqueda realizada.
    /// </remarks>
    /// <seealso cref="CultureInfo"/>
    protected override string GetValue( CultureInfo culture, string name, string type ) {
        var result = _store.GetValue( culture.Name, type, name );
        _logger.Searched( name, type, culture );
        return result;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene todas las cadenas localizadas para las culturas actuales.
    /// </summary>
    /// <param name="includeParentCultures">Indica si se deben incluir las culturas padre en la búsqueda de cadenas localizadas.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan todas las cadenas localizadas encontradas.
    /// </returns>
    /// <remarks>
    /// Este método itera a través de las culturas de la interfaz de usuario actuales y recupera las cadenas localizadas
    /// para cada cultura. Si <paramref name="includeParentCultures"/> es falso, el método detiene la búsqueda
    /// después de obtener las cadenas de la cultura actual.
    /// </remarks>
    /// <seealso cref="LocalizedString"/>
    public override IEnumerable<LocalizedString> GetAllStrings( bool includeParentCultures ) {
        var result = new List<LocalizedString>();
        var cultures = Helpers.Culture.GetCurrentUICultures();
        foreach ( var culture in cultures ) {
            var resources = GetAllStrings( culture.Name );
            result.AddRange( resources );
            if ( includeParentCultures == false )
                return result;
        }
        return result;
    }

    /// <summary>
    /// Obtiene todas las cadenas localizadas para una cultura específica.
    /// </summary>
    /// <param name="culture">La cultura para la cual se desean obtener las cadenas localizadas.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan las cadenas localizadas
    /// para la cultura especificada. Si no se encuentran tipos, se obtendrán las cadenas localizadas
    /// sin un tipo específico.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una
    /// implementación personalizada de la obtención de cadenas localizadas.
    /// </remarks>
    protected virtual IEnumerable<LocalizedString> GetAllStrings(string culture)
    {
        var result = new List<LocalizedString>();
        var types = _store.GetTypes();
        if (types == null || types.Count == 0)
        {
            result.AddRange(GetAllStrings(culture, null));
            return result;
        }
        foreach (var type in types)
            result.AddRange(GetAllStrings(culture, type));
        return result;
    }

    /// <summary>
    /// Obtiene todas las cadenas localizadas para una cultura y tipo específicos.
    /// </summary>
    /// <param name="culture">La cultura para la cual se desean obtener las cadenas localizadas.</param>
    /// <param name="type">El tipo de recursos que se desean recuperar.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan las cadenas localizadas.
    /// Si no se encuentran recursos, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual IEnumerable<LocalizedString> GetAllStrings(string culture, string type)
    {
        var resources = _store.GetResources(culture, type);
        if (resources == null)
            return new List<LocalizedString>();
        return ToLocalizedStrings(resources);
    }

    /// <summary>
    /// Convierte una colección de pares clave-valor en una lista de cadenas localizadas.
    /// </summary>
    /// <param name="resources">Una colección de pares clave-valor donde la clave es el identificador del recurso y el valor es la cadena localizable.</param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar funcionalidad adicional.
    /// </remarks>
    protected virtual IEnumerable<LocalizedString> ToLocalizedStrings(IEnumerable<KeyValuePair<string, string>> resources)
    {
        var result = new List<LocalizedString>();
        foreach (var resource in resources)
            result.Add(new LocalizedString(resource.Key, resource.Value, false, null));
        return result;
    }
}