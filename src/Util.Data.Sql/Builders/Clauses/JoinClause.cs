using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Conditions;
using Util.Data.Sql.Builders.Core;
using Util.Helpers;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula de unión en una consulta.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para definir cómo se deben unir dos o más conjuntos de datos 
/// en una consulta, especificando las condiciones de unión y el tipo de unión.
/// </remarks>
public class JoinClause : ClauseBase, IJoinClause
{

    #region constante

    private const string JoinKey = "Join";
    private const string LeftJoinKey = "Left Join";
    private const string RightJoinKey = "Right Join";

    #endregion

    #region Campo

    protected readonly StringBuilder Result;
    protected StringBuilder Condition;
    protected readonly IColumnCache ColumnCache;
    protected readonly IConditionFactory ConditionFactory;

    #endregion

    #region Método constructor

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JoinClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.</param>
    /// <param name="result">Un objeto <see cref="StringBuilder"/> que contendrá el resultado de la cláusula de unión. Si es nulo, se inicializa uno nuevo.</param>
    /// <param name="condition">Un objeto <see cref="StringBuilder"/> que contendrá las condiciones de la cláusula de unión. Si es nulo, se inicializa uno nuevo.</param>
    public JoinClause(SqlBuilderBase sqlBuilder, StringBuilder result = null, StringBuilder condition = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        Condition = condition ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
        ConditionFactory = sqlBuilder.ConditionFactory;
    }

    #endregion

    #region Join(Inner Join)

    /// <inheritdoc />
    /// <summary>
    /// Une la tabla especificada utilizando la clave de unión predeterminada.
    /// </summary>
    /// <param name="table">El nombre de la tabla a la que se desea unir.</param>
    /// <remarks>
    /// Este método llama a otra sobrecarga de <see cref="Join(string, string)"/> 
    /// utilizando una clave de unión predeterminada.
    /// </remarks>
    /// <seealso cref="Join(string, string)"/>
    public void Join(string table)
    {
        Join(JoinKey, table);
    }

    /// <summary>
    /// Une la tabla especificada a la consulta actual utilizando el tipo de unión indicado.
    /// </summary>
    /// <param name="joinType">El tipo de unión a utilizar (por ejemplo, INNER, LEFT, RIGHT).</param>
    /// <param name="table">El nombre de la tabla que se va a unir.</param>
    /// <remarks>
    /// Este método se encarga de preparar la consulta para incluir una unión con la tabla especificada.
    /// Asegúrese de que la tabla exista en la base de datos antes de llamar a este método.
    /// </remarks>
    private void Join(string joinType, string table)
    {
        AppendOn();
        AppendJoin(joinType, table);
    }

    /// <summary>
    /// Agrega la condición actual al resultado con el prefijo " On ".
    /// </summary>
    /// <remarks>
    /// Este método verifica si la longitud de la condición es mayor que cero antes de 
    /// agregarla al resultado. Si la condición está vacía, el método no realiza ninguna acción.
    /// Después de agregar la condición, se limpia para permitir una nueva entrada.
    /// </remarks>
    private void AppendOn()
    {
        if (Condition.Length == 0)
            return;
        Result.Append(" On ");
        Result.Append(Condition);
        Result.AppendLine(" ");
        Condition.Clear();
    }

    /// <summary>
    /// Agrega una cláusula de unión a la consulta actual.
    /// </summary>
    /// <param name="joinType">El tipo de unión que se va a utilizar (por ejemplo, INNER JOIN, LEFT JOIN).</param>
    /// <param name="table">El nombre de la tabla que se va a unir.</param>
    private void AppendJoin(string joinType, string table)
    {
        Result.AppendFormat("{0} ", joinType);
        var item = CreateTableItem(table);
        item.AppendTo(Result);
    }

    /// <summary>
    /// Crea un nuevo elemento de tabla con el dialecto especificado y el nombre de la tabla proporcionado.
    /// </summary>
    /// <param name="table">El nombre de la tabla para el cual se creará el elemento de tabla.</param>
    /// <returns>Un nuevo objeto <see cref="TableItem"/> que representa la tabla especificada.</returns>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban para proporcionar
    /// una implementación personalizada si es necesario.
    /// </remarks>
    protected virtual TableItem CreateTableItem(string table)
    {
        return new(Dialect, table);
    }

