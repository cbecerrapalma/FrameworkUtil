namespace Util.Security.Authentication; 

/// <summary>
/// Representa una identidad que no ha sido autenticada.
/// Esta clase hereda de <see cref="ClaimsIdentity"/> y se utiliza para
/// representar un usuario que no ha pasado por un proceso de autenticación.
/// </summary>
public class UnauthenticatedIdentity : ClaimsIdentity {
    /// <summary>
    /// Indica si el usuario está autenticado.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>false</c>, lo que significa que el usuario no está autenticado.
    /// </value>
    public override bool IsAuthenticated => false;

    public static readonly UnauthenticatedIdentity Instance = new();
}