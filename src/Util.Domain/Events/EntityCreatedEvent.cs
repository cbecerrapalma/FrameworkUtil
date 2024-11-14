namespace Util.Domain.Events; 

/// <summary>
/// Representa un evento que se genera cuando una entidad es creada.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que ha sido creada.</typeparam>
public class EntityCreatedEvent<TEntity> : IEvent {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityCreatedEvent"/>.
    /// </summary>
    /// <param name="entity">La entidad que se ha creado.</param>
    public EntityCreatedEvent( TEntity entity ) {
        Entity = entity;
    }

    /// <summary>
    /// Obtiene la entidad de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la entidad que se está manejando.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de la entidad que se está representando.
    /// </typeparam>
    public TEntity Entity { get; }
}