namespace Util.Scheduling; 

/// <summary>
/// Representa un disparador de trabajos para Hangfire.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IJobTrigger"/> y permite definir la lógica
/// para activar trabajos en Hangfire.
/// </remarks>
public class HangfireTrigger : IJobTrigger {
    /// <summary>
    /// Obtiene o establece el retraso asociado.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que no hay un retraso definido.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TimeSpan"/> que representa el retraso, o <c>null</c> si no se ha definido.
    /// </value>
    public TimeSpan? Delay { get; set; }
    /// <summary>
    /// Obtiene o establece la expresión cron que define un horario específico.
    /// </summary>
    /// <remarks>
    /// La expresión cron se utiliza comúnmente para programar tareas en sistemas basados en Unix.
    /// Asegúrese de que la expresión esté en el formato correcto para evitar errores en la programación.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la expresión cron.
    /// </value>
    public string Cron { get; set; }
}