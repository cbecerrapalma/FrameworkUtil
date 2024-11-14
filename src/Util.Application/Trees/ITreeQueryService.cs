using Util.Data.Queries;

namespace Util.Applications.Trees; 

/// <summary>
/// Define un servicio de consulta para árboles que permite realizar operaciones de consulta 
/// sobre estructuras jerárquicas.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa 
/// los nodos del árbol.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que contiene los criterios de consulta 
/// para filtrar los nodos del árbol.</typeparam>
/// <seealso cref="IQueryService{TDto, TQuery}"/>
public interface ITreeQueryService<TDto, in TQuery> : IQueryService<TDto, TQuery>
    where TDto : new()
    where TQuery : IPage {
    /// <summary>
    /// Obtiene una lista de objetos de tipo <typeparamref name="TDto"/> 
    /// basados en los identificadores de los padres proporcionados.
    /// </summary>
    /// <param name="parentIds">Una cadena que contiene los identificadores de los padres, 
    /// separados por comas.</param>
    /// <returns>Una tarea que representa la operación asincrónica. 
    /// El valor de retorno contiene una lista de objetos de tipo <typeparamref name="TDto"/>.</returns>
    /// <remarks>
    /// Este método es útil para recuperar datos relacionados con un conjunto específico 
    /// de padres, permitiendo filtrar los resultados según los identificadores proporcionados.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto que se devolverá en la lista.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TDto>> GetByParentIds( string parentIds );
}