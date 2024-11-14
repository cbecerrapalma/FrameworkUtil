using Util.Domain.Entities;
using Util.Domain.Repositories;

namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Clase base abstracta para repositorios que maneja entidades de tipo TEntity.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que el repositorio manejará.</typeparam>
/// <remarks>
/// Esta clase proporciona una implementación básica para las operaciones de repositorio,
/// permitiendo la extensión y personalización en repositorios concretos.
/// </remarks>
/// <seealso cref="IRepository{TEntity}"/>
public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, Guid>, IRepository<TEntity>
    where TEntity : class, IAggregateRoot<Guid> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RepositoryBase"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para las operaciones de repositorio.</param>
    protected RepositoryBase(IUnitOfWork unitOfWork)
        : base(unitOfWork) { }
}

/// <summary>
/// Clase base abstracta para repositorios que proporciona una implementación común 
/// para operaciones de acceso a datos sobre entidades de tipo <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que representa el repositorio.</typeparam>
/// <typeparam name="TKey">El tipo de la clave primaria de la entidad.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="StoreBase{TEntity, TKey}"/> y 
/// debe ser implementada por repositorios concretos que manejan entidades 
/// que implementan la interfaz <see cref="IAggregateRoot{TKey}"/>.
/// </remarks>
public abstract class RepositoryBase<TEntity, TKey> : StoreBase<TEntity, TKey>, IRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<TKey> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RepositoryBase"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para las operaciones de repositorio.</param>
    protected RepositoryBase(IUnitOfWork unitOfWork) : base(unitOfWork) { }
}