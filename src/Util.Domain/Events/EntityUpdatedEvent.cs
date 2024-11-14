using Util.Domain.Compare;

namespace Util.Domain.Events; 

/// <summary>
/// Representa un evento que se genera cuando una entidad es actualizada.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que ha sido actualizada.</typeparam>
public class EntityUpdatedEvent<TEntity> : IEvent {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EntityUpdatedEvent"/>.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que ha sido actualizada.</typeparam>
    /// <param name="entity">La entidad que ha sido actualizada.</param>
    /// <param name="changeValues">Una colección de los valores que han cambiado en la entidad.</param>
    public EntityUpdatedEvent( TEntity entity, ChangeValueCollection changeValues ) {
        Entity = entity;
        ChangeValues = changeValues;
    }

    /// <summary>
    /// Obtiene la entidad de tipo <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso a la entidad asociada.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de la entidad que se está gestionando.
    /// </typeparam>
    public TEntity Entity { get; }

    /// <summary>
    /// Obtiene la colección de valores que se pueden cambiar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una instancia de <see cref="ChangeValueCollection"/> 
    /// que contiene los valores que se pueden modificar en el contexto actual.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="ChangeValueCollection"/> que representa los valores cambiables.
    /// </value>
    public ChangeValueCollection ChangeValues { get; }
}