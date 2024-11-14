using Util.Helpers;

namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Clase que gestiona los eventos de integración en el sistema.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IIntegrationEventManager"/> y se encarga de
/// la 
public class IntegrationEventManager : IIntegrationEventManager
{

    #region Campo

    protected IIntegrationEventLogStore Store;
    protected Util.Sessions.ISession Session;
    protected IGetAppId GetAppIdService;
    protected DaprOptions Options;
    protected ILogger<IntegrationEventManager> Log;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IntegrationEventManager"/>.
    /// </summary>
    /// <param name="store">La instancia de <see cref="IIntegrationEventLogStore"/> utilizada para almacenar eventos de integración.</param>
    /// <param name="session">La sesión actual, implementada por <see cref="Util.Sessions.ISession"/>.</param>
    /// <param name="getAppIdService">El servicio utilizado para obtener el ID de la aplicación, implementado por <see cref="IGetAppId"/>.</param>
    /// <param name="options">Las opciones de configuración de Dapr, encapsuladas en <see cref="IOptions{DaprOptions}"/>.</param>
    /// <param name="logger">El registrador utilizado para registrar eventos, implementado por <see cref="ILogger{IntegrationEventManager}"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="store"/> o <paramref name="getAppIdService"/> son nulos.
    /// </exception>
    /// <remarks>
    /// Si <paramref name="session"/> es nulo, se asignará una instancia de <see cref="Util.Sessions.NullSession"/>.
    /// Si <paramref name="options"/> es nulo, se utilizarán valores predeterminados de <see cref="DaprOptions"/>.
    /// Si <paramref name="logger"/> es nulo, se utilizará <see cref="NullLogger{IntegrationEventManager}"/>.
    /// </remarks>
    public IntegrationEventManager(IIntegrationEventLogStore store, Util.Sessions.ISession session, IGetAppId getAppIdService,
        IOptions<DaprOptions> options, ILogger<IntegrationEventManager> logger)
    {
        Store = store ?? throw new ArgumentNullException(nameof(store));
        Session = session ?? Util.Sessions.NullSession.Instance;
        GetAppIdService = getAppIdService ?? throw new ArgumentNullException(nameof(getAppIdService));
        Options = options?.Value ?? new DaprOptions();
        Log = logger ?? NullLogger<IntegrationEventManager>.Instance;
    }

    #endregion

    #region IncrementAsync

