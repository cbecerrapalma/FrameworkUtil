using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para trabajar con cláusulas de unión en consultas.
/// </summary>
public static class JoinClauseExtensions
{

    #region Join(Inner join)

    /// <summary>
    /// Agrega una cláusula JOIN a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula JOIN.</param>
    /// <param name="table">El nombre de la tabla que se unirá.</param>
    /// <returns>La fuente original con la cláusula JOIN aplicada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los tipos que implementan la interfaz <see cref="IJoin"/> 
    /// permitiendo agregar dinámicamente cláusulas JOIN a las consultas SQL.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Join<T>(this T source, string table) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.Join(table);
        return source;
    }

    /// <summary>
    /// Une la fuente actual a una cláusula SQL utilizando un constructor SQL especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le aplicará la unión.</param>
    /// <param name="builder">El constructor SQL que se utilizará para construir la cláusula de unión.</param>
    /// <param name="alias">El alias que se asignará a la unión en la cláusula SQL.</param>
    /// <returns>La fuente original después de aplicar la unión.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="IJoin"/> 
    /// permitiendo agregar cláusulas de unión a un constructor SQL.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Join<T>(this T source, ISqlBuilder builder, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.Join(builder, alias);
        return source;
    }

    /// <summary>
    /// Une la fuente actual a otra parte de la consulta SQL utilizando una acción especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le aplicará la unión.</param>
    /// <param name="action">La acción que define cómo se debe construir la cláusula de unión.</param>
    /// <param name="alias">El alias que se asignará a la parte unida.</param>
    /// <returns>La fuente original después de aplicar la unión.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método permite agregar una cláusula de unión a una consulta SQL de manera fluida.
    /// Es útil en la construcción de consultas complejas donde se requiere unir varias tablas o partes.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Join<T>(this T source, Action<ISqlBuilder> action, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.Join(action, alias);
        return source;
    }

    #endregion

    #region LeftJoin(Left outer join)

    /// <summary>
    /// Realiza una unión izquierda (Left Join) con la tabla especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente sobre la cual se realiza la unión izquierda.</param>
    /// <param name="table">El nombre de la tabla con la que se realizará la unión izquierda.</param>
    /// <returns>
    /// La fuente original después de aplicar la unión izquierda.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo realizar una unión izquierda
    /// con la tabla especificada. Se verifica si la fuente es un <see cref="ISqlPartAccessor"/> 
    /// para poder acceder a la cláusula de unión y aplicar la operación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    public static T LeftJoin<T>(this T source, string table) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.LeftJoin(table);
        return source;
    }

    /// <summary>
    /// Realiza una unión izquierda (Left Join) en la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente de datos que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente de datos sobre la que se realizará la unión izquierda.</param>
    /// <param name="builder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida.</param>
    /// <returns>
    /// La fuente de datos original después de aplicar la unión izquierda.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de las fuentes de datos que implementan la interfaz <see cref="IJoin"/>.
    /// Asegúrese de que el objeto <paramref name="source"/> no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T LeftJoin<T>(this T source, ISqlBuilder builder, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.LeftJoin(builder, alias);
        return source;
    }

    /// <summary>
    /// Realiza una unión izquierda en la consulta SQL utilizando el generador de SQL especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se realiza la unión izquierda.</param>
    /// <param name="action">Una acción que define cómo se construye la cláusula de unión.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida.</param>
    /// <returns>La fuente original después de aplicar la unión izquierda.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método permite agregar una cláusula de unión izquierda a una consulta SQL, 
    /// facilitando la combinación de datos de diferentes tablas mientras se preservan todos los registros de la tabla principal.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    public static T LeftJoin<T>(this T source, Action<ISqlBuilder> action, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.LeftJoin(action, alias);
        return source;
    }

    #endregion

    #region RightJoin(Right outer join)

    /// <summary>
    /// Realiza una unión a la derecha en la cláusula de unión de la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se realiza la unión a la derecha.</param>
    /// <param name="table">El nombre de la tabla con la que se realizará la unión.</param>
    /// <returns>
    /// La fuente original después de aplicar la unión a la derecha.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Esta extensión permite agregar una cláusula de unión a la derecha a una consulta SQL.
    /// Se requiere que la fuente implemente la interfaz <see cref="IJoin"/> y que sea accesible a través de <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T RightJoin<T>(this T source, string table) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.RightJoin(table);
        return source;
    }

    /// <summary>
    /// Realiza una unión derecha en la consulta SQL utilizando el constructor de SQL proporcionado.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente sobre la que se realiza la unión derecha.</param>
    /// <param name="builder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla unida.</param>
    /// <returns>Devuelve la fuente original después de aplicar la unión derecha.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo realizar una unión derecha
    /// en una consulta SQL. Asegúrese de que la fuente implementa la interfaz <see cref="IJoin"/>.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T RightJoin<T>(this T source, ISqlBuilder builder, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.RightJoin(builder, alias);
        return source;
    }

    /// <summary>
    /// Realiza una unión derecha en la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente sobre la cual se realiza la unión.</param>
    /// <param name="action">La acción que define la cláusula de unión.</param>
    /// <param name="alias">El alias que se utilizará para la tabla unida.</param>
    /// <returns>
    /// Devuelve la fuente original después de aplicar la unión derecha.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo agregar una cláusula de unión derecha
    /// en la construcción de una consulta SQL.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IJoin"/>
    public static T RightJoin<T>(this T source, Action<ISqlBuilder> action, string alias) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.RightJoin(action, alias);
        return source;
    }

    #endregion

    #region On(Establecer condiciones de conexión.)

    /// <summary>
    /// Agrega una cláusula ON a la parte SQL especificada.
    /// </summary>
    /// <typeparam name="T">El tipo que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La instancia sobre la que se aplica la cláusula ON.</param>
    /// <param name="column">El nombre de la columna que se utilizará en la cláusula ON.</param>
    /// <param name="value">El valor que se comparará con la columna especificada.</param>
    /// <param name="operator">El operador que se utilizará para la comparación. Por defecto es <see cref="Operator.Equal"/>.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta. Por defecto es <c>false</c>.</param>
    /// <returns>La instancia original de <typeparamref name="T"/> con la cláusula ON aplicada.</returns>
    /// <remarks>
    /// Este método permite construir dinámicamente una cláusula ON para unirse a otras tablas en una consulta SQL.
    /// Asegúrese de que el objeto <paramref name="source"/> no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static T On<T>(this T source, string column, object value, Operator @operator = Operator.Equal, bool isParameterization = false) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.On(column, value, @operator, isParameterization);
        return source;
    }

    #endregion

    #region AppendJoin(Agregar a la cláusula de unión interna.)

    /// <summary>
    /// Agrega una cláusula de unión a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula de unión.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula de unión a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como cruda (true) o no (false).</param>
    /// <returns>
    /// La fuente original con la cláusula de unión agregada.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IJoin"/> permitiendo agregar dinámicamente cláusulas de unión
    /// a las consultas SQL generadas.
    /// </remarks>
    public static T AppendJoin<T>(this T source, string sql, bool raw = false) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.AppendJoin(sql, raw);
        return source;
    }

    #endregion

    #region AppendLeftJoin(Agregar a la cláusula de unión externa izquierda.)

    /// <summary>
    /// Agrega una cláusula de unión izquierda (LEFT JOIN) a la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula de unión izquierda.</param>
    /// <param name="sql">La cadena SQL que representa la tabla o subconsulta a unir.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar. El valor predeterminado es <c>false</c>.</param>
    /// <returns>La fuente original con la cláusula de unión izquierda agregada.</returns>
    /// <remarks>
    /// Este método permite extender la funcionalidad de las consultas SQL al agregar una unión izquierda,
    /// lo que permite incluir registros de la tabla de la izquierda incluso si no hay coincidencias en la tabla de la derecha.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="IJoin"/>
    public static T AppendLeftJoin<T>(this T source, string sql, bool raw = false) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.AppendLeftJoin(sql, raw);
        return source;
    }

    #endregion

    #region AppendRightJoin(Agregar a la cláusula de unión externa derecha.)

    /// <summary>
    /// Agrega una cláusula de unión derecha a la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula de unión derecha.</param>
    /// <param name="sql">La consulta SQL que se utilizará en la cláusula de unión.</param>
    /// <param name="raw">Indica si la consulta SQL debe ser tratada como texto sin procesar.</param>
    /// <returns>
    /// La fuente original con la cláusula de unión derecha agregada.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo la construcción de consultas SQL más complejas.
    /// </remarks>
    public static T AppendRightJoin<T>(this T source, string sql, bool raw = false) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.AppendRightJoin(sql, raw);
        return source;
    }

    #endregion

    #region AppendOn(Agregar a la cláusula On)

    /// <summary>
    /// Agrega una cláusula ON a la cláusula JOIN de un objeto que implementa la interfaz <see cref="IJoin"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IJoin"/>.</typeparam>
    /// <param name="source">El objeto al que se le agregará la cláusula ON.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula ON que se desea agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar. El valor predeterminado es <c>false</c>.</param>
    /// <returns>El objeto original <paramref name="source"/> con la cláusula ON agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método es una extensión que permite modificar la cláusula JOIN de un objeto que implementa <see cref="IJoin"/> 
    /// al agregar una cláusula ON específica. Se utiliza comúnmente en la construcción dinámica de consultas SQL.
    /// </remarks>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendOn<T>(this T source, string sql, bool raw = false) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.AppendOn(sql, raw);
        return source;
    }

    #endregion

    #region ClearJoin(Vaciar la cláusula Join.)

    /// <summary>
    /// Limpia la cláusula de unión de un objeto que implementa la interfaz <see cref="IJoin"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IJoin"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula de unión.</param>
    /// <returns>El objeto original después de limpiar la cláusula de unión.</returns>
    /// <remarks>
    /// Este método es una extensión que permite a cualquier objeto que implemente <see cref="IJoin"/> 
    /// limpiar su cláusula de unión de manera sencilla. Si el objeto es nulo, se lanzará una excepción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IJoin"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearJoin<T>(this T source) where T : IJoin
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.JoinClause.Clear();
        return source;
    }

    #endregion
}