    /// <inheritdoc />
    /// <summary>
    /// Une la consulta actual con otra utilizando una clave de unión especificada.
    /// </summary>
    /// <param name="builder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida.</param>
    /// <remarks>
    /// Este método invoca una sobrecarga del método Join que utiliza una clave de unión predefinida.
    /// </remarks>
    /// <seealso cref="Join(ISqlBuilder, string)"/>
    public void Join(ISqlBuilder builder, string alias)
    {
        Join(JoinKey, builder, alias);
    }

    /// <summary>
    /// Realiza una operación de unión en una consulta SQL.
    /// </summary>
    /// <param name="joinType">El tipo de unión que se va a realizar (por ejemplo, INNER JOIN, LEFT JOIN, etc.).</param>
    /// <param name="builder">Una instancia de <see cref="ISqlBuilder"/> que representa la consulta SQL que se unirá.</param>
    /// <param name="alias">Un alias que se utilizará para la tabla unida.</param>
    /// <remarks>
    /// Este método verifica si el <paramref name="builder"/> es nulo antes de proceder con la operación de unión.
    /// Si el <paramref name="builder"/> es nulo, el método no realiza ninguna acción.
    /// </remarks>
    private void Join(string joinType, ISqlBuilder builder, string alias)
    {
        if (builder == null)
            return;
        AppendOn();
        Result.AppendFormat("{0} ", joinType);
        var item = new SqlBuilderItem(Dialect, builder, alias);
        item.AppendTo(Result);
    }

