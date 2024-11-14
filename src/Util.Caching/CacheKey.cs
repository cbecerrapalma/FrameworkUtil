namespace Util.Caching; 

/// <summary>
/// Representa una clave para el sistema de caché.
/// </summary>
public class CacheKey {
    private string _key;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CacheKey"/>.
    /// </summary>
    public CacheKey() {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CacheKey"/>.
    /// </summary>
    /// <param name="key">La cadena de formato que se utilizará para generar la clave de caché.</param>
    /// <param name="parameters">Los parámetros que se insertarán en la cadena de formato.</param>
    public CacheKey( string key,params object[] parameters) {
        _key = string.Format( key, parameters );
    }

    /// <summary>
    /// Obtiene o establece la clave asociada.
    /// </summary>
    /// <value>
    /// La clave como una cadena de texto.
    /// </value>
    /// <remarks>
    /// Al obtener el valor, se llama al método <see cref="ToString"/> para representar la clave.
    /// Al establecer el valor, se asigna a la variable privada <c>_key</c>.
    /// </remarks>
    public string Key {
        get => ToString();
        set => _key = value;
    }

    /// <summary>
    /// Obtiene o establece el prefijo asociado.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el prefijo.
    /// </value>
    public string Prefix { get; set; }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el objeto actual, compuesta por el prefijo y la clave.
    /// </returns>
    public override string ToString() {
        return $"{Prefix}{_key}";
    }
}