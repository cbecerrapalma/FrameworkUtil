using Util.Data;
using Util.Data.Trees;

namespace Util.Applications.Trees; 

/// <summary>
/// Representa una acción de consulta para una tabla en forma de árbol.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa los nodos del árbol.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que contiene los criterios de consulta.</typeparam>
/// <seealso cref="TreeQueryActionBase{TDto, TQuery}"/>
public class TreeTableQueryAction<TDto, TQuery> : TreeQueryActionBase<TDto, TQuery>
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeTableQueryAction"/>.
    /// </summary>
    /// <param name="service">El servicio de consulta que se utilizará para obtener los datos.</param>
    /// <param name="loadMode">El modo de carga que determina cómo se cargarán los datos.</param>
    /// <param name="loadOperation">La operación de carga que se realizará.</param>
    /// <param name="maxPageSize">El tamaño máximo de página para la carga de datos.</param>
    /// <param name="isFirstLoad">Indica si es la primera carga de datos.</param>
    /// <param name="isExpandAll">Indica si se deben expandir todos los nodos.</param>
    /// <param name="isExpandForRoot">Indica si se debe expandir el nodo raíz.</param>
    /// <param name="queryBefore">Una acción que se ejecutará antes de realizar la consulta.</param>
    /// <param name="processData">Una acción que se ejecutará para procesar los datos obtenidos.</param>
    public TreeTableQueryAction( ITreeQueryService<TDto, TQuery> service, LoadMode loadMode, LoadOperation loadOperation,
        int maxPageSize, bool isFirstLoad, bool isExpandAll, bool isExpandForRoot, 
        Action<TQuery> queryBefore, Action<PageList<TDto>, TQuery> processData ) 
        : base( service, loadMode, loadOperation, maxPageSize, isFirstLoad, isExpandAll,isExpandForRoot,null, queryBefore, processData ) {
    }

    /// <summary>
    /// Convierte una lista de páginas de datos en un resultado dinámico.
    /// </summary>
    /// <param name="data">La lista de páginas que contiene los datos a convertir.</param>
    /// <param name="async">Indica si la operación debe realizarse de manera asíncrona. Por defecto es false.</param>
    /// <param name="allExpand">Indica si se deben expandir todos los elementos. Por defecto es false.</param>
    /// <returns>Un resultado dinámico que representa los datos convertidos.</returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una conversión específica 
    /// de los datos de tipo <typeparamref name="TDto"/> en un formato de resultado adecuado.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de datos que se está convirtiendo.</typeparam>
    /// <seealso cref="PageList{TDto}"/>
    /// <seealso cref="TreeTableResult{TDto}"/>
    protected override dynamic ToResult( PageList<TDto> data, bool async = false, bool allExpand = false ) {
        return data.Convert( new TreeTableResult<TDto>( data.Data, async, allExpand ).GetResult() );
    }
}