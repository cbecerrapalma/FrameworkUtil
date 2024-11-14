using Util.Data.Queries;
using Util.Data.Sql.Builders.Caches;
using Util.Data.Sql.Builders.Conditions;

namespace Util.Data.Sql.Builders.Clauses;

/// <summary>
/// Representa una cláusula WHERE en una consulta.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para construir condiciones que filtran los resultados de una consulta 
/// en función de criterios específicos. Hereda de <see cref="ClauseBase"/> y 
/// implementa la interfaz <see cref="IWhereClause"/>.
/// </remarks>
public class WhereClause : ClauseBase, IWhereClause
{

    #region Campo

    protected readonly StringBuilder Result;
    protected readonly IColumnCache ColumnCache;
    protected readonly IConditionFactory ConditionFactory;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="WhereClause"/>.
    /// </summary>
    /// <param name="sqlBuilder">
    /// El objeto <see cref="SqlBuilderBase"/> que se utilizará para construir la consulta SQL.
    /// </param>
    /// <param name="result">
    /// Un objeto <see cref="StringBuilder"/> opcional que contendrá el resultado de la cláusula WHERE. 
    /// Si se proporciona un valor nulo, se inicializará un nuevo <see cref="StringBuilder"/>.
    /// </param>
    public WhereClause(SqlBuilderBase sqlBuilder, StringBuilder result = null) : base(sqlBuilder)
    {
        Result = result ?? new StringBuilder();
        ColumnCache = sqlBuilder.ColumnCache;
        ConditionFactory = sqlBuilder.ConditionFactory;
    }

    #endregion

    #region And

    /// <inheritdoc />
    /// <summary>
    /// Combina la condición actual con otra condición utilizando una operación lógica "Y".
    /// </summary>
    /// <param name="condition">La condición que se va a combinar con la condición actual.</param>
    /// <remarks>
    /// Este método crea una nueva instancia de <see cref="AndCondition"/> con la condición proporcionada
    /// y la agrega al resultado actual.
    /// </remarks>
    /// <seealso cref="AndCondition"/>
    public void And(ISqlCondition condition)
    {
        new AndCondition(condition).AppendTo(Result);
    }

    #endregion

    #region Or

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición lógica "OR" a la consulta SQL.
    /// </summary>
    /// <param name="condition">La condición que se desea agregar con la operación "OR".</param>
    /// <remarks>
    /// Este método crea una nueva instancia de <see cref="OrCondition"/> utilizando la condición proporcionada
    /// y la añade al resultado de la consulta SQL.
    /// </remarks>
    /// <seealso cref="OrCondition"/>
    public void Or(ISqlCondition condition)
    {
        new OrCondition(condition).AppendTo(Result);
    }

    #endregion

    #region Where

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición a la cláusula WHERE utilizando un operador específico.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="operator">El operador que se utilizará para la comparación.</param>
    /// <remarks>
    /// Este método asegura que el nombre de la columna sea seguro antes de crear la condición.
    /// Se utiliza el método <see cref="ConditionFactory.Create"/> para generar la condición adecuada.
    /// </remarks>
    public void Where(string column, object value, Operator @operator)
    {
        column = ColumnCache.GetSafeColumn(column);
        And(ConditionFactory.Create(column, value, @operator));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición de filtro en una consulta SQL utilizando una columna específica y un constructor de SQL.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará el filtro.</param>
    /// <param name="builder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <param name="operator">El operador que se utilizará para la comparación en la condición.</param>
    /// <remarks>
    /// Si el constructor de SQL proporcionado es nulo, no se realizará ninguna acción.
    /// </remarks>
    public void Where(string column, ISqlBuilder builder, Operator @operator)
    {
        if (builder == null)
            return;
        Where(column, (object)builder, @operator);
    }

    /// <inheritdoc />
    /// <summary>
    /// Aplica una condición a una columna específica utilizando un constructor de SQL.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="action">Una acción que permite construir la consulta SQL.</param>
    /// <param name="operator">El operador que se utilizará en la condición.</param>
    /// <remarks>
    /// Este método crea un nuevo constructor de SQL y ejecuta la acción proporcionada 
    /// para construir la parte de la consulta que se aplicará a la columna especificada.
    /// Si la acción es nula, el método no realiza ninguna operación.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void Where(string column, Action<ISqlBuilder> action, Operator @operator)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        Where(column, builder, @operator);
    }

    #endregion

    #region IsNull

    /// <inheritdoc />
    /// <summary>
    /// Verifica si el valor de la columna especificada es nulo.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método utiliza el operador de igualdad para comparar el valor de la columna con <c>null</c>.
    /// </remarks>
    /// <seealso cref="Where(string, object, Operator)"/>
    public void IsNull(string column)
    {
        Where(column, (object)null, Operator.Equal);
    }

