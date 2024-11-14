namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Define una cláusula de ordenamiento para una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y debe ser implementada por clases que 
/// proporcionen funcionalidad para especificar el orden de los resultados en una consulta SQL.
/// </remarks>
public interface IOrderByClause : ISqlClause {
    /// <summary>
    /// Ordena los elementos de acuerdo con el criterio especificado.
    /// </summary>
    /// <param name="order">Una cadena que representa el criterio de ordenamiento. Puede ser 'asc' para ascendente o 'desc' para descendente.</param>
    /// <remarks>
    /// Este método permite al usuario especificar cómo se deben ordenar los elementos. 
    /// Asegúrese de que el valor proporcionado en el parámetro <paramref name="order"/> sea válido 
    /// para evitar excepciones durante la ejecución.
    /// </remarks>
    /// <seealso cref="OrderByDescending(string)"/>
    void OrderBy( string order );
    /// <summary>
    /// Agrega una cadena SQL a una colección existente.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite construir consultas SQL dinámicamente al agregar nuevas cadenas SQL.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la cadena SQL se agrega tal cual, 
    /// sin ningún tipo de procesamiento adicional. Si es falso, se pueden aplicar ciertas 
    /// transformaciones o validaciones a la cadena antes de agregarla.
    /// </remarks>
    void AppendSql( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido o estado del objeto actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece todas las propiedades y campos del objeto a sus valores predeterminados.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de ordenación actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>Una nueva instancia de <see cref="IOrderByClause"/> que representa la cláusula de ordenación clonada.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de ordenación actual, 
    /// lo que puede ser útil para modificar la cláusula sin afectar a la original.
    /// </remarks>
    IOrderByClause Clone(SqlBuilderBase builder);
}