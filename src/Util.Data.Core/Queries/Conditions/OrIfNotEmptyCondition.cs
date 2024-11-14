namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición que evalúa si al menos uno de los elementos en una colección no está vacío.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se evaluará con esta condición.</typeparam>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ICondition{TEntity}"/> y proporciona una lógica específica
/// para determinar si al menos uno de los elementos cumple con la condición de no estar vacío.
/// </remarks>
public class OrIfNotEmptyCondition<TEntity> : ICondition<TEntity> {
    private readonly List<Expression<Func<TEntity, bool>>> _conditions;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrIfNotEmptyCondition"/>.
    /// Esta clase representa una condición que evalúa si al menos una de las condiciones proporcionadas es verdadera.
    /// </summary>
    /// <param name="condition1">La primera condición a evaluar.</param>
    /// <param name="condition2">La segunda condición a evaluar.</param>
    /// <param name="conditions">Condiciones adicionales a evaluar.</param>
    public OrIfNotEmptyCondition( Expression<Func<TEntity, bool>> condition1, Expression<Func<TEntity, bool>> condition2, params Expression<Func<TEntity, bool>>[] conditions ) {
        _conditions = new List<Expression<Func<TEntity, bool>>> { condition1, condition2 };
        _conditions.AddRange( conditions );
    }

    /// <summary>
    /// Obtiene una expresión que representa una condición combinada para el tipo de entidad especificado.
    /// </summary>
    /// <returns>
    /// Una expresión que evalúa a un valor booleano, que representa la condición combinada de las condiciones especificadas.
    /// Si no hay condiciones, se devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método itera sobre una colección de condiciones y combina cada una de ellas utilizando la operación lógica OR.
    /// Se utiliza la clase <see cref="WhereIfNotEmptyCondition{TEntity}"/> para obtener la condición de cada elemento.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de entidad para el cual se está construyendo la condición.
    /// </typeparam>
    public Expression<Func<TEntity, bool>> GetCondition() {
        Expression<Func<TEntity, bool>> result = null;
        foreach( var condition in _conditions ) {
            var predicate = new WhereIfNotEmptyCondition<TEntity>( condition ).GetCondition();
            if( predicate == null )
                continue;
            result = result.Or( predicate );
        }
        return result;
    }
}