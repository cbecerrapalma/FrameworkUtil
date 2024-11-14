namespace Util.Microservices.Dapr.ServiceInvocations;

/// <summary>
/// Clase base abstracta para filtros de respuesta.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación base para los filtros de respuesta,
/// permitiendo a las clases derivadas personalizar el comportamiento del filtrado
/// de respuestas.
/// </remarks>
public abstract class ResponseFilterBase : IResponseFilter {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>true</c>, lo que indica que la funcionalidad está habilitada.
    /// </value>
    public bool Enabled => true;
    /// <summary>
    /// Obtiene el orden de un elemento.
    /// </summary>
    /// <remarks>
    /// Este valor es un entero que representa el orden del elemento en una colección o lista.
    /// Actualmente, el valor devuelto es siempre 0.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el orden del elemento.
    /// </returns>
    public int Order => 0;
    /// <summary>
    /// Define un método abstracto para manejar el contexto de respuesta.
    /// </summary>
    /// <param name="context">El contexto de respuesta que se debe manejar.</param>
    /// <remarks>
    /// Las clases derivadas deben implementar este método para proporcionar la lógica específica
    /// de manejo de la respuesta.
    /// </remarks>
    public abstract void Handle( ResponseContext context );
}