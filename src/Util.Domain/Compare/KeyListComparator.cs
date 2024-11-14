namespace Util.Domain.Compare; 

/// <summary>
/// Clase que compara listas de claves de tipo genérico <typeparamref name="TKey"/>.
/// </summary>
/// <remarks>
/// Esta clase permite realizar comparaciones entre dos listas de claves,
/// facilitando la identificación de elementos comunes, únicos y diferencias
/// entre las listas.
/// </remarks>
/// <typeparam name="TKey">El tipo de las claves que se van a comparar.</typeparam>
public class KeyListComparator<TKey> {
    /// <summary>
    /// Compara dos listas de claves y determina los elementos que se deben crear, actualizar o eliminar.
    /// </summary>
    /// <param name="newList">La nueva lista de claves que se va a comparar.</param>
    /// <param name="originalList">La lista original de claves con la que se va a comparar.</param>
    /// <returns>
    /// Un objeto <see cref="KeyListCompareResult{TKey}"/> que contiene las listas de claves que se deben crear, actualizar y eliminar.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="newList"/> o <paramref name="originalList"/> son nulos.</exception>
    /// <remarks>
    /// Este método realiza una comparación entre dos listas de claves y devuelve un resultado que indica qué elementos deben ser creados, actualizados o eliminados.
    /// </remarks>
    public KeyListCompareResult<TKey> Compare( IEnumerable<TKey> newList, IEnumerable<TKey> originalList ) {
        newList.CheckNull( nameof(newList) );
        originalList.CheckNull( nameof( originalList ) );
        var newEntities = newList.ToList();
        var originalEntities = originalList.ToList();
        var createList = GetCreateList( newEntities, originalEntities );
        var updateList = GetUpdateList( newEntities, originalEntities );
        var deleteList = GetDeleteList( newEntities, originalEntities );
        return new KeyListCompareResult<TKey>( createList, updateList, deleteList );
    }

    /// <summary>
    /// Obtiene una lista de elementos que están en la nueva lista pero no en la lista original.
    /// </summary>
    /// <param name="newList">La lista de elementos nuevos que se va a comparar.</param>
    /// <param name="originalList">La lista original contra la cual se comparan los elementos nuevos.</param>
    /// <returns>Una lista que contiene los elementos que están en <paramref name="newList"/> pero no en <paramref name="originalList"/>.</returns>
    /// <remarks>Este método utiliza la función Except para determinar los elementos únicos de la nueva lista.</remarks>
    /// <typeparam name="TKey">El tipo de los elementos en las listas.</typeparam>
    private List<TKey> GetCreateList( List<TKey> newList, List<TKey> originalList ) {
        var result = newList.Except( originalList );
        return result.ToList();
    }

    /// <summary>
    /// Obtiene una lista de elementos que están presentes en ambas listas proporcionadas.
    /// </summary>
    /// <param name="newList">La lista de nuevos elementos que se va a comparar.</param>
    /// <param name="originalList">La lista original con la que se va a comparar.</param>
    /// <returns>Una lista de elementos que están presentes en ambas listas.</returns>
    /// <typeparam name="TKey">El tipo de los elementos en las listas.</typeparam>
    private List<TKey> GetUpdateList( List<TKey> newList, List<TKey> originalList ) {
        return newList.FindAll( id => originalList.Exists( t => t.Equals( id ) ) );
    }

    /// <summary>
    /// Obtiene una lista de elementos que están presentes en la lista original pero no en la nueva lista.
    /// </summary>
    /// <param name="newList">La nueva lista que se comparará con la lista original.</param>
    /// <param name="originalList">La lista original de la cual se eliminarán los elementos que también están en la nueva lista.</param>
    /// <returns>Una lista de elementos que están en la lista original pero no en la nueva lista.</returns>
    /// <remarks>
    /// Este método utiliza el método Except para determinar los elementos que deben ser eliminados,
    /// devolviendo una lista que contiene solo aquellos elementos que no están en la nueva lista.
    /// </remarks>
    private List<TKey> GetDeleteList( List<TKey> newList, List<TKey> originalList ) {
        var result = originalList.Except( newList );
        return result.ToList();
    }
}