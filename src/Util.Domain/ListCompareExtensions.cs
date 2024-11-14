using Util.Domain.Compare;

namespace Util.Domain; 

/// <summary>
/// Proporciona métodos de extensión para comparar listas.
/// </summary>
public static class ListCompareExtensions {
    /// <summary>
    /// Compara dos listas de entidades del mismo tipo y devuelve el resultado de la comparación.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IKey{Guid}"/>.</typeparam>
    /// <param name="newList">La nueva lista de entidades que se va a comparar.</param>
    /// <param name="originalList">La lista original de entidades con la que se va a comparar.</param>
    /// <returns>Un objeto de tipo <see cref="ListCompareResult{TEntity, Guid}"/> que contiene el resultado de la comparación.</returns>
    /// <seealso cref="IKey{Guid}"/>
    /// <seealso cref="ListCompareResult{TEntity, Guid}"/>
    public static ListCompareResult<TEntity, Guid> Compare<TEntity>( this IEnumerable<TEntity> newList, IEnumerable<TEntity> originalList )
        where TEntity : IKey<Guid> {
        return Compare<TEntity, Guid>( newList, originalList );
    }

    /// <summary>
    /// Compara dos listas de entidades para determinar las diferencias entre ellas.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IKey{TKey}"/>.</typeparam>
    /// <typeparam name="TKey">El tipo de la clave que identifica de manera única a cada entidad.</typeparam>
    /// <param name="newList">La nueva lista de entidades que se va a comparar.</param>
    /// <param name="originalList">La lista original de entidades con la que se va a comparar.</param>
    /// <returns>
    /// Un objeto <see cref="ListCompareResult{TEntity, TKey}"/> que contiene los resultados de la comparación,
    /// incluyendo las diferencias encontradas entre las dos listas.
    /// </returns>
    /// <remarks>
    /// Este método extiende <see cref="IEnumerable{TEntity}"/> para facilitar la comparación de listas
    /// de entidades que implementan la interfaz <see cref="IKey{TKey}"/>.
    /// </remarks>
    /// <seealso cref="IKey{TKey}"/>
    /// <seealso cref="ListCompareResult{TEntity, TKey}"/>
    public static ListCompareResult<TEntity, TKey> Compare<TEntity, TKey>( this IEnumerable<TEntity> newList, IEnumerable<TEntity> originalList )
        where TEntity : IKey<TKey> {
        var comparator = new ListComparator<TEntity, TKey>();
        return comparator.Compare( newList, originalList );
    }

    /// <summary>
    /// Compara dos listas de identificadores únicos (GUID) y devuelve el resultado de la comparación.
    /// </summary>
    /// <param name="newList">La nueva lista de identificadores únicos a comparar.</param>
    /// <param name="originalList">La lista original de identificadores únicos con la que se comparará.</param>
    /// <returns>
    /// Un objeto <see cref="KeyListCompareResult{Guid}"/> que contiene el resultado de la comparación 
    /// entre <paramref name="newList"/> y <paramref name="originalList"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una instancia de <see cref="KeyListComparator{Guid}"/> para realizar la comparación.
    /// Asegúrese de que ambas listas no sean nulas antes de llamar a este método.
    /// </remarks>
    public static KeyListCompareResult<Guid> Compare( this IEnumerable<Guid> newList, IEnumerable<Guid> originalList ) {
        var comparator = new KeyListComparator<Guid>();
        return comparator.Compare( newList, originalList );
    }

    /// <summary>
    /// Compara dos listas de cadenas y devuelve el resultado de la comparación.
    /// </summary>
    /// <param name="newList">La nueva lista de cadenas que se va a comparar.</param>
    /// <param name="originalList">La lista original de cadenas con la que se va a comparar.</param>
    /// <returns>Un objeto <see cref="KeyListCompareResult{T}"/> que contiene el resultado de la comparación.</returns>
    /// <remarks>
    /// Este método es una extensión para <see cref="IEnumerable{T}"/> que permite comparar dos listas de cadenas.
    /// Utiliza la clase <see cref="KeyListComparator{T}"/> para realizar la comparación.
    /// </remarks>
    public static KeyListCompareResult<string> Compare( this IEnumerable<string> newList, IEnumerable<string> originalList ) {
        var comparator = new KeyListComparator<string>();
        return comparator.Compare( newList, originalList );
    }

    /// <summary>
    /// Compara dos listas de enteros y devuelve el resultado de la comparación.
    /// </summary>
    /// <param name="newList">La nueva lista de enteros que se va a comparar.</param>
    /// <param name="originalList">La lista original de enteros con la que se va a comparar.</param>
    /// <returns>Un objeto <see cref="KeyListCompareResult{int}"/> que contiene el resultado de la comparación.</returns>
    /// <remarks>
    /// Este método es una extensión para <see cref="IEnumerable{int}"/> que permite comparar dos listas
    /// de enteros y obtener un resultado que indica las diferencias entre ellas.
    /// </remarks>
    /// <seealso cref="KeyListComparator{T}"/>
    public static KeyListCompareResult<int> Compare( this IEnumerable<int> newList, IEnumerable<int> originalList ) {
        var comparator = new KeyListComparator<int>();
        return comparator.Compare( newList, originalList );
    }

    /// <summary>
    /// Compara dos listas de números enteros largos.
    /// </summary>
    /// <param name="newList">La nueva lista de números enteros largos que se va a comparar.</param>
    /// <param name="originalList">La lista original de números enteros largos con la que se va a comparar.</param>
    /// <returns>Un objeto <see cref="KeyListCompareResult{long}"/> que contiene el resultado de la comparación.</returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="IEnumerable{long}"/> para facilitar la comparación de listas.
    /// Utiliza una instancia de <see cref="KeyListComparator{long}"/> para realizar la comparación.
    /// </remarks>
    public static KeyListCompareResult<long> Compare( this IEnumerable<long> newList, IEnumerable<long> originalList ) {
        var comparator = new KeyListComparator<long>();
        return comparator.Compare( newList, originalList );
    }
}