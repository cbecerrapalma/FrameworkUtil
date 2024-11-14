namespace Util.Microservices;

/// <summary>
/// Representa el contexto de una respuesta, que puede incluir información adicional
/// sobre el resultado de una operación.
/// </summary>
public class ResponseContext {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ResponseContext"/>.
    /// </summary>
    /// <param name="responseMessage">El mensaje de respuesta HTTP que se asociará con este contexto.</param>
    /// <param name="context">El contexto HTTP que se asociará con este contexto.</param>
    public ResponseContext( HttpResponseMessage responseMessage, HttpContext context ) {
        ResponseMessage = responseMessage;
        HttpContext = context;
    }

    /// <summary>
    /// Obtiene el mensaje de respuesta HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al mensaje de respuesta HTTP que se genera al procesar una solicitud.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="HttpResponseMessage"/> que representa el mensaje de respuesta.
    /// </returns>
    public HttpResponseMessage ResponseMessage { get; }

    /// <summary>
    /// Obtiene el contexto HTTP actual.
    /// </summary>
    /// <remarks>
    /// Este propiedad proporciona acceso al contexto HTTP que contiene información sobre la solicitud y la respuesta.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="HttpContext"/> que representa el contexto HTTP actual.
    /// </value>
    public HttpContext HttpContext { get; }
}