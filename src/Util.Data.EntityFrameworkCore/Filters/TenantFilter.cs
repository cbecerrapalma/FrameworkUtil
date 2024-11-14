namespace Util.Data.EntityFrameworkCore.Filters;

/// <summary>
/// Clase que representa un filtro para inquilinos.
/// Hereda de <see cref="FilterBase{ITenant}"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para aplicar criterios de filtrado específicos a una colección de inquilinos.
/// </remarks>
/// <typeparam name="ITenant">Tipo de inquilino que será filtrado.</typeparam>
public class TenantFilter : FilterBase<ITenant> {
    private readonly ITenantManager _manager;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantFilter"/>.
    /// </summary>
    /// <param name="manager">La instancia de <see cref="ITenantManager"/> que se utilizará para gestionar los inquilinos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="manager"/> es <c>null</c>.</exception>
    public TenantFilter( ITenantManager manager ) {
        _manager = manager ?? throw new ArgumentNullException( nameof( manager ) );
    }

    /// <summary>
    /// Obtiene una expresión que filtra entidades según el estado del administrador y el contexto de unidad de trabajo.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad sobre el cual se aplica el filtro.</typeparam>
    /// <param name="state">El estado actual que contiene información sobre la unidad de trabajo.</param>
    /// <returns>
    /// Una expresión que representa el filtro aplicado a las entidades, o <c>null</c> si el administrador está deshabilitado,
    /// el filtro de inquilinos está deshabilitado, o si el estado no es de tipo <see cref="UnitOfWorkBase"/>.
    /// </returns>
    /// <remarks>
    /// Esta función verifica primero si el administrador está habilitado y si el filtro de inquilinos está deshabilitado.
    /// Luego, intenta convertir el estado a una unidad de trabajo. Si la conversión es exitosa, crea una expresión que combina
    /// la verificación de que el filtro de inquilinos está habilitado y la comparación del identificador de inquilino de la entidad
    /// con el identificador de inquilino actual de la unidad de trabajo.
    /// </remarks>
    /// <seealso cref="UnitOfWorkBase"/>
    public override Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class {
        if ( _manager.Enabled() == false )
            return null;
        if ( _manager.IsDisableTenantFilter() )
            return null;
        var unitOfWork = state as UnitOfWorkBase;
        if ( unitOfWork == null )
            return null;
        Expression<Func<TEntity, bool>> isEnabled = entity => !unitOfWork.IsTenantFilterEnabled;
        Expression<Func<TEntity, bool>> expression = entity => EF.Property<string>( entity, "TenantId" ) == unitOfWork.CurrentTenantId;
        return isEnabled.Or( expression );
    }
}