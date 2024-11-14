using Util.Helpers;

namespace Util.Http;

/// <summary>
/// Representa una solicitud HTTP genérica que puede devolver un resultado de tipo especificado.
/// </summary>
/// <typeparam name="TResult">El tipo de resultado que se espera de la solicitud HTTP. Debe ser una clase.</typeparam>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IHttpRequest{TResult}"/> y proporciona métodos para realizar solicitudes HTTP
/// y manejar las respuestas de manera tipada.
/// </remarks>
public class HttpRequest<TResult> : IHttpRequest<TResult> where TResult : class
{

    #region Campo

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private string _httpClientName;
    private readonly HttpMethod _httpMethod;
    private readonly string _url;
    private JsonSerializerOptions _jsonSerializerOptions;
    private bool _ignoreSsl;
    private HttpCompletionOption _completionOption;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="httpClientFactory">La fábrica de clientes HTTP utilizada para crear instancias de <see cref="HttpClient"/>.</param>
    /// <param name="httpClient">Una instancia de <see cref="HttpClient"/> que se utilizará para realizar la solicitud.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la solicitud, como GET, POST, etc.</param>
    /// <param name="url">La URL a la que se enviará la solicitud.</param>
    /// <exception cref="ArgumentNullException">Se lanza si tanto <paramref name="httpClientFactory"/> como <paramref name="httpClient"/> son nulos.</exception>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="url"/> está vacío.</exception>
    /// <remarks>
    /// Este constructor configura los parámetros de la solicitud HTTP, incluyendo encabezados, parámetros de consulta, 
    /// parámetros generales, archivos, cookies y opciones de contenido HTTP.
    /// </remarks>
    public HttpRequest(IHttpClientFactory httpClientFactory, HttpClient httpClient, HttpMethod httpMethod, string url)
    {
        if (httpClientFactory == null && httpClient == null)
            throw new ArgumentNullException(nameof(httpClientFactory));
        if (url.IsEmpty())
            throw new ArgumentNullException(nameof(url));
        _httpClientFactory = httpClientFactory;
        _httpClient = httpClient;
        _httpMethod = httpMethod;
        _url = url;
        HeaderParams = new Dictionary<string, string>();
        QueryParams = new Dictionary<string, string>();
        Params = new Dictionary<string, object>();
        Files = new List<FileData>();
        Cookies = new Dictionary<string, string>();
        HttpContentType = Util.Http.HttpContentType.Json.Description();
        CharacterEncoding = System.Text.Encoding.UTF8;
        IsUseCookies = true;
        IsFileParameterQuotes = true;
        _completionOption = HttpCompletionOption.ResponseContentRead;
    }

    #endregion

    #region atributo

    /// <summary>
    /// Obtiene o establece el tiempo de espera para las solicitudes HTTP.
    /// </summary>
    /// <remarks>
    /// Este valor se utiliza para determinar cuánto tiempo esperar antes de que una solicitud HTTP se considere fallida.
    /// Si se establece en <c>null</c>, se utilizará el tiempo de espera predeterminado del cliente HTTP.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TimeSpan"/> que representa el tiempo de espera, o <c>null</c> si no se ha establecido.
    /// </value>
    protected TimeSpan? HttpTimeout { get; private set; }
    /// <summary>
    /// Obtiene la dirección base del URI.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la dirección base que se utiliza para construir las solicitudes de red.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la dirección base del URI.
    /// </value>
    protected string BaseAddressUri { get; private set; }
    /// <summary>
    /// Obtiene la ruta del certificado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la ubicación del archivo del certificado utilizado en la aplicación.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta del certificado.
    /// </value>
    protected string CertificatePath { get; private set; }
    /// <summary>
    /// Obtiene la contraseña del certificado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura desde fuera de la clase, 
    /// lo que significa que solo puede ser establecida dentro de la clase.
    /// </remarks>
    /// <value>
    /// La contraseña del certificado como una cadena de caracteres.
    /// </value>
    protected string CertificatePassword { get; private set; }
    /// <summary>
    /// Obtiene el tipo de contenido HTTP asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena el tipo de contenido que se utilizará en las solicitudes HTTP.
    /// El tipo de contenido es un valor que indica la naturaleza del contenido que se está enviando o recibiendo.
    /// </remarks>
    /// <value>
    /// Un string que representa el tipo de contenido HTTP, como "application/json" o "text/html".
    /// </value>
    protected string HttpContentType { get; private set; }
    /// <summary>
    /// Obtiene la codificación de caracteres utilizada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a la codificación de caracteres que se utiliza para la lectura o escritura de datos.
    /// La propiedad es de solo lectura desde fuera de la clase, lo que significa que solo puede ser establecida dentro de la misma.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="Encoding"/> que representa la codificación de caracteres.
    /// </value>
    protected Encoding CharacterEncoding { get; private set; }
    /// <summary>
    /// Obtiene los parámetros de encabezado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un diccionario que contiene pares clave-valor,
    /// donde la clave es una cadena que representa el nombre del parámetro de encabezado
    /// y el valor es una cadena que representa el valor correspondiente.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey, TValue}"/> que contiene los parámetros de encabezado.
    /// </returns>
    protected IDictionary<string, string> HeaderParams { get; }
    /// <summary>
    /// Obtiene un diccionario que contiene las cookies.
    /// </summary>
    /// <remarks>
    /// Este diccionario almacena pares clave-valor donde la clave es el nombre de la cookie 
    /// y el valor es el contenido de la cookie. Las cookies se utilizan comúnmente para 
    /// mantener el estado de la sesión del usuario y almacenar información relevante 
    /// entre solicitudes.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey, TValue}"/> que representa las cookies.
    /// </returns>
    protected IDictionary<string, string> Cookies { get; }
    /// <summary>
    /// Obtiene un diccionario que contiene los parámetros de consulta.
    /// </summary>
    /// <remarks>
    /// Este diccionario almacena pares clave-valor, donde la clave es el nombre del parámetro de consulta
    /// y el valor es el valor asociado a ese parámetro. Es útil para construir cadenas de consulta
    /// en solicitudes HTTP o para procesar parámetros de consulta en una aplicación.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey,TValue}"/> que representa los parámetros de consulta.
    /// </returns>
    protected IDictionary<string, string> QueryParams { get; }
    /// <summary>
    /// Obtiene un diccionario que contiene parámetros clave-valor.
    /// </summary>
    /// <remarks>
    /// Este diccionario permite almacenar y acceder a parámetros de manera dinámica,
    /// facilitando la gestión de datos en tiempo de ejecución.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey, TValue}"/> donde la clave es de tipo <see cref="string"/>
    /// y el valor es de tipo <see cref="object"/>.
    /// </returns>
    protected IDictionary<string, object> Params { get; }
    /// <summary>
    /// Obtiene el valor del parámetro.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura desde fuera de la clase, lo que significa que su valor solo puede ser establecido dentro de la clase.
    /// </remarks>
    /// <value>
    /// Un objeto que representa el parámetro.
    /// </value>
    protected object Param { get; private set; }
    /// <summary>
    /// Obtiene la lista de archivos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una colección de objetos <see cref="FileData"/> que representan los archivos.
    /// La lista es de solo lectura desde fuera de la clase, ya que el setter es privado.
    /// </remarks>
    /// <value>
    /// Una lista de tipo <see cref="List{FileData}"/> que contiene los datos de los archivos.
    /// </value>
    protected List<FileData> Files { get; private set; }
    /// <summary>
    /// Obtiene un valor que indica si se deben utilizar cookies.
    /// </summary>
    /// <value>
    /// <c>true</c> si se utilizan cookies; de lo contrario, <c>false</c>.
    /// </value>
    protected bool IsUseCookies { get; private set; }
    /// <summary>
    /// Indica si el parámetro de archivo está entre comillas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar si el valor del parámetro de archivo 
    /// debe ser tratado como una cadena entre comillas, lo cual puede ser necesario 
    /// para manejar correctamente rutas de archivo que contienen espacios u otros 
    /// caracteres especiales.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el parámetro de archivo está entre comillas; de lo contrario, <c>false</c>.
    /// </value>
    protected bool IsFileParameterQuotes { get; private set; }
    /// <summary>
    /// Obtiene o establece una función que se ejecuta antes de enviar un mensaje de solicitud HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una acción personalizada que se ejecutará antes de realizar la solicitud.
    /// La función debe aceptar un objeto de tipo <see cref="HttpRequestMessage"/> y devolver un valor booleano.
    /// </remarks>
    /// <value>
    /// Una función que toma un <see cref="HttpRequestMessage"/> y devuelve un valor booleano.
    /// </value>
    protected Func<HttpRequestMessage, bool> SendBeforeAction { get; private set; }
    /// <summary>
    /// Obtiene o establece una función que se ejecutará después de enviar una solicitud HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una acción personalizada que se ejecutará sobre la respuesta de la solicitud HTTP.
    /// La función debe aceptar un objeto <see cref="HttpResponseMessage"/> y devolver un <see cref="Task{TResult}"/>.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la función después de procesar la respuesta.</typeparam>
    protected Func<HttpResponseMessage, Task<TResult>> SendAfterAction { get; private set; }
    /// <summary>
    /// Obtiene o establece una función que convierte un string en un resultado de tipo TResult.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite asignar una función que toma un string como entrada y devuelve un resultado de tipo TResult.
    /// Es útil para realizar conversiones o transformaciones de datos en el contexto de la aplicación.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la función de conversión.</typeparam>
    protected Func<string, TResult> ConvertAction { get; private set; }
    /// <summary>
    /// Obtiene la acción que se ejecutará al completar exitosamente una operación.
    /// </summary>
    /// <value>
    /// Una acción que toma un parámetro de tipo <typeparamref name="TResult"/> y no devuelve ningún valor.
    /// </value>
    protected Action<TResult> SuccessAction { get; private set; }
    /// <summary>
    /// Representa una función que se ejecuta cuando una operación se completa con éxito.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que devuelve la función.</typeparam>
    /// <remarks>
    /// Esta propiedad permite asignar una función que toma un resultado de tipo <typeparamref name="TResult"/> 
    /// y devuelve una tarea que representa la operación asíncrona.
    /// </remarks>
    protected Func<TResult, Task> SuccessFunc { get; private set; }
    /// <summary>
    /// Representa una acción que se ejecuta cuando se produce un fallo en la respuesta HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una acción personalizada que se ejecutará al manejar un error en la respuesta.
    /// </remarks>
    /// <value>
    /// Una acción que toma un <see cref="HttpResponseMessage"/> y un objeto adicional como parámetros.
    /// </value>
    protected Action<HttpResponseMessage, object> FailAction { get; private set; }
    /// <summary>
    /// Representa una acción que se ejecuta al completar una operación, 
    /// la cual recibe un objeto <see cref="HttpResponseMessage"/> y un objeto adicional.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una lógica personalizada que se ejecutará 
    /// al finalizar una acción, facilitando la manipulación de la respuesta 
    /// y otros datos relacionados.
    /// </remarks>
    /// <value>
    /// Una acción que toma un <see cref="HttpResponseMessage"/> y un objeto adicional.
    /// </value>
    protected Action<HttpResponseMessage, object> CompleteAction { get; private set; }

