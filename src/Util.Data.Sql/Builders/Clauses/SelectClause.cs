using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de selección en una consulta.
/// </summary>
/// <remarks>
/// Esta clase es parte de la construcción de consultas y se utiliza para definir
/// qué columnas o expresiones se seleccionarán en una consulta SQL.
/// </remarks>
public class SelectClause : ClauseBase, ISelectClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected readonly IColumnCache ColumnCache;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SelectClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> opcional que contendrá el resultado de la cláusula SELECT. Si se proporciona como null, se inicializará uno nuevo.</param>
    public SelectClause(SqlBuilderBase sqlBuilder, StringBuilder result = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
    }

    #endregion

    #region Select(Configurar nombres de columnas.)

    /// <inheritdoc />
    /// <summary>
    /// Selecciona un elemento y lo marca como activo.
    /// </summary>
    /// <remarks>
    /// Este método limpia el contenido actual de <see cref="Result"/> 
    /// y agrega un asterisco (*) al principio, indicando que el elemento ha sido seleccionado.
    /// </remarks>
    /// <seealso cref="Result"/>
    public void Select()
    {
        Result.Clear();
        Result.Append("*");
    }

    /// <inheritdoc />
    /// <summary>
    /// Selecciona las columnas especificadas para la consulta.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas a seleccionar. 
    /// Puede ser una lista de nombres de columnas separados por comas o un asterisco (*) 
    /// para seleccionar todas las columnas.</param>
    /// <remarks>
    /// Si la cadena de columnas está vacía, no se realiza ninguna acción. 
    /// Si se pasa un asterisco, se invoca el método <see cref="Select()"/> 
    /// para seleccionar todas las columnas. De lo contrario, se procesan las columnas 
    /// especificadas y se añaden a la consulta mediante el método <see cref="AppendColumns(string)"/>.
    /// </remarks>
    public void Select(string columns)
    {
        if (columns.IsEmpty())
            return;
        if (columns == "*")
        {
            Select();
            return;
        }
        AppendColumns(ColumnCache.GetSafeColumns(columns));
    }

    /// <summary>
    /// Agrega columnas a la cadena de resultados, separándolas con comas.
    /// </summary>
    /// <param name="columns">Las columnas que se agregarán a la cadena de resultados.</param>
    /// <remarks>
    /// Si ya hay contenido en la cadena de resultados, se eliminará la última coma antes de agregar las nuevas columnas.
    /// </remarks>
    protected void AppendColumns(string columns)
    {
        if (Result.Length > 0)
            Result.RemoveEnd(",").Append(",");
        Result.Append(columns);
    }

    /// <inheritdoc />
    /// <summary>
    /// Selecciona un elemento y lo agrega al resultado del constructor SQL.
    /// </summary>
    /// <param name="builder">El constructor SQL que se utilizará para crear la parte de la consulta.</param>
    /// <param name="alias">El alias que se asignará al elemento seleccionado.</param>
    /// <remarks>
    /// Este método verifica si el constructor SQL proporcionado es nulo antes de proceder. 
    /// Si ya hay elementos en el resultado, se agrega una coma para separar los elementos.
    /// Luego, se crea un nuevo elemento de construcción SQL y se agrega al resultado.
    /// </remarks>
    public void Select(ISqlBuilder builder, string alias)
    {
        if (builder == null)
            return;
        if (Result.Length > 0)
            Result.Append(",");
        var item = new SqlBuilderItem(Dialect, builder, alias);
        item.AppendTo(Result);
    }

    /// <inheritdoc />
    /// <summary>
    /// Selecciona una consulta SQL utilizando un generador de SQL.
    /// </summary>
    /// <param name="action">Una acción que toma un <see cref="ISqlBuilder"/> para construir la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará en la consulta SQL.</param>
    /// <remarks>
    /// Este método permite construir una consulta SQL de manera fluida utilizando un generador de SQL.
    /// Si la acción proporcionada es nula, el método no realizará ninguna operación.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void Select(Action<ISqlBuilder> action, string alias)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        Select(builder, alias);
    }

    #endregion

    #region AppendSql(Agregar a la cláusula Select)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cadena SQL al resultado, procesándola según el parámetro especificado.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar al resultado.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como cruda (true) o procesada (false).</param>
    /// <remarks>
    /// Si la cadena SQL está vacía o contiene solo espacios en blanco, no se realizará ninguna acción.
    /// Si el parámetro <paramref name="raw"/> es verdadero, se agrega la cadena SQL tal cual.
    /// Si es falso, se aplica un procesamiento a la cadena SQL antes de agregarla.
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

    /// <inheritdoc />
    /// <summary>
    /// Agrega una representación SQL al constructor de consultas especificado.
    /// </summary>
    /// <param name="builder">El constructor de consultas al que se le agregará la representación SQL. No debe ser nulo.</param>
    /// <remarks>
    /// Este método verifica si el parámetro <paramref name="builder"/> es nulo antes de intentar agregar la representación SQL.
    /// Si es nulo, no se realiza ninguna acción.
    /// </remarks>
    public void AppendSql(ISqlBuilder builder)
    {
        if (builder == null)
            return;
        builder.AppendTo(Result);
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega una instrucción SQL utilizando un constructor de SQL.
    /// </summary>
    /// <param name="action">Una acción que recibe un <see cref="ISqlBuilder"/> para construir la instrucción SQL.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="action"/> es nulo, no se realiza ninguna acción.
    /// Se crea un nuevo <see cref="ISqlBuilder"/> y se pasa a la acción proporcionada.
    /// Después de ejecutar la acción, se llama a <see cref="AppendSql(ISqlBuilder)"/> 
    /// para agregar la instrucción SQL construida.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="AppendSql(ISqlBuilder)"/>
    public void AppendSql(Action<ISqlBuilder> action)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        AppendSql(builder);
    }

    #endregion

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado tiene longitud mayor que cero.
    /// </summary>
    /// <returns>
    /// Devuelve true si el resultado contiene elementos; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay resultados disponibles 
    /// antes de proceder con otras operaciones que dependen de la existencia de resultados.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una representación de la consulta al <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se agregará la consulta.</param>
    /// <remarks>
    /// Este método valida la consulta antes de agregarla al <paramref name="builder"/>. 
    /// Si la validación falla, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="Validate"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("Select ");
        builder.Append(Result);
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia el contenido del resultado.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de resultados.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
    }

    #endregion

    #region Clone(Copiar la cláusula Select)

    /// <inheritdoc />
    /// <summary>
    /// Crea una copia de la cláusula de selección actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>Una nueva instancia de <see cref="ISelectClause"/> que es una copia de la cláusula de selección actual.</returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StringBuilder"/> para construir el resultado de la cláusula de selección
    /// y devuelve una nueva instancia de <see cref="SelectClause"/> con los datos copiados.
    /// </remarks>
    /// <seealso cref="ISelectClause"/>
    /// <seealso cref="SelectClause"/>
    public virtual ISelectClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new SelectClause(builder, result);
    }

    #endregion
}