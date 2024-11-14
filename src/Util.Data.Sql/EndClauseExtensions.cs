using Util.Data.Queries;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="EndClause"/>.
/// </summary>
public static class EndClauseExtensions
{

    #region Skip(Configurar el número de filas a omitir.)

    /// <summary>
    /// Salta un número especificado de elementos en la fuente.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IEnd"/>.</typeparam>
    /// <param name="source">La fuente de la que se saltarán los elementos.</param>
    /// <param name="count">El número de elementos a saltar.</param>
    /// <returns>
    /// Devuelve la fuente original después de aplicar la operación de salto.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IEnd"/>.
    /// Si la fuente es un <see cref="ISqlPartAccessor"/>, se aplicará el método <c>Skip</c> en la cláusula final.
    /// </remarks>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Skip<T>(this T source, int count) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.Skip(count);
        return source;
    }

    #endregion

    #region Take(Configurar la obtención de la cantidad de filas.)

    /// <summary>
    /// Toma un número específico de elementos de una fuente que implementa la interfaz <see cref="IEnd"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que debe implementar <see cref="IEnd"/>.</typeparam>
    /// <param name="source">La fuente de la que se tomarán los elementos.</param>
    /// <param name="count">El número de elementos a tomar de la fuente.</param>
    /// <returns>
    /// Devuelve la fuente original después de aplicar la operación de toma.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="IEnd"/> 
    /// permitiendo tomar un número específico de elementos mediante la cláusula <see cref="ISqlPartAccessor.EndClause"/>.
    /// </remarks>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Take<T>(this T source, int count) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.Take(count);
        return source;
    }

    #endregion

    #region Page(Configurar la paginación.)

    /// <summary>
    /// Extiende la funcionalidad del tipo especificado para permitir la paginación.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IEnd"/>.</typeparam>
    /// <param name="source">La instancia del objeto que se va a extender.</param>
    /// <param name="page">El objeto de tipo <see cref="IPage"/> que contiene la información de paginación.</param>
    /// <returns>Devuelve la instancia original del objeto <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es un <see cref="ISqlPartAccessor"/> 
    /// y, en caso afirmativo, llama al método <c>Page</c> en la cláusula de fin del acceso SQL.
    /// </remarks>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="IPage"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Page<T>(this T source, IPage page) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.Page(page);
        return source;
    }

    #endregion

    #region AppendEnd(Agregar al final de Sql)

    /// <summary>
    /// Agrega una cláusula al final de un objeto que implementa la interfaz <see cref="IEnd"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IEnd"/>.</typeparam>
    /// <param name="source">El objeto al que se le agregará la cláusula.</param>
    /// <param name="sql">La cadena SQL que se desea agregar al final.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como cruda.</param>
    /// <returns>El objeto original con la cláusula SQL agregada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IEnd"/> 
    /// permitiendo agregar una cláusula SQL al final de su definición.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendEnd<T>(this T source, string sql, bool raw = false) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.AppendSql(sql, raw);
        return source;
    }

    #endregion

    #region ClearPage(Limpiar paginación)

    /// <summary>
    /// Limpia la página de la cláusula final del objeto que implementa <see cref="IEnd"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="IEnd"/>.</typeparam>
    /// <param name="source">El objeto del que se desea limpiar la página.</param>
    /// <returns>El mismo objeto <paramref name="source"/> después de limpiar la página.</returns>
    /// <remarks>
    /// Este método es una extensión que permite limpiar la página de la cláusula final
    /// de un objeto que implementa la interfaz <see cref="ISqlPartAccessor"/>.
    /// Se lanza una excepción si <paramref name="source"/> es nulo.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearPage<T>(this T source) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.ClearPage();
        return source;
    }

    #endregion

    #region ClearEnd(Limpiar la cláusula final.)

    /// <summary>
    /// Limpia la cláusula final del objeto que implementa la interfaz <see cref="IEnd"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IEnd"/>.</typeparam>
    /// <param name="source">El objeto del cual se limpiará la cláusula final.</param>
    /// <returns>El mismo objeto <paramref name="source"/> después de limpiar la cláusula final.</returns>
    /// <remarks>
    /// Este método es una extensión que permite limpiar la cláusula final de un objeto que implementa la interfaz <see cref="ISqlPartAccessor"/>.
    /// Se lanza una excepción si <paramref name="source"/> es nulo.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="IEnd"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearEnd<T>(this T source) where T : IEnd
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.EndClause.Clear();
        return source;
    }

    #endregion
}