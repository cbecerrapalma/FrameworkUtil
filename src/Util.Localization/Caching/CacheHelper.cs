namespace Util.Localization.Caching;

/// <summary>
/// Proporciona métodos estáticos para gestionar el almacenamiento en caché.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten agregar, obtener y eliminar elementos de la caché,
/// optimizando el rendimiento de la aplicación al reducir el acceso a recursos costosos.
/// </remarks>
internal static class CacheHelper {
    /// <summary>
    /// Genera una clave de caché basada en la cultura, el tipo y el nombre proporcionados.
    /// </summary>
    /// <param name="culture">La cultura que se utilizará en la clave de caché.</param>
    /// <param name="type">El tipo que se incluirá en la clave de caché.</param>
    /// <param name="name">El nombre que se utilizará en la clave de caché.</param>
    /// <returns>
    /// Una cadena que representa la clave de caché generada, en el formato "cultura-tipo-nombre".
    /// </returns>
    public static string GetCacheKey( string culture, string type, string name ) {
        return $"{culture}-{type}-{name}";
    }

    /// <summary>
    /// Calcula la fecha de expiración sumando un valor aleatorio a la expiración configurada.
    /// </summary>
    /// <param name="options">Las opciones de localización que contienen la configuración de expiración.</param>
    /// <returns>Un entero que representa la nueva fecha de expiración.</returns>
    public static int GetExpiration( LocalizationOptions options ) {
        return options.Expiration + Random.Shared.Next( 1200 );
    }
}