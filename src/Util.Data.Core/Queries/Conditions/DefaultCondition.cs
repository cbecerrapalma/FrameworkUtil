namespace Util.Data.Queries.Conditions; 

/// <summary>
/// Representa una condición predeterminada que se puede aplicar a entidades del tipo especificado.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplica la condición. Debe ser una clase.</typeparam>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ICondition{TEntity}"/> y proporciona una base para definir condiciones
/// que pueden ser utilizadas en consultas o filtros sobre entidades.
/// </remarks>
public class DefaultCondition<TEntity> : ICondition<TEntity> where TEntity : class {
    private readonly Expression<Func<TEntity, bool>> _condition;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultCondition"/>.
    /// </summary>
    /// <param name="condition">Una expresión que representa la condición a aplicar sobre la entidad del tipo <typeparamref name="TEntity"/>.</param>
    public DefaultCondition( Expression<Func<TEntity, bool>> condition ) {
        _condition = condition;
    }

    /// <summary>
    /// Obtiene la condición de filtrado para la entidad especificada.
    /// </summary>
    /// <returns>
    /// Una expresión que representa la condición de filtrado, 
    /// la cual se puede utilizar para evaluar si una entidad cumple con 
    /// ciertos criterios.
    /// </returns>
    public Expression<Func<TEntity, bool>> GetCondition() {
        return _condition;
    }
}