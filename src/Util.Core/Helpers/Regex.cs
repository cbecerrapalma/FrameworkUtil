namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos para trabajar con expresiones regulares.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos estáticos que permiten realizar operaciones de búsqueda y manipulación de cadenas 
/// utilizando expresiones regulares, facilitando la validación, búsqueda y reemplazo de patrones en texto.
/// </remarks>
public static class Regex {
    /// <summary>
    /// Obtiene un diccionario de valores a partir de una cadena de entrada y un patrón de expresión regular.
    /// </summary>
    /// <param name="input">La cadena de entrada que se va a evaluar.</param>
    /// <param name="pattern">El patrón de expresión regular que se utilizará para hacer coincidir la cadena de entrada.</param>
    /// <param name="resultPatterns">Un arreglo de patrones de resultados que se utilizarán para extraer valores del resultado de la coincidencia.</param>
    /// <param name="options">Opciones de expresión regular que modifican el comportamiento de la coincidencia. Por defecto es <see cref="RegexOptions.IgnoreCase"/>.</param>
    /// <returns>
    /// Un diccionario que contiene los valores extraídos de la cadena de entrada si se encuentra una coincidencia; de lo contrario, un diccionario vacío.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la coincidencia.
    /// Si la cadena de entrada es nula o está vacía, se devuelve un diccionario vacío.
    /// Si no se encuentra ninguna coincidencia con el patrón, también se devuelve un diccionario vacío.
    /// </remarks>
    /// <seealso cref="AddResults(Dictionary{string, string}, Match, string[])"/>
    public static Dictionary<string, string> GetValues( string input, string pattern, string[] resultPatterns, RegexOptions options = RegexOptions.IgnoreCase ) {
        var result = new Dictionary<string, string>();
        if( string.IsNullOrWhiteSpace( input ) )
            return result;
        var match = System.Text.RegularExpressions.Regex.Match( input, pattern, options );
        if( match.Success == false )
            return result;
        AddResults( result, match, resultPatterns );
        return result;
    }

    /// <summary>
    /// Agrega los resultados de una coincidencia a un diccionario de resultados.
    /// </summary>
    /// <param name="result">El diccionario donde se almacenarán los resultados.</param>
    /// <param name="match">La coincidencia de la que se extraerán los resultados.</param>
    /// <param name="resultPatterns">Un arreglo de patrones de resultados que se utilizarán para extraer los valores de la coincidencia.</param>
    /// <remarks>
    /// Si el arreglo de patrones de resultados es nulo, se agrega una entrada al diccionario con una clave vacía y el valor de la coincidencia.
    /// De lo contrario, se itera sobre cada patrón en el arreglo y se agrega al diccionario el resultado correspondiente.
    /// </remarks>
    private static void AddResults( Dictionary<string, string> result, Match match, string[] resultPatterns ) {
        if( resultPatterns == null ) {
            result.Add( string.Empty, match.Value );
            return;
        }
        foreach( var resultPattern in resultPatterns )
            result.Add( resultPattern, match.Result( resultPattern ) );
    }

    /// <summary>
    /// Obtiene un valor de una cadena de entrada que coincide con un patrón de expresión regular.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscará el patrón.</param>
    /// <param name="pattern">El patrón de expresión regular que se utilizará para buscar en la cadena de entrada.</param>
    /// <param name="resultPattern">El patrón de resultado opcional que se aplicará al valor coincidente. Si está vacío, se devuelve el valor coincidente directamente.</param>
    /// <param name="options">Las opciones de expresión regular que se aplicarán durante la búsqueda. Por defecto, se ignoran las mayúsculas y minúsculas.</param>
    /// <returns>
    /// Devuelve el valor coincidente si se encuentra; de lo contrario, devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la búsqueda.
    /// Si la cadena de entrada es nula o está vacía, se devuelve una cadena vacía.
    /// Si no se encuentra ninguna coincidencia, también se devuelve una cadena vacía.
    /// </remarks>
    public static string GetValue( string input, string pattern, string resultPattern = "", RegexOptions options = RegexOptions.IgnoreCase ) {
        if( string.IsNullOrWhiteSpace( input ) )
            return string.Empty;
        var match = System.Text.RegularExpressions.Regex.Match( input, pattern, options );
        if( match.Success == false )
            return string.Empty;
        return string.IsNullOrWhiteSpace( resultPattern ) ? match.Value : match.Result( resultPattern );
    }

    /// <summary>
    /// Divide una cadena de texto en un arreglo de cadenas utilizando un patrón de expresión regular.
    /// </summary>
    /// <param name="input">La cadena de texto que se desea dividir.</param>
    /// <param name="pattern">El patrón de expresión regular que se utilizará para dividir la cadena.</param>
    /// <param name="options">Opciones que modifican el comportamiento de la búsqueda de la expresión regular. Por defecto, se utiliza <see cref="RegexOptions.IgnoreCase"/>.</param>
    /// <returns>
    /// Un arreglo de cadenas que contiene las subcadenas resultantes de la división. 
    /// Si la cadena de entrada es nula o está vacía, se devuelve un arreglo vacío.
    /// </returns>
    public static string[] Split( string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase ) {
        if( string.IsNullOrWhiteSpace( input ) )
            return new string[]{};
        return System.Text.RegularExpressions.Regex.Split( input, pattern, options );
    }

