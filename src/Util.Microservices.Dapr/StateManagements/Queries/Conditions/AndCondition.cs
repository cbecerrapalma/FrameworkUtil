namespace Util.Microservices.Dapr.StateManagements.Queries.Conditions; 

/// <summary>
/// Representa una condición que se evalúa como verdadera solo si todas las condiciones
/// subyacentes son verdaderas.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStateCondition"/> y permite combinar
/// múltiples condiciones utilizando una lógica AND.
/// </remarks>
public class AndCondition : IStateCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AndCondition"/>.
    /// </summary>
    /// <param name="condition1">La primera condición de estado que se va a agregar. No puede ser <c>null</c>.</param>
    /// <param name="condition2">La segunda condición de estado que se va a agregar. Puede ser <c>null</c>.</param>
    /// <remarks>
    /// Si ambas condiciones son <c>null</c>, no se inicializa la lista de condiciones.
    /// </remarks>
    public AndCondition( IStateCondition condition1, IStateCondition condition2 = null ) {
        if ( condition1 == null && condition2 == null )
            return;
        And = new List<object>();
        AddCondition( condition1 );
        AddCondition( condition2 );
    }

    /// <summary>
    /// Representa una colección de condiciones que se deben cumplir en una operación lógica "AND".
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar una lista de objetos que representan las condiciones 
    /// que deben evaluarse como verdaderas para que la operación general sea considerada verdadera.
    /// </remarks>
    /// <returns>
    /// Una lista de objetos que representan las condiciones "AND".
    /// </returns>
    [JsonPropertyName( "AND" )]
    public List<object> And { get; set; }

    /// <summary>
    /// Agrega una condición al conjunto de condiciones.
    /// </summary>
    /// <param name="condition">La condición que se desea agregar. Si es nula, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Si la condición es de tipo <see cref="AndCondition"/>, se agregarán todas las condiciones contenidas en ella.
    /// De lo contrario, se agregará la condición directamente al conjunto de condiciones.
    /// </remarks>
    /// <seealso cref="AndCondition"/>
    public void AddCondition( IStateCondition condition ) {
        if ( condition == null )
            return;
        if ( condition is AndCondition andCondition ) {
            foreach ( var item in andCondition.And ) {
                And.Add( item );
            }
            return;
        }
        And.Add( condition );
    }
}