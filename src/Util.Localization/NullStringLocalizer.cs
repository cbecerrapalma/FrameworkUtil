namespace Util.Localization;

/// <summary>
/// Representa un localizador de cadenas que no proporciona traducciones.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStringLocalizer"/> y se utiliza
/// para manejar situaciones donde no se desea proporcionar localización de cadenas.
/// </remarks>
public class NullStringLocalizer : IStringLocalizer {
    public static readonly IStringLocalizer Instance = new NullStringLocalizer();

    /// <inheritdoc />
    public LocalizedString this[string name] => new( name, name,true );

    /// <inheritdoc />
    public LocalizedString this[ string name, params object[] arguments ] {
        get {
            var value = string.Format( name, arguments );
            return new LocalizedString( value, value, true );
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene todas las cadenas localizadas.
    /// </summary>
    /// <param name="includeParentCultures">
    /// Indica si se deben incluir las culturas padre en la búsqueda de cadenas localizadas.
    /// </param>
    /// <returns>
    /// Una colección de objetos <see cref="LocalizedString"/> que representan todas las cadenas localizadas.
    /// </returns>
    /// <remarks>
    /// Este método devuelve una lista vacía si no se encuentran cadenas localizadas.
    /// </remarks>
    public IEnumerable<LocalizedString> GetAllStrings( bool includeParentCultures ) => new List<LocalizedString>();
}