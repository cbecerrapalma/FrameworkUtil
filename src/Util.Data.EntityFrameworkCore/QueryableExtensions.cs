using Util.Data.Queries;

namespace Util.Data.EntityFrameworkCore;

/// <summary>
/// Proporciona métodos de extensión para trabajar con colecciones de tipo <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{

    #region ToPageList(Obtener lista paginada.)

    /// <summary>
    /// Convierte un <see cref="IQueryable{TEntity}"/> en una lista paginada.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está paginando.</typeparam>
    /// <param name="source">La fuente de datos que se va a paginar.</param>
    /// <param name="parameter">Los parámetros de paginación que contienen información sobre el tamaño de la página y el número total de elementos.</param>
    /// <returns>Una instancia de <see cref="PageList{TEntity}"/> que contiene la lista paginada de entidades.</returns>
    /// <remarks>
    /// Este método verifica si el parámetro total es menor o igual a cero y, de ser así, lo establece en el conteo total de elementos en la fuente.
    /// Luego, ordena la fuente de datos según los parámetros proporcionados, omite los elementos según el número de página y toma el tamaño de la página especificado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> o <paramref name="parameter"/> son nulos.</exception>
    /// <seealso cref="IQueryable{TEntity}"/>
    /// <seealso cref="IPage"/>
    /// <seealso cref="PageList{TEntity}"/>
    public static PageList<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> source, IPage parameter) where TEntity : class
    {
        source.CheckNull(nameof(source));
        parameter.CheckNull(nameof(parameter));
        if (parameter.Total <= 0)
            parameter.Total = source.Count();
        var list = source.OrderBy(parameter, "Id").Skip(parameter.GetSkipCount()).Take(parameter.PageSize).ToList();
        return new PageList<TEntity>(parameter, list);
    }

    #endregion

    #region ToPageListAsync(Obtener lista paginada.)

    /// <summary>
    /// Convierte una consulta de tipo <see cref="IQueryable{TEntity}"/> en una lista paginada de resultados.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se está consultando.</typeparam>
    /// <param name="source">La fuente de datos que se va a paginar.</param>
    /// <param name="parameter">Los parámetros de paginación que contienen información como el tamaño de página y el número total de elementos.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado contiene una instancia de <see cref="PageList{TEntity}"/> que incluye la lista paginada de entidades.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el parámetro de paginación tiene un total de elementos menor o igual a cero y, en tal caso, lo establece en el conteo total de elementos de la fuente.
    /// Luego, ordena la fuente de datos según los parámetros proporcionados, omite los elementos según el número de página y el tamaño de página, y finalmente toma la cantidad especificada de elementos.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> o <paramref name="parameter"/> son nulos.</exception>
    /// <seealso cref="IQueryable{TEntity}"/>
    /// <seealso cref="PageList{TEntity}"/>
    public static async Task<PageList<TEntity>> ToPageListAsync<TEntity>(this IQueryable<TEntity> source, IPage parameter) where TEntity : class
    {
        source.CheckNull(nameof(source));
        parameter.CheckNull(nameof(parameter));
        if (parameter.Total <= 0)
            parameter.Total = await source.CountAsync();
        var list = await source.OrderBy(parameter, "Id").Skip(parameter.GetSkipCount()).Take(parameter.PageSize).ToListAsync();
        return new PageList<TEntity>(parameter, list);
    }

    #endregion
}