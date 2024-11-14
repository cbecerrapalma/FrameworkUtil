namespace Util.Applications.Locks; 

/// <summary>
/// Representa una implementación de cerrojo que no realiza ninguna acción.
/// Esta clase se utiliza cuando no se desea bloquear un recurso.
/// </summary>
public class NullLock : ILock {
    public static readonly ILock Instance = new NullLock();

    /// <inheritdoc />
    /// <summary>
    /// Intenta bloquear un recurso identificado por la clave especificada.
    /// </summary>
    /// <param name="key">La clave que identifica el recurso a bloquear.</param>
    /// <param name="expiration">El tiempo de expiración opcional para el bloqueo. Si no se proporciona, el bloqueo no tendrá un tiempo de expiración.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si el bloqueo fue exitoso.</returns>
    /// <remarks>
    /// Este método es útil para implementar mecanismos de control de concurrencia en aplicaciones donde varios procesos pueden intentar acceder al mismo recurso simultáneamente.
    /// </remarks>
    public Task<bool> LockAsync( string key, TimeSpan? expiration = null ) {
        return Task.FromResult( true );
    }

    /// <inheritdoc />
    /// <summary>
    /// Desbloquea de manera asíncrona un recurso o entidad.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación de desbloqueo. La tarea se completa inmediatamente.
    /// </returns>
    /// <remarks>
    /// Este método no realiza ninguna acción adicional y simplemente devuelve una tarea completada.
    /// </remarks>
    public Task UnLockAsync() {
        return Task.CompletedTask;
    }
}