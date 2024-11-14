namespace Util.Logging; 

/// <summary>
/// Representa una implementación nula de un registro de log.
/// Esta clase no realiza ninguna acción al registrar información,
/// lo que puede ser útil en situaciones donde no se desea 
/// realizar un registro real, como en pruebas o entornos de producción 
/// donde el registro no es necesario.
/// </summary>
/// <typeparam name="TCategoryName">El tipo que representa el nombre de la categoría del log.</typeparam>
public class NullLog<TCategoryName> : ILog<TCategoryName> {
    public static readonly ILog<TCategoryName> Instance = new NullLog<TCategoryName>();

    /// <inheritdoc />
    /// <summary>
    /// Establece el identificador del evento para el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="ILog"/> para permitir el encadenamiento de llamadas.</returns>
    public ILog EventId( EventId eventId ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una excepción y devuelve la instancia actual de <see cref="ILog"/>.
    /// </summary>
    /// <param name="exception">La excepción que se va a registrar.</param>
    /// <returns>La instancia actual de <see cref="ILog"/> para permitir el encadenamiento de llamadas.</returns>
    /// <seealso cref="ILog"/>
    public ILog Exception( Exception exception ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el valor de una propiedad específica.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que se desea establecer.</param>
    /// <param name="propertyValue">El valor que se asignará a la propiedad.</param>
    /// <returns>Devuelve la instancia actual de <see cref="ILog"/>.</returns>
    /// <remarks>
    /// Este método permite configurar propiedades de registro de manera fluida.
    /// </remarks>
    public ILog Property( string propertyName, string propertyValue ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el estado del registro.
    /// </summary>
    /// <param name="state">El objeto que representa el estado a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="ILog"/>.</returns>
    /// <remarks>
    /// Este método permite modificar el estado del registro, permitiendo que se
    /// realicen operaciones adicionales en función del nuevo estado establecido.
    /// </remarks>
    public ILog State( object state ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje con formato utilizando el mensaje especificado y los argumentos proporcionados.
    /// </summary>
    /// <param name="message">El mensaje que se va a registrar, que puede contener marcadores de posición para los argumentos.</param>
    /// <param name="args">Los argumentos que se utilizarán para formatear el mensaje.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que permite la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite registrar mensajes de log de manera flexible, permitiendo la inclusión de múltiples argumentos
    /// para personalizar el contenido del mensaje.
    /// </remarks>
    public ILog Message( string message, params object[] args ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el nivel de registro especificado está habilitado.
    /// </summary>
    /// <param name="logLevel">El nivel de registro que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nivel de registro está habilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c>, lo que indica que ningún nivel de registro está habilitado.
    /// </remarks>
    public bool IsEnabled( LogLevel logLevel ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia un nuevo ámbito de registro con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se pasará al ámbito.</typeparam>
    /// <param name="state">El estado que se asociará con el nuevo ámbito.</param>
    /// <returns>Un objeto que se puede utilizar para liberar recursos cuando el ámbito ya no sea necesario.</returns>
    /// <remarks>
    /// Este método crea un nuevo ámbito de registro que puede ser utilizado para agrupar mensajes de registro 
    /// relacionados con el estado proporcionado. El objeto devuelto debe ser descartado para liberar 
    /// los recursos asociados con el ámbito.
    /// </remarks>
    public IDisposable BeginScope<TState>( TState state ) {
        return new DisposeAction( () => { } );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de traza.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite realizar operaciones de registro adicionales.
    /// </returns>
    /// <remarks>
    /// Este método permite registrar información de traza que puede ser útil para el diagnóstico y la depuración.
    /// </remarks>
    public ILog LogTrace() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de depuración.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite encadenar llamadas a métodos de registro.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar información de depuración que puede ser útil durante el desarrollo o la resolución de problemas.
    /// </remarks>
    public ILog LogDebug() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de información.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite realizar más operaciones de registro.
    /// </returns>
    /// <remarks>
    /// Este método permite al usuario registrar información en el sistema de logging. 
    /// Se puede utilizar para registrar eventos que no son errores, pero que son relevantes 
    /// para el seguimiento del comportamiento de la aplicación.
    /// </remarks>
    public ILog LogInformation() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de advertencia en el sistema de logs.
    /// </summary>
    /// <returns>
    /// Devuelve una instancia de <see cref="ILog"/> que permite encadenar llamadas a otros métodos de registro.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para registrar advertencias que no son errores pero que requieren atención.
    /// </remarks>
    public ILog LogWarning() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un error en el sistema de logs.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite encadenar llamadas para registrar otros mensajes o errores.
    /// </returns>
    /// <remarks>
    /// Este método implementa la funcionalidad para registrar un error y puede ser utilizado en un contexto donde se requiera
    /// un seguimiento de errores en la aplicación.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public ILog LogError() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de nivel crítico.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite realizar más operaciones de registro.
    /// </returns>
    /// <inheritdoc />
    public ILog LogCritical() {
        return this;
    }
}

/// <summary>
/// Implementa una clase de registro que no realiza ninguna acción.
/// Esta clase es útil para situaciones donde se requiere un registro, 
/// pero no se desea que se realice ninguna operación de registro real.
/// </summary>
public class NullLog : ILog {
    public static readonly  ILog Instance = new NullLog();

    /// <inheritdoc />
    /// <summary>
    /// Establece el identificador de evento para el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="ILog"/> para permitir el encadenamiento de llamadas.</returns>
    public ILog EventId( EventId eventId ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una excepción.
    /// </summary>
    /// <param name="exception">La excepción que se va a registrar.</param>
    /// <returns>Devuelve la instancia actual de <see cref="ILog"/> para permitir la chaining de métodos.</returns>
    /// <remarks>
    /// Este método implementa la interfaz <see cref="ILog"/> y permite registrar excepciones en el sistema de logging.
    /// </remarks>
    public ILog Exception( Exception exception ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el valor de una propiedad específica.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que se desea establecer.</param>
    /// <param name="propertyValue">El valor que se asignará a la propiedad.</param>
    /// <returns>Devuelve la instancia actual del objeto <see cref="ILog"/>.</returns>
    /// <remarks>
    /// Este método permite configurar propiedades de registro de manera fluida.
    /// </remarks>
    public ILog Property( string propertyName, string propertyValue ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el estado del registro.
    /// </summary>
    /// <param name="state">El objeto que representa el estado que se desea establecer.</param>
    /// <returns>Devuelve la instancia actual del registro.</returns>
    /// <remarks>
    /// Este método permite modificar el estado del registro sin necesidad de crear una nueva instancia.
    /// </remarks>
    public ILog State( object state ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje con formato utilizando los argumentos proporcionados.
    /// </summary>
    /// <param name="message">El mensaje que se va a registrar, que puede contener marcadores de posición para los argumentos.</param>
    /// <param name="args">Los argumentos que se utilizarán para formatear el mensaje.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que permite el encadenamiento de llamadas.</returns>
    /// <remarks>
    /// Este método permite registrar mensajes de log de manera flexible, utilizando un formato similar al de <c>string.Format</c>.
    /// </remarks>
    public ILog Message( string message, params object[] args ) {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si un nivel de registro específico está habilitado.
    /// </summary>
    /// <param name="logLevel">El nivel de registro que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nivel de registro está habilitado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método siempre devuelve <c>false</c>, lo que indica que ningún nivel de registro está habilitado.
    /// </remarks>
    public bool IsEnabled( LogLevel logLevel ) {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia un nuevo ámbito de trabajo con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se pasará al ámbito.</typeparam>
    /// <param name="state">El estado que se utilizará en el ámbito.</param>
    /// <returns>Un objeto <see cref="IDisposable"/> que se puede utilizar para liberar el ámbito.</returns>
    /// <remarks>
    /// Este método crea un nuevo ámbito que puede ser utilizado para agrupar operaciones relacionadas.
    /// Al finalizar el uso del ámbito, se debe llamar al método <see cref="IDisposable.Dispose"/> 
    /// del objeto devuelto para liberar los recursos asociados.
    /// </remarks>
    public IDisposable BeginScope<TState>( TState state ) {
        return new DisposeAction( () => { } );
    }

    /// <inheritdoc />
    /// <summary>
    /// Devuelve una instancia de <see cref="ILog"/> que permite registrar mensajes de traza.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que se puede utilizar para registrar información de traza.
    /// </returns>
    /// <remarks>
    /// Este método es útil para habilitar el registro de información detallada que puede ser útil para el diagnóstico y la depuración.
    /// </remarks>
    public ILog LogTrace() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de depuración.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite la encadenación de llamadas.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar información útil durante el desarrollo y la depuración.
    /// </remarks>
    public ILog LogDebug() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Devuelve una instancia de <see cref="ILog"/> que permite registrar información.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que se puede utilizar para registrar mensajes de información.
    /// </returns>
    /// <seealso cref="ILog"/>
    public ILog LogInformation() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de advertencia en el sistema de logs.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="ILog"/> que permite encadenar llamadas a otros métodos de registro.
    /// </returns>
    /// <remarks>
    /// Este método permite al usuario registrar advertencias que pueden ser útiles para el diagnóstico
    /// de problemas sin interrumpir el flujo normal de la aplicación.
    /// </remarks>
    public ILog LogWarning() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un error y devuelve la instancia actual del logger.
    /// </summary>
    /// <returns>
    /// La instancia actual de <see cref="ILog"/> para permitir el encadenamiento de llamadas.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para registrar errores en el sistema de logging.
    /// </remarks>
    public ILog LogError() {
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de nivel crítico.
    /// </summary>
    /// <returns>
    /// Devuelve una instancia de <see cref="ILog"/> para permitir el encadenamiento de métodos.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar eventos que requieren atención inmediata.
    /// </remarks>
    public ILog LogCritical() {
        return this;
    }
}