namespace Util.Expressions; 

/// <summary>
/// Clase que permite rebindear parámetros en expresiones LINQ.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ExpressionVisitor"/> y sobreescribe el método 
/// <see cref="VisitParameter(ParameterExpression)"/> para modificar los parámetros 
/// de las expresiones visitadas.
/// </remarks>
public class ParameterRebinder : ExpressionVisitor {
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ParameterRebinder"/>.
    /// </summary>
    /// <param name="map">Un diccionario que mapea expresiones de parámetros originales a nuevas expresiones de parámetros.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="map"/> es nulo, se inicializa con un nuevo diccionario vacío.
    /// </remarks>
    public ParameterRebinder( Dictionary<ParameterExpression, ParameterExpression> map ) {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    /// <summary>
    /// Reemplaza los parámetros en una expresión dada utilizando un mapa de parámetros.
    /// </summary>
    /// <param name="map">Un diccionario que mapea los parámetros originales a los nuevos parámetros.</param>
    /// <param name="exp">La expresión en la que se reemplazarán los parámetros.</param>
    /// <returns>
    /// Una nueva expresión con los parámetros reemplazados según el mapa proporcionado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="ParameterRebinder"/> para realizar la visita y el reemplazo de los parámetros en la expresión.
    /// </remarks>
    public static Expression ReplaceParameters( Dictionary<ParameterExpression, ParameterExpression> map, Expression exp ) {
        return new ParameterRebinder( map ).Visit( exp );
    }

    /// <summary>
    /// Visita un nodo de expresión de tipo <see cref="ParameterExpression"/>.
    /// </summary>
    /// <param name="parameterExpression">La expresión de parámetro que se está visitando.</param>
    /// <returns>
    /// La expresión de parámetro, posiblemente reemplazada si se encuentra un mapeo en <see cref="_map"/>.
    /// </returns>
    /// <remarks>
    /// Este método busca en un diccionario de mapeo para determinar si el parámetro actual debe ser reemplazado
    /// por otro. Si se encuentra un reemplazo, se utiliza este en lugar del parámetro original.
    /// </remarks>
    protected override Expression VisitParameter( ParameterExpression parameterExpression ) {
        if( _map.TryGetValue( parameterExpression, out var replacement ) )
            parameterExpression = replacement;
        return base.VisitParameter( parameterExpression );
    }
}