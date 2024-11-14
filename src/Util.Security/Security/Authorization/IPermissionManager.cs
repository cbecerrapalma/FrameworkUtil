using System.Threading;

namespace Util.Security.Authorization; 

/// <summary>
/// Define una interfaz para gestionar permisos en el sistema.
/// </summary>
public interface IPermissionManager {
    /// <summary>
    /// Verifica si el usuario tiene permiso para acceder al recurso especificado.
    /// </summary>
    /// <param name="resourceUri">La URI del recurso al que se desea acceder.</param>
    /// <returns>Devuelve <c>true</c> si el usuario tiene permiso; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método evalúa los permisos del usuario en función de la URI proporcionada.
    /// Asegúrese de que la URI esté correctamente formateada y que el usuario esté autenticado.
    /// </remarks>
    bool HasPermission(string resourceUri);
    /// <summary>
    /// Verifica si el usuario tiene permiso para acceder al recurso especificado.
    /// </summary>
    /// <param name="resourceUri">La URI del recurso para el cual se está verificando el permiso.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si el usuario tiene permiso (true) o no (false).</returns>
    /// <remarks>
    /// Este método permite determinar si el usuario actual tiene los derechos necesarios para acceder al recurso indicado.
    /// Es útil en escenarios donde se requiere control de acceso basado en permisos.
    /// </remarks>
    /// <seealso cref="System.Threading.Tasks.Task{TResult}"/>
    Task<bool> HasPermissionAsync( string resourceUri, CancellationToken cancellationToken = default );
}