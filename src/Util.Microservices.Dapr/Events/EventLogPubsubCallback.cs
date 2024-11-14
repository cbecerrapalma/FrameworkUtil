namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Clase que implementa la interfaz <see cref="IPubsubCallback"/> para manejar 
/// los eventos de 
public class EventLogPubsubCallback : IPubsubCallback
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EventLogPubsubCallback"/>.
    /// </summary>
    /// <param name="manager">Una instancia de <see cref="IIntegrationEventManager"/> que se utilizará para gestionar eventos de integración.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="manager"/> es <c>null</c>.</exception>
    public EventLogPubsubCallback(IIntegrationEventManager manager)
    {
        Manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    protected IIntegrationEventManager Manager;

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta antes de 
    public virtual Task OnPublishBefore(PubsubArgument argument, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta después de 
    public virtual async Task OnPublishAfter(PubsubArgument argument, CancellationToken cancellationToken = default)
    {
        var result = await Manager.CreatePublishLogAsync(argument, cancellationToken);
        if (result == NullIntegrationEventLog.Instance)
            return;
        await Manager.IncrementAsync(cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta antes de procesar una suscripción.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que contiene información sobre la suscripción.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se enviará la suscripción.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    /// <remarks>
    /// Este método crea un registro de la suscripción utilizando el administrador de suscripciones.
    /// </remarks>
    /// <seealso cref="Manager"/>
    public virtual async Task OnSubscriptionBefore(IntegrationEventLog eventLog, string routeUrl, CancellationToken cancellationToken = default)
    {
        await Manager.CreateSubscriptionLogAsync(eventLog, routeUrl, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja la lógica que se debe ejecutar después de una suscripción, dependiendo del resultado de la misma.
    /// </summary>
    /// <param name="eventId">El identificador del evento de suscripción.</param>
    /// <param name="isSuccess">Indica si la suscripción fue exitosa o no.</param>
    /// <param name="message">Un mensaje que describe el resultado de la suscripción, utilizado en caso de fallo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método se invoca después de que se ha intentado realizar una suscripción, y se encarga de llamar a los métodos correspondientes
    /// en el gestor de suscripciones, dependiendo de si la operación fue exitosa o fallida.
    /// </remarks>
    /// <seealso cref="Manager.SubscriptionSuccessAsync(string, CancellationToken)"/>
    /// <seealso cref="Manager.SubscriptionFailAsync(string, string, CancellationToken)"/>
    public virtual async Task OnSubscriptionAfter(string eventId, bool isSuccess, string message, CancellationToken cancellationToken = default)
    {
        if (isSuccess)
        {
            await Manager.SubscriptionSuccessAsync(eventId, cancellationToken);
            return;
        }
        await Manager.SubscriptionFailAsync(eventId, message, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta antes de volver a 
    public virtual Task OnRepublishBefore(IntegrationEventLog eventLog, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se invoca después de la re
    public virtual Task OnRepublishAfter(IntegrationEventLog eventLog, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}