    #endregion

    #region Timeout(Configurar el intervalo de tiempo de espera.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el tiempo de espera para la solicitud HTTP.
    /// </summary>
    /// <param name="timeout">El tiempo de espera que se aplicará a la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> con el tiempo de espera configurado.</returns>
    /// <remarks>
    /// Este método permite definir un límite de tiempo para la ejecución de la solicitud.
    /// Si la solicitud no se completa dentro del tiempo especificado, se generará una excepción.
    /// </remarks>
    public IHttpRequest<TResult> Timeout(TimeSpan timeout)
    {
        HttpTimeout = timeout;
        return this;
    }

    #endregion

    #region HttpClientName(Configurar el nombre del cliente Http.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el nombre del cliente HTTP.
    /// </summary>
    /// <param name="name">El nombre del cliente HTTP que se va a establecer.</param>
    /// <returns>Una instancia del objeto actual <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite configurar el nombre del cliente HTTP que se utilizará en las solicitudes subsiguientes.
    /// </remarks>
    public IHttpRequest<TResult> HttpClientName(string name)
    {
        _httpClientName = name;
        return this;
    }

    #endregion

    #region BaseAddress(Establecer la dirección base.)

    /// <summary>
    /// Establece la dirección base para las solicitudes HTTP.
    /// </summary>
    /// <param name="baseAddress">La dirección base que se utilizará para las solicitudes.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    public IHttpRequest<TResult> BaseAddress(string baseAddress)
    {
        BaseAddressUri = baseAddress;
        return this;
    }

    #endregion

    #region ContentType(Configurar el tipo de contenido.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de contenido para la solicitud HTTP.
    /// </summary>
    /// <param name="contentType">El tipo de contenido que se desea establecer.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el tipo de contenido configurado.</returns>
    /// <remarks>
    /// Este método utiliza la descripción del tipo de contenido proporcionado por el parámetro <paramref name="contentType"/> 
    /// para configurar la solicitud HTTP. Asegúrese de que el tipo de contenido sea compatible con el servidor 
    /// al que se está realizando la solicitud.
    /// </remarks>
    /// <seealso cref="HttpContentType"/>
    public IHttpRequest<TResult> ContentType(HttpContentType contentType)
    {
        return ContentType(contentType.Description());
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el tipo de contenido para la solicitud HTTP.
    /// </summary>
    /// <param name="contentType">El tipo de contenido que se va a establecer, por ejemplo, "application/json".</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el tipo de contenido actualizado.</returns>
    public IHttpRequest<TResult> ContentType(string contentType)
    {
        HttpContentType = contentType;
        return this;
    }

    #endregion

    #region HttpCompletion(Configurar el modo de finalización de respuesta.)

    /// <inheritdoc />
    /// <summary>
    /// Establece la opción de finalización para la solicitud HTTP.
    /// </summary>
    /// <param name="option">La opción de finalización que se aplicará a la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la opción de finalización configurada.</returns>
    /// <remarks>
    /// Este método permite especificar cómo se debe manejar la finalización de la solicitud HTTP,
    /// lo que puede afectar el comportamiento de la misma en función de la opción seleccionada.
    /// </remarks>
    public IHttpRequest<TResult> HttpCompletion(HttpCompletionOption option)
    {
        _completionOption = option;
        return this;
    }

    #endregion

    #region Encoding(Configurar la codificación de caracteres)

    /// <inheritdoc />
    /// <summary>
    /// Establece la codificación para la solicitud HTTP utilizando el nombre de la codificación especificada.
    /// </summary>
    /// <param name="encoding">El nombre de la codificación que se desea utilizar.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la codificación establecida.</returns>
    /// <exception cref="System.ArgumentException">Se lanza si el nombre de la codificación no es válido.</exception>
    /// <seealso cref="Encoding(System.Text.Encoding)"/>
    public IHttpRequest<TResult> Encoding(string encoding)
    {
        return Encoding(System.Text.Encoding.GetEncoding(encoding));
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece la codificación de la solicitud HTTP.
    /// </summary>
    /// <param name="encoding">La codificación que se aplicará a la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> con la codificación actualizada.</returns>
    public IHttpRequest<TResult> Encoding(Encoding encoding)
    {
        CharacterEncoding = encoding;
        return this;
    }

    #endregion

    #region BearerToken(Configurar el token de acceso.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el token de autorización en la cabecera de la solicitud HTTP.
    /// </summary>
    /// <param name="token">El token de acceso que se utilizará para la autorización.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método agrega un encabezado "Authorization" con el formato "Bearer {token}" 
    /// a la solicitud HTTP, lo que permite la autenticación basada en tokens.
    /// </remarks>
    public IHttpRequest<TResult> BearerToken(string token)
    {
        Header("Authorization", $"Bearer {token}");
        return this;
    }

    #endregion

    #region Certificate(Configurar certificado)

    /// <inheritdoc />
    /// <summary>
    /// Establece la ruta y la contraseña del certificado para la solicitud HTTP.
    /// </summary>
    /// <param name="path">La ruta del certificado que se utilizará en la solicitud.</param>
    /// <param name="password">La contraseña del certificado.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método es útil para configurar la autenticación basada en certificados en solicitudes HTTP.
    /// </remarks>
    public IHttpRequest<TResult> Certificate(string path, string password)
    {
        CertificatePath = path;
        CertificatePassword = password;
        return this;
    }

    #endregion

    #region IgnoreSsl(¿Se debe ignorar el certificado SSL?)

    /// <inheritdoc />
    /// <summary>
    /// Ignora la validación del certificado SSL para las solicitudes HTTP.
    /// </summary>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/> 
    /// con la configuración de ignorar SSL aplicada.
    /// </returns>
    /// <remarks>
    /// Este método es útil en entornos de desarrollo o pruebas donde se utilizan 
    /// certificados SSL autofirmados. No se recomienda su uso en producción debido 
    /// a implicaciones de seguridad.
    /// </remarks>
    public IHttpRequest<TResult> IgnoreSsl()
    {
        _ignoreSsl = true;
        return this;
    }

    #endregion

    #region JsonSerializerOptions(Configurar la configuración de serialización Json.)

    /// <inheritdoc />
    /// <summary>
    /// Establece las opciones de serialización JSON para la solicitud HTTP.
    /// </summary>
    /// <param name="options">Las opciones de serialización JSON que se aplicarán a la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite configurar cómo se serializarán los objetos a JSON al realizar la solicitud.
    /// </remarks>
    public IHttpRequest<TResult> JsonSerializerOptions(JsonSerializerOptions options)
    {
        _jsonSerializerOptions = options;
        return this;
    }

    #endregion

    #region GetJsonSerializerOptions(Obtener la configuración de serialización JSON.)

    /// <summary>
    /// Obtiene las opciones de configuración para el serializador JSON.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> que contiene las configuraciones para la serialización JSON.
    /// </returns>
    /// <remarks>
    /// Si las opciones de serialización JSON ya han sido configuradas, se devolverán esas opciones.
    /// De lo contrario, se crearán nuevas opciones con configuraciones predeterminadas que incluyen:
    /// <list type="bullet">
    /// <item>
    /// <description>Ignorar propiedades con valores nulos durante la serialización.</description>
    /// </item>
    /// <item>
    /// <description>Permitir la lectura de números desde cadenas.</description>
    /// </item>
    /// <item>
    /// <description>Configurar el codificador para permitir todos los rangos Unicode.</description>
    /// </item>
    /// <item>
    /// <description>Incluir convertidores personalizados para <see cref="DateTime"/> y <see cref="Nullable{DateTime}"/>.</description>
    /// </item>
    /// </list>
    /// </remarks>
    protected virtual JsonSerializerOptions GetJsonSerializerOptions()
    {
        if (_jsonSerializerOptions != null)
            return _jsonSerializerOptions;
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter()
            }
        };
    }

