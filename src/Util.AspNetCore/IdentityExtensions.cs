using System.Security.Claims;
using Util.Security.Authentication;

namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para trabajar con identidades.
/// </summary>
public static class IdentityExtensions {
    /// <summary>
    /// Obtiene el valor de un reclamo específico de la identidad de un usuario.
    /// </summary>
    /// <param name="identity">La identidad de usuario de la cual se extraerá el reclamo.</param>
    /// <param name="type">El tipo de reclamo que se desea obtener.</param>
    /// <returns>
    /// El valor del reclamo especificado si existe; de lo contrario, una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión de la clase <see cref="ClaimsIdentity"/>.
    /// Se utiliza para facilitar la obtención de valores de reclamos sin necesidad de verificar la existencia del reclamo cada vez.
    /// </remarks>
    public static string GetValue( this ClaimsIdentity identity, string type ) {
        var claim = identity.FindFirst( type );
        if( claim == null )
            return string.Empty;
        return claim.Value;
    }

    /// <summary>
    /// Obtiene la identidad de las reclamaciones del contexto HTTP actual.
    /// </summary>
    /// <param name="context">El contexto HTTP del cual se extraerá la identidad.</param>
    /// <returns>
    /// Devuelve una instancia de <see cref="ClaimsIdentity"/> si el contexto contiene un usuario autenticado,
    /// de lo contrario, devuelve una instancia de <see cref="UnauthenticatedIdentity"/>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contexto es nulo o si el usuario no está autenticado. 
    /// Si el usuario está autenticado, se devuelve su identidad de reclamaciones.
    /// </remarks>
    /// <seealso cref="ClaimsIdentity"/>
    /// <seealso cref="UnauthenticatedIdentity"/>
    public static ClaimsIdentity GetIdentity( this HttpContext context ) {
        if( context?.User is not { } principal )
            return UnauthenticatedIdentity.Instance;
        if( principal.Identity is ClaimsIdentity identity )
            return identity;
        return UnauthenticatedIdentity.Instance;
    }
}