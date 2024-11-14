using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Conditions;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de agrupamiento en una consulta.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para definir cómo se deben agrupar los resultados de una consulta
/// en función de uno o más campos especificados.
/// </remarks>
public class GroupByClause : ClauseBase, IGroupByClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected StringBuilder Condition;
    protected readonly IColumnCache ColumnCache;
    protected readonly IConditionFactory ConditionFactory;

    #endregion

    #region Método constructor

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GroupByClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utiliza para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> que contiene el resultado de la cláusula. Si es <c>null</c>, se inicializa uno nuevo.</param>
    /// <param name="condition">Un objeto <see cref="StringBuilder"/> que contiene la condición de la cláusula. Si es <c>null</c>, se inicializa uno nuevo.</param>
    /// <remarks>
    /// Esta clase se utiliza para representar una cláusula GROUP BY en una consulta SQL.
    /// </remarks>
    public GroupByClause(SqlBuilderBase sqlBuilder, StringBuilder result = null, StringBuilder condition = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        Condition = condition ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
        ConditionFactory = sqlBuilder.ConditionFactory;
    }

    #endregion

    #region GroupBy(Agregar columna de grupo.)

    /// <inheritdoc />
    /// <summary>
    /// Agrupa los resultados basados en las columnas especificadas.
    /// </summary>
    /// <param name="columns">Una cadena que representa las columnas por las cuales se desea agrupar. 
    /// Si esta cadena está vacía o es nula, no se realizará ninguna acción.</param>
    /// <remarks>
    /// Este método modifica el objeto <c>Result</c> añadiendo las columnas agrupadas. 
    /// Si ya hay resultados previos, se añadirá una coma antes de las nuevas columnas.
    /// </remarks>
    /// <seealso cref="ColumnCache.GetSafeColumns(string)"/>
    public void GroupBy(string columns)
    {
        if (string.IsNullOrWhiteSpace(columns))
            return;
        if (Result.Length > 0)
            Result.Append(",");
        Result.Append(ColumnCache.GetSafeColumns(columns));
    }

    #endregion

    #region Having(Agregar condiciones de agrupamiento.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición a la cláusula "HAVING" de una consulta.
    /// </summary>
    /// <param name="expression">La expresión que se evaluará en la condición.</param>
    /// <param name="value">El valor que se comparará con la expresión.</param>
    /// <param name="@operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta. Por defecto es <c>true</c>.</param>
    /// <remarks>
    /// Este método permite construir condiciones complejas en la cláusula "HAVING" de una consulta SQL,
    /// facilitando la creación de filtros sobre los resultados agregados.
    /// </remarks>
    /// <seealso cref="ConditionFactory"/>
    public void Having(string expression, object value, Operator @operator = Operator.Equal, bool isParameterization = true)
    {
        And(ConditionFactory.Create(expression, value, @operator, isParameterization));
    }

    /// <summary>
    /// Agrega una condición lógica "AND" a la condición existente.
    /// </summary>
    /// <param name="condition">La condición SQL que se desea combinar con la condición actual mediante "AND".</param>
    /// <remarks>
    /// Este método crea una nueva instancia de <see cref="AndCondition"/> con la condición proporcionada
    /// y la añade a la colección de condiciones existentes.
    /// </remarks>
    private void And(ISqlCondition condition)
    {
        new AndCondition(condition).AppendTo(Condition);
    }

    #endregion

    #region AppendGroupBy(Agregar a la cláusula Group By.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cláusula GROUP BY a la consulta SQL actual.
    /// </summary>
    /// <param name="sql">La cláusula GROUP BY en formato SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL proporcionada es cruda (raw) o si debe ser procesada.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="sql"/> es nulo o está vacío, no se realizará ninguna acción.
    /// Si <paramref name="raw"/> es verdadero, se agrega la cláusula SQL tal cual se proporciona.
    /// Si <paramref name="raw"/> es falso, se procesará la cláusula SQL mediante el método <see cref="ReplaceRawSql"/> antes de agregarla.
    /// </remarks>
    public void AppendGroupBy(string sql, bool raw)
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

    #region AppendHaving(Agregar a la cláusula HAVING)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cláusula HAVING a la consulta SQL actual.
    /// </summary>
    /// <param name="sql">La cadena SQL que representa la condición a agregar.</param>
    /// <param name="raw">Indica si la condición SQL proporcionada es cruda (raw) o debe ser procesada.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="sql"/> es nulo o está vacío, no se realizará ninguna acción.
    /// Si <paramref name="raw"/> es verdadero, se agrega la condición tal cual se proporciona.
    /// De lo contrario, se procesa la cadena SQL antes de agregarla.
    /// </remarks>
    public void AppendHaving(string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        if (raw)
        {
            And(new SqlCondition(sql));
            return;
        }
        And(new SqlCondition(ReplaceRawSql(sql)));
    }

    #endregion

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado contiene elementos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el resultado tiene una longitud mayor que cero; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay datos disponibles en el resultado.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una representación de la cláusula "Group By" y, opcionalmente, una cláusula "Having" 
    /// a un objeto <see cref="StringBuilder"/> dado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la representación.</param>
    /// <remarks>
    /// Este método verifica primero si el objeto <paramref name="builder"/> es nulo 
    /// y si la validación de la cláusula es exitosa. Si la validación falla, no se realiza 
    /// ninguna acción. Si la cláusula "Group By" es válida, se agrega al <paramref name="builder"/>.
    /// Si hay una condición especificada, se agrega la cláusula "Having" seguida de la condición.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es nulo.</exception>
    /// <seealso cref="Validate"/>
    /// <seealso cref="StringBuilder"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("Group By ");
        builder.Append(Result);
        if (Condition.Length == 0)
            return;
        builder.Append(" Having ");
        builder.Append(Condition);
    }

    #endregion

    #region Clear(Limpiar)

    /// <inheritdoc />
    /// <summary>
    /// Limpia los resultados y las condiciones almacenadas.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de las colecciones de resultados y condiciones.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
        Condition.Clear();
    }

    #endregion

    #region Clone(Copiar la cláusula Group By.)

    /// <inheritdoc />
    /// <summary>
    /// Clona la cláusula de agrupamiento actual.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IGroupByClause"/> que representa la cláusula de agrupamiento clonada.
    /// </returns>
    /// <remarks>
    /// Este método crea una copia de la cláusula de agrupamiento actual, incluyendo su resultado y condición.
    /// </remarks>
    public virtual IGroupByClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        var condition = new StringBuilder();
        condition.Append(Condition);
        return new GroupByClause(builder, result, condition);
    }

    #endregion
}