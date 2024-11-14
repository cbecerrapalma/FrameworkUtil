namespace Util.Data.EntityFrameworkCore.Filters; 

/// <summary>
/// Clase base abstracta para filtros que implementan la interfaz <see cref="IFilter{TFilterType}"/>.
/// </summary>
/// <typeparam name="TFilterType">El tipo de filtro que hereda de <see cref="FilterBase{TFilterType}"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona una estructura básica para la implementación de filtros específicos,
/// permitiendo la creación de filtros personalizados que pueden ser utilizados en diferentes contextos.
/// </remarks>
public abstract class FilterBase<TFilterType> : IFilter<TFilterType> where TFilterType : class {
    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es <c>true</c>, lo que significa que la funcionalidad está habilitada al inicio.
    /// </remarks>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    public virtual bool IsEnabled { get; private set; } = true;

    /// <summary>
    /// Determina si una entidad está habilitada en función de su tipo.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo de entidad está asignable desde el tipo de filtro; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para comprobar si el tipo de entidad proporcionado es compatible con el tipo de filtro definido.
    /// </remarks>
    /// <seealso cref="TFilterType"/>
    public virtual bool IsEntityEnabled<TEntity>() {
        return typeof(TFilterType).IsAssignableFrom( typeof(TEntity) );
    }

    /// <summary>
    /// Habilita la funcionalidad asociada a esta instancia.
    /// </summary>
    /// <remarks>
    /// Este método establece la propiedad <c>IsEnabled</c> en <c>true</c>,
    /// lo que indica que la instancia está habilitada y lista para su uso.
    /// </remarks>
    public virtual void Enable() {
        IsEnabled = true;
    }

    /// <summary>
    /// Desactiva la funcionalidad del objeto actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que, al ser dispuesto, volverá a habilitar la funcionalidad.
    /// Si el objeto ya está deshabilitado, se devuelve <see cref="DisposeAction.Null"/>.
    /// </returns>
    /// <remarks>
    /// Este método cambia el estado del objeto a deshabilitado. Si se llama a este método cuando el objeto ya está deshabilitado,
    /// no se realiza ninguna acción adicional.
    /// </remarks>
    public virtual IDisposable Disable() {
        if ( IsEnabled == false )
            return DisposeAction.Null;
        IsEnabled = false;
        return new DisposeAction( Enable );
    }

    /// <summary>
    /// Obtiene una expresión que representa un predicado para filtrar entidades del tipo especificado.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad para el cual se genera la expresión.</typeparam>
    /// <param name="state">Un objeto que contiene el estado o contexto necesario para construir la expresión.</param>
    /// <returns>
    /// Una expresión que representa un predicado que puede ser utilizado para filtrar entidades de tipo <typeparamref name="TEntity"/>.
    /// </returns>
    /// <remarks>
    /// Esta función es abstracta y debe ser implementada por las clases derivadas.
    /// La implementación debe definir la lógica específica para construir la expresión de filtrado
    /// basada en el estado proporcionado.
    /// </remarks>
    /// <seealso cref="Expression{TDelegate}"/>
    public abstract Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class;
}