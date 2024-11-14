namespace Util.Security.Authentication; 

/// <summary>
/// Representa un principal no autenticado que hereda de <see cref="ClaimsPrincipal"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para representar a un usuario que no ha sido autenticado,
/// permitiendo el manejo de reclamaciones sin requerir credenciales válidas.
/// </remarks>
public class UnauthenticatedPrincipal : ClaimsPrincipal {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnauthenticatedPrincipal"/>.
    /// Esta clase representa un principal no autenticado.
    /// </summary>
    private UnauthenticatedPrincipal(){
    }

    public static readonly UnauthenticatedPrincipal Instance = new();

    /// <summary>
    /// Obtiene la identidad del usuario.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve una instancia de <see cref="UnauthenticatedIdentity"/> que representa una identidad no autenticada.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IIdentity"/> que representa la identidad del usuario.
    /// </returns>
    public override IIdentity Identity => UnauthenticatedIdentity.Instance;
}