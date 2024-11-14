namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Interfaz que representa una cláusula SELECT en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlClause"/> y define el comportamiento
/// específico para las cláusulas SELECT en una consulta SQL.
/// </remarks>
public interface ISelectClause : ISqlClause {
    /// <summary>
    /// Realiza la selección de un elemento o conjunto de elementos.
    /// </summary>
    /// <remarks>
    /// Este método puede ser utilizado para seleccionar elementos en una interfaz de usuario 
    /// o en una colección de datos. La implementación específica de la selección dependerá 
    /// del contexto en el que se utilice.
    /// </remarks>
    void Select();
    /// <summary>
    /// Selecciona las columnas especificadas para la consulta.
    /// </summary>
    /// <param name="columns">Una cadena que contiene los nombres de las columnas a seleccionar, separados por comas.</param>
    /// <remarks>
    /// Este método permite especificar qué columnas se desean incluir en la consulta. 
    /// Si se pasan columnas no válidas, se puede lanzar una excepción.
    /// </remarks>
    void Select(string columns);
    /// <summary>
    /// Selecciona datos utilizando el constructor SQL especificado.
    /// </summary>
    /// <param name="builder">El constructor SQL que se utilizará para crear la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla o conjunto de resultados en la consulta.</param>
    /// <remarks>
    /// Este método permite personalizar la selección de datos en una consulta SQL.
    /// Asegúrese de que el alias proporcionado sea único dentro del contexto de la consulta.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void Select( ISqlBuilder builder, string alias );
    /// <summary>
    /// Selecciona una acción para construir una consulta SQL utilizando un generador SQL.
    /// </summary>
    /// <param name="action">La acción que se aplicará al generador SQL.</param>
    /// <param name="alias">El alias que se utilizará en la consulta SQL.</param>
    /// <remarks>
    /// Este método permite personalizar la construcción de una consulta SQL mediante el uso de un generador SQL.
    /// El alias proporcionado se puede utilizar para referirse a la consulta en otras partes de la misma.
    /// </remarks>
    /// <example>
    /// <code>
    /// Select(builder => builder.Select("ColumnName"), "t");
    /// </code>
    /// </example>
    void Select( Action<ISqlBuilder> action, string alias );
    /// <summary>
    /// Agrega una cadena SQL a una colección existente.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como texto sin procesar.</param>
    /// <remarks>
    /// Este método permite concatenar una nueva instrucción SQL a la colección actual.
    /// Si el parámetro <paramref name="raw"/> es verdadero, la cadena SQL se agrega sin ningún tipo de procesamiento adicional.
    /// De lo contrario, se pueden aplicar transformaciones o validaciones antes de agregarla.
    /// </remarks>
    /// <example>
    /// <code>
    /// AppendSql("SELECT * FROM Usuarios", true);
    /// </code>
    /// </example>
    void AppendSql( string sql, bool raw );
    /// <summary>
    /// Agrega una instrucción SQL al constructor de consultas.
    /// </summary>
    /// <param name="builder">El constructor de SQL al que se le agregará la instrucción.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas SQL utilizando el objeto <paramref name="builder"/>.
    /// Asegúrese de que el objeto <paramref name="builder"/> esté correctamente inicializado antes de llamar a este método.
    /// </remarks>
    void AppendSql( ISqlBuilder builder );
    /// <summary>
    /// Agrega una acción que modifica el generador de SQL.
    /// </summary>
    /// <param name="action">La acción que se aplicará al generador de SQL.</param>
    /// <remarks>
    /// Este método permite personalizar la construcción de una consulta SQL mediante el uso de un generador de SQL.
    /// La acción proporcionada se ejecutará con una instancia de <see cref="ISqlBuilder"/> que se puede utilizar para 
    /// agregar cláusulas, condiciones y otros elementos a la consulta.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    void AppendSql( Action<ISqlBuilder> action );
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece el estado del objeto, eliminando todos los datos almacenados.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de selección actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utiliza para construir la consulta SQL.</param>
    /// <returns>Una nueva instancia de <see cref="ISelectClause"/> que es una copia de la cláusula de selección actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de selección, lo que puede ser útil para modificar la cláusula sin afectar la original.
    /// </remarks>
    ISelectClause Clone(SqlBuilderBase builder);
}