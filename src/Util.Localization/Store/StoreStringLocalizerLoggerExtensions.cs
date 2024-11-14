namespace Util.Localization.Store;

/// <summary>
/// Proporciona métodos de extensión para el registro de localizadores de cadenas en la tienda.
/// </summary>
internal static class StoreStringLocalizerLoggerExtensions
{
    private static readonly Action<ILogger, string, string, CultureInfo, Exception> _searched;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StoreStringLocalizerLoggerExtensions"/>.
    /// Este constructor estático define un mensaje de registro que se utilizará para registrar la búsqueda de recursos localizados.
    /// </summary>
    static StoreStringLocalizerLoggerExtensions()
    {
        _searched = LoggerMessage.Define<string, string, CultureInfo>(
            LogLevel.Debug,
            1,
            $"{nameof(StoreStringLocalizer)} busca el recurso localizado llamado '{{Key}}', de tipo '{{Type}}', con la cultura regional '{{Culture}}'."
        );
    }

    /// <summary>
    /// Registra un evento de búsqueda con los parámetros especificados.
    /// </summary>
    /// <param name="logger">El registrador que se utilizará para registrar el evento.</param>
    /// <param name="key">La clave de búsqueda que se está registrando.</param>
    /// <param name="type">El tipo de búsqueda que se está realizando.</param>
    /// <param name="culture">La cultura que se utilizará para el registro, que puede afectar el formato de los datos.</param>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ILogger"/> y permite registrar un evento de búsqueda 
    /// sin necesidad de proporcionar un objeto adicional.
    /// </remarks>
    public static void Searched(this ILogger logger, string key, string type, CultureInfo culture)
    {
        _searched(logger, key, type, culture, null);
    }
}