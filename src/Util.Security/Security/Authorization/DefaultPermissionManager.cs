using System.Threading;

namespace Util.Security.Authorization;

/// <summary>
/// Clase que implementa la interfaz <see cref="IPermissionManager"/> 
/// para gestionar permisos por defecto en la aplicación.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos para verificar y asignar permisos 
/// a los usuarios dentro del sistema. Se puede extender para 
/// personalizar la lógica de permisos según las necesidades específicas.
/// </remarks>
public class DefaultPermissionManager : IPermissionManager {
    /// <inheritdoc />
    /// <summary>
    /// Verifica si el usuario tiene permiso para acceder al recurso especificado.
    /// </summary>
    /// <param name="resourceUri">La URI del recurso para el cual se desea verificar el permiso.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el usuario tiene permiso para acceder al recurso; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public bool HasPermission( string resourceUri ) {
        return true;
    }

    /// <inheritdoc />
    /// <summary>
    /// Verifica si el usuario tiene permiso para acceder a un recurso específico.
    /// </summary>
    /// <param name="resourceUri">La URI del recurso para el cual se está verificando el permiso.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un valor booleano que indica si el usuario tiene permiso (true) o no (false).
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve true en la implementación actual.
    /// </remarks>
    public Task<bool> HasPermissionAsync( string resourceUri, CancellationToken cancellationToken = default ) {
        return Task.FromResult( true );
    }
}