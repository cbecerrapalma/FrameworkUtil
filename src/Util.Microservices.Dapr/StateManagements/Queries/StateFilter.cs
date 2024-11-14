using Util.Microservices.Dapr.StateManagements.Queries.Conditions;

namespace Util.Microservices.Dapr.StateManagements.Queries;

/// <summary>
/// Representa un filtro para estados.
/// </summary>
public class StateFilter {
    private IStateCondition _condition;

    /// <summary>
    /// Obtiene el valor de la condición.
    /// </summary>
    /// <returns>
    /// Un objeto que representa la condición actual.
    /// </returns>
    public object GetCondition() {
        return _condition;
    }

    /// <summary>
    /// Restablece el filtro de estado, eliminando cualquier condición existente.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="StateFilter"/> para permitir la encadenación de métodos.
    /// </returns>
    public StateFilter Clear() {
        _condition = null;
        return this;
    }

    /// <summary>
    /// Establece una condición de igualdad para un filtro de estado.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a comparar.</param>
    /// <param name="value">El valor que se va a comparar con la propiedad.</param>
    /// <returns>El filtro de estado actualizado con la nueva condición de igualdad.</returns>
    public StateFilter Equal( string property, object value ) {
        _condition = _condition.And( new EqualCondition( property, value ) );
        return this;
    }

    /// <summary>
    /// Agrega una condición que verifica si el valor de una propiedad está dentro de un conjunto de valores especificado.
    /// </summary>
    /// <param name="property">El nombre de la propiedad que se va a evaluar.</param>
    /// <param name="values">Una colección de valores que se utilizarán para la comparación.</param>
    /// <returns>Una instancia del filtro de estado actual con la nueva condición aplicada.</returns>
    public StateFilter In( string property, IEnumerable<object> values ) {
        _condition = _condition.And( new InCondition( property, values ) );
        return this;
    }

    /// <summary>
    /// Combina la condición actual con otra condición utilizando una operación lógica "Y".
    /// </summary>
    /// <param name="condition">La condición que se desea combinar con la condición actual.</param>
    /// <returns>Devuelve la instancia actual de <see cref="StateFilter"/> con la nueva condición combinada.</returns>
    public StateFilter And( IStateCondition condition ) {
        _condition = _condition.And( condition );
        return this;
    }

    /// <summary>
    /// Combina la condición actual con otra condición utilizando una operación lógica OR.
    /// </summary>
    /// <param name="condition">La condición que se desea combinar con la condición actual.</param>
    /// <returns>Una instancia de <see cref="StateFilter"/> que representa la combinación de condiciones.</returns>
    public StateFilter Or( IStateCondition condition ) {
        _condition = _condition.Or( condition );
        return this;
    }

    /// <summary>
    /// Compara el valor de una propiedad especificada con un valor dado para determinar la igualdad.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que contiene la propiedad a comparar.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad a comparar.</param>
    /// <param name="value">El valor con el que se comparará la propiedad.</param>
    /// <returns>Un objeto de tipo <see cref="StateFilter"/> que representa el resultado de la comparación.</returns>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para obtener el nombre de la propiedad y luego realiza
    /// la comparación con el valor especificado.
    /// </remarks>
    /// <seealso cref="StateQueryHelper.GetProperty"/>
    public StateFilter Equal<T>( Expression<Func<T, object>> expression, object value ) {
        return Equal( StateQueryHelper.GetProperty( expression ), value );
    }

    /// <summary>
    /// Filtra los estados en función de una propiedad específica y un conjunto de valores.
    /// </summary>
    /// <typeparam name="T">El tipo de la entidad que contiene la propiedad a filtrar.</typeparam>
    /// <param name="expression">Una expresión que representa la propiedad de la entidad que se va a filtrar.</param>
    /// <param name="values">Una colección de valores que se utilizarán para filtrar los estados.</param>
    /// <returns>Un objeto <see cref="StateFilter"/> que representa el filtro aplicado.</returns>
    /// <remarks>
    /// Este método permite aplicar un filtro a los estados basándose en los valores proporcionados 
    /// para una propiedad específica de la entidad. La expresión debe ser una función que 
    /// seleccione la propiedad de interés.
    /// </remarks>
    /// <seealso cref="StateQueryHelper.GetProperty(Expression{Func{T, object}})"/>
    public StateFilter In<T>( Expression<Func<T, object>> expression, IEnumerable<object> values ) {
        return In( StateQueryHelper.GetProperty( expression ), values );
    }
}