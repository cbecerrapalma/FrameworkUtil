namespace Util.Microservices.Events;

/// <summary>
/// Interfaz que define un gestor de eventos de integración.
/// </summary>
/// <remarks>
/// Esta interfaz es utilizada para manejar eventos de integración en el sistema.
/// Debe ser implementada por cualquier clase que desee gestionar eventos de forma transitoria.
/// </remarks>
public interface IIntegrationEventManager : ITransientDependency {
    /// <summary>
    /// Incrementa un valor de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de incremento.</returns>
    /// <remarks>
    /// Este método permite incrementar un valor de forma no bloqueante. 
    /// Si se proporciona un <paramref name="cancellationToken"/>, 
    /// la operación puede ser cancelada si es necesario.
    /// </remarks>
    /// <exception cref="OperationCanceledException">
    /// Se lanza si la operación es cancelada a través del <paramref name="cancellationToken"/>.
    /// </exception>
    Task IncrementAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Asynchronously obtiene el conteo de elementos.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene el conteo de elementos como un entero.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para obtener el número total de elementos en una colección o fuente de datos.
    /// Si se cancela la operación, se lanzará una excepción de operación cancelada.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<int> GetCountAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Asynchronously restablece el contador a cero.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite restablecer el contador de manera segura en un entorno asíncrono,
    /// asegurando que la operación se pueda cancelar si es necesario.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task ClearCountAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un registro de evento de integración de forma asíncrona utilizando el identificador del evento.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se desea obtener.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro de evento de integración.</returns>
    /// <remarks>
    /// Este método permite recuperar un registro específico de eventos de integración almacenados,
    /// lo que puede ser útil para el seguimiento y la gestión de eventos en sistemas distribuidos.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> GetAsync( string eventId, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda de manera asíncrona un registro de evento de integración en el sistema.
    /// </summary>
    /// <param name="eventLog">El registro de evento de integración que se desea guardar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el registro de evento.</returns>
    /// <remarks>
    /// Este método permite almacenar un registro de evento en la base de datos o en el sistema de almacenamiento correspondiente.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la operación de guardado.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task SaveAsync( IntegrationEventLog eventLog, CancellationToken cancellationToken = default );
    /// <summary>
    /// Verifica si se puede realizar una suscripción para un evento específico.
    /// </summary>
    /// <param name="eventId">El identificador del evento para el cual se desea verificar la suscripción.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor devuelto es verdadero si se puede realizar la suscripción; de lo contrario, falso.</returns>
    /// <remarks>
    /// Este método permite determinar si un usuario o sistema puede suscribirse a un evento dado,
    /// teniendo en cuenta las reglas de negocio y las condiciones actuales del sistema.
    /// </remarks>
    /// <seealso cref="Task"/>
    Task<bool> CanSubscriptionAsync( string eventId, CancellationToken cancellationToken = default );
    /// <summary>
    /// Determina si se puede realizar una suscripción basada en el registro del evento de integración proporcionado.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se evaluará para determinar la posibilidad de suscripción.</param>
    /// <returns>Devuelve <c>true</c> si se puede realizar la suscripción; de lo contrario, devuelve <c>false</c>.</returns>
    /// <remarks>
    /// Este método evalúa las condiciones necesarias para permitir una suscripción, basándose en el estado y los datos del 
    /// registro del evento de integración. Es importante asegurarse de que el <paramref name="eventLog"/> no sea nulo 
    /// antes de llamar a este método para evitar excepciones.
    /// </remarks>
    bool CanSubscription( IntegrationEventLog eventLog );
    /// <summary>
    /// Determina si la suscripción a un evento fue exitosa.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se va a evaluar.</param>
    /// <returns>Devuelve <c>true</c> si la suscripción fue exitosa; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método evalúa el estado del registro del evento de integración
    /// para determinar si la suscripción se completó correctamente.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    bool IsSubscriptionSuccess( IntegrationEventLog eventLog );
    /// <summary>
    /// Crea un registro de log para la 
    Task<IntegrationEventLog> CreatePublishLogAsync( PubsubArgument argument, CancellationToken cancellationToken = default );
    /// <summary>
    /// Crea un registro de suscripción para un evento específico.
    /// </summary>
    /// <param name="eventId">El identificador único del evento para el cual se está creando el registro de suscripción.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se enviará el evento.</param>
    /// <param name="cancellationToken">Token opcional para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro de evento de integración creado.</returns>
    /// <remarks>
    /// Este método es útil para registrar eventos que deben ser enviados a una URL específica.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> CreateSubscriptionLogAsync( string eventId, string routeUrl, CancellationToken cancellationToken = default );
    /// <summary>
    /// Crea un registro de suscripción para un evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se va a suscribir.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se enviará el evento.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro de evento de integración creado.</returns>
    /// <remarks>
    /// Este método se utiliza para registrar un evento de integración que se enviará a una URL específica.
    /// Es útil para mantener un seguimiento de los eventos que han sido enviados y sus respectivos estados.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> CreateSubscriptionLogAsync( IntegrationEventLog eventLog, string routeUrl, CancellationToken cancellationToken = default );
    /// <summary>
    /// Asynchronously maneja el éxito de una suscripción y registra el evento correspondiente.
    /// </summary>
    /// <param name="eventId">El identificador único del evento de suscripción.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro del evento de integración.</returns>
    /// <remarks>
    /// Este método se utiliza para procesar la confirmación de una suscripción y registrar el evento en el sistema.
    /// Asegúrese de que el <paramref name="eventId"/> proporcionado sea válido y esté asociado a una suscripción existente.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> SubscriptionSuccessAsync( string eventId, CancellationToken cancellationToken = default );
    /// <summary>
    /// Procesa la suscripción exitosa de un evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se está procesando.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene el registro de evento de integración.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para manejar la lógica necesaria cuando se recibe una suscripción exitosa,
    /// permitiendo realizar acciones adicionales, como actualizar el estado del registro o notificar otros sistemas.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> SubscriptionSuccessAsync( IntegrationEventLog eventLog, CancellationToken cancellationToken = default );
    /// <summary>
    /// Registra un fallo en la suscripción de un evento.
    /// </summary>
    /// <param name="eventId">El identificador del evento que falló.</param>
    /// <param name="message">Un mensaje que describe el motivo del fallo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro del evento de integración.</returns>
    /// <remarks>
    /// Este método es útil para manejar errores en la suscripción de eventos y permite registrar información relevante 
    /// para el diagnóstico y la resolución de problemas.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> SubscriptionFailAsync( string eventId, string message, CancellationToken cancellationToken = default );
    /// <summary>
    /// Procesa un fallo en la suscripción de un evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que ha fallado.</param>
    /// <param name="message">El mensaje que describe el motivo del fallo.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el registro del evento de integración actualizado.</returns>
    /// <remarks>
    /// Este método se utiliza para manejar los errores que ocurren durante la suscripción a eventos de integración,
    /// permitiendo registrar información adicional sobre el fallo y facilitando la recuperación o el reintento de la operación.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task<IntegrationEventLog> SubscriptionFailAsync( IntegrationEventLog eventLog, string message, CancellationToken cancellationToken = default );
}