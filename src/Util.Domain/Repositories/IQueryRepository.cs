using Util.Data.Stores;
using Util.Domain.Entities;

namespace Util.Domain.Repositories; 

/// <summary>
/// Representa un repositorio de consultas genérico para entidades que implementan 
/// <see cref="IAggregateRoot{TKey}"/> con una clave de tipo <see cref="Guid"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se gestionará en el repositorio.</typeparam>
/// <remarks>
/// Este repositorio permite realizar operaciones de consulta sobre entidades específicas 
/// sin necesidad de implementar la lógica de acceso a datos en cada caso.
/// </remarks>
public interface IQueryRepository<TEntity> : IQueryRepository<TEntity, Guid> where TEntity : class, IAggregateRoot<Guid> {
}

/// <summary>
/// Interfaz que define un repositorio de consultas para entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa el repositorio.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
/// <remarks>
/// Esta interfaz extiende de <see cref="IQueryStore{TEntity, TKey}"/> y está diseñada
/// para ser implementada por repositorios que manejan entidades que son raíces de agregado.
/// </remarks>
public interface IQueryRepository<TEntity, in TKey> : IQueryStore<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey> {
}