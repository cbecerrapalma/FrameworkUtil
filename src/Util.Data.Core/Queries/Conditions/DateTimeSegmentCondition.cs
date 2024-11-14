using Util.Data.Queries.Conditions.Internal;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición de segmento basada en un valor de tipo <see cref="DateTime"/>.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se utilizará en la condición.</typeparam>
/// <typeparam name="TProperty">El tipo de propiedad que se evaluará en la condición.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="SegmentConditionBase{TEntity, TProperty, DateTime}"/> 
/// y proporciona funcionalidades específicas para trabajar con propiedades de tipo <see cref="DateTime"/>.
/// </remarks>
public class DateTimeSegmentCondition<TEntity, TProperty> : SegmentConditionBase<TEntity, TProperty, DateTime> where TEntity : class {
    private readonly DateTimeQuery _query;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DateTimeSegmentCondition"/>.
    /// </summary>
    /// <param name="propertyExpression">La expresión que representa la propiedad del tipo <typeparamref name="TEntity"/> que se va a evaluar.</param>
    /// <param name="min">El valor mínimo del rango de fechas, o <c>null</c> si no se establece un límite inferior.</param>
    /// <param name="max">El valor máximo del rango de fechas, o <c>null</c> si no se establece un límite superior.</param>
    /// <param name="boundary">Especifica el tipo de límite que se aplicará a los valores mínimo y máximo.</param>
    /// <remarks>
    /// Esta clase se utiliza para definir condiciones basadas en un rango de fechas para una propiedad específica de un entidad.
    /// </remarks>
    /// <typeparam name="TEntity">El tipo de entidad que contiene la propiedad evaluada.</typeparam>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está evaluando.</typeparam>
    /// <seealso cref="Boundary"/>
    public DateTimeSegmentCondition( Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max, Boundary boundary = Boundary.Both )
        : base( propertyExpression, min, max, boundary ) {
        _query = new DateTimeQuery();
    }

    /// <summary>
    /// Determina si la fecha mínima es mayor que la fecha máxima.
    /// </summary>
    /// <param name="min">La fecha mínima a comparar.</param>
    /// <param name="max">La fecha máxima a comparar.</param>
    /// <returns>
    /// Devuelve true si la fecha mínima es mayor que la fecha máxima; de lo contrario, devuelve false.
    /// </returns>
    protected override bool IsMinGreaterMax(DateTime? min, DateTime? max) 
    { 
        return min > max; 
    }

    /// <summary>
    /// Obtiene la expresión que representa el valor mínimo.
    /// </summary>
    /// <returns>
    /// Una expresión que representa el valor mínimo.
    /// </returns>
    protected override Expression GetMinValueExpression() 
    { 
        return GetMinValueExpression(GetMinValue()); 
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor mínimo de tiempo de inicio.
    /// </summary>
    /// <param name="value">El valor de fecha y hora que se utilizará como tiempo de inicio mínimo. Puede ser nulo.</param>
    /// <returns>Una expresión que representa la propiedad <c>BeginTime</c> del objeto <c>_query</c>.</returns>
    protected Expression GetMinValueExpression(DateTime? value) 
    {
        _query.BeginTime = value;
        return Expression.Property(Expression.Constant(_query), "BeginTime");
    }

    /// <summary>
    /// Obtiene la expresión que representa el valor máximo.
    /// </summary>
    /// <returns>
    /// Una expresión que representa el valor máximo calculado.
    /// </returns>
    protected override Expression GetMaxValueExpression() 
    { 
        return GetMaxValueExpression(GetMaxValue()); 
    }

    /// <summary>
    /// Obtiene una expresión que representa el valor máximo de la propiedad EndTime de la consulta.
    /// </summary>
    /// <param name="value">El valor de fecha y hora que se establecerá como el tiempo final de la consulta.</param>
    /// <returns>
    /// Una expresión que representa la propiedad EndTime de la consulta.
    /// </returns>
    protected Expression GetMaxValueExpression(DateTime? value)
    {
        _query.EndTime = value;
        return Expression.Property(Expression.Constant(_query), "EndTime");
    }
}