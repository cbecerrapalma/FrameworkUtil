using Util.Domain.Compare;

namespace Util.Domain.Entities; 

/// <summary>
/// Representa una entidad en el dominio.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IDomainObject"/> y puede ser implementada por cualquier clase que represente una entidad en el contexto de un dominio específico.
/// </remarks>
public interface IEntity : IDomainObject {
    /// <summary>
    /// Inicializa los recursos necesarios para el funcionamiento del sistema.
    /// </summary>
    /// <remarks>
    /// Este método debe ser llamado antes de utilizar cualquier otro método de la clase.
    /// Asegúrese de que todos los recursos se configuren correctamente para evitar errores.
    /// </remarks>
    void Init();
}

/// <summary>
/// Define una interfaz para entidades que tienen una clave de tipo TKey.
/// </summary>
/// <typeparam name="TKey">El tipo de la clave de la entidad.</typeparam>
public interface IEntity<out TKey> : IKey<TKey>, IEntity {
}

/// <summary>
/// Define una interfaz genérica para entidades que permiten la comparación de cambios y la identificación por clave.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que implementa la interfaz <see cref="IEntity{TEntity, TKey}"/>.</typeparam>
/// <typeparam name="TKey">El tipo de la clave que identifica de manera única a la entidad.</typeparam>
/// <remarks>
/// Esta interfaz extiende <see cref="ICompareChange{TEntity}"/> para permitir la comparación de cambios en la entidad
/// y <see cref="IEntity{TKey}"/> para proporcionar funcionalidad de identificación por clave.
/// </remarks>
public interface IEntity<TEntity, out TKey> : ICompareChange<TEntity>, IEntity<TKey> where TEntity : IEntity {
    /// <summary>
    /// Clona la instancia actual de la entidad.
    /// </summary>
    /// <returns>Una nueva instancia de <typeparamref name="TEntity"/> que es una copia de la entidad actual.</returns>
    /// <remarks>
    /// Este método permite crear una copia independiente de la entidad, lo que puede ser útil para evitar modificaciones no deseadas en la instancia original.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de la entidad que se está clonando.</typeparam>
    TEntity Clone();
}