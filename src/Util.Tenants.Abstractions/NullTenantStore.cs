namespace Util.Tenants;

/// <summary>
/// Representa una clase que implementa la inyección de dependencias.
/// </summary>
/// <remarks>
/// Esta clase permite gestionar la creación y el ciclo de vida de las dependencias
/// a través de un contenedor de inversión de control (IoC).
/// </remarks>
/// <typeparam name="T">El tipo de servicio que se va a resolver.</typeparam>
/// <seealso cref="IDependencyResolver"/>
[Ioc(-9)]
public class NullTenantStore : ITenantStore {
    public static readonly ITenantStore Instance = new NullTenantStore();

    /// <summary>
    /// Obtiene la información del inquilino correspondiente al identificador proporcionado.
    /// </summary>
    /// <param name="tenantId">El identificador del inquilino que se desea obtener.</param>
    /// <returns>
    /// Un objeto <see cref="TenantInfo"/> que contiene la información del inquilino, o null si no se encuentra.
    /// </returns>
    public TenantInfo GetTenant( string tenantId ) {
        return null;
    }
}