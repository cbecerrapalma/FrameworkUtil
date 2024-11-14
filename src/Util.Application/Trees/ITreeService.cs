using Util.Applications.Dtos;
using Util.Data.Trees;

namespace Util.Applications.Trees; 

/// <summary>
/// Interfaz que define los servicios relacionados con árboles.
/// </summary>
/// <typeparam name="TDto">Tipo de objeto de transferencia de datos que representa un nodo en el árbol.</typeparam>
/// <typeparam name="TQuery">Tipo de objeto utilizado para realizar consultas en el árbol.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITreeService{TDto, TDto, TDto, TQuery}"/> 
/// y permite la implementación de servicios de árbol que utilizan un solo tipo de DTO.
/// </remarks>
public interface ITreeService<TDto, in TQuery> : ITreeService<TDto, TDto, TDto, TQuery>
    where TDto : ITreeNode, new()
    where TQuery : ITreeQueryParameter {
}

/// <summary>
/// Interfaz que define los servicios para la gestión de árboles.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que representa un nodo del árbol.</typeparam>
/// <typeparam name="TCreateRequest">El tipo de objeto que representa la solicitud para crear un nuevo nodo en el árbol.</typeparam>
/// <typeparam name="TUpdateRequest">El tipo de objeto que representa la solicitud para actualizar un nodo existente en el árbol.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa las consultas que se pueden realizar sobre el árbol.</typeparam>
/// <seealso cref="ITreeQueryService{TDto, TQuery}"/>
public interface ITreeService<TDto, in TCreateRequest, in TUpdateRequest, in TQuery> : ITreeQueryService<TDto, TQuery>
    where TDto : ITreeNode, new()
    where TCreateRequest : IRequest, new()
    where TUpdateRequest : IDto,new()
    where TQuery : ITreeQueryParameter {
    /// <summary>
    /// Crea un nuevo recurso de forma asíncrona.
    /// </summary>
    /// <param name="request">El objeto que contiene los datos necesarios para crear el recurso.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es una cadena que representa el identificador del recurso creado.</returns>
    /// <remarks>
    /// Este método debe ser llamado en un contexto asíncrono y puede lanzar excepciones si la creación del recurso falla.
    /// </remarks>
    /// <typeparam name="TCreateRequest">El tipo del objeto que contiene los datos para la creación del recurso.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<string> CreateAsync( TCreateRequest request );
    /// <summary>
    /// Actualiza de forma asíncrona un recurso utilizando la solicitud proporcionada.
    /// </summary>
    /// <typeparam name="TUpdateRequest">El tipo de la solicitud de actualización.</typeparam>
    /// <param name="request">La solicitud que contiene los datos necesarios para la actualización.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <remarks>
    /// Este método permite realizar una actualización en el recurso correspondiente
    /// de manera asíncrona, lo que permite que la aplicación continúe ejecutándose
    /// mientras se procesa la solicitud.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task UpdateAsync( TUpdateRequest request );
    /// <summary>
    /// Elimina de forma asíncrona los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a eliminar, separados por comas.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos a la vez, proporcionando sus identificadores en una sola cadena.
    /// Asegúrese de que los identificadores sean válidos y existan en el sistema antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task DeleteAsync( string ids );
    /// <summary>
    /// Habilita de manera asíncrona los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a habilitar, separados por comas.</param>
    /// <returns>Una tarea que representa la operación asíncrona de habilitación. El resultado de la tarea puede indicar el éxito o el fracaso de la operación.</returns>
    /// <remarks>
    /// Este método permite habilitar múltiples elementos en una sola llamada, lo que puede ser útil para optimizar el rendimiento 
    /// al evitar múltiples llamadas a la base de datos o al servicio.
    /// </remarks>
    /// <seealso cref="DisableAsync(string)"/>
    Task EnableAsync( string ids );
    /// <summary>
    /// Desactiva los elementos especificados por sus identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores de los elementos a desactivar, separados por comas.</param>
    /// <returns>Una tarea que representa la operación asincrónica de desactivación.</returns>
    /// <remarks>
    /// Este método permite desactivar múltiples elementos a la vez, proporcionando sus identificadores en una sola cadena.
    /// Asegúrese de que los identificadores sean válidos y existan en el sistema antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="EnableAsync(string)"/>
    Task DisableAsync( string ids );
}