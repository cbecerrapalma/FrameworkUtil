using Util.Data.Queries;
using Util.Data.Sql.Builders.Conditions;
using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IConditionFactory"/> para crear condiciones SQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos para generar condiciones SQL que pueden ser utilizadas en consultas.
/// </remarks>
public class SqlConditionFactory : IConditionFactory {
    private readonly IParameterManager _parameterManager;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlConditionFactory"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros SQL.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="parameterManager"/> es <c>null</c>.</exception>
    public SqlConditionFactory( IParameterManager parameterManager ) {
        _parameterManager = parameterManager ?? throw new ArgumentNullException( nameof( parameterManager ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una condición SQL basada en el operador y el valor proporcionados.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplica la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición.</param>
    /// <param name="operator">El operador que define el tipo de comparación a realizar.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta SQL.</param>
    /// <returns>Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL creada.</returns>
    /// <remarks>
    /// Este método evalúa el operador proporcionado y crea la condición SQL correspondiente.
    /// Se manejan operadores como igual, no igual, mayor que, menor que, y otros.
    /// En caso de que el operador no esté implementado, se lanzará una excepción <see cref="NotImplementedException"/>.
    /// </remarks>
    public virtual ISqlCondition Create( string column, object value, Operator @operator, bool isParameterization = true ) {
        if( IsInCondition( @operator, value ) )
            return new InCondition( _parameterManager, column, value, isParameterization );
        if( IsNotInCondition( @operator, value ) )
            return new NotInCondition( _parameterManager, column, value, isParameterization );
        switch( @operator ) {
            case Operator.Equal:
                return new EqualCondition( _parameterManager, column, value, isParameterization );
            case Operator.NotEqual:
                return new NotEqualCondition( _parameterManager, column, value, isParameterization );
            case Operator.Greater:
                return new GreaterCondition( _parameterManager, column, value, isParameterization );
            case Operator.GreaterEqual:
                return new GreaterEqualCondition( _parameterManager, column, value, isParameterization );
            case Operator.Less:
                return new LessCondition( _parameterManager, column, value, isParameterization );
            case Operator.LessEqual:
                return new LessEqualCondition( _parameterManager, column, value, isParameterization );
            case Operator.Contains:
                return new ContainsCondition( _parameterManager, column, value, isParameterization );
            case Operator.Starts:
                return new StartsCondition( _parameterManager, column, value, isParameterization );
            case Operator.Ends:
                return new EndsCondition( _parameterManager, column, value, isParameterization );
            default:
                throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Determina si un operador específico está en la condición "In".
    /// </summary>
    /// <param name="operator">El operador que se va a evaluar.</param>
    /// <param name="value">El valor que se está comparando (no se utiliza en la lógica actual).</param>
    /// <returns>
    /// Devuelve <c>true</c> si el operador es "In"; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    private bool IsInCondition( Operator @operator, object value ) {
        if( @operator == Operator.In )
            return true;
        return false;
    }

    /// <summary>
    /// Determina si un operador no está en una condición específica.
    /// </summary>
    /// <param name="@operator">El operador que se va a evaluar.</param>
    /// <param name="value">El valor que se está comparando (no se utiliza en la lógica actual).</param>
    /// <returns>
    /// Devuelve <c>true</c> si el operador es <c>Operator.NotIn</c>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    private bool IsNotInCondition( Operator @operator, object value ) {
        if( @operator == Operator.NotIn )
            return true;
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una condición SQL basada en un rango de valores para una columna específica.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la condición.</param>
    /// <param name="minValue">El valor mínimo del rango.</param>
    /// <param name="maxValue">El valor máximo del rango.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta SQL. Por defecto es true.</param>
    /// <returns>Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL creada.</returns>
    /// <remarks>
    /// Este método permite crear condiciones que pueden ser utilizadas para filtrar resultados en consultas SQL.
    /// Se puede especificar si se desea utilizar la parametrización, lo que puede ayudar a prevenir inyecciones SQL.
    /// </remarks>
    /// <seealso cref="ISqlCondition"/>
    /// <seealso cref="SegmentCondition"/>
    public virtual ISqlCondition Create( string column, object minValue, object maxValue, Boundary boundary, bool isParameterization = true ) {
        return new SegmentCondition( _parameterManager, column, minValue, maxValue, boundary, isParameterization );
    }
}