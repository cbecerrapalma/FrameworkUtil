namespace Util.Scheduling; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IJobInfo"/>.
/// </summary>
public static class IJobInfoExtensions {
    /// <summary>
    /// Establece el nombre de un objeto <see cref="IJobInfo"/> si es del tipo <see cref="QuartzJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> que se va a modificar.</param>
    /// <param name="name">El nuevo nombre que se asignará al trabajo.</param>
    /// <returns>El objeto <see cref="IJobInfo"/> modificado.</returns>
    /// <remarks>
    /// Este método es una extensión que permite cambiar el nombre de un trabajo de Quartz 
    /// de manera fluida. Si el objeto proporcionado no es de tipo <see cref="QuartzJobInfo"/>,
    /// no se realizará ninguna modificación.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="QuartzJobInfo"/>
    public static IJobInfo Name( this IJobInfo source,string name ) {
        if ( source is QuartzJobInfo job ) 
            job.Name = name;
        return source;
    }

    /// <summary>
    /// Establece el grupo de un objeto <see cref="IJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> que se va a modificar.</param>
    /// <param name="name">El nombre del grupo que se asignará al trabajo.</param>
    /// <returns>El objeto <see cref="IJobInfo"/> modificado con el nuevo grupo.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="IJobInfo"/> permitiendo cambiar el grupo
    /// de un trabajo específico. Si el objeto <paramref name="source"/> no es de tipo <see cref="QuartzJobInfo"/>,
    /// no se realizará ninguna modificación.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="QuartzJobInfo"/>
    public static IJobInfo Group( this IJobInfo source, string name ) {
        if ( source is QuartzJobInfo job )
            job.Group = name;
        return source;
    }

    /// <summary>
    /// Obtiene el nombre de un trabajo a partir de una instancia de <see cref="IJobInfo"/>.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IJobInfo"/> de la cual se desea obtener el nombre.</param>
    /// <returns>
    /// El nombre del trabajo si <paramref name="source"/> es una instancia de <see cref="QuartzJobInfo"/>; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IJobInfo"/> y permite acceder al nombre de un trabajo de manera sencilla.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="QuartzJobInfo"/>
    public static string GetName( this IJobInfo source ) {
        if ( source is QuartzJobInfo job )
            return job.Name;
        return null;
    }

    /// <summary>
    /// Obtiene el grupo de un objeto <see cref="IJobInfo"/> si es del tipo <see cref="QuartzJobInfo"/>.
    /// </summary>
    /// <param name="source">El objeto <see cref="IJobInfo"/> del cual se desea obtener el grupo.</param>
    /// <returns>
    /// El grupo del trabajo si el objeto es de tipo <see cref="QuartzJobInfo"/>; de lo contrario, devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IJobInfo"/> y permite acceder al grupo de un trabajo de manera segura.
    /// </remarks>
    /// <seealso cref="IJobInfo"/>
    /// <seealso cref="QuartzJobInfo"/>
    public static string GetGroup( this IJobInfo source ) {
        if ( source is QuartzJobInfo job )
            return job.Group;
        return null;
    }
}