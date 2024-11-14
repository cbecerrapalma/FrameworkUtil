using Util.Data.Queries.Conditions.Internal;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición de segmento que utiliza un valor entero como criterio.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplica la condición.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad que se evaluará en la condición.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="SegmentConditionBase{TEntity, TProperty, int}"/> y proporciona
/// funcionalidades específicas para trabajar con propiedades de tipo entero.
/// </remarks>
public class IntSegmentCondition<TEntity, TProperty> : SegmentConditionBase<TEntity, TProperty, int> where TEntity : class {
    private readonly IntQuery _query;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IntSegmentCondition"/>.
    /// </summary>
    /// <param name="propertyExpression">La expresión que representa la propiedad del tipo <typeparamref name="TEntity"/> que se va a evaluar.</param>
    /// <param name="min">El valor mínimo del segmento, o <c>null</c> si no se establece un límite inferior.</param>
    /// <param name="max">El valor máximo del segmento, o <c>null</c> si no se establece un límite superior.</param>
    /// <param name="boundary">Los límites del segmento que determinan si se incluyen los valores mínimo y máximo. Por defecto es <see cref="Boundary.Both"/>.</param>
    /// <remarks>
    /// Esta clase se utiliza para definir condiciones de segmento basadas en valores enteros.
    /// </remarks>
    public IntSegmentCondition( Expression<Func<TEntity, TProperty>> propertyExpression, int? min, int? max, Boundary boundary = Boundary.Both )
        : base( propertyExpression,min,max, boundary ) {
        _query = new IntQuery();
    }

    /// <summary>
    /// Determina si el valor mínimo es mayor que el valor máximo.
    /// </summary>
    /// <param name="min">El valor mínimo a comparar, puede ser nulo.</param>
    /// <param name="max">El valor máximo a comparar, puede ser nulo.</param>
    /// <returns>
    /// Devuelve true si el valor mínimo es mayor que el valor máximo; de lo contrario, devuelve false.
    /// </returns>
    protected override bool IsMinGreaterMax(int? min, int? max) {
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
    /// Una expresión que accede a la propiedad <c>MaxValue</c> de la consulta actual.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar la lógica específica 
    /// de obtención del valor máximo en la consulta.
    /// </remarks>
    protected override Expression GetMaxValueExpression() {
        _query.MaxValue = GetMaxValue();
        return Expression.Property(Expression.Constant(_query), "MaxValue");
    }
}