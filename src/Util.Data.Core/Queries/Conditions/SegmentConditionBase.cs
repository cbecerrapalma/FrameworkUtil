using Util.Expressions;
using Util.Helpers;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Clase base abstracta que representa una condición de segmento.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplica la condición.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad de la entidad que se evaluará.</typeparam>
/// <typeparam name="TValue">El tipo del valor que se utilizará en la condición.</typeparam>
public abstract class SegmentConditionBase<TEntity, TProperty, TValue> : ICondition<TEntity>
    where TEntity : class
    where TValue : struct {
    private readonly Expression<Func<TEntity, TProperty>> _propertyExpression;
    private readonly PredicateExpressionBuilder<TEntity> _builder;
    private TValue? _min;
    private TValue? _max;
    private readonly Boundary _boundary;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SegmentConditionBase"/>.
    /// </summary>
    /// <param name="propertyExpression">La expresión que representa la propiedad del tipo <typeparamref name="TEntity"/> a evaluar.</param>
    /// <param name="min">El valor mínimo permitido para la propiedad, o <c>null</c> si no se establece un límite inferior.</param>
    /// <param name="max">El valor máximo permitido para la propiedad, o <c>null</c> si no se establece un límite superior.</param>
    /// <param name="boundary">El tipo de límite que se aplicará a los valores mínimo y máximo.</param>
    /// <remarks>
    /// Este constructor se utiliza para crear condiciones de segmento que se pueden aplicar a consultas sobre entidades de tipo <typeparamref name="TEntity"/>.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad sobre la que se aplicará la condición.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <typeparam name="TValue">El tipo de los valores mínimo y máximo.</typeparam>
    /// <seealso cref="PredicateExpressionBuilder{TEntity}"/>
    protected SegmentConditionBase( Expression<Func<TEntity, TProperty>> propertyExpression, TValue? min, TValue? max, Boundary boundary ) {
        _builder = new PredicateExpressionBuilder<TEntity>();
        _propertyExpression = propertyExpression;
        _min = min;
        _max = max;
        _boundary = boundary;
    }

    /// <summary>
    /// Obtiene el tipo de la propiedad a partir de la expresión lambda proporcionada.
    /// </summary>
    /// <returns>
    /// El tipo de la propiedad representado por la expresión lambda.
    /// </returns>
    protected Type GetPropertyType() 
    { 
        return Lambda.GetType(_propertyExpression); 
    }

    /// <summary>
    /// Obtiene el límite asociado.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <see cref="Boundary"/> que representa el límite.
    /// </returns>
    protected Boundary GetBoundary() {
        return _boundary;
    }

    /// <summary>
    /// Obtiene una expresión que representa una condición para filtrar entidades.
    /// </summary>
    /// <returns>
    /// Una expresión lambda que toma un parámetro de tipo <typeparamref name="TEntity"/> 
    /// y devuelve un valor booleano que indica si la entidad cumple con la condición.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un constructor de expresiones para construir dinámicamente 
    /// la condición basada en los valores mínimos y máximos ajustados.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se está filtrando.
    /// </typeparam>
    public Expression<Func<TEntity, bool>> GetCondition() {
        _builder.Clear();
        Adjust( _min, _max );
        CreateLeftExpression();
        CreateRightExpression();
        return _builder.ToLambda();
    }

    /// <summary>
    /// Ajusta los valores mínimo y máximo si el mínimo no es mayor que el máximo.
    /// </summary>
    /// <param name="min">El nuevo valor mínimo a establecer.</param>
    /// <param name="max">El nuevo valor máximo a establecer.</param>
    /// <remarks>
    /// Este método verifica si el valor mínimo proporcionado es mayor que el valor máximo.
    /// Si no lo es, no se realizan cambios en los valores de _min y _max.
    /// </remarks>
    private void Adjust( TValue? min, TValue? max ) {
        if( IsMinGreaterMax( min, max ) == false )
            return;
        _min = max;
        _max = min;
    }

    /// <summary>
    /// Determina si el valor mínimo es mayor que el valor máximo.
    /// </summary>
    /// <param name="min">El valor mínimo a comparar. Puede ser nulo.</param>
    /// <param name="max">El valor máximo a comparar. Puede ser nulo.</param>
    /// <returns>
    /// Devuelve verdadero si el valor mínimo es mayor que el valor máximo; de lo contrario, devuelve falso.
    /// </returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// La implementación debe considerar los casos en los que los parámetros son nulos.
    /// </remarks>
    /// <typeparam name="TValue">El tipo de los valores a comparar.</typeparam>
    protected abstract bool IsMinGreaterMax(TValue? min, TValue? max);

    /// <summary>
    /// Crea una expresión para el lado izquierdo de una operación.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el valor mínimo es nulo. Si no lo es, 
    /// obtiene la expresión del valor mínimo, la convierte al tipo de propiedad 
    /// correspondiente y luego la agrega al constructor de expresiones.
    /// </remarks>
    private void CreateLeftExpression() {
        if( _min == null )
            return;
        var minValueExpression = GetMinValueExpression();
        var expression = Expression.Convert( minValueExpression, GetPropertyType() );
        _builder.Append( _propertyExpression, CreateLeftOperator( _boundary ), expression );
    }

    /// <summary>
    /// Crea un operador para el límite izquierdo basado en el tipo de límite especificado.
    /// </summary>
    /// <param name="boundary">El límite que determina el operador a crear.</param>
    /// <returns>
    /// Un operador que representa la comparación adecuada para el límite izquierdo.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación específica.
    /// </remarks>
    /// <seealso cref="Boundary"/>
    /// <seealso cref="Operator"/>
    protected virtual Operator CreateLeftOperator( Boundary? boundary ) {
        switch( boundary ) {
            case Boundary.Left:
                return Operator.GreaterEqual;
            case Boundary.Both:
                return Operator.GreaterEqual;
            default:
                return Operator.Greater;
        }
    }

    /// <summary>
    /// Obtiene el valor mínimo almacenado.
    /// </summary>
    /// <returns>
    /// El valor mínimo de tipo <typeparamref name="TValue"/> si existe; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder al valor mínimo que se ha almacenado en la colección o estructura de datos.
    /// Si no se ha establecido un valor mínimo, el método retornará null.
    /// </remarks>
    protected TValue? GetMinValue() {
        return _min;
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor mínimo.
    /// </summary>
    /// <returns>
    /// Una expresión que contiene el valor mínimo definido.
    /// </returns>
    protected virtual Expression GetMinValueExpression() {
        return Lambda.Constant( _min, _propertyExpression );
    }

    /// <summary>
    /// Crea una expresión de la parte derecha de una operación basada en un límite.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el valor máximo (_max) es nulo. Si no lo es, 
    /// genera una expresión que representa el valor máximo convertido al tipo 
    /// de propiedad correspondiente y lo agrega a un constructor de expresiones.
    /// </remarks>
    private void CreateRightExpression() {
        if( _max == null )
            return;
        var maxValueExpression = GetMaxValueExpression();
        var expression = Expression.Convert( maxValueExpression, GetPropertyType() );
        _builder.Append( _propertyExpression, CreateRightOperator( _boundary ), expression );
    }

    /// <summary>
    /// Crea un operador derecho basado en el límite especificado.
    /// </summary>
    /// <param name="boundary">El límite que determina el operador a crear.</param>
    /// <returns>
    /// Un operador que representa la relación adecuada según el límite.
    /// </returns>
    /// <remarks>
    /// Este método devuelve un operador que se utiliza para comparar valores en función del límite.
    /// Si el límite es 'Right' o 'Both', se devuelve el operador 'LessEqual'. 
    /// En caso contrario, se devuelve 'Less'.
    /// </remarks>
    protected virtual Operator CreateRightOperator(Boundary? boundary) {
        switch (boundary) {
            case Boundary.Right:
                return Operator.LessEqual;
            case Boundary.Both:
                return Operator.LessEqual;
            default:
                return Operator.Less;
        }
    }

    /// <summary>
    /// Obtiene el valor máximo almacenado.
    /// </summary>
    /// <returns>
    /// El valor máximo de tipo <typeparamref name="TValue"/> si está disponible; de lo contrario, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método permite acceder al valor máximo que se ha almacenado en la instancia actual.
    /// Si no se ha establecido un valor máximo, se retornará null.
    /// </remarks>
    protected TValue? GetMaxValue() {
        return _max;
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor máximo.
    /// </summary>
    /// <returns>
    /// Una expresión que contiene el valor máximo definido por el campo <c>_max</c>
    /// y la expresión de propiedad <c>_propertyExpression</c>.
    /// </returns>
    protected virtual Expression GetMaxValueExpression() {
        return Lambda.Constant( _max, _propertyExpression );
    }
}