using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para la cláusula GroupBy.
/// </summary>
public static class GroupByClauseExtensions
{

    #region GroupBy(Agregar columna de grupo.)

    /// <summary>
    /// Agrupa los elementos de la fuente según las columnas especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IGroupBy"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se aplicará la agrupación.</param>
    /// <param name="columns">Una cadena que representa las columnas por las cuales se realizará la agrupación.</param>
    /// <returns>
    /// La fuente original después de aplicar la cláusula de agrupación.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo agregar una cláusula de agrupación 
    /// a la consulta SQL subyacente si la fuente implementa <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <seealso cref="IGroupBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T GroupBy<T>(this T source, string columns) where T : IGroupBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.GroupByClause.GroupBy(columns);
        return source;
    }

    #endregion

    #region Having(Agregar condiciones de agrupamiento.)

    /// <summary>
    /// Agrega una cláusula HAVING a la consulta SQL basada en el objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IGroupBy"/>.</typeparam>
    /// <param name="source">El objeto fuente al que se le agregará la cláusula HAVING.</param>
    /// <param name="expression">La expresión que se utilizará en la cláusula HAVING.</param>
    /// <param name="value">El valor que se comparará con la expresión.</param>
    /// <param name="operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe usar la parametrización en la consulta. Por defecto es <c>true</c>.</param>
    /// <returns>El objeto fuente con la cláusula HAVING agregada.</returns>
    /// <remarks>
    /// Este método permite construir consultas SQL más complejas al agregar condiciones a los resultados agrupados.
    /// Asegúrese de que el objeto fuente no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto <paramref name="source"/> es nulo.</exception>
    public static T Having<T>(this T source, string expression, object value, Operator @operator = Operator.Equal, bool isParameterization = true) where T : IGroupBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.GroupByClause.Having(expression, value, @operator, isParameterization);
        return source;
    }

    #endregion

    #region AppendGroupBy(Agregar a la cláusula Group By.)

    /// <summary>
    /// Agrega una cláusula GROUP BY a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IGroupBy"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula GROUP BY.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula GROUP BY que se desea agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar. El valor predeterminado es <c>false</c>.</param>
    /// <returns>La fuente original con la cláusula GROUP BY agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de cualquier objeto que implemente <see cref="IGroupBy"/> 
    /// permitiendo agregar dinámicamente cláusulas GROUP BY a las consultas SQL.
    /// </remarks>
    /// <seealso cref="IGroupBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendGroupBy<T>(this T source, string sql, bool raw = false) where T : IGroupBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.GroupByClause.AppendGroupBy(sql, raw);
        return source;
    }

    #endregion

    #region AppendHaving(Agregar a la cláusula HAVING)

    /// <summary>
    /// Agrega una cláusula HAVING a la consulta SQL del objeto que implementa <see cref="IGroupBy"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IGroupBy"/>.</typeparam>
    /// <param name="source">El objeto al que se le añadirá la cláusula HAVING.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula HAVING a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar.</param>
    /// <returns>El objeto original con la cláusula HAVING añadida.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="IGroupBy"/> 
    /// permitiendo agregar cláusulas HAVING de manera fluida en la construcción de consultas SQL.
    /// </remarks>
    /// <seealso cref="IGroupBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendHaving<T>(this T source, string sql, bool raw = false) where T : IGroupBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.GroupByClause.AppendHaving(sql, raw);
        return source;
    }

    #endregion

    #region ClearGroupBy(Limpiar la cláusula Group By.)

    /// <summary>
    /// Limpia la cláusula GROUP BY de un objeto que implementa la interfaz <see cref="IGroupBy"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IGroupBy"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula GROUP BY.</param>
    /// <returns>El mismo objeto <paramref name="source"/> después de limpiar la cláusula GROUP BY.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite a los objetos que implementan <see cref="IGroupBy"/> 
    /// limpiar su cláusula GROUP BY de manera sencilla. Si el objeto también implementa 
    /// <see cref="ISqlPartAccessor"/>, se accederá a su propiedad <see cref="ISqlPartAccessor.GroupByClause"/> 
    /// para realizar la limpieza.
    /// </remarks>
    /// <seealso cref="IGroupBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearGroupBy<T>(this T source) where T : IGroupBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.GroupByClause.Clear();
        return source;
    }

    #endregion
}