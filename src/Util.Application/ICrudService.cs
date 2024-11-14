using Util.Applications.Dtos;
using Util.Data.Queries;

namespace Util.Applications; 

/// <summary>
/// Interfaz genérica que define los métodos básicos de un servicio de CRUD (Crear, Leer, Actualizar, Eliminar).
/// </summary>
/// <typeparam name="TDto">Tipo de objeto de transferencia de datos que representa la entidad.</typeparam>
/// <typeparam name="TQuery">Tipo de objeto utilizado para realizar consultas sobre la entidad.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="ICrudService{TDto, TDto, TDto, TQuery}"/> 
/// y permite la implementación de servicios de CRUD con un solo tipo de DTO y un tipo de consulta.
/// </remarks>
public interface ICrudService<TDto, in TQuery> : ICrudService<TDto, TDto, TDto, TQuery>
    where TDto : IDto, new()
    where TQuery : IPage {
}

/// <summary>
/// Interfaz que define un servicio de operaciones CRUD (Crear, Leer, Actualizar, Eliminar).
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa la entidad.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud para crear una nueva entidad.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud para actualizar una entidad existente.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa los criterios de consulta para obtener entidades.</typeparam>
/// <seealso cref="IQueryService{TDto, TQuery}"/>
public interface ICrudService<TDto, in TCreateRequest, in TUpdateRequest, in TQuery> : IQueryService<TDto, TQuery>
    where TDto : IDto, new()
    where TCreateRequest : IRequest, new()
    where TUpdateRequest : IDto, new()
    where TQuery : IPage {
    /// <summary>
    /// Crea un nuevo recurso de forma asíncrona.
    /// </summary>
    /// <param name="request">El objeto que contiene los datos necesarios para crear el recurso.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene una cadena que representa el identificador del recurso creado.</returns>
    /// <remarks>
    /// Este método debe ser llamado cuando se necesite crear un nuevo recurso en el sistema.
    /// Asegúrese de que el objeto <paramref name="request"/> esté correctamente inicializado antes de llamar a este método.
    /// </remarks>
    /// <typeparam name="TCreateRequest">El tipo del objeto que contiene la información necesaria para la creación del recurso.</typeparam>
    /// <seealso cref="UpdateAsync"/>
    /// <seealso cref="DeleteAsync"/>
    Task<string> CreateAsync( TCreateRequest request );
    /// <summary>
    /// Actualiza de manera asíncrona un recurso utilizando la solicitud proporcionada.
    /// </summary>
    /// <param name="request">El objeto de solicitud que contiene los datos necesarios para la actualización.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <remarks>
    /// Este método debe ser implementado para realizar la lógica de actualización específica
    /// del recurso que se está manejando. Asegúrese de manejar adecuadamente las excepciones
    /// que puedan surgir durante el proceso de actualización.
    /// </remarks>
    /// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud de actualización.</typeparam>
    /// <seealso cref="Task"/>
    Task UpdateAsync( TUpdateRequest request );
    /// <summary>
    /// Elimina de manera asíncrona los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a eliminar, separados por comas.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos de forma simultánea. 
    /// Se recomienda validar los identificadores antes de llamar a este método para evitar errores.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task DeleteAsync( string ids );
    /// <summary>
    /// Guarda una lista de objetos de tipo <typeparamref name="TDto"/> en función de las operaciones especificadas.
    /// </summary>
    /// <param name="creationList">Lista de objetos a crear.</param>
    /// <param name="updateList">Lista de objetos a actualizar.</param>
    /// <param name="deleteList">Lista de objetos a eliminar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de objetos de tipo <typeparamref name="TDto"/> que han sido procesados.</returns>
    /// <remarks>
    /// Este método permite realizar operaciones de creación, actualización y eliminación de manera eficiente.
    /// Asegúrese de que las listas proporcionadas no sean nulas y contengan los objetos adecuados para cada operación.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto que se va a manejar en las operaciones de guardado.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TDto>> SaveAsync( List<TDto> creationList, List<TDto> updateList, List<TDto> deleteList );
}