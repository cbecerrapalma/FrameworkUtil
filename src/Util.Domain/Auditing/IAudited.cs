namespace Util.Domain.Auditing; 

/// <summary>
/// Interfaz que representa un objeto que ha sido auditado, 
/// incluyendo información sobre su creación y modificación.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ICreationAudited"/> y 
/// <see cref="IModificationAudited"/>, lo que permite acceder 
/// a las propiedades relacionadas con la creación y modificación 
/// de un objeto.
/// </remarks>
public interface IAudited : ICreationAudited, IModificationAudited {
}

/// <summary>
/// Interfaz que representa un objeto auditado, que incluye información sobre la creación y modificación.
/// </summary>
/// <typeparam name="TKey">El tipo de la clave primaria del objeto auditado.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="ICreationAudited{TKey}"/> y <see cref="IModificationAudited{TKey}"/>,
/// lo que permite que las implementaciones tengan acceso a las propiedades relacionadas con la creación y
/// modificación de un objeto.
/// </remarks>
public interface IAudited<TKey> : ICreationAudited<TKey>, IModificationAudited<TKey> {
}