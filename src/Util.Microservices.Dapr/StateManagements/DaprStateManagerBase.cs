using Util.Helpers;
using Util.Microservices.Dapr.StateManagements.Queries;

namespace Util.Microservices.Dapr.StateManagements;

/// <summary>
/// Clase base para la gestión del estado en Dapr.
/// </summary>
/// <typeparam name="TStateManager">El tipo de gestor de estado que hereda de <see cref="IStateManagerBase{TStateManager}"/>.</typeparam>
public partial class DaprStateManagerBase<TStateManager> : IStateManagerBase<TStateManager> where TStateManager : IStateManagerBase<TStateManager>
{

    #region Campo

    protected readonly DaprClient Client;
    protected DaprOptions Options;
    protected readonly ILogger Logger;
    protected IKeyGenerator KeyGenerator;
    protected Dictionary<string, string> Metadatas;
    protected ConsistencyMode ConsistencyMode;
    protected List<StateTransactionRequest> StateTransactionRequests;
    protected JsonSerializerOptions SerializerOptions;
    protected bool IsTransaction;
    protected StateFilter Filter;
    protected StateSort Sort;
    protected StatePage Page;
    private string _storeName;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprStateManagerBase"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr utilizado para las operaciones de estado.</param>
    /// <param name="options">Las opciones de configuración de Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros utilizada para crear instancias de logger.</param>
    /// <param name="keyGenerator">El generador de claves utilizado para generar identificadores únicos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="client"/> o <paramref name="keyGenerator"/> son nulos.</exception>
    /// <remarks>
    /// Este constructor establece los valores iniciales para las propiedades de la clase,
    /// incluyendo el modo de consistencia, las solicitudes de transacción de estado, 
    /// y las configuraciones de filtrado, ordenamiento y paginación.
    /// </remarks>
    public DaprStateManagerBase(DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory, IKeyGenerator keyGenerator)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Options = options?.Value ?? new DaprOptions();
        Logger = loggerFactory?.CreateLogger(typeof(DaprStateManager)) ?? NullLogger<DaprStateManager>.Instance;
        KeyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
        Metadatas = new Dictionary<string, string>();
        ConsistencyMode = ConsistencyMode.Eventual;
        StateTransactionRequests = new List<StateTransactionRequest>();
        Filter = new StateFilter();
        Sort = new StateSort();
        Page = new StatePage();
    }

    #endregion

    #region Operaciones auxiliares

    /// <summary>
    /// Devuelve una instancia del gestor de estado actual.
    /// </summary>
    /// <returns>
    /// Una instancia de <typeparamref name="TStateManager"/> que representa el gestor de estado.
    /// </returns>
    /// <remarks>
    /// Este método realiza un casting del objeto actual a <typeparamref name="TStateManager"/>.
    /// Asegúrese de que el tipo de objeto actual sea compatible con <typeparamref name="TStateManager"/> 
    /// para evitar excepciones en tiempo de ejecución.
    /// </remarks>
    /// <typeparam name="TStateManager">
    /// El tipo del gestor de estado que se está devolviendo.
    /// </typeparam>
    private TStateManager Return()
    {
        return (TStateManager)(object)this;
    }

    /// <summary>
    /// Obtiene el nombre de la tienda.
    /// </summary>
    /// <returns>
    /// Devuelve el nombre de la tienda si está definido; de lo contrario, devuelve "statestore".
    /// </returns>
    protected string GetStoreName()
    {
        return _storeName.IsEmpty() ? "statestore" : _storeName;
    }

    /// <summary>
    /// Obtiene una clave basada en el valor proporcionado o devuelve la clave existente si no está vacía.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="key">La clave que se desea obtener.</param>
    /// <param name="value">El valor del cual se generará una clave si la clave proporcionada está vacía.</param>
    /// <returns>
    /// Devuelve la clave proporcionada si no está vacía; de lo contrario, genera una nueva clave utilizando el identificador del valor.
    /// </returns>
    /// <remarks>
    /// Este método es útil para asegurar que siempre se tenga una clave válida, ya sea utilizando una clave existente o generando una nueva.
    /// </remarks>
    protected virtual string GetKey<TValue>(string key, TValue value) where TValue : IDataKey
    {
        if (key.IsEmpty() == false)
            return key;
        return KeyGenerator.CreateKey<TValue>(value.Id);
    }

    /// <summary>
    /// Determina si el tipo especificado por el parámetro genérico <typeparamref name="TValue"/> 
    /// es asignable a la interfaz <see cref="IDataType"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo que se va a evaluar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si <typeparamref name="TValue"/> es asignable a <see cref="IDataType"/>; 
    /// de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected bool IsDataType<TValue>()
    {
        return typeof(TValue).IsAssignableTo(typeof(IDataType));
    }

    /// <summary>
    /// Determina si el tipo especificado implementa la interfaz <see cref="IDataKey"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo <typeparamref name="TValue"/> implementa <see cref="IDataKey"/>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected bool IsDataKey<TValue>()
    {
        return typeof(TValue).IsAssignableTo(typeof(IDataKey));
    }

    /// <summary>
    /// Determina si el tipo especificado implementa la interfaz <see cref="IETag"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo que se va a verificar.</typeparam>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo <typeparamref name="TValue"/> implementa la interfaz <see cref="IETag"/>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected bool IsETag<TValue>()
    {
        return typeof(TValue).IsAssignableTo(typeof(IETag));
    }

    /// <summary>
    /// Establece una condición de tipo de dato para el sistema.
    /// </summary>
    /// <typeparam name="TValue">El tipo de dato que se está evaluando.</typeparam>
    /// <remarks>
    /// Este método utiliza el nombre completo del tipo de dato especificado por el parámetro genérico 
    /// <typeparamref name="TValue"/> y lo compara con una condición específica.
    /// </remarks>
    /// <seealso cref="IsDataType{T}"/>
    protected void SetDataTypeCondition<TValue>()
    {
        EqualIf("dataType", typeof(TValue).FullName, IsDataType<TValue>());
    }

    #endregion

    #region StoreName

    /// <inheritdoc />
    /// <summary>
    /// Almacena el nombre de la tienda.
    /// </summary>
    /// <param name="storeName">El nombre de la tienda que se va a almacenar.</param>
    /// <returns>Una instancia del administrador de estado.</returns>
    /// <remarks>
    /// Este método asigna el valor del parámetro <paramref name="storeName"/> a la variable interna 
    /// <c>_storeName</c> y luego devuelve el resultado de la llamada al método <c>Return()</c>.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TStateManager StoreName(string storeName)
    {
        _storeName = storeName;
        return Return();
    }

    #endregion

    #region Clear

    /// <inheritdoc />
    /// <summary>
    /// Limpia el estado del administrador de estados.
    /// </summary>
    /// <returns>
    /// Un objeto del tipo <typeparamref name="TStateManager"/> que representa el estado limpio.
    /// </returns>
    /// <remarks>
    /// Este método restablece todas las metadatas, establece el modo de consistencia a eventual,
    /// limpia las solicitudes de transacción y marca el estado de transacción como falso.
    /// También se invoca el método <see cref="ClearQuery"/> para limpiar cualquier consulta activa.
    /// </remarks>
    public TStateManager Clear()
    {
        Metadatas.Clear();
        ConsistencyMode = ConsistencyMode.Eventual;
        StateTransactionRequests.Clear();
        IsTransaction = false;
        ClearQuery();
        return Return();
    }

    /// <summary>
    /// Limpia los filtros, ordenamientos y restablece la página actual a su estado inicial.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para reiniciar el estado de la consulta, eliminando cualquier filtro o 
    /// criterio de ordenamiento previamente aplicado. También restablece la paginación a su estado 
    /// inicial.
    /// </remarks>
    protected void ClearQuery()
    {
        Filter.Clear();
        Sort.Clear();
        Page = new StatePage();
    }

    #endregion

    #region BeginTransaction

    /// <inheritdoc />
    /// <summary>
    /// Inicia una nueva transacción.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TStateManager"/> que representa el administrador del estado de la transacción actual.
    /// </returns>
    /// <remarks>
    /// Este método establece el estado de la transacción a verdadero, indicando que una transacción está en curso.
    /// </remarks>
    /// <typeparam name="TStateManager">
    /// Tipo del administrador del estado que se devuelve.
    /// </typeparam>
    /// <seealso cref="EndTransaction"/>
    public TStateManager BeginTransaction()
    {
        IsTransaction = true;
        return Return();
    }

    #endregion

    #region JsonSerializerOptions

    /// <inheritdoc />
    /// <summary>
    /// Establece las opciones de serialización JSON para el administrador de estado.
    /// </summary>
    /// <param name="options">Las opciones de serialización JSON a aplicar.</param>
    /// <returns>El administrador de estado actual.</returns>
    /// <remarks>
    /// Este método permite configurar las opciones de serialización que se utilizarán al serializar y deserializar objetos.
    /// </remarks>
    public TStateManager JsonSerializerOptions(JsonSerializerOptions options)
    {
        SerializerOptions = options;
        return Return();
    }

    #endregion

    #region Metadata

    /// <summary>
    /// Establece un par clave-valor en el gestor de metadatos.
    /// </summary>
    /// <param name="key">La clave del metadato que se desea establecer.</param>
    /// <param name="value">El valor del metadato que se desea asociar a la clave.</param>
    /// <returns>Una instancia del gestor de estados.</returns>
    /// <remarks>
    /// Si la clave o el valor están vacíos, el método no realizará ninguna acción y devolverá el gestor de estados actual.
    /// Si la clave ya existe, se eliminará el metadato anterior antes de agregar el nuevo.
    /// </remarks>
    public TStateManager Metadata(string key, string value)
    {
        if (key.IsEmpty())
            return Return();
        if (value.IsEmpty())
            return Return();
        RemoveMetadata(key);
        Metadatas.Add(key, value);
        return Return();
    }

    /// <summary>
    /// Establece los metadatos utilizando un diccionario de pares clave-valor.
    /// </summary>
    /// <param name="metadata">Un diccionario que contiene los metadatos a establecer, donde la clave es una cadena y el valor es también una cadena.</param>
    /// <returns>Devuelve una instancia de <typeparamref name="TStateManager"/>.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="metadata"/> es nulo, se devuelve inmediatamente el resultado de <see cref="Return()"/>.
    /// De lo contrario, se itera sobre cada elemento del diccionario y se llama al método <see cref="Metadata(string, string)"/> 
    /// para establecer cada metadato.
    /// </remarks>
    /// <typeparam name="TStateManager">El tipo de gestor de estado que se está utilizando.</typeparam>
    public TStateManager Metadata(IDictionary<string, string> metadata)
    {
        if (metadata == null)
            return Return();
        foreach (var item in metadata)
            Metadata(item.Key, item.Value);
        return Return();
    }

    #endregion

    #region RemoveMetadata

    /// <summary>
    /// Elimina la metadata asociada a la clave especificada.
    /// </summary>
    /// <param name="key">La clave de la metadata que se desea eliminar.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    /// <remarks>
    /// Si la clave proporcionada está vacía, no se realizará ninguna acción.
    /// Si la clave existe en el diccionario de metadatas, se eliminará la entrada correspondiente.
    /// </remarks>
    public TStateManager RemoveMetadata(string key)
    {
        if (key.IsEmpty())
            return Return();
        if (Metadatas.ContainsKey(key))
            Metadatas.Remove(key);
        return Return();
    }

    #endregion

    #region ContentType

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de contenido para el gestor de estado.
    /// </summary>
    /// <param name="type">El tipo de contenido que se desea establecer.</param>
    /// <returns>Una instancia del gestor de estado actual.</returns>
    /// <remarks>
    /// Este método permite definir el tipo de contenido que se utilizará en el contexto del gestor de estado.
    /// Se invoca el método <see cref="Metadata"/> para registrar el tipo de contenido especificado.
    /// </remarks>
    /// <seealso cref="Metadata(string, string)"/>
    /// <seealso cref="Return()"/>
    public TStateManager ContentType(string type)
    {
        Metadata("contentType", type);
        return Return();
    }

    #endregion

    #region JsonType

    /// <summary>
    /// Configura el tipo de contenido como JSON.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TStateManager"/> que representa el estado actual del gestor.
    /// </returns>
    /// <typeparam name="TStateManager">
    /// El tipo del gestor de estado que se está utilizando.
    /// </typeparam>
    public TStateManager JsonType()
    {
        return ContentType("application/json");
    }

    #endregion

    #region AddAsync

    /// <inheritdoc />
    /// <summary>
    /// Agrega un valor asociado a una clave de forma asíncrona.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a agregar.</typeparam>
    /// <param name="key">La clave asociada al valor que se va a agregar.</param>
    /// <param name="value">El valor que se va a agregar.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando la clave está vacía.</exception>
    /// <remarks>
    /// Este método verifica si se está en una transacción. Si no es así, guarda el estado de forma directa.
    /// En caso contrario, agrega una solicitud de transacción al conjunto de solicitudes.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de agregar el valor.
    /// </returns>
    /// <seealso cref="StateTransactionRequest"/>
    public virtual async Task AddAsync<TValue>(string key, TValue value, CancellationToken cancellationToken = default)
    {
        if (key.IsEmpty())
            throw new ArgumentNullException(nameof(key));
        InitAdd(value);
        if (IsTransaction == false)
        {
            await Client.SaveStateAsync(GetStoreName(), key, value, CreateStateOptions(), Metadatas, cancellationToken);
            Clear();
            return;
        }
        var request = new StateTransactionRequest(key, ToBytes(value), StateOperationType.Upsert, null, Metadatas, CreateStateOptions());
        StateTransactionRequests.Add(request);
    }

    /// <summary>
    /// Inicializa los componentes necesarios para agregar un nuevo valor.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a inicializar.</typeparam>
    /// <param name="value">El valor que se va a utilizar para la inicialización.</param>
    /// <remarks>
    /// Este método llama a otros métodos para inicializar el ID, el tipo de dato y el tipo JSON
    /// basado en el valor proporcionado.
    /// </remarks>
    /// <seealso cref="InitId(TValue)"/>
    /// <seealso cref="InitDataType(TValue)"/>
    /// <seealso cref="InitJsonType{TValue}()"/>
    protected virtual void InitAdd<TValue>(TValue value)
    {
        InitId(value);
        InitDataType(value);
        InitJsonType<TValue>();
    }

    /// <summary>
    /// Inicializa el identificador de un objeto que implementa la interfaz <see cref="IDataKey"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a inicializar.</typeparam>
    /// <param name="value">El valor que se va a verificar e inicializar si es necesario.</param>
    /// <remarks>
    /// Este método verifica si el valor proporcionado implementa la interfaz <see cref="IDataKey"/>.
    /// Si es así, y si el identificador del objeto está vacío, se le asigna un nuevo identificador único.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    protected virtual void InitId<TValue>(TValue value)
    {
        if (value is not IDataKey data)
            return;
        if (data.Id.IsEmpty())
            data.Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Inicializa el tipo de dato especificado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de dato que se va a inicializar.</typeparam>
    /// <param name="value">El valor que se utilizará para inicializar el tipo de dato.</param>
    /// <remarks>
    /// Este método verifica si el valor proporcionado implementa la interfaz <see cref="IDataType"/>.
    /// Si es así, establece la propiedad <c>DataType</c> con el nombre completo del tipo de <typeparamref name="TValue"/>.
    /// </remarks>
    protected virtual void InitDataType<TValue>(TValue value)
    {
        if (value is IDataType dataType)
            dataType.DataType = typeof(TValue).FullName;
    }

    /// <summary>
    /// Inicializa el tipo JSON para el tipo especificado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de valor que se va a inicializar.</typeparam>
    /// <remarks>
    /// Este método verifica si el tipo especificado es una cadena. Si es así, no realiza ninguna acción.
    /// De lo contrario, se llama al método <see cref="JsonType"/> para realizar la inicialización necesaria.
    /// </remarks>
    /// <seealso cref="JsonType"/>
    protected virtual void InitJsonType<TValue>()
    {
        var type = typeof(TValue);
        if (type == typeof(string))
            return;
        JsonType();
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="StateOptions"/> con las configuraciones de consistencia y concurrencia especificadas.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="StateOptions"/> que contiene las configuraciones de consistencia y concurrencia.
    /// </returns>
    /// <remarks>
    /// Este método utiliza las propiedades <see cref="ConsistencyMode"/> y <see cref="ConcurrencyMode"/> 
    /// para establecer los valores de consistencia y concurrencia en el objeto <see cref="StateOptions"/>.
    /// </remarks>
    protected StateOptions CreateStateOptions()
    {
        return new StateOptions
        {
            Consistency = ConsistencyMode,
            Concurrency = ConcurrencyMode.FirstWrite
        };
    }

    /// <summary>
    /// Convierte un valor del tipo especificado a un arreglo de bytes utilizando la serialización JSON.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a convertir a bytes.</typeparam>
    /// <param name="value">El valor que se desea convertir a un arreglo de bytes.</param>
    /// <returns>Un arreglo de bytes que representa el valor serializado en formato JSON.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    /// <seealso cref="Json.ToBytes{TValue}(TValue, JsonSerializerOptions)"/>
    protected virtual byte[] ToBytes<TValue>(TValue value)
    {
        return Json.ToBytes(value, SerializerOptions);
    }

    #endregion

    #region UpdateAsync

    /// <inheritdoc />
    /// <summary>
    /// Actualiza de manera asíncrona el estado asociado a una clave específica.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor que se va a actualizar.</typeparam>
    /// <param name="key">La clave del estado que se desea actualizar.</param>
    /// <param name="value">El nuevo valor que se asignará a la clave.</param>
    /// <param name="etag">El etag que se utiliza para la verificación de concurrencia.</param>
    /// <param name="cancellationToken">El token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor devuelto es <c>true</c> si la actualización fue exitosa; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Si la operación no está dentro de una transacción, se intentará guardar el estado utilizando el cliente.
    /// Si ocurre una excepción de Dapr, se maneja adecuadamente y se verifica si el error es un conflicto de escritura.
    /// En caso de estar en una transacción, se agrega la solicitud de transacción a la lista de solicitudes.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la clave está vacía.</exception>
    /// <exception cref="DaprException">Se lanza si ocurre un error al intentar guardar el estado.</exception>
    public async Task<bool> UpdateAsync<TValue>(string key, TValue value, string etag, CancellationToken cancellationToken = default)
    {
        if (key.IsEmpty())
            throw new ArgumentNullException(nameof(key));
        InitJsonType<TValue>();
        if (IsTransaction == false)
        {
            try
            {
                var result = await Client.TrySaveStateAsync(GetStoreName(), key, value, etag, CreateStateOptions(), Metadatas, cancellationToken);
                Clear();
                return result;
            }
            catch (DaprException ex)
            {
                Clear();
                var exception = ex.InnerException;
                if (exception != null && exception.Message.Contains("E11000"))
                    return false;
                throw;
            }
        }
        var request = new StateTransactionRequest(key, ToBytes(value), StateOperationType.Upsert, etag, Metadatas, CreateStateOptions());
        StateTransactionRequests.Add(request);
        return true;
    }

    #endregion

    #region RemoveAsync

    /// <inheritdoc />
    /// <summary>
    /// Elimina un estado de forma asíncrona utilizando la clave especificada.
    /// </summary>
    /// <param name="key">
    /// La clave del estado que se desea eliminar. No puede ser nula o vacía.
    /// </param>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. 
    /// El valor predeterminado es <c>default</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="key"/> es nula o vacía.
    /// </exception>
    /// <remarks>
    /// Si no se está en una transacción, se eliminará el estado directamente. 
    /// Si se está en una transacción, se añadirá una solicitud de eliminación a la lista de solicitudes de transacción.
    /// </remarks>
    /// <seealso cref="StateTransactionRequest"/>
    /// <seealso cref="Client.DeleteStateAsync"/>
    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        if (key.IsEmpty())
            throw new ArgumentNullException(nameof(key));
        if (IsTransaction == false)
        {
            await Client.DeleteStateAsync(GetStoreName(), key, CreateStateOptions(), Metadatas, cancellationToken);
            Clear();
            return;
        }
        var request = new StateTransactionRequest(key, null, StateOperationType.Delete, null, Metadatas, CreateStateOptions());
        StateTransactionRequests.Add(request);
    }

    #endregion

    #region RemoveByIdAsync

    /// <summary>
    /// Elimina un elemento de forma asíncrona utilizando su identificador.
    /// </summary>
    /// <typeparam name="TValue">El tipo del elemento que implementa la interfaz <see cref="IDataKey"/>.</typeparam>
    /// <param name="id">El identificador del elemento a eliminar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación.</returns>
    /// <remarks>
    /// Este método primero obtiene la clave del elemento a eliminar utilizando el identificador proporcionado,
    /// y luego procede a eliminar el elemento asociado a esa clave.
    /// </remarks>
    /// <seealso cref="GetKeyByIdAsync{TValue}(string, CancellationToken)"/>
    /// <seealso cref="RemoveAsync(object, CancellationToken)"/>
    public async Task RemoveByIdAsync<TValue>(string id, CancellationToken cancellationToken = default) where TValue : IDataKey
    {
        var key = await GetKeyByIdAsync<TValue>(id, cancellationToken);
        await RemoveAsync(key, cancellationToken);
    }

    #endregion

    #region SaveAsync

    /// <inheritdoc />
    /// <summary>
    /// Guarda de forma asíncrona un objeto de tipo <typeparamref name="TValue"/> en el almacenamiento.
    /// </summary>
    /// <typeparam name="TValue">El tipo del objeto que se va a guardar. Debe implementar <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="value">El objeto que se desea guardar. No puede ser nulo.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona. Por defecto es <see cref="CancellationToken.None"/>.</param>
    /// <param name="key">La clave bajo la cual se almacenará el objeto. Si es nula, se generará una clave automáticamente.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor que contiene la clave bajo la cual se guardó el objeto.</returns>
    /// <remarks>
    /// Este método verifica si se está realizando una transacción. Si no es así, intenta actualizar el objeto existente.
    /// Si la actualización falla debido a un conflicto de concurrencia, se lanzará una excepción <see cref="ConcurrencyException"/>.
    /// En caso de que se esté realizando una transacción, se prepara una solicitud de transacción para guardar el objeto.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="value"/> es nulo.</exception>
    /// <exception cref="ConcurrencyException">Se lanza si la actualización del objeto falla debido a un conflicto de concurrencia.</exception>
    /// <seealso cref="IDataKey"/>
    /// <seealso cref="IETag"/>
    public async Task<string> SaveAsync<TValue>(TValue value, CancellationToken cancellationToken = default, string key = null) where TValue : IDataKey, IETag
    {
        value.CheckNull(nameof(value));
        InitSave(value);
        key = GetKey(key, value);
        if (IsTransaction == false)
        {
            var result = await UpdateAsync(key, value, value.ETag ?? string.Empty, cancellationToken);
            if (result == false)
                throw new ConcurrencyException($"Error al guardar el estado, key:{key}");
            Clear();
            return key;
        }
        var bytes = ToBytes(value);
        var request = new StateTransactionRequest(key, bytes, StateOperationType.Upsert, value.ETag ?? string.Empty, Metadatas, CreateStateOptions());
        StateTransactionRequests.Add(request);
        return key;
    }

    /// <summary>
    /// Inicializa el proceso de guardado para un objeto de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo de objeto que implementa las interfaces <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="value">El objeto que se va a inicializar para el guardado.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una funcionalidad adicional
    /// durante el proceso de inicialización del guardado.
    /// </remarks>
    protected virtual void InitSave<TValue>(TValue value) where TValue : IDataKey, IETag
    {
        InitId(value);
        InitDataType(value);
    }

    /// <inheritdoc />
    /// <summary>
    /// Guarda de manera asíncrona una colección de valores en el estado.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que se van a guardar. Debe implementar <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="values">Una colección de valores que se van a guardar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <see cref="CancellationToken.None"/>.</param>
    /// <remarks>
    /// Este método verifica si la operación se realiza dentro de una transacción. Si no es así, se guarda la colección completa de valores de forma masiva.
    /// En caso contrario, se prepara cada elemento para ser guardado individualmente en una transacción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="values"/> es nulo.</exception>
    /// <seealso cref="IDataKey"/>
    /// <seealso cref="IETag"/>
    public async Task SaveAsync<TValue>(IEnumerable<TValue> values, CancellationToken cancellationToken = default) where TValue : IDataKey, IETag
    {
        values.CheckNull(nameof(values));
        InitJsonType<TValue>();
        var items = ToSaveStateItems(values);
        if (IsTransaction == false)
        {
            await Client.SaveBulkStateAsync(GetStoreName(), items, cancellationToken);
            Clear();
            return;
        }
        foreach (var item in items)
        {
            var bytes = ToBytes(item.Value);
            var request = new StateTransactionRequest(item.Key, bytes, StateOperationType.Upsert, item.ETag ?? string.Empty, Metadatas, CreateStateOptions());
            StateTransactionRequests.Add(request);
        }
    }

    /// <summary>
    /// Convierte una colección de valores en una lista de elementos de estado guardados.
    /// </summary>
    /// <typeparam name="TValue">El tipo de los valores que implementan las interfaces <see cref="IDataKey"/> y <see cref="IETag"/>.</typeparam>
    /// <param name="values">Una colección de valores que se convertirán en elementos de estado guardados.</param>
    /// <returns>Una lista de elementos de estado guardados que representan los valores proporcionados.</returns>
    /// <remarks>
    /// Este método inicializa cada valor utilizando el método <see cref="InitSave"/> y obtiene una clave a través del método <see cref="GetKey"/>.
    /// Luego, crea un nuevo <see cref="SaveStateItem{TValue}"/> para cada valor y lo agrega a la lista de resultados.
    /// </remarks>
    /// <seealso cref="IDataKey"/>
    /// <seealso cref="IETag"/>
    /// <seealso cref="SaveStateItem{TValue}"/>
    protected List<SaveStateItem<TValue>> ToSaveStateItems<TValue>(IEnumerable<TValue> values) where TValue : IDataKey, IETag
    {
        var result = new List<SaveStateItem<TValue>>();
        foreach (var value in values)
        {
            InitSave(value);
            var key = GetKey(null, value);
            var item = new SaveStateItem<TValue>(key, value, value.ETag, CreateStateOptions(), Metadatas);
            result.Add(item);
        }
        return result;
    }

    #endregion

    #region CommitAsync

    /// <inheritdoc />
    /// <summary>
    /// Realiza un commit asíncrono de la transacción de estado.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método ejecuta una transacción de estado utilizando el cliente y luego limpia el estado local.
    /// </remarks>
    /// <seealso cref="Client"/>
    /// <seealso cref="GetStoreName"/>
    /// <seealso cref="Clear"/>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Client.ExecuteStateTransactionAsync(GetStoreName(), StateTransactionRequests, Metadatas, cancellationToken);
        Clear();
    }

    #endregion
}