namespace Util.Microservices.Events; 

/// <summary>
/// Interfaz que define un callback para el sistema de 
public interface IPubsubCallback : ISingletonDependency {
    /// <summary>
    /// Evento que se desencadena antes de 
    Task OnPublishBefore( PubsubArgument argument, CancellationToken cancellationToken = default );
    /// <summary>
    /// Maneja el evento de 
    Task OnPublishAfter( PubsubArgument argument, CancellationToken cancellationToken = default );
    /// <summary>
    /// Maneja la suscripción antes de que se realice un evento de integración.
    /// </summary>
    /// <param name="integrationEventLog">El registro del evento de integración que contiene información sobre el evento que se va a procesar.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se enviará el evento de integración.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación. El valor predeterminado es <c langword="default"/>.</param>
    /// <remarks>
    /// Este método se utiliza para preparar y validar la información necesaria antes de que se procese el evento de integración.
    /// Asegúrese de que el registro del evento de integración esté correctamente configurado antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    Task OnSubscriptionBefore( IntegrationEventLog integrationEventLog,string routeUrl, CancellationToken cancellationToken = default );
    /// <summary>
    /// Maneja la suscripción después de un evento.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se ha procesado.</param>
    /// <param name="isSuccess">Indica si la operación fue exitosa.</param>
    /// <param name="message">Un mensaje que proporciona información adicional sobre el resultado de la operación.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación.</param>
    /// <remarks>
    /// Este método se invoca después de que se ha completado una suscripción, permitiendo realizar acciones adicionales
    /// basadas en el resultado de la operación.
    /// </remarks>
    Task OnSubscriptionAfter( string eventId, bool isSuccess, string message, CancellationToken cancellationToken = default );
    /// <summary>
    /// Maneja el evento de re
    Task OnRepublishBefore( IntegrationEventLog eventLog, CancellationToken cancellationToken = default );
    /// <summary>
    /// Maneja la re
    Task OnRepublishAfter( IntegrationEventLog eventLog, CancellationToken cancellationToken = default );
}