using Util.Data.Sql.Builders.Core;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula FROM en una consulta.
/// </summary>
/// <remarks>
/// Esta clase es parte de la estructura de una consulta y se utiliza para especificar la fuente de datos 
/// desde la cual se obtendrán los resultados. Puede incluir tablas, vistas o subconsultas.
/// </remarks>
public class FromClause : ClauseBase, IFromClause
{

    #region Campo

    protected readonly StringBuilder Result;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FromClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> opcional que contendrá el resultado de la construcción de la consulta. Si es <c>null</c>, se inicializa uno nuevo.</param>
    public FromClause(SqlBuilderBase sqlBuilder, StringBuilder result = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
    }

    #endregion

    #region From(Configuración de la tabla.)

    /// <inheritdoc />
    /// <summary>
    /// Convierte una cadena que representa una tabla en un objeto y lo agrega a la colección de resultados.
    /// </summary>
    /// <param name="table">La cadena que representa la tabla a convertir.</param>
    /// <remarks>
    /// Si la cadena proporcionada está vacía, el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="CreateTableItem(string)"/>
    /// <seealso cref="Result"/>
    public void From(string table)
    {
        if (table.IsEmpty())
            return;
        var item = CreateTableItem(table);
        item.AppendTo(Result);
    }

    /// <summary>
    /// Crea un nuevo elemento de tabla.
    /// </summary>
    /// <param name="table">El nombre de la tabla para el elemento que se va a crear.</param>
    /// <returns>Un nuevo objeto <see cref="TableItem"/> que representa la tabla especificada.</returns>
    protected virtual TableItem CreateTableItem(string table)
    {
        return new(Dialect, table);
    }

    /// <inheritdoc />
    /// <summary>
    /// Convierte un objeto <see cref="ISqlBuilder"/> en un elemento SQL y lo agrega al resultado.
    /// </summary>
    /// <param name="builder">El objeto <see cref="ISqlBuilder"/> que se va a convertir.</param>
    /// <param name="alias">El alias que se asignará al elemento SQL.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="builder"/> es nulo, la función no realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="SqlBuilderItem"/>
    public void From(ISqlBuilder builder, string alias)
    {
        if (builder == null)
            return;
        var item = new SqlBuilderItem(Dialect, builder, alias);
        item.AppendTo(Result);
    }

    /// <inheritdoc />
    /// <summary>
    /// Configura la cláusula FROM de una consulta SQL utilizando una acción que construye el SQL.
    /// </summary>
    /// <param name="action">Una acción que recibe un <see cref="ISqlBuilder"/> para construir la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará en la cláusula FROM.</param>
    /// <remarks>
    /// Si la acción proporcionada es nula, el método no realiza ninguna operación.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void From(Action<ISqlBuilder> action, string alias)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        From(builder, alias);
    }

    #endregion

    #region AppendSql(Agregar al cláusula From.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cadena SQL al resultado, procesándola según el parámetro especificado.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar al resultado.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser agregada sin procesar (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si la cadena SQL está vacía o es nula, no se realizará ninguna acción.
    /// Si el parámetro <paramref name="raw"/> es verdadero, se agrega la cadena SQL tal cual.
    /// Si el parámetro <paramref name="raw"/> es falso, se procesa la cadena SQL antes de agregarla.
    /// </remarks>
    public void AppendSql(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            Result.Append(sql);
            return;
        }
        Result.Append(ReplaceRawSql(sql));
    }

    #endregion

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado tiene una longitud mayor a cero.
    /// </summary>
    /// <returns>
    /// Devuelve true si el resultado tiene uno o más elementos; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay resultados disponibles antes de proceder con otras operaciones.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega información al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la información.</param>
    /// <remarks>
    /// Este método valida el estado actual antes de realizar la operación de adición. 
    /// Si la validación falla, no se realizará ninguna modificación en el <paramref name="builder"/>.
    /// </remarks>
    /// <seealso cref="Validate"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("From ");
        builder.Append(Result);
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia todos los elementos de la colección de resultados.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la propiedad <see cref="Result"/>.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
    }

    #endregion

    #region Clone(Copie la cláusula De)

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia del objeto actual de la cláusula FROM.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <returns>Una nueva instancia de <see cref="IFromClause"/> que representa la cláusula FROM clonada.</returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StringBuilder"/> para construir el resultado de la cláusula FROM
    /// a partir del estado actual del objeto. La implementación de clonación permite crear una nueva
    /// cláusula sin modificar la original.
    /// </remarks>
    /// <seealso cref="IFromClause"/>
    /// <seealso cref="SqlBuilderBase"/>
    public virtual IFromClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new FromClause(builder, result);
    }

    #endregion
}