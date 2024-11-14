namespace Util.Data.Metadata; 

/// <summary>
/// Define un contrato para la conversión de tipos.
/// </summary>
public interface ITypeConverter {
    /// <summary>
    /// Convierte un tipo de dato en formato de cadena a un tipo de dato de base de datos.
    /// </summary>
    /// <param name="dataType">El tipo de dato en formato de cadena que se desea convertir.</param>
    /// <param name="length">La longitud opcional del tipo de dato, si aplica.</param>
    /// <returns>Un valor de tipo <see cref="DbType"/> que representa el tipo de dato convertido, o null si no se puede convertir.</returns>
    /// <remarks>
    /// Este método es útil para mapear tipos de datos de aplicaciones a tipos de datos de bases de datos,
    /// permitiendo una mayor flexibilidad en la manipulación de datos.
    /// </remarks>
    DbType? ToType( string dataType, int? length = null );
}