using Util.Caching;

namespace Util.Applications.Locks; 

/// <summary>
/// Representa una implementación por defecto de un mecanismo de bloqueo.
/// </summary>
/// <remarks>
/// Esta clase proporciona un comportamiento básico para el manejo de bloqueos,
/// permitiendo que los recursos sean protegidos de accesos concurrentes.
/// </remarks>
public class DefaultLock : ILock {
    private readonly ICache _cache;
    private string _key;
    private TimeSpan? _expiration;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultLock"/>.
    /// </summary>
    /// <param name="cache">La instancia de <see cref="ICache"/> que se utilizará para el almacenamiento en caché.</param>
    public DefaultLock( ICache cache ) {
        _cache = cache;
    }

    /// <inheritdoc />
    /// <summary>
    /// Intenta bloquear un recurso asociado a la clave especificada.
    /// </summary>
    /// <param name="key">La clave del recurso que se desea bloquear.</param>
    /// <param name="expiration">El tiempo de expiración opcional para el bloqueo. Si no se proporciona, el bloqueo no tendrá un tiempo de expiración.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el bloqueo se ha establecido correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la clave ya existe en la caché. Si la clave ya está presente, el método devuelve <c>false</c>,
    /// indicando que el bloqueo no se pudo establecer. Si la clave no existe, intenta establecer el bloqueo y devuelve
    /// <c>true</c> si se logró establecer.
    /// </remarks>
    /// <seealso cref="UnlockAsync(string)"/>
    public async Task<bool> LockAsync( string key, TimeSpan? expiration = null ) {
        _key = key;
        _expiration = expiration;
        if ( await _cache.ExistsAsync( key ) )
            return false;
        return await _cache.TrySetAsync( key, 1, new CacheOptions { Expiration = expiration } );
    }

    /// <inheritdoc />
    /// <summary>
    /// Desbloquea un recurso asociado a la clave especificada.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el recurso ya está desbloqueado o si no existe en la caché.
    /// Si el recurso está bloqueado y existe en la caché, se procederá a eliminarlo.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de desbloqueo.
    /// </returns>
    public async Task UnLockAsync() {
        if ( _expiration != null )
            return;
        if ( await _cache.ExistsAsync( _key ) == false )
            return;
        await _cache.RemoveAsync( _key );
    }
}