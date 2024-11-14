namespace Util.Exceptions; 

/// <summary>
/// Representa una advertencia que hereda de la clase <see cref="Exception"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para lanzar advertencias específicas en el flujo de la aplicación,
/// permitiendo que se manejen de manera diferenciada de las excepciones estándar.
/// </remarks>
public class Warning : Exception {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Warning"/> con la excepción especificada.
    /// </summary>
    /// <param name="exception">La excepción que se desea asociar con la advertencia.</param>
    public Warning( Exception exception )
        : this( null, exception ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Warning"/>.
    /// </summary>
    /// <param name="message">El mensaje que describe la advertencia.</param>
    /// <param name="exception">La excepción asociada a la advertencia, si existe. Por defecto es <c>null</c>.</param>
    /// <param name="code">Un código opcional que representa la advertencia. Por defecto es <c>null</c>.</param>
    /// <param name="httpStatusCode">El código de estado HTTP opcional asociado a la advertencia. Por defecto es <c>null</c>.</param>
    /// <remarks>
    /// Esta clase se utiliza para representar advertencias que pueden incluir un mensaje, una excepción, un código y un código de estado HTTP.
    /// </remarks>
    public Warning( string message, Exception exception = null, string code = null, int? httpStatusCode = null )
        : base( message ?? "", exception ) {
        Code = code;
        HttpStatusCode = httpStatusCode;
    }

    /// <summary>
    /// Obtiene o establece el código asociado.
    /// </summary>
    /// <value>
    /// Una cadena que representa el código.
    /// </value>
    public string Code { get; set; }

    /// <summary>
    /// Obtiene o establece el código de estado HTTP.
    /// </summary>
    /// <remarks>
    /// Este código puede ser nulo, lo que indica que no se ha definido un código de estado.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el código de estado HTTP, o null si no se ha definido.
    /// </value>
    public int? HttpStatusCode { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si la localización está habilitada.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que la localización no ha sido especificada.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de la localización. 
    /// Puede ser <c>true</c> si la localización está habilitada, 
    /// <c>false</c> si no lo está, o <c>null</c> si no se ha especificado.
    /// </value>
    public bool? IsLocalization { get; set; }

    /// <summary>
    /// Obtiene un mensaje basado en el estado de producción.
    /// </summary>
    /// <param name="isProduction">Indica si el entorno es de producción.</param>
    /// <returns>
    /// Un mensaje en forma de cadena que puede variar según el estado de producción.
    /// </returns>
    public virtual string GetMessage( bool isProduction = false ) {
        return GetMessage( this );
    }

    /// <summary>
    /// Obtiene un mensaje concatenado de todas las excepciones en la cadena de excepciones.
    /// </summary>
    /// <param name="ex">La excepción de la cual se extraerán los mensajes.</param>
    /// <returns>Un string que contiene todos los mensajes de las excepciones, concatenados y sin espacios en blanco al inicio o al final.</returns>
    /// <remarks>
    /// Este método recorre todas las excepciones en la cadena de excepciones, 
    /// utilizando el método <see cref="GetExceptions(Exception)"/> para obtener la lista 
    /// de excepciones y el método <see cref="AppendMessage(StringBuilder, Exception)"/> 
    /// para añadir cada mensaje al resultado.
    /// </remarks>
    public static string GetMessage( Exception ex ) {
        var result = new StringBuilder();
        var list = GetExceptions( ex );
        foreach( var exception in list )
            AppendMessage( result, exception );
        return result.ToString().Trim( Environment.NewLine.ToCharArray() );
    }

    /// <summary>
    /// Agrega el mensaje de una excepción al objeto StringBuilder proporcionado.
    /// </summary>
    /// <param name="result">El objeto StringBuilder al que se le agregará el mensaje de la excepción.</param>
    /// <param name="exception">La excepción cuyo mensaje se desea agregar. Si es null, no se realiza ninguna acción.</param>
    private static void AppendMessage( StringBuilder result, Exception exception ) {
        if( exception == null )
            return;
        result.AppendLine( exception.Message );
    }

    /// <summary>
    /// Obtiene una lista de excepciones asociadas con el objeto actual.
    /// </summary>
    /// <returns>
    /// Una lista que contiene las excepciones encontradas.
    /// </returns>
    /// <remarks>
    /// Este método llama a otra sobrecarga de <see cref="GetExceptions(object)"/> 
    /// para obtener las excepciones específicas del objeto actual.
    /// </remarks>
    public IList<Exception> GetExceptions() {
        return GetExceptions( this );
    }

    /// <summary>
    /// Obtiene una lista de excepciones a partir de una excepción dada.
    /// </summary>
    /// <param name="ex">La excepción de la cual se desea obtener la lista de excepciones.</param>
    /// <returns>Una lista de excepciones que contiene la excepción original y sus excepciones internas.</returns>
    /// <remarks>
    /// Este método recorre la cadena de excepciones, añadiendo cada excepción a la lista resultante.
    /// </remarks>
    public static IList<Exception> GetExceptions( Exception ex ) {
        var result = new List<Exception>();
        AddException( result, ex );
        return result;
    }

    /// <summary>
    /// Agrega una excepción y sus excepciones internas a una lista de excepciones.
    /// </summary>
    /// <param name="result">La lista donde se agregarán las excepciones.</param>
    /// <param name="exception">La excepción que se va a agregar a la lista.</param>
    /// <remarks>
    /// Este método verifica si la excepción proporcionada es nula. Si no lo es, 
    /// la agrega a la lista y luego llama recursivamente a sí mismo para agregar 
    /// cualquier excepción interna asociada.
    /// </remarks>
    private static void AddException( List<Exception> result, Exception exception ) {
        if( exception == null )
            return;
        result.Add( exception );
        AddException( result, exception.InnerException );
    }
}