    /// <inheritdoc />
    /// <summary>
    /// Une una acción de construcción SQL a la consulta actual utilizando un alias especificado.
    /// </summary>
    /// <param name="action">La acción que se ejecutará para construir la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará para la unión en la consulta SQL.</param>
    /// <remarks>
    /// Si la acción proporcionada es nula, el método no realizará ninguna operación.
    /// Se crea un nuevo constructor SQL y se pasa a la acción para que se configure.
    /// Luego, se llama a otro método Join para realizar la unión con el constructor y el alias proporcionado.
    /// </remarks>
    public void Join(Action<ISqlBuilder> action, string alias)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        Join(builder, alias);
    }

    #endregion

    #region LeftJoin(Left join (unión externa izquierda))

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión izquierda con la tabla especificada.
    /// </summary>
    /// <param name="table">El nombre de la tabla con la que se realizará la unión izquierda.</param>
    /// <remarks>
    /// La unión izquierda devuelve todos los registros de la tabla de la izquierda y los registros coincidentes de la tabla de la derecha. 
    /// Si no hay coincidencias, se devolverán valores nulos para las columnas de la tabla de la derecha.
    /// </remarks>
    /// <seealso cref="Join(string, string)"/>
    public void LeftJoin(string table)
    {
        Join(LeftJoinKey, table);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión izquierda en la consulta SQL.
    /// </summary>
    /// <param name="builder">El objeto que construye la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará para la tabla unida.</param>
    /// <remarks>
    /// Esta función utiliza el método <see cref="Join"/> para llevar a cabo la unión izquierda
    /// utilizando una clave predefinida llamada <c>LeftJoinKey</c>.
    /// </remarks>
    public void LeftJoin(ISqlBuilder builder, string alias)
    {
        Join(LeftJoinKey, builder, alias);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión izquierda en la consulta SQL utilizando el generador de SQL proporcionado.
    /// </summary>
    /// <param name="action">Una acción que toma un <see cref="ISqlBuilder"/> para construir la parte de la consulta SQL que se unirá.</param>
    /// <param name="alias">El alias que se utilizará para la tabla unida.</param>
    /// <remarks>
    /// Si la acción proporcionada es nula, el método no realizará ninguna operación.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void LeftJoin(Action<ISqlBuilder> action, string alias)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        LeftJoin(builder, alias);
    }

    #endregion

    #region RightJoin(Right outer join)

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión derecha con la tabla especificada.
    /// </summary>
    /// <param name="table">El nombre de la tabla con la que se realizará la unión derecha.</param>
    /// <remarks>
    /// La unión derecha devuelve todos los registros de la tabla de la derecha y los registros coincidentes de la tabla de la izquierda. 
    /// Si no hay coincidencias, se devolverán valores nulos para las columnas de la tabla de la izquierda.
    /// </remarks>
    /// <seealso cref="Join(string, string)"/>
    public void RightJoin(string table)
    {
        Join(RightJoinKey, table);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión derecha en la consulta SQL.
    /// </summary>
    /// <param name="builder">El constructor de SQL que se utilizará para generar la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla en la unión.</param>
    /// <remarks>
    /// Esta función invoca el método <see cref="Join"/> utilizando la clave de unión derecha.
    /// </remarks>
    public void RightJoin(ISqlBuilder builder, string alias)
    {
        Join(RightJoinKey, builder, alias);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una unión derecha en la consulta SQL utilizando el generador de SQL proporcionado.
    /// </summary>
    /// <param name="action">Una acción que toma un <see cref="ISqlBuilder"/> para construir la parte de la consulta SQL.</param>
    /// <param name="alias">El alias que se utilizará para la tabla en la unión.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="action"/> es nulo, la operación se cancela y no se realiza ninguna unión.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void RightJoin(Action<ISqlBuilder> action, string alias)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        RightJoin(builder, alias);
    }

    #endregion

    #region On(Establecer condiciones de conexión.)

    /// <inheritdoc />
    /// <summary>
    /// Procesa una condición basada en la columna y el valor proporcionados.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplica la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="@operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta.</param>
    /// <remarks>
    /// Este método utiliza un caché de columnas para asegurar que el nombre de la columna sea seguro para su uso en la consulta.
    /// Además, se encarga de obtener el valor adecuado, considerando si se requiere parametrización.
    /// Finalmente, crea una condición utilizando la fábrica de condiciones y la aplica.
    /// </remarks>
    /// <seealso cref="ConditionFactory"/>
    public void On(string column, object value, Operator @operator = Operator.Equal, bool isParameterization = false)
    {
        column = ColumnCache.GetSafeColumn(column);
        value = GetValue(value, isParameterization);
        var condition = ConditionFactory.Create(column, value, @operator, isParameterization);
        On(condition);
    }

    /// <summary>
    /// Obtiene un valor basado en el tipo de entrada y si se está utilizando parametrización.
    /// </summary>
    /// <param name="value">El valor de entrada que se va a procesar.</param>
    /// <param name="isParameterization">Indica si se está utilizando parametrización.</param>
    /// <returns>
    /// Devuelve el valor procesado. Si el valor de entrada es null, se devuelve null.
    /// Si se está utilizando parametrización, se devuelve el valor tal cual. 
    /// Si el valor no es de tipo string, se devuelve el valor original. 
    /// Si el valor es de tipo string, se devuelve un valor seguro obtenido de la caché de columnas.
    /// </returns>
    private object GetValue(object value, bool isParameterization)
    {
        if (value == null)
            return null;
        if (isParameterization)
            return value;
        if (value.GetType() != typeof(string))
            return value;
        return ColumnCache.GetSafeColumn(value.ToString());
    }

    /// <summary>
    /// Procesa una condición SQL y la agrega a la condición existente.
    /// </summary>
    /// <param name="condition">La condición SQL que se va a procesar. Si es <c>null</c>, no se realiza ninguna acción.</param>
    public void On(ISqlCondition condition)
    {
        if (condition == null)
            return;
        new AndCondition(condition).AppendTo(Condition);
    }

    #endregion

    #region AppendJoin(Agregar a la cláusula de unión interna.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cláusula SQL de unión a la consulta actual.
    /// </summary>
    /// <param name="sql">La cadena SQL que representa la cláusula de unión a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar.</param>
    /// <remarks>
    /// Este método utiliza la clave de unión definida por <c>JoinKey</c> para agregar la cláusula SQL.
    /// </remarks>
    /// <seealso cref="AppendSql(string, bool)"/>
    public void AppendJoin(string sql, bool raw)
    {
        AppendSql(JoinKey, sql, raw);
    }

    /// <summary>
    /// Agrega una cláusula SQL a la consulta actual, utilizando el tipo de unión especificado.
    /// </summary>
    /// <param name="joinType">El tipo de unión que se va a utilizar (por ejemplo, INNER JOIN, LEFT JOIN).</param>
    /// <param name="sql">La cadena SQL que se va a agregar a la consulta.</param>
    /// <param name="raw">Indica si la cadena SQL proporcionada debe ser tratada como cruda (sin modificaciones).</param>
    /// <remarks>
    /// Este método verifica si la cadena SQL es nula o está vacía antes de proceder a agregarla.
    /// Si el parámetro <paramref name="raw"/> es verdadero, se agrega la cadena SQL tal cual.
    /// De lo contrario, se aplica un reemplazo a la cadena SQL mediante el método <see cref="ReplaceRawSql(string)"/>.
    /// </remarks>
    private void AppendSql(string joinType, string sql, bool raw)
    {
        if (string.IsNullOrWhiteSpace(sql))
            return;
        AppendOn();
        Result.AppendFormat("{0} ", joinType);
        if (raw)
        {
            Result.Append(sql);
            return;
        }
        Result.Append(ReplaceRawSql(sql));
    }

    #endregion

    #region AppendLeftJoin(Agregar a la cláusula de unión externa izquierda.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cláusula LEFT JOIN a la consulta SQL actual.
    /// </summary>
    /// <param name="sql">La cadena SQL que representa la cláusula LEFT JOIN.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar.</param>
    /// <inheritdoc />
    public void AppendLeftJoin(string sql, bool raw)
    {
        AppendSql(LeftJoinKey, sql, raw);
    }

    #endregion

    #region AppendRightJoin(Agregar a la cláusula de unión externa derecha.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una cláusula de unión derecha a la consulta SQL.
    /// </summary>
    /// <param name="sql">La cadena SQL que representa la cláusula de unión derecha.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como cruda (true) o procesada (false).</param>
    /// <inheritdoc />
    public void AppendRightJoin(string sql, bool raw)
    {
        AppendSql(RightJoinKey, sql, raw);
    }

    #endregion

    #region AppendOn(Agregar a la cláusula On)

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición SQL a la consulta actual.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cadena SQL es cruda (true) o si debe ser procesada (false).</param>
    /// <remarks>
    /// Si el parámetro <paramref name="raw"/> es verdadero, se agrega la condición SQL tal cual. 
    /// Si es falso, se aplica un procesamiento a la cadena SQL antes de agregarla.
    /// </remarks>
    /// <seealso cref="SqlCondition"/>
    public void AppendOn(string sql, bool raw)
    {
        if (raw)
        {
            On(new SqlCondition(sql));
            return;
        }
        On(new SqlCondition(ReplaceRawSql(sql)));
    }

    #endregion

    #region Validate(Verificación)

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado tiene contenido.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el resultado tiene una longitud mayor a cero; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay datos disponibles en la propiedad <c>Result</c>.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo(Agregar al generador de cadenas.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega el resultado a un objeto <see cref="StringBuilder"/> después de realizar la validación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregará el resultado.</param>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="builder"/> es nulo y, si la validación es exitosa,
    /// llama al método <see cref="AppendOn"/> antes de agregar el resultado al <paramref name="builder"/>.
    /// Al final, se elimina el último delimitador de línea.
    /// </remarks>
    /// <seealso cref="Validate"/>
    /// <seealso cref="AppendOn"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        AppendOn();
        builder.Append(Result);
        builder.RemoveEnd($" {Common.Line}");
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

    #region Clone(Copiar la cláusula Join.)

    /// <inheritdoc />
    /// <summary>
    /// Clona la cláusula de unión actual, creando una nueva instancia de <see cref="JoinClause"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la nueva cláusula.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IJoinClause"/> que representa la cláusula de unión clonada.
    /// </returns>
    /// <remarks>
    /// Este método crea una copia de la cláusula de unión, incluyendo el resultado y la condición.
    /// </remarks>
    /// <seealso cref="JoinClause"/>
    public virtual IJoinClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        var condition = new StringBuilder();
        condition.Append(Condition);
        return new JoinClause(builder, result, condition);
    }

    #endregion
}