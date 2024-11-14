namespace Util.Domain.Compare; 

/// <summary>
/// Clase que compara listas de entidades basadas en una clave específica.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IKey{TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave que se utiliza para la comparación.</typeparam>
public class ListComparator<TEntity, TKey> where TEntity : IKey<TKey> {
    /// <summary>
    /// Compara dos listas de entidades y determina cuáles han sido creadas, actualizadas o eliminadas.
    /// </summary>
    /// <param name="newList">La nueva lista de entidades que se va a comparar.</param>
    /// <param name="originalList">La lista original de entidades con la que se va a comparar.</param>
    /// <returns>
    /// Un objeto <see cref="ListCompareResult{TEntity, TKey}"/> que contiene las listas de entidades creadas, actualizadas y eliminadas.
    /// </returns>
    /// <remarks>
    /// Este método verifica que ambas listas no sean nulas antes de proceder con la comparación.
    /// Se generan tres listas: una para las entidades creadas, otra para las actualizadas y otra para las eliminadas.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está comparando.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave que identifica de manera única a cada entidad.</typeparam>
    public ListCompareResult<TEntity, TKey> Compare( IEnumerable<TEntity> newList, IEnumerable<TEntity> originalList ) {
        newList.CheckNull( nameof( newList ) );
        originalList.CheckNull( nameof( originalList ) );
        var newEntities = newList.ToList();
        var originalEntities = originalList.ToList();
        var createList = GetCreateList( newEntities, originalEntities );
        var updateList = GetUpdateList( newEntities, originalEntities );
        var deleteList = GetDeleteList( newEntities, originalEntities );
        return new ListCompareResult<TEntity, TKey>( createList, updateList, deleteList );
    }

    /// <summary>
    /// Obtiene una lista de elementos que están en la nueva lista pero no en la lista original.
    /// </summary>
    /// <param name="newList">La lista de nuevos elementos que se va a comparar.</param>
    /// <param name="originalList">La lista original de elementos que se utiliza como referencia.</param>
    /// <returns>Una lista de elementos que están presentes en <paramref name="newList"/> pero no en <paramref name="originalList"/>.</returns>
    /// <remarks>Este método utiliza la función Except para determinar la diferencia entre las dos listas.</remarks>
    /// <typeparam name="TEntity">El tipo de los elementos en las listas.</typeparam>
    private List<TEntity> GetCreateList( List<TEntity> newList, List<TEntity> originalList ) {
        var result = newList.Except( originalList );
        return result.ToList();
    }

    /// <summary>
    /// Obtiene una lista de entidades que están presentes en la nueva lista y también en la lista original.
    /// </summary>
    /// <param name="newList">La lista de entidades nuevas que se va a comparar.</param>
    /// <param name="originalList">La lista de entidades originales con la que se va a comparar.</param>
    /// <returns>
    /// Una lista de entidades que están presentes en ambas listas.
    /// </returns>
    private List<TEntity> GetUpdateList( List<TEntity> newList, List<TEntity> originalList ) {
        return newList.FindAll( entity => originalList.Exists( t => t.Id.Equals( entity.Id ) ) );
    }

    /// <summary>
    /// Obtiene una lista de elementos que están en la lista original pero no en la nueva lista.
    /// </summary>
    /// <param name="newList">La nueva lista de elementos que se comparará con la lista original.</param>
    /// <param name="originalList">La lista original de elementos de la cual se eliminarán los elementos que están en la nueva lista.</param>
    /// <returns>Una lista de elementos que están en la lista original pero no en la nueva lista.</returns>
    /// <remarks>
    /// Este método utiliza el método Except para determinar los elementos que deben ser eliminados.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de los elementos en las listas.</typeparam>
    private List<TEntity> GetDeleteList( List<TEntity> newList, List<TEntity> originalList ) {
        var result = originalList.Except( newList );
        return result.ToList();
    }
}