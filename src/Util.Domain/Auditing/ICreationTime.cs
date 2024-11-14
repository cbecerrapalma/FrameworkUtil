namespace Util.Domain.Auditing;

/// <summary>
/// Interfaz que representa un objeto que tiene un tiempo de creación.
/// </summary>
public interface ICreationTime {
    /// <summary>
    /// Obtiene o establece la fecha y hora de creación.
    /// </summary>
    /// <remarks>
    /// Este campo es de tipo nullable, lo que significa que puede contener un valor de fecha y hora o ser nulo.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la fecha y hora de creación, o <c>null</c> si no se ha establecido.
    /// </value>
    DateTime? CreationTime { get; set; }
}