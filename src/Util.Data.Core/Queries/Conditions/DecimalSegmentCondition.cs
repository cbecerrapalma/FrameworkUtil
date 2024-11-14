using Util.Data.Queries.Conditions.Internal;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición de segmento que opera sobre propiedades de tipo decimal.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplica la condición.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="SegmentConditionBase{TEntity, TProperty, decimal}"/> 
/// y proporciona funcionalidades específicas para trabajar con valores decimales en condiciones de segmento.
/// </remarks>
public class DecimalSegmentCondition<TEntity, TProperty> : SegmentConditionBase<TEntity, TProperty, decimal> where TEntity : class {
    private readonly DecimalQuery _query;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DecimalSegmentCondition"/>.
    /// </summary>
    /// <param name="propertyExpression">Una expresión que representa la propiedad del tipo <typeparamref name="TEntity"/> a evaluar.</param>
    /// <param name="min">El valor mínimo del segmento decimal, o <c>null</c> si no se establece un límite inferior.</param>
    /// <param name="max">El valor máximo del segmento decimal, o <c>null</c> si no se establece un límite superior.</param>
    /// <param name="boundary">Especifica los límites del segmento, que pueden ser <see cref="Boundary.Both"/>, <see cref="Boundary.Left"/>, <see cref="Boundary.Right"/> o <see cref="Boundary.None"/>.</param>
    /// <remarks>
    /// Esta clase se utiliza para definir condiciones de segmento en base a valores decimales, permitiendo establecer límites superior e inferior.
    /// </remarks>
    public DecimalSegmentCondition( Expression<Func<TEntity, TProperty>> propertyExpression, decimal? min, decimal? max, Boundary boundary = Boundary.Both )
        : base( propertyExpression, min, max, boundary ) {
        _query = new DecimalQuery();
    }

    /// <summary>
    /// Determina si el valor mínimo es mayor que el valor máximo.
    /// </summary>
    /// <param name="min">El valor mínimo a comparar.</param>
    /// <param name="max">El valor máximo a comparar.</param>
    /// <returns>
    /// Devuelve true si el valor mínimo es mayor que el valor máximo; de lo contrario, devuelve false.
    /// </returns>
    protected override bool IsMinGreaterMax(decimal? min, decimal? max) {
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
    /// Una expresión que accede a la propiedad <c>MaxValue</c> del objeto <c>_query</c>.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que establece el valor máximo en la consulta antes de devolver la expresión.
    /// </remarks>
    protected override Expression GetMaxValueExpression() {
        _query.MaxValue = GetMaxValue();
        return Expression.Property(Expression.Constant(_query), "MaxValue");
    }
}