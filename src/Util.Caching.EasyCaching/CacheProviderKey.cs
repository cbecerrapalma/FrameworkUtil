namespace Util.Caching.EasyCaching; 

/// <summary>
/// Proporciona claves para el acceso a la caché.
/// </summary>
public static class CacheProviderKey {
    public const string MemoryCache = "DefaultInMemory";
    public const string RedisCache = "DefaultRedis";
    public const string HybridCache = "DefaultHybrid";
    public const string RedisBus = "DefaultRedis";
    public const string SystemTextJson = "SystemTextJson";
}