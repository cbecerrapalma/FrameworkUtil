namespace Util.Microservices.Dapr.ServiceInvocations;

/// <summary>
/// Clase base abstracta para la invocación de servicios Dapr.
/// </summary>
/// <typeparam name="TServiceInvocation">El tipo de invocación de servicio que hereda de <see cref="IServiceInvocationBase{TServiceInvocation}"/>.</typeparam>
public abstract class DaprServiceInvocationBase<TServiceInvocation> : IServiceInvocationBase<TServiceInvocation> where TServiceInvocation : IServiceInvocationBase<TServiceInvocation> {
    /// <summary>
    /// Obtiene el cliente Dapr utilizado para realizar llamadas a los servicios Dapr.
    /// </summary>
    /// <remarks>
    /// Este cliente permite interactuar con el runtime de Dapr, facilitando la comunicación entre servicios
    /// y el uso de características como el estado, la pub/sub y las invocaciones de servicios.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="DaprClient"/> que representa el cliente Dapr.
    /// </value>
    protected DaprClient Client { get; }
    /// <summary>
    /// Obtiene las opciones de configuración de Dapr.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="DaprOptions"/> que contiene las opciones de Dapr.
    /// </value>
    protected DaprOptions Options { get; }
    /// <summary>
    /// Obtiene o establece el registrador de logs utilizado para registrar información, advertencias y errores.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite a las clases que la implementan registrar mensajes en un sistema de logging,
    /// facilitando la depuración y el monitoreo de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="ILogger"/>.
    /// </value>
    protected ILogger Log { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador de la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es utilizada para almacenar el ID único de la aplicación,
    /// que puede ser utilizado para la autenticación o la identificación en servicios externos.
    /// </remarks>
    protected string AppId { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el resultado de la descompresión es exitoso.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar el estado de la operación de descompresión.
    /// Un valor verdadero indica que la descompresión se realizó correctamente, 
    /// mientras que un valor falso indica que hubo un error en el proceso.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el resultado de la descompresión es exitoso; de lo contrario, <c>false</c>.
    /// </value>
    protected bool IsUnpackResult { get; set; }
    /// <summary>
    /// Obtiene o establece un diccionario que contiene los encabezados.
    /// </summary>
    /// <remarks>
    /// Este diccionario almacena pares clave-valor donde la clave es el nombre del encabezado 
    /// y el valor es el contenido del encabezado. Es útil para gestionar información adicional 
    /// que se puede enviar o recibir en una solicitud o respuesta.
    /// </remarks>
    /// <value>
    /// Un diccionario de tipo <see cref="IDictionary{TKey,TValue}"/> que asocia cadenas a cadenas.
    /// </value>
    protected IDictionary<string, string> Headers { get; set; }
    /// <summary>
    /// Obtiene o establece una lista de claves de encabezado que se importan.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena las claves de encabezado que se utilizan durante el proceso de importación.
    /// Las claves son representadas como una lista de cadenas, permitiendo un acceso fácil y manipulación de los mismos.
    /// </remarks>
    /// <value>
    /// Una lista de cadenas que representa las claves de encabezado.
    /// </value>
    protected List<string> ImportHeaderKeys { get; set; }
    /// <summary>
    /// Obtiene o establece una lista de claves de encabezado que deben ser eliminadas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite especificar las claves de encabezado que no se deben considerar 
    /// durante el procesamiento de datos. Es útil para filtrar información no deseada 
    /// antes de realizar operaciones adicionales.
    /// </remarks>
    /// <value>
    /// Una lista de cadenas que representa las claves de encabezado a eliminar.
    /// </value>
    protected IList<string> RemoveHeaderKeys { get; set; }
    /// <summary>
    /// Obtiene o establece una función que toma una cadena como entrada y devuelve un estado de servicio.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una acción personalizada que se ejecutará para determinar el estado del servicio
    /// basado en una entrada de tipo cadena. La función debe devolver un valor de tipo <see cref="ServiceState"/>.
    /// </remarks>
    /// <value>
    /// Una función que recibe un <see cref="string"/> y devuelve un <see cref="ServiceState"/>.
    /// </value>
    protected Func<string, ServiceState> OnStateAction { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprServiceInvocationBase"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr que se utilizará para las invocaciones de servicio.</param>
    /// <param name="options">Las opciones de configuración de Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros para crear instancias de <see cref="ILogger"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="client"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este constructor configura el cliente Dapr, las opciones y el logger, además de inicializar
    /// los diccionarios y listas necesarios para la invocación de servicios.
    /// </remarks>
    protected DaprServiceInvocationBase(DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        Options = options?.Value ?? new DaprOptions();
        Log = loggerFactory?.CreateLogger(typeof(IServiceInvocation)) ?? NullLogger.Instance;
        Headers = new Dictionary<string, string>();
        ImportHeaderKeys = new List<string>();
        RemoveHeaderKeys = new List<string>();
        IsUnpackResult = Options.ServiceInvocation.IsUnpackResult;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el identificador de la aplicación y devuelve una invocación de servicio.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que se va a establecer.</param>
    /// <returns>Una instancia de <typeparamref name="TServiceInvocation"/> que representa la invocación del servicio.</returns>
    /// <remarks>
    /// Este método asigna el valor del parámetro <paramref name="appId"/> a la propiedad <c>AppId</c>
    /// y luego llama al método <c>Return()</c> para obtener la invocación del servicio.
    /// </remarks>
    /// <typeparam name="TServiceInvocation">El tipo de la invocación de servicio que se devuelve.</typeparam>
    public TServiceInvocation Service( string appId ) {
        AppId = appId;
        return Return();
    }

    /// <summary>
    /// Devuelve una instancia del tipo <typeparamref name="TServiceInvocation"/> 
    /// que representa el objeto actual.
    /// </summary>
    /// <returns>
    /// Una instancia de <typeparamref name="TServiceInvocation"/> 
    /// que es una conversión del objeto actual.
    /// </returns>
    /// <typeparam name="TServiceInvocation">
    /// El tipo que se espera que sea devuelto por este método.
    /// </typeparam>
    private TServiceInvocation Return() {
        return (TServiceInvocation)(object)this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Desempaqueta el resultado de una invocación de servicio.
    /// </summary>
    /// <param name="isUnpack">Indica si se debe desempaquetar el resultado.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TServiceInvocation"/> que representa el resultado desempaquetado.</returns>
    /// <remarks>
    /// Este método establece el estado de desempaquetado y luego devuelve el resultado de la invocación.
    /// </remarks>
    public TServiceInvocation UnpackResult( bool isUnpack ) {
        IsUnpackResult = isUnpack;
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el token de autorización en el encabezado de la solicitud.
    /// </summary>
    /// <param name="token">El token de autorización que se utilizará en el encabezado.</param>
    /// <returns>Una instancia de <typeparamref name="TServiceInvocation"/> que permite encadenar llamadas.</returns>
    /// <remarks>
    /// Este método agrega un encabezado "Authorization" con el formato "Bearer {token}" a la solicitud.
    /// Asegúrese de que el token proporcionado sea válido y esté correctamente formateado.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TServiceInvocation BearerToken( string token ) {
        Header( "Authorization",$"Bearer {token}" );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece un encabezado con la clave y el valor especificados.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a establecer.</param>
    /// <param name="value">El valor del encabezado que se va a establecer.</param>
    /// <returns>Una instancia del servicio de invocación.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. Si la clave ya existe, se elimina antes de agregar el nuevo valor.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TServiceInvocation Header( string key, string value ) {
        if ( key.IsEmpty() )
            return Return();
        if ( Headers.ContainsKey( key ) )
            Headers.Remove( key );
        Headers.Add( key, value );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los encabezados para la invocación del servicio.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a establecer, donde la clave es el nombre del encabezado y el valor es su correspondiente valor.</param>
    /// <returns>Una instancia del tipo <typeparamref name="TServiceInvocation"/> que permite encadenar llamadas.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="headers"/> es nulo, no se realizará ninguna acción y se devolverá la instancia actual.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TServiceInvocation Header( IDictionary<string, string> headers ) {
        if ( headers == null )
            return Return();
        foreach ( var header in headers )
            Header( header.Key, header.Value );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Importa un encabezado utilizando la clave proporcionada.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea importar.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TServiceInvocation"/> que representa el resultado de la operación.</returns>
    /// <remarks>
    /// Este método verifica si la clave está vacía. Si es así, se devuelve el resultado inmediatamente.
    /// Si la clave no está vacía y no está presente en la colección <c>ImportHeaderKeys</c>, se agrega a la colección.
    /// Finalmente, se devuelve el resultado de la operación.
    /// </remarks>
    /// <typeparam name="TServiceInvocation">El tipo de objeto que se devuelve al finalizar la operación.</typeparam>
    public TServiceInvocation ImportHeader( string key ) {
        if ( key.IsEmpty() )
            return Return();
        if( ImportHeaderKeys.Contains( key ) == false )
            ImportHeaderKeys.Add( key );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Importa los encabezados a partir de una colección de claves.
    /// </summary>
    /// <param name="keys">Una colección de claves que se utilizarán para importar los encabezados.</param>
    /// <returns>Una instancia de <typeparamref name="TServiceInvocation"/> que representa el resultado de la operación.</returns>
    /// <remarks>
    /// Si la colección de claves es nula, se devolverá el resultado por defecto.
    /// Para cada clave en la colección, se invocará el método <see cref="ImportHeader(string)"/>.
    /// </remarks>
    /// <seealso cref="ImportHeader(string)"/>
    public TServiceInvocation ImportHeader( IEnumerable<string> keys ) {
        if ( keys == null )
            return Return();
        foreach ( var key in keys )
            ImportHeader( key );
        return Return();
    }

    /// <inheritdoc />
    /// <summary>
    /// Elimina un encabezado de la lista de claves de encabezado a eliminar.
    /// </summary>
    /// <param name="key">La clave del encabezado que se desea eliminar.</param>
    /// <returns>Una instancia de <typeparamref name="TServiceInvocation"/> que representa el estado actual.</returns>
    /// <remarks>
    /// Si la clave proporcionada está vacía, no se realiza ninguna acción.
    /// Si la clave no está en la lista de claves a eliminar, se agrega a la lista.
    /// </remarks>
    /// <seealso cref="Return"/>
    public TServiceInvocation RemoveHeader( string key ) {
        if ( key.IsEmpty() )
            return Return();
        if ( RemoveHeaderKeys.Contains( key ) == false )
            RemoveHeaderKeys.Add( key );
        return Return();
    }

    /// <summary>
    /// Establece una acción que se ejecutará cuando cambie el estado del servicio.
    /// </summary>
    /// <param name="action">Una función que toma una cadena y devuelve un estado de servicio.</param>
    /// <returns>Una instancia del tipo <typeparamref name="TServiceInvocation"/>.</returns>
    /// <remarks>
    /// Esta función permite definir cómo se debe manejar el cambio de estado del servicio,
    /// proporcionando una lógica personalizada a través de la función proporcionada.
    /// </remarks>
    /// <typeparam name="TServiceInvocation">El tipo de la instancia que se devuelve.</typeparam>
    public TServiceInvocation OnState( Func<string, ServiceState> action ) {
        OnStateAction = action;
        return Return();
    }
}