    #endregion

    #region IsNotNull

    /// <inheritdoc />
    /// <summary>
    /// Verifica que el valor de la columna especificada no sea nulo.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a comprobar.</param>
    /// <remarks>
    /// Este método utiliza el operador de desigualdad para asegurarse de que el valor de la columna no sea nulo.
    /// </remarks>
    /// <seealso cref="Where(string, object, Operator)"/>
    public void IsNotNull(string column)
    {
        Where(column, (object)null, Operator.NotEqual);
    }

    #endregion

    #region IsEmpty

    /// <inheritdoc />
    /// <summary>
    /// Verifica si la columna especificada está vacía o es nula.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método utiliza el caché de columnas para obtener una representación segura de la columna,
    /// y luego crea condiciones para comprobar si la columna es nula o está vacía (cadena vacía).
    /// Ambas condiciones se combinan usando una operación lógica OR, y el resultado se combina con
    /// otras condiciones existentes mediante una operación lógica AND.
    /// </remarks>
    /// <seealso cref="ColumnCache"/>
    /// <seealso cref="ConditionFactory"/>
    /// <seealso cref="Operator"/>
    public void IsEmpty(string column)
    {
        column = ColumnCache.GetSafeColumn(column);
        var nullCondition = ConditionFactory.Create(column, null, Operator.Equal);
        var emptyCondition = ConditionFactory.Create(column, "''", Operator.Equal, false);
        And(new OrCondition(nullCondition, emptyCondition));
    }

    #endregion

    #region IsNotEmpty

    /// <inheritdoc />
    /// <summary>
    /// Verifica que la columna especificada no esté vacía ni sea nula.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a verificar.</param>
    /// <remarks>
    /// Este método utiliza la caché de columnas para obtener una representación segura de la columna
    /// y crea condiciones para comprobar que la columna no sea nula y no esté vacía.
    /// Las condiciones se combinan utilizando una operación AND.
    /// </remarks>
    /// <seealso cref="ColumnCache"/>
    /// <seealso cref="ConditionFactory"/>
    public void IsNotEmpty(string column)
    {
        column = ColumnCache.GetSafeColumn(column);
        var notNullCondition = ConditionFactory.Create(column, null, Operator.NotEqual);
        var notEmptyCondition = ConditionFactory.Create(column, "''", Operator.NotEqual, false);
        And(new AndCondition(notNullCondition, notEmptyCondition));
    }

    #endregion

