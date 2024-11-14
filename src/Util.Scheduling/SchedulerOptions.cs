namespace Util.Scheduling; 

/// <summary>
/// Representa las opciones de configuración para el programador.
/// </summary>
public class SchedulerOptions {
    /// <summary>
    /// Obtiene o establece un valor que indica si se deben escanear trabajos.
    /// </summary>
    /// <value>
    /// <c>true</c> si se deben escanear trabajos; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsScanJobs { get; set; }
}