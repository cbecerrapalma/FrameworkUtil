using Util.Data;
using Util.Domain.Properties;
using Util.Exceptions;

namespace Util.Domain.Trees; 

/// <summary>
/// Clase que gestiona la actualización de rutas para entidades jerárquicas.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se está gestionando.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única de la entidad.</typeparam>
/// <typeparam name="TParentId">El tipo del identificador del padre de la entidad.</typeparam>
public class UpdatePathManager<TEntity, TKey, TParentId>
    where TEntity : class, ITreeEntity<TEntity, TKey, TParentId> {
    private readonly ITreeRepository<TEntity, TKey, TParentId> _repository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdatePathManager"/>.
    /// </summary>
    /// <param name="repository">La instancia del repositorio que se utilizará para gestionar las entidades.</param>
    public UpdatePathManager( ITreeRepository<TEntity, TKey, TParentId> repository ) {
        _repository = repository;
    }

    /// <summary>
    /// Actualiza la ruta de un entidad de tipo <typeparamref name="TEntity"/> de forma asíncrona.
    /// </summary>
    /// <param name="entity">La entidad que se va a actualizar.</param>
    /// <remarks>
    /// Este método verifica si la entidad no es nula y si no se está intentando mover a sí misma como hijo.
    /// Si la entidad ya tiene el mismo padre, no se realiza ninguna acción.
    /// Si la entidad tiene hijos, se verifica que el nuevo padre no sea uno de ellos.
    /// Finalmente, se inicializa la ruta de la entidad y se actualizan las rutas de sus hijos.
    /// </remarks>
    /// <exception cref="Warning">Se lanza una advertencia si se intenta mover la entidad a uno de sus hijos.</exception>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public async Task UpdatePathAsync( TEntity entity ) {
        entity.CheckNull( nameof( entity ) );
        if( entity.ParentId.Equals( entity.Id ) )
            throw new Warning( DomainResource.NotSupportMoveToChildren );
        var old = await _repository.NoTracking().FindByIdAsync( entity.Id );
        if( old == null )
            return;
        if( entity.ParentId.Equals( old.ParentId ) )
            return;
        var children = await _repository.GetAllChildrenAsync( entity );
        if( children.Exists( t => t.Id.Equals( entity.ParentId ) ) )
            throw new Warning( DomainResource.NotSupportMoveToChildren );
        var parent = await _repository.FindByIdAsync( entity.ParentId );
        entity.InitPath( parent );
        UpdateChildrenPath( entity, children );
        await _repository.UpdateAsync( children );
    }

    /// <summary>
    /// Actualiza la ruta de los hijos de un entidad padre.
    /// </summary>
    /// <param name="parent">La entidad padre cuya ruta se actualizará.</param>
    /// <param name="children">La lista de entidades hijas que se actualizarán.</param>
    /// <remarks>
    /// Este método recorre recursivamente la lista de entidades hijas y actualiza la ruta de cada una de ellas
    /// en función de su entidad padre. Si el padre o la lista de hijos son nulos, el método no realiza ninguna acción.
    /// </remarks>
    private void UpdateChildrenPath( TEntity parent, List<TEntity> children ) {
        if( parent == null || children == null )
            return;
        var list = children.Where( t => t.ParentId.Equals( parent.Id ) ).ToList();
        foreach( var child in list ) {
            child.InitPath( parent );
            UpdateChildrenPath( child, children );
        }
    }
}