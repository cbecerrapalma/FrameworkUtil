using Util.Applications.Properties;
using Util.Data;
using Util.Data.Trees;

namespace Util.Applications.Trees;

/// <summary>
/// Clase base abstracta para acciones de consulta sobre un árbol.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará en la consulta.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto de consulta que se utilizará para realizar la acción.</typeparam>
public abstract class TreeQueryActionBase<TDto, TQuery>
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter
{

    #region Campo

    private readonly ITreeQueryService<TDto, TQuery> _service;
    private readonly LoadMode _loadMode;
    private readonly LoadOperation _loadOperation;
    private readonly int _maxPageSize;
    private readonly bool _isFirstLoad;
    private readonly bool _isExpandAll;
    private readonly bool _isExpandForRootAsync;
    private readonly string _loadKeys;
    private readonly Action<TQuery> _queryBefore;
    private readonly Action<PageList<TDto>, TQuery> _queryAfter;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeQueryActionBase{TDto, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio de consulta de árbol que se utilizará para realizar las operaciones de carga.</param>
    /// <param name="loadMode">El modo de carga que determina cómo se cargan los datos.</param>
    /// <param name="loadOperation">La operación de carga que se va a realizar.</param>
    /// <param name="maxPageSize">El tamaño máximo de página permitido para la carga de datos.</param>
    /// <param name="isFirstLoad">Indica si es la primera carga de datos.</param>
    /// <param name="isExpandAll">Indica si se deben expandir todos los nodos.</param>
    /// <param name="isExpandForRootAsync">Indica si la expansión para la raíz debe realizarse de forma asíncrona.</param>
    /// <param name="loadKeys">Las claves de carga que se utilizarán durante la operación de carga.</param>
    /// <param name="queryBefore">Una acción que se ejecuta antes de realizar la consulta.</param>
    /// <param name="queryAfter">Una acción que se ejecuta después de realizar la consulta.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="service"/> es null.</exception>
    protected TreeQueryActionBase(ITreeQueryService<TDto, TQuery> service, LoadMode loadMode, LoadOperation loadOperation,
        int maxPageSize, bool isFirstLoad, bool isExpandAll, bool isExpandForRootAsync, string loadKeys,
        Action<TQuery> queryBefore, Action<PageList<TDto>, TQuery> queryAfter)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _loadMode = loadMode;
        _loadOperation = loadOperation;
        _maxPageSize = maxPageSize;
        _isFirstLoad = isFirstLoad;
        _isExpandAll = isExpandAll;
        _isExpandForRootAsync = isExpandForRootAsync;
        _loadKeys = loadKeys;
        _queryBefore = queryBefore;
        _queryAfter = queryAfter;
    }

    #endregion

    #region QueryAsync

    /// <summary>
    /// Ejecuta una consulta de manera asíncrona utilizando el objeto de consulta proporcionado.
    /// </summary>
    /// <typeparam name="TQuery">El tipo del objeto de consulta que se va a ejecutar.</typeparam>
    /// <param name="query">El objeto de consulta que contiene los parámetros necesarios para la ejecución.</param>
    /// <returns>
    /// Un objeto dinámico que representa el resultado de la consulta ejecutada.
    /// </returns>
    /// <remarks>
    /// Este método verifica que el objeto de consulta no sea nulo, invoca un evento opcional antes de la ejecución de la consulta,
    /// inicializa los parámetros de la consulta y finalmente obtiene el resultado de la misma de manera asíncrona.
    /// </remarks>
    /// <seealso cref="InitQueryParam(TQuery)"/>
    /// <seealso cref="GetQueryResult(TQuery)"/>
    public async Task<dynamic> QueryAsync(TQuery query)
    {
        query.CheckNull(nameof(query));
        _queryBefore?.Invoke(query);
        InitQueryParam(query);
        return await GetQueryResult(query);
    }

    /// <summary>
    /// Inicializa los parámetros de consulta para el objeto de consulta especificado.
    /// </summary>
    /// <param name="query">El objeto de consulta que se va a inicializar.</param>
    /// <remarks>
    /// Este método establece el orden de la consulta a "SortId" si no se ha especificado ningún orden.
    /// También establece la propiedad Path a null y, si la operación de carga es LoadChildren, 
    /// no realiza más cambios. En caso contrario, establece ParentId a null.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo del objeto de consulta que se está inicializando.</typeparam>
    protected virtual void InitQueryParam(TQuery query)
    {
        if (query.Order.IsEmpty())
            query.Order = "SortId";
        query.Path = null;
        if (_loadOperation == LoadOperation.LoadChildren)
            return;
        query.ParentId = null;
    }

    /// <summary>
    /// Obtiene el resultado de una consulta de forma asíncrona.
    /// </summary>
    /// <param name="query">La consulta que se va a ejecutar.</param>
    /// <returns>Un objeto dinámico que representa el resultado de la consulta.</returns>
    /// <remarks>
    /// Este método determina el tipo de operación de carga a realizar.
    /// Si la operación es <see cref="LoadOperation.LoadChildren"/>, se carga la información de los hijos.
    /// De lo contrario, se ejecuta la consulta general.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se va a procesar.</typeparam>
    protected virtual async Task<dynamic> GetQueryResult(TQuery query)
    {
        if (_loadOperation == LoadOperation.LoadChildren)
            return await LoadChildren(query);
        return await LoadQuery(query);
    }

    #endregion

    #region SyncQuery

    /// <summary>
    /// Realiza una consulta paginada de forma asíncrona utilizando el servicio especificado.
    /// </summary>
    /// <param name="query">La consulta que se utilizará para obtener los datos paginados.</param>
    /// <returns>
    /// Un objeto <see cref="PageList{TDto}"/> que contiene la lista de resultados paginados.
    /// </returns>
    /// <remarks>
    /// Este método establece el tamaño máximo de página antes de realizar la consulta.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se está utilizando.</typeparam>
    /// <typeparam name="TDto">El tipo de datos de transferencia que se espera en la respuesta.</typeparam>
    protected virtual async Task<PageList<TDto>> SyncQuery(TQuery query)
    {
        query.PageSize = _maxPageSize;
        return await _service.PageQueryAsync(query);
    }

    #endregion

    #region AsyncQuery

    /// <summary>
    /// Realiza una consulta asíncrona y devuelve una lista paginada de DTOs.
    /// </summary>
    /// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se devolverá.</typeparam>
    /// <param name="query">La consulta que se utilizará para obtener los datos.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea contiene una lista paginada de objetos de tipo <typeparamref name="TDto"/>.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    /// <seealso cref="PageList{T}"/>
    protected virtual async Task<PageList<TDto>> AsyncQuery(TQuery query)
    {
        return await _service.PageQueryAsync(query);
    }

    #endregion

    #region ToResult

    /// <summary>
    /// Convierte una lista de datos paginados en un resultado dinámico.
    /// </summary>
    /// <param name="data">La lista de datos paginados que se va a convertir.</param>
    /// <param name="async">Indica si la conversión debe realizarse de manera asíncrona. El valor predeterminado es <c>false</c>.</param>
    /// <param name="allExpand">Indica si se deben expandir todos los elementos. El valor predeterminado es <c>false</c>.</param>
    /// <returns>Un resultado dinámico que representa la conversión de la lista de datos.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de los objetos que se encuentran en la lista de datos.</typeparam>
    protected abstract dynamic ToResult(PageList<TDto> data, bool async = false, bool allExpand = false);

    #endregion

    #region LoadChildren

    /// <summary>
    /// Carga de manera asíncrona los elementos hijos basados en la consulta proporcionada.
    /// </summary>
    /// <param name="query">La consulta que contiene los parámetros necesarios para cargar los elementos hijos.</param>
    /// <returns>Un objeto dinámico que representa los elementos hijos cargados.</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando el identificador del padre está vacío.</exception>
    /// <remarks>
    /// Este método verifica si el identificador del padre está vacío y, de ser así, lanza una excepción.
    /// Luego establece la página de la consulta en 1 y decide el modo de carga a utilizar.
    /// Si el modo de carga es <see cref="LoadMode.RootAsync"/>, se utiliza el método <see cref="SyncLoadChildren"/>.
    /// En caso contrario, se utiliza el método <see cref="AsyncLoadChildren"/>.
    /// </remarks>
    protected virtual async Task<dynamic> LoadChildren(TQuery query)
    {
        if (query.ParentId.IsEmpty())
            throw new InvalidOperationException(ApplicationResource.ParentIdIsEmpty);
        query.Page = 1;
        if (_loadMode == LoadMode.RootAsync)
            return await SyncLoadChildren(query);
        return await AsyncLoadChildren(query);
    }

    /// <summary>
    /// Carga de manera asíncrona los elementos hijos de un padre especificado en la consulta.
    /// </summary>
    /// <param name="query">La consulta que contiene el identificador del padre y otros parámetros necesarios para la carga de datos.</param>
    /// <returns>Un objeto dinámico que contiene los resultados de la carga de datos, incluyendo un total y una lista de elementos.</returns>
    /// <remarks>
    /// Este método inicializa la consulta para cargar los elementos hijos, ejecuta la consulta y luego 
    /// elimina el elemento padre de los resultados antes de devolverlos.
    /// </remarks>
    /// <seealso cref="InitSyncLoadChildrenQuery(TQuery)"/>
    /// <seealso cref="SyncQuery(dynamic)"/>
    /// <seealso cref="ToResult(dynamic, bool, bool)"/>
    protected virtual async Task<dynamic> SyncLoadChildren(TQuery query)
    {
        var parentId = query.ParentId.SafeString();
        var queryParam = await InitSyncLoadChildrenQuery(query);
        var data = await SyncQuery(queryParam);
        data.Total -= 1;
        data.Data.RemoveAll(t => t.Id == parentId);
        _queryAfter?.Invoke(data, query);
        return ToResult(data, false, _isExpandForRootAsync);
    }

    /// <summary>
    /// Inicializa una consulta para cargar los hijos de un elemento padre de manera asíncrona.
    /// </summary>
    /// <param name="query">La consulta que se va a inicializar, que contiene el identificador del padre.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TQuery"/> que representa la consulta inicializada con la información del padre.
    /// </returns>
    /// <remarks>
    /// Este método obtiene el elemento padre utilizando su identificador y actualiza la consulta con la ruta y el nivel del padre.
    /// También establece el identificador del padre en null, ya que se está preparando para cargar los hijos.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se está inicializando.</typeparam>
    protected virtual async Task<TQuery> InitSyncLoadChildrenQuery(TQuery query)
    {
        var parent = await _service.GetByIdAsync(query.ParentId);
        query.Path = parent.Path;
        query.Level = null;
        query.ParentId = null;
        return query;
    }

    /// <summary>
    /// Carga de manera asíncrona los elementos secundarios basados en la consulta proporcionada.
    /// </summary>
    /// <param name="query">La consulta que se utilizará para cargar los elementos secundarios.</param>
    /// <returns>
    /// Un objeto dinámico que representa el resultado de la carga de los elementos secundarios.
    /// </returns>
    /// <remarks>
    /// Este método inicializa la consulta para cargar los elementos secundarios, ejecuta la consulta de forma sincrónica,
    /// y luego invoca un posible callback después de la consulta. Finalmente, convierte el resultado en un formato adecuado.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se utilizará para cargar los elementos secundarios.</typeparam>
    /// <seealso cref="InitAsyncLoadChildrenQuery"/>
    /// <seealso cref="SyncQuery"/>
    /// <seealso cref="ToResult"/>
    protected virtual async Task<dynamic> AsyncLoadChildren(TQuery query)
    {
        var queryParam = await InitAsyncLoadChildrenQuery(query);
        var data = await SyncQuery(queryParam);
        _queryAfter?.Invoke(data, query);
        return ToResult(data, true, _isExpandAll);
    }

    /// <summary>
    /// Inicializa la consulta para cargar los hijos de manera asíncrona.
    /// </summary>
    /// <param name="query">La consulta que se va a inicializar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con la consulta inicializada.</returns>
    protected virtual Task<TQuery> InitAsyncLoadChildrenQuery(TQuery query)
    {
        query.Level = null;
        query.Path = null;
        return Task.FromResult(query);
    }

    #endregion

    #region LoadQuery

    /// <summary>
    /// Carga una consulta de manera sincrónica o asincrónica, dependiendo del modo de carga configurado.
    /// </summary>
    /// <param name="query">La consulta a cargar, de tipo <typeparamref name="TQuery"/>.</param>
    /// <returns>Un objeto dinámico que representa el resultado de la consulta cargada.</returns>
    /// <remarks>
    /// Este método determina el modo de carga a utilizar. Si el modo de carga es <c>LoadMode.Sync</c>, 
    /// se ejecutará el método <c>SyncLoadQuery</c>. De lo contrario, se ejecutará el método 
    /// <c>AsyncLoadQuery</c>.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se va a cargar.</typeparam>
    /// <seealso cref="SyncLoadQuery(TQuery)"/>
    /// <seealso cref="AsyncLoadQuery(TQuery)"/>
    protected virtual async Task<dynamic> LoadQuery(TQuery query)
    {
        if (_loadMode == LoadMode.Sync)
            return await SyncLoadQuery(query);
        return await AsyncLoadQuery(query);
    }

    /// <summary>
    /// Carga de manera asíncrona los resultados de una consulta y realiza operaciones adicionales.
    /// </summary>
    /// <param name="query">La consulta que se va a ejecutar.</param>
    /// <returns>Un objeto dinámico que contiene los resultados de la consulta.</returns>
    /// <remarks>
    /// Este método ejecuta la consulta proporcionada, añade los padres que faltan en los datos obtenidos,
    /// y luego invoca un evento opcional para realizar acciones posteriores a la consulta.
    /// Finalmente, convierte los datos en un resultado adecuado.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se va a procesar.</typeparam>
    /// <seealso cref="SyncQuery(TQuery)"/>
    /// <seealso cref="AddMissingParents(dynamic)"/>
    /// <seealso cref="ToResult(dynamic, bool, bool)"/>
    protected virtual async Task<dynamic> SyncLoadQuery(TQuery query)
    {
        var data = await SyncQuery(query);
        await AddMissingParents(data);
        _queryAfter?.Invoke(data, query);
        return ToResult(data, false, _isExpandAll);
    }

    /// <summary>
    /// Agrega los padres que faltan a la lista de datos proporcionada.
    /// </summary>
    /// <param name="data">La lista de datos que contiene los elementos a los que se les pueden agregar padres faltantes.</param>
    /// <remarks>
    /// Este método verifica si hay elementos en la lista que tienen un nivel mayor a 1. 
    /// Si no hay tales elementos, el método termina sin realizar ninguna acción. 
    /// Si hay elementos con nivel mayor a 1, se obtienen los identificadores de los padres que faltan 
    /// y se recuperan los objetos correspondientes a esos identificadores. 
    /// Finalmente, se añaden los objetos recuperados a la lista de datos, asegurando que no haya duplicados.
    /// </remarks>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    protected virtual async Task AddMissingParents(PageList<TDto> data)
    {
        if (data.Data.Any(t => t.Level > 1) == false)
            return;
        var ids = data.Data.GetMissingParentIds();
        var list = await _service.GetByIdsAsync(ids.Join());
        data.Data.AddRange(list.DistinctBy(t => t.Id));
    }

    /// <summary>
    /// Carga de manera asíncrona una consulta basada en el tipo de consulta proporcionado.
    /// </summary>
    /// <param name="query">La consulta que se va a cargar de forma asíncrona.</param>
    /// <returns>
    /// Un objeto dinámico que representa el resultado de la consulta cargada.
    /// </returns>
    /// <remarks>
    /// Si es la primera carga, se invoca a <see cref="AsyncFirstLoadQuery"/>. 
    /// En caso contrario, se establece el tamaño de página y se realiza una consulta asíncrona.
    /// Después de obtener los datos, se añaden los padres que faltan y se ejecuta un 
    /// posible callback definido en <c>_queryAfter</c>.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se está procesando.</typeparam>
    protected virtual async Task<dynamic> AsyncLoadQuery(TQuery query)
    {
        if (_isFirstLoad)
            return await AsyncFirstLoadQuery(query);
        query.PageSize = _maxPageSize;
        var data = await AsyncQuery(query);
        await AddMissingParents(data);
        _queryAfter?.Invoke(data, query);
        return ToResult(data, true, _isExpandAll);
    }

    /// <summary>
    /// Realiza una consulta asíncrona para cargar los datos iniciales basados en el query proporcionado.
    /// </summary>
    /// <param name="query">El objeto de consulta que contiene los parámetros necesarios para la consulta.</param>
    /// <returns>Un objeto dinámico que contiene los resultados de la consulta y los datos cargados.</returns>
    /// <remarks>
    /// Este método establece el nivel de la consulta a 1, ejecuta la consulta asíncrona y, si hay claves de carga disponibles,
    /// obtiene los IDs de los padres y los nodos correspondientes para expandir la estructura de datos.
    /// También permite la ejecución de un callback opcional después de que se obtienen los datos.
    /// </remarks>
    /// <seealso cref="AsyncQuery"/>
    /// <seealso cref="GetLoadParentIds"/>
    /// <seealso cref="GetLoadNodes"/>
    /// <seealso cref="ExpandParentNodes"/>
    /// <seealso cref="ToResult"/>
    protected virtual async Task<dynamic> AsyncFirstLoadQuery(TQuery query)
    {
        query.Level = 1;
        var data = await AsyncQuery(query);
        if (_loadKeys.IsEmpty() == false)
        {
            var parentIds = await GetLoadParentIds(_loadKeys);
            var nodes = await GetLoadNodes(parentIds.Join());
            data.Data.AddRange(nodes);
            ExpandParentNodes(data, parentIds);
        }
        _queryAfter?.Invoke(data, query);
        return ToResult(data, true, _isExpandAll);
    }

    /// <summary>
    /// Obtiene una lista de identificadores de padres a partir de una cadena de claves.
    /// </summary>
    /// <param name="keys">Una cadena que contiene los identificadores de los nodos seleccionados.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es una lista de cadenas que contiene los identificadores de los nodos padres.
    /// </returns>
    /// <remarks>
    /// Este método llama a un servicio para obtener los nodos seleccionados a partir de los identificadores proporcionados y luego extrae los identificadores de los nodos padres de cada nodo.
    /// </remarks>
    protected virtual async Task<List<string>> GetLoadParentIds(string keys)
    {
        var result = new List<string>();
        var selectedNodes = await _service.GetByIdsAsync(keys);
        selectedNodes.ForEach(t => result.AddRange(t.GetParentIdsFromPath(false)));
        return result;
    }

    /// <summary>
    /// Obtiene una lista de nodos cargados a partir de los identificadores de padre especificados.
    /// </summary>
    /// <param name="parentIds">Una cadena que contiene los identificadores de los nodos padre.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, que contiene una lista de objetos de tipo <typeparamref name="TDto"/>.
    /// </returns>
    /// <typeparam name="TDto">El tipo de objeto que representa el DTO (Data Transfer Object) que se devolverá en la lista.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <seealso cref="GetByParentIds(string)"/>
    protected virtual async Task<List<TDto>> GetLoadNodes(string parentIds)
    {
        return await _service.GetByParentIds(parentIds);
    }

    /// <summary>
    /// Expande los nodos padre en una lista de páginas.
    /// </summary>
    /// <param name="data">La lista de páginas que contiene los nodos a expandir.</param>
    /// <param name="parentIds">Una lista de identificadores de los nodos padre que se deben expandir.</param>
    /// <remarks>
    /// Este método busca en la lista de nodos aquellos que coinciden con los identificadores proporcionados
    /// y establece su propiedad <c>Expanded</c> en <c>true</c>.
    /// </remarks>
    protected virtual void ExpandParentNodes(PageList<TDto> data, List<string> parentIds)
    {
        var nodes = data.Data.FindAll(node => parentIds.Any(id => node.Id == id));
        nodes.ForEach(t => t.Expanded = true);
    }

    #endregion
}