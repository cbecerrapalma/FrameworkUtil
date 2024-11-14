namespace Util.Security;

/// <summary>
/// Representa las opciones de control de acceso (ACL) para la configuración de permisos.
/// </summary>
public class AclOptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si se permite el acceso anónimo.
    /// </summary>
    /// <remarks>
    /// Si este valor es verdadero, los usuarios no autenticados pueden acceder a los recursos protegidos.
    /// De lo contrario, se requiere autenticación para acceder a dichos recursos.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se permite el acceso anónimo; de lo contrario, <c>false</c>.
    /// </value>
    public bool AllowAnonymous { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar la lista de control de acceso (ACL).
    /// </summary>
    /// <remarks>
    /// Si este valor se establece en verdadero, las restricciones de acceso definidas en la ACL no se aplicarán.
    /// De lo contrario, se respetarán las configuraciones de acceso definidas.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se debe ignorar la ACL; de lo contrario, <c>false</c>.
    /// </value>
    public bool IgnoreAcl { get; set; }
    /// <summary>
    /// Obtiene o establece la URI del recurso.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URI del recurso.
    /// </value>
    public string ResourceUri { get; set; }
}