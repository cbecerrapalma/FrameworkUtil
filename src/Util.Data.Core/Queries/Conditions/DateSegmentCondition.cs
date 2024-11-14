namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición de segmento de fecha para una entidad específica.
/// </summary>
/// <typeparam name="TEntity">El tipo de la entidad que se está utilizando.</typeparam>
/// <typeparam name="TProperty">El tipo de la propiedad de fecha que se está evaluando.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="DateTimeSegmentCondition{TEntity, TProperty}"/> y proporciona funcionalidades específicas
/// para manejar condiciones relacionadas con segmentos de fecha en el contexto de la entidad definida.
/// </remarks>
public class DateSegmentCondition<TEntity, TProperty> : DateTimeSegmentCondition<TEntity, TProperty> where TEntity : class {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DateSegmentCondition"/>.
    /// </summary>
    /// <param name="propertyExpression">Una expresión que representa la propiedad de tipo <typeparamref name="TProperty"/> de la entidad <typeparamref name="TEntity"/>.</param>
    /// <param name="min">La fecha mínima permitida para el segmento de fechas. Puede ser nula.</param>
    /// <param name="max">La fecha máxima permitida para el segmento de fechas. Puede ser nula.</param>
    /// <param name="boundary">El límite que determina cómo se deben considerar las fechas mínimas y máximas.</param>
    public DateSegmentCondition( Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max, Boundary boundary )
        : base( propertyExpression, min, max, boundary ) {
    }

    /// <summary>
    /// Obtiene la expresión que representa el valor mínimo.
    /// </summary>
    /// <returns>
    /// Una expresión que representa el valor mínimo ajustado según los límites establecidos.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe la implementación base para proporcionar una lógica específica 
    /// para determinar el valor mínimo. Si el límite es derecho o ninguno, se añade un día 
    /// al valor mínimo obtenido.
    /// </remarks>
    protected override Expression GetMinValueExpression() {
        var minValue = GetMinValue().SafeValue().Date;
        if( GetBoundary() == Boundary.Right || GetBoundary() == Boundary.Neither )
            minValue = minValue.AddDays( 1 );
        return GetMinValueExpression( minValue );
    }

    /// <summary>
    /// Obtiene la expresión que representa el valor máximo.
    /// </summary>
    /// <returns>
    /// Una expresión que representa el valor máximo ajustado según los límites establecidos.
    /// </returns>
    /// <remarks>
    /// Este método verifica el límite establecido y ajusta el valor máximo sumando un día si el límite es derecho o ambos.
    /// </remarks>
    protected override Expression GetMaxValueExpression() {
        var maxValue = GetMaxValue().SafeValue().Date;
        if ( GetBoundary() == Boundary.Right || GetBoundary() == Boundary.Both )
            maxValue = maxValue.AddDays( 1 );
        return GetMaxValueExpression( maxValue );
    }

    /// <summary>
    /// Crea el operador izquierdo para la comparación de límites.
    /// </summary>
    /// <param name="boundary">El límite que se utilizará para determinar el operador.</param>
    /// <returns>Un operador que representa la comparación de mayor o igual.</returns>
    protected override Operator CreateLeftOperator(Boundary? boundary) 
    { 
        return Operator.GreaterEqual; 
    }

    /// <summary>
    /// Crea el operador derecho para la comparación de límites.
    /// </summary>
    /// <param name="boundary">El límite opcional que se puede utilizar para determinar el operador.</param>
    /// <returns>
    /// Un operador que representa la comparación de menor que.
    /// </returns>
    protected override Operator CreateRightOperator(Boundary? boundary) 
    {
        return Operator.Less;
    }
}