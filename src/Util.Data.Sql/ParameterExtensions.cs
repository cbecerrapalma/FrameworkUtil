using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para trabajar con parámetros.
/// </summary>
public static class ParameterExtensions
{

    #region AddDynamicParams(Agregar parámetros dinámicos)

    /// <summary>
    /// Agrega parámetros dinámicos al objeto fuente que implementa la interfaz <see cref="ISqlParameter"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que debe implementar <see cref="ISqlParameter"/>.</typeparam>
    /// <param name="source">El objeto fuente al que se le agregarán los parámetros dinámicos.</param>
    /// <param name="param">El objeto que contiene los parámetros dinámicos a agregar.</param>
    /// <returns>El objeto fuente con los parámetros dinámicos agregados.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método verifica si el objeto fuente es nulo y, si es así, lanza una excepción. 
    /// Si el objeto fuente implementa <see cref="ISqlPartAccessor"/>, se agregan los parámetros dinámicos 
    /// utilizando el administrador de parámetros del objeto.
    /// </remarks>
    /// <seealso cref="ISqlParameter"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AddDynamicParams<T>(this T source, object param) where T : ISqlParameter
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.ParameterManager.AddDynamicParams(param);
        return source;
    }

    #endregion

    #region AddParam(Agregar parámetros SQL)

    /// <summary>
    /// Agrega un parámetro a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que debe implementar <see cref="ISqlParameter"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará el parámetro.</param>
    /// <param name="name">El nombre del parámetro que se agregará.</param>
    /// <param name="dbType">El tipo de datos del parámetro.</param>
    /// <param name="direction">La dirección del parámetro (entrada, salida, etc.).</param>
    /// <returns>La fuente original con el nuevo parámetro agregado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="ISqlParameter"/> 
    /// permitiendo la adición de parámetros a través de un gestor de parámetros.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISqlParameter"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AddParam<T>(this T source, string name, DbType dbType, ParameterDirection direction) where T : ISqlParameter
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.ParameterManager.Add(name, null, dbType, direction);
        return source;
    }

    /// <summary>
    /// Agrega un parámetro a la fuente especificada.
    /// </summary>
    /// <typeparam name="T">El tipo de la fuente que implementa <see cref="ISqlParameter"/>.</typeparam>
    /// <param name="source">La fuente a la que se le agregará el parámetro.</param>
    /// <param name="name">El nombre del parámetro que se agregará.</param>
    /// <param name="value">El valor del parámetro. Por defecto es <c>null</c>.</param>
    /// <param name="dbType">El tipo de datos del parámetro. Por defecto es <c>null</c>.</param>
    /// <param name="direction">La dirección del parámetro. Por defecto es <c>null</c>.</param>
    /// <param name="size">El tamaño del parámetro. Por defecto es <c>null</c>.</param>
    /// <param name="precision">La precisión del parámetro. Por defecto es <c>null</c>.</param>
    /// <param name="scale">La escala del parámetro. Por defecto es <c>null</c>.</param>
    /// <returns>La fuente original con el parámetro agregado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la fuente permitiendo la adición de parámetros
    /// que pueden ser utilizados en consultas SQL. Asegúrese de que la fuente implementa
    /// <see cref="ISqlParameter"/> para que el método funcione correctamente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlParameter"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T AddParam<T>(this T source, string name, object value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null) where T : ISqlParameter
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.ParameterManager.Add(name, value, dbType, direction, size, precision, scale);
        return source;
    }

    #endregion

    #region GetParams(Obtener lista de parámetros.)

    /// <summary>
    /// Obtiene una lista de parámetros SQL a partir de una fuente que implementa <see cref="ISqlParameter"/>.
    /// </summary>
    /// <param name="source">La fuente de la que se obtendrán los parámetros SQL.</param>
    /// <returns>
    /// Una lista de parámetros SQL que se encuentran en la fuente, o <c>null</c> si la fuente no es un <see cref="ISqlPartAccessor"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="ISqlParameter"/> y permite acceder a los parámetros
    /// a través de la propiedad <see cref="ParameterManager"/> de la interfaz <see cref="ISqlPartAccessor"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <seealso cref="ISqlParameter"/>
    /// <seealso cref="ISqlPartAccessor"/>
    /// <seealso cref="SqlParam"/>
    public static IReadOnlyList<SqlParam> GetParams(this ISqlParameter source)
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            return accessor.ParameterManager.GetParams();
        return default;
    }

    #endregion

    #region GetDynamicParams(Obtener lista de parámetros dinámicos.)

    /// <summary>
    /// Obtiene una lista de parámetros dinámicos a partir de una fuente de parámetros SQL.
    /// </summary>
    /// <param name="source">La fuente de parámetros SQL desde la cual se obtendrán los parámetros dinámicos.</param>
    /// <returns>
    /// Una lista de objetos que representan los parámetros dinámicos, o <c>null</c> si la fuente no es un <see cref="ISqlPartAccessor"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlParameter"/> y verifica si la fuente es un <see cref="ISqlPartAccessor"/>.
    /// Si es así, se accede al administrador de parámetros para obtener los parámetros dinámicos.
    /// </remarks>
    public static IReadOnlyList<object> GetDynamicParams(this ISqlParameter source)
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            return accessor.ParameterManager.GetDynamicParams();
        return default;
    }

    #endregion

    #region GetParam(Obtener el valor del parámetro.)

    /// <summary>
    /// Obtiene el valor de un parámetro de tipo específico desde una fuente de parámetros SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="source">La fuente de parámetros SQL desde la cual se obtiene el valor.</param>
    /// <param name="name">El nombre del parámetro cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor del parámetro especificado, convertido al tipo <typeparamref name="T"/>. 
    /// Si el parámetro no se encuentra o la conversión falla, se devuelve el valor predeterminado de <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="ISqlParameter"/> y permite acceder a los parámetros de manera segura,
    /// verificando primero si la fuente es nula y luego tratando de obtener el parámetro a través de diferentes interfaces.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la fuente de parámetros es nula.</exception>
    public static T GetParam<T>(this ISqlParameter source, string name)
    {
        source.CheckNull(nameof(source));
        if (source is IGetParameter target)
            return target.GetParam<T>(name);
        if (source is ISqlPartAccessor accessor)
            return (T)accessor.ParameterManager.GetValue(name);
        return default;
    }

    #endregion

    #region ClearParams(Limpiar parámetros de Sql)

    /// <summary>
    /// Limpia los parámetros de un objeto que implementa la interfaz <see cref="ISqlParameter"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="ISqlParameter"/>.</typeparam>
    /// <param name="source">El objeto del cual se desean limpiar los parámetros.</param>
    /// <returns>El objeto original después de limpiar los parámetros.</returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es de tipo <see cref="IClearParameters"/> 
    /// y, si es así, llama al método <see cref="IClearParameters.ClearParams"/> para limpiar los parámetros.
    /// Si el objeto es de tipo <see cref="ISqlPartAccessor"/>, se limpia la colección de parámetros a través 
    /// del <see cref="ParameterManager"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <seealso cref="ISqlParameter"/>
    /// <seealso cref="IClearParameters"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearParams<T>(this T source) where T : ISqlParameter
    {
        source.CheckNull(nameof(source));
        if (source is IClearParameters target)
        {
            target.ClearParams();
            return source;
        }
        if (source is ISqlPartAccessor accessor)
            accessor.ParameterManager.Clear();
        return source;
    }

    #endregion
}