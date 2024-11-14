namespace Util.Localization;

/// <summary>
/// Clase que implementa la interfaz <see cref="IStringLocalizer"/> 
/// para proporcionar localización de cadenas en la aplicación.
/// </summary>
/// <remarks>
/// Esta clase permite la recuperación de cadenas localizadas 
/// basándose en la cultura actual del usuario.
/// </remarks>
internal class StringLocalizer : IStringLocalizer {
    private readonly IStringLocalizer _localizer;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StringLocalizer"/>.
    /// </summary>
    /// <param name="factory">La fábrica de localizadores de cadenas que se utilizará para crear el localizador.</param>
    /// <remarks>
    /// Este constructor utiliza la fábrica proporcionada para crear un localizador de cadenas basado en el ensamblado actual.
    /// </remarks>
    public StringLocalizer( IStringLocalizerFactory factory ) {
        var assemblyName = new AssemblyName( GetType().Assembly.FullName! );
        _localizer = factory.Create( string.Empty, assemblyName.FullName );
    }

    /// <inheritdoc />
    public LocalizedString this[string name] => _localizer[name];

    /// <inheritdoc />
    public LocalizedString this[string name, params object[] arguments] => _localizer[name, arguments];

    /// <inheritdoc />
    /// <summary>
    /// Obtiene todas las cadenas localizadas.
    /// </summary>
    /// <param name="includeParentCultures">
    /// Indica si se deben incluir las culturas padre en la búsqueda de cadenas localizadas.
    /// </param>
    /// <returns>
    /// Una colección de <see cref="LocalizedString"/> que contiene todas las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un localizador para recuperar las cadenas, y el comportamiento puede variar
    /// dependiendo del valor del parámetro <paramref name="includeParentCultures"/>.
    /// </remarks>
    /// <seealso cref="LocalizedString"/>
    public IEnumerable<LocalizedString> GetAllStrings( bool includeParentCultures ) => _localizer.GetAllStrings( includeParentCultures );
}