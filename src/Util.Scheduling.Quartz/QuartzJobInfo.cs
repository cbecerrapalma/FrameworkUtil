namespace Util.Scheduling; 

/// <summary>
/// Representa la información de un trabajo programado en Quartz.
/// </summary>
public class QuartzJobInfo : IJobInfo {
    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de caracteres.
    /// </value>
    public string Name { get; set; }
    /// <summary>
    /// Obtiene o establece el grupo al que pertenece el objeto.
    /// </summary>
    /// <value>
    /// Una cadena que representa el grupo. 
    /// Puede ser nula o vacía si no se ha asignado un grupo.
    /// </value>
    public string Group { get; set; }
}