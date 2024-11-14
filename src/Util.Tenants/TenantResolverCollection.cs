namespace Util.Tenants;

/// <summary>
/// Representa una colección de resolutores de inquilinos.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IEnumerable{ITenantResolver}"/> 
/// para permitir la enumeración de los resolutores de inquilinos.
/// </remarks>
public class TenantResolverCollection : IEnumerable<ITenantResolver> {
    private readonly Dictionary<string, ITenantResolver> _resolvers;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantResolverCollection"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para almacenar y gestionar una colección de resolutores de inquilinos.
    /// </remarks>
    public TenantResolverCollection() {
        _resolvers = new Dictionary<string, ITenantResolver>();
    }

    /// <summary>
    /// Devuelve un enumerador que itera a través de la colección.
    /// </summary>
    /// <returns>
    /// Un <see cref="IEnumerator"/> que puede utilizarse para recorrer la colección.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() 
    { 
        return GetEnumerator(); 
    }

    /// <summary>
    /// Obtiene un enumerador que itera a través de la colección de resolutores de inquilinos.
    /// </summary>
    /// <returns>
    /// Un enumerador que permite recorrer la colección de <see cref="ITenantResolver"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetResolvers"/> para obtener la colección de resolutores 
    /// de inquilinos antes de devolver su enumerador.
    /// </remarks>
    public IEnumerator<ITenantResolver> GetEnumerator() {
        return GetResolvers().GetEnumerator();
    }

    /// <summary>
    /// Obtiene una lista de resolutores de inquilinos ordenados por prioridad.
    /// </summary>
    /// <returns>
    /// Una lista de objetos que implementan la interfaz <see cref="ITenantResolver"/>,
    /// ordenados de mayor a menor según su prioridad.
    /// </returns>
    /// <remarks>
    /// Este método selecciona los valores de los resolutores almacenados en la colección 
    /// interna <c>_resolvers</c>, y los ordena en función de la propiedad <c>Priority</c>
    /// de cada resolutor.
    /// </remarks>
    public List<ITenantResolver> GetResolvers() {
        return _resolvers
            .Select( t => t.Value )
            .OrderByDescending( t => t.Priority )
            .ToList();
    }

    /// <summary>
    /// Obtiene una lista de resolutores de tipo específico.
    /// </summary>
    /// <typeparam name="TResolver">El tipo de resolutor que se desea obtener. Debe implementar la interfaz <see cref="ITenantResolver"/>.</typeparam>
    /// <returns>
    /// Una lista de instancias de <typeparamref name="TResolver"/> que cumplen con el criterio especificado.
    /// </returns>
    /// <remarks>
    /// Este método filtra los resolutores almacenados en la colección interna, seleccionando aquellos que son del tipo especificado 
    /// y ordenándolos en orden descendente según su prioridad.
    /// </remarks>
    /// <seealso cref="ITenantResolver"/>
    public List<TResolver> GetResolvers<TResolver>() where TResolver : ITenantResolver {
        return _resolvers
            .Where( t => t.Value.GetType() == typeof( TResolver ) )
            .Select( t => (TResolver)t.Value )
            .OrderByDescending( t => t.Priority )
            .ToList();
    }

    /// <summary>
    /// Obtiene una instancia de un resolvedor de inquilinos especificado por el tipo.
    /// </summary>
    /// <typeparam name="TResolver">El tipo del resolvedor de inquilinos que se desea obtener. Debe implementar la interfaz <see cref="ITenantResolver"/>.</typeparam>
    /// <returns>
    /// Una instancia del tipo especificado <typeparamref name="TResolver"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el nombre completo del tipo <typeparamref name="TResolver"/> como clave para recuperar la instancia correspondiente.
    /// </remarks>
    public TResolver GetResolver<TResolver>() where TResolver: ITenantResolver {
        var key = typeof( TResolver ).FullName;
        return GetResolver<TResolver>( key );
    }

