namespace Util.Microservices.Dapr.StateManagements.Queries.Conditions; 

/// <summary>
/// Representa una condición lógica que evalúa si al menos una de las condiciones especificadas es verdadera.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IStateCondition"/> y permite combinar múltiples condiciones
/// utilizando la operación lógica OR. Si al menos una de las condiciones es verdadera, la condición general se
/// considera verdadera.
/// </remarks>
public class OrCondition : IStateCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrCondition"/>.
    /// </summary>
    /// <param name="condition1">La primera condición de estado que se agregará. No puede ser <c>null</c>.</param>
    /// <param name="condition2">La segunda condición de estado que se agregará. Puede ser <c>null</c>.</param>
    /// <remarks>
    /// Si ambas condiciones son <c>null</c>, no se realiza ninguna acción.
    /// </remarks>
    public OrCondition( IStateCondition condition1, IStateCondition condition2 = null ) {
        if ( condition1 == null && condition2 == null )
            return;
        Or = new List<object>();
        AddCondition( condition1 );
        AddCondition( condition2 );
    }

    /// <summary>
    /// Representa una colección de objetos que se utilizan en una operación lógica "OR".
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar múltiples elementos que se evaluarán en una operación lógica "OR".
    /// </remarks>
    /// <returns>
    /// Una lista de objetos que representan los elementos de la operación "OR".
    /// </returns>
    [JsonPropertyName("OR")]
    public List<object> Or { get; set; }

    /// <summary>
    /// Agrega una condición al conjunto de condiciones.
    /// </summary>
    /// <param name="condition">La condición que se desea agregar. Si es nula, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Si la condición es del tipo <see cref="OrCondition"/>, se agregarán todas las condiciones contenidas en ella.
    /// De lo contrario, se agregará la condición directamente al conjunto.
    /// </remarks>
    /// <seealso cref="OrCondition"/>
    public void AddCondition( IStateCondition condition ) {
        if ( condition == null )
            return;
        if ( condition is OrCondition orCondition ) {
            foreach ( var item in orCondition.Or ) {
                Or.Add( item );
            }
            return;
        }
        Or.Add( condition );
    }
}