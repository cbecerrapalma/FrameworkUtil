using Util.Helpers;

namespace Util.Tenants.Managements;

/// <summary>
/// Clase que gestiona el cambio de inquilinos en la aplicación.
/// Implementa la interfaz <see cref="ISwitchTenantManager"/>.
/// </summary>
public class SwitchTenantManager : ISwitchTenantManager {
    public const string Key = "x-switch-tenant";

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del inquilino (tenant) actual desde una cookie.
    /// </summary>
    /// <returns>
    /// Un string que representa el identificador del inquilino, o null si no se encuentra la cookie.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Web"/> para acceder a las cookies almacenadas.
    /// Asegúrese de que la cookie con la clave especificada esté presente antes de llamar a este método.
    /// </remarks>
    public string GetSwitchTenantId() {
        return Web.GetCookie( Key );
    }

    /// <inheritdoc />
    /// <summary>
    /// Cambia el inquilino actual utilizando el identificador del inquilino proporcionado.
    /// </summary>
    /// <param name="tenantId">El identificador del inquilino al que se desea cambiar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de cambio de inquilino.</returns>
    /// <remarks>
    /// Este método establece una cookie con el identificador del inquilino especificado.
    /// </remarks>
    public Task SwitchTenantAsync( string tenantId ) {
        Web.SetCookie( Key, tenantId );
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Restablece el inquilino actual eliminando la cookie asociada.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de restablecimiento del inquilino.
    /// </returns>
    /// <remarks>
    /// Este método elimina la cookie identificada por la clave especificada.
    /// </remarks>
    public Task ResetTenantAsync() {
        Web.RemoveCookie( Key );
        return Task.CompletedTask;
    }
}