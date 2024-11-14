namespace Util.Caching; 

/// <summary>
/// Proporciona métodos de extensión para trabajar con claves de caché.
/// </summary>
public static class CacheKeyExtensions {
    /// <summary>
    /// Valida que la clave de caché y su propiedad clave no sean nulas.
    /// </summary>
    /// <param name="cacheKey">La instancia de <see cref="CacheKey"/> que se va a validar.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="cacheKey"/> o <paramref name="cacheKey.Key"/> son nulos.</exception>
    /// <remarks>
    /// Este método es una extensión que permite comprobar la validez de un objeto <see cref="CacheKey"/> 
    /// asegurando que tanto el objeto en sí como su propiedad clave no sean nulos.
    /// </remarks>
    public static void Validate( this CacheKey cacheKey ) {
        cacheKey.CheckNull( nameof( cacheKey ) );
        cacheKey.Key.CheckNull( nameof( cacheKey.Key ) );
    }
}