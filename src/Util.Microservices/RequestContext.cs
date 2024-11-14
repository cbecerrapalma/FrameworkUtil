namespace Util.Microservices; 

/// <summary>
/// Representa el contexto de una solicitud en el sistema.
/// </summary>
public class RequestContext {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RequestContext"/>.
    /// </summary>
    /// <param name="requestMessage">El mensaje de solicitud HTTP que se asociará con este contexto.</param>
    /// <param name="context">El contexto HTTP que se asociará con este contexto.</param>
    public RequestContext( HttpRequestMessage requestMessage, HttpContext context ) {
        RequestMessage = requestMessage;
        HttpContext = context;
    }

    /// <summary>
    /// Obtiene el mensaje de solicitud HTTP asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al objeto <see cref="HttpRequestMessage"/> 
    /// que representa la solicitud HTTP actual. Es útil para obtener información 
    /// sobre la solicitud, como el método, la URI y los encabezados.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="HttpRequestMessage"/> que representa la solicitud HTTP.
    /// </value>
    public HttpRequestMessage RequestMessage { get; }

    /// <summary>
    /// Obtiene el contexto HTTP actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al contexto HTTP que contiene información sobre la solicitud y la respuesta actuales.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="HttpContext"/> que representa el contexto HTTP.
    /// </value>
    public HttpContext HttpContext { get; }
}