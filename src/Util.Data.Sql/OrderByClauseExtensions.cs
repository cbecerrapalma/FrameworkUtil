using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para ordenar colecciones de datos.
/// </summary>
public static class OrderByClauseExtensions
{

    #region OrderBy(Ordenar)

    /// <summary>
    /// Ordena el objeto fuente según la cláusula de ordenamiento especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que implementa la interfaz <see cref="IOrderBy"/>.</typeparam>
    /// <param name="source">El objeto fuente que se va a ordenar.</param>
    /// <param name="order">La cadena que representa la cláusula de ordenamiento.</param>
    /// <returns>El objeto fuente ordenado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el objeto fuente es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IOrderBy"/> 
    /// permitiendo aplicar una cláusula de ordenamiento a través de una cadena.
    /// </remarks>
    /// <seealso cref="IOrderBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T OrderBy<T>(this T source, string order) where T : IOrderBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.OrderByClause.OrderBy(order);
        return source;
    }

    #endregion

    #region AppendOrderBy(Agregar a la cláusula Order By.)

    /// <summary>
    /// Agrega una cláusula ORDER BY a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IOrderBy"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula ORDER BY.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula ORDER BY a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar. El valor predeterminado es <c>false</c>.</param>
    /// <returns>La fuente original con la cláusula ORDER BY agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IOrderBy"/> 
    /// permitiendo la adición de cláusulas ORDER BY de manera fluida.
    /// </remarks>
    /// <seealso cref="IOrderBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendOrderBy<T>(this T source, string sql, bool raw = false) where T : IOrderBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.OrderByClause.AppendSql(sql, raw);
        return source;
    }

    #endregion

    #region ClearOrderBy(Vaciar la cláusula Order By.)

    /// <summary>
    /// Limpia la cláusula de ordenamiento de un objeto que implementa la interfaz <see cref="IOrderBy"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IOrderBy"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula de ordenamiento.</param>
    /// <returns>El objeto original después de limpiar la cláusula de ordenamiento.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite a los objetos que implementan <see cref="IOrderBy"/>
    /// limpiar su cláusula de ordenamiento de manera sencilla.
    /// </remarks>
    /// <seealso cref="IOrderBy"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearOrderBy<T>(this T source) where T : IOrderBy
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.OrderByClause.Clear();
        return source;
    }

    #endregion
}