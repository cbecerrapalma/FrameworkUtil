namespace Util.Http; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="HttpResponseMessage"/>.
/// </summary>
public static class HttpResponseMessageExtensions {
    /// <summary>
    /// Obtiene el tipo de contenido del encabezado de la respuesta HTTP.
    /// </summary>
    /// <param name="response">La respuesta HTTP de la cual se desea obtener el tipo de contenido.</param>
    /// <returns>
    /// Devuelve el tipo de contenido como una cadena, o null si no está disponible.
    /// </returns>
    public static string GetContentType( this HttpResponseMessage response )  {
        return response?.Content.Headers.ContentType?.MediaType;
    }
}