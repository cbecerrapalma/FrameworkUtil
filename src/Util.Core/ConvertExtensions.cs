namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para la conversión de tipos.
/// </summary>
public static class ConvertExtensions {
    /// <summary>
    /// Convierte un objeto en una cadena de texto segura.
    /// </summary>
    /// <param name="input">El objeto que se desea convertir a cadena.</param>
    /// <returns>Una cadena que representa el objeto, o una cadena vacía si el objeto es nulo.</returns>
    /// <remarks>
    /// Este método utiliza el operador de coalescencia nula para devolver una cadena vacía en caso de que el objeto de entrada sea nulo.
    /// También se asegura de eliminar los espacios en blanco al principio y al final de la cadena resultante.
    /// </remarks>
    public static string SafeString( this object input ) {
        return input?.ToString()?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Convierte una cadena en un valor booleano.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a booleano.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena representa un valor verdadero; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> y utiliza un método de conversión auxiliar 
    /// para realizar la conversión.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert.ToBool(string)"/>
    public static bool ToBool( this string obj ) {
        return Util.Helpers.Convert.ToBool( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor booleano o nulo.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a booleano.</param>
    /// <returns>
    /// Un valor booleano si la conversión es exitosa; de lo contrario, null.
    /// </returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="string"/> y utiliza un método auxiliar 
    /// de la clase <see cref="Util.Helpers.Convert"/> para realizar la conversión.
    /// </remarks>
    public static bool? ToBoolOrNull( this string obj ) {
        return Util.Helpers.Convert.ToBoolOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en un entero.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a un entero.</param>
    /// <returns>El valor entero resultante de la conversión.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene el formato correcto para un entero.</exception>
    /// <exception cref="ArgumentNullException">Se produce si la cadena es nula.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToInt(string)"/>
    public static int ToInt( this string obj ) {
        return Util.Helpers.Convert.ToInt( obj );
    }

    /// <summary>
    /// Convierte una cadena en un entero nullable (int?).
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a un entero. Puede ser nula.</param>
    /// <returns>
    /// Un entero nullable que representa el valor convertido de la cadena, 
    /// o null si la conversión no es posible o si la cadena es nula o vacía.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> y permite 
    /// realizar conversiones seguras de cadenas a enteros, evitando excepciones 
    /// en caso de que la cadena no sea un número válido.
    /// </remarks>
    public static int? ToIntOrNull( this string obj ) {
        return Util.Helpers.Convert.ToIntOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor de tipo <see cref="long"/>.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="long"/>.</param>
    /// <returns>El valor convertido de tipo <see cref="long"/>.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene el formato correcto.</exception>
    /// <exception cref="ArgumentNullException">Se produce si la cadena es <c>null</c>.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToLong(string)"/>
    public static long ToLong( this string obj ) {
        return Util.Helpers.Convert.ToLong( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor de tipo <see cref="long"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="long"/>.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> y permite realizar la conversión de manera segura,
    /// evitando excepciones en caso de que la cadena no sea un número válido.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert.ToLongOrNull(string)"/>
    public static long? ToLongOrNull( this string obj ) {
        return Util.Helpers.Convert.ToLongOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor de tipo <see cref="double"/>.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="double"/>.</param>
    /// <returns>El valor de tipo <see cref="double"/> resultante de la conversión.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene el formato correcto para un número.</exception>
    /// <exception cref="ArgumentNullException">Se produce si la cadena es <c>null</c>.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToDouble(string)"/>
    public static double ToDouble( this string obj ) {
        return Util.Helpers.Convert.ToDouble( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor de tipo <see cref="double"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="double"/>.</param>
    /// <returns>
    /// Un valor de tipo <see cref="double"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="string"/> y utiliza un método auxiliar de la clase <see cref="Util.Helpers.Convert"/>.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert.ToDoubleOrNull(string)"/>
    public static double? ToDoubleOrNull( this string obj ) {
        return Util.Helpers.Convert.ToDoubleOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor decimal.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a decimal.</param>
    /// <returns>El valor decimal resultante de la conversión de la cadena.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene el formato correcto para un número decimal.</exception>
    /// <exception cref="ArgumentNullException">Se produce si la cadena es null.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToDecimal(string)"/>
    public static decimal ToDecimal( this string obj ) {
        return Util.Helpers.Convert.ToDecimal( obj );
    }

    /// <summary>
    /// Convierte una cadena en un valor decimal nullable.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a decimal.</param>
    /// <returns>
    /// Un valor decimal nullable que representa la cadena convertida, 
    /// o null si la conversión falla o la cadena es nula o vacía.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> 
    /// y utiliza el método <see cref="Util.Helpers.Convert.ToDecimalOrNull"/> 
    /// para realizar la conversión.
    /// </remarks>
    public static decimal? ToDecimalOrNull( this string obj ) {
        return Util.Helpers.Convert.ToDecimalOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena de texto en un objeto <see cref="DateTime"/>.
    /// </summary>
    /// <param name="obj">La cadena que representa la fecha y hora a convertir.</param>
    /// <returns>Un objeto <see cref="DateTime"/> que representa la fecha y hora especificadas en la cadena.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene un formato válido para una fecha y hora.</exception>
    /// <exception cref="ArgumentNullException">Se produce si la cadena es <c>null</c>.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToDateTime(string)"/>
    public static DateTime ToDateTime( this string obj ) {
        return Util.Helpers.Convert.ToDateTime( obj );
    }

    /// <summary>
    /// Convierte una cadena en un objeto <see cref="DateTime"/> o devuelve <c>null</c> si la conversión falla.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="DateTime"/>.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="string"/> y permite realizar conversiones de forma segura,
    /// evitando excepciones en caso de que la cadena no tenga un formato válido.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert.ToDateTimeOrNull(string)"/>
    public static DateTime? ToDateTimeOrNull( this string obj ) {
        return Util.Helpers.Convert.ToDateTimeOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en un objeto Guid.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a Guid.</param>
    /// <returns>Un objeto Guid que representa la cadena proporcionada.</returns>
    /// <exception cref="FormatException">Se produce si la cadena no tiene el formato correcto para un Guid.</exception>
    /// <seealso cref="Util.Helpers.Convert.ToGuid(string)"/>
    public static Guid ToGuid( this string obj ) {
        return Util.Helpers.Convert.ToGuid( obj );
    }

    /// <summary>
    /// Convierte una cadena en un objeto <see cref="Guid"/> o devuelve <c>null</c> si la cadena no es válida.
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a <see cref="Guid"/>.</param>
    /// <returns>
    /// Un objeto <see cref="Guid"/> si la conversión es exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="string"/> y permite convertir cadenas que representan un <see cref="Guid"/> 
    /// en un objeto <see cref="Guid"/> o en <c>null</c> si la cadena no tiene un formato válido.
    /// </remarks>
    /// <seealso cref="Util.Helpers.Convert.ToGuidOrNull(string)"/>
    public static Guid? ToGuidOrNull( this string obj ) {
        return Util.Helpers.Convert.ToGuidOrNull( obj );
    }

    /// <summary>
    /// Convierte una cadena en una lista de identificadores únicos globales (GUID).
    /// </summary>
    /// <param name="obj">La cadena que se desea convertir a una lista de GUID.</param>
    /// <returns>
    /// Una lista de GUID generada a partir de la cadena proporcionada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Util.Helpers.Convert"/> para realizar la conversión.
    /// Asegúrese de que la cadena de entrada esté en un formato válido para evitar excepciones.
    /// </remarks>
    public static List<Guid> ToGuidList( this string obj ) {
        return Util.Helpers.Convert.ToGuidList( obj );
    }

    /// <summary>
    /// Convierte una lista de cadenas en una lista de GUIDs.
    /// </summary>
    /// <param name="obj">La lista de cadenas a convertir.</param>
    /// <returns>Una lista de GUIDs generados a partir de las cadenas proporcionadas. Si la lista de entrada es nula, se devuelve una lista vacía.</returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="IList{T}"/> para permitir la conversión directa de cadenas a GUIDs.
    /// Cada cadena se convierte utilizando el método <see cref="ToGuid()"/>. 
    /// Asegúrese de que las cadenas sean representaciones válidas de GUIDs para evitar excepciones durante la conversión.
    /// </remarks>
    public static List<Guid> ToGuidList( this IList<string> obj ) {
        if( obj == null )
            return new List<Guid>();
        return obj.Select( t => t.ToGuid() ).ToList();
    }
}