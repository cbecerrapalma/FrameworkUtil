namespace Util.Security.Authorization;

/// <summary>
/// Interfaz que define un contrato para la creación de resultados no autorizados.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para proporcionar una implementación de un resultado que indica que el acceso a un recurso está prohibido.
/// </remarks>
public interface IUnauthorizedResultFactory : ISingletonDependency {
    /// <summary>
    /// Obtiene el código de estado HTTP.
    /// </summary>
    /// <remarks>
    /// Este código de estado representa la respuesta de una solicitud HTTP,
    /// indicando si la solicitud fue exitosa o si ocurrió un error.
    /// </remarks>
    /// <value>
    /// Un entero que representa el código de estado HTTP.
    /// </value>
    int HttpStatusCode { get; }
    /// <summary>
    /// Crea un resultado basado en el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que se utilizará para crear el resultado.</param>
    /// <returns>Un objeto que representa el resultado creado.</returns>
    /// <remarks>
    /// Este método puede utilizarse para generar respuestas personalizadas 
    /// basadas en la información contenida en el contexto HTTP, como 
    /// parámetros de solicitud, encabezados y otros datos relevantes.
    /// </remarks>
    object CreateResult( HttpContext context );
}