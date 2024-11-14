namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Clase que implementa el almacenamiento de eventos de integración.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar el registro y la recuperación de eventos de integración
/// en un sistema distribuido, permitiendo la persistencia y el seguimiento de eventos
/// que cruzan los límites de los servicios.
/// </remarks>
public class IntegrationEventLogStore : IIntegrationEventLogStore {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IntegrationEventLogStore"/>.
    /// </summary>
    /// <param name="stateManager">El administrador de estado que se utilizará para gestionar el estado de los eventos de integración.</param>
    /// <param name="options">Las opciones de configuración para Dapr, que incluyen la configuración del almacén de eventos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="stateManager"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor establece el nombre del almacén de eventos utilizando la configuración proporcionada en <paramref name="options"/>.
    /// Si no se proporciona un nombre de almacén específico, se utilizará el valor predeterminado "statestore".
    /// </remarks>
    public IntegrationEventLogStore( IStateManager stateManager, IOptions<DaprOptions> options ) {
        StateManager = stateManager ?? throw new ArgumentNullException( nameof( stateManager ) );
        StateManager.StoreName( options?.Value.Pubsub?.EventLogStoreName ?? "statestore" );
    }

    public const string KeyCount = "IntegrationEventCount";
    protected IStateManager StateManager;

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un registro de evento de integración de forma asíncrona utilizando el identificador del evento.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se desea recuperar.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene el registro de evento de integración correspondiente al <paramref name="eventId"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="StateManager"/> para recuperar el registro de evento de integración desde la fuente de datos.
    /// </remarks>
    /// <seealso cref="StateManager"/>
    public virtual async Task<IntegrationEventLog> GetAsync( string eventId, CancellationToken cancellationToken = default ) {
        return await StateManager.GetByIdAsync<IntegrationEventLog>( eventId, cancellationToken );
    }

    /// <inheritdoc />
    /// <summary>
    /// Guarda de manera asíncrona un registro de evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro de evento de integración que se va a guardar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el registro.</returns>
    /// <remarks>
    /// Este método utiliza el <see cref="StateManager"/> para realizar la operación de guardado.
    /// </remarks>
    public virtual async Task SaveAsync( IntegrationEventLog eventLog, CancellationToken cancellationToken = default ) {
        await StateManager.SaveAsync( eventLog, cancellationToken );
    }

    /// <inheritdoc />
    /// <summary>
    /// Incrementa el contador de eventos de integración de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    /// <remarks>
    /// Este método obtiene el conteo actual de eventos de integración, incrementa el valor en uno y guarda el nuevo conteo.
    /// </remarks>
    /// <seealso cref="GetIntegrationEventCount"/>
    /// <seealso cref="StateManager.SaveAsync"/>
    public virtual async Task IncrementAsync( CancellationToken cancellationToken = default ) {
        var result = await GetIntegrationEventCount( cancellationToken );
        result.Count = result.Count.SafeValue() + 1;
        await StateManager.SaveAsync( result, cancellationToken, KeyCount );
    }

    /// <summary>
    /// Obtiene el conteo de eventos de integración de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventCount"/> que representa el conteo de eventos de integración.
    /// Si no se encuentra un conteo existente, se devuelve un nuevo objeto <see cref="IntegrationEventCount"/> por defecto.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="StateManager"/> para recuperar el conteo de eventos de integración almacenado.
    /// </remarks>
    protected async Task<IntegrationEventCount> GetIntegrationEventCount(CancellationToken cancellationToken)
    {
        return await StateManager.GetAsync<IntegrationEventCount>(KeyCount, cancellationToken) ?? new IntegrationEventCount();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el conteo de eventos de integración de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{int}"/> que representa el resultado asíncrono de la operación, 
    /// conteniendo el número de eventos de integración.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="GetIntegrationEventCount(CancellationToken)"/> 
    /// para obtener el conteo y asegura que el valor devuelto sea seguro mediante 
    /// el uso de <see cref="SafeValue()"/>.
    /// </remarks>
    public virtual async Task<int> GetCountAsync( CancellationToken cancellationToken = default ) {
        var result = await GetIntegrationEventCount( cancellationToken );
        return result.Count.SafeValue();
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina de manera asíncrona el conteo almacenado en el StateManager.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de eliminación del conteo.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas.
    /// </remarks>
    public virtual async Task ClearCountAsync( CancellationToken cancellationToken = default ) {
        await StateManager.RemoveAsync( KeyCount, cancellationToken );
    }
}