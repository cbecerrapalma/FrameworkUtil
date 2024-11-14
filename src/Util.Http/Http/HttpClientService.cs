namespace Util.Http;

/// <summary>
/// Proporciona servicios de cliente HTTP para realizar solicitudes a servicios web.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IHttpClient"/> y maneja la configuración
/// y ejecución de solicitudes HTTP, así como el manejo de respuestas.
/// </remarks>
public class HttpClientService : IHttpClient
{

    #region Campo

    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient _httpClient;

    #endregion

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HttpClientService"/>.
    /// </summary>
    /// <param name="factory">Una instancia de <see cref="IHttpClientFactory"/> que se utilizará para crear instancias de <see cref="HttpClient"/>. Si se proporciona <c>null</c>, se utilizará el valor predeterminado.</param>
    public HttpClientService(IHttpClientFactory factory = null)
    {
        _httpClientFactory = factory;
    }

    #endregion

    #region SetHttpClient(Configurar el cliente Http)
    /// <summary>
    /// Establece el cliente HTTP utilizado por el servicio.
    /// </summary>
    /// <param name="client">El cliente HTTP que se desea establecer.</param>
    /// <returns>La instancia actual de <see cref="HttpClientService"/>.</returns>
    public HttpClientService SetHttpClient(HttpClient client)
    {
        _httpClient = client;
        return this;
    }
    #endregion

    #region Get

