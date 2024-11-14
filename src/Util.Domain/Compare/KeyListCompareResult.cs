namespace Util.Domain.Compare; 

/// <summary>
/// Representa el resultado de la comparación de listas de claves.
/// </summary>
/// <typeparam name="TKey">El tipo de las claves que se están comparando.</typeparam>
public class KeyListCompareResult<TKey> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="KeyListCompareResult"/>.
    /// </summary>
    /// <param name="createList">Lista de claves que se han creado.</param>
    /// <param name="updateList">Lista de claves que se han actualizado.</param>
    /// <param name="deleteList">Lista de claves que se han eliminado.</param>
    public KeyListCompareResult( List<TKey> createList, List<TKey> updateList, List<TKey> deleteList ) {
        CreateList = createList;
        UpdateList = updateList;
        DeleteList = deleteList;
    }

    /// <summary>
    /// Obtiene una lista de claves de tipo <typeparamref name="TKey"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una lista que contiene elementos de tipo <typeparamref name="TKey"/>.
    /// La lista es de solo lectura y se inicializa en el momento de la creación del objeto.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de las claves que se almacenan en la lista.</typeparam>
    /// <returns>Una lista de claves de tipo <typeparamref name="TKey"/>.</returns>
    public List<TKey> CreateList { get; }

    /// <summary>
    /// Representa una lista de claves que se puede actualizar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una lista de claves de tipo <typeparamref name="TKey"/>.
    /// La lista es de solo lectura y se puede utilizar para obtener las claves actuales.
    /// </remarks>
    /// <typeparam name="TKey">El tipo de las claves en la lista.</typeparam>
    /// <value>
    /// Una lista de claves de tipo <typeparamref name="TKey"/>.
    /// </value>
    public List<TKey> UpdateList { get; }

    /// <summary>
    /// Obtiene la lista de claves que se deben eliminar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una lista de claves de tipo <typeparamref name="TKey"/> 
    /// que representan los elementos que han sido marcados para eliminación. 
    /// La lista es de solo lectura y no se puede modificar directamente.
    /// </remarks>
    /// <typeparam name="TKey">
    /// El tipo de las claves en la lista.
    /// </typeparam>
    /// <seealso cref="AddToDeleteList(TKey key)"/>
    /// <seealso cref="ClearDeleteList()"/>
    public List<TKey> DeleteList { get; }
}