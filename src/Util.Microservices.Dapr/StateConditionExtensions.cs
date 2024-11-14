using Util.Microservices.Dapr.StateManagements.Queries.Conditions;

namespace Util.Microservices.Dapr;

/// <summary>
/// Proporciona métodos de extensión para trabajar con condiciones de estado.
/// </summary>
public static class StateConditionExtensions {
    /// <summary>
    /// Combina dos condiciones de estado en una condición de estado "And".
    /// </summary>
    /// <param name="source">La condición de estado original a la que se le añadirá la nueva condición.</param>
    /// <param name="condition">La nueva condición de estado que se desea combinar.</param>
    /// <returns>
    /// Una nueva condición de estado que representa la combinación de ambas condiciones.
    /// Si <paramref name="condition"/> es <c>null</c>, se devuelve <paramref name="source"/>.
    /// Si <paramref name="source"/> es <c>null</c>, se devuelve <paramref name="condition"/>.
    /// </returns>
    /// <remarks>
    /// Este método permite crear una condición compuesta que solo se cumplirá si ambas condiciones individuales se cumplen.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    /// <seealso cref="AndCondition"/>
    public static IStateCondition And( this IStateCondition source, IStateCondition condition ) {
        if ( condition == null )
            return source;
        if ( source == null )
            return condition;
        if ( source is AndCondition andCondition ) {
            andCondition.AddCondition( condition );
            return andCondition;
        }
        if ( condition is AndCondition andCondition2 ) {
            andCondition2.AddCondition( source );
            return andCondition2;
        }
        return new AndCondition( source, condition );
    }

    /// <summary>
    /// Combina dos condiciones de estado utilizando una operación lógica OR.
    /// </summary>
    /// <param name="source">La condición de estado original a la que se le añadirá la nueva condición.</param>
    /// <param name="condition">La nueva condición de estado que se combinará con la condición original.</param>
    /// <returns>
    /// Una nueva condición de estado que representa la combinación de ambas condiciones. 
    /// Si <paramref name="condition"/> es <c>null</c>, se devuelve <paramref name="source"/>. 
    /// Si <paramref name="source"/> es <c>null</c>, se devuelve <paramref name="condition"/>. 
    /// Si ambas condiciones son del tipo <see cref="OrCondition"/>, se combinan en una sola instancia de <see cref="OrCondition"/>.
    /// </returns>
    /// <remarks>
    /// Este método permite la creación de condiciones compuestas, facilitando la evaluación de múltiples condiciones de estado 
    /// en un solo paso. Es útil en escenarios donde se requiere evaluar si al menos una de varias condiciones es verdadera.
    /// </remarks>
    /// <seealso cref="IStateCondition"/>
    /// <seealso cref="OrCondition"/>
    public static IStateCondition Or( this IStateCondition source, IStateCondition condition ) {
        if ( condition == null )
            return source;
        if ( source == null )
            return condition;
        if ( source is OrCondition orCondition ) {
            orCondition.AddCondition( condition );
            return orCondition;
        }
        if ( condition is OrCondition orCondition2 ) {
            orCondition2.AddCondition( source );
            return orCondition2;
        }
        return new OrCondition( source, condition );
    }
}