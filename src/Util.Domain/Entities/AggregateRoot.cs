using Util.Domain.Events;

namespace Util.Domain.Entities;

/// <summary>
/// Representa la clase base para una raíz de agregado con una clave de tipo Guid.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad de la raíz del agregado.</typeparam>
public abstract class AggregateRoot<TEntity> : AggregateRoot<TEntity, Guid> where TEntity : IAggregateRoot<TEntity, Guid>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AggregateRoot{TEntity}"/> con un identificador específico.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    protected AggregateRoot(Guid id) : base(id)
    {
    }
}

/// <summary>
/// Representa la clase base para una raíz de agregado con una clave de tipo genérico.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad de la raíz del agregado.</typeparam>
/// <typeparam name="TKey">El tipo de la clave de la entidad.</typeparam>
public abstract class AggregateRoot<TEntity, TKey> : EntityBase<TEntity, TKey>, IAggregateRoot<TEntity, TKey>, IDomainEventManager where TEntity : IAggregateRoot<TEntity, TKey>
{
    private List<IEvent> _domainEvents;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AggregateRoot{TEntity, TKey}"/> con un identificador específico.
    /// </summary>
    /// <param name="id">El identificador único de la entidad.</param>
    protected AggregateRoot(TKey id) : base(id)
    {
    }

    /// <summary>
    /// Obtiene o establece la versión de concurrencia de la entidad.
    /// </summary>
    public byte[] Version { get; set; }

    /// <summary>
    /// Obtiene una colección de eventos de dominio asociados a la entidad.
    /// </summary>
    /// <remarks>
    /// Los eventos de dominio permiten capturar y manejar cambios importantes en el estado de la entidad.
    /// </remarks>
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents?.AsReadOnly();

    /// <summary>
    /// Agrega un evento de dominio a la colección de eventos de la entidad.
    /// </summary>
    /// <param name="event">El evento de dominio a agregar.</param>
    public void AddDomainEvent(IEvent @event)
    {
        if (@event == null)
            return;
        _domainEvents ??= new List<IEvent>();
        _domainEvents.Add(@event);
    }

    /// <summary>
    /// Elimina un evento de dominio de la colección de eventos de la entidad.
    /// </summary>
    /// <param name="event">El evento de dominio a eliminar.</param>
    public void RemoveDomainEvent(IEvent @event)
    {
        if (@event == null)
            return;
        _domainEvents?.Remove(@event);
    }

    /// <summary>
    /// Elimina todos los eventos de dominio de la colección de eventos de la entidad.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}
