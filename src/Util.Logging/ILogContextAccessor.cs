namespace Util.Logging; 

/// <summary>
/// Interfaz que proporciona acceso al contexto de registro.
/// </summary>
public interface ILogContextAccessor {
    /// <summary>
    /// Obtiene o establece el contexto de registro.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="LogContext"/> que representa el contexto de registro actual.
    /// </value>
    LogContext Context { get; set; }
}