namespace Util.Tenants; 

/// <summary>
/// Define la interfaz para un inquilino en el sistema.
/// </summary>
public interface ITenant {
    /// <summary>
    /// Obtiene o establece el identificador del inquilino.
    /// </summary>
    /// <value>
    /// El identificador del inquilino como una cadena.
    /// </value>
    string TenantId { get; set; }
}