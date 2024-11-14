using String = Util.Helpers.String;

namespace Util;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="string"/>.
/// </summary>
public static class StringExtensions {
    /// <summary>
    /// Elimina una cadena de inicio específica de una cadena dada.
    /// </summary>
    /// <param name="value">La cadena de la que se eliminará el inicio.</param>
    /// <param name="start">La cadena que se desea eliminar al inicio de <paramref name="value"/>.</param>
    /// <param name="ignoreCase">Indica si la comparación debe ignorar mayúsculas y minúsculas. El valor predeterminado es <c>true</c>.</param>
    /// <returns>
    /// Una nueva cadena con el inicio especificado eliminado si se encuentra; de lo contrario, devuelve la cadena original.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> y permite eliminar un prefijo de manera sencilla.
    /// </remarks>
    public static string RemoveStart( this string value, string start, bool ignoreCase = true ) {
        return String.RemoveStart( value, start, ignoreCase );
    }

    /// <summary>
    /// Elimina una cadena especificada del final de la cadena actual.
    /// </summary>
    /// <param name="value">La cadena de la que se eliminará el final.</param>
    /// <param name="end">La cadena que se desea eliminar del final de <paramref name="value"/>.</param>
    /// <param name="ignoreCase">Indica si la comparación debe ignorar mayúsculas y minúsculas. El valor predeterminado es <c>true</c>.</param>
    /// <returns>
    /// Una nueva cadena que representa el valor original sin la cadena especificada al final. 
    /// Si <paramref name="value"/> no termina con <paramref name="end"/>, se devuelve <paramref name="value"/> sin cambios.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="string"/> y permite eliminar de manera sencilla
    /// una subcadena del final de una cadena, considerando la opción de ignorar mayúsculas y minúsculas.
    /// </remarks>
    public static string RemoveEnd( this string value, string end, bool ignoreCase = true ) {
        return String.RemoveEnd( value, end, ignoreCase );
    }

    /// <summary>
    /// Elimina el prefijo especificado de la cadena representada por el objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="value">El objeto <see cref="StringBuilder"/> del cual se eliminará el prefijo.</param>
    /// <param name="start">La cadena que se desea eliminar al inicio del objeto <see cref="StringBuilder"/>.</param>
    /// <returns>Un nuevo <see cref="StringBuilder"/> sin el prefijo especificado, si estaba presente; de lo contrario, devuelve el objeto original.</returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="StringBuilder"/> y permite manipular cadenas de manera más eficiente.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    public static StringBuilder RemoveStart( this StringBuilder value, string start ) {
        return String.RemoveStart( value, start );
    }

    /// <summary>
    /// Elimina el sufijo especificado de la instancia de <see cref="StringBuilder"/> si está presente.
    /// </summary>
    /// <param name="value">La instancia de <see cref="StringBuilder"/> de la cual se eliminará el sufijo.</param>
    /// <param name="end">El sufijo que se desea eliminar.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="StringBuilder"/> sin el sufijo especificado si estaba presente; 
    /// de lo contrario, devuelve la instancia original sin cambios.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="StringBuilder"/> y permite manipular cadenas de texto 
    /// de manera más eficiente al eliminar un sufijo específico.
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    public static StringBuilder RemoveEnd( this StringBuilder value, string end ) {
        return String.RemoveEnd( value, end );
    }

    /// <summary>
    /// Elimina el texto especificado al inicio del contenido de un objeto <see cref="StringWriter"/>.
    /// </summary>
    /// <param name="writer">El objeto <see cref="StringWriter"/> del cual se eliminará el texto inicial.</param>
    /// <param name="start">El texto que se desea eliminar del inicio del contenido del <see cref="StringWriter"/>.</param>
    /// <returns>
    /// Devuelve el mismo objeto <see cref="StringWriter"/> después de realizar la eliminación, 
    /// o <c>null</c> si el objeto <paramref name="writer"/> es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la clase <see cref="StringWriter"/> 
    /// al permitir la eliminación de un prefijo específico del contenido.
    /// </remarks>
    public static StringWriter RemoveStart( this StringWriter writer, string start ) {
        if ( writer == null )
            return null;
        var builder = writer.GetStringBuilder();
        builder.RemoveStart( start );
        return writer;
    }

    /// <summary>
    /// Elimina una cadena específica del final del contenido de un objeto <see cref="StringWriter"/>.
    /// </summary>
    /// <param name="writer">El objeto <see cref="StringWriter"/> del cual se eliminará la cadena.</param>
    /// <param name="end">La cadena que se desea eliminar del final del contenido del <see cref="StringWriter"/>.</param>
    /// <returns>
    /// El mismo objeto <see cref="StringWriter"/> después de haber eliminado la cadena especificada,
    /// o <c>null</c> si el <paramref name="writer"/> es <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método modifica el contenido del <see cref="StringWriter"/> original. Si la cadena
    /// especificada no se encuentra al final del contenido, no se realiza ninguna modificación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="end"/> es <c>null</c>.</exception>
    public static StringWriter RemoveEnd( this StringWriter writer, string end ) {
        if ( writer == null )
            return null;
        var builder = writer.GetStringBuilder();
        builder.RemoveEnd( end );
        return writer;
    }
}