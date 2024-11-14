namespace Util.Security.Authorization; 

/// <summary>
/// Proporciona políticas de autorización basadas en listas de control de acceso (ACL).
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IAuthorizationPolicyProvider"/> y se encarga de
/// generar políticas de autorización según las reglas definidas en las ACL.
/// </remarks>
public class AclPolicyProvider : IAuthorizationPolicyProvider {
    /// <summary>
    /// Obtiene una política de autorización de forma asíncrona basada en el nombre de la política proporcionada.
    /// </summary>
    /// <param name="policyName">El nombre de la política de autorización que se desea obtener.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que es la política de autorización construida.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un constructor de políticas de autorización para agregar requisitos a la política
    /// obtenida a través del método <see cref="AclPolicyHelper.GetRequirement(string)"/>.
    /// </remarks>
    public Task<AuthorizationPolicy> GetPolicyAsync( string policyName ) {
        var builder = new AuthorizationPolicyBuilder();
        var requirement = AclPolicyHelper.GetRequirement( policyName );
        builder.AddRequirements( requirement );
        return Task.FromResult( builder.Build() );
    }

    /// <summary>
    /// Obtiene la política de autorización predeterminada.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, que contiene la política de autorización predeterminada.
    /// </returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() {
        return Task.FromResult( new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build() );
    }

    /// <summary>
    /// Obtiene una política de autorización de respaldo de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una instancia de <see cref="AuthorizationPolicy"/> 
    /// que es nula en este caso.
    /// </returns>
    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() {
        return Task.FromResult<AuthorizationPolicy>( null );
    }
}