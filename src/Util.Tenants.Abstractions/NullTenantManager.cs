namespace Util.Tenants;

/// <summary>
/// Representa un contenedor de inversión de control (IoC) que gestiona la creación y el ciclo de vida de los objetos.
/// </summary>
/// <remarks>
/// Este contenedor permite la inyección de dependencias, facilitando la gestión de las instancias de las clases
/// y promoviendo un diseño más limpio y desacoplado.
/// </remarks>
/// <param name="id">El identificador único para el contenedor IoC.</param>
/// <seealso cref="IServiceProvider"/>
/// <seealso cref="DependencyInjection"/>
[Ioc( -9 )]
public class NullTenantManager : ITenantManager {
    public static readonly ITenantManager Instance = new NullTenantManager();

    /// <inheritdoc />
    /// <summary>
    /// Indica si la funcionalidad está habilitada.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la funcionalidad está habilitada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c> en su implementación actual.
    /// </remarks>
    public bool Enabled() {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Indica si se permite el uso de múltiples bases de datos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si se permite el uso de múltiples bases de datos; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public bool AllowMultipleDatabase() {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el objeto actual es un host.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el objeto actual es un host; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c> en la implementación actual.
    /// </remarks>
    public bool IsHost() {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Indica si el filtro de inquilinos está deshabilitado.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de inquilinos está deshabilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public bool IsDisableTenantFilter() {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del inquilino.
    /// </summary>
    /// <returns>
    /// El identificador del inquilino como una cadena. 
    /// En este caso, siempre devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación que no proporciona un valor válido.
    /// Se recomienda implementar una lógica adecuada para retornar el identificador del inquilino.
    /// </remarks>
    public string GetTenantId() {
        return null;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la información del inquilino.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="TenantInfo"/> que representa la información del inquilino, o <c>null</c> si no se encuentra información disponible.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para recuperar datos específicos relacionados con el inquilino actual.
    /// </remarks>
    public TenantInfo GetTenant() {
        return null;
    }
}