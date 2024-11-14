namespace Util.Microservices;

/// <summary>
/// Define una interfaz para los tipos de datos.
/// </summary>
public interface IDataType {
    /// <summary>
    /// Obtiene o establece el tipo de dato.
    /// </summary>
    /// <value>
    /// Un string que representa el tipo de dato.
    /// </value>
    string DataType { get; set; }
}