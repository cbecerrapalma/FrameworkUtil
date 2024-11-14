using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para la cláusula SELECT en consultas LINQ.
/// </summary>
public static class SelectClauseExtensions
{

    #region Select(Configurar columna)

    /// <summary>
    /// Extensión que permite aplicar una operación de selección sobre un objeto que implementa la interfaz <see cref="ISelect"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="ISelect"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se aplicará la operación de selección.</param>
    /// <returns>El objeto original después de aplicar la operación de selección.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es null y lanza una excepción si es el caso.
    /// Si el objeto implementa <see cref="ISqlPartAccessor"/>, se invoca el método <see cref="Select"/> de la cláusula de selección.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Select<T>(this T source) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.Select();
        return source;
    }

    /// <summary>
    /// Extiende la funcionalidad de un objeto que implementa la interfaz <see cref="ISelect"/> 
    /// para permitir la selección de columnas específicas en una consulta SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="ISelect"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se realiza la selección de columnas.</param>
    /// <param name="columns">Una cadena que representa las columnas a seleccionar.</param>
    /// <returns>El objeto fuente después de aplicar la selección de columnas.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método verifica si el objeto fuente es nulo y, si es así, lanza una excepción. 
    /// Si el objeto fuente implementa <see cref="ISqlPartAccessor"/>, se llama al método 
    /// <see cref="ISqlPartAccessor.SelectClause.Select(string)"/> para realizar la selección de columnas.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Select<T>(this T source, string columns) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.Select(columns);
        return source;
    }

    /// <summary>
    /// Extensión que permite seleccionar una cláusula SQL a partir de un objeto que implementa la interfaz <see cref="ISelect"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="ISelect"/>.</typeparam>
    /// <param name="source">El objeto fuente del cual se seleccionará la cláusula SQL.</param>
    /// <param name="builder">El constructor SQL que se utilizará para construir la consulta.</param>
    /// <param name="alias">El alias que se asignará a la cláusula seleccionada.</param>
    /// <returns>El objeto fuente después de aplicar la selección de la cláusula SQL.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Select<T>(this T source, ISqlBuilder builder, string alias) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.Select(builder, alias);
        return source;
    }

    /// <summary>
    /// Extensión que permite seleccionar una cláusula SQL a partir de un objeto que implementa <see cref="ISelect"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa <see cref="ISelect"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se realizará la selección.</param>
    /// <param name="action">La acción que define la cláusula SQL a seleccionar.</param>
    /// <param name="alias">El alias que se asignará a la selección.</param>
    /// <returns>El objeto fuente después de aplicar la selección.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Esta extensión permite construir dinámicamente una cláusula SQL utilizando un <see cref="ISqlBuilder"/>.
    /// Es útil en contextos donde se requiere construir consultas SQL de manera fluida.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Select<T>(this T source, Action<ISqlBuilder> action, string alias) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.Select(action, alias);
        return source;
    }

    #endregion

    #region AppendSelect(Agregar a la cláusula Select)

    /// <summary>
    /// Agrega una cláusula SELECT a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que debe implementar la interfaz <see cref="ISelect"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula SELECT.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula SELECT a agregar.</param>
    /// <param name="raw">Indica si la cláusula SQL debe ser tratada como texto sin procesar.</param>
    /// <returns>La fuente original con la cláusula SELECT agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="ISelect"/> 
    /// permitiendo agregar dinámicamente cláusulas SELECT a las instancias que la implementan.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendSelect<T>(this T source, string sql, bool raw = false) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.AppendSql(sql, raw);
        return source;
    }

    /// <summary>
    /// Agrega una cláusula SELECT a la fuente especificada utilizando el generador de SQL proporcionado.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente, que debe implementar la interfaz <see cref="ISelect"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula SELECT.</param>
    /// <param name="builder">El generador de SQL que se utilizará para construir la cláusula SELECT.</param>
    /// <returns>La fuente original con la cláusula SELECT agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de cualquier objeto que implemente <see cref="ISelect"/> 
    /// permitiendo agregar una cláusula SELECT a través de un generador de SQL.
    /// </remarks>
    public static T AppendSelect<T>(this T source, ISqlBuilder builder) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.AppendSql(builder);
        return source;
    }

    /// <summary>
    /// Agrega una cláusula de selección a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="ISelect"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula de selección.</param>
    /// <param name="action">Una acción que define cómo se debe construir la cláusula de selección.</param>
    /// <returns>La fuente original con la cláusula de selección agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="ISelect"/> 
    /// permitiendo agregar dinámicamente cláusulas de selección mediante el uso de 
    /// un <see cref="Action{ISqlBuilder}"/>.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T AppendSelect<T>(this T source, Action<ISqlBuilder> action) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.AppendSql(action);
        return source;
    }

    #endregion

    #region ClearSelect(Vaciar la cláusula Select.)

    /// <summary>
    /// Limpia la cláusula SELECT del objeto que implementa la interfaz <see cref="ISelect"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="ISelect"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula SELECT.</param>
    /// <returns>El objeto <paramref name="source"/> después de limpiar la cláusula SELECT.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite limpiar la cláusula SELECT de un objeto que implementa la interfaz <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <seealso cref="ISelect"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearSelect<T>(this T source) where T : ISelect
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SelectClause.Clear();
        return source;
    }

    #endregion
}