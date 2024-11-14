namespace Util.Caching; 

/// <summary>
/// Atributo que indica que un método o clase debe utilizar 
/// Redis como mecanismo de almacenamiento en caché.
/// </summary>
/// <remarks>
/// Este atributo se puede aplicar a métodos o clases para 
/// habilitar el almacenamiento en caché utilizando Redis. 
/// Se puede personalizar la configuración del caché a través 
/// de las propiedades del atributo.
/// </remarks>
public class RedisCacheAttribute : CacheAttribute {
    /// <summary>
    /// Obtiene una instancia de caché utilizando el contexto proporcionado.
    /// </summary>
    /// <param name="context">El contexto de aspecto que contiene información sobre el servicio.</param>
    /// <returns>Una instancia de <see cref="ICache"/> que representa la caché obtenida.</returns>
    /// <remarks>
    /// Este método se utiliza para acceder a la implementación de caché específica, en este caso, una caché de Redis.
    /// </remarks>
    protected override ICache GetCache(AspectContext context) {
        return context.ServiceProvider.GetService<IRedisCache>();
    }
}