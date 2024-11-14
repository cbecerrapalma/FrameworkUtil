namespace Util.Data.Sql.Builders.Clauses; 

/// <summary>
/// Define una cláusula de agrupamiento en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta interfaz es parte de la construcción de consultas SQL y permite especificar cómo se deben agrupar los resultados.
/// </remarks>
public interface IGroupByClause : ISqlClause {
    /// <summary>
    /// Agrupa los datos según las columnas especificadas.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas por las cuales se realizará el agrupamiento.</param>
    /// <remarks>
    /// Este método permite agrupar los datos en función de las columnas proporcionadas. 
    /// Asegúrate de que las columnas especificadas existan en el conjunto de datos.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void GroupBy( string columns );
    /// <summary>
    /// Establece una condición para una consulta utilizando una expresión, un valor y un operador.
    /// </summary>
    /// <param name="expression">La expresión que se evaluará en la condición.</param>
    /// <param name="value">El valor que se comparará con la expresión.</param>
    /// <param name="@operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta. El valor por defecto es <c>true</c>.</param>
    /// <remarks>
    /// Este método permite construir condiciones dinámicas para consultas, facilitando la creación de filtros
    /// en base a diferentes criterios. La parametrización ayuda a prevenir ataques de inyección SQL.
    /// </remarks>
    /// <seealso cref="Operator"/>
    void Having( string expression, object value, Operator @operator = Operator.Equal,bool isParameterization = true );
    /// <summary>
    /// Agrega una cláusula GROUP BY a la consulta SQL proporcionada.
    /// </summary>
    /// <param name="sql">La consulta SQL a la que se le añadirá la cláusula GROUP BY.</param>
    /// <param name="raw">Indica si la cláusula GROUP BY debe ser añadida en su forma cruda.</param>
    /// <remarks>
    /// Este método modifica la consulta SQL original para incluir la cláusula GROUP BY,
    /// permitiendo agrupar los resultados según los criterios especificados.
    /// </remarks>
    /// <seealso cref="System.String"/>
    void AppendGroupBy( string sql, bool raw );
    /// <summary>
    /// Agrega una cláusula HAVING a una consulta SQL existente.
    /// </summary>
    /// <param name="sql">La consulta SQL a la que se le añadirá la cláusula HAVING.</param>
    /// <param name="raw">Indica si la cláusula HAVING debe ser añadida en formato crudo.</param>
    /// <remarks>
    /// Este método permite modificar una consulta SQL existente para incluir condiciones adicionales
    /// que se aplican a los resultados agregados. La cláusula HAVING se utiliza comúnmente en
    /// combinación con funciones de agregación como COUNT, SUM, AVG, etc.
    /// </remarks>
    /// <example>
    /// <code>
    /// string consulta = "SELECT COUNT(*) FROM Ventas GROUP BY Producto";
    /// AppendHaving(consulta, true);
    /// </code>
    /// </example>
    void AppendHaving( string sql, bool raw );
    /// <summary>
    /// Limpia el contenido actual.
    /// </summary>
    /// <remarks>
    /// Este método restablece el estado del objeto, eliminando cualquier dato o configuración previa.
    /// </remarks>
    void Clear();
    /// <summary>
    /// Clona la cláusula de agrupamiento actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para la clonación.</param>
    /// <returns>Una nueva instancia de <see cref="IGroupByClause"/> que es una copia de la cláusula actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia de la cláusula de agrupamiento, lo que puede ser útil
    /// cuando se necesita modificar la cláusula sin afectar la original.
    /// </remarks>
    IGroupByClause Clone(SqlBuilderBase builder);
}