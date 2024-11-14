namespace Util.Data.Queries.Conditions.Internal;

/// <summary>
/// Representa una consulta que opera con valores decimales.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para realizar operaciones específicas sobre datos decimales,
/// permitiendo filtrado, ordenamiento y otras manipulaciones necesarias en el contexto
/// de consultas a bases de datos o colecciones de datos.
/// </remarks>
internal class DecimalQuery {
    /// <summary>
    /// Obtiene o establece el valor mínimo permitido.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si se establece, debe ser un número decimal que representa el límite inferior.
    /// </remarks>
    /// <value>
    /// Un valor decimal que representa el mínimo permitido, o null si no se ha establecido.
    /// </value>
    public decimal? MinValue { get; set; }
    /// <summary>
    /// Obtiene o establece el valor máximo permitido.
    /// </summary>
    /// <remarks>
    /// Este valor es de tipo <see cref="decimal"/> y puede ser nulo.
    /// Si se establece en null, no habrá un límite superior.
    /// </remarks>
    /// <value>
    /// Un valor decimal que representa el límite superior, o null si no hay límite.
    /// </value>
    public decimal? MaxValue { get; set; }
}