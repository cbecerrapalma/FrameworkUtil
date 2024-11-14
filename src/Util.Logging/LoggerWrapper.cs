namespace Util.Logging; 

/// <summary>
/// Clase que implementa la interfaz <see cref="ILoggerWrapper"/> para proporcionar funcionalidades de registro.
/// </summary>
public class LoggerWrapper : ILoggerWrapper {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LoggerWrapper"/>.
    /// </summary>
    /// <param name="logger">Una instancia de <see cref="ILogger"/> que se utilizará para el registro de eventos.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="logger"/> es <c>null</c>.</exception>
    public LoggerWrapper( ILogger logger ) {
        Logger = logger ?? throw new ArgumentNullException( nameof( logger ) );
    }

    /// <summary>
    /// Obtiene el registrador de logs utilizado por la clase.
    /// </summary>
    /// <remarks>
    /// Este registrador permite registrar información, advertencias y errores
    /// durante la ejecución de la aplicación, facilitando el diagnóstico y la
    /// solución de problemas.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ILogger"/>.
    /// </value>
    protected ILogger Logger { get; }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el registro está habilitado para un nivel de log específico.
    /// </summary>
    /// <param name="logLevel">El nivel de log que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el registro está habilitado para el nivel de log especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <seealso cref="Logger.IsEnabled(LogLevel)"/>
    public virtual bool IsEnabled( LogLevel logLevel ) {
        return Logger.IsEnabled( logLevel );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de log con el nivel de log especificado, el identificador del evento, el estado, 
    /// una excepción opcional y un formateador personalizado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se va a registrar.</typeparam>
    /// <param name="logLevel">El nivel de log que se utilizará para el mensaje.</param>
    /// <param name="eventId">El identificador del evento asociado con el mensaje de log.</param>
    /// <param name="state">El estado que se va a registrar.</param>
    /// <param name="exception">La excepción asociada con el mensaje de log, si la hay.</param>
    /// <param name="formatter">Una función que formatea el estado y la excepción en un string.</param>
    /// <seealso cref="ILogger{TState}"/>
    /// <remarks>
    /// Este método permite personalizar la forma en que se registran los mensajes de log, 
    /// facilitando la inclusión de información adicional a través del estado y la excepción.
    /// </remarks>
    public virtual void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter ) {
        Logger.Log( logLevel, eventId, state, exception, formatter );
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia un nuevo ámbito de registro con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo de estado que se utilizará en el ámbito.</typeparam>
    /// <param name="state">El estado que se asociará con el nuevo ámbito de registro.</param>
    /// <returns>Un objeto <see cref="IDisposable"/> que se puede utilizar para finalizar el ámbito.</returns>
    /// <remarks>
    /// Este método permite agrupar los registros generados durante el ámbito especificado,
    /// lo que facilita la organización y el seguimiento de los eventos de registro.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    public virtual IDisposable BeginScope<TState>( TState state ) {
        return Logger.BeginScope( state );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de traza en el sistema de registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada con el evento, si la hay.</param>
    /// <param name="message">El mensaje que se va a registrar.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    public void LogTrace( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogTrace( eventId,exception,message,args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de depuración en el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada al evento, si la hay.</param>
    /// <param name="message">El mensaje de depuración que se desea registrar.</param>
    /// <param name="args">Los argumentos opcionales que se formatearán en el mensaje.</param>
    public void LogDebug( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogDebug( eventId, exception, message, args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de información en el registro de eventos.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada al evento, si la hay.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje.</param>
    public void LogInformation( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogInformation( eventId, exception, message, args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de advertencia en el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento asociado con el mensaje de advertencia.</param>
    /// <param name="exception">La excepción que se está registrando, si corresponde.</param>
    /// <param name="message">El mensaje de advertencia que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    public void LogWarning( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogWarning( eventId, exception, message, args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un error en el sistema de logging.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción que se ha producido.</param>
    /// <param name="message">El mensaje de error que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje.</param>
    public void LogError( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogError( eventId, exception, message, args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de nivel crítico en el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada con el evento, si la hay.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    public void LogCritical( EventId eventId, Exception exception, string message, params object[] args ) {
        Logger.LogCritical( eventId, exception, message, args );
    }
}