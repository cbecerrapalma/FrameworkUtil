namespace Util.Caching; 

/// <summary>
/// Representa las opciones de configuración para el sistema de caché.
/// </summary>
public class CacheOptions {
    /// <summary>
    /// Obtiene o establece el tiempo de expiración.
    /// </summary>
    /// <remarks>
    /// Este propiedad representa un intervalo de tiempo que puede ser nulo.
    /// Si el valor es nulo, significa que no hay un tiempo de expiración definido.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TimeSpan"/> que representa el tiempo de expiración, o <c>null</c> si no está definido.
    /// </value>
    public TimeSpan? Expiration { get; set; }
}