using Util.Domain.Entities;

namespace Util.Domain.Trees; 

/// <summary>
/// Define una entidad de árbol que puede tener una jerarquía de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que implementa esta interfaz.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única de la entidad.</typeparam>
/// <typeparam name="TParentId">El tipo del identificador del padre de la entidad.</typeparam>
/// <remarks>
/// Esta interfaz extiende otras interfaces que proporcionan funcionalidades 
/// adicionales como la raíz del agregado, el identificador del padre, 
/// la gestión de rutas, habilitación y ordenamiento.
/// </remarks>
public interface ITreeEntity<TEntity, TKey, TParentId> : IAggregateRoot<TEntity, TKey>, IParentId<TParentId>, IPath, IEnabled, ISortId where TEntity : ITreeEntity<TEntity, TKey, TParentId> {
    /// <summary>
    /// Inicializa la ruta necesaria para el funcionamiento del sistema.
    /// </summary>
    /// <remarks>
    /// Este método configura los parámetros iniciales y establece las rutas que serán utilizadas
    /// a lo largo de la ejecución del programa. Es importante llamar a este método antes de
    /// realizar cualquier operación que dependa de la ruta configurada.
    /// </remarks>
    void InitPath();
    /// <summary>
    /// Inicializa la ruta del entidad padre especificada.
    /// </summary>
    /// <param name="parent">La entidad padre de tipo <typeparamref name="TEntity"/> que se utilizará para inicializar la ruta.</param>
    /// <typeparam name="TEntity">El tipo de la entidad que se está inicializando.</typeparam>
    /// <remarks>
    /// Este método es útil para establecer la jerarquía o la relación de rutas entre entidades.
    /// Asegúrese de que la entidad padre no sea nula antes de llamar a este método.
    /// </remarks>
    void InitPath(TEntity parent);
    /// <summary>
    /// Obtiene una lista de identificadores de los padres a partir de una ruta especificada.
    /// </summary>
    /// <param name="excludeSelf">Indica si se debe excluir el identificador del objeto actual de la lista. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Una lista de identificadores de tipo <typeparamref name="TKey"/> que representan los padres del objeto actual.</returns>
    /// <remarks>
    /// Este método es útil para obtener la jerarquía de padres en una estructura de árbol o grafo,
    /// permitiendo la manipulación o análisis de la relación entre nodos.
    /// </remarks>
    /// <seealso cref="List{TKey}"/>
    List<TKey> GetParentIdsFromPath( bool excludeSelf = true );
}