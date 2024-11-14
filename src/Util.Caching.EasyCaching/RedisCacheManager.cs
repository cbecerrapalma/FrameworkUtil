namespace Util.Caching.EasyCaching; 

/// <summary>
/// Clase que gestiona el almacenamiento en caché utilizando Redis.
/// Hereda de <see cref="CacheManager"/> e implementa <see cref="IRedisCache"/>.
/// </summary>
public class RedisCacheManager : CacheManager, IRedisCache {
    private readonly IRedisCachingProvider _provider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RedisCacheManager"/>.
    /// </summary>
    /// <param name="factory">La fábrica de proveedores de caché que se utilizará para obtener el proveedor de caché Redis.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="factory"/> es <c>null</c>.</exception>
    public RedisCacheManager( IEasyCachingProviderFactory factory ) : base( factory?.GetCachingProvider( CacheProviderKey.RedisCache ) ) {
        if ( factory == null )
            throw new ArgumentNullException( nameof(factory) );
        _provider = factory.GetRedisProvider( CacheProviderKey.RedisCache );
    }
}