namespace Util.Scheduling; 

/// <summary>
/// Define una interfaz para un trabajo que puede ser ejecutado.
/// </summary>
public interface IJob {
    /// <summary>
    /// Configura los parámetros necesarios para el funcionamiento del sistema.
    /// </summary>
    /// <remarks>
    /// Este método debe ser llamado al inicio de la aplicación para asegurar que
    /// todos los ajustes estén correctamente establecidos antes de proceder con
    /// otras operaciones.
    /// </remarks>
    void Config();
}