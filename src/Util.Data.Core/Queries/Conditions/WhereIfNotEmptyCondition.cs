using Util.Helpers;
using Util.Properties;

namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición que se aplica solo si un valor no está vacío.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplica la condición.</typeparam>
public class WhereIfNotEmptyCondition<TEntity> : ICondition<TEntity> {
    private readonly Expression<Func<TEntity, bool>> _condition;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="WhereIfNotEmptyCondition"/>.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición a aplicar.</param>
    public WhereIfNotEmptyCondition( Expression<Func<TEntity, bool>> condition ) {
        _condition = condition;
    }

    /// <summary>
    /// Obtiene una condición de tipo expresión para la entidad especificada.
    /// </summary>
    /// <returns>
    /// Una expresión que representa la condición para el tipo de entidad <typeparamref name="TEntity"/>.
    /// Si no hay condición definida o si la condición es vacía, se devuelve <c>null</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si hay más de una condición definida en <c>_condition</c>.
    /// </exception>
    /// <remarks>
    /// Este método verifica si la condición es nula o si contiene más de una condición, en cuyo caso lanza una excepción.
    /// Si la condición es válida y no está vacía, se devuelve la condición.
    /// </remarks>
    /// <typeparam name="TEntity">
    /// El tipo de entidad para el cual se está obteniendo la condición.
    /// </typeparam>
    public Expression<Func<TEntity, bool>> GetCondition() {
        if( _condition == null )
            return null;
        if( Lambda.GetConditionCount( _condition ) > 1 )
            throw new InvalidOperationException( string.Format( R.CanOnlyOneCondition, _condition ) );
        return _condition.Value().SafeString().IsEmpty() ? null : _condition;
    }
}