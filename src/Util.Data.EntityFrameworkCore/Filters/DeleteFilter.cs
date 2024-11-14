namespace Util.Data.EntityFrameworkCore.Filters; 

/// <summary>
/// Clase que representa un filtro para eliminar elementos.
/// Hereda de <see cref="FilterBase{T}"/> donde T es <see cref="IDelete"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para aplicar criterios de filtrado específicos para operaciones de eliminación.
/// </remarks>
/// <typeparam name="T">Tipo de elemento que implementa <see cref="IDelete"/>.</typeparam>
public class DeleteFilter : FilterBase<IDelete> {
    /// <summary>
    /// Obtiene una expresión que filtra entidades basadas en su estado de eliminación.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad que se va a filtrar.</typeparam>
    /// <param name="state">El estado que se utiliza para determinar si se aplica un filtro adicional.</param>
    /// <returns>
    /// Una expresión que representa el criterio de filtrado para las entidades.
    /// </returns>
    /// <remarks>
    /// Si el estado proporcionado es nulo o no es del tipo <see cref="UnitOfWorkBase"/>, 
    /// la expresión solo filtrará las entidades que no están marcadas como eliminadas.
    /// Si el filtro de eliminación está habilitado en el <see cref="UnitOfWorkBase"/>, 
    /// se aplicará un filtro adicional.
    /// </remarks>
    /// <seealso cref="UnitOfWorkBase"/>
    public override Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class {
        var unitOfWork = state as UnitOfWorkBase;
        Expression<Func<TEntity, bool>> expression = entity => !EF.Property<bool>( entity, "IsDeleted" );
        if ( unitOfWork == null )
            return expression;
        Expression<Func<TEntity, bool>> isEnabled = entity => !unitOfWork.IsDeleteFilterEnabled;
        return isEnabled.Or( expression );
    }
}