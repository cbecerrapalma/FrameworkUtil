using Util.Helpers;

namespace Util.Tenants.Managements;

/// <summary>
/// Clase que implementa la interfaz <see cref="IViewAllTenantManager"/>.
/// Esta clase se encarga de gestionar la visualización de todos los inquilinos.
/// </summary>
public class ViewAllTenantManager : IViewAllTenantManager {
    public const string Key = "x-view-all-tenant";

    /// <inheritdoc />
    /// <summary>
    /// Determina si el filtro de inquilinos está deshabilitado.
    /// </summary>
    /// <returns>
    /// Devuelve <c>true</c> si el filtro de inquilinos está deshabilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si existe una cookie con la clave especificada. Si la cookie está vacía, se asume que el filtro no está deshabilitado.
    /// En caso contrario, se convierte el valor de la cookie a un booleano para determinar el estado del filtro.
    /// </remarks>
    public bool IsDisableTenantFilter() {
        var result = Web.GetCookie( Key );
        if ( result.IsEmpty() )
            return false;
        return result.ToBool();
    }

    /// <inheritdoc />
    /// <summary>
    /// Habilita la visualización de todos los elementos.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de habilitar la visualización de todos los elementos.
    /// </returns>
    /// <remarks>
    /// Este método establece una cookie en el navegador para permitir la visualización de todos los elementos.
    /// </remarks>
    public Task EnableViewAllAsync() {
        Web.SetCookie( Key,"true" );
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Deshabilita la visualización de todos los elementos al eliminar la cookie correspondiente.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de deshabilitar la visualización. 
    /// El resultado de la tarea es completado de manera inmediata.
    /// </returns>
    /// <remarks>
    /// Este método es útil para gestionar la visibilidad de ciertos elementos en la interfaz de usuario 
    /// al eliminar la cookie que controla esta configuración.
    /// </remarks>
    public Task DisableViewAllAsync() {
        Web.RemoveCookie( Key );
        return Task.CompletedTask;
    }
}