using Util.Helpers;

namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Clase que implementa la interfaz <see cref="IIntegrationEventBus"/>.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar la 
public class DaprIntegrationEventBus : IIntegrationEventBus
{

    #region Campo

    protected readonly DaprClient Client;
    protected DaprOptions Options;
    protected readonly ILogger Logger;
    protected IPubsubCallback PubsubCallback;
    protected IIntegrationEventManager EventManager;
    protected string Pubsub;
    protected string TopicName;
    protected string CloudEventType;
    protected Dictionary<string, string> Headers;
    protected List<string> ImportHeaderKeys;
    protected IList<string> RemoveHeaderKeys;
    protected Dictionary<string, string> Metadatas;
    protected Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, bool> OnBeforeAction;
    protected Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, Task> OnAfterAction;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprIntegrationEventBus"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr utilizado para la comunicación.</param>
    /// <param name="options">Las opciones de configuración para Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de logger.</param>
    /// <param name="serviceProvider">El proveedor de servicios utilizado para resolver dependencias.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="client"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Esta clase se encarga de gestionar la integración con el bus de eventos de Dapr,
    /// permitiendo la 
    public DaprIntegrationEventBus(DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Options = options?.Value ?? new DaprOptions();
        Logger = loggerFactory?.CreateLogger(typeof(DaprEventBus)) ?? NullLogger<DaprEventBus>.Instance;
        serviceProvider.CheckNull(nameof(serviceProvider));
        PubsubCallback = serviceProvider.GetService<IPubsubCallback>() ?? NullPubsubCallback.Instance;
        EventManager = serviceProvider.GetRequiredService<IIntegrationEventManager>();
        Headers = new Dictionary<string, string>();
        ImportHeaderKeys = new List<string>();
        RemoveHeaderKeys = new List<string>();
        Metadatas = new Dictionary<string, string>();
    }

    #endregion

    #region PubsubName

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre del sistema de 
    public IIntegrationEventBus PubsubName(string pubsubName)
    {
        Pubsub = pubsubName;
        return this;
    }

    #endregion

    #region Topic

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre del tema para el bus de eventos de integración.
    /// </summary>
    /// <param name="topic">El nombre del tema que se va a establecer.</param>
    /// <returns>Una instancia del bus de eventos de integración.</returns>
    /// <inheritdoc />
    public IIntegrationEventBus Topic(string topic)
    {
        TopicName = topic;
        return this;
    }

    #endregion

    #region Header

