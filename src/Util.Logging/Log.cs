namespace Util.Logging;

/// <summary>
/// Representa un sistema de registro que implementa la interfaz <see cref="ILog"/>.
/// </summary>
public class Log : ILog
{

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Log"/>.
    /// </summary>
    /// <param name="logger">Una instancia de <see cref="ILoggerWrapper"/> que se utilizará para registrar mensajes de log.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="logger"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor establece el logger y inicializa las propiedades de log, el mensaje de log y los argumentos del mensaje.
    /// </remarks>
    public Log(ILoggerWrapper logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        LogProperties = new Dictionary<string, object>();
        LogMessage = new StringBuilder();
        LogMessageArgs = new List<object>();
    }

    #endregion

    #region Null(Ejemplo de operación de registro vacío.)

    public static ILog Null = NullLog.Instance;

    #endregion

    #region atributo

    /// <summary>
    /// Obtiene el envoltorio del registrador de logs.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una instancia de un registrador de logs,
    /// que puede ser utilizado para registrar información, advertencias y errores
    /// durante la ejecución de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ILoggerWrapper"/>.
    /// </value>
    protected ILoggerWrapper Logger { get; }
    /// <summary>
    /// Obtiene o establece el nivel de registro para la clase.
    /// </summary>
    /// <remarks>
    /// El nivel de registro determina la severidad de los mensajes que se registrarán.
    /// Los niveles de registro pueden incluir información, advertencias, errores, etc.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="LogLevel"/> que representa el nivel de registro actual.
    /// </value>
    protected LogLevel LogLevel { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador del evento de registro.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para categorizar y filtrar eventos de registro.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="EventId"/> que representa el identificador del evento.
    /// </value>
    protected EventId LogEventId { get; set; }
    /// <summary>
    /// Obtiene o establece la excepción que se ha registrado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar información sobre excepciones que ocurren durante la ejecución 
    /// de la aplicación, permitiendo un mejor manejo y seguimiento de errores.
    /// </remarks>
    /// <exception cref="Exception">
    /// La excepción que se ha producido y se está registrando.
    /// </exception>
    protected Exception LogException { get; set; }
    /// <summary>
    /// Obtiene o establece un diccionario que contiene propiedades de registro.
    /// </summary>
    /// <remarks>
    /// Este diccionario se utiliza para almacenar pares clave-valor que representan
    /// información adicional que se puede incluir en los registros.
    /// </remarks>
    /// <value>
    /// Un diccionario de tipo <see cref="IDictionary{TKey, TValue}"/> donde la clave es un <see cref="string"/>
    /// y el valor es un <see cref="object"/>.
    /// </value>
    protected IDictionary<string, object> LogProperties { get; set; }
    /// <summary>
    /// Obtiene o establece el estado del registro.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar información relacionada con el estado del registro,
    /// la cual puede ser utilizada para fines de depuración o seguimiento del flujo de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que representa el estado del registro.
    /// </value>
    protected object LogState { get; set; }
    /// <summary>
    /// Obtiene el mensaje de registro como un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un <see cref="StringBuilder"/> que se utiliza para construir mensajes de registro.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="StringBuilder"/> que contiene el mensaje de registro.
    /// </value>
    protected StringBuilder LogMessage { get; }
    /// <summary>
    /// Obtiene la lista de argumentos de mensaje de registro.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a los argumentos que se utilizan al registrar mensajes.
    /// Los argumentos pueden ser utilizados para formatear el mensaje de registro o para proporcionar información adicional.
    /// </remarks>
    /// <value>
    /// Una lista de objetos que representan los argumentos de mensaje de registro.
    /// </value>
    protected List<object> LogMessageArgs { get; }

    #endregion

    #region EventId(Configurar el identificador de eventos de registro.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el identificador del evento de registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="ILog"/>.</returns>
    /// <remarks>
    /// Este método permite asignar un identificador específico a un evento de registro,
    /// lo que facilita la categorización y búsqueda de eventos en los registros.
    /// </remarks>
    public virtual ILog EventId(EventId eventId)
    {
        LogEventId = eventId;
        return this;
    }

    #endregion

    #region Exception(Configuración anormal.)

    /// <inheritdoc />
    /// <summary>
    /// Registra una excepción en el sistema de logs.
    /// </summary>
    /// <param name="exception">La excepción que se desea registrar.</param>
    /// <returns>Una instancia del logger actual para permitir el encadenamiento de llamadas.</returns>
    /// <remarks>
    /// Este método sobrescribe el comportamiento de un método base para registrar excepciones.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public virtual ILog Exception(Exception exception)
    {
        LogException = exception;
        return this;
    }

    #endregion

    #region Property(Configurar propiedades de extensión personalizadas.)

    /// <inheritdoc />
    /// <summary>
    /// Establece o actualiza el valor de una propiedad de registro.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que se desea establecer o actualizar.</param>
    /// <param name="propertyValue">El valor que se asignará a la propiedad especificada.</param>
    /// <returns>Devuelve la instancia actual de <see cref="ILog"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Si <paramref name="propertyName"/> está vacío, no se realizará ninguna acción y se devolverá la instancia actual.
    /// Si la propiedad ya existe, su valor se concatenará con el nuevo <paramref name="propertyValue"/>.
    /// Si la propiedad no existe, se agregará a la colección de propiedades de registro.
    /// </remarks>
    public virtual ILog Property(string propertyName, string propertyValue)
    {
        if (propertyName.IsEmpty())
            return this;
        if (LogProperties.ContainsKey(propertyName))
        {
            LogProperties[propertyName] += propertyValue;
            return this;
        }
        LogProperties.Add(propertyName, propertyValue);
        return this;
    }

    #endregion

    #region State(Configurar el objeto de estado del registro.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el estado del registro.
    /// </summary>
    /// <param name="state">El objeto que representa el estado a establecer.</param>
    /// <returns>Una instancia del registro actual para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite asignar un estado específico al registro, que puede ser utilizado posteriormente
    /// para realizar un seguimiento o para propósitos de depuración.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public virtual ILog State(object state)
    {
        LogState = state;
        return this;
    }

    #endregion

    #region Message(Configurar mensajes de registro.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje con los argumentos proporcionados.
    /// </summary>
    /// <param name="message">El mensaje que se va a registrar.</param>
    /// <param name="args">Los argumentos que se van a incluir en el mensaje.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que permite la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite agregar un mensaje a un registro de log, utilizando un formato que puede incluir 
    /// argumentos adicionales. Los argumentos se almacenan para su uso posterior.
    /// </remarks>
    public virtual ILog Message(string message, params object[] args)
    {
        LogMessage.Append(message);
        LogMessageArgs.AddRange(args);
        return this;
    }

    #endregion

    #region IsEnabled(¿Está habilitado?)

    /// <inheritdoc />
    /// <summary>
    /// Determina si el registro está habilitado para el nivel de log especificado.
    /// </summary>
    /// <param name="logLevel">El nivel de log que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el registro está habilitado para el nivel de log especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <seealso cref="Logger.IsEnabled(LogLevel)"/>
    public virtual bool IsEnabled(LogLevel logLevel)
    {
        return Logger.IsEnabled(logLevel);
    }

    #endregion

    #region BeginScope(Abrir el rango de registros.)

    /// <inheritdoc />
    /// <summary>
    /// Inicia un nuevo ámbito de registro con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se va a asociar con el ámbito de registro.</typeparam>
    /// <param name="state">El estado que se utilizará para el nuevo ámbito.</param>
    /// <returns>Un objeto <see cref="IDisposable"/> que representa el ámbito de registro. Al llamar a <c>Dispose</c> en este objeto, se finaliza el ámbito.</returns>
    /// <remarks>
    /// Este método permite agrupar mensajes de registro relacionados bajo un mismo contexto, facilitando la identificación de eventos en el registro.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    public virtual IDisposable BeginScope<TState>(TState state)
    {
        return Logger.BeginScope(state);
    }

    #endregion

    #region LogTrace(Escribir un registro de seguimiento.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de traza en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que permite encadenar llamadas.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el logger y verifica si hay un mensaje para registrar. 
    /// Si hay un mensaje, se registra con el nivel de traza. Si no, se establece el nivel de log en traza 
    /// y se llama al método <see cref="WriteLog"/> para procesar el registro.
    /// Al finalizar, se limpia el estado del logger.
    /// </remarks>
    /// <exception cref="Exception">
    /// Se puede lanzar una excepción si ocurre un error durante el registro del log.
    /// </exception>
    public virtual ILog LogTrace()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogTrace(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Trace;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region LogDebug(Escribir un registro de depuración.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de depuración en el sistema de registro.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el sistema de registro, verifica si hay un mensaje para registrar,
    /// y si es así, lo envía al registrador. Si no hay mensaje, establece el nivel de registro
    /// en depuración y llama al método <see cref="WriteLog"/> para registrar la entrada.
    /// Al finalizar, se limpia el estado del registro.
    /// </remarks>
    public virtual ILog LogDebug()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogDebug(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Debug;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region LogInformation(Escribir un registro de información.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de información en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que representa la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el sistema de logging, verifica si hay un mensaje para registrar,
    /// y si es así, lo envía al logger. Si no hay mensaje, establece el nivel de log en 
    /// <see cref="LogLevel.Information"/> y llama al método <see cref="WriteLog"/> para 
    /// realizar el registro.
    /// </remarks>
    /// <exception cref="Exception">
    /// Puede lanzar una excepción si ocurre un error durante el proceso de logging.
    /// </exception>
    public virtual ILog LogInformation()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogInformation(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Information;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region LogWarning(Escribir un registro de advertencia.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de advertencia en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que permite encadenar llamadas.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el sistema de logging, verifica si hay un mensaje para registrar,
    /// y si es así, lo registra como una advertencia. Si no hay mensaje, establece el nivel de log
    /// en advertencia y llama al método <see cref="WriteLog"/> para manejar el registro.
    /// Al final, se asegura de limpiar el estado del logger llamando al método <see cref="Clear"/>.
    /// </remarks>
    public virtual ILog LogWarning()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogWarning(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Warning;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region LogError(Escribir un registro de errores.)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de error en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que representa el registro de error.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el sistema de logging, verifica si hay un mensaje que registrar,
    /// y si es así, lo envía al logger. Si no hay mensaje, establece el nivel de log en Error
    /// y llama al método <see cref="WriteLog"/> para registrar el error.
    /// Al finalizar, se limpia el estado del logger.
    /// </remarks>
    public virtual ILog LogError()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogError(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Error;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region LogCritical(Escribe un registro fatal)

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de nivel crítico en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que permite encadenar llamadas.
    /// </returns>
    /// <remarks>
    /// Este método inicializa el sistema de logging, verifica si hay un mensaje para registrar,
    /// y si es así, lo envía al logger. Si no hay mensaje, establece el nivel de log en crítico
    /// y llama al método <see cref="WriteLog"/> para registrar el evento.
    /// Al finalizar, se asegura de limpiar cualquier estado interno mediante el método <see cref="Clear"/>.
    /// </remarks>
    public virtual ILog LogCritical()
    {
        try
        {
            Init();
            if (LogMessage.Length > 0)
            {
                Logger.LogCritical(LogEventId, LogException, GetMessage(), GetMessageArgs());
                return this;
            }
            LogLevel = LogLevel.Critical;
            return WriteLog();
        }
        finally
        {
            Clear();
        }
    }

    #endregion

    #region Métodos auxiliares

    /// <summary>
    /// Inicializa el estado del objeto, configurando su contenido basado en el estado actual.
    /// </summary>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una
    /// inicialización personalizada. La implementación predeterminada llama al método
    /// <see cref="ConvertStateToContent"/> para realizar la conversión del estado.
    /// </remarks>
    protected virtual void Init()
    {
        ConvertStateToContent();
    }

    /// <summary>
    /// Convierte el estado de registro en contenido utilizable.
    /// </summary>
    /// <remarks>
    /// Este método verifica si el estado de registro es nulo. Si no lo es, 
    /// convierte el estado en un diccionario y agrega las propiedades de 
    /// registro que no están vacías a la colección de propiedades de registro.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se puede lanzar si hay un problema al convertir el estado de registro.
    /// </exception>
    protected virtual void ConvertStateToContent()
    {
        if (LogState == null)
            return;
        var state = Util.Helpers.Convert.ToDictionary(LogState);
        foreach (var item in state)
        {
            if (item.Value.SafeString().IsEmpty())
                continue;
            LogProperties.Add(item);
        }
    }

    /// <summary>
    /// Obtiene un mensaje formateado que incluye las propiedades de registro y el mensaje de registro.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el mensaje de registro, incluyendo las propiedades si están disponibles.
    /// </returns>
    /// <remarks>
    /// Si no hay propiedades de registro, solo se devolverá el mensaje de registro.
    /// Si hay propiedades, se formatearán en un formato de clave-valor dentro de corchetes, seguido del mensaje de registro.
    /// </remarks>
    protected virtual string GetMessage()
    {
        if (LogProperties.Count == 0)
            return LogMessage.ToString();
        var result = new StringBuilder();
        result.Append("[");
        foreach (var item in LogProperties)
        {
            result.Append(item.Key);
            result.Append(":{");
            result.Append(item.Key);
            result.Append("},");
        }
        result.RemoveEnd(",");
        result.Append("]");
        result.Append(LogMessage);
        return result.ToString();
    }

    /// <summary>
    /// Obtiene los argumentos del mensaje de registro.
    /// </summary>
    /// <returns>
    /// Un arreglo de objetos que contiene los argumentos del mensaje de registro. 
    /// Si no hay propiedades de registro, se devuelve un arreglo que contiene solo los argumentos del mensaje.
    /// </returns>
    /// <remarks>
    /// Este método combina las propiedades de registro y los argumentos del mensaje en un solo arreglo.
    /// Si no hay propiedades de registro definidas, solo se devolverán los argumentos del mensaje.
    /// </remarks>
    protected virtual object[] GetMessageArgs()
    {
        if (LogProperties.Count == 0)
            return LogMessageArgs.ToArray();
        var result = new List<object>();
        result.AddRange(LogProperties.Values);
        result.AddRange(LogMessageArgs);
        return result.ToArray();
    }

    /// <summary>
    /// Escribe un registro en el sistema de logging.
    /// </summary>
    /// <returns>
    /// Devuelve una instancia de <see cref="ILog"/> que representa el registro escrito.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar
    /// una implementación personalizada del registro. Se utiliza el logger configurado para
    /// registrar el contenido, la excepción y el mensaje formateado.
    /// </remarks>
    protected virtual ILog WriteLog()
    {
        Logger.Log(LogLevel, LogEventId, GetContent(), LogException, GetFormatMessage);
        return this;
    }

    /// <summary>
    /// Obtiene el contenido de las propiedades de registro.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene las propiedades de registro si existen, 
    /// de lo contrario, devuelve null.
    /// </returns>
    protected IDictionary<string, object> GetContent()
    {
        if (LogProperties.Count == 0)
            return null;
        return LogProperties;
    }

    /// <summary>
    /// Obtiene un mensaje formateado basado en el contenido proporcionado y una excepción.
    /// </summary>
    /// <param name="content">Un diccionario que contiene el contenido para el formateo del mensaje.</param>
    /// <param name="exception">La excepción que se utilizará para obtener el mensaje, si no es nula.</param>
    /// <returns>
    /// Un mensaje formateado como una cadena. Si la excepción no es nula, se devuelve su mensaje.
    /// Si el contenido es nulo, se devuelve null.
    /// </returns>
    protected virtual string GetFormatMessage(IDictionary<string, object> content, Exception exception)
    {
        if (exception != null)
            return exception.Message;
        if (content == null)
            return null;
        return GetFormatMessage(content);
    }

    /// <summary>
    /// Genera un mensaje formateado a partir de un diccionario de contenido.
    /// </summary>
    /// <param name="content">Un diccionario que contiene pares clave-valor donde la clave es una cadena y el valor es un objeto.</param>
    /// <returns>Una cadena que representa el mensaje formateado, donde cada par clave-valor está separado por comas.</returns>
    /// <remarks>
    /// Este método itera sobre cada elemento del diccionario, concatenando la clave y el valor en un formato específico.
    /// Los valores se convierten a cadena utilizando el método <c>SafeString()</c> para asegurar que no se produzcan excepciones 
    /// en caso de que el valor sea nulo o no se pueda convertir a cadena.
    /// Al final, se elimina la última coma añadida al mensaje.
    /// </remarks>
    protected virtual string GetFormatMessage(IDictionary<string, object> content)
    {
        var result = new StringBuilder();
        foreach (var item in content)
        {
            result.Append(item.Key);
            result.Append(":");
            result.Append(item.Value.SafeString());
            result.Append(",");
        }
        return result.ToString().RemoveEnd(",");
    }

    /// <summary>
    /// Limpia todos los registros de log actuales, restableciendo sus valores a los predeterminados.
    /// </summary>
    /// <remarks>
    /// Este método establece el nivel de log a ninguno, el identificador de evento a cero,
    /// la excepción a nula, el estado a nulo, y reinicia las propiedades del log a un nuevo diccionario vacío.
    /// Además, se limpian los mensajes de log y los argumentos del mensaje.
    /// </remarks>
    protected virtual void Clear()
    {
        LogLevel = LogLevel.None;
        LogEventId = 0;
        LogException = null;
        LogState = null;
        LogProperties = new Dictionary<string, object>();
        LogMessage.Clear();
        LogMessageArgs.Clear();
    }

    #endregion
}