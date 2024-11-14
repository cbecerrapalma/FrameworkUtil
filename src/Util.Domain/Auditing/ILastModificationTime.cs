namespace Util.Domain.Auditing;

/// <summary>
/// Interfaz que representa un objeto que tiene un tiempo de última modificación.
/// </summary>
public interface ILastModificationTime {
    /// <summary>
    /// Obtiene o establece la fecha y hora de la última modificación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de tipo nullable, lo que significa que puede contener un valor de fecha y hora o ser nula si no se ha realizado ninguna modificación.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la última fecha y hora de modificación, o <c>null</c> si no hay una fecha de modificación registrada.
    /// </value>
    DateTime? LastModificationTime { get; set; }
}