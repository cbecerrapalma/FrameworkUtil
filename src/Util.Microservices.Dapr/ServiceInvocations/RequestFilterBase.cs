namespace Util.Microservices.Dapr.ServiceInvocations; 

/// <summary>
/// Clase base abstracta para filtros de solicitud.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IRequestFilter"/> y proporciona una base para crear filtros de solicitud personalizados.
/// </remarks>
public abstract class RequestFilterBase : IRequestFilter {
    /// <summary>
    /// Obtiene un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>true</c>, lo que indica que la funcionalidad está habilitada.
    /// </value>
    public bool Enabled => true;
    /// <summary>
    /// Obtiene el orden del elemento.
    /// </summary>
    /// <value>
    /// Un entero que representa el orden, que en este caso es siempre 0.
    /// </value>
    public int Order => 0;
    /// <summary>
    /// Maneja el contexto de la solicitud.
    /// </summary>
    /// <param name="context">El contexto de la solicitud que se debe manejar.</param>
    /// <remarks>
    /// Esta es una clase abstracta que define el método Handle.
    /// Las clases derivadas deben implementar este método para proporcionar
    /// la lógica específica de manejo de la solicitud.
    /// </remarks>
    public abstract void Handle( RequestContext context );
}