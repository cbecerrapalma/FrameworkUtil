using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="StartClause"/>.
/// </summary>
public static class StartClauseExtensions
{

    #region Cte(Configurar una expresión de tabla común (CTE).)

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa <see cref="IStart"/> 
    /// para agregar una cláusula CTE (Common Table Expression) a la consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IStart"/>.</typeparam>
    /// <param name="source">El objeto fuente que se va a extender.</param>
    /// <param name="name">El nombre de la cláusula CTE.</param>
    /// <param name="builder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <returns>El objeto fuente con la cláusula CTE agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método permite agregar una cláusula CTE a una consulta SQL de manera fluida,
    /// facilitando la construcción de consultas más complejas.
    /// </remarks>
    /// <seealso cref="IStart"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Cte<T>(this T source, string name, ISqlBuilder builder) where T : IStart
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.StartClause.Cte(name, builder);
        return source;
    }

    #endregion

    #region Append(Agregar al inicio de SQL.)

    /// <summary>
    /// Agrega una cláusula SQL a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IStart"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula SQL.</param>
    /// <param name="sql">La cláusula SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL es en formato crudo. El valor predeterminado es <c>false</c>.</param>
    /// <returns>La fuente original después de agregar la cláusula SQL.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo la adición de cláusulas SQL de manera fluida.
    /// </remarks>
    /// <seealso cref="IStart"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Append<T>(this T source, string sql, bool raw = false) where T : IStart
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.StartClause.Append(sql, raw);
        return source;
    }

    #endregion

    #region AppendLine(Agregar al inicio de Sql y saltar de línea.)

    /// <summary>
    /// Agrega una nueva línea de texto SQL al objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente, que debe implementar la interfaz <see cref="IStart"/>.</typeparam>
    /// <param name="source">El objeto fuente al que se le agregará la línea SQL.</param>
    /// <param name="sql">La línea de texto SQL que se desea agregar.</param>
    /// <param name="raw">Indica si la línea SQL debe ser tratada como texto sin procesar. El valor predeterminado es <c>false</c>.</param>
    /// <returns>El objeto fuente modificado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método es una extensión que permite agregar fácilmente líneas SQL a un objeto que implementa <see cref="IStart"/>.
    /// Asegúrese de que el objeto fuente no sea <c>null</c> antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IStart"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendLine<T>(this T source, string sql, bool raw = false) where T : IStart
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.StartClause.AppendLine(sql, raw);
        return source;
    }

    #endregion

    #region AppendStart(Agregar al inicio de Sql)

    /// <summary>
    /// Agrega una cadena SQL al inicio del objeto que implementa la interfaz <see cref="IStart"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IStart"/>.</typeparam>
    /// <param name="source">El objeto al que se le agregará la cadena SQL.</param>
    /// <param name="sql">La cadena SQL que se desea agregar al inicio.</param>
    /// <param name="raw">Indica si la cadena SQL debe ser tratada como cruda.</param>
    /// <returns>El objeto <paramref name="source"/> con la cadena SQL agregada al inicio.</returns>
    /// <seealso cref="IStart"/>
    public static T AppendStart<T>(this T source, string sql, bool raw = false) where T : IStart
    {
        return source.Append(sql, raw);
    }

    #endregion

    #region ClearCte(Limpiar la expresión de tabla común (CTE))

    /// <summary>
    /// Limpia la cláusula CTE (Common Table Expression) del objeto que implementa <see cref="IStart"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IStart"/>.</typeparam>
    /// <param name="source">El objeto del cual se limpiará la cláusula CTE.</param>
    /// <returns>El mismo objeto <paramref name="source"/> después de limpiar la cláusula CTE.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IStart"/> 
    /// permitiendo limpiar la cláusula CTE si el objeto también implementa <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IStart"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearCte<T>(this T source) where T : IStart
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.StartClause.ClearCte();
        return source;
    }

    #endregion

    #region ClearStart(Limpiar la cláusula inicial.)

    /// <summary>
    /// Limpia la cláusula de inicio del objeto que implementa la interfaz <see cref="IStart"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IStart"/>.</typeparam>
    /// <param name="source">El objeto del cual se limpiará la cláusula de inicio.</param>
    /// <returns>El objeto original después de limpiar la cláusula de inicio.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión que permite limpiar la cláusula de inicio de un objeto que implementa la interfaz <see cref="ISqlPartAccessor"/>.
    /// Si el objeto no es de tipo <see cref="ISqlPartAccessor"/>, no se realizará ninguna acción sobre la cláusula de inicio.
    /// </remarks>
    /// <seealso cref="IStart"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearStart<T>(this T source) where T : IStart
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.StartClause.Clear();
        return source;
    }

    #endregion
}