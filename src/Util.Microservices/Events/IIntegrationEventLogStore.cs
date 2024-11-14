namespace Util.Microservices.Events;

/// <summary>
/// Define una interfaz para el almacenamiento de registros de eventos de integración.
/// </summary>
/// <remarks>
/// Esta interfaz es utilizada para gestionar el almacenamiento de eventos de integración,
/// permitiendo la persistencia y recuperación de los mismos en un sistema de mensajería.
/// </remarks>
public interface IIntegrationEventLogStore : ITransientDependency {
    /// <summary>
    /// Obtiene un registro de evento de integración de forma asíncrona utilizando el identificador del evento.
    /// </summary>
    /// <param name="eventId">El identificador único del evento que se desea obtener.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea contiene el registro de evento de integración solicitado.</returns>
    /// <remarks>
    /// Este método permite recuperar información sobre un evento de integración específico, 
    /// lo cual puede ser útil para auditoría, seguimiento o reintentos de procesamiento de eventos.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> GetAsync( string eventId, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda de manera asíncrona un registro de evento de integración en el sistema.
    /// </summary>
    /// <param name="eventLog">El registro de evento de integración que se desea guardar.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método permite almacenar un registro de evento en la base de datos o en otro almacenamiento persistente,
    /// asegurando que la operación se pueda cancelar si es necesario.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task SaveAsync( IntegrationEventLog eventLog, CancellationToken cancellationToken = default );
    /// <summary>
    /// Incrementa un valor de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de incremento.</returns>
    /// <remarks>
    /// Este método permite incrementar un valor en un contexto asíncrono, lo que permite que la operación no bloquee el hilo de ejecución actual.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias si se solicita la cancelación.
    /// </remarks>
    Task IncrementAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene el conteo de elementos de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un entero que indica el conteo de elementos.</returns>
    /// <remarks>
    /// Este método permite realizar una operación de conteo sin bloquear el hilo de ejecución actual.
    /// Se recomienda manejar el token de cancelación para permitir que la operación se detenga si es necesario.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<int> GetCountAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Asynchronously restablece el contador a cero.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es un valor booleano que indica si el contador fue restablecido exitosamente.
    /// </returns>
    /// <remarks>
    /// Este método puede ser llamado en cualquier momento para reiniciar el contador. Si se proporciona un token de cancelación y se cancela la operación, el método puede lanzar una excepción.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task ClearCountAsync( CancellationToken cancellationToken = default );
}