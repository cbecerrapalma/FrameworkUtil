namespace Util.Domain.Compare; 

/// <summary>
/// Representa el resultado de la comparación entre dos listas de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IKey{TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave que identifica de manera única a cada entidad.</typeparam>
public class ListCompareResult<TEntity, TKey> where TEntity : IKey<TKey> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ListCompareResult"/>.
    /// </summary>
    /// <param name="createList">Lista de elementos que se han creado.</param>
    /// <param name="updateList">Lista de elementos que se han actualizado.</param>
    /// <param name="deleteList">Lista de elementos que se han eliminado.</param>
    public ListCompareResult( List<TEntity> createList, List<TEntity> updateList, List<TEntity> deleteList ) {
        CreateList = createList;
        UpdateList = updateList;
        DeleteList = deleteList;
    }

    /// <summary>
    /// Obtiene una lista de entidades del tipo especificado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una lista que contiene instancias de <typeparamref name="TEntity"/>.
    /// La lista se puede utilizar para almacenar y manipular múltiples entidades.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se almacenará en la lista.
    /// </typeparam>
    /// <returns>
    /// Una lista de entidades del tipo <typeparamref name="TEntity"/>.
    /// </returns>
    public List<TEntity> CreateList { get; }

    /// <summary>
    /// Obtiene la lista de entidades que han sido actualizadas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una lista de entidades de tipo <typeparamref name="TEntity"/> 
    /// que han sido modificadas en el contexto actual. La lista es de solo lectura.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que se está gestionando en la lista.</typeparam>
    /// <returns>Una lista de entidades actualizadas.</returns>
    public List<TEntity> UpdateList { get; }

    /// <summary>
    /// Obtiene la lista de entidades que se eliminarán.
    /// </summary>
    /// <value>
    /// Una lista de entidades de tipo <typeparamref name="TEntity"/> que representan las entidades a eliminar.
    /// </value>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la lista de entidades que están marcadas para eliminación.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de las entidades que se manejarán en la lista.
    /// </typeparam>
    public List<TEntity> DeleteList { get; }
}