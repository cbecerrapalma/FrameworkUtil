namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Interfaz que representa una cláusula FROM en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y define los métodos y propiedades
/// necesarios para construir una cláusula FROM en una consulta SQL.
/// </remarks>
public interface IFromClause : ISqlClause {
    /// <summary>
    /// Establece la tabla de origen para la operación.
    /// </summary>
    /// <param name="table">El nombre de la tabla desde la cual se realizará la operación.</param>
    /// <remarks>
    /// Este método permite especificar una tabla específica en la base de datos 
    /// que se utilizará en las consultas o manipulaciones posteriores. 
    /// Asegúrese de que el nombre de la tabla sea válido y exista en la base de datos.
    /// </remarks>
    void From(string table);
    /// <summary>
    /// Establece la fuente de datos para la consulta utilizando un generador de SQL.
    /// </summary>
    /// <param name="builder">El generador de SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la fuente de datos en la consulta.</param>
    void From( ISqlBuilder builder, string alias );
    /// <summary>
    /// Ejecuta una acción sobre un generador de SQL con un alias especificado.
    /// </summary>
    /// <param name="action">La acción que se ejecutará sobre el generador de SQL.</param>
    /// <param name="alias">El alias que se utilizará en la construcción de la consulta SQL.</param>
    /// <remarks>
    /// Este método permite personalizar la construcción de una consulta SQL mediante el uso de un generador de SQL.
    /// El alias se puede utilizar para referirse a tablas o columnas de manera más legible en la consulta.
    /// </remarks>
    void From( Action<ISqlBuilder> action, string alias );
    /// <summary>
    /// Agrega una cadena SQL a una colección de consultas.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como una consulta en bruto.</param>
    /// <remarks>
    /// Este método permite agregar consultas SQL a un sistema de gestión de bases de datos.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la cadena SQL se agrega sin ningún tipo de procesamiento adicional.
    /// De lo contrario, se puede aplicar algún tipo de validación o transformación antes de agregarla.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void AppendSql( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos o datos almacenados, dejando el estado inicial.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula FROM actual, creando una nueva instancia que se puede modificar sin afectar a la original.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <returns>Una nueva instancia de <see cref="IFromClause"/> que representa la cláusula FROM clonada.</returns>
    /// <remarks>
    /// Este método es útil para crear variaciones de consultas SQL sin modificar la cláusula original.
    /// </remarks>
    IFromClause Clone(SqlBuilderBase builder);
}