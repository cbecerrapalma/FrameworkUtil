namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Representa una clase que implementa el patrón de inversión de control (IoC).
/// </summary>
/// <remarks>
/// Esta clase permite la gestión de dependencias y la inyección de servicios 
/// en componentes de la aplicación, facilitando la escalabilidad y el mantenimiento.
/// </remarks>
/// <typeparam name="T">El tipo de servicio que se va a inyectar.</typeparam>
/// <seealso cref="IServiceProvider"/>
[Ioc(1)]
public class DaprEventBus : IEventBus {
    private readonly ILocalEventBus _localEventBus;
    private readonly IIntegrationEventBus _integrationEventBus;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprEventBus"/>.
    /// </summary>
    /// <param name="localEventBus">Una instancia de <see cref="ILocalEventBus"/> que se utilizará para manejar eventos locales.</param>
    /// <param name="integrationEventBus">Una instancia de <see cref="IIntegrationEventBus"/> que se utilizará para manejar eventos de integración.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="localEventBus"/> o <paramref name="integrationEventBus"/> son nulos.</exception>
    public DaprEventBus( ILocalEventBus localEventBus, IIntegrationEventBus integrationEventBus ) {
        _localEventBus = localEventBus ?? throw new ArgumentNullException( nameof( localEventBus ) );
        _integrationEventBus = integrationEventBus ?? throw new ArgumentNullException( nameof( integrationEventBus ) );
    }

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de forma asíncrona en el bus de eventos local y, si es un evento de integración, también lo 
    public async Task PublishAsync<TEvent>( TEvent @event, CancellationToken cancellationToken = default ) where TEvent : IEvent {
        await _localEventBus.PublishAsync( @event, cancellationToken );
        if ( @event is not IIntegrationEvent integrationEvent  )
            return;
        await _integrationEventBus.PublishAsync( integrationEvent, cancellationToken );
    }
}