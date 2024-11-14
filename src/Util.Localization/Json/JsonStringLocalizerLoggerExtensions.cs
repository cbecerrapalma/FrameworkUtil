namespace Util.Localization.Json;

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="ILogger"/> 
/// que permiten registrar mensajes relacionados con la localización de cadenas JSON.
/// </summary>
internal static class JsonStringLocalizerLoggerExtensions
{
    private static readonly Action<ILogger, string, string, CultureInfo, Exception> _searchedLocation;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JsonStringLocalizerLoggerExtensions"/>.
    /// Este constructor está diseñado para definir un mensaje de registro que se utilizará para
    /// registrar la búsqueda de claves en localizadores de cadenas JSON.
    /// </summary>
    static JsonStringLocalizerLoggerExtensions()
    {
        _searchedLocation = LoggerMessage.Define<string, string, CultureInfo>(
            LogLevel.Debug,
            1,
            $"{nameof(JsonStringLocalizer)} buscó '{{Key}}' en '{{LocationSearched}}' con la cultura '{{Culture}}'.");
    }

    /// <summary>
    /// Registra la ubicación buscada utilizando el registrador proporcionado.
    /// </summary>
    /// <param name="logger">El registrador que se utilizará para registrar la ubicación buscada.</param>
    /// <param name="key">La clave asociada a la ubicación buscada.</param>
    /// <param name="searchedLocation">La ubicación que se está buscando.</param>
    /// <param name="culture">La información cultural que se utilizará para el registro.</param>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ILogger"/> y permite registrar
    /// información sobre una ubicación específica que ha sido buscada.
    /// </remarks>
    public static void SearchedLocation(this ILogger logger, string key, string searchedLocation, CultureInfo culture)
    {
        _searchedLocation(logger, key, searchedLocation, culture, null);
    }
}