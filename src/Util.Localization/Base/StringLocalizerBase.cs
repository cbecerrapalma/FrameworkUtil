using Util.Localization.Caching;

namespace Util.Localization.Base;

/// <summary>
/// Clase base abstracta que proporciona una implementación común para la localización de cadenas.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStringLocalizer"/> y define métodos y propiedades
/// que pueden ser utilizados por las clases derivadas para ofrecer soporte de localización.
/// </remarks>
public abstract class StringLocalizerBase : IStringLocalizer {
    protected readonly IMemoryCache Cache;
    protected readonly string Type;
    protected LocalizationOptions Options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StringLocalizerBase"/>.
    /// </summary>
    /// <param name="cache">La caché en memoria utilizada para almacenar las traducciones localizadas.</param>
    /// <param name="type">El tipo de localización que se está utilizando.</param>
    /// <param name="options">Las opciones de localización que se aplicarán. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <remarks>
    /// Este constructor configura la caché, el tipo y las opciones de localización para la instancia actual.
    /// </remarks>
    protected StringLocalizerBase( IMemoryCache cache, string type, IOptions<LocalizationOptions> options ) {
        Cache = cache;
        Type = type;
        Options = options?.Value ?? new LocalizationOptions();
    }

    /// <inheritdoc />
    public LocalizedString this[string name] {
        get {
            if( name.IsEmpty() )
                throw new ArgumentNullException( nameof( name ) );
            return GetResult( name );
        }
    }

    /// <inheritdoc />
    public LocalizedString this[string name, params object[] arguments] {
        get {
            if( name.IsEmpty() )
                throw new ArgumentNullException( nameof( name ) );
            return GetResult( name, arguments );
        }
    }

    /// <summary>
    /// Obtiene una cadena localizada basada en el nombre proporcionado y los argumentos especificados.
    /// </summary>
    /// <param name="name">El nombre de la cadena localizada que se desea obtener.</param>
    /// <param name="arguments">Argumentos opcionales que se utilizarán para formatear la cadena resultante.</param>
    /// <returns>
    /// Una <see cref="LocalizedString"/> que contiene la cadena localizada formateada. 
    /// Si no se encuentra la cadena localizada, se devuelve un resultado formateado con el nombre y los argumentos proporcionados.
    /// </returns>
    /// <remarks>
    /// Este método busca la cadena localizada en las culturas de la interfaz de usuario actuales.
    /// Si la cadena no se encuentra en ninguna de las culturas, se devuelve una cadena formateada con el nombre y los argumentos.
    /// </remarks>
    protected virtual LocalizedString GetResult(string name, params object[] arguments) {
        LocalizedString result = null;
        var cultures = Util.Helpers.Culture.GetCurrentUICultures();
        foreach (var culture in cultures) {
            result = GetLocalizedStringByCache(culture, name);
            if (result.ResourceNotFound == false)
                return FormatResult(result, name, arguments);
        }
        return FormatResult(result, name, arguments);
    }

    /// <summary>
    /// Obtiene una cadena localizada utilizando un sistema de caché.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para la localización.</param>
    /// <param name="name">El nombre de la cadena que se desea localizar.</param>
    /// <returns>
    /// Una <see cref="LocalizedString"/> que representa la cadena localizada.
    /// </returns>
    /// <remarks>
    /// Si la caché es nula, se obtiene la cadena localizada directamente.
    /// De lo contrario, se intenta recuperar la cadena de la caché, y si no está disponible,
    /// se crea una nueva entrada en la caché con una expiración relativa.
    /// </remarks>
    /// <seealso cref="GetLocalizedString(CultureInfo, string)"/>
    protected virtual LocalizedString GetLocalizedStringByCache(CultureInfo culture, string name) {
        var key = CacheHelper.GetCacheKey(culture.Name, Type, name);
        return Cache == null ? GetLocalizedString(culture, name) : Cache.GetOrCreate(key, entry => {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(CacheHelper.GetExpiration(Options));
            return GetLocalizedString(culture, name);
        });
    }

    /// <summary>
    /// Obtiene una cadena localizada basada en la cultura y el nombre proporcionados.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para obtener la cadena localizada.</param>
    /// <param name="name">El nombre de la cadena que se desea localizar.</param>
    /// <returns>
    /// Un objeto <see cref="LocalizedString"/> que contiene la cadena localizada. 
    /// Si no se encuentra un valor para la cultura y el nombre dados, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar 
    /// una lógica de localización personalizada.
    /// </remarks>
    protected virtual LocalizedString GetLocalizedString(CultureInfo culture, string name)
    {
        var value = GetValue(culture, name, Type);
        if (value.IsEmpty())
            return new LocalizedString(name, string.Empty, true, null);
        return new LocalizedString(name, value, false, null);
    }

    /// <summary>
    /// Obtiene un valor basado en la cultura, el nombre y el tipo especificados.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para obtener el valor.</param>
    /// <param name="name">El nombre del valor que se desea obtener.</param>
    /// <param name="type">El tipo del valor que se desea obtener.</param>
    /// <returns>Un string que representa el valor obtenido según los parámetros proporcionados.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// Asegúrese de manejar adecuadamente las diferencias culturales en la implementación.
    /// </remarks>
    protected abstract string GetValue(CultureInfo culture, string name, string type);

    /// <summary>
    /// Formatea el resultado de una cadena localizada, reemplazando los argumentos en la cadena.
    /// </summary>
    /// <param name="result">La cadena localizada que se va a formatear.</param>
    /// <param name="name">El nombre que se utilizará si el resultado es nulo o no se encuentra el recurso.</param>
    /// <param name="arguments">Los argumentos que se utilizarán para formatear la cadena.</param>
    /// <returns>
    /// Un objeto <see cref="LocalizedString"/> que contiene el resultado formateado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el resultado es nulo o si el recurso no se encuentra. 
    /// Si no se proporcionan argumentos, se devuelve el resultado original.
    /// </remarks>
    protected virtual LocalizedString FormatResult(LocalizedString result, string name, params object[] arguments) {
        if (result == null)
            return new LocalizedString(name, string.Format(name, arguments), true, null);
        if (result.ResourceNotFound)
            return new LocalizedString(result.Name, string.Format(result.Name, arguments), true, null);
        if (arguments == null || arguments.Length == 0)
            return result;
        return new LocalizedString(result.Name, string.Format(result.Value, arguments), false, result.SearchedLocation);
    }

    /// <summary>
    /// Obtiene todas las cadenas localizadas.
    /// </summary>
    /// <param name="includeParentCultures">Indica si se deben incluir las culturas padre en la búsqueda de cadenas localizadas.</param>
    /// <returns>
    /// Una colección de <see cref="LocalizedString"/> que contiene todas las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// </remarks>
    public abstract IEnumerable<LocalizedString> GetAllStrings( bool includeParentCultures );
}