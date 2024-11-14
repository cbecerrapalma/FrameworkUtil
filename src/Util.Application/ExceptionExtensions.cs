using Util.Aop;
using Util.Exceptions;
using Util.Properties;

namespace Util.Applications; 

/// <summary>
/// Proporciona métodos de extensión para manejar excepciones.
/// </summary>
public static class ExceptionExtensions {
    /// <summary>
    /// Obtiene un mensaje de aviso o error a partir de una excepción.
    /// </summary>
    /// <param name="exception">La excepción de la cual se desea obtener el mensaje.</param>
    /// <param name="isProduction">Indica si el entorno es de producción. Si es verdadero, se devuelve un mensaje genérico en caso de error.</param>
    /// <returns>
    /// Un string que representa el mensaje de la excepción o un mensaje de error genérico si se encuentra en producción.
    /// Si la excepción es nula, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método extiende la clase <see cref="Exception"/> y permite obtener un mensaje más amigable
    /// dependiendo del tipo de excepción y del entorno en el que se esté ejecutando el código.
    /// </remarks>
    /// <seealso cref="Warning"/>
    /// <seealso cref="GetRawException()"/>
    public static string GetPrompt( this Exception exception, bool isProduction = false ) {
        if( exception == null )
            return null;
        exception = exception.GetRawException();
        if( exception == null )
            return null;
        if( exception is Warning warning )
            return warning.GetMessage( isProduction );
        return isProduction ? R.SystemError : exception.Message;
    }

    /// <summary>
    /// Obtiene el código de estado HTTP asociado a una excepción.
    /// </summary>
    /// <param name="exception">La excepción de la cual se desea obtener el código de estado HTTP.</param>
    /// <returns>
    /// Un código de estado HTTP como un entero nullable si se encuentra, 
    /// de lo contrario, null si la excepción es nula o no se puede determinar el código.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la excepción es nula y si es así, 
    /// devuelve null. Luego intenta obtener la excepción original 
    /// utilizando el método <c>GetRawException</c>. Si la excepción 
    /// original es de tipo <c>Warning</c>, se devuelve el código 
    /// de estado HTTP asociado. En caso contrario, se devuelve null.
    /// </remarks>
    /// <seealso cref="Warning"/>
    public static int? GetHttpStatusCode( this Exception exception ) {
        if ( exception == null )
            return null;
        exception = exception.GetRawException();
        if ( exception == null )
            return null;
        if ( exception is Warning warning )
            return warning.HttpStatusCode;
        return null;
    }

    /// <summary>
    /// Obtiene el código de error de una excepción, si está disponible.
    /// </summary>
    /// <param name="exception">La excepción de la cual se desea obtener el código de error.</param>
    /// <returns>El código de error como una cadena, o null si no se puede obtener.</returns>
    /// <remarks>
    /// Este método verifica si la excepción es nula y, en caso afirmativo, retorna null.
    /// Luego intenta obtener la excepción original a través del método <see cref="GetRawException"/>.
    /// Si la excepción original es de tipo <see cref="Warning"/>, se devuelve su código.
    /// En caso contrario, se retorna null.
    /// </remarks>
    /// <seealso cref="Warning"/>
    public static string GetErrorCode( this Exception exception ) {
        if ( exception == null )
            return null;
        exception = exception.GetRawException();
        if ( exception == null )
            return null;
        if ( exception is Warning warning )
            return warning.Code;
        return null;
    }
}