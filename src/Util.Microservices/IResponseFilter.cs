namespace Util.Microservices; 

/// <summary>
/// Define un filtro de respuesta que permite modificar o procesar respuestas.
/// </summary>
public interface IResponseFilter {
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
    /// Esta propiedad es de solo lectura y devuelve un valor entero que indica el orden.
    /// </remarks>
    /// <value>
    /// Un entero que representa el orden del elemento.
    /// </value>
    int Order { get; }
    /// <summary>
    /// Maneja el contexto de respuesta.
    /// </summary>
    /// <param name="context">El contexto de respuesta que se va a manejar.</param>
    void Handle(ResponseContext context);
}