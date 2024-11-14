namespace Util.Security.Authorization; 

/// <summary>
/// Clase que proporciona métodos de ayuda para la gestión de políticas de control de acceso (ACL).
/// </summary>
public static class AclPolicyHelper {
    public const string Prefix = "acl_";

    /// <summary>
    /// Obtiene un requisito de acceso basado en la política proporcionada.
    /// </summary>
    /// <param name="policy">La política en formato de cadena que contiene la información del requisito.</param>
    /// <returns>
    /// Un objeto de tipo <see cref="AclRequirement"/> que representa el requisito de acceso extraído de la política.
    /// </returns>
    /// <remarks>
    /// Este método elimina un prefijo específico de la política antes de convertirla a un objeto <see cref="AclRequirement"/>.
    /// Asegúrese de que la política esté en el formato correcto para evitar errores de conversión.
    /// </remarks>
    public static AclRequirement GetRequirement( string policy ) {
        var json = policy.RemoveStart( Prefix );
        return Util.Helpers.Json.ToObject<AclRequirement>( json );
    }

    /// <summary>
    /// Obtiene una política en formato JSON a partir de una URI y un indicador de ignorar.
    /// </summary>
    /// <param name="uri">La URI para la cual se obtiene la política.</param>
    /// <param name="ignore">Indica si se debe ignorar la política.</param>
    /// <returns>
    /// Una cadena que representa la política en formato JSON.
    /// </returns>
    public static string GetPolicy( string uri,bool ignore ) {
        var requirement = new AclRequirement {
            Uri = uri,
            Ignore = ignore
        };
        return $"{Prefix}{Util.Helpers.Json.ToJson( requirement )}";
    }
}