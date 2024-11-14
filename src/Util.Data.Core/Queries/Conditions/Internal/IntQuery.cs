namespace Util.Data.Queries.Conditions.Internal;

/// <summary>
/// Clase interna que representa una consulta de enteros.
/// </summary>
internal class IntQuery {
    /// <summary>
    /// Obtiene o establece el valor mínimo.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si se establece, debe ser un entero que representa el valor mínimo permitido.
    /// </remarks>
    /// <value>
    /// Un entero que representa el valor mínimo o <c>null</c> si no se ha establecido.
    /// </value>
    public int? MinValue { get; set; }
    /// <summary>
    /// Obtiene o establece el valor máximo.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si se establece en un valor, representa el límite superior permitido.
    /// </remarks>
    /// <value>
    /// Un entero que representa el valor máximo, o null si no se ha establecido.
    /// </value>
    public int? MaxValue { get; set; }
}