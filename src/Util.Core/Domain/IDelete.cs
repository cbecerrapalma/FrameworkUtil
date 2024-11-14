namespace Util.Domain; 

/// <summary>
/// Interfaz que define un contrato para la eliminación de entidades.
/// </summary>
public interface IDelete {
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento ha sido eliminado.
    /// </summary>
    /// <value>
    /// <c>true</c> si el elemento ha sido eliminado; de lo contrario, <c>false</c>.
    /// </value>
    bool IsDeleted { get; set; }
}