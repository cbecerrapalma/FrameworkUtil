namespace Util.Helpers;

/// <summary>
/// Proporciona métodos estáticos para convertir tipos de datos.
/// </summary>
public static class Convert
{

    #region ToInt(Convertir a un entero de 32 bits.)

    /// <summary>
    /// Convierte un objeto en un entero. Si la conversión falla, devuelve 0.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a entero.</param>
    /// <returns>El valor entero resultante de la conversión, o 0 si la conversión falla.</returns>
    public static int ToInt(object input)
    {
        return ToIntOrNull(input) ?? 0;
    }

    #endregion

    #region ToIntOrNull(Convertir a un entero de 32 bits nullable.)

    /// <summary>
    /// Convierte un objeto a un entero nullable. 
    /// Si la conversión es exitosa, devuelve el valor entero; de lo contrario, devuelve null.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a entero.</param>
    /// <returns>Un entero nullable que representa el valor convertido, o null si la conversión falla.</returns>
    /// <remarks>
    /// Este método intenta primero convertir el objeto a una cadena segura y luego a un entero. 
    /// Si la conversión falla, intenta convertir el objeto a un doble y luego a un entero.
    /// En caso de que ambas conversiones fallen, se devuelve null.
    /// </remarks>
    /// <seealso cref="ToDoubleOrNull(object, double)"/>
    public static int? ToIntOrNull(object input)
    {
        var success = int.TryParse(input.SafeString(), out var result);
        if (success)
            return result;
        try
        {
            var temp = ToDoubleOrNull(input, 0);
            if (temp == null)
                return null;
            return System.Convert.ToInt32(temp);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region ToLong(Convertir a un entero de 64 bits.)

    /// <summary>
    /// Convierte un objeto en un valor de tipo <see cref="long"/>.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="long"/>.</param>
    /// <returns>El valor convertido a <see cref="long"/>. Si la conversión falla, se devuelve 0.</returns>
    /// <remarks>
    /// Este método intenta convertir el objeto proporcionado a un valor de tipo <see cref="long"/>.
    /// Si el objeto es nulo o no se puede convertir, se devolverá 0 como valor predeterminado.
    /// </remarks>
    /// <seealso cref="ToLongOrNull(object)"/>
    public static long ToLong(object input)
    {
        return ToLongOrNull(input) ?? 0;
    }

    #endregion

    #region ToLongOrNull(Convertir a un entero de 64 bits nullable.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo <see cref="long"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="long"/>.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método intenta primero parsear el objeto como un <see cref="string"/> seguro.
    /// Si la conversión falla, intenta convertir el objeto a un <see cref="decimal"/> y luego a <see cref="long"/>.
    /// En caso de que cualquier conversión falle, se captura la excepción y se devuelve <c>null</c>.
    /// </remarks>
    /// <seealso cref="ToDecimalOrNull(object, decimal)"/>
    public static long? ToLongOrNull(object input)
    {
        var success = long.TryParse(input.SafeString(), out var result);
        if (success)
            return result;
        try
        {
            var temp = ToDecimalOrNull(input, 0);
            if (temp == null)
                return null;
            return System.Convert.ToInt64(temp);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region ToFloat(Convertir a tipo de punto flotante de 32 bits.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo float.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a float.</param>
    /// <param name="digits">Número opcional de dígitos decimales a considerar en la conversión.</param>
    /// <returns>El valor convertido a float. Si la conversión falla, se devuelve 0.</returns>
    public static float ToFloat(object input, int? digits = null)
    {
        return ToFloatOrNull(input, digits) ?? 0;
    }

    #endregion

    #region ToFloatOrNull(Convertir a tipo de punto flotante de 32 bits nullable.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo <see cref="float"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="float"/>.</param>
    /// <param name="digits">El número de decimales a los que se redondeará el resultado. Si es <c>null</c>, no se realizará redondeo.</param>
    /// <returns>
    /// Un valor de tipo <see cref="float"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="float.TryParse"/> para intentar convertir el objeto de entrada en un número de punto flotante.
    /// Si la conversión es exitosa y se especifica un número de dígitos, el resultado se redondeará a la cantidad de dígitos indicada.
    /// </remarks>
    /// <seealso cref="float"/>
    public static float? ToFloatOrNull(object input, int? digits = null)
    {
        var success = float.TryParse(input.SafeString(), out var result);
        if (!success)
            return null;
        if (digits == null)
            return result;
        return (float)Math.Round(result, digits.Value);
    }

    #endregion

    #region ToDouble(Convertir a tipo de punto flotante de 64 bits.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo <see cref="double"/>.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="double"/>.</param>
    /// <param name="digits">Número opcional de dígitos decimales a considerar en la conversión.</param>
    /// <returns>El valor convertido a <see cref="double"/>. Si la conversión falla, se devuelve 0.</returns>
    public static double ToDouble(object input, int? digits = null)
    {
        return ToDoubleOrNull(input, digits) ?? 0;
    }

    #endregion

    #region ToDoubleOrNull(Convertir a un tipo de punto flotante de 64 bits que puede ser nulo.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo <see cref="double"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="double"/>.</param>
    /// <param name="digits">El número de decimales a los que redondear el resultado. Si es <c>null</c>, no se realiza redondeo.</param>
    /// <returns>El valor convertido a <see cref="double"/> si la conversión es exitosa; de lo contrario, <c>null</c>.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="double.TryParse"/> para intentar convertir el objeto a un número de punto flotante.
    /// Si la conversión es exitosa y se proporciona un valor para <paramref name="digits"/>, el resultado se redondea a ese número de decimales.
    /// </remarks>
    /// <seealso cref="double"/>
    /// <seealso cref="Math.Round(double, int)"/>
    public static double? ToDoubleOrNull(object input, int? digits = null)
    {
        var success = double.TryParse(input.SafeString(), out var result);
        if (!success)
            return null;
        if (digits == null)
            return result;
        return Math.Round(result, digits.Value);
    }

    #endregion

    #region ToDecimal(Convertir a tipo de punto flotante de 128 bits.)

    /// <summary>
    /// Convierte un objeto a un valor decimal. Si la conversión falla, se devuelve 0.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a decimal.</param>
    /// <param name="digits">Número opcional de dígitos a considerar en la conversión.</param>
    /// <returns>El valor decimal convertido, o 0 si la conversión no es posible.</returns>
    public static decimal ToDecimal(object input, int? digits = null)
    {
        return ToDecimalOrNull(input, digits) ?? 0;
    }

    #endregion

    #region ToDecimalOrNull(Convertir a un tipo de punto flotante de 128 bits que puede ser nulo.)

    /// <summary>
    /// Convierte un objeto a un valor decimal o devuelve null si la conversión falla.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a decimal.</param>
    /// <param name="digits">Número opcional de dígitos a los que redondear el resultado. Si es null, no se realiza redondeo.</param>
    /// <returns>Un valor decimal redondeado si la conversión es exitosa; de lo contrario, null.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="decimal.TryParse"/> para intentar convertir el objeto a decimal.
    /// Si la conversión es exitosa y se proporciona un valor para <paramref name="digits"/>,
    /// el resultado se redondeará a la cantidad de dígitos especificada.
    /// </remarks>
    public static decimal? ToDecimalOrNull(object input, int? digits = null)
    {
        var success = decimal.TryParse(input.SafeString(), out var result);
        if (!success)
            return null;
        if (digits == null)
            return result;
        return Math.Round(result, digits.Value);
    }

    #endregion

    #region ToBool(Convertir a valor booleano.)

    /// <summary>
    /// Convierte un objeto en un valor booleano.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a booleano.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la conversión es exitosa; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool ToBool(object input)
    {
        return ToBoolOrNull(input) ?? false;
    }

    #endregion

    #region ToBoolOrNull(Convertir a un valor booleano nullable.)

    /// <summary>
    /// Convierte un objeto de entrada en un valor booleano o nulo.
    /// </summary>
    /// <param name="input">El objeto que se va a convertir a booleano.</param>
    /// <returns>
    /// Un valor booleano que representa el resultado de la conversión, 
    /// o null si la conversión no es posible.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el objeto de entrada a una cadena segura 
    /// y luego evalúa su contenido. Si el valor es "1", devuelve true; 
    /// si es "0", devuelve false. Para otros valores, intenta realizar 
    /// un análisis booleano utilizando <see cref="bool.TryParse"/>.
    /// </remarks>
    public static bool? ToBoolOrNull(object input)
    {
        var value = input.SafeString();
        switch (value)
        {
            case "1":
                return true;
            case "0":
                return false;
        }
        return bool.TryParse(value, out var result) ? result : null;
    }

    #endregion

    #region ToDateTime(Convertir a fecha.)

    /// <summary>
    /// Convierte un objeto en un valor de tipo <see cref="DateTime"/>.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="DateTime"/>.</param>
    /// <returns>
    /// Un valor de tipo <see cref="DateTime"/> que representa la fecha y hora del objeto proporcionado.
    /// Si la conversión falla, se devuelve <see cref="DateTime.MinValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el objeto de entrada utilizando el método <see cref="ToDateTimeOrNull"/>.
    /// Si el resultado es nulo, se retorna el valor mínimo de <see cref="DateTime"/>.
    /// </remarks>
    public static DateTime ToDateTime(object input)
    {
        return ToDateTimeOrNull(input) ?? DateTime.MinValue;
    }

    #endregion

    #region ToDateTimeOrNull(Convertir a fecha nullable.)

    /// <summary>
    /// Convierte un objeto a un valor de tipo <see cref="DateTime"/> o devuelve null si la conversión falla.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="DateTime"/>.</param>
    /// <returns>
    /// Un valor de tipo <see cref="DateTime"/> si la conversión es exitosa; de lo contrario, null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="DateTime.TryParse"/> para intentar realizar la conversión.
    /// Se espera que el objeto de entrada sea una cadena o un valor que pueda ser interpretado como una fecha y hora.
    /// </remarks>
    /// <seealso cref="DateTime"/>
    public static DateTime? ToDateTimeOrNull(object input)
    {
        var success = DateTime.TryParse(input.SafeString(), out var result);
        if (success == false)
            return null;
        return result;
    }

    #endregion

    #region ToGuid(Convertir a Guid.)

    /// <summary>
    /// Convierte un objeto en un <see cref="Guid"/>. 
    /// Si la conversión no es posible, devuelve <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="Guid"/>.</param>
    /// <returns>Un <see cref="Guid"/> resultante de la conversión o <see cref="Guid.Empty"/> si la conversión falla.</returns>
    /// <remarks>
    /// Este método intenta convertir el objeto proporcionado en un <see cref="Guid"/>. 
    /// Si el objeto es nulo o no puede ser convertido, se devuelve un <see cref="Guid"/> vacío.
    /// </remarks>
    public static Guid ToGuid(object input)
    {
        return ToGuidOrNull(input) ?? Guid.Empty;
    }

    #endregion

    #region ToGuidOrNull(Convertir a Guid nullable.)

    /// <summary>
    /// Convierte un objeto en un <see cref="Guid"/> o devuelve <c>null</c> si la conversión no es posible.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a <see cref="Guid"/>.</param>
    /// <returns>
    /// Un <see cref="Guid"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto de entrada es <c>null</c>. Si es así, devuelve <c>null</c>.
    /// Si el objeto es un arreglo de bytes, intenta crear un <see cref="Guid"/> a partir de esos bytes.
    /// Si el objeto no es un arreglo de bytes, intenta convertirlo a una cadena y luego a un <see cref="Guid"/>.
    /// </remarks>
    /// <seealso cref="Guid"/>
    public static Guid? ToGuidOrNull(object input)
    {
        if (input == null)
            return null;
        if (input.GetType() == typeof(byte[]))
            return new Guid((byte[])input);
        return Guid.TryParse(input.SafeString(), out var result) ? result : null;
    }

    #endregion

    #region ToGuidList(Convertir a un conjunto de Guid.)

    /// <summary>
    /// Convierte una cadena de texto en una lista de identificadores únicos globales (GUID).
    /// </summary>
    /// <param name="input">La cadena de texto que contiene los GUID separados por comas.</param>
    /// <returns>Una lista de GUIDs extraídos de la cadena de entrada.</returns>
    /// <remarks>
    /// Este método asume que la cadena de entrada está en un formato válido y que los GUID están separados por comas.
    /// Si la cadena no contiene GUID válidos, el método puede lanzar una excepción.
    /// </remarks>
    /// <seealso cref="ToList{T}(string)"/>
    public static List<Guid> ToGuidList(string input)
    {
        return ToList<Guid>(input);
    }

    #endregion

    #region ToBytes(Convertir a un arreglo de bytes.)

    /// <summary>
    /// Convierte una cadena de texto en un arreglo de bytes utilizando la codificación UTF-8.
    /// </summary>
    /// <param name="input">La cadena de texto que se desea convertir a bytes.</param>
    /// <returns>Un arreglo de bytes que representa la cadena de texto en formato UTF-8.</returns>
    public static byte[] ToBytes(string input)
    {
        return ToBytes(input, Encoding.UTF8);
    }

    /// <summary>
    /// Convierte una cadena de texto en un arreglo de bytes utilizando la codificación especificada.
    /// </summary>
    /// <param name="input">La cadena de texto que se desea convertir a bytes. Si es nula o está vacía, se devuelve un arreglo vacío.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena a bytes.</param>
    /// <returns>
    /// Un arreglo de bytes que representa la cadena de texto en la codificación especificada.
    /// Si la cadena de texto es nula o está vacía, se devuelve un arreglo vacío.
    /// </returns>
    public static byte[] ToBytes(string input, Encoding encoding)
    {
        return string.IsNullOrWhiteSpace(input) ? new byte[] { } : encoding.GetBytes(input);
    }

    #endregion

    #region ToBase64(Convertir a una cadena base64.)

    /// <summary>
    /// Convierte una cadena de texto en su representación en Base64.
    /// </summary>
    /// <param name="input">La cadena de texto que se desea convertir a Base64.</param>
    /// <returns>
    /// La representación en Base64 de la cadena de texto proporcionada, 
    /// o null si la cadena de entrada está vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la codificación UTF-8 para convertir la cadena de texto 
    /// en un arreglo de bytes antes de realizar la conversión a Base64.
    /// </remarks>
    public static string ToBase64(string input)
    {
        return input.IsEmpty() ? null : System.Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
    }

    #endregion

    #region ToList(Conversión de colecciones genéricas)

    /// <summary>
    /// Convierte una cadena de texto en una lista de elementos del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos que se agregarán a la lista.</typeparam>
    /// <param name="input">La cadena de texto que contiene los elementos separados por comas.</param>
    /// <returns>Una lista de elementos del tipo especificado, que se han extraído de la cadena de entrada.</returns>
    /// <remarks>
    /// Si la cadena de entrada está vacía o es nula, se devolverá una lista vacía.
    /// Los elementos de la cadena se separan por comas y se convierten al tipo especificado.
    /// </remarks>
    /// <seealso cref="To{T}(string)"/>
    public static List<T> ToList<T>(string input)
    {
        var result = new List<T>();
        if (string.IsNullOrWhiteSpace(input))
            return result;
        var array = input.Split(',');
        result.AddRange(from each in array where !string.IsNullOrWhiteSpace(each) select To<T>(each));
        return result;
    }

    #endregion

    #region To(Conversión genérica universal)

    /// <summary>
    /// Convierte un objeto de entrada a un tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir el objeto de entrada.</typeparam>
    /// <param name="input">El objeto que se desea convertir. Puede ser de cualquier tipo.</param>
    /// <returns>
    /// Retorna el objeto convertido al tipo especificado. Si la conversión falla o el objeto de entrada es nulo,
    /// se devuelve el valor predeterminado del tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método maneja varios tipos de conversión, incluyendo cadenas, enumeraciones y tipos que implementan
    /// <see cref="IConvertible"/>. También puede manejar objetos de tipo <see cref="JsonElement"/> para la
    /// deserialización de JSON.
    /// </remarks>
    /// <exception cref="InvalidCastException">
    /// Se lanza si la conversión no es posible y no se puede manejar adecuadamente.
    /// </exception>
    public static T To<T>(object input)
    {
        if (input == null)
            return default;
        if (input is string && string.IsNullOrWhiteSpace(input.ToString()))
            return default;
        var type = Common.GetType<T>();
        var typeName = type.Name.ToUpperInvariant();
        try
        {
            if (typeName == "STRING" || typeName == "GUID")
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(input.ToString());
            if (type.IsEnum)
                return Enum.Parse<T>(input);
            if (input is IConvertible)
                return (T)System.Convert.ChangeType(input, type, CultureInfo.InvariantCulture);
            if (input is JsonElement element)
            {
                var value = element.GetRawText();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return Json.ToObject<T>(value, options);
            }
            return (T)input;
        }
        catch
        {
            return default;
        }
    }

    #endregion

    #region ToDictionary(Objeto convertido en pares de nombre y valor de propiedad.)

    /// <summary>
    /// Convierte un objeto en un diccionario con claves de tipo string y valores de tipo object.
    /// </summary>
    /// <param name="data">El objeto que se desea convertir en un diccionario.</param>
    /// <returns>Un diccionario que representa el objeto proporcionado.</returns>
    /// <seealso cref="ToDictionary(object, bool)"/>
    public static IDictionary<string, object> ToDictionary(object data)
    {
        return ToDictionary(data, false);
    }

    /// <summary>
    /// Convierte un objeto en un diccionario de pares clave-valor.
    /// </summary>
    /// <param name="data">El objeto que se desea convertir en un diccionario.</param>
    /// <param name="useDisplayName">Indica si se deben usar los nombres de visualización de las propiedades en lugar de los nombres de las propiedades.</param>
    /// <returns>
    /// Un diccionario que contiene los nombres de las propiedades como claves y sus valores correspondientes.
    /// Si el objeto es nulo, se devuelve un diccionario vacío.
    /// </returns>
    /// <remarks>
    /// Si el objeto es una colección de pares clave-valor, se devuelve un nuevo diccionario con esos pares.
    /// En caso contrario, se obtienen las propiedades del objeto y se añaden al diccionario.
    /// </remarks>
    /// <seealso cref="GetPropertyDescriptorName(PropertyDescriptor, bool)"/>
    public static IDictionary<string, object> ToDictionary(object data, bool useDisplayName)
    {
        var result = new Dictionary<string, object>();
        if (data == null)
            return result;
        if (data is IEnumerable<KeyValuePair<string, object>> dic)
            return new Dictionary<string, object>(dic);
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(data))
        {
            var value = property.GetValue(data);
            result.Add(GetPropertyDescriptorName(property, useDisplayName), value);
        }
        return result;
    }

    /// <summary>
    /// Obtiene el nombre del descriptor de propiedad basado en las opciones especificadas.
    /// </summary>
    /// <param name="property">El descriptor de propiedad del cual se obtendrá el nombre.</param>
    /// <param name="useDisplayName">Indica si se debe utilizar el nombre para mostrar en lugar del nombre de la propiedad.</param>
    /// <returns>
    /// Devuelve el nombre de la propiedad, el nombre para mostrar o la descripción, según la configuración.
    /// Si <paramref name="useDisplayName"/> es verdadero, se prioriza la descripción y luego el nombre para mostrar.
    /// Si ambos son nulos o vacíos, se devuelve el nombre de la propiedad.
    /// </returns>
    private static string GetPropertyDescriptorName(PropertyDescriptor property, bool useDisplayName)
    {
        if (useDisplayName == false)
            return property.Name;
        if (string.IsNullOrEmpty(property.Description) == false)
            return property.Description;
        if (string.IsNullOrEmpty(property.DisplayName) == false)
            return property.DisplayName;
        return property.Name;
    }

    #endregion
}