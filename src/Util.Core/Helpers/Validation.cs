namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos estáticos para la validación de datos.
/// </summary>
public static class Validation {
    /// <summary>
    /// Verifica si la cadena de entrada representa un número válido.
    /// </summary>
    /// <param name="input">La cadena que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena representa un número; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método considera números enteros y decimales, incluyendo números negativos.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool resultado = IsNumber("-123.45"); // resultado será true
    /// bool resultado2 = IsNumber("abc"); // resultado2 será false
    /// </code>
    /// </example>
    public static bool IsNumber( string input ) {
        if( input.IsEmpty() )
            return false;
        const string pattern = @"^(-?\d*)(\.\d+)?$";
        return Regex.IsMatch( input, pattern );
    }
}