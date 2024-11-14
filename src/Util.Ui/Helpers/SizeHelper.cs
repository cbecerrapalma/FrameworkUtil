namespace Util.Ui.Helpers;

/// <summary>
/// Proporciona métodos de ayuda para trabajar con tamaños.
/// </summary>
public class SizeHelper {
    /// <summary>
    /// Obtiene un valor formateado en píxeles si el valor proporcionado es un número.
    /// </summary>
    /// <param name="value">El valor que se va a evaluar y formatear.</param>
    /// <returns>
    /// Retorna el valor formateado como una cadena en píxeles si el valor es un número, 
    /// de lo contrario, retorna el valor original. Si el valor está vacío, retorna null.
    /// </returns>
    public static string GetValue( string value ) {
        if ( value.IsEmpty() )
            return null;
        return Util.Helpers.Validation.IsNumber( value ) ? $"{value}px" : value;
    }
}