    /// <inheritdoc />
    /// <summary>
    /// Incrementa de manera asíncrona un contador en la tienda.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de incremento.</returns>
    /// <remarks>
    /// Este método intenta incrementar un contador en la tienda. Si se produce una excepción de concurrencia,
    /// se registrará un mensaje de depuración y se reintentará la operación.
    /// En caso de que ocurra cualquier otra excepción, se registrará un mensaje de error.
    /// </remarks>
    /// <exception cref="ConcurrencyException">
    /// Se lanza cuando hay un conflicto de concurrencia al intentar incrementar el contador.
    /// </exception>
    /// <exception cref="Exception">
    /// Se lanza para cualquier otra excepción que ocurra durante la operación.
    /// </exception>
    public virtual async Task IncrementAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await Store.IncrementAsync(cancellationToken);
        }
        catch (ConcurrencyException)
        {
            Log.LogDebug("Actualizar el conteo de eventos de integración ha encontrado una excepción de concurrencia, se intentará de nuevo.");
            await IncrementAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.LogError(exception, "Actualizar el conteo de eventos de integración falló.");
        }
    }

    #endregion

    #region GetCountAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el conteo de elementos de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{int}"/> que representa la operación asíncrona, 
    /// que contiene el número de elementos en el almacén.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="Store.GetCountAsync(CancellationToken)"/> 
    /// para obtener el conteo de elementos. Si se cancela la operación, 
    /// se lanzará una excepción de operación cancelada.
    /// </remarks>
    public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await Store.GetCountAsync(cancellationToken);
    }

    #endregion

    #region ClearCountAsync

    /// <inheritdoc />
    /// <summary>
    /// Limpia el contador de la tienda de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de limpieza del contador.
    /// </returns>
    /// <remarks>
    /// Este método llama a <see cref="Store.ClearCountAsync(CancellationToken)"/> para realizar la operación.
    /// </remarks>
    public virtual async Task ClearCountAsync(CancellationToken cancellationToken = default)
    {
        await Store.ClearCountAsync(cancellationToken);
    }

    #endregion

    #region GetAsync

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un registro de evento de integración de forma asíncrona.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se desea obtener.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro del evento de integración,
    /// o <see cref="NullIntegrationEventLog.Instance"/> si el registro de eventos está deshabilitado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado a través de la opción 
    /// <see cref="Options.Pubsub.EnableEventLog"/>. Si está deshabilitado, se devuelve una instancia nula.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    /// <seealso cref="NullIntegrationEventLog"/>
    public virtual async Task<IntegrationEventLog> GetAsync(string eventId, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        return await Store.GetAsync(eventId, cancellationToken);
    }

    #endregion

    #region SaveAsync

    /// <summary>
    /// Guarda de manera asíncrona un registro de evento de integración en el almacenamiento.
    /// </summary>
    /// <param name="eventLog">El registro de evento de integración que se va a guardar.</param>
    /// <param name="cancellationToken">Token opcional para la cancelación de la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de guardado.</returns>
    /// <remarks>
    /// Este método verifica que el objeto <paramref name="eventLog"/> no sea nulo antes de proceder a guardarlo.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="eventLog"/> es nulo.</exception>
    public virtual async Task SaveAsync(IntegrationEventLog eventLog, CancellationToken cancellationToken = default)
    {
        eventLog.CheckNull(nameof(eventLog));
        await Store.SaveAsync(eventLog, cancellationToken);
    }

    #endregion

    #region CanSubscriptionAsync

    /// <inheritdoc />
    /// <summary>
    /// Verifica si se puede realizar una suscripción para un evento específico.
    /// </summary>
    /// <param name="eventId">El identificador del evento para el cual se desea verificar la suscripción.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un valor booleano que indica si se puede realizar la suscripción. 
    /// Retorna <c>true</c> si la suscripción es permitida, de lo contrario <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método consulta el registro de eventos si la opción de registro de eventos está habilitada.
    /// Si no está habilitada, se permite la suscripción de manera predeterminada.
    /// </remarks>
    /// <seealso cref="GetAsync(string, CancellationToken)"/>
    /// <seealso cref="CanSubscription(EventLog)"/>
    public async Task<bool> CanSubscriptionAsync(string eventId, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return true;
        var eventLog = await GetAsync(eventId, cancellationToken);
        return CanSubscription(eventLog);
    }

    #endregion

    #region CanSubscription

    /// <inheritdoc />
    /// <summary>
    /// Determina si se puede realizar una suscripción basada en el registro de eventos de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se está evaluando.</param>
    /// <returns>
    /// Devuelve <c>true</c> si se puede realizar la suscripción; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado y si el registro de eventos proporcionado no es nulo.
    /// Si el registro de eventos es nulo, se devuelve <c>false</c>. Luego, se obtiene el registro de suscripción asociado
    /// al registro de eventos. Si no existe un registro de suscripción, se permite la suscripción. Si el registro de
    /// suscripción existe, se permite la suscripción solo si el estado es <see cref="SubscriptionState.Fail"/> y el 
    /// conteo de reintentos es menor que el máximo permitido.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    /// <seealso cref="SubscriptionState"/>
    public virtual bool CanSubscription(IntegrationEventLog eventLog)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return true;
        if (eventLog == null)
            return false;
        var subscriptionLog = GetSubscriptionLog(eventLog);
        if (subscriptionLog == null)
            return true;
        return subscriptionLog.State == SubscriptionState.Fail && subscriptionLog.RetryCount.SafeValue() < Options.Pubsub.MaxRetry;
    }

    /// <summary>
    /// Obtiene el registro de suscripción asociado a un evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración del cual se desea obtener el registro de suscripción.</param>
    /// <returns>
    /// Un objeto <see cref="SubscriptionLog"/> que representa el registro de suscripción asociado al evento de integración.
    /// </returns>
    /// <remarks>
    /// Este método primero obtiene el identificador de la aplicación a partir del identificador del registro del evento,
    /// y luego llama a otro método para obtener el registro de suscripción utilizando el registro del evento y el identificador de la aplicación.
    /// </remarks>
    protected SubscriptionLog GetSubscriptionLog(IntegrationEventLog eventLog)
    {
        var appId = GetAppId(eventLog.Id);
        return GetSubscriptionLog(eventLog, appId);
    }

    /// <summary>
    /// Obtiene el identificador de la aplicación asociado a un evento.
    /// </summary>
    /// <param name="eventId">El identificador del evento para el cual se solicita el identificador de la aplicación.</param>
    /// <returns>El identificador de la aplicación como una cadena.</returns>
    /// <exception cref="Warning">Se lanza una excepción si el identificador de la aplicación está vacío.</exception>
    /// <remarks>
    /// Este método utiliza un servicio para obtener el identificador de la aplicación. 
    /// Si el identificador obtenido es vacío, se lanza una advertencia con información sobre el evento.
    /// </remarks>
    protected string GetAppId(string eventId)
    {
        var appId = GetAppIdService.GetAppId();
        if (appId.IsEmpty())
            throw new Warning($"El ID de la aplicación no puede estar vacío, eventId:{eventId}");
        return appId;
    }

    /// <summary>
    /// Obtiene el registro de suscripción asociado a un identificador de aplicación específico.
    /// </summary>
    /// <param name="eventLog">El registro de eventos de integración que contiene los registros de suscripción.</param>
    /// <param name="appId">El identificador de la aplicación para la cual se desea obtener el registro de suscripción.</param>
    /// <returns>
    /// Un objeto <see cref="SubscriptionLog"/> que representa el registro de suscripción asociado al <paramref name="appId"/> especificado,
    /// o <c>null</c> si no se encuentra ningún registro de suscripción correspondiente.
    /// </returns>
    /// <remarks>
    /// Este método busca en la colección de registros de suscripción del <paramref name="eventLog"/> 
    /// y devuelve el primero que coincide con el <paramref name="appId"/> proporcionado.
    /// </remarks>
    protected SubscriptionLog GetSubscriptionLog(IntegrationEventLog eventLog, string appId)
    {
        return eventLog.SubscriptionLogs.Find(t => t.AppId == appId);
    }

    #endregion

    #region IsSubscriptionSuccess

    /// <inheritdoc />
    /// <summary>
    /// Verifica si la suscripción de un evento fue exitosa.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se está evaluando.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la suscripción fue exitosa; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método comprueba si el registro de eventos está habilitado en las opciones de 
    public virtual bool IsSubscriptionSuccess(IntegrationEventLog eventLog)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return true;
        var subscriptionLog = GetSubscriptionLog(eventLog);
        return subscriptionLog is { State: SubscriptionState.Success };
    }

    #endregion

    #region CreatePublishLogAsync

    /// <inheritdoc />
    /// <summary>
    /// Crea un registro de log para la 
    public virtual async Task<IntegrationEventLog> CreatePublishLogAsync(PubsubArgument argument, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        var result = CreateEventLog(argument);
        if (result == null)
            return NullIntegrationEventLog.Instance;
        await SaveAsync(result, cancellationToken);
        Log.LogDebug("Crear registro de publicación de eventos de integración correctamente, EventLog={@EventLog}", result);
        return result;
    }

    /// <summary>
    /// Crea un registro de evento de integración a partir de los argumentos proporcionados.
    /// </summary>
    /// <param name="argument">Los argumentos de 
    protected virtual IntegrationEventLog CreateEventLog(PubsubArgument argument)
    {
        if (argument.EventData is not CloudEvent<object> cloudEvent)
            return null;
        if (cloudEvent.Data is not IIntegrationEvent integrationEvent)
            return null;
        var appId = GetAppId(integrationEvent.EventId);
        if (appId.IsEmpty())
            throw new Warning($"El ID de la aplicación no puede estar vacío, eventId:{cloudEvent.Id}");
        var result = new IntegrationEventLog
        {
            Id = integrationEvent.EventId,
            AppId = appId,
            UserId = Session.UserId,
            PubsubName = argument.Pubsub,
            Topic = argument.Topic,
            Value = argument,
            State = EventState.Published,
            PublishTime = integrationEvent.EventTime,
            LastModificationTime = Time.Now
        };
        return result;
    }

    #endregion

    #region CreateSubscriptionLogAsync

    /// <inheritdoc />
    /// <summary>
    /// Crea un registro de suscripción para un evento de integración.
    /// </summary>
    /// <param name="eventId">El identificador del evento para el cual se crea el registro de suscripción.</param>
    /// <param name="routeUrl">La URL de la ruta a la que se enviará el evento.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro de suscripción creado,
    /// o una instancia de <see cref="NullIntegrationEventLog"/> si el registro no se pudo crear.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado en las opciones de 
    public virtual async Task<IntegrationEventLog> CreateSubscriptionLogAsync(string eventId, string routeUrl, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        if (eventId.IsEmpty())
        {
            Log.LogWarning("No se pudo crear el registro de suscripción de eventos integrado; el ID del evento no puede estar vacío, routeUrl={@routeUrl}", routeUrl);
            return NullIntegrationEventLog.Instance;
        }
        var eventLog = await GetAsync(eventId, cancellationToken);
        return await CreateSubscriptionLogAsync(eventLog, routeUrl, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea un registro de suscripción de evento de integración de manera asíncrona.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se va a suscribir.</param>
    /// <param name="routeUrl">La URL de la ruta donde se enviará el evento.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para cancelar la operación.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro de suscripción creado,
    /// o un registro nulo si la opción de registro de eventos está deshabilitada o si no se puede suscribir.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado y si se puede realizar la suscripción.
    /// Si se produce una excepción de concurrencia, se reintentará la creación del registro.
    /// En caso de cualquier otra excepción, se registrará un error en el log.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    /// <seealso cref="ConcurrencyException"/>
    public virtual async Task<IntegrationEventLog> CreateSubscriptionLogAsync(IntegrationEventLog eventLog, string routeUrl, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        eventLog.CheckNull(nameof(eventLog));
        if (CanSubscription(eventLog) == false)
            return eventLog;
        AddSubscriptionLog(eventLog, routeUrl);
        eventLog.State = EventState.Processing;
        eventLog.LastModificationTime = Time.Now;
        try
        {
            await SaveAsync(eventLog, cancellationToken);
            Log.LogDebug("Creación exitosa del registro de suscripción al evento de integración, EventLog={@EventLog}", ToDebugEventLog(eventLog));
        }
        catch (ConcurrencyException)
        {
            Log.LogDebug("Se produjo una excepción de simultaneidad al crear un registro de suscripción de eventos integrado y se volverá a intentar pronto，eventId={eventId}, routeUrl={routeUrl}", eventLog.Id, routeUrl);
            return await CreateSubscriptionLogAsync(eventLog.Id, routeUrl, cancellationToken);
        }
        catch (Exception exception)
        {
            Log.LogError(exception, "Error al crear el registro de suscripción a eventos de integración, EventLog={@EventLog}", ToDebugEventLog(eventLog));
        }
        return eventLog;
    }

    /// <summary>
    /// Convierte un registro de evento de integración en un registro de evento de depuración.
    /// </summary>
    /// <param name="eventLog">El registro de evento de integración que se va a convertir.</param>
    /// <returns>
    /// Un nuevo registro de evento de integración con el tipo de datos <see cref="PubsubArgument{CloudEvent{object}}"/>.
    /// </returns>
    /// <remarks>
    /// Este método toma un registro de evento de integración y lo transforma en un formato adecuado para la depuración,
    /// convirtiendo el valor del registro en un objeto JSON y luego deserializándolo en un tipo específico.
    /// Si el evento contiene datos, estos se convierten a JSON para su almacenamiento.
    /// </remarks>
    protected IntegrationEventLog<PubsubArgument<CloudEvent<object>>> ToDebugEventLog(IntegrationEventLog eventLog)
    {
        var json = Util.Helpers.Json.ToJson(eventLog.Value);
        var result = new IntegrationEventLog<PubsubArgument<CloudEvent<object>>>
        {
            Id = eventLog.Id,
            UserId = eventLog.UserId,
            AppId = eventLog.AppId,
            LastModificationTime = eventLog.LastModificationTime,
            DataType = eventLog.DataType,
            ETag = eventLog.ETag,
            PublishTime = eventLog.PublishTime,
            State = eventLog.State,
            PubsubName = eventLog.PubsubName,
            Topic = eventLog.Topic,
            SubscriptionLogs = eventLog.SubscriptionLogs,
            Value = Util.Helpers.Json.ToObject<PubsubArgument<CloudEvent<object>>>(json)
        };
        if (result.Value is { EventData: not null })
            result.Value.EventData.Data = Util.Helpers.Json.ToJson(result.Value.EventData.Data);
        return result;
    }

    /// <summary>
    /// Agrega un registro de suscripción al registro de eventos de integración.
    /// </summary>
    /// <param name="eventLog">El registro de eventos de integración al que se añadirá el registro de suscripción.</param>
    /// <param name="routeUrl">La URL de la ruta asociada al registro de suscripción.</param>
    /// <remarks>
    /// Este método busca un registro de suscripción existente para el evento dado. 
    /// Si no se encuentra, se crea uno nuevo y se añade a la lista de registros de suscripción. 
    /// Si ya existe, se actualiza su estado y se incrementa el contador de reintentos.
    /// </remarks>
    protected virtual void AddSubscriptionLog(IntegrationEventLog eventLog, string routeUrl)
    {
        var appId = GetAppId(eventLog.Id);
        var subscriptionLog = eventLog.SubscriptionLogs.Find(t => t.AppId == appId);
        if (subscriptionLog == null)
        {
            subscriptionLog = CreateSubscriptionLog(appId, routeUrl);
            eventLog.SubscriptionLogs.Add(subscriptionLog);
            return;
        }
        subscriptionLog.State = SubscriptionState.Processing;
        subscriptionLog.LastModificationTime = Time.Now;
        subscriptionLog.RetryCount = subscriptionLog.RetryCount.SafeValue() + 1;
        UpdateRetryTime(subscriptionLog);
    }

    /// <summary>
    /// Crea un nuevo registro de suscripción.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que realiza la suscripción.</param>
    /// <param name="routeUrl">La URL de la ruta asociada a la suscripción.</param>
    /// <returns>Un objeto <see cref="SubscriptionLog"/> que representa el registro de la suscripción creada.</returns>
    /// <remarks>
    /// Este método establece el estado de la suscripción como 'Processing' y registra la hora actual tanto para 
    /// el tiempo de suscripción como para la última modificación.
    /// </remarks>
    protected virtual SubscriptionLog CreateSubscriptionLog(string appId, string routeUrl)
    {
        return new SubscriptionLog
        {
            AppId = appId,
            RouteUrl = routeUrl,
            State = SubscriptionState.Processing,
            SubscriptionTime = Time.Now,
            LastModificationTime = Time.Now,
        };
    }

    /// <summary>
    /// Actualiza el tiempo de reintento del último registro de reintento en el log de suscripción.
    /// </summary>
    /// <param name="subscriptionLog">El objeto SubscriptionLog que contiene los registros de reintento.</param>
    /// <remarks>
    /// Este método verifica si hay registros de reintento disponibles. Si no hay registros, no realiza ninguna acción.
    /// Si hay registros, actualiza el tiempo de reintento del registro con el número más alto.
    /// </remarks>
    protected virtual void UpdateRetryTime(SubscriptionLog subscriptionLog)
    {
        if (subscriptionLog.RetryLogs.Count == 0)
            return;
        var retryLog = subscriptionLog.RetryLogs.MaxBy(t => t.Number);
        retryLog.RetryTime = Time.Now;
    }

    #endregion

    #region SubscriptionSuccessAsync

    /// <inheritdoc />
    /// <summary>
    /// Registra el éxito de la suscripción de un evento en el registro de eventos de integración.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está suscribiendo.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro del evento de integración.
    /// Si el registro de eventos está deshabilitado o si el <paramref name="eventId"/> está vacío, se devuelve <see cref="NullIntegrationEventLog.Instance"/>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado antes de proceder. 
    /// Si el <paramref name="eventId"/> es vacío, se registra una advertencia y se devuelve un registro nulo.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    public virtual async Task<IntegrationEventLog> SubscriptionSuccessAsync(string eventId, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        if (eventId.IsEmpty())
        {
            Log.LogWarning("La actualización del registro de suscripción del evento de integración SubscriptionSuccessAsync falló, el identificador del evento no puede estar vacío.");
            return NullIntegrationEventLog.Instance;
        }
        var eventLog = await GetAsync(eventId, cancellationToken);
        return await SubscriptionSuccessAsync(eventLog, cancellationToken);
    }

    /// <summary>
    /// Procesa la suscripción exitosa de un evento de integración.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que se está procesando.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Un objeto <see cref="IntegrationEventLog"/> que representa el estado actualizado del registro del evento de integración.</returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado y actualiza el estado de la suscripción
    /// a éxito. Si se produce una excepción de concurrencia, se reintenta la operación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="eventLog"/> es nulo.</exception>
    /// <seealso cref="SaveAsync(IntegrationEventLog, CancellationToken)"/>
    /// <seealso cref="UpdateEventLogState(IntegrationEventLog)"/>
    public virtual async Task<IntegrationEventLog> SubscriptionSuccessAsync(IntegrationEventLog eventLog, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        eventLog.CheckNull(nameof(eventLog));
        var appId = GetAppId(eventLog.Id);
        var subscriptionLog = GetSubscriptionLog(eventLog, appId);
        if (subscriptionLog == null)
        {
            Log.LogWarning("Actualizar el registro de suscripción de eventos de integración falló, no se encontró el registro de suscripción, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
            return NullIntegrationEventLog.Instance;
        }
        subscriptionLog.State = SubscriptionState.Success;
        subscriptionLog.LastModificationTime = Time.Now;
        eventLog.LastModificationTime = Time.Now;
        UpdateEventLogState(eventLog);
        try
        {
            await SaveAsync(eventLog, cancellationToken);
            Log.LogDebug("Integración de suscripción de eventos procesada con éxito, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
        }
        catch (ConcurrencyException)
        {
            Log.LogDebug("Actualizando el registro de suscripción de eventos de integración, se ha producido una excepción de concurrencia, se intentará de nuevo, eventId={eventId},appId={appId}", eventLog.Id, appId);
            return await SubscriptionSuccessAsync(eventLog.Id, cancellationToken);
        }
        catch (Exception exception)
        {
            Log.LogError(exception, "Actualizar el registro de suscripción de eventos integrados falló, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
        }
        return eventLog;
    }

    /// <summary>
    /// Actualiza el estado del registro de eventos de integración basado en los estados de los registros de suscripción.
    /// </summary>
    /// <param name="eventLog">El registro de eventos de integración que se va a actualizar.</param>
    /// <remarks>
    /// Este método evalúa el estado de los registros de suscripción asociados al registro de eventos.
    /// Si todos los registros de suscripción tienen un estado de éxito, el estado del registro de eventos se establece en éxito.
    /// Si hay al menos un registro de suscripción en proceso, el estado del registro de eventos se establece en procesamiento.
    /// Si no se cumplen ninguna de las condiciones anteriores, el estado se establece en fallo.
    /// </remarks>
    protected void UpdateEventLogState(IntegrationEventLog eventLog)
    {
        if (eventLog.SubscriptionLogs.All(t => t.State == SubscriptionState.Success))
        {
            eventLog.State = EventState.Success;
            return;
        }
        if (eventLog.SubscriptionLogs.Any(t => t.State == SubscriptionState.Processing))
        {
            eventLog.State = EventState.Processing;
            return;
        }
        eventLog.State = EventState.Fail;
    }

    #endregion

    #region SubscriptionFailAsync

    /// <summary>
    /// Registra un fallo en la suscripción de un evento en el registro de eventos de integración.
    /// </summary>
    /// <param name="eventId">El identificador del evento cuya suscripción ha fallado.</param>
    /// <param name="message">Un mensaje que describe el motivo del fallo de la suscripción.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el registro del evento de integración, 
    /// o una instancia de <see cref="NullIntegrationEventLog"/> si el registro de eventos está deshabilitado 
    /// o si el <paramref name="eventId"/> está vacío.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado. Si no lo está, 
    /// devuelve una instancia nula. Si el <paramref name="eventId"/> está vacío, 
    /// se registra una advertencia y también se devuelve una instancia nula.
    /// </remarks>
    /// <seealso cref="GetAsync(string, CancellationToken)"/>
    /// <seealso cref="SubscriptionFailAsync(IntegrationEventLog, string, CancellationToken)"/>
    public virtual async Task<IntegrationEventLog> SubscriptionFailAsync(string eventId, string message, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        if (eventId.IsEmpty())
        {
            Log.LogWarning("SubscriptionFailAsync actualización de registro de suscripción de eventos fallida, el identificador del evento no puede estar vacío.");
            return NullIntegrationEventLog.Instance;
        }
        var eventLog = await GetAsync(eventId, cancellationToken);
        return await SubscriptionFailAsync(eventLog, message, cancellationToken);
    }

    /// <summary>
    /// Maneja el fallo de una suscripción de evento de integración, actualizando el registro correspondiente 
    /// y registrando la información relevante sobre el error.
    /// </summary>
    /// <param name="eventLog">El registro del evento de integración que ha fallado.</param>
    /// <param name="message">El mensaje que describe el motivo del fallo de la suscripción.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="IntegrationEventLog"/> que representa el estado actualizado del registro del evento 
    /// de integración, o una instancia nula si el registro de eventos no está habilitado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el registro de eventos está habilitado. Si no lo está, devuelve una instancia nula.
    /// Si el registro de suscripción no se encuentra, se registra una advertencia y se devuelve una instancia nula.
    /// Si se encuentra el registro de suscripción, se actualiza su estado a 'Fallido' y se registra el mensaje de fallo.
    /// Se intenta guardar el registro del evento de integración y se manejan las excepciones que puedan ocurrir durante 
    /// este proceso, incluyendo excepciones de concurrencia.
    /// </remarks>
    /// <seealso cref="IntegrationEventLog"/>
    /// <seealso cref="SubscriptionState"/>
    /// <seealso cref="ConcurrencyException"/>
    public virtual async Task<IntegrationEventLog> SubscriptionFailAsync(IntegrationEventLog eventLog, string message, CancellationToken cancellationToken = default)
    {
        if (Options.Pubsub.EnableEventLog == false)
            return NullIntegrationEventLog.Instance;
        eventLog.CheckNull(nameof(eventLog));
        var appId = GetAppId(eventLog.Id);
        var subscriptionLog = GetSubscriptionLog(eventLog, appId);
        if (subscriptionLog == null)
        {
            Log.LogWarning("Actualizar el registro de suscripción de eventos de integración falló, no se encontró el registro de suscripción, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
            return NullIntegrationEventLog.Instance;
        }
        subscriptionLog.State = SubscriptionState.Fail;
        subscriptionLog.LastModificationTime = Time.Now;
        subscriptionLog.RetryLogs.Add(CreateSubscriptionRetryLog(subscriptionLog, message));
        eventLog.LastModificationTime = Time.Now;
        UpdateEventLogState(eventLog);
        try
        {
            await SaveAsync(eventLog, cancellationToken);
            Log.LogDebug("Integración de suscripción de eventos fallida, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
        }
        catch (ConcurrencyException)
        {
            Log.LogDebug("Actualizar el registro de suscripción de eventos de integración ha encontrado una excepción de concurrencia, se intentará de nuevo,eventId={eventId},appId={appId}", eventLog.Id, appId);
            return await SubscriptionFailAsync(eventLog.Id, message, cancellationToken);
        }
        catch (Exception exception)
        {
            Log.LogError(exception, "Actualizar el registro de suscripción de eventos de integración falló, appId={appId},EventLog={@EventLog}", appId, ToDebugEventLog(eventLog));
        }
        return eventLog;
    }

    /// <summary>
    /// Crea un registro de reintento de suscripción basado en el registro de suscripción proporcionado.
    /// </summary>
    /// <param name="subscriptionLog">El registro de suscripción del cual se extraen los registros de reintento.</param>
    /// <param name="message">El mensaje asociado al nuevo registro de reintento.</param>
    /// <returns>
    /// Un nuevo objeto <see cref="SubscriptionRetryLog"/> que contiene el número de reintento incrementado y el mensaje proporcionado.
    /// </returns>
    protected SubscriptionRetryLog CreateSubscriptionRetryLog(SubscriptionLog subscriptionLog, string message)
    {
        var retryLog = subscriptionLog.RetryLogs.MaxBy(t => t.Number);
        var maxNumber = retryLog?.Number ?? 0;
        return new SubscriptionRetryLog { Number = maxNumber + 1, Message = message };
    }

    #endregion
}