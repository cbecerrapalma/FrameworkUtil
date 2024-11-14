namespace Util.Localization.Store;

/// <summary>
/// Interfaz que define las operaciones para un almacén de datos localizado.
/// </summary>
public interface ILocalizedStore {
    /// <summary>
    /// Obtiene un valor basado en la cultura, el tipo y el nombre proporcionados.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará para obtener el valor.</param>
    /// <param name="type">El tipo de valor que se desea obtener.</param>
    /// <param name="name">El nombre asociado al valor que se desea recuperar.</param>
    /// <returns>Un string que representa el valor obtenido según los parámetros especificados.</returns>
    /// <remarks>
    /// Este método es útil para la localización de recursos y la obtención de configuraciones específicas 
    /// según la cultura y el tipo de datos requeridos.
    /// </remarks>
    /// <seealso cref="System.Globalization.CultureInfo"/>
    string GetValue( string culture, string type, string name );
    /// <summary>
    /// Obtiene una lista de culturas disponibles.
    /// </summary>
    /// <returns>
    /// Una lista que contiene las culturas en formato de cadena.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para obtener las culturas que son soportadas por la aplicación,
    /// permitiendo así la localización y la internacionalización de la misma.
    /// </remarks>
    IList<string> GetCultures();
    /// <summary>
    /// Obtiene una lista de tipos en formato de cadena.
    /// </summary>
    /// <returns>
    /// Una lista que contiene los tipos como cadenas.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para recuperar una colección de tipos
    /// que pueden ser utilizados en otras partes de la aplicación.
    /// </remarks>
    IList<string> GetTypes();
    /// <summary>
    /// Obtiene un diccionario de recursos basado en la cultura y el tipo especificados.
    /// </summary>
    /// <param name="culture">La cultura para la cual se desean obtener los recursos, en formato de cadena (por ejemplo, "es-ES").</param>
    /// <param name="type">El tipo de recursos que se desean obtener, en formato de cadena.</param>
    /// <returns>Un diccionario que contiene pares clave-valor de recursos, donde la clave es una cadena y el valor es también una cadena.</returns>
    /// <remarks>
    /// Este método es útil para cargar recursos localizados en función de la cultura y el tipo especificado.
    /// Asegúrese de que los recursos estén disponibles para la cultura solicitada.
    /// </remarks>
    /// <seealso cref="IDictionary{TKey, TValue}"/>
    IDictionary<string, string> GetResources( string culture, string type );
}