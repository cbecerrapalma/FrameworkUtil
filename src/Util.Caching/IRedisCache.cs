namespace Util.Caching; 

/// <summary>
/// Define una interfaz para el acceso a la caché de Redis.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ICache"/> y proporciona métodos específicos
/// para interactuar con una implementación de caché basada en Redis.
/// </remarks>
public interface IRedisCache : ICache {
}