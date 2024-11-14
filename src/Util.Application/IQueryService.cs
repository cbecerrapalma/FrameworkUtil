using Util.Data;
using Util.Data.Queries;

namespace Util.Applications; 

/// <summary>
/// Interfaz que define un servicio de consulta genérico.
/// </summary>
/// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se devolverá.</typeparam>
/// <typeparam name="TQuery">El tipo de objeto que representa la consulta.</typeparam>
/// <seealso cref="IService"/>
public interface IQueryService<TDto, in TQuery> : IService
    where TDto : new()
    where TQuery : IPage {
    /// <summary>
    /// Obtiene una lista de todos los elementos de tipo <typeparamref name="TDto"/> de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea contiene una lista de objetos de tipo <typeparamref name="TDto"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para recuperar todos los registros de una fuente de datos sin bloquear el hilo de ejecución.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de datos que representa el objeto que se va a recuperar.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TDto>> GetAllAsync();
    /// <summary>
    /// Obtiene un objeto de tipo <typeparamref name="TDto"/> de manera asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador del objeto que se desea obtener.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de retorno contiene el objeto de tipo <typeparamref name="TDto"/> correspondiente al identificador proporcionado.</returns>
    /// <remarks>
    /// Este método permite recuperar un objeto específico de la base de datos o de otra fuente de datos 
    /// utilizando su identificador único. Es importante asegurarse de que el identificador proporcionado 
    /// sea válido y exista en la fuente de datos.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto que se está recuperando.</typeparam>
    /// <seealso cref="GetAllAsync"/>
    /// <seealso cref="DeleteAsync"/>
    Task<TDto> GetByIdAsync( object id );
    /// <summary>
    /// Obtiene una lista de objetos de tipo <typeparamref name="TDto"/> a partir de una cadena de identificadores.
    /// </summary>
    /// <param name="ids">Una cadena que contiene los identificadores separados por comas.</param>
    /// <returns>Una tarea que representa la operación asincrónica. El resultado contiene una lista de objetos de tipo <typeparamref name="TDto"/>.</returns>
    /// <remarks>
    /// Este método es útil para recuperar múltiples entidades basadas en sus identificadores.
    /// Asegúrese de que los identificadores proporcionados sean válidos y estén en el formato correcto.
    /// </remarks>
    /// <typeparam name="TDto">El tipo de objeto que se devolverá en la lista.</typeparam>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TDto>> GetByIdsAsync( string ids );
    /// <summary>
    /// Realiza una consulta asíncrona y devuelve una lista de objetos de tipo <typeparamref name="TDto"/>.
    /// </summary>
    /// <typeparam name="TDto">El tipo de objeto que se devolverá en la lista.</typeparam>
    /// <typeparam name="TQuery">El tipo del parámetro de consulta que se utilizará para filtrar los resultados.</typeparam>
    /// <param name="param">El parámetro de consulta que se utilizará para obtener los datos.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado contiene una lista de objetos de tipo <typeparamref name="TDto"/>.</returns>
    /// <remarks>
    /// Este método es útil para realizar consultas a una base de datos o a un servicio externo,
    /// permitiendo la obtención de datos de manera eficiente y no bloqueante.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<List<TDto>> QueryAsync( TQuery param );
    /// <summary>
    /// Realiza una consulta paginada de datos basados en el parámetro de consulta proporcionado.
    /// </summary>
    /// <typeparam name="TDto">El tipo de objeto de transferencia de datos que se devolverá en la lista paginada.</typeparam>
    /// <param name="param">El objeto que contiene los parámetros de consulta para la paginación.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con un resultado que contiene una lista paginada de objetos de tipo <typeparamref name="TDto"/>.</returns>
    /// <remarks>
    /// Este método permite obtener un subconjunto de datos de manera eficiente, facilitando la navegación a través de grandes conjuntos de datos.
    /// Asegúrese de que el parámetro de consulta esté correctamente configurado para obtener los resultados deseados.
    /// </remarks>
    /// <seealso cref="PageList{TDto}"/>
    Task<PageList<TDto>> PageQueryAsync( TQuery param );
}