    /// <summary>
    /// Obtiene un resolvedor de inquilinos basado en la clave proporcionada.
    /// </summary>
    /// <typeparam name="TResolver">El tipo del resolvedor que se desea obtener, que debe implementar la interfaz <see cref="ITenantResolver"/>.</typeparam>
    /// <param name="key">La clave que se utilizará para buscar el resolvedor correspondiente.</param>
    /// <returns>
    /// Un objeto del tipo <typeparamref name="TResolver"/> que corresponde a la clave proporcionada, 
    /// o el valor predeterminado si no se encuentra ningún resolvedor asociado a la clave.
    /// </returns>
    /// <remarks>
    /// Este método busca en una colección de resolvedores y devuelve el primero que coincide con la clave especificada.
    /// Si no se encuentra un resolvedor, se devuelve el valor predeterminado para el tipo especificado.
    /// </remarks>
    /// <seealso cref="ITenantResolver"/>
    public TResolver GetResolver<TResolver>( string key ) where TResolver : ITenantResolver {
        var result = _resolvers.FirstOrDefault( t => t.Key == key );
        if ( result.Value == null )
            return default;
        return (TResolver)result.Value;
    }

    /// <summary>
    /// Agrega un resolvedor de inquilinos al sistema.
    /// </summary>
    /// <param name="resolver">El resolvedor de inquilinos a agregar. No puede ser nulo.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="resolver"/> es nulo, el método no realizará ninguna acción.
    /// Se utiliza el nombre completo del tipo de <paramref name="resolver"/> como clave para su almacenamiento.
    /// </remarks>
    public void Add( ITenantResolver resolver ) {
        if ( resolver == null )
            return;
        var key = resolver.GetType().FullName;
        Add( key, resolver );
    }

    /// <summary>
    /// Agrega un resolvedor de inquilinos asociado a una clave especificada.
    /// </summary>
    /// <param name="key">La clave que se utilizará para identificar el resolvedor de inquilinos.</param>
    /// <param name="resolver">El resolvedor de inquilinos que se desea agregar.</param>
    /// <remarks>
    /// Si la clave está vacía o el resolvedor es nulo, la operación no se realizará.
    /// Si ya existe un resolvedor asociado a la clave, se eliminará antes de agregar el nuevo.
    /// </remarks>
    public void Add( string key, ITenantResolver resolver ) {
        if ( key.IsEmpty() )
            return;
        if ( resolver == null )
            return;
        Remove( key );
        _resolvers.Add( key, resolver );
    }

    /// <summary>
    /// Agrega una lista de resolutores de inquilinos al sistema.
    /// </summary>
    /// <param name="resolvers">Una lista de objetos que implementan la interfaz <see cref="ITenantResolver"/>.</param>
    /// <remarks>
    /// Este método verifica si la lista de resolutores es nula antes de intentar agregar cada uno de ellos.
    /// Si la lista es nula, el método no realiza ninguna acción.
    /// </remarks>
    public void Add( IList<ITenantResolver> resolvers ) {
        if ( resolvers == null )
            return;
        foreach ( var resolver in resolvers )
            Add( resolver );
    }

    /// <summary>
    /// Elimina un resolvedor de inquilinos del contenedor utilizando su tipo.
    /// </summary>
    /// <typeparam name="TResolver">El tipo del resolvedor de inquilinos que se desea eliminar.</typeparam>
    /// <remarks>
    /// Este método busca el resolvedor de inquilinos en función de su tipo genérico y lo elimina del contenedor.
    /// Se espera que el tipo proporcionado implemente la interfaz <see cref="ITenantResolver"/>.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Se lanza si el tipo <typeparamref name="TResolver"/> no implementa <see cref="ITenantResolver"/>.
    /// </exception>
    public void Remove<TResolver>() where TResolver : ITenantResolver {
        var key = typeof( TResolver ).FullName;
        Remove( key );
    }

    /// <summary>
    /// Elimina un resolutor asociado a la clave especificada.
    /// </summary>
    /// <param name="key">La clave del resolutor que se desea eliminar.</param>
    /// <remarks>
    /// Si la clave proporcionada está vacía, el método no realizará ninguna acción.
    /// </remarks>
    public void Remove( string key ) {
        if ( key.IsEmpty() )
            return;
        _resolvers.Remove( key );
    }

    /// <summary>
    /// Limpia todos los resolutores almacenados.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de resolutores,
    /// dejándola vacía. Es útil para reiniciar el estado de los resolutores
    /// en situaciones donde se necesita empezar de nuevo.
    /// </remarks>
    public void Clear() {
        _resolvers.Clear();
    }
}