using Util.Properties;

namespace Util.Exceptions; 

/// <summary>
/// Representa una excepción que se produce en situaciones de concurrencia.
/// Esta clase hereda de <see cref="Warning"/> y se utiliza para señalar
/// advertencias relacionadas con problemas de concurrencia en la aplicación.
/// </summary>
public class ConcurrencyException : Warning {
    private readonly string _message;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConcurrencyException"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una excepción de concurrencia sin un mensaje específico.
    /// </remarks>
    public ConcurrencyException()
        : this( "" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConcurrencyException"/> 
    /// con una excepción específica.
    /// </summary>
    /// <param name="exception">La excepción que se produjo.</param>
    public ConcurrencyException( Exception exception )
        : this( "", exception ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConcurrencyException"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe el error.</param>
    /// <param name="exception">La excepción interna que causó este error. Este parámetro es opcional.</param>
    /// <param name="code">Un código que representa el error. Este parámetro es opcional.</param>
    /// <param name="httpStatusCode">El código de estado HTTP asociado con el error. Este parámetro es opcional.</param>
    public ConcurrencyException( string message, Exception exception = null, string code = null, int? httpStatusCode = null )
        : base( message, exception, code, httpStatusCode ) {
        _message = message;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un mensaje que describe la excepción de concurrencia.
    /// </summary>
    /// <remarks>
    /// Este mensaje combina un mensaje de error predefinido con un mensaje específico de la instancia.
    /// </remarks>
    /// <value>
    /// Un string que representa el mensaje de la excepción de concurrencia.
    /// </value>
    public override string Message => $"{R.ConcurrencyExceptionMessage}.{_message}";

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un mensaje basado en el estado de producción.
    /// </summary>
    /// <param name="isProduction">Indica si se está en un entorno de producción.</param>
    /// <returns>
    /// Un mensaje de error correspondiente a la excepción de concurrencia si 
    /// está en producción; de lo contrario, devuelve un mensaje personalizado 
    /// obtenido mediante el método <see cref="GetMessage(object)"/>.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar un comportamiento 
    /// específico según el entorno de ejecución.
    /// </remarks>
    public override string GetMessage( bool isProduction = false ) {
        if( isProduction )
            return R.ConcurrencyExceptionMessage;
        return GetMessage(this);
    }
}