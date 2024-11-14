namespace Util.Helpers;

/// <summary>
/// Proporciona métodos de extensión para manipular cadenas de texto.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos estáticos que permiten realizar diversas operaciones sobre cadenas,
/// como la manipulación, búsqueda y transformación de texto.
/// </remarks>
public static class String
{

    #region Join(Conectar un conjunto en una cadena de texto separada por un delimitador.)

    /// <summary>
    /// Une los elementos de una colección en una cadena, opcionalmente rodeando cada elemento con comillas y separándolos con un delimitador especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="values">La colección de elementos que se van a unir.</param>
    /// <param name="quotes">Las comillas que se usarán para rodear cada elemento. Por defecto es una cadena vacía.</param>
    /// <param name="separator">El separador que se usará entre los elementos. Por defecto es una coma.</param>
    /// <returns>Una cadena que contiene todos los elementos de la colección unidos, rodeados por las comillas especificadas y separados por el delimitador dado.</returns>
    /// <remarks>
    /// Si la colección <paramref name="values"/> es nula, se devolverá una cadena vacía.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    public static string Join<T>(IEnumerable<T> values, string quotes = "", string separator = ",")
    {
        if (values == null)
            return string.Empty;
        var result = new StringBuilder();
        foreach (var each in values)
            result.AppendFormat("{0}{1}{0}{2}", quotes, each, separator);
        return result.ToString().RemoveEnd(separator);
    }

    #endregion

    #region FirstLowerCase(minúscula inicial)

