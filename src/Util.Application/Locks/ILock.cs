namespace Util.Applications.Locks; 

/// <summary>
/// Define una interfaz para un mecanismo de bloqueo.
/// </summary>
public interface ILock {
    /// <summary>
    /// Intenta bloquear un recurso identificado por la clave especificada.
    /// </summary>
    /// <param name="key">La clave que identifica el recurso a bloquear.</param>
    /// <param name="expiration">El tiempo opcional que el bloqueo debe durar. Si no se especifica, el bloqueo puede ser indefinido.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de retorno es <c>true</c> si el bloqueo se adquirió con éxito; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método es útil en escenarios donde se necesita garantizar la exclusividad de acceso a un recurso.
    /// Si el recurso ya está bloqueado, el método devolverá <c>false</c> sin esperar a que se libere.
    /// </remarks>
    /// <seealso cref="UnlockAsync(string)"/>
    Task<bool> LockAsync( string key, TimeSpan? expiration = null );
    /// <summary>
    /// Desbloquea de manera asíncrona un recurso o entidad.
    /// </summary>
    /// <remarks>
    /// Este método permite liberar un recurso que ha sido bloqueado previamente,
    /// asegurando que otros procesos puedan acceder a él. Es importante asegurarse
    /// de que el recurso esté efectivamente bloqueado antes de intentar desbloquearlo.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de desbloqueo.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el recurso no está bloqueado o si se intenta desbloquear un recurso
    /// que no pertenece al contexto actual.
    /// </exception>
    /// <seealso cref="LockAsync"/>
    Task UnLockAsync();
}