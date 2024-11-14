using Util.Domain.Trees;

namespace Util.Domain; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="ITreeRepository"/>.
/// </summary>
public static class ITreeRepositoryExtensions {
    /// <summary>
    /// Actualiza la ruta de una entidad en el repositorio de árbol de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que implementa <see cref="ITreeEntity{TEntity, TKey, TParentId}"/>.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave de la entidad.</typeparam>
    /// <typeparam name="TParentId">El tipo del identificador del padre de la entidad.</typeparam>
    /// <param name="repository">El repositorio de árbol que se utiliza para realizar la actualización.</param>
    /// <param name="entity">La entidad cuya ruta se va a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona de actualización de la ruta.</returns>
    /// <remarks>
    /// Este método utiliza un <see cref="UpdatePathManager{TEntity, TKey, TParentId}"/> para manejar la lógica de actualización de la ruta.
    /// Asegúrese de que la entidad proporcionada esté correctamente configurada antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="ITreeRepository{TEntity, TKey, TParentId}"/>
    /// <seealso cref="UpdatePathManager{TEntity, TKey, TParentId}"/>
    public static async Task UpdatePathAsync<TEntity, TKey, TParentId>( this ITreeRepository<TEntity, TKey, TParentId> repository, TEntity entity )
        where TEntity : class, ITreeEntity<TEntity, TKey, TParentId> {
        var manager = new UpdatePathManager<TEntity, TKey, TParentId>( repository );
        await manager.UpdatePathAsync( entity );
    }

    /// <summary>
    /// Obtiene una lista de identificadores de padres que faltan para una colección de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="ITreeEntity{TEntity, TKey, TParentKey}"/>.</typeparam>
    /// <param name="entities">Una colección de entidades de tipo <typeparamref name="TEntity"/>.</param>
    /// <returns>
    /// Una lista de cadenas que representan los identificadores de padres que faltan.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para <see cref="IEnumerable{T}"/> y se utiliza para identificar
    /// los identificadores de padres que no están presentes en la colección de entidades.
    /// </remarks>
    /// <seealso cref="ITreeEntity{TEntity, TKey, TParentKey}"/>
    public static List<string> GetMissingParentIds<TEntity>( this IEnumerable<TEntity> entities ) where TEntity : class, ITreeEntity<TEntity, Guid, Guid?> {
        return GetMissingParentIds<TEntity, Guid, Guid?>( entities );
    }

    /// <summary>
    /// Obtiene una lista de identificadores de padres que faltan en la colección de entidades.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="ITreeEntity{TEntity, TKey, TParentId}"/>.</typeparam>
    /// <typeparam name="TKey">El tipo de clave que identifica de manera única a la entidad.</typeparam>
    /// <typeparam name="TParentId">El tipo de identificador del padre de la entidad.</typeparam>
    /// <param name="entities">Una colección de entidades de tipo <typeparamref name="TEntity"/>.</param>
    /// <returns>Una lista de cadenas que representan los identificadores de padres que no están presentes en la colección de entidades.</returns>
    /// <remarks>
    /// Este método verifica cada entidad en la colección proporcionada y obtiene los identificadores de padres desde la ruta de cada entidad.
    /// Luego, compara estos identificadores con los identificadores de las entidades en la colección y devuelve aquellos que faltan.
    /// </remarks>
    /// <seealso cref="ITreeEntity{TEntity, TKey, TParentId}"/>
    public static List<string> GetMissingParentIds<TEntity, TKey, TParentId>( this IEnumerable<TEntity> entities ) where TEntity : class, ITreeEntity<TEntity, TKey, TParentId> {
        var result = new List<string>();
        if ( entities == null )
            return result;
        var list = entities.ToList();
        list.ForEach( entity => {
            if ( entity == null )
                return;
            result.AddRange( entity.GetParentIdsFromPath().Select( t => t.SafeString() ) );
        } );
        var ids = list.Select( t => t?.Id.SafeString() );
        return result.Except( ids ).ToList();
    }
}