namespace Util.Tenants.Resolvers.Internal;

/// <summary>
/// Clase interna que implementa la interfaz <see cref="IDomainTenantResolver"/>.
/// Esta clase se encarga de resolver el inquilino (tenant) basado en el dominio.
/// </summary>
internal class DomainTenantResolver : IDomainTenantResolver {
    private readonly ITenantDomainStore _store;
    private readonly TenantOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DomainTenantResolver"/>.
    /// </summary>
    /// <param name="store">La instancia de <see cref="ITenantDomainStore"/> que se utilizará para almacenar y recuperar dominios de inquilinos.</param>
    /// <param name="options">Las opciones de configuración para los inquilinos, representadas por <see cref="TenantOptions"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="store"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor asegura que se proporcione una instancia válida de <paramref name="store"/> 
    /// y establece las opciones de inquilino a un valor predeterminado si <paramref name="options"/> es <c>null</c>.
    /// </remarks>
    public DomainTenantResolver( ITenantDomainStore store, IOptions<TenantOptions> options ) {
        _store = store ?? throw new ArgumentNullException( nameof( store ) );
        _options = options?.Value ?? new TenantOptions();
    }

    /// <inheritdoc />
    /// <summary>
    /// Resuelve el identificador de inquilino (tenant ID) basado en el host proporcionado.
    /// </summary>
    /// <param name="host">El host del cual se desea resolver el identificador de inquilino.</param>
    /// <returns>
    /// Un <see cref="Task{string}"/> que representa la operación asincrónica, 
    /// que contiene el identificador de inquilino correspondiente al host, o <c>null</c> si el host está vacío.
    /// </returns>
    /// <remarks>
    /// Este método combina un mapa de dominios de múltiples resolutores y utiliza 
    /// un helper para resolver el identificador de inquilino basado en el host y el mapa combinado.
    /// </remarks>
    /// <seealso cref="Resolvers.DomainTenantResolver"/>
    /// <seealso cref="DomainTenantResolverHelper"/>
    public async Task<string> ResolveTenantIdAsync( string host ) {
        if ( host.IsEmpty() )
            return null;
        var domainMap = new Dictionary<string, string>();
        var domainFormat = string.Empty;
        var resolvers = _options.Resolvers.GetResolvers<Resolvers.DomainTenantResolver>();
        foreach ( var resolver in resolvers ) {
            if ( resolver.DomainMap != null ) {
                foreach ( var item in resolver.DomainMap ) {
                    domainMap.Add( item.Key, item.Value );
                }
            }
            domainFormat += $",{resolver.DomainFormat}";
        }
        var map = await DomainTenantResolverHelper.CombineMapAsync( domainMap, _store );
        return DomainTenantResolverHelper.ResolveTenantId( host, map, domainFormat );
    }
}