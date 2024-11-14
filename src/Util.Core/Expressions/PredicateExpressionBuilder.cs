using Util.Data;
using Util.Helpers;

namespace Util.Expressions; 

/// <summary>
/// Clase que permite construir expresiones de predicado para filtrar entidades.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la cual se construyen las expresiones de predicado.</typeparam>
public class PredicateExpressionBuilder<TEntity> {
    private readonly ParameterExpression _parameter;
    private Expression _result;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PredicateExpressionBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea un nuevo parámetro de tipo <typeparamref name="TEntity"/> 
    /// utilizando el método <see cref="Lambda.CreateParameter{T}"/>.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de entidad para el cual se construirá la expresión de predicado.
    /// </typeparam>
    public PredicateExpressionBuilder() {
        _parameter = Lambda.CreateParameter<TEntity>();
    }

    /// <summary>
    /// Obtiene el parámetro asociado.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ParameterExpression"/> que representa el parámetro.
    /// </returns>
    public ParameterExpression GetParameter() {
        return _parameter;
    }

    /// <summary>
    /// Agrega una condición a la consulta actual utilizando el operador especificado.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <param name="property">Una expresión que representa la propiedad de la entidad.</param>
    /// <param name="@operator">El operador que se aplicará a la condición.</param>
    /// <param name="value">El valor que se comparará con la propiedad especificada.</param>
    /// <remarks>
    /// Este método permite construir consultas dinámicas al agregar condiciones basadas en expresiones lambda.
    /// </remarks>
    /// <seealso cref="TEntity"/>
    public void Append<TProperty>( Expression<Func<TEntity, TProperty>> property, Operator @operator, object value ) {
        _result = _result.And( _parameter.Property( Lambda.GetMember( property ) ).Operation( @operator, value ) );
    }

    /// <summary>
    /// Agrega una operación a la expresión actual utilizando el operador especificado y el valor dado.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está manipulando.</typeparam>
    /// <param name="property">Una expresión que representa la propiedad de la entidad.</param>
    /// <param name="@operator">El operador que se aplicará a la propiedad.</param>
    /// <param name="value">La expresión que representa el valor a comparar con la propiedad.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente consultas mediante la combinación de operaciones sobre propiedades de una entidad.
    /// </remarks>
    /// <seealso cref="TEntity"/>
    public void Append<TProperty>( Expression<Func<TEntity, TProperty>> property, Operator @operator, Expression value ) {
        _result = _result.And( _parameter.Property( Lambda.GetMember( property ) ).Operation( @operator, value ) );
    }

    /// <summary>
    /// Agrega una operación lógica a la consulta actual utilizando el operador especificado.
    /// </summary>
    /// <param name="property">El nombre de la propiedad sobre la cual se realizará la operación.</param>
    /// <param name="@operator">El operador que se aplicará a la propiedad.</param>
    /// <param name="value">El valor que se utilizará en la operación.</param>
    /// <remarks>
    /// Este método permite construir dinámicamente condiciones en una consulta, 
    /// facilitando la creación de filtros complejos mediante la combinación de 
    /// múltiples operaciones lógicas.
    /// </remarks>
    public void Append( string property, Operator @operator, object value ) {
        _result = _result.And( _parameter.Property( property ).Operation( @operator, value ) );
    }

    /// <summary>
    /// Agrega una operación lógica a la expresión actual utilizando un operador específico y un valor.
    /// </summary>
    /// <param name="property">El nombre de la propiedad sobre la cual se realizará la operación.</param>
    /// <param name="operator">El operador lógico que se aplicará a la expresión.</param>
    /// <param name="value">El valor que se utilizará en la operación.</param>
    public void Append( string property, Operator @operator, Expression value ) {
        _result = _result.And( _parameter.Property( property ).Operation( @operator, value ) );
    }

    /// <summary>
    /// Limpia el resultado actual estableciendo su valor a null.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para reiniciar el estado del resultado, 
    /// permitiendo que se pueda calcular un nuevo resultado sin interferencias 
    /// de resultados anteriores.
    /// </remarks>
    public void Clear() {
        _result = null;
    }

    /// <summary>
    /// Convierte el resultado almacenado en una expresión lambda que representa una función 
    /// que toma un parámetro de tipo <typeparamref name="TEntity"/> y devuelve un valor booleano.
    /// </summary>
    /// <returns>
    /// Una expresión lambda de tipo <see cref="Expression{Func{TEntity, bool}}"/> 
    /// que representa la condición definida por el resultado.
    /// </returns>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se utilizará en la expresión lambda.
    /// </typeparam>
    public Expression<Func<TEntity, bool>> ToLambda() {
        return _result.ToLambda<Func<TEntity, bool>>( _parameter );
    }
}