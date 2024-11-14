namespace Util.Caching; 

/// <summary>
/// Define un contrato para la generación de claves de caché.
/// </summary>
public interface ICacheKeyGenerator {
    /// <summary>
    /// Crea una clave de caché única basada en la información del método y los argumentos proporcionados.
    /// </summary>
    /// <param name="methodInfo">La información del método que se utilizará para generar la clave.</param>
    /// <param name="args">Los argumentos del método que se incluirán en la clave.</param>
    /// <param name="prefix">Un prefijo opcional que se añadirá a la clave generada.</param>
    /// <returns>Una cadena que representa la clave de caché generada.</returns>
    /// <remarks>
    /// Esta función combina el nombre del método, los tipos de los argumentos y el prefijo 
    /// para formar una clave de caché que puede ser utilizada para almacenar y recuperar 
    /// resultados de manera eficiente.
    /// </remarks>
    /// <seealso cref="MethodInfo"/>
    string CreateCacheKey( MethodInfo methodInfo, object[] args, string prefix );
}