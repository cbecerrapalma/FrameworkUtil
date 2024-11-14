namespace Util.Events; 

/// <summary>
/// Interfaz que define un bus de eventos de integración.
/// </summary>
/// <remarks>
/// Esta interfaz es utilizada para la 
public interface IIntegrationEventBus : ITransientDependency {
    /// <summary>
    /// Establece el nombre del sistema de 
    IIntegrationEventBus PubsubName( string pubsubName );
    /// <summary>
    /// Establece el tema (topic) para el bus de eventos de integración.
    /// </summary>
    /// <param name="topic">El nombre del tema que se utilizará para la 
    IIntegrationEventBus Topic( string topic );
    /// <summary>
    /// Interfaz que define un bus de eventos de integración.
    /// </summary>
    IIntegrationEventBus Header( string key, string value );
    /// <summary>
    /// Interfaz que define un bus de eventos de integración.
    /// </summary>
    IIntegrationEventBus Header( IDictionary<string, string> headers );
    /// <summary>
    /// Importa un encabezado utilizando la clave proporcionada.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea importar.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> que representa el encabezado importado.</returns>
    /// <remarks>
    /// Este método se utiliza para obtener un encabezado específico del sistema de eventos de integración.
    /// Asegúrese de que la clave proporcionada sea válida y exista en el sistema.
    /// </remarks>
    IIntegrationEventBus ImportHeader(string key);
    /// <summary>
    /// Importa un encabezado utilizando una colección de claves.
    /// </summary>
    /// <param name="keys">Una colección de cadenas que representan las claves del encabezado a importar.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> que representa el evento de integración asociado al encabezado importado.</returns>
    /// <remarks>
    /// Este método permite importar encabezados a partir de un conjunto de claves proporcionadas.
    /// Asegúrese de que las claves sean válidas y estén en el formato correcto antes de llamar a este método.
    /// </remarks>
    IIntegrationEventBus ImportHeader(IEnumerable<string> keys);
    /// <summary>
    /// Elimina un encabezado del bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea eliminar.</param>
    /// <returns>Una instancia del bus de eventos de integración actualizada.</returns>
    /// <remarks>
    /// Este método permite quitar un encabezado previamente agregado al bus de eventos.
    /// Si la clave no existe, no se realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="IIntegrationEventBus"/>
    IIntegrationEventBus RemoveHeader( string key );
    /// <summary>
    /// Interfaz que define un bus de eventos de integración.
    /// </summary>
    IIntegrationEventBus Metadata( string key, string value );
    /// <summary>
    /// Interfaz que define un bus de eventos de integración.
    /// </summary>
    IIntegrationEventBus Metadata( IDictionary<string, string> metadata );
    /// <summary>
    /// Elimina los metadatos asociados a la clave especificada del bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del metadato que se desea eliminar.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> con el metadato eliminado.</returns>
    /// <remarks>
    /// Este método permite gestionar los metadatos del bus de eventos, facilitando la limpieza de información no deseada.
    /// Asegúrese de que la clave proporcionada exista antes de llamar a este método para evitar resultados inesperados.
    /// </remarks>
    IIntegrationEventBus RemoveMetadata(string key);
    /// <summary>
    /// Representa un bus de eventos de integración que permite la 
    IIntegrationEventBus Type( string value );
    /// <summary>
    /// Registra una acción que se ejecutará antes de que se publique un evento de integración.
    /// </summary>
    /// <param name="action">Una función que toma un evento de integración, un diccionario de parámetros y otro diccionario de parámetros, y devuelve un valor booleano que indica si se debe continuar con la 
    IIntegrationEventBus OnBefore( Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, bool> action );
    /// <summary>
    /// Registra una acción que se ejecutará después de que se publique un evento de integración.
    /// </summary>
    /// <param name="action">La función que se ejecutará tras la 
    IIntegrationEventBus OnAfter( Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, Task> action );
    /// <summary>
    /// Publica un evento de integración de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa <see cref="IIntegrationEvent"/>.</typeparam>
    /// <param name="event">El evento que se va a 
    Task PublishAsync<TEvent>( TEvent @event, CancellationToken cancellationToken = default ) where TEvent : IIntegrationEvent;
    /// <summary>
    /// Vuelve a 
    Task RepublishAsync( string eventId, CancellationToken cancellationToken = default );
}