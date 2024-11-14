using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para la cláusula INSERT en operaciones de base de datos.
/// </summary>
public static class InsertClauseExtensions
{

    #region Insert(Configurar el conjunto de nombres de tablas y columnas a insertar.)

    /// <summary>
    /// Inserta una cláusula de inserción en el objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IInsert"/>.</typeparam>
    /// <param name="source">El objeto en el que se va a insertar la cláusula.</param>
    /// <param name="columns">Las columnas en las que se va a realizar la inserción.</param>
    /// <param name="table">El nombre de la tabla en la que se va a realizar la inserción. Si es null, se utilizará la tabla predeterminada.</param>
    /// <returns>El objeto fuente con la cláusula de inserción aplicada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IInsert"/> 
    /// permitiendo agregar cláusulas de inserción de manera fluida.
    /// </remarks>
    /// <seealso cref="IInsert"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Insert<T>(this T source, string columns, string table = null) where T : IInsert
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.InsertClause.Insert(columns, table);
        return source;
    }

    #endregion

    #region Values(Configurar el conjunto de valores a insertar.)

    /// <summary>
    /// Establece los valores para la cláusula de inserción de un objeto que implementa la interfaz <see cref="IInsert"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IInsert"/>.</typeparam>
    /// <param name="source">El objeto sobre el cual se establece la cláusula de inserción.</param>
    /// <param name="values">Los valores a insertar en la cláusula.</param>
    /// <returns>El objeto original con la cláusula de inserción actualizada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="IInsert"/> 
    /// permitiendo establecer múltiples valores para la cláusula de inserción de manera fluida.
    /// </remarks>
    /// <seealso cref="IInsert"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Values<T>(this T source, params object[] values) where T : IInsert
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.InsertClause.Values(values);
        return source;
    }

    #endregion

    #region AppendInsert(Agregar a la cláusula Insertar)

    /// <summary>
    /// Agrega una cláusula de inserción a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará la cláusula de inserción.</param>
    /// <param name="sql">La cadena SQL que representa la cláusula de inserción.</param>
    /// <param name="raw">Indica si la cláusula SQL se debe tratar como cruda.</param>
    /// <returns>La fuente original con la cláusula de inserción agregada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo agregar una cláusula de inserción 
    /// de manera fluida. Se asegura de que la fuente no sea nula antes de intentar agregar la cláusula.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendInsert<T>(this T source, string sql, bool raw = false) where T : IFrom
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.InsertClause.AppendInsert(sql, raw);
        return source;
    }

    #endregion

    #region AppendValues(Agregar a la cláusula Values)

    /// <summary>
    /// Agrega valores a la cláusula de inserción de un objeto que implementa la interfaz <see cref="IFrom"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto al que se le agregarán los valores.</param>
    /// <param name="sql">La cadena SQL que contiene los valores a agregar.</param>
    /// <param name="raw">Indica si los valores deben ser tratados como crudos (true) o no (false).</param>
    /// <returns>El objeto original con los valores agregados.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite modificar la cláusula de inserción de un objeto que implementa <see cref="IFrom"/>.
    /// Asegúrese de que el objeto de origen no sea nulo antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AppendValues<T>(this T source, string sql, bool raw = false) where T : IFrom
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.InsertClause.AppendValues(sql, raw);
        return source;
    }

    #endregion

    #region ClearInsert(Vaciar la cláusula Insertar)

    /// <summary>
    /// Limpia la cláusula de inserción del objeto que implementa la interfaz <see cref="IFrom"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa la interfaz <see cref="IFrom"/>.</typeparam>
    /// <param name="source">El objeto del cual se desea limpiar la cláusula de inserción.</param>
    /// <returns>El mismo objeto <paramref name="source"/> después de limpiar la cláusula de inserción.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método es una extensión que permite limpiar la cláusula de inserción de un objeto que implementa <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <seealso cref="IFrom"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearInsert<T>(this T source) where T : IFrom
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.InsertClause.Clear();
        return source;
    }

    #endregion
}