    /// <inheritdoc />
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada y devuelve la respuesta como un objeto de tipo <see cref="IHttpRequest{T}"/>.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <returns>
    /// Un objeto <see cref="IHttpRequest{T}"/> que contiene la respuesta de la solicitud GET.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite obtener la respuesta en formato de cadena.
    /// </remarks>
    /// <seealso cref="Get{T}(string)"/>
    public IHttpRequest<string> Get(string url)
    {
        return Get<string>(url);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada con los parámetros de consulta proporcionados.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <param name="queryString">Un objeto que representa los parámetros de consulta que se agregarán a la URL.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IHttpRequest{T}"/> que contiene la respuesta de la solicitud GET.
    /// </returns>
    /// <seealso cref="IHttpRequest{T}"/>
    public IHttpRequest<string> Get(string url, object queryString)
    {
        return Get<string>(url, queryString);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada y devuelve el resultado deserializado en el tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera recibir de la solicitud HTTP. Debe ser una clase.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <returns>Un objeto que implementa <see cref="IHttpRequest{TResult}"/> que contiene el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite realizar una solicitud GET sin parámetros adicionales.
    /// </remarks>
    /// <seealso cref="Get{TResult}(string, object)"/>
    public IHttpRequest<TResult> Get<TResult>(string url) where TResult : class
    {
        return Get<TResult>(url, null);
    }

    /// <inheritdoc />
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada con un conjunto opcional de parámetros de consulta.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud HTTP.</typeparam>
    /// <param name="url">La URL a la que se realizará la solicitud GET.</param>
    /// <param name="queryString">Un objeto que representa los parámetros de consulta que se agregarán a la URL.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP GET.
    /// </returns>
    /// <remarks>
    /// Si <paramref name="queryString"/> es nulo o una cadena vacía, se realizará la solicitud sin parámetros de consulta.
    /// </remarks>
    public IHttpRequest<TResult> Get<TResult>(string url, object queryString) where TResult : class
    {
        var result = new HttpRequest<TResult>(_httpClientFactory, _httpClient, HttpMethod.Get, url);
        if (queryString.SafeString().IsEmpty())
            return result;
        return result.QueryString(queryString);
    }

    #endregion

    #region Post

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada y devuelve la respuesta como un objeto de tipo <see cref="IHttpRequest{T}"/>.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IHttpRequest{T}"/> que contiene la respuesta de la solicitud POST.
    /// </returns>
    /// <seealso cref="IHttpRequest{T}"/>
    public IHttpRequest<string> Post(string url)
    {
        return Post<string>(url);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <param name="content">El contenido que se enviará en el cuerpo de la solicitud.</param>
    /// <returns>
    /// Un objeto que representa la respuesta de la solicitud HTTP, que contiene el resultado como una cadena.
    /// </returns>
    /// <seealso cref="Post{T}(string, object)"/>
    public IHttpRequest<string> Post(string url, object content)
    {
        return Post<string>(url, content);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <returns>
    /// Un objeto que representa la solicitud HTTP, que contiene el resultado de la operación.
    /// </returns>
    /// <seealso cref="Post{TResult}(string, object)"/>
    public IHttpRequest<TResult> Post<TResult>(string url) where TResult : class
    {
        return Post<TResult>(url, null);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <param name="content">El contenido que se enviará en el cuerpo de la solicitud. Puede ser nulo.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método crea una nueva solicitud HTTP POST utilizando el cliente HTTP configurado. 
    /// Si el contenido es nulo, se devuelve una solicitud sin contenido.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    public IHttpRequest<TResult> Post<TResult>(string url, object content) where TResult : class
    {
        var result = new HttpRequest<TResult>(_httpClientFactory, _httpClient, HttpMethod.Post, url);
        if (content == null)
            return result;
        return result.Content(content);
    }

    #endregion

    #region Put

    /// <inheritdoc />
    /// <summary>
    /// Realiza una solicitud HTTP PUT a la URL especificada.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <returns>Un objeto que representa la respuesta de la solicitud HTTP, encapsulado en un tipo de dato genérico.</returns>
    /// <remarks>
    /// Este método es una implementación que hereda de un método base y proporciona un tipo de respuesta específico.
    /// </remarks>
    /// <seealso cref="IHttpRequest{T}"/>
    public IHttpRequest<string> Put(string url)
    {
        return Put<string>(url);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <param name="content">El contenido que se enviará en la solicitud.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IHttpRequest{T}"/> que representa la respuesta de la solicitud.
    /// </returns>
    /// <seealso cref="IHttpRequest{T}"/>
    public IHttpRequest<string> Put(string url, object content)
    {
        return Put<string>(url, content);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método permite realizar una solicitud PUT sin un cuerpo de contenido. 
    /// Si se necesita enviar datos, se debe utilizar el método sobrecargado que acepta un segundo parámetro.
    /// </remarks>
    public IHttpRequest<TResult> Put<TResult>(string url) where TResult : class
    {
        return Put<TResult>(url, null);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <param name="content">El contenido que se enviará en la solicitud. Puede ser nulo.</param>
    /// <returns>
    /// Un objeto <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP PUT.
    /// Si el contenido es nulo, se devuelve una solicitud sin contenido.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un cliente HTTP para realizar la solicitud y permite especificar 
    /// el tipo de resultado que se espera recibir.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    public IHttpRequest<TResult> Put<TResult>(string url, object content) where TResult : class
    {
        var result = new HttpRequest<TResult>(_httpClientFactory, _httpClient, HttpMethod.Put, url);
        if (content == null)
            return result;
        return result.Content(content);
    }

    #endregion

    #region Delete

    /// <inheritdoc />
    /// <summary>
    /// Elimina un recurso en la URL especificada.
    /// </summary>
    /// <param name="url">La URL del recurso que se desea eliminar.</param>
    /// <returns>Un objeto que representa la solicitud HTTP realizada, conteniendo la respuesta como una cadena.</returns>
    /// <remarks>
    /// Este método es una implementación de la interfaz <see cref="IHttpRequest{T}"/> 
    /// y utiliza el método genérico <see cref="Delete{T}(string)"/> para realizar la operación.
    /// </remarks>
    public IHttpRequest<string> Delete(string url)
    {
        return Delete<string>(url);
    }

    /// <inheritdoc />
    /// <summary>
    /// Envía una solicitud HTTP DELETE a la URL especificada.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud DELETE.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método utiliza un cliente HTTP para realizar la solicitud y devuelve un objeto que permite manejar la respuesta.
    /// </remarks>
    public IHttpRequest<TResult> Delete<TResult>(string url) where TResult : class
    {
        return new HttpRequest<TResult>(_httpClientFactory, _httpClient, HttpMethod.Delete, url);
    }

    #endregion
}