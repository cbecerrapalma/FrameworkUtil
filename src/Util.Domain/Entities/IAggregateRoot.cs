namespace Util.Domain.Entities; 

/// <summary>
/// Interfaz que representa una raíz de agregado en el contexto de DDD (Domain-Driven Design).
/// Una raíz de agregado es una entidad que actúa como punto de entrada para un conjunto de entidades relacionadas.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IEntity"/> y <see cref="IVersion"/>,
/// lo que implica que cualquier clase que implemente <see cref="IAggregateRoot"/> 
/// debe proporcionar una implementación para las propiedades y métodos definidos en esas interfaces.
/// </remarks>
public interface IAggregateRoot : IEntity, IVersion {
}

/// <summary>
/// Representa la interfaz base para una raíz de agregado en el contexto de DDD (Domain-Driven Design).
/// </summary>
/// <typeparam name="TKey">El tipo de la clave que identifica de manera única a la entidad.</typeparam>
/// <remarks>
/// Una raíz de agregado es una entidad que actúa como punto de entrada para un conjunto de entidades relacionadas.
/// Asegura la consistencia de los cambios en el agregado y define las reglas de negocio que se aplican a él.
/// </remarks>
public interface IAggregateRoot<out TKey> : IEntity<TKey>, IAggregateRoot {
}

/// <summary>
/// Representa la interfaz base para una raíz de agregado en el contexto de DDD (Domain-Driven Design).
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa la raíz del agregado.</typeparam>
/// <typeparam name="TKey">El tipo de la clave única que identifica la entidad.</typeparam>
/// <remarks>
/// Esta interfaz extiende de <see cref="IEntity{TEntity, TKey}"/> y <see cref="IAggregateRoot{TKey}"/>.
/// Se utiliza para definir las operaciones y comportamientos que deben ser implementados por las raíces de agregado.
/// </remarks>
public interface IAggregateRoot<TEntity, out TKey> : IEntity<TEntity, TKey>, IAggregateRoot<TKey> where TEntity : IAggregateRoot {
}