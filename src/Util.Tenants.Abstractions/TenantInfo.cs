using Util.Data;

namespace Util.Tenants; 

/// <summary>
/// Representa la información de un inquilino en el sistema.
/// </summary>
public class TenantInfo {
    /// <summary>
    /// Obtiene o establece el identificador del inquilino.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para diferenciar entre diferentes inquilinos en un sistema multi-inquilino.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador del inquilino.
    /// </value>
    public string TenantId { get; set; }
    /// <summary>
    /// Obtiene o establece una colección de cadenas de conexión.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder a las cadenas de conexión que se utilizan para establecer conexiones con bases de datos.
    /// </remarks>
    /// <value>
    /// Una colección de cadenas de conexión.
    /// </value>
    public ConnectionStringCollection ConnectionStrings { get; set; }
}