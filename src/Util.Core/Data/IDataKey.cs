namespace Util.Data;

/// <summary>
/// Interfaz que representa una clave de datos.
/// </summary>
public interface IDataKey {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena.
    /// </value>
    string Id { get; set; }
}