namespace Util.Logging; 

/// <summary>
/// Interfaz que define un envoltorio para funcionalidades de registro (logging).
/// </summary>
public interface ILoggerWrapper {
    /// <summary>
    /// Determina si un nivel de registro específico está habilitado.
    /// </summary>
    /// <param name="logLevel">El nivel de registro que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nivel de registro está habilitado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para comprobar si los mensajes de un determinado nivel de registro
    /// deben ser procesados o ignorados, lo que permite optimizar el rendimiento de la aplicación
    /// al evitar el registro innecesario.
    /// </remarks>
    bool IsEnabled( LogLevel logLevel );
    /// <summary>
    /// Registra un mensaje de log con el nivel de severidad especificado, un identificador de evento, el estado del log, 
    /// una excepción opcional y un formateador que convierte el estado y la excepción en una cadena.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se va a registrar.</typeparam>
    /// <param name="logLevel">El nivel de severidad del log.</param>
    /// <param name="eventId">El identificador del evento asociado al log.</param>
    /// <param name="state">El estado que se va a registrar.</param>
    /// <param name="exception">La excepción asociada al log, si existe.</param>
    /// <param name="formatter">Una función que toma el estado y la excepción y devuelve una cadena formateada.</param>
    /// <remarks>
    /// Este método es útil para registrar información detallada sobre eventos en la aplicación, 
    /// permitiendo a los desarrolladores diagnosticar problemas y entender el flujo de la aplicación.
    /// </remarks>
    /// <seealso cref="LogLevel"/>
    /// <seealso cref="EventId"/>
    /// <seealso cref="Exception"/>
    void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter );
    /// <summary>
    /// Inicia un nuevo ámbito de ejecución con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se pasará al ámbito.</typeparam>
    /// <param name="state">El estado que se utilizará en el nuevo ámbito.</param>
    /// <returns>Un objeto <see cref="IDisposable"/> que se debe usar para liberar el ámbito una vez que se complete el trabajo.</returns>
    /// <remarks>
    /// Este método permite crear un contexto que puede ser utilizado para agrupar operaciones 
    /// relacionadas y gestionar su ciclo de vida. Es importante asegurarse de que el objeto 
    /// devuelto se elimine correctamente para evitar fugas de recursos.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    IDisposable BeginScope<TState>( TState state );
    /// <summary>
    /// Registra un mensaje de traza con un identificador de evento y una excepción asociada.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción que se ha producido, si corresponde.</param>
    /// <param name="message">El mensaje de traza que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    /// <remarks>
    /// Este método permite registrar información detallada sobre eventos en el sistema,
    /// lo que puede ser útil para la depuración y el monitoreo de la aplicación.
    /// </remarks>
    void LogTrace( EventId eventId, Exception exception, string message, params object[] args );
    /// <summary>
    /// Registra un mensaje de depuración con un identificador de evento y una excepción asociada.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada al evento, si existe.</param>
    /// <param name="message">El mensaje de depuración que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje de depuración.</param>
    void LogDebug(EventId eventId, Exception exception, string message, params object[] args);
    /// <summary>
    /// Registra información de un evento, incluyendo un mensaje y una excepción.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada al evento, si la hay.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje.</param>
    /// <remarks>
    /// Este método es útil para registrar información detallada sobre eventos en la aplicación,
    /// permitiendo un mejor seguimiento y diagnóstico de problemas.
    /// </remarks>
    void LogInformation( EventId eventId, Exception exception, string message, params object[] args );
    /// <summary>
    /// Registra un mensaje de advertencia con un identificador de evento y una excepción asociada.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción que se ha producido, si corresponde.</param>
    /// <param name="message">El mensaje de advertencia que se registrará.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    /// <remarks>
    /// Este método permite registrar advertencias que pueden ser útiles para la depuración y el monitoreo de la aplicación.
    /// Se puede incluir información adicional mediante el uso de argumentos de formato.
    /// </remarks>
    void LogWarning(EventId eventId, Exception exception, string message, params object[] args);
    /// <summary>
    /// Registra un error con información detallada.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción que se ha producido.</param>
    /// <param name="message">El mensaje de error que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje.</param>
    /// <remarks>
    /// Este método se utiliza para registrar errores en el sistema, proporcionando un contexto adicional 
    /// a través del identificador del evento y la excepción asociada. Los argumentos opcionales permiten 
    /// personalizar el mensaje de error.
    /// </remarks>
    /// <seealso cref="EventId"/>
    /// <seealso cref="Exception"/>
    void LogError( EventId eventId, Exception exception, string message, params object[] args );
    /// <summary>
    /// Registra un mensaje crítico en el sistema de logs.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se está registrando.</param>
    /// <param name="exception">La excepción asociada al evento, si la hay.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="args">Argumentos opcionales que se pueden incluir en el mensaje.</param>
    /// <remarks>
    /// Este método es útil para registrar información crítica que requiere atención inmediata. 
    /// Se recomienda utilizar este método cuando se produzcan errores graves que puedan afectar 
    /// el funcionamiento de la aplicación.
    /// </remarks>
    /// <seealso cref="LogError"/>
    /// <seealso cref="LogWarning"/>
    void LogCritical( EventId eventId, Exception exception, string message, params object[] args );
}