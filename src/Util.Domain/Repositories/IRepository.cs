using Util.Data.Stores;
using Util.Domain.Entities;

namespace Util.Domain.Repositories; 

/// <summary>
/// Representa un repositorio genérico para la gestión de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que el repositorio manejará.</typeparam>
/// <remarks>
/// Este repositorio hereda de <see cref="IRepository{TEntity, TKey}"/> y <see cref="IQueryRepository{TEntity}"/>.
/// Permite realizar operaciones de creación, lectura, actualización y eliminación (CRUD) sobre entidades de tipo <typeparamref name="TEntity"/>.
/// </remarks>
public interface IRepository<TEntity> : IRepository<TEntity, Guid>, IQueryRepository<TEntity>
    where TEntity : class, IAggregateRoot<Guid> {
}

/// <summary>
/// Representa un repositorio genérico para la gestión de entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que el repositorio manejará.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IQueryRepository{TEntity, TKey}"/> y <see cref="IStore{TEntity, TKey}"/>,
/// proporcionando métodos para consultar y almacenar entidades.
/// </remarks>
public interface IRepository<TEntity, in TKey> : IQueryRepository<TEntity, TKey>, IStore<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey> {
}