using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para trabajar con cláusulas FROM en consultas.
/// </summary>
public static class FromClauseExtensions
{

    #region From(Configuración de la tabla.)

    /// <summary>
    /// Extensión que permite agregar una cláusula FROM a un objeto que implementa la interfaz <see cref="IFrom"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se aplica la extensión.</param>
    /// <param name="table">El nombre de la tabla que se agregará a la cláusula FROM.</param>
    /// <returns>El mismo objeto <paramref name="source"/> con la cláusula FROM actualizada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Esta extensión verifica si el objeto <paramref name="source"/> es nulo y lanza una excepción si es el caso.
    /// Si el objeto implementa <see cref="ISqlPartAccessor"/>, se actualiza la cláusula FROM con el nombre de la tabla proporcionada.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T From<T>(this T source, string table) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.FromClause.From(table);
        return source;
    }

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa la interfaz <see cref="IFrom"/> 
    /// para agregar una cláusula FROM a un constructor SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto fuente que se va a extender.</param>
    /// <param name="builder">El constructor SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la tabla en la cláusula FROM.</param>
    /// <returns>El objeto fuente modificado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método permite a los objetos que implementan <see cref="IFrom"/> 
    /// agregar una cláusula FROM a una consulta SQL mediante el uso de un constructor SQL.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T From<T>(this T source, ISqlBuilder builder, string alias) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.FromClause.From(builder, alias);
        return source;
    }

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa <see cref="IFrom"/> 
    /// permitiendo aplicar una acción sobre el constructor SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se aplicará la acción.</param>
    /// <param name="action">La acción que se aplicará al constructor SQL.</param>
    /// <param name="alias">El alias que se utilizará en la cláusula FROM.</param>
    /// <returns>El objeto fuente modificado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método permite encadenar la construcción de consultas SQL al 
    /// proporcionar un acceso fácil a la cláusula FROM y aplicar configuraciones adicionales 
    /// a través de la acción proporcionada.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T From<T>(this T source, Action<ISqlBuilder> action, string alias) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.FromClause.From(action, alias);
        return source;
    }

    #endregion

    #region AppendFrom(Agregar al cláusula From.)

    /// <summary>
    /// Agrega una cláusula SQL "FROM" a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula SQL.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula "FROM" a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como cruda (true) o no (false).</param>
    /// <returns>La fuente original con la cláusula SQL "FROM" agregada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de cualquier objeto que implemente la interfaz <see cref="IFrom"/>.
    /// Asegúrese de que el objeto fuente no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static T AppendFrom<T>(this T source, string sql, bool raw = false) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.FromClause.AppendSql(sql, raw);
        return source;
    }

    #endregion

    #region ClearFrom(Vaciar la cláusula From)

    /// <summary>
    /// Limpia la cláusula FROM del objeto que implementa la interfaz <see cref="IFrom"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula FROM.</param>
    /// <returns>El objeto original después de limpiar la cláusula FROM.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IFrom"/>.
    /// Si el objeto también implementa <see cref="ISqlPartAccessor"/>, se limpiará la cláusula FROM asociada.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearFrom<T>(this T source) where T : IFrom
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.FromClause.Clear();
        return source;
    }

    #endregion
}