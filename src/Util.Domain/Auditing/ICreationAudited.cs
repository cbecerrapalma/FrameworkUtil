namespace Util.Domain.Auditing; 

/// <summary>
/// Interfaz que representa un objeto que tiene información de auditoría de creación.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ICreationAudited{TKey}"/> y utiliza un tipo de clave 
/// de datos nullable <see cref="Guid?"/> para la identificación.
/// </remarks>
public interface ICreationAudited : ICreationAudited<Guid?> {
}

/// <summary>
/// Interfaz que representa un objeto que tiene información de auditoría de creación.
/// </summary>
/// <typeparam name="TKey">El tipo de la clave primaria del objeto.</typeparam>
/// <remarks>
/// Esta interfaz extiende <see cref="ICreationTime"/> para incluir la funcionalidad de auditoría de creación,
/// permitiendo que las entidades mantengan un registro de cuándo y por quién fueron creadas.
/// </remarks>
public interface ICreationAudited<TKey> : ICreationTime {
    /// <summary>
    /// Obtiene o establece el identificador del creador.
    /// </summary>
    /// <value>
    /// El identificador del creador.
    /// </value>
    TKey CreatorId { get; set; }
}