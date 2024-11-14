using Util.Domain.Compare;

namespace Util.Domain.Events; 

/// <summary>
/// Representa un evento que se genera cuando una entidad de tipo <typeparamref name="TEntity"/> ha cambiado.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que ha cambiado.</typeparam>
public class EntityChangedEvent<TEntity> : IEvent {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityChangedEvent"/>.
    /// </summary>
    /// <param name="entity">La entidad que ha cambiado.</param>
    /// <param name="changeType">El tipo de cambio que se ha realizado en la entidad.</param>
    /// <param name="changeValues">Colección de valores que han cambiado en la entidad. Este parámetro es opcional.</param>
    public EntityChangedEvent( TEntity entity, EntityChangeType changeType, ChangeValueCollection changeValues = null ) {
        Entity = entity;
        ChangeType = changeType;
        ChangeValues = changeValues;
    }

    /// <summary>
    /// Obtiene la entidad de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la entidad asociada, la cual es de tipo genérico <typeparamref name="TEntity"/>.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está manejando.</typeparam>
    public TEntity Entity { get; }

    /// <summary>
    /// Obtiene el tipo de cambio de entidad.
    /// </summary>
    /// <value>
    /// El tipo de cambio de entidad.
    /// </value>
    public EntityChangeType ChangeType { get; }

    /// <summary>
    /// Obtiene la colección de valores que se pueden cambiar.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="ChangeValueCollection"/> que contiene los valores cambiables.
    /// </value>
    public ChangeValueCollection ChangeValues { get; }
}