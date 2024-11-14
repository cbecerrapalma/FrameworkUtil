namespace Util.Domain.Auditing; 

/// <summary>
/// Interfaz que representa un objeto que ha sido auditado en cuanto a modificaciones.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IModificationAudited{TKey}"/> y utiliza un tipo de clave de tipo nullable <see cref="Guid"/>.
/// </remarks>
public interface IModificationAudited : IModificationAudited<Guid?> {
}

/// <summary>
/// Interfaz que representa un objeto que ha sido auditado por modificaciones,
/// incluyendo un identificador de tipo clave y un tiempo de última modificación.
/// </summary>
/// <typeparam name="TKey">El tipo de la clave del objeto auditado.</typeparam>
/// <seealso cref="ILastModificationTime"/>
public interface IModificationAudited<TKey> : ILastModificationTime {
    /// <summary>
    /// Obtiene o establece el identificador del último modificador.
    /// </summary>
    /// <value>
    /// El identificador del último modificador.
    /// </value>
    TKey LastModifierId { get; set; }
}