    /// <summary>
    /// Convierte el primer carácter de la cadena proporcionada a minúscula si no es ya una letra minúscula.
    /// </summary>
    /// <param name="value">La cadena de texto que se desea modificar.</param>
    /// <returns>
    /// Una nueva cadena con el primer carácter en minúscula si es necesario; 
    /// de lo contrario, devuelve la cadena original. Si la cadena está vacía o solo contiene espacios en blanco, devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Rune"/> para manejar caracteres Unicode y asegurar que se manejen correctamente los caracteres que pueden ocupar más de un código de unidad.
    /// </remarks>
    public static string FirstLowerCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        var result = Rune.DecodeFromUtf16(value, out var rune, out var charsConsumed);
        if (result != OperationStatus.Done || Rune.IsLower(rune))
            return value;
        return Rune.ToLowerInvariant(rune) + value[charsConsumed..];
    }

    #endregion

    #region FirstUpperCase(Mayúscula inicial)

    /// <summary>
    /// Convierte la primera letra de la cadena proporcionada a mayúscula.
    /// </summary>
    /// <param name="value">La cadena de texto que se desea modificar.</param>
    /// <returns>
    /// Una nueva cadena con la primera letra en mayúscula. 
    /// Si la cadena está vacía o es nula, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Rune"/> para manejar caracteres Unicode y 
    /// asegurar que la conversión a mayúscula se realice correctamente, incluso para caracteres 
    /// que no son ASCII.
    /// </remarks>
    public static string FirstUpperCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        var result = Rune.DecodeFromUtf16(value, out var rune, out var charsConsumed);
        if (result != OperationStatus.Done || Rune.IsUpper(rune))
            return value;
        return Rune.ToUpperInvariant(rune) + value[charsConsumed..];
    }

    #endregion

    #region RemoveStart(Eliminar la cadena inicial.)

    /// <summary>
    /// Elimina un prefijo específico de una cadena si está presente.
    /// </summary>
    /// <param name="value">La cadena de la cual se eliminará el prefijo.</param>
    /// <param name="start">El prefijo que se desea eliminar.</param>
    /// <param name="ignoreCase">Indica si la comparación debe ser insensible a mayúsculas y minúsculas. Por defecto es <c>true</c>.</param>
    /// <returns>
    /// La cadena resultante sin el prefijo especificado, o la cadena original si el prefijo no está presente.
    /// </returns>
    /// <remarks>
    /// Si la cadena de entrada es nula o está vacía, se devuelve una cadena vacía.
    /// Si el prefijo está vacío, se devuelve la cadena original sin modificaciones.
    /// </remarks>
    public static string RemoveStart(string value, string start, bool ignoreCase = true)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        if (string.IsNullOrEmpty(start))
            return value;
        var options = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (value.StartsWith(start, options) == false)
            return value;
        return value.Substring(start.Length, value.Length - start.Length);
    }

    /// <summary>
    /// Elimina el prefijo especificado de un <see cref="StringBuilder"/> si está presente.
    /// </summary>
    /// <param name="value">El <see cref="StringBuilder"/> del cual se eliminará el prefijo.</param>
    /// <param name="start">El prefijo que se desea eliminar.</param>
    /// <returns>
    /// Un nuevo <see cref="StringBuilder"/> sin el prefijo especificado si estaba presente; 
    /// de lo contrario, devuelve el <see cref="StringBuilder"/> original. 
    /// Si el <paramref name="value"/> es nulo o vacío, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Si el <paramref name="start"/> es nulo o vacío, se devuelve el <paramref name="value"/> sin cambios.
    /// Si el <paramref name="start"/> es más largo que el <paramref name="value"/>, 
    /// también se devuelve el <paramref name="value"/> sin cambios.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    public static StringBuilder RemoveStart(StringBuilder value, string start)
    {
        if (value == null || value.Length == 0)
            return null;
        if (string.IsNullOrEmpty(start))
            return value;
        if (start.Length > value.Length)
            return value;
        var chars = start.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (value[i] != chars[i])
                return value;
        }
        return value.Remove(0, start.Length);
    }

    #endregion

    #region RemoveEnd(Eliminar cadena al final.)

    /// <summary>
    /// Elimina una subcadena del final de una cadena dada si está presente.
    /// </summary>
    /// <param name="value">La cadena de la que se eliminará la subcadena.</param>
    /// <param name="end">La subcadena que se desea eliminar del final de <paramref name="value"/>.</param>
    /// <param name="ignoreCase">Indica si la comparación debe ser insensible a mayúsculas y minúsculas. El valor predeterminado es verdadero.</param>
    /// <returns>
    /// La cadena resultante después de eliminar la subcadena especificada del final. 
    /// Si <paramref name="value"/> es nula o está vacía, se devuelve una cadena vacía. 
    /// Si <paramref name="end"/> es nula o está vacía, se devuelve <paramref name="value"/> sin cambios.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la comparación de cadenas especificada por el parámetro <paramref name="ignoreCase"/> 
    /// para determinar si <paramref name="end"/> está presente al final de <paramref name="value"/>.
    /// </remarks>
    public static string RemoveEnd(string value, string end, bool ignoreCase = true)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        if (string.IsNullOrEmpty(end))
            return value;
        var options = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (value.EndsWith(end, options) == false)
            return value;
        return value.Substring(0, value.LastIndexOf(end, options));
    }

    /// <summary>
    /// Elimina una cadena especificada del final de un objeto <see cref="StringBuilder"/> dado.
    /// </summary>
    /// <param name="value">El objeto <see cref="StringBuilder"/> del cual se eliminará la cadena.</param>
    /// <param name="end">La cadena que se desea eliminar del final del objeto <see cref="StringBuilder"/>.</param>
    /// <returns>
    /// Un nuevo objeto <see cref="StringBuilder"/> sin la cadena especificada al final, 
    /// o <c>null</c> si el objeto <paramref name="value"/> es <c>null</c> o está vacío.
    /// </returns>
    /// <remarks>
    /// Si la cadena <paramref name="end"/> es <c>null</c> o está vacía, 
    /// se devuelve el objeto <paramref name="value"/> sin cambios. 
    /// Si la longitud de <paramref name="end"/> es mayor que la longitud de <paramref name="value"/>, 
    /// también se devuelve <paramref name="value"/> sin cambios.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    public static StringBuilder RemoveEnd(StringBuilder value, string end)
    {
        if (value == null || value.Length == 0)
            return null;
        if (string.IsNullOrEmpty(end))
            return value;
        if (end.Length > value.Length)
            return value;
        var chars = end.ToCharArray();
        for (int i = chars.Length - 1; i >= 0; i--)
        {
            var j = value.Length - (chars.Length - i);
            if (value[j] != chars[i])
                return value;
        }
        return value.Remove(value.Length - end.Length, end.Length);
    }

    #endregion

    #region PinYin(Obtener el código simplificado de pinyin de los caracteres chinos.)

    /// <summary>
    /// Convierte un texto en chino a su representación en Pinyin.
    /// </summary>
    /// <param name="chineseText">El texto en chino que se desea convertir.</param>
    /// <returns>Una cadena que representa el texto en Pinyin en minúsculas. Si el texto de entrada está vacío, se devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método itera sobre cada carácter del texto en chino y utiliza el método <see cref="ResolvePinYin(char)"/> 
    /// para obtener su correspondiente en Pinyin. El resultado final se devuelve en minúsculas.
    /// </remarks>
    public static string PinYin(string chineseText)
    {
        if (chineseText.IsEmpty())
            return string.Empty;
        var result = new StringBuilder();
        foreach (char text in chineseText)
            result.Append(ResolvePinYin(text));
        return result.ToString().ToLower();
    }

    /// <summary>
    /// Resuelve el Pinyin correspondiente a un carácter dado.
    /// </summary>
    /// <param name="text">El carácter del cual se desea obtener el Pinyin.</param>
    /// <returns>
    /// Una cadena que representa el Pinyin del carácter. Si el carácter es un carácter ASCII, 
    /// se devuelve el carácter como una cadena. Si no se puede resolver el Pinyin, se 
    /// intenta obtener un valor por constante.
    /// </returns>
    /// <remarks>
    /// Este método convierte el carácter en bytes utilizando la codificación UTF-8 y 
    /// verifica si el carácter es ASCII. Si no lo es, se calcula el valor Unicode y se 
    /// intenta resolver el Pinyin utilizando el código Unicode. Si no se encuentra un 
    /// Pinyin correspondiente, se utiliza un método alternativo para resolverlo por 
    /// constante.
    /// </remarks>
    /// <seealso cref="ResolveByCode(ushort)"/>
    /// <seealso cref="ResolveByConst(string)"/>
    private static string ResolvePinYin(char text)
    {
        byte[] charBytes = Encoding.UTF8.GetBytes(text.ToString());
        if (charBytes[0] <= 127)
            return text.ToString();
        var unicode = (ushort)(charBytes[0] * 256 + charBytes[1]);
        string pinYin = ResolveByCode(unicode);
        if (pinYin.IsEmpty() == false)
            return pinYin;
        return ResolveByConst(text.ToString());
    }

    /// <summary>
    /// Resuelve un código Unicode en su correspondiente letra.
    /// </summary>
    /// <param name="unicode">El valor Unicode que se desea resolver.</param>
    /// <returns>Una cadena que representa la letra correspondiente al código Unicode, o una cadena vacía si no se encuentra una coincidencia.</returns>
    private static string ResolveByCode(ushort unicode)
    {
        if (unicode >= '\uB0A1' && unicode <= '\uB0C4')
            return "A";
        if (unicode >= '\uB0C5' && unicode <= '\uB2C0' && unicode != 45464)
            return "B";
        if (unicode >= '\uB2C1' && unicode <= '\uB4ED')
            return "C";
        if (unicode >= '\uB4EE' && unicode <= '\uB6E9')
            return "D";
        if (unicode >= '\uB6EA' && unicode <= '\uB7A1')
            return "E";
        if (unicode >= '\uB7A2' && unicode <= '\uB8C0')
            return "F";
        if (unicode >= '\uB8C1' && unicode <= '\uB9FD')
            return "G";
        if (unicode >= '\uB9FE' && unicode <= '\uBBF6')
            return "H";
        if (unicode >= '\uBBF7' && unicode <= '\uBFA5')
            return "J";
        if (unicode >= '\uBFA6' && unicode <= '\uC0AB')
            return "K";
        if (unicode >= '\uC0AC' && unicode <= '\uC2E7')
            return "L";
        if (unicode >= '\uC2E8' && unicode <= '\uC4C2')
            return "M";
        if (unicode >= '\uC4C3' && unicode <= '\uC5B5')
            return "N";
        if (unicode >= '\uC5B6' && unicode <= '\uC5BD')
            return "O";
        if (unicode >= '\uC5BE' && unicode <= '\uC6D9')
            return "P";
        if (unicode >= '\uC6DA' && unicode <= '\uC8BA')
            return "Q";
        if (unicode >= '\uC8BB' && unicode <= '\uC8F5')
            return "R";
        if (unicode >= '\uC8F6' && unicode <= '\uCBF9')
            return "S";
        if (unicode >= '\uCBFA' && unicode <= '\uCDD9')
            return "T";
        if (unicode >= '\uCDDA' && unicode <= '\uCEF3')
            return "W";
        if (unicode >= '\uCEF4' && unicode <= '\uD188')
            return "X";
        if (unicode >= '\uD1B9' && unicode <= '\uD4D0')
            return "Y";
        if (unicode >= '\uD4D1' && unicode <= '\uD7F9')
            return "Z";
        return string.Empty;
    }

    /// <summary>
    /// Resuelve un texto en función de una constante de Pinyin chino.
    /// </summary>
    /// <param name="text">El texto que se desea resolver.</param>
    /// <returns>
    /// Un carácter correspondiente al texto proporcionado si se encuentra en la constante de Pinyin chino; de lo contrario, una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método busca el texto en la constante <see cref="Const.ChinesePinYin"/> 
    /// y devuelve el carácter que sigue al texto encontrado. Si el texto no se encuentra, 
    /// se devuelve una cadena vacía.
    /// </remarks>
    private static string ResolveByConst(string text)
    {
        int index = Const.ChinesePinYin.IndexOf(text, StringComparison.Ordinal);
        if (index < 0)
            return string.Empty;
        return Const.ChinesePinYin.Substring(index + 1, 1);
    }

    #endregion

    #region Extract(Extraer valores de variables de una cadena.)

    /// <summary>
    /// Extrae un diccionario de pares clave-valor a partir de un valor de entrada y un formato especificado.
    /// </summary>
    /// <param name="value">El valor de entrada del cual se extraerán los datos.</param>
    /// <param name="format">El formato que define cómo se deben extraer los datos del valor.</param>
    /// <returns>
    /// Un diccionario que contiene los pares clave-valor extraídos. 
    /// Si el valor o el formato están vacíos, o si el formato no contiene las llaves adecuadas, se devuelve un diccionario vacío.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el valor y el formato son válidos antes de proceder con la extracción. 
    /// Si el formato no contiene llaves de apertura o cierre, se considera inválido y se devuelve un diccionario vacío.
    /// </remarks>
    /// <seealso cref="SplitFormat(string)"/>
    /// <seealso cref="ExtractValue(string, IEnumerable{string})"/>
    public static IDictionary<string, string> Extract(string value, string format)
    {
        var result = new Dictionary<string, string>();
        if (value.IsEmpty())
            return result;
        if (format.IsEmpty())
            return result;
        if (format.Contains("{", StringComparison.Ordinal) == false)
            return result;
        if (format.Contains("}", StringComparison.Ordinal) == false)
            return result;
        var formatItems = SplitFormat(format.SafeString());
        return ExtractValue(value.SafeString(), formatItems);
    }

    /// <summary>
    /// Divide una cadena de formato en una lista de subcadenas, separando los elementos que están entre llaves.
    /// </summary>
    /// <param name="format">La cadena de formato que se desea dividir.</param>
    /// <returns>Una lista de cadenas que contiene los elementos separados del formato.</returns>
    /// <remarks>
    /// Este método analiza la cadena de entrada y extrae las partes que están fuera de las llaves, así como las partes que están dentro de ellas. 
    /// Las llaves se utilizan para identificar los elementos que deben ser tratados de manera especial.
    /// Si la cadena de formato comienza o termina con llaves, se agregan cadenas vacías a la lista de resultados.
    /// </remarks>
    private static List<string> SplitFormat(string format)
    {
        var result = new List<string>();
        var item = new StringBuilder();
        for (int i = 0; i < format.Length; i++)
        {
            var temp = format[i];
            if (temp == '{')
            {
                item.RemoveEnd("{");
                if (i == 0)
                {
                    result.Add(string.Empty);
                }
                if (item.Length > 0)
                {
                    result.Add(item.ToString());
                    item.Clear();
                }
                item.Append(temp);
                continue;
            }
            if (temp == '}')
            {
                if (item.ToString().IsEmpty())
                    continue;
                item.RemoveEnd("}");
                item.Append(temp);
                result.Add(item.ToString());
                item.Clear();
                if (i == format.Length - 1)
                    result.Add(string.Empty);
                continue;
            }
            item.Append(temp);
            if (i == format.Length - 1)
            {
                result.Add(item.ToString());
                item.Clear();
            }
        }
        return result;
    }

    /// <summary>
    /// Extrae valores de una cadena según un formato especificado por una lista de elementos de formato.
    /// </summary>
    /// <param name="value">La cadena de la que se extraerán los valores.</param>
    /// <param name="formatItems">Una lista de elementos de formato que indican cómo se deben extraer los valores.</param>
    /// <returns>Un diccionario que contiene los nombres de las variables como claves y los valores extraídos como valores.</returns>
    /// <remarks>
    /// Este método busca en la cadena de entrada los elementos de formato que están delimitados por llaves 
    /// ({}) y extrae los valores correspondientes entre estos delimitadores. Si no se encuentra un valor, 
    /// se asigna una cadena vacía.
    /// </remarks>
    /// <example>
    /// <code>
    /// var valoresExtraidos = ExtractValue("Nombre: Juan, Edad: 30", new List<string> { "{Nombre}", ":", "{Edad}" });
    /// // valoresExtraidos contendrá: { "Nombre": "Juan", "Edad": "30" }
    /// </code>
    /// </example>
    /// <seealso cref="IDictionary{TKey, TValue}"/>
    private static IDictionary<string, string> ExtractValue(string value, List<string> formatItems)
    {
        var result = new Dictionary<string, string>();
        var leftIndex = 0;
        var length = 0;
        for (int i = 0; i < formatItems.Count; i++)
        {
            var item = formatItems[i];
            if (item == string.Empty)
                continue;
            if (item.StartsWith("{", StringComparison.Ordinal) == false)
            {
                leftIndex += item.Length;
                continue;
            }
            if (i + 1 < formatItems.Count)
            {
                var rightItem = formatItems[i + 1];
                if (rightItem == string.Empty)
                    length = value.Length - leftIndex;
                else
                    length = value.IndexOf(rightItem, leftIndex + 1, StringComparison.OrdinalIgnoreCase) - leftIndex;
            }
            var varName = item.Replace("{", "").Replace("}", "");
            if (length <= 0)
            {
                result.Add(varName, string.Empty);
                continue;
            }
            var variableValue = value.Substring(leftIndex, length);
            result.Add(varName, variableValue);
            leftIndex += length;
        }
        return result;
    }

    #endregion
}