    #region Between

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición que verifica si un valor en una columna está entre un valor mínimo y un valor máximo.
    /// </summary>
    /// <param name="column">El nombre de la columna en la que se aplicará la condición.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango.</param>
    /// <remarks>
    /// Si el valor mínimo es mayor que el valor máximo, se invierte el rango y se crea una condición con los valores invertidos.
    /// </remarks>
    /// <seealso cref="ConditionFactory"/>
    public void Between(string column, int? min, int? max, Boundary boundary)
    {
        column = ColumnCache.GetSafeColumn(column);
        if (min > max)
        {
            And(ConditionFactory.Create(column, max, min, boundary));
            return;
        }
        And(ConditionFactory.Create(column, min, max, boundary));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición que verifica si un valor de columna se encuentra entre un rango definido por un mínimo y un máximo.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="min">El valor mínimo del rango. Puede ser nulo.</param>
    /// <param name="max">El valor máximo del rango. Puede ser nulo.</param>
    /// <param name="boundary">Especifica el tipo de límite a aplicar en la condición.</param>
    /// <remarks>
    /// Si el valor mínimo es mayor que el valor máximo, se invierte el rango y se crea una condición con el máximo como mínimo y el mínimo como máximo.
    /// </remarks>
    /// <seealso cref="ConditionFactory"/>
    public void Between(string column, double? min, double? max, Boundary boundary)
    {
        column = ColumnCache.GetSafeColumn(column);
        if (min > max)
        {
            And(ConditionFactory.Create(column, max, min, boundary));
            return;
        }
        And(ConditionFactory.Create(column, min, max, boundary));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición que verifica si el valor de una columna se encuentra entre un valor mínimo y un valor máximo.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="min">El valor mínimo permitido. Puede ser nulo.</param>
    /// <param name="max">El valor máximo permitido. Puede ser nulo.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites (inclusivo/exclusivo).</param>
    /// <remarks>
    /// Si el valor mínimo es mayor que el valor máximo, se invierte la condición.
    /// </remarks>
    public void Between(string column, decimal? min, decimal? max, Boundary boundary)
    {
        column = ColumnCache.GetSafeColumn(column);
        if (min > max)
        {
            And(ConditionFactory.Create(column, max, min, boundary));
            return;
        }
        And(ConditionFactory.Create(column, min, max, boundary));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una condición que verifica si un valor de columna se encuentra dentro de un rango de fechas.
    /// </summary>
    /// <param name="column">El nombre de la columna que se va a evaluar.</param>
    /// <param name="min">La fecha mínima del rango. Puede ser nula.</param>
    /// <param name="max">La fecha máxima del rango. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la parte de tiempo en la comparación.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango. Puede ser nulo.</param>
    /// <remarks>
    /// Este método utiliza un caché de columnas para asegurar que el nombre de la columna sea seguro para su uso en la consulta.
    /// Se generan condiciones que se añaden a la cláusula AND de la consulta actual.
    /// </remarks>
    /// <seealso cref="ColumnCache"/>
    /// <seealso cref="ConditionFactory"/>
    public void Between(string column, DateTime? min, DateTime? max, bool includeTime, Boundary? boundary)
    {
        column = ColumnCache.GetSafeColumn(column);
        And(ConditionFactory.Create(column, GetMin(min, max, includeTime), GetMax(min, max, includeTime), GetBoundary(boundary, includeTime)));
    }

    /// <summary>
    /// Obtiene el valor mínimo entre dos fechas, considerando la opción de incluir la hora.
    /// </summary>
    /// <param name="min">La fecha mínima a considerar. Puede ser nula.</param>
    /// <param name="max">La fecha máxima a considerar. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la hora en el resultado.</param>
    /// <returns>
    /// Devuelve la fecha mínima entre <paramref name="min"/> y <paramref name="max"/>. 
    /// Si <paramref name="min"/> es nula, se devuelve nulo. 
    /// Si <paramref name="includeTime"/> es falso, se devuelve solo la parte de la fecha sin la hora.
    /// </returns>
    /// <remarks>
    /// Si <paramref name="min"/> es mayor que <paramref name="max"/>, se devuelve <paramref name="max"/> como resultado.
    /// </remarks>
    private DateTime? GetMin(DateTime? min, DateTime? max, bool includeTime)
    {
        if (min == null)
            return null;
        DateTime? result = min;
        if (min > max)
            result = max;
        if (includeTime)
            return result;
        return result.SafeValue().Date;
    }

    /// <summary>
    /// Obtiene el valor máximo entre dos fechas, con la opción de incluir la hora.
    /// </summary>
    /// <param name="min">La fecha mínima a comparar. Puede ser nula.</param>
    /// <param name="max">La fecha máxima a comparar. Puede ser nula.</param>
    /// <param name="includeTime">Indica si se debe incluir la hora en el resultado.</param>
    /// <returns>
    /// Retorna la fecha máxima entre <paramref name="min"/> y <paramref name="max"/>. 
    /// Si <paramref name="max"/> es nula, se retorna nulo. Si <paramref name="includeTime"/> es falso, 
    /// se retorna la fecha sin la hora, ajustada al día siguiente.
    /// </returns>
    /// <remarks>
    /// Si <paramref name="min"/> es mayor que <paramref name="max"/>, se retorna <paramref name="min"/>.
    /// </remarks>
    private DateTime? GetMax(DateTime? min, DateTime? max, bool includeTime)
    {
        if (max == null)
            return null;
        DateTime? result = max;
        if (min > max)
            result = min;
        if (includeTime)
            return result;
        return result.SafeValue().Date.AddDays(1);
    }

    /// <summary>
    /// Obtiene el límite especificado, o un límite predeterminado si no se proporciona uno.
    /// </summary>
    /// <param name="boundary">El límite a evaluar. Puede ser nulo.</param>
    /// <param name="includeTime">Indica si se debe incluir el tiempo en la evaluación del límite.</param>
    /// <returns>
    /// Un objeto <see cref="Boundary"/> que representa el límite especificado o un límite predeterminado.
    /// </returns>
    /// <remarks>
    /// Si el parámetro <paramref name="boundary"/> no es nulo, se devuelve su valor seguro.
    /// Si el parámetro <paramref name="includeTime"/> es verdadero, se devuelve <see cref="Boundary.Both"/>.
    /// De lo contrario, se devuelve <see cref="Boundary.Left"/>.
    /// </remarks>
    private Boundary GetBoundary(Boundary? boundary, bool includeTime)
    {
        if (boundary != null)
            return boundary.SafeValue();
        if (includeTime)
            return Boundary.Both;
        return Boundary.Left;
    }

    #endregion

    #region Exists

    /// <inheritdoc />
    /// <summary>
    /// Verifica si existe una condición en la consulta SQL construida por el <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="ISqlBuilder"/> que se utiliza para construir la consulta SQL.</param>
    /// <remarks>
    /// Si el <paramref name="builder"/> es nulo, la función no realiza ninguna acción.
    /// De lo contrario, se añade una condición de existencia a la consulta.
    /// </remarks>
    public void Exists(ISqlBuilder builder)
    {
        if (builder == null)
            return;
        And(new ExistsCondition(builder));
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica la existencia de una condición en la base de datos utilizando un constructor de SQL.
    /// </summary>
    /// <param name="action">Acción que define la construcción de la consulta SQL.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente una consulta SQL mediante el uso de un 
    /// <see cref="ISqlBuilder"/>. Si la acción proporcionada es nula, el método no realiza ninguna operación.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void Exists(Action<ISqlBuilder> action)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        Exists(builder);
    }

    #endregion

    #region NotExists

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición de "No Existe" a la consulta SQL.
    /// </summary>
    /// <param name="builder">El constructor SQL que se utilizará para crear la condición.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="builder"/> es nulo, la función no realizará ninguna acción.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    public void NotExists(ISqlBuilder builder)
    {
        if (builder == null)
            return;
        And(new NotExistsCondition(builder));
    }

    /// <inheritdoc />
    /// <summary>
    /// Ejecuta una acción sobre un generador de SQL y verifica si no existen resultados.
    /// </summary>
    /// <param name="action">La acción que se ejecutará sobre el generador de SQL.</param>
    /// <remarks>
    /// Si la acción proporcionada es nula, el método no realiza ninguna operación.
    /// Se crea una nueva instancia de <see cref="ISqlBuilder"/> y se pasa a la acción.
    /// Luego, se llama al método <see cref="NotExists(ISqlBuilder)"/> con el generador construido.
    /// </remarks>
    public void NotExists(Action<ISqlBuilder> action)
    {
        if (action == null)
            return;
        var builder = SqlBuilder.New();
        action(builder);
        NotExists(builder);
    }

    #endregion

    #region AppendSql

    /// <inheritdoc />
    /// <summary>
    /// Agrega una condición SQL a la consulta actual.
    /// </summary>
    /// <param name="sql">La cadena SQL que se desea agregar como condición.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como una consulta en bruto.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="sql"/> es nulo o está vacío, no se realizará ninguna acción.
    /// Si <paramref name="raw"/> es verdadero, se agrega la condición SQL tal como está.
    /// Si <paramref name="raw"/> es falso, se reemplazan las partes de la cadena SQL que requieren un tratamiento especial antes de agregarla.
    /// </remarks>
    public void AppendSql(string sql, bool raw)
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

    #region Validate

    /// <inheritdoc />
    /// <summary>
    /// Valida si el resultado tiene longitud mayor a cero.
    /// </summary>
    /// <returns>
    /// Devuelve true si el resultado contiene elementos; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar si hay datos disponibles en el resultado.
    /// </remarks>
    public bool Validate()
    {
        return Result.Length > 0;
    }

    #endregion

    #region AppendTo

    /// <inheritdoc />
    /// <summary>
    /// Agrega una representación de la instancia actual a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se agregará la representación.</param>
    /// <remarks>
    /// Este método verifica si la instancia es válida antes de agregar información al <paramref name="builder"/>.
    /// Si la instancia no es válida, no se realiza ninguna acción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="builder"/> es null.</exception>
    /// <seealso cref="Validate"/>
    /// <seealso cref="StringBuilder"/>
    public void AppendTo(StringBuilder builder)
    {
        builder.CheckNull(nameof(builder));
        if (Validate() == false)
            return;
        builder.Append("Where ");
        builder.Append(Result);
    }

    #endregion

    #region Clear

    /// <inheritdoc />
    /// <summary>
    /// Limpia todos los elementos del resultado.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de resultados, 
    /// dejándola vacía. Es útil para reiniciar el estado de la colección 
    /// antes de agregar nuevos elementos.
    /// </remarks>
    public void Clear()
    {
        Result.Clear();
    }

    #endregion

    #region Clone

    /// <inheritdoc />
    /// <summary>
    /// Clona la cláusula WHERE actual y la asocia con un nuevo generador de SQL.
    /// </summary>
    /// <param name="builder">El generador de SQL que se utilizará para la nueva cláusula WHERE.</param>
    /// <returns>Una nueva instancia de <see cref="IWhereClause"/> que representa la cláusula WHERE clonada.</returns>
    /// <remarks>
    /// Este método crea una copia de la cláusula WHERE actual, manteniendo su contenido,
    /// y la asocia con el generador de SQL proporcionado.
    /// </remarks>
    /// <seealso cref="SqlBuilderBase"/>
    /// <seealso cref="WhereClause"/>
    public virtual IWhereClause Clone(SqlBuilderBase builder)
    {
        var result = new StringBuilder();
        result.Append(Result);
        return new WhereClause(builder, result);
    }

    #endregion
}