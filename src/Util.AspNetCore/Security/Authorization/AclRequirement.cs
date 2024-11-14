namespace Util.Security.Authorization; 

/// <summary>
/// Representa un requisito de autorización que se utiliza para verificar el acceso a recursos.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IAuthorizationRequirement"/> y se utiliza en el contexto de la autorización de ASP.NET.
/// </remarks>
public class AclRequirement : IAuthorizationRequirement {
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar el elemento asociado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el elemento debe ser ignorado; de lo contrario, <c>false</c>.
    /// </value>
    public bool Ignore { get; set; }
    /// <summary>
    /// Obtiene o establece la URI asociada.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URI.
    /// </value>
    public string Uri { get; set; }
}