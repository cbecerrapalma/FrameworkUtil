namespace Util.Microservices.Dapr.StateManagements.Queries.Conditions;

/// <summary>
/// Representa una condición de estado que verifica si un objeto se encuentra dentro de un conjunto específico de condiciones.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStateCondition"/> y proporciona la lógica necesaria para evaluar
/// si un estado particular se cumple en función de las condiciones definidas.
/// </remarks>
public class InCondition : IStateCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="InCondition"/>.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se utilizará en la condición.</param>
    /// <param name="values">Una colección de valores que se compararán con la propiedad.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="property"/> está vacío, no se inicializará la condición.
    /// </remarks>
    public InCondition( string property, IEnumerable<object> values ) {
        if ( property.IsEmpty() )
            return;
        In = new Dictionary<string, IEnumerable<object>> { { property, values } };
    }

    /// <summary>
    /// Representa un conjunto de entradas donde cada clave está asociada a una colección de objetos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar datos en formato de diccionario, donde la clave es una cadena y el valor es una colección de objetos.
    /// </remarks>
    /// <returns>
    /// Un diccionario que contiene las entradas, donde la clave es de tipo <see cref="string"/> y el valor es de tipo <see cref="IEnumerable{object}"/>.
    /// </returns>
    [JsonPropertyName("IN")]
    public Dictionary<string, IEnumerable<object>> In { get; set; }
}