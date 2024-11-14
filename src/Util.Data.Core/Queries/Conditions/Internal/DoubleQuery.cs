namespace Util.Data.Queries.Conditions.Internal;

/// <summary>
/// Representa una consulta que opera sobre valores de tipo doble.
/// </summary>
/// <remarks>
/// Esta clase permite realizar operaciones específicas sobre datos de tipo doble,
/// facilitando la manipulación y análisis de conjuntos de datos numéricos.
/// </remarks>
internal class DoubleQuery {
    /// <summary>
    /// Obtiene o establece el valor mínimo permitido.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si se establece, debe ser menor que el valor máximo permitido.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="double"/> que representa el valor mínimo, o <c>null</c> si no se ha establecido.
    /// </value>
    public double? MinValue { get; set; }
    /// <summary>
    /// Obtiene o establece el valor máximo permitido.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si se establece, debe ser un número positivo.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="double"/> que representa el valor máximo, o <c>null</c> si no se ha establecido.
    /// </value>
    public double? MaxValue { get; set; }
}