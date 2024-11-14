namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IJob"/>.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten extender la funcionalidad de los trabajos 
/// definidos en el sistema de programación de tareas.
/// </remarks>
public static class IJobExtensions {
    /// <summary>
    /// Extensión para obtener los detalles de un trabajo.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IJob"/> de la cual se desea obtener los detalles.</param>
    /// <returns>
    /// Un objeto <see cref="IJobDetail"/> que representa los detalles del trabajo, 
    /// o <c>null</c> si el <paramref name="source"/> no es una instancia de <see cref="JobBase"/>.
    /// </returns>
    public static IJobDetail GetDetail( this IJob source ) {
        if ( source is JobBase job )
            return job.GetJobDetail();
        return null;
    }

    /// <summary>
    /// Obtiene el desencadenador asociado a un trabajo.
    /// </summary>
    /// <param name="source">El trabajo del cual se desea obtener el desencadenador.</param>
    /// <returns>
    /// Un objeto <see cref="ITrigger"/> que representa el desencadenador asociado al trabajo, 
    /// o <c>null</c> si el trabajo no es de tipo <see cref="JobBase"/>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="IJob"/> y permite acceder al desencadenador 
    /// de manera segura, verificando primero si el trabajo es de tipo <see cref="JobBase"/>.
    /// </remarks>
    public static ITrigger GetTrigger( this IJob source ) {
        if ( source is JobBase job )
            return job.GetTrigger();
        return null;
    }
}