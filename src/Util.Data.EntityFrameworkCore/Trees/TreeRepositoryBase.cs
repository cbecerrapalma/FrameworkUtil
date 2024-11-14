using Util.Domain.Trees;

namespace Util.Data.EntityFrameworkCore.Trees;

/// <summary>
/// Representa un repositorio para almacenar y administrar entidades en una estructura de árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que implementa la estructura de árbol.</typeparam>
public abstract class TreeRepositoryBase<TEntity> : TreeRepositoryBase<TEntity, Guid, Guid?>, ITreeRepository<TEntity>
    where TEntity : class, ITreeEntity<TEntity, Guid, Guid?>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="TreeRepositoryBase{TEntity}"/> con la unidad de trabajo especificada.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona la transacción de datos.</param>
    protected TreeRepositoryBase(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}

/// <summary>
/// Representa un repositorio genérico para administrar entidades en una estructura de árbol con soporte para identificadores de entidad y de padre personalizados.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que implementa la estructura de árbol.</typeparam>
/// <typeparam name="TKey">El tipo de identificador de la entidad.</typeparam>
/// <typeparam name="TParentId">El tipo de identificador del padre de la entidad.</typeparam>
public abstract class TreeRepositoryBase<TEntity, TKey, TParentId> : RepositoryBase<TEntity, TKey>, ITreeRepository<TEntity, TKey, TParentId>
    where TEntity : class, ITreeEntity<TEntity, TKey, TParentId>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="TreeRepositoryBase{TEntity, TKey, TParentId}"/> con la unidad de trabajo especificada.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que gestiona la transacción de datos.</param>
    protected TreeRepositoryBase(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    /// <summary>
    /// Obtiene de forma asincrónica todos los hijos de una entidad padre específica en la estructura de árbol.
    /// </summary>
    /// <param name="parent">La entidad padre de la cual se desea obtener todos los hijos.</param>
    /// <returns>Una lista de entidades que son hijos del <paramref name="parent"/>, excluyendo al padre.</returns>
    public virtual async Task<List<TEntity>> GetAllChildrenAsync(TEntity parent)
    {
        var list = await FindAllAsync(t => t.Path.StartsWith(parent.Path));
        return list.Where(t => !t.Id.Equals(parent.Id)).ToList();
    }
}
