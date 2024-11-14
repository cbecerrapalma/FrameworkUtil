using Util.Dependency;

namespace Util.Sessions; 

/// <summary>
/// Define una interfaz para la gestión de sesiones en la aplicación.
/// Esta interfaz hereda de <see cref="ISingletonDependency"/> lo que indica que debe ser 
/// implementada como un servicio singleton.
/// </summary>
public interface ISession : ISingletonDependency {
    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Este miembro proporciona acceso a una instancia de <see cref="IServiceProvider"/> 
    /// que puede ser utilizada para resolver servicios en la aplicación.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="IServiceProvider"/> que permite la resolución de servicios.
    /// </returns>
    IServiceProvider ServiceProvider { get; }
    /// <summary>
    /// Obtiene un valor que indica si el usuario está autenticado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el usuario está autenticado; de lo contrario, <c>false</c>.
    /// </value>
    bool IsAuthenticated { get; }
    /// <summary>
    /// Obtiene el identificador único del usuario.
    /// </summary>
    /// <remarks>
    /// Este identificador es utilizado para distinguir a un usuario de otros en el sistema.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador del usuario.
    /// </value>
    string UserId { get; }
    /// <summary>
    /// Obtiene el identificador del inquilino.
    /// </summary>
    /// <remarks>
    /// Este identificador es utilizado para distinguir entre diferentes inquilinos en un sistema multi-inquilino.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador del inquilino.
    /// </value>
    string TenantId { get; }
}