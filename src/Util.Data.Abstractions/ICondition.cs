namespace Util.Data; 

/// <summary>
/// Define una interfaz para condiciones que pueden aplicarse a entidades de tipo TEntity.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad sobre la que se aplicará la condición.</typeparam>
public interface ICondition<TEntity> {
    /// <summary>
    /// Obtiene una expresión que representa una condición para filtrar entidades.
    /// </summary>
    /// <returns>
    /// Una expresión que toma un parámetro de tipo <typeparamref name="TEntity"/> y devuelve un valor booleano.
    /// </returns>
    /// <typeparam name="TEntity">
    /// El tipo de entidad que se está filtrando.
    /// </typeparam>
    /// <remarks>
    /// Esta expresión se puede utilizar en consultas para aplicar condiciones específicas
    /// al recuperar entidades de una fuente de datos.
    /// </remarks>
    Expression<Func<TEntity, bool>> GetCondition();
}