    /// <summary>
    /// Reemplaza todas las ocurrencias de un patrón en una cadena de entrada por un valor de reemplazo especificado.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se realizará el reemplazo.</param>
    /// <param name="pattern">La expresión regular que define el patrón a buscar.</param>
    /// <param name="replacement">La cadena que se utilizará como reemplazo para cada coincidencia del patrón.</param>
    /// <param name="options">Opciones que modifican el comportamiento de la búsqueda, como la ignorancia de mayúsculas y minúsculas.</param>
    /// <returns>
    /// Una nueva cadena con todas las ocurrencias del patrón reemplazadas por el valor de reemplazo. 
    /// Si la cadena de entrada es nula o está vacía, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar el reemplazo.
    /// </remarks>
    public static string Replace( string input, string pattern,string replacement, RegexOptions options = RegexOptions.IgnoreCase ) {
        if( string.IsNullOrWhiteSpace( input ) )
            return string.Empty;
        return System.Text.RegularExpressions.Regex.Replace( input, pattern, replacement, options );
    }

    /// <summary>
    /// Determina si la cadena de entrada coincide con el patrón especificado.
    /// </summary>
    /// <param name="input">La cadena de entrada que se va a evaluar.</param>
    /// <param name="pattern">El patrón de expresión regular contra el cual se evaluará la cadena de entrada.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena de entrada coincide con el patrón; de lo contrario, <c>false</c>.
    /// </returns>
    public static bool IsMatch( string input, string pattern ) {
        return System.Text.RegularExpressions.Regex.IsMatch( input, pattern );
    }

    /// <summary>
    /// Determina si la cadena de entrada coincide con el patrón especificado utilizando las opciones de expresión regular proporcionadas.
    /// </summary>
    /// <param name="input">La cadena de entrada que se va a evaluar.</param>
    /// <param name="pattern">El patrón de expresión regular que se utilizará para la comparación.</param>
    /// <param name="options">Las opciones que modifican el comportamiento de la expresión regular.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena de entrada coincide con el patrón; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la comparación.
    /// </remarks>
    public static bool IsMatch( string input, string pattern, RegexOptions options ) {
        return System.Text.RegularExpressions.Regex.IsMatch( input, pattern, options );
    }

    /// <summary>
    /// Realiza una búsqueda de coincidencia en una cadena de entrada utilizando un patrón de expresión regular.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscará la coincidencia.</param>
    /// <param name="pattern">El patrón de expresión regular que se utilizará para buscar en la cadena de entrada.</param>
    /// <returns>Un objeto <see cref="Match"/> que contiene información sobre la coincidencia encontrada.</returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la búsqueda.
    /// Si no se encuentra ninguna coincidencia, se devolverá un objeto <see cref="Match"/> que no contiene información de coincidencia.
    /// </remarks>
    public static Match Match( string input, string pattern ) {
        return System.Text.RegularExpressions.Regex.Match( input, pattern );
    }

    /// <summary>
    /// Realiza una búsqueda de coincidencias en una cadena de entrada utilizando una expresión regular.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscará la coincidencia.</param>
    /// <param name="pattern">La expresión regular que se utilizará para encontrar la coincidencia.</param>
    /// <param name="options">Opciones que modifican el comportamiento de la búsqueda de la expresión regular.</param>
    /// <returns>
    /// Un objeto <see cref="Match"/> que contiene información sobre la coincidencia encontrada.
    /// Si no se encuentra ninguna coincidencia, se devuelve un objeto <see cref="Match"/> vacío.
    /// </returns>
    public static Match Match( string input, string pattern, RegexOptions options ) {
        return System.Text.RegularExpressions.Regex.Match( input, pattern, options );
    }

    /// <summary>
    /// Busca todas las coincidencias de una expresión regular en una cadena de entrada.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscarán las coincidencias.</param>
    /// <param name="pattern">La expresión regular que se utilizará para buscar coincidencias en la cadena de entrada.</param>
    /// <returns>
    /// Una colección de coincidencias que representan todas las coincidencias encontradas en la cadena de entrada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la búsqueda.
    /// Asegúrese de que el patrón proporcionado sea una expresión regular válida.
    /// </remarks>
    public static MatchCollection Matches( string input, string pattern ) {
        return System.Text.RegularExpressions.Regex.Matches( input, pattern );
    }

    /// <summary>
    /// Busca todas las coincidencias de una expresión regular en una cadena de entrada.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscarán las coincidencias.</param>
    /// <param name="pattern">La expresión regular que se utilizará para buscar coincidencias.</param>
    /// <param name="options">Opciones que modifican el comportamiento de la búsqueda de la expresión regular.</param>
    /// <returns>Una colección de coincidencias encontradas en la cadena de entrada.</returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> 
    /// para realizar la búsqueda de coincidencias. Asegúrese de que el patrón de expresión 
    /// regular sea válido para evitar excepciones.
    /// </remarks>
    public static MatchCollection Matches( string input, string pattern, RegexOptions options ) {
        return System.Text.RegularExpressions.Regex.Matches( input, pattern, options );
    }

    /// <summary>
    /// Busca todas las coincidencias de una expresión regular en una cadena de entrada.
    /// </summary>
    /// <param name="input">La cadena de entrada en la que se buscarán las coincidencias.</param>
    /// <param name="pattern">La expresión regular que se utilizará para buscar coincidencias.</param>
    /// <param name="options">Opciones que modifican el comportamiento de la búsqueda.</param>
    /// <param name="matchTimeout">El tiempo máximo permitido para la operación de búsqueda.</param>
    /// <returns>
    /// Una colección de coincidencias encontradas en la cadena de entrada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.Text.RegularExpressions.Regex"/> para realizar la búsqueda.
    /// Si no se encuentran coincidencias, se devolverá una colección vacía.
    /// </remarks>
    public static MatchCollection Matches( string input, string pattern, RegexOptions options, TimeSpan matchTimeout ) {
        return System.Text.RegularExpressions.Regex.Matches( input, pattern, options, matchTimeout );
    }
}