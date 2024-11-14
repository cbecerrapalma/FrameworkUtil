namespace Util.Logging; 

/// <summary>
/// Proporciona métodos de extensión para la interfaz <see cref="ILog"/>.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten mejorar la funcionalidad de los registros
/// a través de la interfaz <see cref="ILog"/>. Los métodos de extensión permiten
/// agregar características adicionales sin modificar la interfaz original.
/// </remarks>
public static class ILogExtensions {
    /// <summary>
    /// Agrega un mensaje al registro de logs con formato.
    /// </summary>
    /// <param name="log">El objeto de registro de logs donde se añadirá el mensaje.</param>
    /// <param name="message">El mensaje que se desea registrar, que puede incluir marcadores de formato.</param>
    /// <param name="args">Los argumentos que se utilizarán para formatear el mensaje.</param>
    /// <returns>El objeto de registro de logs actualizado.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad del objeto <see cref="ILog"/> permitiendo agregar mensajes de manera más sencilla.
    /// Se asegura de que el objeto de registro no sea nulo antes de proceder a registrar el mensaje.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="log"/> es nulo.</exception>
    public static ILog Append( this ILog log,string message, params object[] args ) {
        log.CheckNull( nameof( log ) );
        log.Message( message, args );
        return log;
    }

    /// <summary>
    /// Agrega un mensaje al registro de logs si se cumple una condición especificada.
    /// </summary>
    /// <param name="log">El objeto de registro donde se agregará el mensaje.</param>
    /// <param name="message">El mensaje que se registrará si la condición es verdadera.</param>
    /// <param name="condition">La condición que determina si se debe registrar el mensaje.</param>
    /// <param name="args">Argumentos opcionales que se formatearán en el mensaje.</param>
    /// <returns>El objeto de registro para permitir la encadenación de llamadas.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="log"/> es nulo.</exception>
    /// <remarks>
    /// Este método es útil para evitar la creación de mensajes de log innecesarios cuando no se cumplen ciertas condiciones.
    /// </remarks>
    public static ILog AppendIf( this ILog log, string message,bool condition, params object[] args ) {
        log.CheckNull( nameof( log ) );
        if ( condition )
            log.Message( message, args );
        return log;
    }

    /// <summary>
    /// Agrega una línea de mensaje al registro de logs.
    /// </summary>
    /// <param name="log">La instancia de <see cref="ILog"/> donde se registrará el mensaje.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="args">Los argumentos opcionales que se formatearán en el mensaje.</param>
    /// <returns>La misma instancia de <see cref="ILog"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método verifica que la instancia de log no sea nula antes de proceder a registrar el mensaje.
    /// Luego, se agrega un salto de línea después del mensaje.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="log"/> es nulo.</exception>
    public static ILog AppendLine( this ILog log, string message, params object[] args ) {
        log.CheckNull( nameof( log ) );
        log.Message( message, args );
        log.Message( Environment.NewLine );
        return log;
    }

    /// <summary>
    /// Agrega un mensaje al registro de logs si se cumple una condición.
    /// </summary>
    /// <param name="log">El objeto de registro de logs donde se agregará el mensaje.</param>
    /// <param name="message">El mensaje que se desea registrar.</param>
    /// <param name="condition">La condición que determina si se debe registrar el mensaje.</param>
    /// <param name="args">Argumentos opcionales que se utilizarán para formatear el mensaje.</param>
    /// <returns>El objeto de registro de logs, permitiendo la concatenación de llamadas.</returns>
    /// <remarks>
    /// Este método verifica si la condición es verdadera. Si es así, registra el mensaje
    /// formateado con los argumentos proporcionados y agrega una nueva línea después del mensaje.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="log"/> es nulo.</exception>
    public static ILog AppendLineIf( this ILog log, string message, bool condition, params object[] args ) {
        log.CheckNull( nameof( log ) );
        if ( condition ) {
            log.Message( message, args );
            log.Message( Environment.NewLine );
        }
        return log;
    }

    /// <summary>
    /// Agrega una nueva línea en el registro de logs.
    /// </summary>
    /// <param name="log">La instancia de <see cref="ILog"/> en la que se registrará la nueva línea.</param>
    /// <returns>La misma instancia de <see cref="ILog"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="ILog"/> que permite agregar una línea vacía
    /// al registro, facilitando la organización de los mensajes de log.
    /// </remarks>
    public static ILog Line( this ILog log ) {
        log.CheckNull( nameof(log) );
        log.Message( Environment.NewLine );
        return log;
    }
}