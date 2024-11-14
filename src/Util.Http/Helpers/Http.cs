using Util.Http;

namespace Util.Helpers; 

/// <summary>
/// Proporciona métodos estáticos para realizar operaciones relacionadas con HTTP.
/// </summary>
public static class Http {
    /// <summary>
    /// Obtiene una instancia de <see cref="IHttpClient"/> utilizando el contenedor de inversión de control.
    /// </summary>
    /// <remarks>
    /// Esta propiedad está diseñada para proporcionar un acceso fácil y centralizado a una instancia de <see cref="IHttpClient"/> 
    /// que puede ser utilizada para realizar solicitudes HTTP. La instancia se crea a través del contenedor de inyección de dependencias 
    /// configurado en la aplicación.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IHttpClient"/> que puede ser utilizada para realizar operaciones HTTP.
    /// </returns>
    public static IHttpClient Client => Ioc.Create<IHttpClient>();
}