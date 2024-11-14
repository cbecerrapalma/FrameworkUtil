namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Representa un valor de entrada/salida con un rango específico.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para manejar valores que pueden estar dentro de un rango definido,
/// permitiendo operaciones de validación y manipulación de datos.
/// </remarks>
/// <param name="valor">El valor de entrada/salida que se desea establecer.</param>
/// <param name="rangoMinimo">El valor mínimo permitido en el rango.</param>
/// <param name="rangoMaximo">El valor máximo permitido en el rango.</param>
/// <seealso cref="ValidarRango"/>
[Ioc(-9)]
public class NullPubsubCallback : IPubsubCallback {
    public static readonly IPubsubCallback Instance = new NullPubsubCallback();

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta antes de 
    public Task OnPublishBefore( PubsubArgument argument, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta después de 
    public Task OnPublishAfter( PubsubArgument argument, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta antes de que se realice una suscripción.
    /// </summary>
    /// <param name="integrationEventLog">El registro del evento de integración que contiene información sobre el evento.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se está suscribiendo.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método no realiza ninguna acción y completa la tarea inmediatamente.
    /// </remarks>
    public Task OnSubscriptionBefore( IntegrationEventLog integrationEventLog, string routeUrl, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Método que se ejecuta después de una suscripción.
    /// </summary>
    /// <param name="eventId">Identificador del evento relacionado con la suscripción.</param>
    /// <param name="isSuccess">Indica si la suscripción fue exitosa.</param>
    /// <param name="message">Mensaje adicional sobre el resultado de la suscripción.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public Task OnSubscriptionAfter( string eventId, bool isSuccess,string message, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja el evento antes de volver a 
    public Task OnRepublishBefore( IntegrationEventLog eventLog, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Maneja la re
    public Task OnRepublishAfter( IntegrationEventLog eventLog, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }
}