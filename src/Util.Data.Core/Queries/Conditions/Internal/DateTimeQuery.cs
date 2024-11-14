namespace Util.Data.Queries.Conditions.Internal;

/// <summary>
/// Representa una consulta para manipular y obtener información de fechas y horas.
/// </summary>
internal class DateTimeQuery {
    /// <summary>
    /// Obtiene o establece la hora de inicio.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo, lo que indica que no se ha definido una hora de inicio.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la hora de inicio, o <c>null</c> si no se ha establecido.
    /// </value>
    public DateTime? BeginTime { get; set; }
    /// <summary>
    /// Obtiene o establece la hora de finalización.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de tipo nullable, lo que significa que puede contener un valor de tipo <see cref="DateTime"/> 
    /// o un valor nulo si no se ha establecido una hora de finalización.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DateTime"/> que representa la hora de finalización, o <c>null</c> si no se ha definido.
    /// </value>
    public DateTime? EndTime { get; set; }
}