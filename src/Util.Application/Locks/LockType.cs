namespace Util.Applications.Locks;

/// <summary>
/// Representa los tipos de bloqueo que se pueden aplicar.
/// </summary>
public enum LockType
{
    /// <summary>
    /// Bloqueo de usuario, cuando un usuario envía múltiples solicitudes para ejecutar la misma operación, 
    /// solo se ejecuta la primera solicitud y las demás solicitudes son desechadas, sin afectar a otros usuarios.
    /// </summary>
    User = 0,
    /// <summary>
    /// Bloqueo global, esta operación permite que solo se ejecute la solicitud de un usuario a la vez.
    /// </summary>
    Global = 1
}