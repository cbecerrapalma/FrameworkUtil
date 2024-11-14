namespace Util.Microservices.Dapr.StateManagements.Queries.Conditions;

/// <summary>
/// Representa una condición que verifica la igualdad entre dos valores.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStateCondition"/> y proporciona
/// la lógica para evaluar si dos valores son iguales.
/// </remarks>
public class EqualCondition : IStateCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EqualCondition"/>.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor que se va a utilizar para la comparación.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="property"/> está vacío, no se inicializa el diccionario <c>Equal</c>.
    /// </remarks>
    public EqualCondition( string property, object value ) {
        if ( property.IsEmpty() )
            return;
        Equal = new Dictionary<string, object> { { property, value } };
    }

    /// <summary>
    /// Representa un diccionario que almacena pares de clave-valor donde la clave es una cadena 
    /// y el valor es un objeto. Este diccionario se utiliza para almacenar datos relacionados 
    /// con la igualdad en un formato serializable a JSON.
    /// </summary>
    /// <remarks>
    /// La propiedad <see cref="Equal"/> está decorada con el atributo <see cref="JsonPropertyName"/> 
    /// para especificar el nombre de la propiedad en el JSON resultante.
    /// </remarks>
    /// <returns>
    /// Un diccionario que contiene los pares de clave-valor relacionados con la igualdad.
    /// </returns>
    [JsonPropertyName("EQ")]
    public Dictionary<string, object> Equal { get; set; }
}