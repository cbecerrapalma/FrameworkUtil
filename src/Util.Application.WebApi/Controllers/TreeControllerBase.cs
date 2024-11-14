using Util.Applications.Trees;
using Util.Data.Trees;
using Util.Applications.Properties;
using Util.Applications.Dtos;

namespace Util.Applications.Controllers;

/// <summary>
/// Clase base abstracta para controladores de árbol que manejan operaciones comunes
/// para entidades de tipo <typeparamref name="TDto"/> y consultas de tipo <typeparamref name="TQuery"/>.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa las entidades del árbol.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto utilizado para realizar consultas sobre las entidades del árbol.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación base para controladores que gestionan estructuras de árbol,
/// permitiendo la reutilización de código y la consistencia en el manejo de datos.
/// </remarks>
public abstract class TreeControllerBase<TDto, TQuery> : TreeControllerBase<TDto, TDto, TDto, TQuery>
    where TDto : class, ITreeNode, new()
    where TQuery : class, ITreeQueryParameter
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeControllerBase"/>.
    /// </summary>
    /// <param name="service">El servicio de árbol que se utilizará para las operaciones de datos.</param>
    protected TreeControllerBase(ITreeService<TDto, TQuery> service) : base(service) { }
}

/// <summary>
/// Clase base abstracta para controladores de árbol que manejan operaciones de creación y actualización.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos (DTO) utilizado para representar los nodos del árbol.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud para crear un nuevo nodo en el árbol.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud para actualizar un nodo existente en el árbol.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto utilizado para realizar consultas sobre el árbol.</typeparam>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para manejar operaciones de creación y actualización
/// de nodos en un árbol, además de heredar las capacidades de consulta de la clase base <see cref="TreeQueryControllerBase{TDto, TQuery}"/>.
/// </remarks>
public abstract class TreeControllerBase<TDto, TCreateRequest, TUpdateRequest, TQuery> : TreeQueryControllerBase<TDto, TQuery>
    where TDto : class, ITreeNode, new()
    where TCreateRequest : IRequest, new()
    where TUpdateRequest : IDto, new()
    where TQuery : class, ITreeQueryParameter
{

    #region Campo

    private readonly ITreeService<TDto, TCreateRequest, TUpdateRequest, TQuery> _service;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeControllerBase{TDto, TCreateRequest, TUpdateRequest, TQuery}"/>.
    /// </summary>
    /// <param name="service">El servicio que maneja las operaciones de árbol.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="service"/> es <c>null</c>.</exception>
    protected TreeControllerBase(ITreeService<TDto, TCreateRequest, TUpdateRequest, TQuery> service) : base(service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    #endregion

    #region CreateAsync(Crear)

    /// <summary>
    /// Crea un nuevo recurso de forma asíncrona utilizando la solicitud proporcionada.
    /// </summary>
    /// <param name="request">La solicitud de creación que contiene los datos necesarios para crear el recurso.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación. 
    /// Si la solicitud es nula, se devuelve un fallo con un mensaje de error.
    /// En caso contrario, se devuelve el resultado de la creación del recurso.
    /// </returns>
    /// <remarks>
    /// Este método realiza las siguientes acciones:
    /// <list type="bullet">
    /// <item>Verifica si la solicitud es nula y devuelve un fallo si es el caso.</item>
    /// <item>Ejecuta la lógica previa a la creación mediante el método <see cref="CreateBefore(TCreateRequest)"/>.</item>
    /// <item>Crea el recurso llamando al servicio correspondiente.</item>
    /// <item>Recupera el recurso recién creado utilizando su identificador.</item>
    /// <item>Ejecuta la lógica posterior a la creación y devuelve el resultado exitoso.</item>
    /// </list>
    /// </remarks>
    /// <typeparam name="TCreateRequest">El tipo de la solicitud de creación que se está procesando.</typeparam>
    protected async Task<IActionResult> CreateAsync(TCreateRequest request)
    {
        if (request == null)
            return Fail(ApplicationResource.CreateRequestIsEmpty);
        CreateBefore(request);
        var id = await _service.CreateAsync(request);
        var result = await _service.GetByIdAsync(id);
        return Success(CreateAfter(result));
    }

    /// <summary>
    /// Método virtual que se ejecuta antes de crear un nuevo recurso.
    /// </summary>
    /// <param name="request">El objeto de solicitud que contiene los datos necesarios para crear el recurso.</param>
    protected virtual void CreateBefore(TCreateRequest request)
    {
    }

    /// <summary>
    /// Crea un objeto después de procesar el resultado de tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <param name="result">El resultado de tipo <typeparamref name="TDto"/> que se va a procesar.</param>
    /// <returns>Un objeto que representa el resultado procesado.</returns>
    /// <typeparam name="TDto">El tipo de datos del resultado que se está procesando.</typeparam>
    protected virtual object CreateAfter(TDto result)
    {
        return result;
    }

    #endregion

    #region UpdateAsync(Modificar)

    /// <summary>
    /// Actualiza un recurso de acuerdo a la solicitud proporcionada.
    /// </summary>
    /// <param name="id">El identificador del recurso a actualizar.</param>
    /// <param name="request">El objeto de solicitud que contiene los datos de actualización.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación de actualización.
    /// Si la solicitud está vacía o el identificador es vacío, se devuelve un resultado de fallo.
    /// </returns>
    /// <remarks>
    /// Este método verifica que la solicitud no sea nula y que contenga un identificador válido.
    /// Si el identificador de la solicitud está vacío, se asigna el identificador proporcionado como parámetro.
    /// Se llama a <see cref="UpdateBefore"/> antes de realizar la actualización y a <see cref="UpdateAfter"/> después de obtener el resultado.
    /// </remarks>
    /// <seealso cref="UpdateBefore"/>
    /// <seealso cref="UpdateAfter"/>
    /// <seealso cref="_service.UpdateAsync"/>
    /// <seealso cref="_service.GetByIdAsync"/>
    protected async Task<IActionResult> UpdateAsync(string id, TUpdateRequest request)
    {
        if (request == null)
            return Fail(ApplicationResource.UpdateRequestIsEmpty);
        if (id.IsEmpty() && request.Id.IsEmpty())
            return Fail(ApplicationResource.IdIsEmpty);
        if (request.Id.IsEmpty())
            request.Id = id;
        UpdateBefore(request);
        await _service.UpdateAsync(request);
        var result = await _service.GetByIdAsync(request.Id);
        return Success(UpdateAfter(result));
    }

    /// <summary>
    /// Método protegido y virtual que se ejecuta antes de actualizar un objeto.
    /// </summary>
    /// <param name="dto">El objeto de solicitud de actualización que contiene los datos necesarios para la operación.</param>
    protected virtual void UpdateBefore(TUpdateRequest dto)
    {
    }

    /// <summary>
    /// Actualiza el objeto después de realizar una operación.
    /// </summary>
    /// <param name="result">El objeto de tipo <typeparamref name="TDto"/> que se va a actualizar.</param>
    /// <returns>El objeto actualizado de tipo <typeparamref name="TDto"/>.</returns>
    /// <typeparam name="TDto">El tipo de objeto que se está actualizando.</typeparam>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una lógica de actualización específica.
    /// </remarks>
    protected virtual object UpdateAfter(TDto result)
    {
        return result;
    }

    #endregion

    #region DeleteAsync(Eliminar)

    /// <summary>
    /// Elimina los elementos especificados de forma asíncrona.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a eliminar, separados por comas.</param>
    /// <returns>
    /// Un resultado de acción que indica el éxito de la operación de eliminación.
    /// </returns>
    /// <remarks>
    /// Este método llama al servicio para realizar la eliminación de los elementos
    /// y devuelve un resultado de éxito si la operación se completa sin errores.
    /// </remarks>
    protected async Task<IActionResult> DeleteAsync(string ids)
    {
        await _service.DeleteAsync(ids);
        return Success();
    }

    #endregion

    #region EnableAsync(Habilitar)

    /// <summary>
    /// Habilita los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a habilitar, separados por comas.</param>
    /// <returns>
    /// Un resultado de acción que indica el éxito de la operación.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="EnableBefore(string)"/> para realizar acciones previas 
    /// a la habilitación y luego invoca el método <see cref="_service.EnableAsync(string)"/> 
    /// para llevar a cabo la habilitación de los elementos.
    /// </remarks>
    protected async Task<IActionResult> EnableAsync(string ids)
    {
        EnableBefore(ids);
        await _service.EnableAsync(ids);
        return Success();
    }

    /// <summary>
    /// Habilita ciertas funcionalidades o configuraciones antes de realizar una acción específica.
    /// </summary>
    /// <param name="ids">Una cadena que representa los identificadores de los elementos que se deben habilitar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// </remarks>
    protected virtual void EnableBefore(string ids)
    {
    }

    #endregion

    #region DisableAsync(Deshabilitar)

    /// <summary>
    /// Desactiva los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a desactivar, separados por comas.</param>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado de la operación de desactivación.
    /// </returns>
    /// <remarks>
    /// Este método realiza una llamada a <see cref="_service.DisableAsync"/> para desactivar los elementos
    /// y llama a <see cref="DisableBefore"/> para realizar cualquier acción previa necesaria antes de la desactivación.
    /// </remarks>
    protected async Task<IActionResult> DisableAsync(string ids)
    {
        DisableBefore(ids);
        await _service.DisableAsync(ids);
        return Success();
    }

    /// <summary>
    /// Desactiva elementos antes de una acción específica utilizando sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a desactivar, separados por comas.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación específica.
    /// Asegúrese de manejar adecuadamente los identificadores proporcionados para evitar errores en la desactivación.
    /// </remarks>
    protected virtual void DisableBefore(string ids)
    {
    }

    #endregion
}