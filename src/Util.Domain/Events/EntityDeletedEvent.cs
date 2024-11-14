namespace Util.Domain.Events; 

/// <summary>
/// Representa un evento que se genera cuando una entidad es eliminada.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que ha sido eliminada.</typeparam>
public class EntityDeletedEvent<TEntity> : IEvent {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityDeletedEvent"/>.
    /// </summary>
    /// <param name="entity">La entidad que ha sido eliminada.</param>
    public EntityDeletedEvent( TEntity entity ) {
        Entity = entity;
    }

    /// <summary>
    /// Obtiene la entidad de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la entidad que se está utilizando en el contexto actual.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está representando.</typeparam>
    public TEntity Entity { get; }
}