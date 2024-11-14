namespace Util.Domain.Trees; 

/// <summary>
/// Interfaz que define un contrato para objetos que pueden estar habilitados o deshabilitados.
/// </summary>
public interface IEnabled {
    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <value>
    /// <c>true</c> si la funcionalidad está habilitada; de lo contrario, <c>false</c>.
    /// </value>
    bool Enabled { get; set; }
}