    #endregion

    #region Header(Configurar el encabezado de la solicitud.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega un encabezado a la solicitud HTTP.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a agregar.</param>
    /// <param name="value">El valor del encabezado que se va a agregar.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. Si el encabezado ya existe, se elimina antes de agregar el nuevo valor.
    /// </remarks>
    public IHttpRequest<TResult> Header(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (HeaderParams.ContainsKey(key))
            HeaderParams.Remove(key);
        HeaderParams.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los encabezados para la solicitud HTTP.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a establecer, donde la clave es el nombre del encabezado y el valor es su valor correspondiente.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="headers"/> es nulo, la función no realiza ninguna acción y devuelve la instancia actual.
    /// </remarks>
    public IHttpRequest<TResult> Header(IDictionary<string, string> headers)
    {
        if (headers == null)
            return this;
        foreach (var header in headers)
            Header(header.Key, header.Value);
        return this;
    }

    #endregion

    #region QueryString(Configurar la cadena de consulta.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega o actualiza un parámetro en la cadena de consulta.
    /// </summary>
    /// <param name="key">La clave del parámetro que se va a agregar o actualizar.</param>
    /// <param name="value">El valor del parámetro que se va a agregar o actualizar.</param>
    /// <returns>Una instancia del objeto actual con el parámetro de consulta actualizado.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realiza ninguna acción. Si la clave ya existe, se elimina antes de agregar el nuevo valor.
    /// </remarks>
    public IHttpRequest<TResult> QueryString(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (QueryParams.ContainsKey(key))
            QueryParams.Remove(key);
        QueryParams.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece los parámetros de consulta para la solicitud HTTP.
    /// </summary>
    /// <param name="queryString">Un diccionario que contiene los pares clave-valor que representan los parámetros de consulta.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si el diccionario proporcionado es nulo, la función simplemente devuelve la instancia actual sin realizar ninguna modificación.
    /// Se itera sobre cada par clave-valor en el diccionario y se llama al método <see cref="QueryString(string, string)"/> para agregar cada parámetro de consulta.
    /// </remarks>
    public IHttpRequest<TResult> QueryString(IDictionary<string, string> queryString)
    {
        if (queryString == null)
            return this;
        foreach (var param in queryString)
            QueryString(param.Key, param.Value);
        return this;
    }

    /// <summary>
    /// Establece los parámetros de la cadena de consulta a partir de un objeto.
    /// </summary>
    /// <param name="queryString">El objeto que contiene los parámetros que se convertirán en una cadena de consulta.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/>.</returns>
    /// <remarks>
    /// Este método convierte el objeto proporcionado en un diccionario y agrega cada parámetro a la cadena de consulta.
    /// Cada propiedad del objeto se convierte en un parámetro de la cadena de consulta con su respectivo valor.
    /// </remarks>
    public IHttpRequest<TResult> QueryString(object queryString)
    {
        var dic = ToDictionary(queryString);
        foreach (var param in dic)
            QueryString(param.Key, param.Value.ToString());
        return this;
    }

    #endregion

    #region UseCookies(Configurar si se deben llevar cookies automáticamente.)

    /// <inheritdoc />
    /// <summary>
    /// Establece si se deben usar cookies en la solicitud HTTP.
    /// </summary>
    /// <param name="isUseCookies">Indica si se deben usar cookies. True para habilitar, false para deshabilitar.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/>.</returns>
    /// <remarks>
    /// Este método permite al usuario configurar el uso de cookies para la solicitud HTTP.
    /// </remarks>
    public IHttpRequest<TResult> UseCookies(bool isUseCookies)
    {
        IsUseCookies = isUseCookies;
        return this;
    }

    #endregion

    #region Cookie(Configurar cookies)

    /// <inheritdoc />
    /// <summary>
    /// Establece una cookie con la clave y el valor especificados.
    /// </summary>
    /// <param name="key">La clave de la cookie que se va a establecer.</param>
    /// <param name="value">El valor de la cookie que se va a establecer.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Si la clave está vacía, no se realizará ninguna acción. Si la cookie ya existe, se eliminará antes de agregar la nueva.
    /// </remarks>
    public IHttpRequest<TResult> Cookie(string key, string value)
    {
        if (key.IsEmpty())
            return this;
        if (Cookies.ContainsKey(key))
            Cookies.Remove(key);
        Cookies.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece las cookies en la solicitud HTTP.
    /// </summary>
    /// <param name="cookies">Un diccionario que contiene las cookies a establecer, donde la clave es el nombre de la cookie y el valor es su contenido.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="cookies"/> es nulo, no se realizan cambios y se devuelve la instancia actual.
    /// </remarks>
    public IHttpRequest<TResult> Cookie(IDictionary<string, string> cookies)
    {
        if (cookies == null)
            return this;
        foreach (var cookie in cookies)
            Cookie(cookie.Key, cookie.Value);
        return this;
    }

    #endregion

    #region Content(Agregar parámetro de contenido)

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido de la solicitud HTTP asociando un valor a una clave especificada.
    /// </summary>
    /// <param name="key">La clave que se utilizará para asociar el valor en la solicitud.</param>
    /// <param name="value">El valor que se desea asociar a la clave. Si es <c>null</c>, no se realizará ninguna acción.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Si la clave está vacía, o si el valor es <c>null</c>, el método no realizará ninguna modificación 
    /// en los parámetros de la solicitud. Si la clave ya existe en los parámetros, se eliminará antes de 
    /// agregar el nuevo valor.
    /// </remarks>
    public IHttpRequest<TResult> Content(string key, object value)
    {
        if (key.IsEmpty())
            return this;
        if (value == null)
            return this;
        if (Params.ContainsKey(key))
            Params.Remove(key);
        Params.Add(key, value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido de la solicitud HTTP utilizando un diccionario de parámetros.
    /// </summary>
    /// <param name="parameters">Un diccionario que contiene los parámetros a establecer en el contenido de la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/>.</returns>
    /// <remarks>
    /// Si el diccionario de parámetros es nulo, se devuelve la instancia actual sin realizar cambios.
    /// Para cada par clave-valor en el diccionario, se llama al método <see cref="Content(string, object)"/>.
    /// </remarks>
    public IHttpRequest<TResult> Content(IDictionary<string, object> parameters)
    {
        if (parameters == null)
            return this;
        foreach (var param in parameters)
            Content(param.Key, param.Value);
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido de la solicitud HTTP.
    /// </summary>
    /// <param name="value">El objeto que se utilizará como contenido de la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    public IHttpRequest<TResult> Content(object value)
    {
        Param = value;
        return this;
    }

    #endregion

    #region JsonContent(Agregar parámetros JSON.)

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido de la solicitud HTTP como JSON.
    /// </summary>
    /// <param name="value">El objeto que se convertirá a JSON y se enviará como contenido de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el contenido JSON establecido.</returns>
    /// <remarks>
    /// Este método configura el tipo de contenido de la solicitud a "application/json"
    /// y convierte el objeto proporcionado a su representación JSON antes de enviarlo.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    public IHttpRequest<TResult> JsonContent(object value)
    {
        ContentType(Http.HttpContentType.Json);
        return Content(value);
    }

    #endregion

    #region XmlContent(Agregar parámetros Xml)

    /// <inheritdoc />
    /// <summary>
    /// Establece el contenido de la solicitud HTTP como XML.
    /// </summary>
    /// <param name="value">El contenido XML que se enviará en la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud con el contenido XML establecido.</returns>
    /// <remarks>
    /// Este método configura el tipo de contenido de la solicitud a XML y luego asigna el valor proporcionado como el contenido de la solicitud.
    /// </remarks>
    public IHttpRequest<TResult> XmlContent(string value)
    {
        ContentType(Http.HttpContentType.Xml);
        return Content(value);
    }

    #endregion

    #region FileContent(Agregar parámetros de archivo.)

    /// <inheritdoc />
    /// <summary>
    /// Agrega el contenido de un archivo a la solicitud HTTP.
    /// </summary>
    /// <param name="file">El flujo de datos del archivo que se desea agregar.</param>
    /// <param name="fileName">El nombre del archivo que se está agregando.</param>
    /// <param name="name">El nombre que se asignará al archivo en la solicitud. Por defecto es "file".</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método establece el tipo de contenido de la solicitud como "multipart/form-data" 
    /// y asegura que no haya archivos duplicados con el mismo nombre en la colección.
    /// Si se encuentra un archivo con el mismo nombre, se eliminará antes de agregar el nuevo archivo.
    /// </remarks>
    public IHttpRequest<TResult> FileContent(Stream file, string fileName, string name = "file")
    {
        ContentType(Http.HttpContentType.FormData);
        if (Files.Any(t => t.Name == name))
            Files.RemoveAll(t => t.Name == name);
        Files.Add(new FileData(file, fileName, name));
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Agrega el contenido de un archivo a la solicitud HTTP.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se desea agregar a la solicitud.</param>
    /// <param name="name">El nombre del archivo en la solicitud. Por defecto es "file".</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método establece el tipo de contenido de la solicitud como "multipart/form-data"
    /// y asegura que no haya archivos duplicados con el mismo nombre en la colección.
    /// Si ya existe un archivo con el mismo nombre, se eliminará antes de agregar el nuevo.
    /// </remarks>
    public IHttpRequest<TResult> FileContent(string filePath, string name = "file")
    {
        ContentType(Http.HttpContentType.FormData);
        if (Files.Any(t => t.Name == name))
            Files.RemoveAll(t => t.Name == name);
        Files.Add(new FileData(filePath, name));
        return this;
    }

    #endregion

    #region FileParameterQuotes(¿Se deben agregar comillas dobles a los parámetros de carga de archivos?)

    /// <inheritdoc />
    /// <summary>
    /// Establece si los parámetros de archivo deben estar entre comillas.
    /// </summary>
    /// <param name="isQuote">Indica si los parámetros de archivo deben estar entre comillas. El valor predeterminado es true.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/>.</returns>
    /// <remarks>
    /// Este método permite configurar el comportamiento de los parámetros de archivo en las solicitudes HTTP.
    /// </remarks>
    public IHttpRequest<TResult> FileParameterQuotes(bool isQuote = true)
    {
        IsFileParameterQuotes = isQuote;
        return this;
    }

    #endregion

    #region OnSendBefore(Enviar evento antes.)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará antes de enviar la solicitud HTTP.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y devuelve un valor booleano que indica si se debe continuar con el envío de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Esta función permite modificar o validar la solicitud antes de que se envíe. 
    /// Si la acción devuelve <c>false</c>, se puede optar por cancelar el envío de la solicitud.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    public IHttpRequest<TResult> OnSendBefore(Func<HttpRequestMessage, bool> action)
    {
        SendBeforeAction = action;
        return this;
    }

    #endregion

    #region OnSendAfter(Al enviar después)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará después de enviar la solicitud.
    /// </summary>
    /// <param name="action">La función que se ejecutará con la respuesta HTTP después de que se envíe la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir el encadenamiento de llamadas.</returns>
    /// <remarks>
    /// Esta función permite definir un comportamiento personalizado que se ejecutará una vez que se reciba la respuesta del servidor.
    /// </remarks>
    public IHttpRequest<TResult> OnSendAfter(Func<HttpResponseMessage, Task<TResult>> action)
    {
        SendAfterAction = action;
        return this;
    }

    #endregion

    #region OnConvert(Resultado de la conversión de eventos.)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción de conversión que se aplicará a la cadena de entrada.
    /// </summary>
    /// <param name="action">La función que define cómo convertir la cadena de entrada en un resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Esta función permite personalizar el proceso de conversión de la respuesta HTTP a un tipo específico.
    /// </remarks>
    public IHttpRequest<TResult> OnConvert(Func<string, TResult> action)
    {
        ConvertAction = action;
        return this;
    }

    #endregion

    #region OnSuccess(Evento de solicitud exitosa)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cuando la solicitud se complete con éxito.
    /// </summary>
    /// <param name="action">La acción que se ejecutará con el resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Esta función permite definir un comportamiento personalizado que se ejecutará al recibir una respuesta exitosa de la solicitud HTTP.
    /// </remarks>
    public IHttpRequest<TResult> OnSuccess(Action<TResult> action)
    {
        SuccessAction = action;
        return this;
    }

    /// <summary>
    /// Establece una acción que se ejecutará cuando la solicitud se complete con éxito.
    /// </summary>
    /// <param name="action">Una función asincrónica que recibe el resultado de tipo <typeparamref name="TResult"/> y no devuelve ningún valor.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IHttpRequest{TResult}"/>.</returns>
    public IHttpRequest<TResult> OnSuccess(Func<TResult, Task> action)
    {
        SuccessFunc = action;
        return this;
    }

    #endregion

    #region OnFail(Solicitud de fallo de evento.)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cuando la solicitud falle.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe un <see cref="HttpResponseMessage"/> y un objeto adicional.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Esta función permite manejar errores de manera personalizada al definir qué acción tomar cuando la solicitud no se completa con éxito.
    /// </remarks>
    public IHttpRequest<TResult> OnFail(Action<HttpResponseMessage, object> action)
    {
        FailAction = action;
        return this;
    }

    #endregion

    #region OnComplete(Solicitud de finalización de evento.)

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cuando la solicitud HTTP se complete.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe un <see cref="HttpResponseMessage"/> y un objeto adicional.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir el encadenamiento de llamadas.</returns>
    /// <remarks>
    /// Esta función permite definir un comportamiento personalizado que se ejecutará al finalizar la solicitud,
    /// lo que puede ser útil para manejar la respuesta o realizar acciones adicionales basadas en el resultado.
    /// </remarks>
    public IHttpRequest<TResult> OnComplete(Action<HttpResponseMessage, object> action)
    {
        CompleteAction = action;
        return this;
    }

    #endregion

    #region GetResultAsync(Obtener resultados)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el resultado de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TResult"/> que representa el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método crea un mensaje, lo envía y luego procesa la respuesta. 
    /// Si la operación de envío inicial no es exitosa, se devuelve el valor predeterminado de <typeparamref name="TResult"/>.
    /// </remarks>
    /// <seealso cref="CreateMessage"/>
    /// <seealso cref="SendAsync"/>
    /// <seealso cref="SendAfterAsync"/>
    public async Task<TResult> GetResultAsync(CancellationToken cancellationToken = default)
    {
        var message = await CreateMessage();
        if (SendBefore(message) == false)
            return default;
        var response = await SendAsync(message, cancellationToken);
        return await SendAfterAsync(response);
    }

    #endregion

    #region CreateMessage(Crear un mensaje de solicitud.)

    /// <summary>
    /// Crea un mensaje HTTP de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpRequestMessage"/> que representa el mensaje HTTP creado.
    /// </returns>
    /// <remarks>
    /// Este método configura el mensaje HTTP con el método especificado, la URL, 
    /// las cookies y los encabezados necesarios. Además, establece el contenido 
    /// del mensaje utilizando un método asíncrono para crear el contenido HTTP.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    /// <seealso cref="CreateHttpContent"/>
    /// <seealso cref="AddCookies"/>
    /// <seealso cref="AddHeaders"/>
    protected virtual async Task<HttpRequestMessage> CreateMessage()
    {
        var message = new HttpRequestMessage(_httpMethod, GetUrl(_url));
        AddCookies();
        AddHeaders(message);
        message.Content = await CreateHttpContent();
        return message;
    }

    /// <summary>
    /// Obtiene una URL con parámetros de consulta añadidos.
    /// </summary>
    /// <param name="url">La URL base a la que se le añadirán los parámetros de consulta.</param>
    /// <returns>
    /// La URL resultante con los parámetros de consulta añadidos.
    /// </returns>
    protected virtual string GetUrl(string url)
    {
        return QueryHelpers.AddQueryString(url, QueryParams);
    }

    /// <summary>
    /// Agrega las cookies al encabezado de la solicitud.
    /// </summary>
    /// <remarks>
    /// Este método verifica si hay cookies disponibles en la colección.
    /// Si hay cookies, las convierte en valores de encabezado de cookie y las agrega
    /// al encabezado de la solicitud.
    /// </remarks>
    protected virtual void AddCookies()
    {
        if (Cookies.Count == 0)
            return;
        var cookieValues = new List<CookieHeaderValue>();
        foreach (var cookie in Cookies)
            cookieValues.Add(new CookieHeaderValue(cookie.Key, cookie.Value));
        Header("Cookie", cookieValues.Select(t => t.ToString()).Join());
    }

    /// <summary>
    /// Agrega encabezados a la solicitud HTTP especificada.
    /// </summary>
    /// <param name="message">La instancia de <see cref="HttpRequestMessage"/> a la que se le agregarán los encabezados.</param>
    /// <remarks>
    /// Este método itera sobre un conjunto de parámetros de encabezado y los añade a la colección de encabezados de la solicitud.
    /// Asegúrese de que los encabezados no estén duplicados, ya que esto puede causar excepciones.
    /// </remarks>
    protected virtual void AddHeaders(HttpRequestMessage message)
    {
        foreach (var header in HeaderParams)
            message.Headers.Add(header.Key, header.Value);
    }

    /// <summary>
    /// Crea el contenido HTTP basado en el tipo de contenido especificado.
    /// </summary>
    /// <returns>
    /// Una tarea que representa el contenido HTTP creado. Puede ser null si el tipo de contenido no es reconocido.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito por clases derivadas para proporcionar una implementación personalizada.
    /// Los tipos de contenido soportados incluyen:
    /// - application/x-www-form-urlencoded
    /// - application/json
    /// - text/xml
    /// - multipart/form-data
    /// </remarks>
    /// <exception cref="System.Exception">
    /// Se puede lanzar una excepción si ocurre un error durante la creación del contenido.
    /// </exception>
    protected virtual async Task<HttpContent> CreateHttpContent()
    {
        var contentType = HttpContentType.SafeString().ToLower();
        switch (contentType)
        {
            case "application/x-www-form-urlencoded":
                return CreateFormContent();
            case "application/json":
                return CreateJsonContent();
            case "text/xml":
                return CreateXmlContent();
            case "multipart/form-data":
                return await CreateFileUploadContent();
        }
        return null;
    }

    /// <summary>
    /// Crea el contenido de un formulario codificado en URL para ser enviado en una solicitud HTTP.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpContent"/> que contiene los parámetros del formulario codificados en URL.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="GetParameters"/> para obtener los parámetros que se incluirán en el contenido del formulario.
    /// Cada parámetro se convierte a una cadena segura utilizando el método <see cref="SafeString"/>.
    /// </remarks>
    protected virtual HttpContent CreateFormContent()
    {
        return new FormUrlEncodedContent(GetParameters().ToDictionary(t => t.Key, t => t.Value.SafeString()));
    }

    /// <summary>
    /// Obtiene un diccionario de parámetros combinando los parámetros existentes con los parámetros convertidos de otro objeto.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene los parámetros combinados, donde la clave es de tipo <see cref="string"/> y el valor es de tipo <see cref="object"/>.
    /// </returns>
    /// <remarks>
    /// Este método crea un nuevo diccionario a partir de los parámetros existentes y luego intenta agregar parámetros de otro diccionario 
    /// que se obtiene mediante la conversión de un objeto. Si el diccionario convertido es nulo, se devuelve el diccionario original.
    /// Si una clave ya existe en el diccionario resultante, se omite su adición.
    /// </remarks>
    protected IDictionary<string, object> GetParameters()
    {
        var result = new Dictionary<string, object>(Params);
        var dictionary = ToDictionary(Param);
        if (dictionary == null)
            return result;
        foreach (var parameter in dictionary)
        {
            if (result.ContainsKey(parameter.Key))
                continue;
            result.Add(parameter.Key, parameter.Value);
        }
        return result;
    }

    /// <summary>
    /// Convierte un objeto en un diccionario de pares clave-valor.
    /// </summary>
    /// <param name="data">El objeto que se desea convertir a un diccionario.</param>
    /// <returns>
    /// Un diccionario que contiene los pares clave-valor del objeto proporcionado,
    /// excluyendo aquellos que tienen valores nulos.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un helper para realizar la conversión inicial y luego filtra
    /// los resultados para eliminar las entradas con valores nulos.
    /// </remarks>
    protected IDictionary<string, object> ToDictionary(object data)
    {
        var result = Util.Helpers.Convert.ToDictionary(data);
        return result.Where(t => t.Value != null).ToDictionary(t => t.Key, t => t.Value);
    }

    /// <summary>
    /// Crea el contenido JSON para ser utilizado en una solicitud HTTP.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpContent"/> que contiene el contenido JSON, 
    /// o <c>null</c> si el contenido está vacío.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada 
    /// para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual HttpContent CreateJsonContent()
    {
        var content = GetJsonContentValue();
        if (content.IsEmpty())
            return null;
        return new StringContent(content, CharacterEncoding, "application/json");
    }

    /// <summary>
    /// Obtiene el contenido en formato JSON basado en los parámetros disponibles.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido en formato JSON, o null si no hay parámetros disponibles.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay un parámetro único o una colección de parámetros y 
    /// serializa el contenido correspondiente a formato JSON utilizando las opciones de 
    /// serialización definidas por el método <see cref="GetJsonSerializerOptions"/>.
    /// </remarks>
    private string GetJsonContentValue()
    {
        var options = GetJsonSerializerOptions();
        if (Param != null && Params.Count > 0)
            return Json.ToJson(GetParameters(), options);
        if (Param != null)
            return Json.ToJson(Param, options);
        if (Params.Count > 0)
            return Json.ToJson(Params, options);
        return null;
    }

    /// <summary>
    /// Crea el contenido XML para ser utilizado en una solicitud HTTP.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpContent"/> que contiene el contenido XML.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la representación en cadena segura de los parámetros
    /// y establece la codificación de caracteres y el tipo de contenido apropiados.
    /// </remarks>
    protected virtual HttpContent CreateXmlContent()
    {
        return new StringContent(Param.SafeString(), CharacterEncoding, "text/xml");
    }

    /// <summary>
    /// Crea el contenido para la carga de archivos utilizando un formulario multipart.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado de la tarea es un 
    /// <see cref="HttpContent"/> que contiene los datos del archivo para la carga.
    /// </returns>
    /// <remarks>
    /// Este método se encarga de construir el contenido necesario para enviar archivos 
    /// a través de una solicitud HTTP. Se utiliza un <see cref="MultipartFormDataContent"/> 
    /// para encapsular los datos del archivo y otros parámetros necesarios.
    /// </remarks>
    /// <seealso cref="AddFileParameters(MultipartFormDataContent)"/>
    /// <seealso cref="AddFileData(MultipartFormDataContent)"/>
    /// <seealso cref="ClearBoundaryQuotes(MultipartFormDataContent)"/>
    protected virtual async Task<HttpContent> CreateFileUploadContent()
    {
        var result = new MultipartFormDataContent(GetBoundary());
        AddFileParameters(result);
        await AddFileData(result);
        ClearBoundaryQuotes(result);
        return result;
    }

    /// <summary>
    /// Genera un límite único para el uso en multipartes de solicitudes HTTP.
    /// </summary>
    /// <returns>
    /// Un string que representa un límite único, que se compone de una cadena fija seguida de un 
    /// identificador único global (GUID) generado aleatoriamente.
    /// </returns>
    protected virtual string GetBoundary()
    {
        return $"-----{Guid.NewGuid()}";
    }

    /// <summary>
    /// Agrega parámetros de archivo al contenido de tipo MultipartFormDataContent.
    /// </summary>
    /// <param name="content">El contenido al que se agregarán los parámetros de archivo.</param>
    /// <remarks>
    /// Este método obtiene los parámetros mediante el método <see cref="GetParameters"/> 
    /// y los agrega al contenido proporcionado como un ByteArrayContent, 
    /// utilizando la codificación UTF-8.
    /// </remarks>
    protected void AddFileParameters(MultipartFormDataContent content)
    {
        var parameters = GetParameters();
        foreach (var parameter in parameters)
        {
            var item = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(parameter.Value.SafeString()));
            content.Add(item, GetFileParameter(parameter.Key));
        }
    }

    /// <summary>
    /// Obtiene el parámetro de archivo, aplicando comillas si es necesario.
    /// </summary>
    /// <param name="param">El parámetro de archivo que se va a procesar.</param>
    /// <returns>
    /// Devuelve el parámetro de archivo con comillas si <see cref="IsFileParameterQuotes"/> es verdadero; de lo contrario, devuelve el parámetro sin modificaciones.
    /// </returns>
    protected string GetFileParameter(string param)
    {
        return IsFileParameterQuotes ? "\"" + param + "\"" : param;
    }

    /// <summary>
    /// Agrega datos de archivos al contenido de un formulario multipart.
    /// </summary>
    /// <param name="content">El contenido del formulario multipart donde se agregarán los datos de los archivos.</param>
    /// <remarks>
    /// Este método itera a través de una colección de archivos y agrega su contenido al 
    /// objeto <paramref name="content"/>. Si el archivo tiene un flujo asociado, se lee 
    /// directamente desde el flujo. Si no, se verifica si la ruta del archivo es válida 
    /// y se lee el contenido del archivo desde el sistema de archivos.
    /// </remarks>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Se lanza si la ruta del archivo especificado no existe.
    /// </exception>
    /// <exception cref="System.Exception">
    /// Se lanza si ocurre un error al leer el archivo o al agregar los datos al contenido.
    /// </exception>
    protected async Task AddFileData(MultipartFormDataContent content)
    {
        foreach (var file in Files)
        {
            if (file.Stream != null)
            {
                await using var fileStream = file.Stream;
                var bytes = await Util.Helpers.File.ReadToBytesAsync(fileStream);
                AddFileData(content, bytes, file.Name, file.FileName);
                continue;
            }
            if (file.FilePath.IsEmpty())
                continue;
            if (Util.Helpers.File.FileExists(file.FilePath) == false)
                return;
            var fileName = Path.GetFileName(file.FilePath);
            var stream = await Util.Helpers.File.ReadToMemoryStreamAsync(file.FilePath);
            AddFileData(content, stream.ToArray(), file.Name, fileName);
        }
    }

    /// <summary>
    /// Agrega datos de archivo al contenido de tipo MultipartFormDataContent.
    /// </summary>
    /// <param name="content">El contenido MultipartFormDataContent al que se agregarán los datos del archivo.</param>
    /// <param name="stream">El flujo de bytes que representa el archivo.</param>
    /// <param name="name">El nombre del parámetro del archivo.</param>
    /// <param name="fileName">El nombre del archivo que se está agregando.</param>
    /// <remarks>
    /// Este método verifica si el flujo de bytes es nulo antes de intentar agregarlo al contenido.
    /// Si el flujo es nulo, el método no realiza ninguna acción.
    /// </remarks>
    protected void AddFileData(MultipartFormDataContent content, byte[] stream, string name, string fileName)
    {
        if (stream == null)
            return;
        var fileContent = new ByteArrayContent(stream);
        content.Add(fileContent, GetFileParameter(name), GetFileParameter(fileName));
        if (fileContent.Headers is { ContentDisposition: not null })
            fileContent.Headers.ContentDisposition.FileNameStar = null;
    }

    /// <summary>
    /// Elimina las comillas del valor del límite (boundary) en el contenido de tipo MultipartFormData.
    /// </summary>
    /// <param name="content">El contenido MultipartFormData del cual se desea limpiar el límite.</param>
    /// <remarks>
    /// Este método verifica si el contenido tiene un límite definido en sus encabezados. 
    /// Si se encuentra un límite, se eliminan las comillas del valor del límite.
    /// Si no se encuentra un límite, el método no realiza ninguna acción.
    /// </remarks>
    protected void ClearBoundaryQuotes(MultipartFormDataContent content)
    {
        var boundary = content?.Headers?.ContentType?.Parameters.FirstOrDefault(o => o.Name == "boundary");
        if (boundary == null)
            return;
        boundary.Value = boundary.Value?.Replace("\"", null);
    }

    #endregion

    #region SendBefore(Operaciones antes de enviar)

    /// <summary>
    /// Envía un mensaje antes de realizar la acción principal.
    /// </summary>
    /// <param name="message">El mensaje HTTP que se va a enviar.</param>
    /// <returns>
    /// Devuelve true si se puede continuar con el envío del mensaje; de lo contrario, devuelve false.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay una acción definida para ejecutar antes de enviar el mensaje.
    /// Si no hay ninguna acción definida, se permite continuar con el envío.
    /// </remarks>
    protected virtual bool SendBefore(HttpRequestMessage message)
    {
        if (SendBeforeAction == null)
            return true;
        return SendBeforeAction(message);
    }

    #endregion

    #region SendAsync(Enviar solicitud)

    /// <summary>
    /// Envía de manera asíncrona un mensaje HTTP utilizando un cliente HTTP.
    /// </summary>
    /// <param name="message">El mensaje HTTP que se va a enviar.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el resultado de tipo <see cref="HttpResponseMessage"/>.</returns>
    /// <remarks>
    /// Este método inicializa el cliente HTTP y verifica que no sea nulo antes de enviar el mensaje.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el cliente HTTP es nulo.</exception>
    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, CancellationToken cancellationToken)
    {
        var client = GetClient();
        client.CheckNull(nameof(client));
        InitHttpClient(client);
        return await client.SendAsync(message, _completionOption, cancellationToken);
    }

    #endregion

    #region GetClient(Obtener cliente Http)

    /// <summary>
    /// Obtiene una instancia de <see cref="HttpClient"/>.
    /// </summary>
    /// <remarks>
    /// Este método verifica si ya existe una instancia de <see cref="_httpClient"/>. 
    /// Si no existe, crea un nuevo <see cref="HttpClient"/> utilizando un manejador de cliente HTTP configurado.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="HttpClient"/>. Si ya existe una instancia, se devuelve esa; 
    /// de lo contrario, se crea una nueva instancia utilizando el <see cref="_httpClientFactory"/>.
    /// </returns>
    protected HttpClient GetClient()
    {
        if (_httpClient != null)
            return _httpClient;
        var clientHandler = CreateHttpClientHandler();
        InitHttpClientHandler(clientHandler);
        return _httpClientName.IsEmpty() ? _httpClientFactory.CreateClient() : _httpClientFactory.CreateClient(_httpClientName);
    }

    /// <summary>
    /// Crea una instancia de <see cref="HttpClientHandler"/> utilizando una fábrica de manejadores de mensajes.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="HttpClientHandler"/> si se pudo crear correctamente; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la instancia de <see cref="_httpClientFactory"/> es de tipo <see cref="IHttpMessageHandlerFactory"/>.
    /// Si es así, crea un manejador de mensajes. Si el nombre del cliente HTTP está vacío, se utiliza un manejador predeterminado;
    /// de lo contrario, se utiliza el nombre del cliente HTTP proporcionado para crear el manejador.
    /// El método también descompone cualquier <see cref="DelegatingHandler"/> hasta llegar al manejador interno.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si la fábrica de manejadores de mensajes no se puede convertir a <see cref="IHttpMessageHandlerFactory"/>.
    /// </exception>
    protected HttpClientHandler CreateHttpClientHandler()
    {
        var handlerFactory = _httpClientFactory as IHttpMessageHandlerFactory;
        if (handlerFactory == null)
            return null;
        var handler = _httpClientName.IsEmpty() ? handlerFactory.CreateHandler() : handlerFactory.CreateHandler(_httpClientName);
        while (handler is DelegatingHandler delegatingHandler)
        {
            handler = delegatingHandler.InnerHandler;
        }
        return handler as HttpClientHandler;
    }

    /// <summary>
    /// Inicializa el manejador de HTTP proporcionado con configuraciones específicas.
    /// </summary>
    /// <param name="handler">El manejador de HTTP que se va a inicializar. Si es null, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar
    /// configuraciones adicionales o personalizadas al manejador de HTTP.
    /// </remarks>
    protected virtual void InitHttpClientHandler(HttpClientHandler handler)
    {
        if (handler == null)
            return;
        InitCertificate(handler);
        InitUseCookies(handler);
        IgnoreSsl(handler);
    }

    #endregion

    #region InitCertificate(Inicializar certificado)

    /// <summary>
    /// Inicializa el certificado para el manejador de cliente HTTP.
    /// </summary>
    /// <param name="handler">El manejador de cliente HTTP que se utilizará para las solicitudes.</param>
    /// <remarks>
    /// Este método verifica si la ruta del certificado está vacía. Si no lo está, 
    /// crea un nuevo objeto <see cref="X509Certificate2"/> utilizando la ruta y la contraseña del certificado, 
    /// y lo agrega a la colección de certificados del manejador.
    /// </remarks>
    /// <exception cref="System.Security.Cryptography.CryptographicException">
    /// Se produce si hay un error al cargar el certificado.
    /// </exception>
    /// <seealso cref="X509Certificate2"/>
    protected virtual void InitCertificate(HttpClientHandler handler)
    {
        if (CertificatePath.IsEmpty())
            return;
        var certificate = new X509Certificate2(CertificatePath, CertificatePassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
        handler.ClientCertificates.Clear();
        handler.ClientCertificates.Add(certificate);
    }

    #endregion

    #region InitUseCookies(¿La inicialización lleva cookies?)

    /// <summary>
    /// Inicializa el uso de cookies en el manejador de solicitudes HTTP.
    /// </summary>
    /// <param name="handler">El manejador de solicitudes HTTP que se configurará para usar cookies.</param>
    /// <remarks>
    /// Este método verifica si la propiedad <c>UseCookies</c> del <paramref name="handler"/> 
    /// es diferente de la propiedad <c>IsUseCookies</c>. Si es así, se establece 
    /// <c>UseCookies</c> en el valor de <c>IsUseCookies</c>.
    /// </remarks>
    protected virtual void InitUseCookies(HttpClientHandler handler)
    {
        if (handler.UseCookies != IsUseCookies)
            handler.UseCookies = IsUseCookies;
    }

    #endregion

    #region IgnoreSsl(Ignorar errores de certificado SSL.)

    /// <summary>
    /// Configura el manejador de HttpClient para ignorar la validación del certificado SSL.
    /// </summary>
    /// <param name="handler">El manejador de HttpClient que se configurará para ignorar la validación SSL.</param>
    /// <remarks>
    /// Este método se utiliza para permitir conexiones a servidores que presentan certificados SSL no válidos.
    /// Es importante tener en cuenta que ignorar la validación de certificados puede exponer la aplicación a riesgos de seguridad.
    /// </remarks>
    protected virtual void IgnoreSsl(HttpClientHandler handler)
    {
        if (_ignoreSsl == false)
            return;
        handler.ServerCertificateCustomValidationCallback ??= (_, _, _, _) => true;
    }

    #endregion

    #region InitHttpClient(Inicializar el cliente Http.)

    /// <summary>
    /// Inicializa el cliente HTTP con la configuración base necesaria.
    /// </summary>
    /// <param name="client">El cliente HTTP que se va a inicializar.</param>
    /// <remarks>
    /// Este método establece la dirección base y el tiempo de espera para el cliente HTTP.
    /// Debe ser llamado antes de realizar cualquier solicitud HTTP.
    /// </remarks>
    protected virtual void InitHttpClient(HttpClient client)
    {
        InitBaseAddress(client);
        InitTimeout(client);
    }

    #endregion

    #region InitBaseAddress(Inicializar la dirección base.)

    /// <summary>
    /// Inicializa la dirección base del cliente HTTP.
    /// </summary>
    /// <param name="client">El cliente HTTP que se va a inicializar.</param>
    /// <remarks>
    /// Este método establece la propiedad <see cref="HttpClient.BaseAddress"/> 
    /// del cliente HTTP si la URI de la dirección base no está vacía.
    /// </remarks>
    protected virtual void InitBaseAddress(HttpClient client)
    {
        if (BaseAddressUri.IsEmpty())
            return;
        client.BaseAddress = new Uri(BaseAddressUri);
    }

    #endregion

    #region InitTimeout(Initialize timeout interval)

    /// <summary>
    /// Inicializa el tiempo de espera para el cliente HTTP.
    /// </summary>
    /// <param name="client">El cliente HTTP al que se le asignará el tiempo de espera.</param>
    /// <remarks>
    /// Este método verifica si la propiedad <c>HttpTimeout</c> es nula. 
    /// Si no lo es, se establece el tiempo de espera del cliente HTTP utilizando el valor seguro de <c>HttpTimeout</c>.
    /// </remarks>
    protected virtual void InitTimeout(HttpClient client)
    {
        if (HttpTimeout == null)
            return;
        client.Timeout = HttpTimeout.SafeValue();
    }

    #endregion

    #region SendAfterAsync(Enviar después de Async)

    /// <summary>
    /// Envía una acción después de recibir una respuesta HTTP asincrónica.
    /// </summary>
    /// <param name="response">La respuesta HTTP que se ha recibido.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TResult"/> que representa el resultado de la acción enviada, o null si la operación falla.</returns>
    /// <remarks>
    /// Este método verifica si hay una acción definida para enviar después de recibir la respuesta.
    /// Si existe, se ejecuta esa acción. Si no, se procesa el contenido de la respuesta.
    /// Si la respuesta es exitosa, se maneja con el controlador de éxito; de lo contrario, se maneja con el controlador de fallo.
    /// Finalmente, se ejecuta el controlador de finalización independientemente del resultado.
    /// </remarks>
    /// <seealso cref="SendAfterAction"/>
    /// <seealso cref="SuccessHandlerAsync(HttpResponseMessage, string)"/>
    /// <seealso cref="FailHandler(HttpResponseMessage, string)"/>
    /// <seealso cref="CompleteHandler(HttpResponseMessage, string)"/>
    protected virtual async Task<TResult> SendAfterAsync(HttpResponseMessage response)
    {
        if (SendAfterAction != null)
            return await SendAfterAction(response);
        string content = null;
        try
        {
            content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return await SuccessHandlerAsync(response, content);
            FailHandler(response, content);
            return null;
        }
        finally
        {
            CompleteHandler(response, content);
        }
    }

    #endregion

    #region SuccessHandler(Operación procesada con éxito.)

    /// <summary>
    /// Maneja la respuesta exitosa de una solicitud HTTP.
    /// </summary>
    /// <param name="response">El objeto <see cref="HttpResponseMessage"/> que contiene la respuesta de la solicitud.</param>
    /// <param name="content">El contenido de la respuesta en forma de cadena.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TResult"/> que representa el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método convierte el contenido de la respuesta utilizando el tipo de contenido especificado en la respuesta.
    /// Luego, invoca una acción de éxito si está definida y, si se proporciona, ejecuta una función de éxito de manera asíncrona.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que se espera de la operación.</typeparam>
    /// <seealso cref="HttpResponseMessage"/>
    /// <seealso cref="ConvertTo(string, string)"/>
    /// <seealso cref="SuccessAction"/>
    /// <seealso cref="SuccessFunc"/>
    protected virtual async Task<TResult> SuccessHandlerAsync(HttpResponseMessage response, string content)
    {
        var result = ConvertTo(content, response.GetContentType());
        SuccessAction?.Invoke(result);
        if (SuccessFunc != null)
            await SuccessFunc(result);
        return result;
    }

    #endregion

    #region ConvertTo(Convierte el contenido en resultados.)

    /// <summary>
    /// Convierte el contenido proporcionado a un tipo de resultado específico.
    /// </summary>
    /// <param name="content">El contenido que se desea convertir.</param>
    /// <param name="contentType">El tipo de contenido que se está procesando.</param>
    /// <returns>
    /// Un objeto del tipo TResult que representa el contenido convertido.
    /// Si no se puede realizar la conversión, se devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una acción de conversión personalizada si está disponible.
    /// Si el tipo de resultado es una cadena, se devuelve el contenido tal cual.
    /// Si el tipo de contenido es "application/json", se intenta deserializar el contenido a TResult.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado al que se convertirá el contenido.</typeparam>
    /// <seealso cref="ConvertAction"/>
    /// <seealso cref="Json.ToObject{T}(string, JsonSerializerOptions)"/>
    protected virtual TResult ConvertTo(string content, string contentType)
    {
        if (ConvertAction != null)
            return ConvertAction(content);
        if (typeof(TResult) == typeof(string))
            return (TResult)(object)content;
        return contentType.SafeString().ToLower() == "application/json" ? Json.ToObject<TResult>(content, GetJsonSerializerOptions()) : null;
    }

    #endregion

    #region FailHandler(Operación de manejo de fallos.)

    /// <summary>
    /// Maneja la falla de una respuesta HTTP.
    /// </summary>
    /// <param name="response">La respuesta HTTP que contiene información sobre el error.</param>
    /// <param name="content">El contenido asociado a la respuesta que puede proporcionar más detalles sobre el error.</param>
    /// <remarks>
    /// Este método invoca una acción de falla si está definida, permitiendo que el consumidor del método maneje la respuesta de error de manera personalizada.
    /// </remarks>
    protected virtual void FailHandler(HttpResponseMessage response, object content)
    {
        FailAction?.Invoke(response, content);
    }

    #endregion

    #region CompleteHandler(Ejecutar operación completada.)

    /// <summary>
    /// Maneja la finalización de una acción HTTP, invocando un delegado si está disponible.
    /// </summary>
    /// <param name="response">La respuesta HTTP que se ha recibido.</param>
    /// <param name="content">El contenido asociado a la respuesta HTTP.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una funcionalidad adicional
    /// al manejar la respuesta y el contenido.
    /// </remarks>
    /// <seealso cref="CompleteAction"/>
    protected virtual void CompleteHandler(HttpResponseMessage response, object content)
    {
        CompleteAction?.Invoke(response, content);
    }

    #endregion

    #region GetStreamAsync(Obtener flujo)

    /// <inheritdoc />
    /// <summary>
    /// Obtiene un flujo de bytes de manera asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{Byte[]}"/> que representa la operación asíncrona, 
    /// conteniendo un arreglo de bytes que representa el flujo obtenido, 
    /// o <c>null</c> si el mensaje no se envió correctamente.
    /// </returns>
    /// <remarks>
    /// Este método crea un mensaje, lo envía y espera una respuesta. 
    /// Si el envío del mensaje falla, se devuelve <c>null</c>.
    /// </remarks>
    /// <seealso cref="CreateMessage"/>
    /// <seealso cref="SendBefore(Message)"/>
    /// <seealso cref="SendAsync(Message, CancellationToken)"/>
    /// <seealso cref="GetStream(Response)"/>
    public async Task<byte[]> GetStreamAsync(CancellationToken cancellationToken = default)
    {
        var message = await CreateMessage();
        if (SendBefore(message) == false)
            return default;
        var response = await SendAsync(message, cancellationToken);
        return await GetStream(response);
    }

    /// <summary>
    /// Obtiene el contenido de la respuesta HTTP como un arreglo de bytes.
    /// </summary>
    /// <param name="response">La respuesta HTTP de la que se desea obtener el contenido.</param>
    /// <returns>
    /// Un arreglo de bytes que representa el contenido de la respuesta si la solicitud fue exitosa; de lo contrario, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método maneja la lectura del contenido de la respuesta y verifica si el código de estado de la respuesta indica éxito.
    /// Si la respuesta no es exitosa, se invoca un manejador de fallos.
    /// En cualquier caso, se invoca un manejador de finalización al finalizar la operación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="response"/> es <c>null</c>.</exception>
    protected virtual async Task<byte[]> GetStream(HttpResponseMessage response)
    {
        byte[] content = null;
        try
        {
            content = await response.Content.ReadAsByteArrayAsync();
            if (response.IsSuccessStatusCode)
                return content;
            FailHandler(response, content);
            return null;
        }
        finally
        {
            CompleteHandler(response, content);
        }
    }

    #endregion

    #region WriteAsync(Escribir en un archivo.)

    /// <inheritdoc />
    /// <summary>
    /// Escribe de manera asíncrona un archivo en la ruta especificada.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de escritura.</returns>
    /// <remarks>
    /// Este método obtiene un flujo de bytes de manera asíncrona y lo escribe en el archivo especificado.
    /// Asegúrese de que la ruta del archivo sea válida y que tenga permisos de escritura.
    /// </remarks>
    /// <seealso cref="GetStreamAsync(CancellationToken)"/>
    /// <seealso cref="Util.Helpers.File.WriteAsync(string, byte[], CancellationToken)"/>
    public async Task WriteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var bytes = await GetStreamAsync(cancellationToken);
        await Util.Helpers.File.WriteAsync(filePath, bytes, cancellationToken);
    }

    #endregion
}