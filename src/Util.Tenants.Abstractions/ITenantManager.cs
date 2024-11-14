namespace Util.Tenants;

/// <summary>
/// Interfaz que define las operaciones relacionadas con la gestión de inquilinos.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IScopeDependency"/> lo que indica que las implementaciones
/// de esta interfaz deben ser tratadas como dependencias con un alcance específico.
/// </remarks>
public interface ITenantManager : IScopeDependency {
    /// <summary>
    /// Indica si la funcionalidad está habilitada o no.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la funcionalidad está habilitada; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    bool Enabled();
    /// <summary>
    /// Indica si se permite el uso de múltiples bases de datos.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si se permiten múltiples bases de datos; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método puede ser útil en escenarios donde se requiere acceder a varias bases de datos 
    /// simultáneamente, permitiendo así una mayor flexibilidad en la gestión de datos.
    /// </remarks>
    bool AllowMultipleDatabase();
    /// <summary>
    /// Determina si la instancia actual es un host.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si la instancia es un host; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para verificar el estado de la instancia y determinar si 
    /// tiene las capacidades necesarias para actuar como un host en el contexto 
    /// específico de la aplicación.
    /// </remarks>
    bool IsHost();
    /// <summary>
    /// Obtiene el identificador del inquilino (tenant).
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del inquilino.
    /// </returns>
    /// <remarks>
    /// Este método es útil para recuperar el ID del inquilino que se utiliza en operaciones relacionadas con la gestión de usuarios y recursos en un entorno multi-inquilino.
    /// </remarks>
    string GetTenantId();
    /// <summary>
    /// Obtiene la información del inquilino actual.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="TenantInfo"/> que contiene los detalles del inquilino.
    /// </returns>
    /// <remarks>
    /// Este método puede lanzar excepciones si no se puede acceder a la información del inquilino.
    /// Asegúrese de manejar adecuadamente las excepciones al llamar a este método.
    /// </remarks>
    TenantInfo GetTenant();
    /// <summary>
    /// Indica si el filtro de inquilinos está deshabilitado.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de inquilinos está deshabilitado; de lo contrario, <c>false</c>.
    /// </returns>
    bool IsDisableTenantFilter();
}