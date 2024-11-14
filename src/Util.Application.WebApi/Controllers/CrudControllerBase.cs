using Util.Applications.Dtos;
using Util.Applications.Models;
using Util.Applications.Properties;
using Util.Data.Queries;

namespace Util.Applications.Controllers; 

/// <summary>
/// Clase base abstracta para controladores CRUD que maneja operaciones básicas sobre un recurso.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa el recurso.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto utilizado para realizar consultas sobre el recurso.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="CrudControllerBase{TDto, TDto, TDto, TQuery}"/> y proporciona una implementación
/// simplificada para los controladores que no requieren un tipo de entrada y salida diferenciados.
/// </remarks>
public abstract class CrudControllerBase<TDto, TQuery> : CrudControllerBase<TDto, TDto, TDto, TQuery>
    where TDto : IDto, new()
    where TQuery : IPage {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudControllerBase{TDto, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio de CRUD que se utilizará para realizar operaciones sobre los datos.</param>
    protected CrudControllerBase( ICrudService<TDto, TQuery> service )
        : base( service ) {
    }
}

/// <summary>
/// Clase base abstracta para controladores CRUD que proporciona operaciones comunes
/// para la creación, actualización y eliminación de entidades.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos (DTO) que representa la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud de creación de una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud de actualización de una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa los parámetros de consulta para obtener entidades.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="QueryControllerBase{TDto, TQuery}"/> y proporciona métodos
/// para las operaciones de creación, actualización y eliminación, además de las operaciones de consulta.
/// </remarks>
public abstract class CrudControllerBase<TDto, TCreateRequest, TUpdateRequest, TQuery> : QueryControllerBase<TDto, TQuery>
    where TDto : IDto, new()
    where TCreateRequest : IRequest, new()
    where TUpdateRequest : IDto, new()
    where TQuery : IPage {
    private readonly ICrudService<TDto, TCreateRequest, TUpdateRequest, TQuery> _service;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CrudControllerBase{TDto, TCreateRequest, TUpdateRequest, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio de CRUD que se utilizará para las operaciones de datos.</param>
    /// <remarks>
    /// Este constructor llama al constructor base y asigna el servicio proporcionado a la variable de instancia.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto de transferencia de datos.</typeparam>
    /// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud de creación.</typeparam>
    /// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud de actualización.</typeparam>
    /// <typeparam name="TQuery">El tipo de objeto que representa la consulta.</typeparam>
    /// <seealso cref="CrudControllerBase{TDto, TCreateRequest, TUpdateRequest, TQuery}"/>
    protected CrudControllerBase( ICrudService<TDto, TCreateRequest, TUpdateRequest, TQuery> service )
        : base( service ) {
        _service = service;
    }

    /// <summary>
    /// Crea un nuevo recurso de forma asíncrona.
    /// </summary>
    /// <param name="request">Objeto que contiene los datos necesarios para crear el recurso.</param>
    /// <returns>
    /// Un resultado de acción que indica el éxito o fracaso de la operación,
    /// junto con el recurso creado si la operación fue exitosa.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto de solicitud es nulo y, en caso afirmativo,
    /// devuelve un resultado de fallo. Si la solicitud es válida, se ejecutan las
    /// operaciones necesarias antes de la creación del recurso y se procede a
    /// crear el recurso utilizando el servicio correspondiente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="request"/> es nulo.</exception>
    /// <seealso cref="CreateBefore(TCreateRequest)"/>
    /// <seealso cref="_service.CreateAsync(TCreateRequest)"/>
    /// <seealso cref="_service.GetByIdAsync(int)"/>
    protected async Task<IActionResult> CreateAsync(TCreateRequest request) {
        if (request == null)
            return Fail(ApplicationResource.CreateRequestIsEmpty);
        CreateBefore(request);
        var id = await _service.CreateAsync(request);
        var result = await _service.GetByIdAsync(id);
        return Success(result);
    }

    /// <summary>
    /// Método virtual que se ejecuta antes de crear un nuevo recurso.
    /// </summary>
    /// <param name="request">El objeto de solicitud que contiene los datos necesarios para crear el recurso.</param>
    protected virtual void CreateBefore(TCreateRequest request) {
    }

    /// <summary>
    /// Actualiza un recurso de forma asíncrona utilizando el identificador y la solicitud de actualización proporcionados.
    /// </summary>
    /// <param name="id">El identificador del recurso a actualizar.</param>
    /// <param name="request">La solicitud de actualización que contiene los nuevos datos del recurso.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación de actualización.
    /// Si la solicitud de actualización es nula o el identificador está vacío, se devuelve un resultado de fallo.
    /// En caso contrario, se devuelve un resultado de éxito con el recurso actualizado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la solicitud de actualización es nula y si el identificador proporcionado es vacío.
    /// Si el identificador de la solicitud también está vacío, se asigna el identificador del recurso a la solicitud.
    /// Luego, se realiza una actualización del recurso y se obtiene el recurso actualizado para devolverlo como resultado.
    /// </remarks>
    protected async Task<IActionResult> UpdateAsync( string id, TUpdateRequest request ) {
        if( request == null )
            return Fail( ApplicationResource.UpdateRequestIsEmpty );
        if( id.IsEmpty() && request.Id.IsEmpty() )
            return Fail( ApplicationResource.IdIsEmpty );
        if( request.Id.IsEmpty() )
            request.Id = id;
        UpdateBefore( request );
        await _service.UpdateAsync( request );
        var result = await _service.GetByIdAsync( request.Id );
        return Success( result );
    }

    /// <summary>
    /// Método virtual que se ejecuta antes de realizar una actualización.
    /// </summary>
    /// <param name="dto">Objeto de tipo <typeparamref name="TUpdateRequest"/> que contiene los datos necesarios para la actualización.</param>
    /// <remarks>
    /// Este método puede ser sobreescrito en clases derivadas para implementar lógica adicional 
    /// antes de llevar a cabo la actualización de los datos.
    /// </remarks>
    /// <typeparam name="TUpdateRequest">Tipo del objeto que contiene la información de actualización.</typeparam>
    protected virtual void UpdateBefore( TUpdateRequest dto ) {
    }

    /// <summary>
    /// Elimina los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a eliminar, separados por comas.</param>
    /// <returns>Un resultado de acción que indica el éxito de la operación de eliminación.</returns>
    /// <remarks>
    /// Este método llama al servicio para realizar la eliminación de manera asíncrona.
    /// </remarks>
    /// <seealso cref="IActionResult"/>
    protected async Task<IActionResult> DeleteAsync(string ids) 
    { 
        await _service.DeleteAsync(ids); 
        return Success(); 
    }

    /// <summary>
    /// Guarda los elementos proporcionados en el modelo de solicitud.
    /// </summary>
    /// <param name="request">El modelo de solicitud que contiene las listas de elementos a crear, actualizar y eliminar.</param>
    /// <returns>
    /// Un resultado de acción que indica el éxito o fracaso de la operación.
    /// </returns>
    /// <remarks>
    /// Este método procesa las listas de creación, actualización y eliminación contenidas en el modelo de solicitud.
    /// Si el modelo de solicitud es nulo, se devuelve un resultado de fallo con un mensaje de error.
    /// De lo contrario, se deserializan las listas y se pasan al servicio para su procesamiento.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el <paramref name="request"/> es nulo.</exception>
    /// <seealso cref="SaveModel"/>
    /// <seealso cref="_service.SaveAsync(List{TDto}, List{TDto}, List{TDto})"/>
    protected async Task<IActionResult> SaveAsync( SaveModel request ) {
        if ( request == null )
            return Fail( ApplicationResource.RequestIsEmpty );
        var creationList = Util.Helpers.Json.ToObject<List<TDto>>( request.CreationList );
        var updateList = Util.Helpers.Json.ToObject<List<TDto>>( request.UpdateList );
        var deleteList = Util.Helpers.Json.ToObject<List<TDto>>( request.DeleteList );
        await _service.SaveAsync( creationList, updateList, deleteList );
        return Success();
    }
}