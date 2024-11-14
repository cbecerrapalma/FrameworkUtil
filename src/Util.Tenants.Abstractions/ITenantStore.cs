namespace Util.Tenants;

/// <summary>
/// Define una interfaz para el almacenamiento de inquilinos.
/// </summary>
/// <remarks>
/// Esta interfaz extiende <see cref="IScopeDependency"/> y se utiliza para gestionar la información relacionada con los inquilinos en un sistema multitenencia.
/// </remarks>
public interface ITenantStore : IScopeDependency {
    /// <summary>
    /// Obtiene la información del inquilino basado en el identificador proporcionado.
    /// </summary>
    /// <param name="tenantId">El identificador único del inquilino que se desea obtener.</param>
    /// <returns>Un objeto <see cref="TenantInfo"/> que contiene la información del inquilino.</returns>
    /// <remarks>
    /// Este método puede lanzar una excepción si el inquilino no se encuentra o si el <paramref name="tenantId"/> es inválido.
    /// </remarks>
    /// <seealso cref="TenantInfo"/>
    TenantInfo GetTenant( string tenantId );
}