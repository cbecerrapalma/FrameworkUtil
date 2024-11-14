namespace Util.Domain; 

/// <summary>
/// Define una interfaz para representar una versión.
/// </summary>
public interface IVersion {
    /// <summary>
    /// Obtiene o establece la versión como un arreglo de bytes.
    /// </summary>
    /// <remarks>
    /// Este arreglo de bytes puede representar diferentes formatos de versión,
    /// como números de versión en formato binario o cualquier otro tipo de
    /// información relevante que se desee almacenar en formato de bytes.
    /// </remarks>
    /// <value>
    /// Un arreglo de bytes que representa la versión.
    /// </value>
    byte[] Version { get; set; }
}