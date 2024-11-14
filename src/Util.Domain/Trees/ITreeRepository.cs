using Util.Domain.Repositories;

namespace Util.Domain.Trees; 

/// <summary>
/// Define un repositorio para entidades de tipo árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa <see cref="ITreeEntity{TEntity, TKey, TParentKey}"/>.</typeparam>
/// <remarks>
/// Este repositorio permite realizar operaciones específicas para entidades que tienen una estructura jerárquica.
/// </remarks>
/// <seealso cref="ITreeEntity{TEntity, TKey, TParentKey}"/>
public interface ITreeRepository<TEntity> : ITreeRepository<TEntity, Guid, Guid?> where TEntity : class, ITreeEntity<TEntity, Guid, Guid?> {
}

/// <summary>
/// Interfaz que define un repositorio para manejar entidades en una estructura de árbol.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se manejará en el repositorio.</typeparam>
/// <typeparam name="TKey">El tipo de clave única para identificar cada entidad.</typeparam>
/// <typeparam name="TParentId">El tipo de identificador del padre de la entidad en la estructura de árbol.</typeparam>
/// <seealso cref="IRepository{TEntity, TKey}"/>
public interface ITreeRepository<TEntity, in TKey, in TParentId> : IRepository<TEntity, TKey>
    where TEntity : class,ITreeEntity<TEntity, TKey, TParentId> {
    /// <summary>
    /// Obtiene una lista de todos los hijos de una entidad padre específica de forma asíncrona.
    /// </summary>
    /// <param name="parent">La entidad padre de la cual se desean obtener los hijos.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea es una lista de entidades hijas de tipo <typeparamref name="TEntity"/>.</returns>
    /// <remarks>
    /// Este método es útil para recuperar de manera eficiente todas las entidades hijas asociadas a una entidad padre en un contexto de base de datos.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que representa tanto a la entidad padre como a las entidades hijas.</typeparam>
    /// <seealso cref="GetAllAsync"/>
    Task<List<TEntity>> GetAllChildrenAsync( TEntity parent );
}