namespace Util.Caching.EasyCaching; 

/// <summary>
/// Clase que implementa la generación de claves para el caché.
/// </summary>
/// <remarks>
/// Esta clase es responsable de crear claves únicas que se utilizarán para almacenar y recuperar datos en el caché.
/// </remarks>
public class CacheKeyGenerator : ICacheKeyGenerator {
    private readonly IEasyCachingKeyGenerator _keyGenerator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CacheKeyGenerator"/>.
    /// </summary>
    /// <param name="keyGenerator">Una instancia de <see cref="IEasyCachingKeyGenerator"/> que se utilizará para generar claves de caché.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="keyGenerator"/> es <c>null</c>.</exception>
    public CacheKeyGenerator( IEasyCachingKeyGenerator keyGenerator ) {
        _keyGenerator = keyGenerator ?? throw new ArgumentNullException( nameof(keyGenerator) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una clave de caché basada en la información del método y los argumentos proporcionados.
    /// </summary>
    /// <param name="methodInfo">La información del método para el cual se está creando la clave de caché.</param>
    /// <param name="args">Los argumentos que se pasan al método.</param>
    /// <param name="prefix">Un prefijo que se añadirá a la clave de caché.</param>
    /// <returns>
    /// Una cadena que representa la clave de caché generada.
    /// </returns>
    /// <seealso cref="MethodInfo"/>
    public string CreateCacheKey( MethodInfo methodInfo, object[] args, string prefix ) {
        return _keyGenerator.GetCacheKey( methodInfo, args, prefix );
    }
}