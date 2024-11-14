namespace Util.Logging.Serilog; 

/// <summary>
/// Representa las opciones de configuración para el registro de eventos.
/// </summary>
public class LogOptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si se deben limpiar los proveedores.
    /// </summary>
    /// <value>
    /// <c>true</c> si se deben limpiar los proveedores; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsClearProviders { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la aplicación.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre de la aplicación.
    /// </value>
    public string Application { get; set; }
}