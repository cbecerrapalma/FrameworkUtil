using Util.Applications.Dtos;
using Util.Applications.Properties;
using Util.Data;
using Util.Data.Queries;

namespace Util.Applications.Controllers;

/// <summary>
/// Clase base abstracta para controladores de consultas que manejan operaciones relacionadas con 
/// un tipo específico de DTO (Data Transfer Object) y una consulta específica.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se utilizará en el controlador.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto de consulta que se utilizará para realizar las operaciones.</typeparam>
/// <remarks>
/// Esta clase proporciona una estructura común para los controladores que manejan consultas, 
/// permitiendo la reutilización de código y la implementación de lógica específica de consulta 
/// en clases derivadas.
/// </remarks>
/// <seealso cref="WebApiControllerBase"/>
public abstract class QueryControllerBase<TDto, TQuery> : WebApiControllerBase
    where TQuery : IPage
    where TDto : IDto, new()
{

    #region Campo

    private readonly IQueryService<TDto, TQuery> _service;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="QueryControllerBase{TDto, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio de consulta que se utilizará para realizar operaciones de consulta.</param>
    protected QueryControllerBase(IQueryService<TDto, TQuery> service)
    {
        _service = service;
    }

    #endregion

    #region GetAsync(Obtener una sola entidad.)

    /// <summary>
    /// Obtiene un recurso de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador del recurso que se desea obtener.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación,
    /// que incluye el recurso solicitado si se encuentra disponible.
    /// </returns>
    /// <remarks>
    /// Este método llama a un servicio para obtener el recurso por su identificador.
    /// Si el recurso es encontrado, se devuelve con un estado de éxito.
    /// En caso contrario, se debe manejar el error correspondiente.
    /// </remarks>
    protected async Task<IActionResult> GetAsync(string id)
    {
        var result = await _service.GetByIdAsync(id);
        return Success(result);
    }

    #endregion

    #region QueryAsync(Consulta)

    /// <summary>
    /// Ejecuta una consulta de forma asíncrona utilizando el objeto de consulta proporcionado.
    /// </summary>
    /// <typeparam name="TQuery">El tipo de objeto de consulta que se va a procesar.</typeparam>
    /// <param name="query">El objeto de consulta que contiene los parámetros necesarios para la operación.</param>
    /// <returns>
    /// Un resultado de acción que indica el éxito o fracaso de la operación, junto con los datos resultantes si la consulta fue exitosa.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto de consulta es nulo y, en caso afirmativo, devuelve un error.
    /// Antes de ejecutar la consulta, se puede realizar alguna operación previa mediante el método <see cref="QueryBefore"/>.
    /// Después de obtener la lista de resultados, se puede realizar una operación posterior mediante el método <see cref="QueryAfter"/>.
    /// </remarks>
    protected async Task<IActionResult> QueryAsync(TQuery query)
    {
        if (query == null)
            return Fail(ApplicationResource.QueryIsEmpty);
        QueryBefore(query);
        var list = await _service.QueryAsync(query);
        var result = QueryAfter(list);
        return Success(result);
    }

    /// <summary>
    /// Método que se ejecuta antes de realizar una consulta.
    /// </summary>
    /// <param name="query">La consulta que se va a ejecutar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar lógica adicional
    /// antes de que se ejecute la consulta.
    /// </remarks>
    protected virtual void QueryBefore(TQuery query)
    {
    }

    /// <summary>
    /// Realiza una consulta después de procesar una lista de objetos de tipo TDto.
    /// </summary>
    /// <param name="list">La lista de objetos de tipo TDto que se va a procesar.</param>
    /// <returns>La lista procesada de objetos de tipo TDto.</returns>
    protected virtual dynamic QueryAfter(List<TDto> list)
    {
        return list;
    }

    #endregion

    #region PageQueryAsync(Consulta paginada)

    /// <summary>
    /// Ejecuta una consulta de paginación de manera asíncrona.
    /// </summary>
    /// <typeparam name="TQuery">El tipo de la consulta que se va a ejecutar.</typeparam>
    /// <param name="query">La consulta que se va a procesar.</param>
    /// <returns>
    /// Un resultado de acción que contiene el resultado de la consulta de paginación.
    /// Si la consulta es nula, se devuelve un resultado de fallo.
    /// </returns>
    /// <remarks>
    /// Este método realiza las siguientes operaciones:
    /// 1. Verifica si la consulta es nula y, en caso afirmativo, devuelve un fallo.
    /// 2. Llama al método <see cref="PageQueryBefore"/> para realizar operaciones previas a la consulta.
    /// 3. Ejecuta la consulta de paginación a través del servicio correspondiente.
    /// 4. Llama al método <see cref="PageQueryAfter"/> para procesar el resultado de la consulta.
    /// 5. Devuelve un resultado exitoso con el resultado procesado.
    /// </remarks>
    protected async Task<IActionResult> PageQueryAsync(TQuery query)
    {
        if (query == null)
            return Fail(ApplicationResource.QueryIsEmpty);
        PageQueryBefore(query);
        var pageList = await _service.PageQueryAsync(query);
        var result = PageQueryAfter(pageList);
        return Success(result);
    }

    /// <summary>
    /// Se ejecuta antes de realizar una consulta de página.
    /// </summary>
    /// <param name="query">La consulta que se va a modificar o procesar antes de su ejecución.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para implementar lógica personalizada
    /// antes de que se ejecute la consulta.
    /// </remarks>
    protected virtual void PageQueryBefore(TQuery query)
    {
    }

    /// <summary>
    /// Realiza operaciones adicionales después de la consulta de una lista de páginas.
    /// </summary>
    /// <param name="pageList">La lista de páginas que se ha consultado.</param>
    /// <returns>La lista de páginas posiblemente modificada.</returns>
    protected virtual dynamic PageQueryAfter(PageList<TDto> pageList)
    {
        return pageList;
    }

    #endregion

    #region GetItemsAsync(Obtener lista de elementos.)

    /// <summary>
    /// Obtiene una lista de elementos de forma asíncrona según la consulta proporcionada.
    /// </summary>
    /// <param name="query">La consulta que se utilizará para filtrar los elementos a obtener.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación.
    /// Si la consulta es nula, se devuelve un resultado de fallo con un mensaje correspondiente.
    /// En caso contrario, se devuelve un resultado de éxito con la lista de elementos obtenidos.
    /// </returns>
    /// <remarks>
    /// Este método carga los elementos seleccionados y aplica un filtro a la consulta antes de cargar los elementos
    /// que coinciden con la consulta filtrada. Los elementos se transforman a un formato adecuado antes de ser devueltos.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta utilizada para filtrar los elementos.</typeparam>
    /// <typeparam name="TDto">El tipo de los objetos de datos que se obtendrán.</typeparam>
    protected async Task<IActionResult> GetItemsAsync(TQuery query)
    {
        if (query == null)
            return Fail(ApplicationResource.QueryIsEmpty);
        var result = new List<TDto>();
        await LoadSelectedItems(result);
        query = FilterGetItemsQuery(query);
        await LoadItemsByQuery(result, query);
        return Success(result.Select(dto => ToItem(dto, query)));
    }

    /// <summary>
    /// Carga los elementos seleccionados de manera asíncrona y los agrega a la lista proporcionada.
    /// </summary>
    /// <param name="items">La lista de elementos a la que se agregarán los elementos seleccionados.</param>
    /// <remarks>
    /// Este método obtiene las claves necesarias para cargar los elementos seleccionados.
    /// Si no hay claves disponibles, el método no realiza ninguna acción.
    /// De lo contrario, se llama a un servicio para obtener los elementos por sus identificadores
    /// y se agregan a la lista proporcionada.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de carga de elementos.
    /// </returns>
    protected virtual async Task LoadSelectedItems(List<TDto> items)
    {
        var keys = GetLoadKeys();
        if (keys.IsEmpty())
            return;
        var selectedItems = await _service.GetByIdsAsync(keys);
        items.AddRange(selectedItems);
    }

    /// <summary>
    /// Obtiene las claves de carga desde la consulta de la solicitud actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa las claves de carga extraídas de la consulta,
    /// o una cadena vacía si no se encuentran claves.
    /// </returns>
    protected string GetLoadKeys()
    {
        return Request.Query["load_keys"].SafeString();
    }

    /// <summary>
    /// Filtra y devuelve la consulta de elementos.
    /// </summary>
    /// <param name="query">La consulta que se va a filtrar.</param>
    /// <returns>La consulta filtrada.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar
    /// una lógica de filtrado específica.
    /// </remarks>
    /// <typeparam name="TQuery">El tipo de la consulta que se está filtrando.</typeparam>
    protected virtual TQuery FilterGetItemsQuery(TQuery query)
    {
        return query;
    }

    /// <summary>
    /// Carga elementos en una lista a partir de una consulta especificada.
    /// </summary>
    /// <param name="items">La lista de elementos donde se agregarán los resultados de la consulta.</param>
    /// <param name="query">La consulta que se utilizará para obtener los elementos.</param>
    /// <returns>Una tarea que representa la operación asíncrona de carga de elementos.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas.
    /// Utiliza un servicio para realizar la consulta paginada y agrega los elementos obtenidos a la lista proporcionada.
    /// </remarks>
    protected virtual async Task LoadItemsByQuery(List<TDto> items, TQuery query)
    {
        var pageList = await _service.PageQueryAsync(query);
        items.AddRange(pageList.Data);
    }

    /// <summary>
    /// Convierte un objeto DTO en un objeto Item.
    /// </summary>
    /// <param name="dto">El objeto DTO que se va a convertir.</param>
    /// <param name="query">La consulta asociada al objeto DTO.</param>
    /// <returns>Un objeto Item que representa el DTO proporcionado.</returns>
    protected virtual Item ToItem(TDto dto, TQuery query)
    {
        return ToItem(dto);
    }

    /// <summary>
    /// Convierte un objeto de tipo <typeparamref name="TDto"/> a un objeto de tipo <see cref="Item"/>.
    /// </summary>
    /// <param name="dto">El objeto de tipo <typeparamref name="TDto"/> que se va a convertir.</param>
    /// <returns>Un objeto de tipo <see cref="Item"/> que representa la conversión del objeto <paramref name="dto"/>.</returns>
    /// <exception cref="NotImplementedException">Se lanza cuando el método no ha sido implementado.</exception>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <typeparam name="TDto">El tipo del objeto que se va a convertir.</typeparam>
    protected virtual Item ToItem(TDto dto)
    {
        throw new NotImplementedException(nameof(ToItem));
    }

    #endregion
}