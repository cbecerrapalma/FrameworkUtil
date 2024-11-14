using Util.Data.Queries.Conditions.Internal;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición de segmento que opera sobre propiedades de tipo <see cref="double"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la cual se aplica la condición.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad que se evalúa en la condición.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="SegmentConditionBase{TEntity, TProperty, T}"/> y proporciona funcionalidades específicas
/// para trabajar con valores de tipo <see cref="double"/> en condiciones de segmento.
/// </remarks>
public class DoubleSegmentCondition<TEntity, TProperty> : SegmentConditionBase<TEntity, TProperty, double> where TEntity : class {
    private readonly DoubleQuery _query;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DoubleSegmentCondition"/>.
    /// </summary>
    /// <param name="propertyExpression">Una expresión que representa la propiedad del tipo <typeparamref name="TEntity"/> que se va a evaluar.</param>
    /// <param name="min">El valor mínimo del rango, o <c>null</c> si no se establece un límite inferior.</param>
    /// <param name="max">El valor máximo del rango, o <c>null</c> si no se establece un límite superior.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites en la condición. Por defecto es <see cref="Boundary.Both"/>.</param>
    /// <remarks>
    /// Esta clase se utiliza para definir una condición de segmento en la que se evalúa si un valor de propiedad se encuentra dentro de un rango específico.
    /// </remarks>
    public DoubleSegmentCondition( Expression<Func<TEntity, TProperty>> propertyExpression, double? min, double? max, Boundary boundary = Boundary.Both )
        : base( propertyExpression, min, max, boundary ) {
        _query = new DoubleQuery();
    }

    /// <summary>
    /// Determina si el valor mínimo es mayor que el valor máximo.
    /// </summary>
    /// <param name="min">El valor mínimo a comparar.</param>
    /// <param name="max">El valor máximo a comparar.</param>
    /// <returns>
    /// Devuelve true si el valor mínimo es mayor que el valor máximo; de lo contrario, devuelve false.
    /// </returns>
    protected override bool IsMinGreaterMax(double? min, double? max)
    {
        return min > max;
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor mínimo de la consulta.
    /// </summary>
    /// <returns>
    /// Una expresión que accede a la propiedad <c>MinValue</c> de la consulta actual.
    /// </returns>
    protected override Expression GetMinValueExpression() {
        _query.MinValue = GetMinValue();
        return Expression.Property(Expression.Constant(_query), "MinValue");
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor máximo de la consulta.
    /// </summary>
    /// <returns>
    /// Una expresión que accede a la propiedad <c>MaxValue</c> de la consulta.
    /// </returns>
    protected override Expression GetMaxValueExpression() 
    { 
        _query.MaxValue = GetMaxValue(); 
        return Expression.Property(Expression.Constant(_query), "MaxValue"); 
    }
}