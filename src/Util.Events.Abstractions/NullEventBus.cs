namespace Util.Events;

/// <summary>
/// Representa un contenedor de inversión de control (IoC) que permite la gestión de dependencias.
/// </summary>
/// <remarks>
/// Este contenedor facilita la inyección de dependencias y la resolución de instancias de clases,
/// promoviendo un diseño desacoplado y más fácil de probar.
/// </remarks>
/// <typeparam name="TService">El tipo del servicio que se desea resolver.</typeparam>
/// <seealso cref="IDependencyResolver"/>
[Ioc(-9)]
public class NullEventBus : IEventBus {
    public static readonly IEventBus Instance = new NullEventBus();

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa la interfaz <see cref="IEvent"/>.</typeparam>
    /// <param name="event">El evento que se va a 
    public Task PublishAsync<TEvent>( TEvent @event, CancellationToken cancellationToken = default ) where TEvent : IEvent {
        return Task.CompletedTask;
    }
}

/// <summary>
/// Representa un atributo que indica que una clase o un método debe ser gestionado por un contenedor de inversión de control (IoC).
/// </summary>
/// <remarks>
/// Este atributo se utiliza para marcar clases o métodos que deben ser inyectados o gestionados por un contenedor IoC,
/// facilitando la implementación de patrones de diseño como la inyección de dependencias.
/// </remarks>
/// <param name="priority">La prioridad de la inyección. Un valor negativo indica que la inyección tiene una prioridad baja.</param>
[Ioc( -9 )]
public class NullLocalEventBus : ILocalEventBus {
    public static readonly ILocalEventBus Instance = new NullLocalEventBus();

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de tipo <typeparamref name="TEvent"/> de manera asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que se va a 
    public Task PublishAsync<TEvent>( TEvent @event, CancellationToken cancellationToken = default ) where TEvent : IEvent {
        return Task.CompletedTask;
    }
}

/// <summary>
/// Representa una clase que gestiona la inversión de control (IoC) para la inyección de dependencias.
/// </summary>
/// <remarks>
/// Esta clase permite registrar y resolver dependencias de manera eficiente,
/// facilitando la creación de aplicaciones más modulares y mantenibles.
/// </remarks>
/// <param name="value">El valor que se utilizará para la configuración de IoC.</param>
/// <seealso cref="SomeOtherClass"/>
[Ioc( -9 )]
public class NullIntegrationEventBus : IIntegrationEventBus {
    public static readonly IIntegrationEventBus Instance = new NullIntegrationEventBus();

    /// <inheritdoc />
    /// <summary>
    /// Envía el evento de integración de forma inmediata.
    /// </summary>
    /// <param name="isSend">Indica si se debe enviar el evento. El valor predeterminado es verdadero.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Este método permite enviar eventos de integración de manera sincrónica.
    /// </remarks>
    public IIntegrationEventBus SendNow( bool isSend = true ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre del sistema de 
    public IIntegrationEventBus PubsubName( string pubsubName ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tema para el bus de eventos de integración.
    /// </summary>
    /// <param name="topic">El tema que se asignará al bus de eventos.</param>
    /// <returns>Una instancia del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite configurar el tema que se utilizará para la 
    public IIntegrationEventBus Topic( string topic ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un encabezado para el evento de integración.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a establecer.</param>
    /// <param name="value">El valor del encabezado que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite agregar información adicional a los eventos que se envían a través del bus de eventos.
    /// Los encabezados pueden ser utilizados para transportar metadatos relevantes para el procesamiento del evento.
    /// </remarks>
    public IIntegrationEventBus Header( string key, string value ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los encabezados para el bus de eventos de integración.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a establecer.</param>
    /// <returns>Una instancia del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite configurar los encabezados que se enviarán junto con los eventos.
    /// </remarks>
    public IIntegrationEventBus Header( IDictionary<string, string> headers ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Importa el encabezado de un evento de integración.
    /// </summary>
    /// <param name="key">La clave que identifica el encabezado a importar.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> que representa el bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite importar un encabezado específico utilizando la clave proporcionada.
    /// </remarks>
    /// <seealso cref="IIntegrationEventBus"/>
    public IIntegrationEventBus ImportHeader( string key ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Importa el encabezado de eventos de integración utilizando una colección de claves.
    /// </summary>
    /// <param name="keys">Una colección de cadenas que representan las claves para importar el encabezado.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> que representa el bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite importar un encabezado basado en las claves proporcionadas.
    /// </remarks>
    public IIntegrationEventBus ImportHeader( IEnumerable<string> keys ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un encabezado del bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea eliminar.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite remover un encabezado previamente agregado al bus de eventos.
    /// </remarks>
    public IIntegrationEventBus RemoveHeader( string key ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un par de metadatos para el bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del metadato que se desea establecer.</param>
    /// <param name="value">El valor asociado a la clave del metadato.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite agregar información adicional que puede ser utilizada por el sistema
    /// para identificar o clasificar eventos de integración.
    /// </remarks>
    public IIntegrationEventBus Metadata( string key, string value ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los metadatos para el bus de eventos de integración.
    /// </summary>
    /// <param name="metadata">Un diccionario que contiene pares clave-valor representando los metadatos.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite agregar información adicional que puede ser utilizada por los suscriptores de eventos.
    /// </remarks>
    public IIntegrationEventBus Metadata( IDictionary<string, string> metadata ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina la metadata asociada a la clave especificada.
    /// </summary>
    /// <param name="key">La clave de la metadata que se desea eliminar.</param>
    /// <returns>Una instancia del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite limpiar la metadata del bus de eventos, lo que puede ser útil
    /// para gestionar la información que se almacena y asegurar que solo se mantenga la
    /// información relevante.
    /// </remarks>
    public IIntegrationEventBus RemoveMetadata( string key ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de evento de integración.
    /// </summary>
    /// <param name="value">El valor que representa el tipo de evento.</param>
    /// <returns>Una instancia del bus de eventos de integración.</returns>
    /// <remarks>
    /// Este método permite definir el tipo de evento que se utilizará en el bus de eventos de integración.
    /// </remarks>
    public IIntegrationEventBus Type( string value ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una acción que se ejecutará antes de procesar un evento de integración.
    /// </summary>
    /// <param name="action">La función que se ejecutará antes de procesar el evento. Debe aceptar un evento de integración, un diccionario de parámetros y otro diccionario de resultados, y devolver un valor booleano que indica si continuar o no.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Esta función permite a los desarrolladores interceptar el flujo de procesamiento de eventos de integración y realizar acciones personalizadas antes de que se complete el procesamiento.
    /// </remarks>
    public IIntegrationEventBus OnBefore( Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, bool> action ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una acción que se ejecutará después de que se procese un evento de integración.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe un evento de integración y dos diccionarios de cadenas como parámetros.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/> para permitir la encadenación de llamadas.</returns>
    public IIntegrationEventBus OnAfter( Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, Task> action ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Publica un evento de integración de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que se va a 
    public Task PublishAsync<TEvent>( TEvent @event, CancellationToken cancellationToken = default ) where TEvent : IIntegrationEvent {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    /// <summary>
    /// Vuelve a 
    public Task RepublishAsync( string eventId, CancellationToken cancellationToken = default ) {
        return Task.CompletedTask;
    }
}