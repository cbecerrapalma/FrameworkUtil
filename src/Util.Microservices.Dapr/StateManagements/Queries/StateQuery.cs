namespace Util.Microservices.Dapr.StateManagements.Queries;

/// <summary>
/// Representa una consulta de estado en el sistema.
/// </summary>
public class StateQuery {
    /// <summary>
    /// Representa un filtro que se puede aplicar a una consulta o conjunto de datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar un objeto que define los criterios de filtrado.
    /// El tipo de objeto es dinámico, lo que permite flexibilidad en la definición del filtro.
    /// </remarks>
    /// <returns>
    /// Un objeto que representa el filtro aplicado.
    /// </returns>
    [JsonPropertyName("filter")]
    public object Filter { get; set; }
    /// <summary>
    /// Representa el criterio de ordenamiento para un estado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para especificar cómo se deben ordenar los estados en una colección.
    /// </remarks>
    /// <returns>
    /// Un objeto de tipo <see cref="StateSort"/> que indica el criterio de ordenamiento.
    /// </returns>
    [JsonPropertyName( "sort" )]
    public StateSort Sort { get; set; }
    /// <summary>
    /// Representa la página de estado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad contiene información sobre la página actual del estado.
    /// </remarks>
    /// <returns>
    /// Un objeto de tipo <see cref="StatePage"/> que representa la página de estado.
    /// </returns>
    [JsonPropertyName( "page" )]
    public StatePage Page { get; set; }
}