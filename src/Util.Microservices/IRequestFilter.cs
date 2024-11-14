namespace Util.Microservices; 

/// <summary>
/// Define un contrato para filtros de solicitudes.
/// </summary>
public interface IRequestFilter {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    bool Enabled { get; }
    /// <summary>
    /// Representa el orden de un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona el valor del orden asignado.
    /// </remarks>
    /// <value>
    /// Un entero que indica el orden del elemento.
    /// </value>
    int Order { get; }
    /// <summary>
    /// Maneja el contexto de la solicitud proporcionado.
    /// </summary>
    /// <param name="context">El contexto de la solicitud que se va a manejar.</param>
    /// <remarks>
    /// Este método es responsable de procesar la información contenida en el contexto de la solicitud,
    /// realizando las acciones necesarias según la lógica de negocio definida.
    /// </remarks>
    void Handle(RequestContext context);
}