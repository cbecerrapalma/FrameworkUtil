namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IJobInfo"/>.
/// </summary>
public static class IJobInfoExtensions {
    /// <summary>
    /// Establece el identificador de un objeto <see cref="IJobInfo"/> si es de tipo <see cref="HangfireJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> que se va a modificar.</param>
    /// <param name="id">El nuevo identificador que se asignará al objeto.</param>
    /// <returns>El objeto <see cref="IJobInfo"/> modificado.</returns>
    /// <remarks>
    /// Este método es una extensión que permite cambiar el identificador de un trabajo de Hangfire.
    /// Si el objeto proporcionado no es de tipo <see cref="HangfireJobInfo"/>, no se realizará ninguna modificación.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="HangfireJobInfo"/>
    public static IJobInfo Id( this IJobInfo source,string id ) {
        if ( source is HangfireJobInfo job ) 
            job.Id = id;
        return source;
    }

    /// <summary>
    /// Obtiene el identificador de un trabajo (job) de Hangfire.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IJobInfo"/> de la cual se desea obtener el identificador.</param>
    /// <returns>
    /// El identificador del trabajo si <paramref name="source"/> es una instancia de <see cref="HangfireJobInfo"/>; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    public static string GetId( this IJobInfo source ) {
        if ( source is HangfireJobInfo job )
            return job.Id;
        return null;
    }

    /// <summary>
    /// Establece la cola para un objeto <see cref="IJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> al que se le asignará la cola.</param>
    /// <param name="queue">El nombre de la cola que se asignará al trabajo.</param>
    /// <returns>El objeto <see cref="IJobInfo"/> con la cola asignada.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobInfo"/> para permitir la asignación de una cola específica
    /// a un trabajo de Hangfire. Si el objeto <paramref name="source"/> es de tipo <see cref="HangfireJobInfo"/>,
    /// se actualizará la propiedad <c>Queue</c> con el valor proporcionado.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="HangfireJobInfo"/>
    public static IJobInfo Queue( this IJobInfo source, string queue ) {
        if ( source is HangfireJobInfo job )
            job.Queue = queue;
        return source;
    }

    /// <summary>
    /// Obtiene la cola asociada a un objeto <see cref="IJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> del cual se desea obtener la cola.</param>
    /// <returns>
    /// Una cadena que representa la cola del trabajo. 
    /// Si el objeto <paramref name="source"/> es de tipo <see cref="HangfireJobInfo"/>, 
    /// se devuelve la cola correspondiente; de lo contrario, se devuelve "default".
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IJobInfo"/> 
    /// y permite obtener la cola de manera sencilla y directa.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="HangfireJobInfo"/>
    public static string GetQueue( this IJobInfo source ) {
        if ( source is HangfireJobInfo job )
            return job.Queue;
        return "default";
    }
}