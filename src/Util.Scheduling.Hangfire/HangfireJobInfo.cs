namespace Util.Scheduling; 

/// <summary>
/// Representa la información de un trabajo en Hangfire.
/// </summary>
public class HangfireJobInfo : IJobInfo {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Representa la cola asociada a un objeto.
    /// </summary>
    /// <remarks>
    /// Este propiedad permite establecer o recuperar el nombre de la cola.
    /// Por defecto, el valor es "default".
    /// </remarks>
    /// <value>
    /// Una cadena que representa el nombre de la cola.
    /// </value>
    public string Queue { get; set; } = "default";
}