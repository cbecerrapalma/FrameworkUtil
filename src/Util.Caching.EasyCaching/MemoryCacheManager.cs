namespace Util.Caching.EasyCaching; 

/// <summary>
/// Clase que gestiona el almacenamiento en caché en memoria.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="CacheManager"/> y implementa la interfaz <see cref="ILocalCache"/>.
/// Proporciona métodos para almacenar, recuperar y eliminar elementos de la caché en memoria.
/// </remarks>
public class MemoryCacheManager : CacheManager, ILocalCache {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MemoryCacheManager"/>.
    /// </summary>
    /// <param name="factory">La fábrica de proveedores de caché que se utilizará para obtener el proveedor de caché en memoria.</param>
    /// <remarks>
    /// Este constructor obtiene el proveedor de caché en memoria utilizando la clave de proveedor de caché especificada.
    /// </remarks>
    public MemoryCacheManager( IEasyCachingProviderFactory factory ) : base( factory?.GetCachingProvider( CacheProviderKey.MemoryCache ) ) {
    }
}