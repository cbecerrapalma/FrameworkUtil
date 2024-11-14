using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql;

/// <summary>
/// Proporciona métodos de extensión para construir consultas SQL utilizando el patrón Set.
/// </summary>
public static class SqlBuilderSetExtensions
{

    #region Union(Combinar conjuntos de resultados.)

    /// <summary>
    /// Realiza una unión de conjuntos SQL utilizando el objeto fuente y un conjunto de constructores SQL proporcionados.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que debe implementar la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El objeto fuente sobre el cual se realizará la unión.</param>
    /// <param name="builders">Un conjunto de constructores SQL que se utilizarán en la operación de unión.</param>
    /// <returns>El objeto fuente después de aplicar la unión con los constructores SQL.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan la interfaz <see cref="ISet"/> 
    /// permitiendo combinar múltiples constructores SQL en una única operación.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Union<T>(this T source, params ISqlBuilder[] builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Union(builders);
        return source;
    }

    /// <summary>
    /// Realiza una unión de conjuntos SQL utilizando un conjunto de constructores SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del conjunto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El conjunto de origen sobre el cual se realizará la unión.</param>
    /// <param name="builders">Una colección de constructores SQL que se unirán al conjunto de origen.</param>
    /// <returns>El conjunto de origen después de aplicar la unión con los constructores SQL proporcionados.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los conjuntos que implementan <see cref="ISet"/> 
    /// permitiendo la combinación de múltiples constructores SQL en una sola operación.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Union<T>(this T source, IEnumerable<ISqlBuilder> builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Union(builders);
        return source;
    }

    #endregion

    #region UnionAll(Combinar conjuntos de resultados.)

    /// <summary>
    /// Realiza una operación de unión de todos los conjuntos de SQL especificados en el parámetro <paramref name="builders"/> 
    /// con el conjunto de origen.
    /// </summary>
    /// <typeparam name="T">El tipo del conjunto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El conjunto de origen sobre el cual se realiza la operación de unión.</param>
    /// <param name="builders">Un arreglo de constructores de SQL que se unirán al conjunto de origen.</param>
    /// <returns>El conjunto de origen después de aplicar la operación de unión.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Esta extensión permite combinar múltiples conjuntos de SQL en uno solo, facilitando la construcción de consultas más complejas.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T UnionAll<T>(this T source, params ISqlBuilder[] builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.UnionAll(builders);
        return source;
    }

    /// <summary>
    /// Realiza una operación de unión de todos los conjuntos de SQL especificados en el conjunto de origen.
    /// </summary>
    /// <typeparam name="T">El tipo del conjunto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El conjunto de origen sobre el cual se realizará la operación de unión.</param>
    /// <param name="builders">Una colección de constructores de SQL que se unirán al conjunto de origen.</param>
    /// <returns>El conjunto de origen después de aplicar la operación de unión.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los conjuntos que implementan la interfaz <see cref="ISet"/> 
    /// permitiendo combinar múltiples constructores de SQL en una sola operación.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T UnionAll<T>(this T source, IEnumerable<ISqlBuilder> builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.UnionAll(builders);
        return source;
    }

    #endregion

    #region Intersect(Intersección)

    /// <summary>
    /// Realiza la intersección de un conjunto con los constructores SQL proporcionados.
    /// </summary>
    /// <typeparam name="T">El tipo del conjunto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El conjunto sobre el cual se realizará la intersección.</param>
    /// <param name="builders">Los constructores SQL que se utilizarán para la intersección.</param>
    /// <returns>El conjunto original después de aplicar la intersección.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los conjuntos que implementan la interfaz <see cref="ISet"/> 
    /// al permitir la intersección con múltiples constructores SQL.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Intersect<T>(this T source, params ISqlBuilder[] builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Intersect(builders);
        return source;
    }

    /// <summary>
    /// Realiza la intersección de un conjunto con una colección de constructores SQL.
    /// </summary>
    /// <typeparam name="T">El tipo del conjunto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El conjunto sobre el cual se realizará la intersección.</param>
    /// <param name="builders">Una colección de constructores SQL que se utilizarán para la intersección.</param>
    /// <returns>El conjunto original después de aplicar la intersección.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los conjuntos que implementan la interfaz <see cref="ISet"/> 
    /// al permitir la intersección con otros constructores SQL, facilitando la construcción de consultas más complejas.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Intersect<T>(this T source, IEnumerable<ISqlBuilder> builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Intersect(builders);
        return source;
    }

    #endregion

    #region Except(Diferencia de conjuntos)

    /// <summary>
    /// Elimina los constructores SQL especificados del conjunto de constructores SQL del objeto fuente.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto fuente que debe implementar la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El objeto fuente del cual se eliminarán los constructores SQL.</param>
    /// <param name="builders">Los constructores SQL que se desean eliminar del conjunto.</param>
    /// <returns>
    /// El objeto fuente después de haber eliminado los constructores SQL especificados.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T Except<T>(this T source, params ISqlBuilder[] builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Except(builders);
        return source;
    }

    /// <summary>
    /// Excluye un conjunto de constructores SQL de un objeto que implementa la interfaz <see cref="ISet"/>.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto que implementa la interfaz <see cref="ISet"/>.</typeparam>
    /// <param name="source">El objeto del cual se excluirán los constructores SQL.</param>
    /// <param name="builders">Una colección de constructores SQL a excluir.</param>
    /// <returns>
    /// El objeto <paramref name="source"/> después de haber excluido los constructores SQL especificados.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de los objetos que implementan <see cref="ISet"/> 
    /// permitiendo la exclusión de constructores SQL específicos de su conjunto.
    /// </remarks>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlBuilder"/>
    public static T Except<T>(this T source, IEnumerable<ISqlBuilder> builders) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Except(builders);
        return source;
    }

    #endregion

    #region ClearSets(Limpiar colección)

    /// <summary>
    /// Limpia los conjuntos de un objeto que implementa la interfaz <see cref="ISet"/>.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que implementa <see cref="ISet"/>.</typeparam>
    /// <param name="source">El objeto del cual se limpiarán los conjuntos.</param>
    /// <returns>El objeto <paramref name="source"/> después de limpiar los conjuntos.</returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo y lanza una excepción si es así.
    /// Si el objeto implementa <see cref="ISqlPartAccessor"/>, se limpiará el conjunto asociado a su 
    /// <see cref="ISqlPartAccessor.SqlBuilderSet"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    /// <seealso cref="ISet"/>
    /// <seealso cref="ISqlPartAccessor"/>
    public static T ClearSets<T>(this T source) where T : ISet
    {
        source.CheckNull(nameof(source));
        if (source is ISqlPartAccessor accessor)
            accessor.SqlBuilderSet.Clear();
        return source;
    }

    #endregion
}