namespace Util.Data.EntityFrameworkCore.Filters; 

/// <summary>
/// Clase que gestiona los filtros aplicados a los datos.
/// Implementa la interfaz <see cref="IFilterManager"/>.
/// </summary>
public class FilterManager : IFilterManager {
    private static readonly object Sync = new();
    private static readonly List<Type> _filterTypes = new();
    private readonly Dictionary<Type, IFilter> _filters = new();
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FilterManager"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="serviceProvider"/> es <c>null</c>.</exception>
    public FilterManager( IServiceProvider serviceProvider ) {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException( nameof(serviceProvider) );
    }

    /// <summary>
    /// Agrega un tipo de filtro a la colección de tipos de filtro si no está presente.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo de filtro que se desea agregar.</typeparam>
    /// <remarks>
    /// Este método es estático y se asegura de que el acceso a la colección de tipos de filtro sea seguro para subprocesos
    /// mediante un bloqueo. Si el tipo de filtro ya existe en la colección, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="RemoveFilterType{TFilterType}"/>
    public static void AddFilterType<TFilterType>() {
        var type = typeof( TFilterType );
        lock ( Sync ) {
            if( _filterTypes.Contains( type ) )
                return;
            _filterTypes.Add( type );
        }
    }

    /// <summary>
    /// Elimina un tipo de filtro de la colección de tipos de filtro.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo de filtro que se desea eliminar.</typeparam>
    /// <remarks>
    /// Este método asegura que la operación de eliminación sea segura para hilos mediante un bloqueo.
    /// Si el tipo de filtro especificado no se encuentra en la colección, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="AddFilterType{TFilterType}"/>
    public static void RemoveFilterType<TFilterType>() {
        var type = typeof( TFilterType );
        lock( Sync ) {
            if( _filterTypes.Contains( type ) ) 
                _filterTypes.Remove( type );
        }
    }

    /// <summary>
    /// Limpia todos los tipos de filtro almacenados.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de tipos de filtro,
    /// dejando la colección vacía. Se utiliza para restablecer el estado de los filtros.
    /// </remarks>
    public static void ClearFilterTypes() {
        _filterTypes.Clear();
    }

    /// <inheritdoc />
    /// <summary>
    /// Habilita un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea habilitar. Debe ser una clase.</typeparam>
    /// <remarks>
    /// Este método busca un filtro del tipo <typeparamref name="TFilterType"/> y, si se encuentra, lo habilita.
    /// </remarks>
    /// <seealso cref="GetFilter{TFilterType}"/>
    public void EnableFilter<TFilterType>() where TFilterType : class {
        var filter = GetFilter<TFilterType>();
        filter?.Enable();
    }

    /// <inheritdoc />
    /// <summary>
    /// Desactiva el filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea desactivar. Debe ser una clase.</typeparam>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que permite restaurar el estado del filtro cuando se dispose.
    /// Si no se encuentra un filtro del tipo especificado, se devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método busca un filtro del tipo <typeparamref name="TFilterType"/> y, si lo encuentra,
    /// llama a su método <c>Disable</c>. Es importante asegurarse de que el filtro esté correctamente
    /// implementado para evitar comportamientos inesperados.
    /// </remarks>
    /// <seealso cref="GetFilter{TFilterType}"/>
    public IDisposable DisableFilter<TFilterType>() where TFilterType : class {
        var filter = GetFilter<TFilterType>();
        return filter?.Disable();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un filtro del tipo especificado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea obtener. Debe ser una clase.</typeparam>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IFilter"/> correspondiente al tipo de filtro solicitado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el filtro basado en el tipo proporcionado.
    /// Asegúrese de que el tipo especificado esté registrado y disponible para su uso.
    /// </remarks>
    /// <seealso cref="IFilter"/>
    public IFilter GetFilter<TFilterType>() where TFilterType : class {
        return GetFilter( typeof(TFilterType) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un filtro del tipo especificado.
    /// </summary>
    /// <param name="filterType">El tipo del filtro que se desea obtener.</param>
    /// <returns>Una instancia del filtro correspondiente al tipo especificado.</returns>
    /// <remarks>
    /// Si el filtro del tipo solicitado no se encuentra en el diccionario de filtros,
    /// se crea una nueva instancia utilizando el proveedor de servicios y se agrega
    /// al diccionario para su uso posterior.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="filterType"/> es null.</exception>
    /// <seealso cref="IFilter"/>
    public IFilter GetFilter( Type filterType ) {
        if( _filters.ContainsKey( filterType ) == false ) {
            var serviceType = typeof( IFilter<> ).MakeGenericType( filterType );
            var filter = _serviceProvider.GetService( serviceType );
            _filters.Add( filterType, (IFilter)filter );
        }
        return _filters[filterType];
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si una entidad de tipo especificado está habilitada en alguno de los filtros.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de la entidad que se desea verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si la entidad está habilitada en al menos un filtro; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método itera a través de todos los tipos de filtro disponibles y verifica si la entidad
    /// especificada está habilitada en cada uno de ellos. Si encuentra al menos un filtro que habilita
    /// la entidad, retorna <c>true</c> inmediatamente.
    /// </remarks>
    /// <seealso cref="GetFilter(Type)"/>
    public bool IsEntityEnabled<TEntity>() {
        foreach( var type in _filterTypes ) {
            var filter = GetFilter( type );
            if ( filter.IsEntityEnabled<TEntity>() )
                return true;
        }
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si un filtro de un tipo específico está habilitado.
    /// </summary>
    /// <typeparam name="TFilterType">El tipo del filtro que se desea verificar. Debe ser una clase.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro está habilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetFilter{TFilterType}"/> para obtener la instancia del filtro.
    /// Si no se encuentra el filtro, se considera que no está habilitado.
    /// </remarks>
    public bool IsEnabled<TFilterType>() where TFilterType : class {
        var filter = GetFilter<TFilterType>();
        if ( filter == null )
            return false;
        return filter.IsEnabled;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene una expresión que representa un filtro para una entidad específica.
    /// </summary>
    /// <typeparam name="TEntity">El tipo de entidad para el cual se genera la expresión.</typeparam>
    /// <param name="state">El estado que se utiliza para aplicar los filtros.</param>
    /// <returns>
    /// Una expresión que representa el filtro aplicado a la entidad especificada.
    /// Si no se aplican filtros, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método itera sobre una colección de tipos de filtros y combina las expresiones de filtro 
    /// para la entidad especificada utilizando una operación lógica AND.
    /// </remarks>
    /// <seealso cref="GetFilter(Type)"/>
    /// <seealso cref="IsEntityEnabled{TEntity}"/>
    /// <seealso cref="And(Expression{Func{TEntity, bool}})"/>
    public Expression<Func<TEntity, bool>> GetExpression<TEntity>( object state ) where TEntity : class {
        Expression<Func<TEntity, bool>> expression = null;
        foreach ( var type in _filterTypes ) {
            var filter = GetFilter( type );
            if ( filter.IsEntityEnabled<TEntity>() )
                expression = expression.And( filter.GetExpression<TEntity>( state ) );
        }
        return expression;
    }
}