    /// <inheritdoc />
    /// <summary>
    /// Establece un encabezado en el bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a establecer.</param>
    /// <param name="value">El valor del encabezado que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Si la clave proporcionada está vacía, no se realiza ninguna acción.
    /// Si la clave ya existe, se eliminará antes de agregar el nuevo valor.
    /// </remarks>
    /// <seealso cref="IIntegrationEventBus"/>
    public IIntegrationEventBus Header(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (Headers.ContainsKey(key))
            Headers.Remove(key);
        Headers.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los encabezados para el bus de eventos de integración.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a establecer, donde la clave es el nombre del encabezado y el valor es su contenido.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="headers"/> es nulo, no se realizarán cambios en los encabezados.
    /// </remarks>
    public IIntegrationEventBus Header(IDictionary<string, string> headers)
    {
        if (headers == null)
            return this;
        foreach (var header in headers)
            Header(header.Key, header.Value);
        return this;
    }

    #endregion

    #region ImportHeader

    /// <summary>
    /// Importa un encabezado utilizando la clave proporcionada.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a importar.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. Si la clave no se encuentra en la colección de claves de encabezado de importación,
    /// se agrega a dicha colección.
    /// </remarks>
    public IIntegrationEventBus ImportHeader(string key)
    {
        if (key.IsEmpty())
            return this;
        if (ImportHeaderKeys.Contains(key) == false)
            ImportHeaderKeys.Add(key);
        return this;
    }

    /// <summary>
    /// Importa un encabezado utilizando una colección de claves.
    /// </summary>
    /// <param name="keys">Una colección de claves que se utilizarán para importar el encabezado.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Si la colección de claves es nula, el método simplemente devuelve la instancia actual sin realizar ninguna acción.
    /// Para cada clave en la colección, se llama al método <see cref="ImportHeader(string)"/>.
    /// </remarks>
    public IIntegrationEventBus ImportHeader(IEnumerable<string> keys)
    {
        if (keys == null)
            return this;
        foreach (var key in keys)
            ImportHeader(key);
        return this;
    }

    #endregion

    #region RemoveHeader

    /// <summary>
    /// Elimina un encabezado de la lista de encabezados a eliminar.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea eliminar.</param>
    /// <returns>La instancia actual de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Si la clave proporcionada está vacía, no se realiza ninguna acción.
    /// Si la clave no está presente en la lista de claves a eliminar, se agrega.
    /// </remarks>
    public IIntegrationEventBus RemoveHeader(string key)
    {
        if (key.IsEmpty())
            return this;
        if (RemoveHeaderKeys.Contains(key) == false)
            RemoveHeaderKeys.Add(key);
        return this;
    }

    #endregion

    #region Metadata

    /// <inheritdoc />
    /// <summary>
    /// Establece un par clave-valor en los metadatos del bus de eventos de integración.
    /// </summary>
    /// <param name="key">La clave del metadato que se desea establecer.</param>
    /// <param name="value">El valor asociado a la clave del metadato.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Si la clave o el valor están vacíos, no se realiza ninguna acción.
    /// Si la clave ya existe, se eliminará el metadato existente antes de agregar el nuevo.
    /// </remarks>
    /// <seealso cref="RemoveMetadata(string)"/>
    public IIntegrationEventBus Metadata(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (value.IsEmpty())
            return this;
        RemoveMetadata(key);
        Metadatas.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los metadatos para el bus de eventos de integración.
    /// </summary>
    /// <param name="metadata">Un diccionario que contiene pares clave-valor que representan los metadatos.</param>
    /// <returns>Devuelve la instancia actual del bus de eventos de integración.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="metadata"/> es nulo, el método simplemente devuelve la instancia actual sin realizar ninguna acción.
    /// </remarks>
    public IIntegrationEventBus Metadata(IDictionary<string, string> metadata)
    {
        if (metadata == null)
            return this;
        foreach (var item in metadata)
            Metadata(item.Key, item.Value);
        return this;
    }

    #endregion

    #region RemoveMetadata

    /// <inheritdoc />
    /// <summary>
    /// Elimina la metadata asociada a la clave especificada.
    /// </summary>
    /// <param name="key">La clave de la metadata que se desea eliminar.</param>
    /// <returns>Una instancia del bus de eventos de integración actual.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. 
    /// Si la clave existe en el diccionario de metadatas, se elimina.
    /// </remarks>
    /// <seealso cref="IIntegrationEventBus"/>
    public IIntegrationEventBus RemoveMetadata(string key)
    {
        if (key.IsEmpty())
            return this;
        if (Metadatas.ContainsKey(key))
            Metadatas.Remove(key);
        return this;
    }

    #endregion

    #region Type

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de evento de integración.
    /// </summary>
    /// <param name="value">El tipo de evento que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="IIntegrationEventBus"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite configurar el tipo de evento que se utilizará en el bus de eventos de integración.
    /// </remarks>
    public IIntegrationEventBus Type(string value)
    {
        CloudEventType = value;
        return this;
    }

    #endregion

    #region OnBefore

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará antes de que se procese un evento de integración.
    /// </summary>
    /// <param name="action">La función que se ejecutará antes de procesar el evento. Debe aceptar un evento de integración, un diccionario de parámetros y otro diccionario de resultados, y devolver un valor booleano que indique si continuar o no con el procesamiento.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Esta función permite a los desarrolladores interceptar el flujo de procesamiento de eventos y realizar acciones personalizadas antes de que se ejecute el evento.
    /// </remarks>
    public IIntegrationEventBus OnBefore(Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, bool> action)
    {
        OnBeforeAction = action;
        return this;
    }

    #endregion

    #region OnAfter

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará después de que se procese un evento de integración.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que toma un evento de integración y dos diccionarios de cadenas como parámetros y devuelve una tarea.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IIntegrationEventBus"/>.</returns>
    /// <remarks>
    /// Esta función permite a los suscriptores definir lógica adicional que se ejecutará después de que se complete el procesamiento del evento.
    /// </remarks>
    public IIntegrationEventBus OnAfter(Func<IIntegrationEvent, Dictionary<string, string>, Dictionary<string, string>, Task> action)
    {
        OnAfterAction = action;
        return this;
    }

    #endregion

    #region PublishAsync

    /// <inheritdoc />
    /// <summary>
    /// Publica de manera asíncrona un evento de integración.
    /// </summary>
    /// <typeparam name="TEvent">El tipo del evento que implementa <see cref="IIntegrationEvent"/>.</typeparam>
    /// <param name="event">El evento que se va a 
    public virtual async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
    {
        @event.CheckNull(nameof(@event));
        cancellationToken.ThrowIfCancellationRequested();
        ImportHeaders();
        Init(@event);
        if (OnBeforeAction != null && OnBeforeAction(@event, Headers, Metadatas) == false)
            return;
        await PublishEventAsync(@event, cancellationToken);
    }

    /// <summary>
    /// Importa los encabezados necesarios para el procesamiento.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación personalizada de la importación de encabezados.
    /// Primero, se eliminan los encabezados existentes y luego se establecen los nuevos encabezados.
    /// </remarks>
    protected virtual void ImportHeaders()
    {
        RemoveHeaders();
        SetHeaders();
    }

    /// <summary>
    /// Elimina las claves de encabezado especificadas de las colecciones de encabezados.
    /// </summary>
    /// <remarks>
    /// Este método recorre una lista de claves de encabezado a eliminar y las elimina de las colecciones 
    /// de encabezados, claves de encabezado de importación y claves de encabezado de opciones de 
    private void RemoveHeaders()
    {
        foreach (var key in RemoveHeaderKeys)
        {
            Headers.Remove(key);
            ImportHeaderKeys.Remove(key);
            Options.Pubsub.ImportHeaderKeys.Remove(key);
        }
    }

    /// <summary>
    /// Establece los encabezados para la importación.
    /// </summary>
    /// <remarks>
    /// Este método obtiene los encabezados de importación mediante el método <see cref="GetImportHeaders"/> 
    /// y los agrega a la colección de encabezados.
    /// </remarks>
    private void SetHeaders()
    {
        var headers = GetImportHeaders();
        foreach (var item in headers)
            Headers.TryAdd(item.Key, item.Value);
    }

    /// <summary>
    /// Obtiene los encabezados de importación a partir de las claves definidas en las opciones de Pubsub.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene los encabezados de importación, donde la clave es el nombre del encabezado y el valor es su contenido.
    /// Si no se encuentran encabezados, se devuelve un diccionario vacío.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si hay claves de encabezado disponibles y si hay encabezados en la solicitud web.
    /// Solo se agregarán al resultado aquellos encabezados que existan en la solicitud.
    /// </remarks>
    private IDictionary<string, string> GetImportHeaders()
    {
        var result = new Dictionary<string, string>();
        ImportHeaderKeys.AddRange(Options.Pubsub.ImportHeaderKeys);
        if (ImportHeaderKeys.Count == 0)
            return result;
        var headers = Web.Request?.Headers;
        if (headers == null)
            return result;
        foreach (var key in ImportHeaderKeys.Distinct())
        {
            if (headers.TryGetValue(key, out var value))
                result.Add(key, value);
        }
        return result;
    }

    /// <summary>
    /// Inicializa los parámetros necesarios para el evento de integración.
    /// </summary>
    /// <param name="integrationEvent">El evento de integración que se va a inicializar.</param>
    protected void Init(IIntegrationEvent integrationEvent)
    {
        InitPubsubName(integrationEvent);
        InitTopic(integrationEvent);
    }

    /// <summary>
    /// Inicializa el nombre del sistema de 
    protected void InitPubsubName(IIntegrationEvent integrationEvent)
    {
        if (Pubsub.IsEmpty() == false)
            return;
        Pubsub = PubsubNameAttribute.GetName(integrationEvent.GetType());
    }

    /// <summary>
    /// Inicializa el nombre del tema basado en el evento de integración proporcionado.
    /// </summary>
    /// <param name="integrationEvent">El evento de integración del cual se extraerá el nombre del tema.</param>
    /// <remarks>
    /// Este método verifica si el nombre del tema ya ha sido inicializado. Si no es así, 
    /// utiliza el atributo de nombre del tema para establecer el nombre basado en el tipo 
    /// del evento de integración.
    /// </remarks>
    protected void InitTopic(IIntegrationEvent integrationEvent)
    {
        if (TopicName.IsEmpty() == false)
            return;
        TopicName = TopicNameAttribute.GetName(integrationEvent.GetType());
    }

    /// <summary>
    /// Publica un evento de integración de manera asíncrona.
    /// </summary>
    /// <param name="event">El evento de integración que se va a 
    protected virtual async Task PublishEventAsync(IIntegrationEvent @event, CancellationToken cancellationToken)
    {
        Logger.LogTrace("Publicando evento {@Event} en {PubsubName}.{Topic}", @event, Pubsub, TopicName);
        var cloudEvent = CreateCloudEvent(@event);
        var argument = CreatePubsubArgument(cloudEvent);
        await PubsubCallback.OnPublishBefore(argument, cancellationToken);
        await Client.PublishEventAsync(Pubsub, TopicName, cloudEvent, Metadatas, cancellationToken);
        await PubsubCallback.OnPublishAfter(argument, cancellationToken);
        if (OnAfterAction != null)
            await OnAfterAction(@event, Headers, Metadatas);
    }

    /// <summary>
    /// Crea un nuevo evento en la nube a partir de un evento de integración.
    /// </summary>
    /// <param name="event">El evento de integración que se utilizará para crear el evento en la nube.</param>
    /// <returns>Un objeto <see cref="CloudEvent{T}"/> que representa el evento en la nube creado.</returns>
    /// <remarks>
    /// Este método toma un evento de integración y lo convierte en un evento en la nube,
    /// estableciendo el identificador del evento y otros parámetros relevantes como el tipo
    /// de evento y los encabezados, si están disponibles.
    /// </remarks>
    protected CloudEvent<object> CreateCloudEvent(IIntegrationEvent @event)
    {
        var result = new CloudEvent<object>(@event.EventId, @event)
        {
            Type = CloudEventType,
            Headers = Headers.Count > 0 ? Headers : null
        };
        return result;
    }

    /// <summary>
    /// Crea un argumento de Pubsub a partir de un evento de Cloud.
    /// </summary>
    /// <param name="cloudEvent">El evento de Cloud del cual se generará el argumento de Pubsub.</param>
    /// <returns>
    /// Un objeto de tipo <see cref="PubsubArgument"/> que contiene la información del evento de Cloud y metadatos si están disponibles.
    /// </returns>
    /// <remarks>
    /// Si la colección de metadatos contiene elementos, se incluirán en el argumento de Pubsub.
    /// De lo contrario, se creará el argumento sin metadatos.
    /// </remarks>
    protected PubsubArgument CreatePubsubArgument(CloudEvent<object> cloudEvent)
    {
        return Metadatas.Count > 0 ? new PubsubArgument(Pubsub, TopicName, cloudEvent, Metadatas) : new PubsubArgument(Pubsub, TopicName, cloudEvent);
    }

    #endregion

    #region RepublishAsync

    /// <inheritdoc />
    /// <summary>
    /// Vuelve a 
    public virtual async Task RepublishAsync(string eventId, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return;
        if (eventId.IsEmpty())
        {
            Logger.LogWarning("Republicación de eventos de integración fallida, el identificador del evento no puede estar vacío.");
            return;
        }
        var eventLog = await EventManager.GetAsync(eventId, cancellationToken);
        await RepublishAsync(eventLog, cancellationToken);
    }

    /// <summary>
    /// Vuelve a 
    protected virtual async Task RepublishAsync(IntegrationEventLog eventLog, CancellationToken cancellationToken = default)
    {
        eventLog.CheckNull(nameof(eventLog));
        eventLog.SubscriptionLogs.ForEach(t =>
        {
            if (t.State == SubscriptionState.Fail)
                t.RetryCount = 0;
        });
        await EventManager.SaveAsync(eventLog, cancellationToken);
        var json = Util.Helpers.Json.ToJson(eventLog.Value);
        var argument = Util.Helpers.Json.ToObject<PubsubArgument<CloudEvent<object>>>(json);
        var cloudEvent = argument.GetEventData<CloudEvent<object>>();
        Logger.LogTrace("Republishing event {@Event} to {PubsubName}.{Topic}", cloudEvent.Data, eventLog.PubsubName, eventLog.Topic);
        await PubsubCallback.OnRepublishBefore(eventLog, cancellationToken);
        await Client.PublishEventAsync(eventLog.PubsubName, eventLog.Topic, cloudEvent, argument.Metadata ?? new Dictionary<string, string>(), cancellationToken);
        await PubsubCallback.OnRepublishAfter(eventLog, cancellationToken);
    }

    #endregion
}