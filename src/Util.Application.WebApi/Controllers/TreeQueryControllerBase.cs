using Util.Applications.Trees;
using Util.Data.Trees;
using Util.Data;

namespace Util.Applications.Controllers;

/// <summary>
/// Clase base abstracta para controladores que manejan consultas de árboles.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará.</typeparam>
/// <typeparam name="TQuery">El tipo de consulta que se utilizará para obtener datos.</typeparam>
/// <remarks>
/// Esta clase proporciona una estructura básica para implementar controladores que 
/// gestionan operaciones relacionadas con árboles, permitiendo la consulta y 
/// manipulación de datos en un formato estructurado.
/// </remarks>
public abstract class TreeQueryControllerBase<TDto, TQuery> : WebApiControllerBase
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter
{

    #region Campo

    private readonly ITreeQueryService<TDto, TQuery> _service;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeQueryControllerBase{TDto, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio de consulta de árbol que se utilizará para las operaciones.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando el parámetro <paramref name="service"/> es nulo.</exception>
    protected TreeQueryControllerBase(ITreeQueryService<TDto, TQuery> service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    #endregion

    #region GetMaxPageSize(Obtener el tamaño máximo de paginación.)

    /// <summary>
    /// Obtiene el tamaño máximo de página permitido.
    /// </summary>
    /// <returns>
    /// El tamaño máximo de página, que en este caso es 999.
    /// </returns>
    protected virtual int GetMaxPageSize()
    {
        return 999;
    }

    #endregion

    #region GetLoadMode(Obtener modo de carga.)

    /// <summary>
    /// Obtiene el modo de carga a partir de los parámetros de la solicitud.
    /// </summary>
    /// <returns>
    /// Devuelve un valor de <see cref="LoadMode"/> que representa el modo de carga.
    /// Si no se puede determinar el modo de carga, se devuelve <see cref="LoadMode.Sync"/> por defecto.
    /// </returns>
    /// <remarks>
    /// Este método intenta analizar el parámetro "loadMode" de la consulta de la solicitud.
    /// Si el valor es válido y se puede convertir a <see cref="LoadMode"/>, se devuelve ese valor.
    /// En caso contrario, se retorna el modo de carga por defecto.
    /// </remarks>
    protected virtual LoadMode GetLoadMode()
    {
        var loadMode = Request.Query["loadMode"].SafeString();
        var result = Util.Helpers.Enum.Parse<LoadMode?>(loadMode);
        if (result != null)
            return result.Value;
        return LoadMode.Sync;
    }

    #endregion

    #region IsFirstLoad(¿Es la primera vez que se carga?)

    /// <summary>
    /// Determina si es la primera carga de la página basándose en el parámetro de consulta "is_search".
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el parámetro "is_search" es igual a "false"; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected virtual bool IsFirstLoad()
    {
        var isSearch = Request.Query["is_search"].SafeString();
        if (isSearch == "false")
            return true;
        return false;
    }

    #endregion

    #region IsExpandAll(¿Expandir todos los nodos?)

    /// <summary>
    /// Determina si la opción de expandir todo está activada a partir de los parámetros de la solicitud.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la opción de expandir todo está activada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected virtual bool IsExpandAll()
    {
        var isExpandAll = Request.Query["is_expand_all"].SafeString();
        if (isExpandAll == "true")
            return true;
        return false;
    }

    #endregion

    #region IsExpandForRootAsync(Si el modo de carga asincrónica del nodo raíz expande los nodos secundarios)

    /// <summary>
    /// Determina si la expansión está habilitada para la raíz de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la expansión está habilitada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica el valor del parámetro de consulta "is_expand_for_root_async".
    /// Si el valor es "false", se considera que la expansión no está habilitada.
    /// En cualquier otro caso, se considera que la expansión está habilitada.
    /// </remarks>
    protected virtual bool IsExpandForRootAsync()
    {
        var isExpand = Request.Query["is_expand_for_root_async"].SafeString();
        if (isExpand == "false")
            return false;
        return true;
    }

    #endregion

    #region GetLoadKeys(Obtener la lista de identificadores que necesitan ser cargados.)

    /// <summary>
    /// Obtiene las claves de carga a partir de la consulta.
    /// </summary>
    /// <param name="query">La consulta que se está procesando.</param>
    /// <returns>Una cadena que representa las claves de carga extraídas de la consulta.</returns>
    protected virtual string GetLoadKeys(TQuery query)
    {
        return Request.Query["load_keys"].SafeString();
    }

    #endregion

    #region GetAsync(Obtener una sola entidad)

    /// <summary>
    /// Obtiene un recurso de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador del recurso que se desea obtener.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación.
    /// Si el recurso se encuentra, se devuelve un resultado exitoso con el recurso.
    /// De lo contrario, se devuelve un resultado de error.
    /// </returns>
    /// <remarks>
    /// Este método llama a un servicio para obtener el recurso por su identificador.
    /// Se espera que el servicio maneje la lógica de acceso a datos y la validación.
    /// </remarks>
    protected async Task<IActionResult> GetAsync(string id)
    {
        var result = await _service.GetByIdAsync(id);
        return Success(result);
    }

    #endregion

    #region QueryAsync(Consulta de tabla en forma de árbol.)

    /// <summary>
    /// Ejecuta una consulta de forma asíncrona utilizando el objeto de consulta proporcionado.
    /// </summary>
    /// <param name="query">El objeto de consulta que contiene los parámetros necesarios para realizar la consulta.</param>
    /// <returns>
    /// Un resultado de acción que contiene los datos consultados, encapsulados en un objeto de tipo <see cref="IActionResult"/>.
    /// </returns>
    /// <remarks>
    /// Este método crea una instancia de <see cref="TreeTableQueryAction{TDto, TQuery}"/> para manejar la lógica de la consulta.
    /// Se configura con varios parámetros como el modo de carga, la operación a realizar, el tamaño máximo de la página,
    /// y opciones de expansión. Luego, se ejecuta la consulta y se devuelve el resultado.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo del objeto de consulta que se utilizará para la operación.</typeparam>
    /// <typeparam name="TDto">El tipo de objeto de datos que se devolverá como resultado de la consulta.</typeparam>
    /// <seealso cref="TreeTableQueryAction{TDto, TQuery}"/>
    protected async Task<IActionResult> QueryAsync(TQuery query)
    {
        var action = new TreeTableQueryAction<TDto, TQuery>(_service, GetLoadMode(), GetOperation(query),
            GetMaxPageSize(), IsFirstLoad(), IsExpandAll(), IsExpandForRootAsync(), QueryBefore, QueryAfter);
        var result = await action.QueryAsync(query);
        return Success(result);
    }

    /// <summary>
    /// Obtiene la operación de carga correspondiente a la consulta especificada.
    /// </summary>
    /// <param name="query">La consulta que se va a evaluar para determinar la operación de carga.</param>
    /// <returns>
    /// Un valor de <see cref="LoadOperation"/> que indica la operación de carga a realizar.
    /// Si el <paramref name="query"/> no tiene un Id de padre, se devuelve <see cref="LoadOperation.Query"/>.
    /// De lo contrario, se devuelve <see cref="LoadOperation.LoadChildren"/>.
    /// </returns>
    protected virtual LoadOperation GetOperation(TQuery query)
    {
        if (query.ParentId.IsEmpty())
            return LoadOperation.Query;
        return LoadOperation.LoadChildren;
    }

    /// <summary>
    /// Método que se invoca antes de realizar una consulta.
    /// </summary>
    /// <param name="query">El objeto de consulta que se va a procesar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas 
    /// para proporcionar lógica adicional antes de que se ejecute la consulta.
    /// </remarks>
    protected virtual void QueryBefore(TQuery query)
    {
    }

    /// <summary>
    /// Se ejecuta después de realizar una consulta sobre una lista de datos.
    /// </summary>
    /// <param name="data">La lista de datos de tipo <typeparamref name="TDto"/> que se ha consultado.</param>
    /// <param name="query">El objeto de consulta de tipo <typeparamref name="TQuery"/> que se utilizó para realizar la consulta.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar lógica adicional
    /// después de que se haya realizado la consulta.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de datos que representa el objeto de transferencia de datos.</typeparam>
    /// <typeparam name="TQuery">El tipo de datos que representa el objeto de consulta.</typeparam>
    protected virtual void QueryAfter(PageList<TDto> data, TQuery query) { }

    #endregion
}