namespace Util.Logging; 

/// <summary>
/// Define una interfaz para el registro de logs con un tipo de categoría específico.
/// </summary>
/// <typeparam name="TCategoryName">El tipo que representa el nombre de la categoría del log.</typeparam>
/// <seealso cref="ILog"/>
public interface ILog<out TCategoryName> : ILog {
}

/// <summary>
/// Define la interfaz para un sistema de registro de logs.
/// </summary>
public interface ILog {
    /// <summary>
    /// Establece el identificador de evento para el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="ILog"/> con el identificador de evento actualizado.</returns>
    /// <remarks>
    /// Este método permite asociar un identificador único a un evento de registro,
    /// facilitando la identificación y el filtrado de eventos en los registros.
    /// </remarks>
    ILog EventId(EventId eventId);
    /// <summary>
    /// Registra una excepción en el sistema de logs.
    /// </summary>
    /// <param name="exception">La excepción que se desea registrar.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el log de la excepción registrada.</returns>
    /// <remarks>
    /// Este método es útil para capturar y almacenar información sobre errores que ocurren en la aplicación,
    /// lo que facilita el diagnóstico y la resolución de problemas.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog Exception( Exception exception );
    /// <summary>
    /// Establece una propiedad en el registro de eventos.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que se va a establecer.</param>
    /// <param name="propertyValue">El valor de la propiedad que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="ILog"/> con la propiedad establecida.</returns>
    /// <remarks>
    /// Este método permite agregar o actualizar propiedades en el registro de eventos,
    /// lo que puede ser útil para incluir información adicional en los registros.
    /// Asegúrese de que el nombre de la propiedad sea único para evitar sobrescribir
    /// valores existentes.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog Property( string propertyName, string propertyValue );
    /// <summary>
    /// Interfaz que define un método para registrar el estado de un objeto.
    /// </summary>
    ILog State( object state );
    /// <summary>
    /// Registra un mensaje en el sistema de logs.
    /// </summary>
    /// <param name="message">El mensaje que se desea registrar. Puede contener formato de cadena.</param>
    /// <param name="args">Los argumentos que se utilizarán para formatear el mensaje.</param>
    /// <remarks>
    /// Este método permite registrar mensajes que pueden incluir información dinámica mediante el uso de parámetros.
    /// Se recomienda utilizar este método para registrar eventos importantes o información de depuración.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog Message( string message, params object[] args );
    /// <summary>
    /// Determina si un nivel de registro específico está habilitado.
    /// </summary>
    /// <param name="logLevel">El nivel de registro que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nivel de registro está habilitado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para comprobar si los mensajes de un nivel de registro particular deben ser procesados 
    /// o ignorados, lo que permite optimizar el rendimiento de la aplicación al evitar el registro innecesario.
    /// </remarks>
    bool IsEnabled( LogLevel logLevel );
    /// <summary>
    /// Inicia un nuevo alcance de registro con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se pasará al nuevo alcance.</typeparam>
    /// <param name="state">El estado que se utilizará en el nuevo alcance.</param>
    /// <returns>Un objeto <see cref="IDisposable"/> que se puede utilizar para finalizar el alcance.</returns>
    /// <remarks>
    /// Este método permite crear un contexto de registro que puede ser utilizado para agrupar mensajes de registro 
    /// relacionados. Al finalizar el alcance, se debe llamar al método <see cref="IDisposable.Dispose"/> 
    /// para liberar los recursos asociados.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    IDisposable BeginScope<TState>( TState state );
    /// <summary>
    /// Obtiene una instancia de un objeto que implementa la interfaz <see cref="ILog"/> 
    /// para registrar información de seguimiento (trace).
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que se puede utilizar para registrar mensajes de seguimiento.
    /// </returns>
    /// <remarks>
    /// Este método es útil para habilitar el registro de eventos de seguimiento que pueden ayudar 
    /// en la depuración y monitoreo de la aplicación.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog LogTrace();
    /// <summary>
    /// Obtiene una instancia de registro para el nivel de depuración.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que se utiliza para registrar mensajes de depuración.
    /// </returns>
    /// <remarks>
    /// Este método es útil para registrar información detallada que puede ser utilizada para diagnosticar problemas durante el desarrollo.
    /// </remarks>
    ILog LogDebug();
    /// <summary>
    /// Obtiene una instancia de un registro de información.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el registro de información.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar información en el sistema de logging.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog LogInformation();
    /// <summary>
    /// Registra un mensaje de advertencia en el sistema de logs.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que representa el registro de la advertencia.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar advertencias que pueden requerir atención, pero que no son errores críticos.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog LogWarning();
    /// <summary>
    /// Registra un error en el sistema de logs.
    /// </summary>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el registro del error.</returns>
    /// <remarks>
    /// Este método se utiliza para capturar y registrar errores que ocurren durante la ejecución de la aplicación.
    /// Asegúrese de que el sistema de logging esté correctamente configurado antes de llamar a este método.
    /// </remarks>
    ILog LogError();
    /// <summary>
    /// Registra un mensaje de nivel crítico.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ILog"/> que representa el registro del mensaje crítico.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar eventos que son críticos para el funcionamiento de la aplicación,
    /// lo que puede indicar un fallo grave o una condición que requiere atención inmediata.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog LogCritical();
}