namespace Util.Data.Sql.Builders.Caches; 

/// <summary>
/// Define una interfaz para el almacenamiento en caché de columnas.
/// </summary>
public interface IColumnCache {
    /// <summary>
    /// Obtiene una cadena de columnas seguras a partir de una cadena de columnas dada.
    /// </summary>
    /// <param name="columns">La cadena que contiene los nombres de las columnas, separados por comas.</param>
    /// <returns>Una cadena que contiene los nombres de las columnas que son consideradas seguras.</returns>
    /// <remarks>
    /// Este método valida cada nombre de columna en la cadena de entrada y devuelve solo aquellos que cumplen con 
    /// los criterios de seguridad definidos. Los nombres de columnas que no son seguros se excluyen del resultado.
    /// </remarks>
    string GetSafeColumns( string columns );
    /// <summary>
    /// Obtiene una versión segura del nombre de la columna proporcionada.
    /// </summary>
    /// <param name="column">El nombre de la columna que se desea hacer seguro.</param>
    /// <returns>Una cadena que representa el nombre de la columna en un formato seguro.</returns>
    /// <remarks>
    /// Este método puede realizar validaciones o transformaciones en el nombre de la columna
    /// para asegurar que sea compatible con el sistema o la base de datos en uso.
    /// </remarks>
    string GetSafeColumn( string column );
}