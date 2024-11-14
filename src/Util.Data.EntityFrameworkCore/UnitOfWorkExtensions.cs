namespace Util.Data.EntityFrameworkCore; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="IUnitOfWork"/>.
/// </summary>
public static class UnitOfWorkExtensions {
    /// <summary>
    /// Obtiene una lista de las migraciones aplicadas de la base de datos de un contexto específico.
    /// </summary>
    /// <param name="source">La unidad de trabajo que implementa <see cref="IUnitOfWork"/>.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una lista de cadenas que contiene los nombres de las migraciones aplicadas.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la unidad de trabajo proporcionada es nula y si es del tipo <see cref="DbContext"/>. 
    /// Si no es así, devuelve una lista vacía.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static async Task<List<string>> GetAppliedMigrationsAsync( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if ( source is not DbContext unitOfWork )
            return new List<string>();
        var result = await unitOfWork.Database.GetAppliedMigrationsAsync();
        return result.ToList();
    }

    /// <summary>
    /// Realiza la migración de la base de datos de forma asíncrona utilizando el contexto de unidad de trabajo proporcionado.
    /// </summary>
    /// <param name="source">La unidad de trabajo que contiene el contexto de la base de datos a migrar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede usarse para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de migración.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es de tipo <see cref="DbContext"/> antes de intentar realizar la migración.
    /// Si el objeto no es un <see cref="DbContext"/>, el método no realiza ninguna acción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static async Task MigrateAsync( this IUnitOfWork source, CancellationToken cancellationToken = default ) {
        source.CheckNull( nameof( source ) );
        if ( source is not DbContext unitOfWork )
            return;
        await unitOfWork.Database.MigrateAsync( cancellationToken );
    }

    /// <summary>
    /// Asegura que la base de datos asociada al contexto se haya creado.
    /// </summary>
    /// <param name="source">La unidad de trabajo que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la base de datos fue creada; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la interfaz <see cref="IUnitOfWork"/>.
    /// Si el objeto <paramref name="source"/> es nulo, se lanzará una excepción.
    /// Si el <paramref name="source"/> no es del tipo <see cref="DbContext"/>, 
    /// el método devolverá <c>false</c>.
    /// </remarks>
    public static bool EnsureCreated( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if( source is not DbContext unitOfWork )
            return false;
        return unitOfWork.Database.EnsureCreated();
    }

    /// <summary>
    /// Asegura que la base de datos esté creada de manera asíncrona.
    /// </summary>
    /// <param name="source">La unidad de trabajo que se utilizará para verificar la creación de la base de datos.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la base de datos fue creada; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es una instancia de <see cref="DbContext"/>.
    /// Si no lo es, se devuelve <c>false</c>. Si es una instancia válida, se llama al método <see cref="Database.EnsureCreatedAsync"/> 
    /// para asegurar que la base de datos esté creada.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es <c>null</c>.</exception>
    public static async Task<bool> EnsureCreatedAsync( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if( source is not DbContext unitOfWork )
            return false;
        return await unitOfWork.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// Asegura que la base de datos asociada al contexto se elimine si existe.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IUnitOfWork"/> que se va a verificar y eliminar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la base de datos fue eliminada exitosamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el <paramref name="source"/> es nulo y si es una instancia de <see cref="DbContext"/>.
    /// Si no es así, devuelve <c>false</c>. Si es una instancia válida, intenta eliminar la base de datos.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="source"/> es nulo.
    /// </exception>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="DbContext"/>
    public static bool EnsureDeleted( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if( source is not DbContext unitOfWork )
            return false;
        return unitOfWork.Database.EnsureDeleted();
    }

    /// <summary>
    /// Asegura que la base de datos asociada al contexto se elimine.
    /// </summary>
    /// <param name="source">La unidad de trabajo que contiene el contexto de la base de datos.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la base de datos se eliminó correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo y si es una instancia de <see cref="DbContext"/>.
    /// Si no es así, devuelve <c>false</c>. Si es una instancia válida, se llama al método <see cref="Database.EnsureDeletedAsync"/> 
    /// para intentar eliminar la base de datos.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static async Task<bool> EnsureDeletedAsync( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if( source is not DbContext unitOfWork )
            return false;
        return await unitOfWork.Database.EnsureDeletedAsync();
    }

    /// <summary>
    /// Limpia el caché de seguimiento de cambios del contexto de base de datos.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IUnitOfWork"/> que se va a limpiar.</param>
    /// <remarks>
    /// Este método verifica si la instancia proporcionada es un <see cref="DbContext"/> 
    /// y, si es así, llama al método <see cref="ChangeTracker.Clear"/> para limpiar el 
    /// caché de seguimiento de cambios. Si la instancia no es un <see cref="DbContext"/>, 
    /// no se realiza ninguna acción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    public static void ClearCache( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if ( source is not DbContext unitOfWork )
            return;
        unitOfWork.ChangeTracker.Clear();
    }

    /// <summary>
    /// Obtiene una representación de depuración de la vista del rastreador de cambios 
    /// del contexto de la base de datos asociado al objeto <paramref name="source"/>.
    /// </summary>
    /// <param name="source">La instancia de <see cref="IUnitOfWork"/> de la cual se desea obtener la vista de depuración.</param>
    /// <returns>
    /// Una cadena que representa la vista de depuración del rastreador de cambios, 
    /// o <c>null</c> si <paramref name="source"/> no es un <see cref="DbContext"/>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el objeto <paramref name="source"/> es nulo y lanza una excepción 
    /// si es necesario. Si <paramref name="source"/> es un <see cref="DbContext"/>, 
    /// se llama a <see cref="DbContext.ChangeTracker.DetectChanges"/> para asegurarse de que 
    /// todos los cambios se hayan detectado antes de obtener la vista de depuración.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es nulo.</exception>
    public static string GetChangeTrackerDebugView( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if( source is not DbContext unitOfWork )
            return null;
        unitOfWork.ChangeTracker.DetectChanges();
        return unitOfWork.ChangeTracker.DebugView.LongView;
    }

    /// <summary>
    /// Determina si hay cambios en el contexto de la unidad de trabajo.
    /// </summary>
    /// <param name="source">La unidad de trabajo que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si hay entidades modificadas en el contexto; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método comprueba si el objeto proporcionado es de tipo <see cref="DbContext"/> 
    /// y utiliza el <see cref="ChangeTracker"/> para verificar si hay entradas con estado modificado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="source"/> es <c>null</c>.
    /// </exception>
    public static bool IsChange( this IUnitOfWork source ) {
        source.CheckNull( nameof( source ) );
        if ( source is not DbContext unitOfWork )
            return false;
        return unitOfWork.ChangeTracker.Entries().Any( t => t.State == EntityState.Modified );
    }

    /// <summary>
    /// Verifica si se puede establecer una conexión con la base de datos.
    /// </summary>
    /// <param name="source">La unidad de trabajo que se va a verificar.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Devuelve <c>true</c> si se puede conectar a la base de datos; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método extiende la interfaz <see cref="IUnitOfWork"/> y utiliza un contexto de base de datos para verificar la conexión.
    /// Si el <paramref name="source"/> no es un <see cref="DbContext"/>, se devolverá <c>false</c>.
    /// </remarks>
    public static async Task<bool> CanConnectAsync( this IUnitOfWork source, CancellationToken cancellationToken = default ) {
        source.CheckNull( nameof( source ) );
        if ( source is not DbContext unitOfWork )
            return false;
        return await unitOfWork.Database.CanConnectAsync( cancellationToken );
    }
}