using Util.Helpers;

namespace Util.Sessions; 

/// <summary>
/// Representa una sesión de usuario en la aplicación.
/// Implementa la interfaz <see cref="ISession"/> para proporcionar funcionalidades relacionadas con la gestión de sesiones.
/// </summary>
public class UserSession : ISession {
    public static readonly ISession Null = NullSession.Instance;

    public static readonly ISession Instance = new UserSession();

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el proveedor de servicios asociado a la instancia actual.
    /// </summary>
    /// <remarks>
    /// Este miembro es virtual, lo que permite que las clases derivadas 
    /// puedan sobrescribir este comportamiento si es necesario.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="IServiceProvider"/> que representa el proveedor de servicios.
    /// </returns>
    public virtual IServiceProvider ServiceProvider => Web.ServiceProvider;

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un valor que indica si el usuario actual está autenticado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve <c>true</c> si el usuario ha pasado por el proceso de autenticación y tiene una identidad válida;
    /// de lo contrario, devuelve <c>false</c>.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el usuario está autenticado; de lo contrario, <c>false</c>.
    /// </value>
    /// <seealso cref="Web.Identity"/>
    public virtual bool IsAuthenticated => Web.Identity.IsAuthenticated;

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del usuario actual.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del usuario. 
    /// Si el identificador de usuario no está disponible, se devuelve el valor del identificador de nombre.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si el valor del identificador de usuario está disponible 
    /// a través de la reclamación de tipo <see cref="ClaimTypes.UserId"/>. 
    /// Si no se encuentra, se intenta obtener el valor del identificador de nombre 
    /// utilizando <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/>.
    /// </remarks>
    public virtual string UserId {
        get {
            var result = Web.Identity.GetValue( ClaimTypes.UserId );
            return result.IsEmpty() ? Web.Identity.GetValue( System.Security.Claims.ClaimTypes.NameIdentifier ) : result;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del inquilino (tenant) asociado.
    /// </summary>
    /// <remarks>
    /// Este identificador se extrae de los valores de identidad del usuario actual,
    /// utilizando el tipo de reclamación <see cref="ClaimTypes.TenantId"/>.
    /// </remarks>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del inquilino.
    /// </returns>
    public virtual string TenantId => Web.Identity.GetValue( ClaimTypes.TenantId );
}