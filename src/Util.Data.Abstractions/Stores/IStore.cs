using Util.Domain;

namespace Util.Data.Stores; 

/// <summary>
/// Interfaz genérica que representa un almacén de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se almacenará.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IStore{TEntity, TKey}"/> y <see cref="IQueryStore{TEntity}"/>.
/// Proporciona métodos para el almacenamiento y la consulta de entidades del tipo especificado.
/// </remarks>
public interface IStore<TEntity> : IStore<TEntity, Guid>, IQueryStore<TEntity>
    where TEntity : class, IKey<Guid> {
}

/// <summary>
/// Interfaz que define un almacén de entidades genéricas.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se almacenará. Debe ser una clase que implemente <see cref="IKey{TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave que identifica de manera única a cada entidad.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IQueryStore{TEntity, TKey}"/> y proporciona métodos para almacenar entidades.
/// </remarks>
public interface IStore<TEntity, in TKey> : IQueryStore<TEntity, TKey> where TEntity : class, IKey<TKey> {
    /// <summary>
    /// Agrega una entidad de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a agregar.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite agregar una nueva entidad a la base de datos de forma asíncrona.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones no deseadas.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a agregar.</typeparam>
    /// <seealso cref="RemoveAsync(TEntity, CancellationToken)"/>
    /// <seealso cref="UpdateAsync(TEntity, CancellationToken)"/>
    Task AddAsync( TEntity entity, CancellationToken cancellationToken = default );
    /// <summary>
    /// Agrega una colección de entidades de forma asíncrona.
    /// </summary>
    /// <param name="entities">La colección de entidades que se van a agregar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de agregar las entidades.</returns>
    /// <remarks>
    /// Este método permite agregar múltiples entidades a la base de datos de manera eficiente.
    /// Asegúrese de que las entidades no existan previamente en la base de datos para evitar excepciones.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="entities"/> es null.</exception>
    /// <seealso cref="RemoveAsync(IEnumerable{TEntity}, CancellationToken)"/>
    /// <seealso cref="UpdateAsync(IEnumerable{TEntity}, CancellationToken)"/>
    Task AddAsync( IEnumerable<TEntity> entities, CancellationToken cancellationToken = default );
    /// <summary>
    /// Actualiza una entidad de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización.</returns>
    /// <remarks>
    /// Este método permite actualizar una entidad existente en la base de datos. 
    /// Asegúrese de que la entidad proporcionada contenga los datos actualizados y que 
    /// esté correctamente configurada para la operación de actualización.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a actualizar.</typeparam>
    /// <seealso cref="Task"/>
    Task UpdateAsync( TEntity entity, CancellationToken cancellationToken = default );
    /// <summary>
    /// Actualiza un conjunto de entidades de tipo <typeparamref name="TEntity"/> de manera asíncrona.
    /// </summary>
    /// <param name="entities">Una colección de entidades que se van a actualizar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación de actualización asíncrona.</returns>
    /// <remarks>
    /// Este método permite realizar actualizaciones en múltiples entidades de forma eficiente.
    /// Asegúrese de que las entidades proporcionadas estén en un estado válido antes de llamar a este método.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de las entidades que se están actualizando.</typeparam>
    /// <seealso cref="IEnumerable{T}"/>
    Task UpdateAsync( IEnumerable<TEntity> entities, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <param name="id">El identificador del elemento que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método permite eliminar un elemento de manera eficiente sin bloquear el hilo de ejecución.
    /// Asegúrese de que el identificador proporcionado sea válido y que el elemento exista antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveAsync( object id, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina una entidad de forma asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar una entidad de la base de datos de manera asíncrona. 
    /// Se recomienda utilizar el <paramref name="cancellationToken"/> para poder cancelar la operación si es necesario.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="entity"/> es null.</exception>
    /// <seealso cref="AddAsync(TEntity, CancellationToken)"/>
    /// <seealso cref="UpdateAsync(TEntity, CancellationToken)"/>
    Task RemoveAsync( TEntity entity, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina de manera asíncrona un conjunto de elementos identificados por sus claves.
    /// </summary>
    /// <param name="ids">Una colección de claves que identifican los elementos a eliminar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples elementos de manera eficiente. 
    /// Se recomienda utilizar el <paramref name="cancellationToken"/> para permitir la cancelación de la operación si es necesario.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="ids"/> es nulo.</exception>
    /// <seealso cref="AddAsync"/>
    /// <seealso cref="UpdateAsync"/>
    Task RemoveAsync( IEnumerable<TKey> ids, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina una colección de entidades de forma asíncrona.
    /// </summary>
    /// <param name="entities">La colección de entidades que se van a eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    /// <remarks>
    /// Este método permite eliminar múltiples entidades de la base de datos. 
    /// Se recomienda utilizar el token de cancelación para manejar escenarios donde 
    /// la operación de eliminación pueda ser prolongada o si se requiere abortar la operación.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task RemoveAsync( IEnumerable<TEntity> entities, CancellationToken cancellationToken = default );
}