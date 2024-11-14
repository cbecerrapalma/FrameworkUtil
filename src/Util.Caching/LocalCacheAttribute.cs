namespace Util.Caching; 

/// <summary>
/// Atributo que indica que un método o clase debe utilizar una caché local.
/// Hereda de <see cref="CacheAttribute"/>.
/// </summary>
/// <remarks>
/// Este atributo se puede aplicar a métodos o clases para especificar que los resultados
/// deben ser almacenados en una caché local, mejorando así el rendimiento al evitar
/// cálculos repetidos o llamadas a servicios externos.
/// </remarks>
public class LocalCacheAttribute : CacheAttribute {
    /// <summary>
    /// Obtiene la caché local a partir del contexto de aspecto proporcionado.
    /// </summary>
    /// <param name="context">El contexto del aspecto que contiene información sobre el servicio.</param>
    /// <returns>Una instancia de <see cref="ILocalCache"/> que representa la caché local.</returns>
    protected override ICache GetCache(AspectContext context) 
    { 
        return context.ServiceProvider.GetService<ILocalCache>(); 
    }
}