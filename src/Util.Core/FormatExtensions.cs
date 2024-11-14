using Util.Properties;

namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para formatear cadenas.
/// </summary>
public static class FormatExtensions {
    /// <summary>
    /// Convierte un valor booleano en su representación de cadena correspondiente.
    /// </summary>
    /// <param name="value">El valor booleano a convertir.</param>
    /// <returns>
    /// Una cadena que representa el valor booleano: 
    /// "Sí" si el valor es verdadero, o "No" si el valor es falso.
    /// </returns>
    public static string Description( this bool value ) {
        return value ? R.Yes : R.No;
    }

    /// <summary>
    /// Devuelve una descripción basada en el valor booleano proporcionado.
    /// </summary>
    /// <param name="value">El valor booleano nullable que se va a evaluar.</param>
    /// <returns>
    /// Una cadena que representa la descripción del valor booleano. 
    /// Devuelve una cadena vacía si el valor es null.
    /// </returns>
    public static string Description( this bool? value ) {
        return value == null ? "" : Description( value.Value );
    }
}