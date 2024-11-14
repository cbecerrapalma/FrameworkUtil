namespace Util.Logging; 

/// <summary>
/// Representa un registro de log genérico para una categoría específica.
/// </summary>
/// <typeparam name="TCategoryName">El tipo que representa el nombre de la categoría del log.</typeparam>
public class Log<TCategoryName> : ILog<TCategoryName> {
    private readonly ILog _log;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Log"/>.
    /// </summary>
    /// <param name="factory">La fábrica de registros que se utilizará para crear el registro.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="factory"/> es nulo.</exception>
    /// <remarks>
    /// Este constructor verifica que la fábrica proporcionada no sea nula y luego crea un registro utilizando el tipo de categoría especificado por <typeparamref name="TCategoryName"/>.
    /// </remarks>
    /// <typeparam name="TCategoryName">El tipo que representa el nombre de la categoría del registro.</typeparam>
    public Log( ILogFactory factory ) {
        factory.CheckNull( nameof(factory) );
        _log = factory.CreateLog(typeof( TCategoryName ) );
    }

    public static ILog<TCategoryName> Null = NullLog<TCategoryName>.Instance;

    /// <inheritdoc />
    /// <summary>
    /// Establece el identificador del evento para el registro.
    /// </summary>
    /// <param name="eventId">El identificador del evento que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el registro con el identificador de evento establecido.</returns>
    /// <remarks>
    /// Este método permite asociar un identificador único a un evento de registro, facilitando su identificación y seguimiento.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public ILog EventId( EventId eventId ) {
        return _log.EventId( eventId );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra una excepción en el sistema de logs.
    /// </summary>
    /// <param name="exception">La excepción que se desea registrar.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el registro de la excepción.</returns>
    /// <remarks>
    /// Este método delega la llamada al método <see cref="_log.Exception(Exception)"/> 
    /// para realizar el registro de la excepción.
    /// </remarks>
    public ILog Exception( Exception exception ) {
        return _log.Exception( exception );
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una propiedad en el registro con el nombre y valor especificados.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que se va a establecer.</param>
    /// <param name="propertyValue">El valor de la propiedad que se va a establecer.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="ILog"/> con la propiedad establecida.</returns>
    /// <seealso cref="ILog"/>
    public ILog Property( string propertyName, string propertyValue ) {
        return _log.Property( propertyName, propertyValue );
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el estado del registro.
    /// </summary>
    /// <param name="state">El objeto que representa el estado a establecer.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="ILog"/> que representa el registro actualizado.</returns>
    /// <seealso cref="ILog"/>
    public ILog State( object state ) {
        return _log.State( state );
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía un mensaje de registro con formato a la instancia de registro.
    /// </summary>
    /// <param name="message">El mensaje de registro que se va a enviar, que puede contener formato.</param>
    /// <param name="args">Los argumentos que se utilizarán para formatear el mensaje.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que representa el registro del mensaje enviado.</returns>
    /// <remarks>
    /// Este método permite registrar mensajes que pueden incluir parámetros de formato, 
    /// facilitando la creación de mensajes dinámicos y personalizados.
    /// </remarks>
    public ILog Message( string message, params object[] args ) {
        return _log.Message( message, args );
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el registro está habilitado para un nivel de log específico.
    /// </summary>
    /// <param name="logLevel">El nivel de log que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el registro está habilitado para el nivel de log especificado; de lo contrario, <c>false</c>.
    /// </returns>
    /// <seealso cref="LogLevel"/>
    public bool IsEnabled( LogLevel logLevel ) {
        return _log.IsEnabled( logLevel );
    }

    /// <inheritdoc />
    /// <summary>
    /// Inicia un nuevo ámbito de registro con el estado especificado.
    /// </summary>
    /// <typeparam name="TState">El tipo del estado que se va a asociar con el ámbito de registro.</typeparam>
    /// <param name="state">El estado que se va a utilizar para el nuevo ámbito.</param>
    /// <returns>
    /// Un objeto <see cref="IDisposable"/> que se puede utilizar para finalizar el ámbito de registro.
    /// </returns>
    /// <remarks>
    /// Al llamar a este método, se crea un nuevo ámbito que puede ser utilizado para registrar información contextual 
    /// que se relaciona con el estado proporcionado. Es importante asegurarse de que el objeto devuelto se elimine 
    /// adecuadamente para evitar fugas de recursos.
    /// </remarks>
    /// <seealso cref="IDisposable"/>
    public IDisposable BeginScope<TState>( TState state ) {
        return _log.BeginScope( state );
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de traza utilizando el sistema de registro.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa la instancia de registro de traza.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para registrar información detallada que puede ser útil para el diagnóstico y la depuración.
    /// </remarks>
    public ILog LogTrace() {
        return _log.LogTrace();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de depuración utilizando el registro definido.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa el registro de depuración.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobrescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    public virtual ILog LogDebug() {
        return _log.LogDebug();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de información utilizando el logger asociado.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa el registro de información.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    public virtual ILog LogInformation() {
        return _log.LogInformation();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de advertencia en el sistema de registro.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa el registro de la advertencia.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// una implementación personalizada del registro de advertencias.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public virtual ILog LogWarning() {
        return _log.LogWarning();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un error utilizando el logger asociado.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que representa el logger para el error registrado.
    /// </returns>
    /// <remarks>
    /// Este método permite registrar un error en el sistema de logging.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public virtual ILog LogError() {
        return _log.LogError();
    }

    /// <inheritdoc />
    /// <summary>
    /// Registra un mensaje de nivel crítico.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ILog"/> que permite realizar operaciones de registro adicionales.
    /// </returns>
    /// <remarks>
    /// Este método es parte de la implementación de un sistema de registro y se utiliza para 
    /// registrar eventos que requieren atención inmediata.
    /// </remarks>
    /// <seealso cref="ILog"/>
    public virtual ILog LogCritical() {
        return _log.LogCritical();
    }
}