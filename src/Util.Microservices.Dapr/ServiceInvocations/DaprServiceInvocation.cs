namespace Util.Microservices.Dapr.ServiceInvocations;

/// <summary>
/// Representa un servicio de invocación Dapr que implementa la interfaz <see cref="IServiceInvocation"/>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DaprServiceInvocationBase{T}"/> y proporciona la funcionalidad necesaria
/// para invocar servicios a través de Dapr.
/// </remarks>
/// <typeparam name="IServiceInvocation">El tipo de la interfaz que define las operaciones de invocación de servicio.</typeparam>
public class DaprServiceInvocation : DaprServiceInvocationBase<IServiceInvocation>, IServiceInvocation
{

    #region Método de construcción

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprServiceInvocation"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr que se utilizará para invocar servicios.</param>
    /// <param name="options">Las opciones de configuración para Dapr.</param>
    /// <param name="loggerFactory">La fábrica de registros que se utilizará para crear instancias de <see cref="ILogger"/>.</param>
    public DaprServiceInvocation(DaprClient client, IOptions<DaprOptions> options, ILoggerFactory loggerFactory) : base(client, options, loggerFactory)
    {
    }

    #endregion

    #region Evento

    /// <summary>
    /// Representa una función que se ejecuta antes de una acción específica.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una lógica personalizada que se ejecutará antes de que se procese una solicitud HTTP.
    /// </remarks>
    /// <value>
    /// Una función que toma un <see cref="HttpRequestMessage"/> como parámetro y devuelve un valor booleano que indica si se debe continuar con la acción.
    /// </value>
    protected Func<HttpRequestMessage, bool> OnBeforeAction { get; set; }
    /// <summary>
    /// Representa una acción que se ejecuta al obtener un resultado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una función que toma un objeto de tipo <see cref="HttpResponseMessage"/> y un objeto de tipo <see cref="JsonSerializerOptions"/> 
    /// junto con un <see cref="CancellationToken"/> y devuelve un <see cref="Task"/> que produce un objeto. 
    /// Se utiliza para procesar la respuesta de una solicitud HTTP de manera asíncrona.
    /// </remarks>
    /// <value>
    /// Una función que recibe un <see cref="HttpResponseMessage"/>, un <see cref="JsonSerializerOptions"/> y un <see cref="CancellationToken"/>, 
    /// y devuelve un <see cref="Task"/> que produce un objeto.
    /// </value>
    protected Func<HttpResponseMessage, JsonSerializerOptions, CancellationToken, Task<object>> OnResultAction { get; set; }
    /// <summary>
    /// Obtiene o establece una acción que se ejecutará después de que se complete una acción HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir un comportamiento personalizado que se ejecutará después de que se haya procesado la respuesta HTTP.
    /// </remarks>
    /// <value>
    /// Una acción que toma un <see cref="HttpResponseMessage"/> como parámetro.
    /// </value>
    protected Action<HttpResponseMessage> OnAfterAction { get; set; }
    /// <summary>
    /// Representa una acción que se ejecuta cuando una solicitud HTTP se procesa exitosamente.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/> 
    /// como parámetros, junto con un objeto adicional, y devuelve un <see cref="Task"/> que representa la operación asíncrona.
    /// </remarks>
    /// <value>
    /// Una función que se ejecuta al completar exitosamente una solicitud HTTP.
    /// </value>
    protected Func<HttpRequestMessage, HttpResponseMessage, object, Task> OnSuccessAction { get; set; }
    /// <summary>
    /// Obtiene o establece la acción a ejecutar cuando se produce un error.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir un comportamiento personalizado que se ejecutará 
    /// en caso de que ocurra una excepción durante el procesamiento de una solicitud HTTP.
    /// </remarks>
    /// <value>
    /// Una función que toma un <see cref="HttpRequestMessage"/>, un <see cref="HttpResponseMessage"/> 
    /// y una <see cref="Exception"/>, y devuelve un <see cref="Task"/>.
    /// </value>
    protected Func<HttpRequestMessage, HttpResponseMessage, Exception, Task> OnFailAction { get; set; }
    /// <summary>
    /// Obtiene o establece una función que se ejecuta cuando se recibe una respuesta no autorizada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir un comportamiento personalizado al manejar respuestas HTTP que indican que la solicitud no fue autorizada.
    /// </remarks>
    /// <value>
    /// Una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/> como parámetros y devuelve un <see cref="Task"/>.
    /// </value>
    protected Func<HttpRequestMessage, HttpResponseMessage, Task> OnUnauthorizedAction { get; set; }
    /// <summary>
    /// Representa una acción que se ejecuta al completar una operación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir una función que se ejecutará cuando se complete una operación
    /// que involucra un mensaje de solicitud HTTP y su correspondiente respuesta.
    /// </remarks>
    /// <value>
    /// Una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/> 
    /// como parámetros y devuelve un <see cref="Task"/>.
    /// </value>
    protected Func<HttpRequestMessage, HttpResponseMessage, Task> OnCompleteAction { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará antes de la invocación del servicio.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y devuelve un valor booleano que indica si se debe continuar con la invocación.</param>
    /// <returns>La instancia actual de <see cref="IServiceInvocation"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Esta función permite interceptar y modificar la solicitud HTTP antes de que se envíe.
    /// </remarks>
    public IServiceInvocation OnBefore(Func<HttpRequestMessage, bool> action)
    {
        OnBeforeAction = action;
        return this;
    }

    /// <summary>
    /// Establece una acción que se ejecutará cuando se reciba un resultado.
    /// </summary>
    /// <param name="action">La función que procesa el mensaje de respuesta HTTP, las opciones de serialización JSON y un token de cancelación, devolviendo un objeto de forma asíncrona.</param>
    /// <returns>Devuelve la instancia actual de <see cref="IServiceInvocation"/>.</returns>
    public IServiceInvocation OnResult(Func<HttpResponseMessage, JsonSerializerOptions, CancellationToken, Task<object>> action)
    {
        OnResultAction = action;
        return this;
    }

    /// <summary>
    /// Establece una acción que se ejecutará después de recibir una respuesta HTTP.
    /// </summary>
    /// <param name="action">La acción que se ejecutará con el mensaje de respuesta HTTP.</param>
    /// <returns>La instancia actual de <see cref="IServiceInvocation"/>.</returns>
    public IServiceInvocation OnAfter(Action<HttpResponseMessage> action)
    {
        OnAfterAction = action;
        return this;
    }

    /// <inheritdoc />
    /// <summary>
    /// Establece una acción que se ejecutará cuando la invocación del servicio sea exitosa.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera de la invocación del servicio.</typeparam>
    /// <param name="action">La acción a ejecutar, que recibe el mensaje de solicitud, el mensaje de respuesta y el resultado de la invocación.</param>
    /// <returns>La instancia actual de <see cref="IServiceInvocation"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Esta función permite definir un comportamiento personalizado que se ejecutará cuando la invocación del servicio se complete con éxito.
    /// Asegúrese de que la acción proporcionada no sea nula antes de asignarla.
    /// </remarks>
    public IServiceInvocation OnSuccess<TResponse>(Func<HttpRequestMessage, HttpResponseMessage, TResponse, Task> action)
    {
        if (action != null)
            OnSuccessAction = (request, response, result) => action(request, response, (TResponse)result);
        return this;
    }

    /// <summary>
    /// Establece una acción que se ejecutará cuando una invocación falle.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/>, un <see cref="HttpResponseMessage"/> y una <see cref="Exception"/> como parámetros, y devuelve una tarea asincrónica.</param>
    /// <returns>La instancia actual de <see cref="IServiceInvocation"/>.</returns>
    /// <remarks>
    /// Esta acción se invocará en caso de que ocurra un error durante la ejecución de la invocación de servicio.
    /// </remarks>
    public IServiceInvocation OnFail(Func<HttpRequestMessage, HttpResponseMessage, Exception, Task> action)
    {
        OnFailAction = action;
        return this;
    }

    /// <summary>
    /// Establece una acción a ejecutar cuando se produce un error de autorización.
    /// </summary>
    /// <param name="action">La acción que se ejecutará, que recibe un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/>, y devuelve una tarea asincrónica.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Esta función permite definir un comportamiento personalizado cuando se detecta un estado de no autorizado.
    /// </remarks>
    public IServiceInvocation OnUnauthorized(Func<HttpRequestMessage, HttpResponseMessage, Task> action)
    {
        OnUnauthorizedAction = action;
        return this;
    }

    /// <summary>
    /// Establece una acción que se ejecutará al completar la invocación del servicio.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/>, y devuelve una tarea asincrónica.</param>
    /// <returns>La instancia actual de <see cref="IServiceInvocation"/>.</returns>
    public IServiceInvocation OnComplete(Func<HttpRequestMessage, HttpResponseMessage, Task> action)
    {
        OnCompleteAction = action;
        return this;
    }

    #endregion

    #region Método de sobrecarga de llamadas. 

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado utilizando el método HTTP GET.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que simplifica la invocación de métodos al utilizar el método HTTP GET por defecto.
    /// </remarks>
    /// <seealso cref="InvokeAsync(string, HttpMethod, CancellationToken)"/>
    public async Task InvokeAsync(string methodName, CancellationToken cancellationToken = default)
    {
        await InvokeAsync(methodName, HttpMethod.Get, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca un método de forma asíncrona utilizando el nombre del método y el método HTTP especificado.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones asíncronas a métodos especificados, facilitando la interacción con servicios web o APIs.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TRequest, TResponse}"/>
    public async Task InvokeAsync(string methodName, HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        await InvokeAsync<object, object>(methodName, null, httpMethod, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado con los datos proporcionados.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para permitir la cancelación de la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="InvokeAsync(string, object, HttpMethod, CancellationToken)"/> 
    /// para realizar la invocación con el método HTTP POST.
    /// </remarks>
    public async Task InvokeAsync(string methodName, object data, CancellationToken cancellationToken = default)
    {
        await InvokeAsync(methodName, data, HttpMethod.Post, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado con los datos proporcionados y el método HTTP indicado.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite invocar métodos de manera asíncrona sin necesidad de especificar el tipo de retorno.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TRequest, TResponse}(string, TRequest, HttpMethod, CancellationToken)"/>
    public async Task InvokeAsync(string methodName, object data, HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        await InvokeAsync<object, object>(methodName, data, httpMethod, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado y devuelve una respuesta de tipo TResponse.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de la respuesta esperada.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="cancellationToken">Token para la cancelación de la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado de tipo TResponse.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="InvokeAsync{TResponse}(string, HttpMethod, CancellationToken)"/> 
    /// con el método HTTP GET para realizar la invocación.
    /// </remarks>
    public async Task<TResponse> InvokeAsync<TResponse>(string methodName, CancellationToken cancellationToken = default)
    {
        return await InvokeAsync<TResponse>(methodName, HttpMethod.Get, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado utilizando el método HTTP indicado.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta esperada del método invocado.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene la respuesta del tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite invocar un método sin parámetros adicionales.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TResponse}(string, object, HttpMethod, CancellationToken)"/>
    public async Task<TResponse> InvokeAsync<TResponse>(string methodName, HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        return await InvokeAsync<TResponse>(methodName, null, httpMethod, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca un método de forma asíncrona y devuelve una respuesta de tipo especificado.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de la respuesta esperada.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método HTTP GET para realizar la invocación.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TResponse}(string, object, HttpMethod, CancellationToken)"/>
    public async Task<TResponse> InvokeAsync<TResponse>(string methodName, object data, CancellationToken cancellationToken = default)
    {
        return await InvokeAsync<TResponse>(methodName, data, HttpMethod.Get, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado con los datos proporcionados y el método HTTP indicado.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta esperada del método invocado.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene la respuesta del método invocado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que simplifica la invocación al permitir especificar solo el tipo de respuesta.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TRequest, TResponse}(string, TRequest, HttpMethod, CancellationToken)"/>
    public async Task<TResponse> InvokeAsync<TResponse>(string methodName, object data, HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        return await InvokeAsync<object, TResponse>(methodName, data, httpMethod, cancellationToken);
    }

    /// <inheritdoc />
    /// <summary>
    /// Invoca de manera asíncrona un método especificado con los datos proporcionados.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se envían al método.</typeparam>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera del método.</typeparam>
    /// <param name="methodName">El nombre del método que se va a invocar.</param>
    /// <param name="data">Los datos que se enviarán al método.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/> que contiene la respuesta del método invocado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método HTTP GET para realizar la invocación.
    /// </remarks>
    /// <seealso cref="InvokeAsync{TRequest, TResponse}(string, TRequest, HttpMethod, CancellationToken)"/>
    public async Task<TResponse> InvokeAsync<TRequest, TResponse>(string methodName, TRequest data, CancellationToken cancellationToken = default)
    {
        return await InvokeAsync<TRequest, TResponse>(methodName, data, HttpMethod.Get, cancellationToken);
    }

    #endregion

    #region InvokeAsync

    /// <inheritdoc />
    /// <summary>
    /// Invoca un método de servicio de forma asíncrona y maneja la solicitud y respuesta.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se envían en la solicitud.</typeparam>
    /// <typeparam name="TResponse">El tipo de datos que se esperan en la respuesta.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán en la solicitud.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la solicitud.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TResponse"/> que contiene los datos de la respuesta.
    /// Si la invocación falla, se devuelve el valor predeterminado de <typeparamref name="TResponse"/>.
    /// </returns>
    /// <remarks>
    /// Este método realiza una serie de pasos, incluyendo la creación de la solicitud, 
    /// el filtrado de la misma, la invocación del método, y el manejo de la respuesta. 
    /// También permite la ejecución de acciones antes y después de la invocación.
    /// </remarks>
    /// <seealso cref="CreateRequest"/>
    /// <seealso cref="FilterRequest"/>
    /// <seealso cref="InvokeBefore"/>
    /// <seealso cref="Client.InvokeMethodWithResponseAsync"/>
    /// <seealso cref="FilterResponse"/>
    /// <seealso cref="ToResult{TResponse}"/>
    /// <seealso cref="InvokeAfter"/>
    /// <seealso cref="FailHandlerAsync"/>
    /// <seealso cref="CompleteHandlerAsync"/>
    public async Task<TResponse> InvokeAsync<TRequest, TResponse>(string methodName, TRequest data, HttpMethod httpMethod, CancellationToken cancellationToken = default)
    {
        Log.LogTrace("Preparándose para llamar al método del servicio.,AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        var request = CreateRequest(methodName, data, httpMethod);
        request = FilterRequest(request);
        if (await InvokeBefore(methodName, data, httpMethod, request) == false)
            return default;
        HttpResponseMessage response = null;
        try
        {
            response = await Client.InvokeMethodWithResponseAsync(request, cancellationToken);
            response = FilterResponse(response);
            OnAfterAction?.Invoke(response);
            var result = await ToResult<TResponse>(response, cancellationToken);
            await InvokeAfter(methodName, data, httpMethod, request, response, result);
            return result.Data;
        }
        catch (Warning)
        {
            throw;
        }
        catch (Exception exception)
        {
            await FailHandlerAsync(methodName, data, httpMethod, request, response, exception, null);
        }
        finally
        {
            await CompleteHandlerAsync(methodName, data, httpMethod, request, response);
        }
        return default;
    }

    #endregion

    #region CreateRequest

    /// <summary>
    /// Crea un objeto <see cref="HttpRequestMessage"/> para invocar un método especificado.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se enviarán en la solicitud.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán en la solicitud.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la solicitud.</param>
    /// <returns>
    /// Un objeto <see cref="HttpRequestMessage"/> configurado con el método y los datos especificados.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    /// <seealso cref="CreateInvokeMethodRequest"/>
    /// <seealso cref="AddHeaders"/>
    protected virtual HttpRequestMessage CreateRequest<TRequest>(string methodName, TRequest data, HttpMethod httpMethod)
    {
        var result = CreateInvokeMethodRequest(methodName, data, httpMethod);
        AddHeaders(result, methodName);
        return result;
    }

    /// <summary>
    /// Crea un objeto <see cref="HttpRequestMessage"/> para invocar un método específico.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se enviarán en la solicitud.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán en la solicitud. Puede ser null si se utiliza el método GET.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la solicitud (por ejemplo, GET, POST).</param>
    /// <returns>
    /// Un objeto <see cref="HttpRequestMessage"/> configurado para invocar el método especificado.
    /// </returns>
    /// <remarks>
    /// Este método ajusta el nombre del método y, si es necesario, agrega una cadena de consulta basada en los datos proporcionados.
    /// Si los datos son nulos o el método HTTP es GET, se crea una solicitud sin cuerpo.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    /// <seealso cref="HttpMethod"/>
    private HttpRequestMessage CreateInvokeMethodRequest<TRequest>(string methodName, TRequest data, HttpMethod httpMethod)
    {
        methodName = GetMethodName(methodName);
        methodName = GetMethodNameWithQueryString(methodName, data, httpMethod);
        if (data == null || httpMethod == HttpMethod.Get)
            return Client.CreateInvokeMethodRequest(httpMethod, AppId, methodName);
        return Client.CreateInvokeMethodRequest(httpMethod, AppId, methodName, data);
    }

    /// <summary>
    /// Obtiene el nombre del método formateado para la API.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea formatear.</param>
    /// <returns>
    /// Devuelve el nombre del método formateado. Si el nombre del método está vacío, se devuelve una cadena vacía.
    /// Si el nombre del método comienza con "/", se devuelve tal cual.
    /// En caso contrario, se devuelve el nombre del método precedido por "/api/".
    /// </returns>
    /// <remarks>
    /// Este método es virtual, lo que permite que las clases derivadas lo sobreescriban si es necesario.
    /// </remarks>
    protected virtual string GetMethodName(string methodName)
    {
        if (methodName.IsEmpty())
            return string.Empty;
        if (methodName.StartsWith("/"))
            return methodName;
        return $"/api/{methodName}";
    }

    /// <summary>
    /// Obtiene el nombre del método con una cadena de consulta si el método HTTP es GET.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se utilizará para construir la cadena de consulta.</typeparam>
    /// <param name="methodName">El nombre del método al que se le añadirá la cadena de consulta.</param>
    /// <param name="data">Los datos que se convertirán en una cadena de consulta.</param>
    /// <param name="httpMethod">El método HTTP que se está utilizando.</param>
    /// <returns>
    /// El nombre del método con la cadena de consulta añadida si el método HTTP es GET; de lo contrario, devuelve el nombre del método original.
    /// </returns>
    /// <remarks>
    /// Este método es útil para construir URLs que incluyan parámetros de consulta basados en los datos proporcionados.
    /// Si el método HTTP no es GET o si los datos son nulos, se devuelve el nombre del método sin modificaciones.
    /// </remarks>
    protected virtual string GetMethodNameWithQueryString<TRequest>(string methodName, TRequest data, HttpMethod httpMethod)
    {
        if (httpMethod != HttpMethod.Get)
            return methodName;
        if (data == null)
            return methodName;
        return QueryHelpers.AddQueryString(methodName, ToDictionary(data));
    }

    /// <summary>
    /// Convierte un objeto en un diccionario de pares clave-valor.
    /// </summary>
    /// <param name="data">El objeto que se desea convertir en un diccionario.</param>
    /// <returns>Un diccionario que contiene las claves y valores del objeto, omitiendo aquellos valores que son nulos.</returns>
    /// <remarks>
    /// Este método utiliza un helper para realizar la conversión inicial y luego filtra los pares clave-valor,
    /// asegurándose de que solo se incluyan aquellos cuyo valor no sea nulo. Los valores se convierten a cadenas
    /// seguras utilizando el método <see cref="SafeString"/>.
    /// </remarks>
    protected IDictionary<string, string> ToDictionary(object data)
    {
        var result = Util.Helpers.Convert.ToDictionary(data);
        return result.Where(t => t.Value != null).ToDictionary(t => t.Key, t => t.Value.SafeString());
    }

    /// <summary>
    /// Agrega encabezados a un objeto <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="message">El objeto <see cref="HttpRequestMessage"/> al que se le agregarán los encabezados.</param>
    /// <param name="methodName">El nombre del método que está realizando la operación, utilizado para el registro de advertencias.</param>
    /// <remarks>
    /// Este método primero elimina los encabezados existentes y luego intenta agregar nuevos encabezados obtenidos de un método llamado <see cref="GetHeaders"/>.
    /// Si la adición de un encabezado falla en el objeto <see cref="HttpRequestMessage"/>, se intenta agregarlo a los encabezados del contenido de la solicitud.
    /// Si ambos intentos fallan, se registra una advertencia con información sobre el encabezado que no se pudo agregar.
    /// </remarks>
    /// <seealso cref="RemoveHeaders"/>
    /// <seealso cref="GetHeaders"/>
    /// <seealso cref="HttpRequestMessage"/>
    protected virtual void AddHeaders(HttpRequestMessage message, string methodName)
    {
        RemoveHeaders();
        var headers = GetHeaders();
        foreach (var key in headers.Keys)
        {
            bool? success = message.Headers.TryAddWithoutValidation(key, headers[key].ToArray());
            if (success.SafeValue())
                continue;
            message.Content ??= new StringContent(string.Empty);
            success = message.Content.Headers.TryAddWithoutValidation(key, headers[key].ToArray());
            if (success.SafeValue())
                continue;
            Log.LogWarning("Agregar encabezados de solicitud falló, Key:{RequestHeaderKey},AppId:{AppId},MethodName:{MethodName}", key, AppId, methodName);
        }
    }

    /// <summary>
    /// Elimina las claves de encabezado especificadas de las colecciones de encabezados.
    /// </summary>
    /// <remarks>
    /// Este método recorre una lista de claves de encabezado a eliminar y las elimina
    /// de las colecciones de encabezados, claves de encabezado de importación y claves
    /// de encabezado de importación del servicio.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanzará si <c>RemoveHeaderKeys</c> es nulo.
    /// </exception>
    private void RemoveHeaders()
    {
        foreach (var key in RemoveHeaderKeys)
        {
            Headers.Remove(key);
            ImportHeaderKeys.Remove(key);
            Options.ServiceInvocation.ImportHeaderKeys.Remove(key);
        }
    }

    /// <summary>
    /// Obtiene un diccionario de encabezados, combinando los encabezados de importación con los encabezados existentes.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene los encabezados combinados, donde las claves son cadenas y los valores son <see cref="StringValues"/>.
    /// </returns>
    /// <remarks>
    /// Este método primero obtiene los encabezados de importación y luego actualiza el diccionario eliminando las claves existentes 
    /// que también están en los encabezados de importación y añadiendo nuevas claves desde el diccionario de encabezados.
    /// </remarks>
    private IDictionary<string, StringValues> GetHeaders()
    {
        var result = GetImportHeaders();
        foreach (var key in Headers.Keys)
        {
            if (result.ContainsKey(key))
                result.Remove(key);
            result.Add(key, Headers[key]);
        }
        return result;
    }

    /// <summary>
    /// Obtiene los encabezados de importación a partir de las claves definidas en las opciones de invocación de servicio.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene los encabezados de importación, donde la clave es el nombre del encabezado y el valor es un conjunto de valores de tipo <see cref="StringValues"/>.
    /// Si no se encuentran encabezados, se devuelve un diccionario vacío.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si hay claves de encabezado definidas. Si no hay claves, devuelve un diccionario vacío.
    /// Luego, intenta obtener los encabezados de la solicitud web actual. Si no hay encabezados disponibles, también devuelve un diccionario vacío.
    /// Finalmente, para cada clave de encabezado distinta, intenta obtener su valor de los encabezados de la solicitud y lo agrega al diccionario de resultados.
    /// </remarks>
    private IDictionary<string, StringValues> GetImportHeaders()
    {
        var result = new Dictionary<string, StringValues>();
        ImportHeaderKeys.AddRange(Options.ServiceInvocation.ImportHeaderKeys);
        if (ImportHeaderKeys.Count == 0)
            return result;
        var headers = Util.Helpers.Web.Request?.Headers;
        if (headers == null)
            return result;
        foreach (var key in ImportHeaderKeys.Distinct())
        {
            if (headers.TryGetValue(key, out var value))
                result.Add(key, value);
        }
        return result;
    }

    #endregion

    #region InvokeBefore

    /// <summary>
    /// Invoca una acción antes de realizar una llamada de servicio.
    /// </summary>
    /// <param name="methodName">El nombre del método que se va a invocar.</param>
    /// <param name="data">Los datos que se enviarán con la invocación.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="message">El mensaje HTTP que se enviará.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la invocación puede continuar; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método permite ejecutar una acción personalizada antes de la invocación del servicio,
    /// utilizando la configuración especificada en <c>Options.ServiceInvocation.OnBefore</c>.
    /// Si no se ha definido ninguna acción personalizada, se asume que la invocación puede continuar.
    /// </remarks>
    protected async Task<bool> InvokeBefore(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage message)
    {
        if (Options?.ServiceInvocation?.OnBefore != null)
            await Options.ServiceInvocation.OnBefore(new ServiceInvocationArgument(AppId, methodName, httpMethod, data, message));
        if (OnBeforeAction == null)
            return true;
        return OnBeforeAction(message);
    }

    #endregion

    #region FilterRequest

    /// <summary>
    /// Filtra un mensaje de solicitud HTTP aplicando filtros configurados.
    /// </summary>
    /// <param name="request">El mensaje de solicitud HTTP que se va a filtrar.</param>
    /// <returns>
    /// El mensaje de solicitud HTTP filtrado después de aplicar los filtros.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay filtros de solicitud configurados y, si es así, 
    /// los aplica en el orden especificado. Si no hay filtros habilitados, 
    /// se devuelve el mensaje de solicitud original.
    /// </remarks>
    protected virtual HttpRequestMessage FilterRequest(HttpRequestMessage request)
    {
        var context = new RequestContext(request, Util.Helpers.Web.HttpContext);
        var requestFilters = Options?.ServiceInvocation?.RequestFilters;
        if (requestFilters == null || requestFilters.Count == 0)
            return request;
        foreach (var filter in requestFilters.Where(t => t is { Enabled: true }).OrderBy(t => t.Order))
            filter.Handle(context);
        return context.RequestMessage;
    }

    #endregion

    #region FilterResponse

    /// <summary>
    /// Filtra la respuesta HTTP utilizando los filtros de respuesta configurados.
    /// </summary>
    /// <param name="response">La respuesta HTTP que se va a filtrar.</param>
    /// <returns>La respuesta HTTP filtrada.</returns>
    /// <remarks>
    /// Este método verifica si hay filtros de respuesta habilitados y los aplica en el orden especificado.
    /// Si no hay filtros disponibles, se devuelve la respuesta original.
    /// </remarks>
    /// <seealso cref="ResponseContext"/>
    /// <seealso cref="Util.Helpers.Web.HttpContext"/>
    protected virtual HttpResponseMessage FilterResponse(HttpResponseMessage response)
    {
        var context = new ResponseContext(response, Util.Helpers.Web.HttpContext);
        var responseFilters = Options?.ServiceInvocation?.ResponseFilters;
        if (responseFilters == null || responseFilters.Count == 0)
            return response;
        foreach (var filter in responseFilters.Where(t => t is { Enabled: true }).OrderBy(t => t.Order))
            filter.Handle(context);
        return context.ResponseMessage;
    }

    #endregion

    #region ToResult

    /// <summary>
    /// Convierte la respuesta HTTP en un resultado de servicio.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera del servicio.</typeparam>
    /// <param name="response">La respuesta HTTP que se va a convertir.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="ServiceResult{TResponse}"/> que contiene el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método determina si debe desempaquetar el resultado basado en la propiedad <c>IsUnpackResult</c>.
    /// Si <c>IsUnpackResult</c> es verdadero, se llamará al método <c>ToUnpackResult</c>; de lo contrario, se llamará a <c>ToNotUnpackResult</c>.
    /// </remarks>
    protected virtual async Task<ServiceResult<TResponse>> ToResult<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (IsUnpackResult)
            return await ToUnpackResult<TResponse>(response, cancellationToken);
        return await ToNotUnpackResult<TResponse>(response, cancellationToken);
    }

    /// <summary>
    /// Desempaqueta el resultado de una respuesta HTTP en un objeto de tipo <see cref="ServiceResult{TResponse}"/>.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera recibir.</typeparam>
    /// <param name="response">La respuesta HTTP que se va a procesar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Un objeto <see cref="ServiceResult{TResponse}"/> que contiene el resultado de la operación.
    /// </returns>
    /// <remarks>
    /// Este método asegura que la respuesta HTTP fue exitosa antes de intentar leer el contenido. 
    /// Si se ha definido una acción personalizada para procesar el resultado, se ejecutará dicha acción; 
    /// de lo contrario, se leerá directamente el contenido JSON de la respuesta.
    /// </remarks>
    /// <exception cref="HttpRequestException">
    /// Se lanza si la respuesta HTTP no es exitosa.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si el resultado es nulo después de la acción personalizada.
    /// </exception>
    /// <seealso cref="ServiceResult{TResponse}"/>
    protected virtual async Task<ServiceResult<TResponse>> ToUnpackResult<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();
        if (OnResultAction == null)
            return await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>(Client.JsonSerializerOptions, cancellationToken);
        var objResult = await OnResultAction(response, Client.JsonSerializerOptions, cancellationToken);
        var result = objResult as ServiceResult<TResponse>;
        result!.CheckNull(nameof(result));
        return result;
    }

    /// <summary>
    /// Convierte el resultado de una respuesta HTTP en un objeto de tipo <see cref="ServiceResult{TResponse}"/> sin deserializar el contenido.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera recibir.</typeparam>
    /// <param name="response">La respuesta HTTP que se desea procesar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="ServiceResult{TResponse}"/> que contiene el código de estado y, opcionalmente, los datos deserializados de la respuesta.
    /// </returns>
    /// <remarks>
    /// Este método verifica el código de estado de la respuesta y, si no es exitoso, lee el mensaje de error del contenido de la respuesta.
    /// Si el código de estado es exitoso y no se proporciona una acción de resultado, se deserializa el contenido JSON en el tipo especificado.
    /// Si se proporciona una acción de resultado, se utiliza para procesar el contenido de la respuesta.
    /// </remarks>
    /// <seealso cref="ServiceResult{TResponse}"/>
    protected virtual async Task<ServiceResult<TResponse>> ToNotUnpackResult<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var result = new ServiceResult<TResponse>
        {
            Code = GetNotUnpackStateCode(response)
        };
        if (result.Code != StateCode.Ok)
        {
            result.Message = await response.Content.ReadAsStringAsync(cancellationToken);
            return result;
        }
        if (OnResultAction == null)
        {
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            if (json.IsEmpty())
                return result;
            result.Data = Util.Helpers.Json.ToObject<TResponse>(json, Client.JsonSerializerOptions);
            return result;
        }
        var content = await OnResultAction(response, Client.JsonSerializerOptions, cancellationToken);
        result.Data = Util.Helpers.Convert.To<TResponse>(content);
        return result;
    }

    /// <summary>
    /// Obtiene el código de estado correspondiente a la respuesta HTTP, 
    /// sin descomprimir el contenido de la respuesta.
    /// </summary>
    /// <param name="response">La respuesta HTTP de la que se extraerá el código de estado.</param>
    /// <returns>
    /// Un código de estado que representa el resultado de la respuesta HTTP.
    /// Puede ser uno de los siguientes: 
    /// <list type="bullet">
    /// <item><description><see cref="StateCode.Ok"/> si la respuesta es exitosa.</description></item>
    /// <item><description><see cref="StateCode.Unauthorized"/> si la respuesta indica que el acceso está denegado.</description></item>
    /// <item><description><see cref="StateCode.Fail"/> para cualquier otro estado de error.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// Este método evalúa el estado de la respuesta y devuelve un código de estado 
    /// predefinido basado en el resultado de la evaluación.
    /// </remarks>
    protected virtual string GetNotUnpackStateCode(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return StateCode.Ok;
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            return StateCode.Unauthorized;
        return StateCode.Fail;
    }

    #endregion

    #region InvokeAfter

    /// <summary>
    /// Invoca un manejador específico después de realizar una operación asíncrona,
    /// basado en el estado del resultado del servicio.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta esperada del servicio.</typeparam>
    /// <param name="methodName">El nombre del método que se está invocando.</param>
    /// <param name="data">Los datos que se pasan al método.</param>
    /// <param name="httpMethod">El método HTTP utilizado para la solicitud.</param>
    /// <param name="request">El objeto <see cref="HttpRequestMessage"/> que representa la solicitud HTTP.</param>
    /// <param name="response">El objeto <see cref="HttpResponseMessage"/> que representa la respuesta HTTP.</param>
    /// <param name="result">El resultado del servicio que contiene la información sobre el estado y los datos.</param>
    /// <remarks>
    /// Este método evalúa el estado del resultado del servicio y llama al manejador correspondiente
    /// para manejar el resultado de acuerdo con el estado: éxito, fallo o no autorizado.
    /// </remarks>
    /// <seealso cref="ServiceResult{TResponse}"/>
    /// <seealso cref="ServiceState"/>
    protected virtual async Task InvokeAfter<TResponse>(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage request, HttpResponseMessage response, ServiceResult<TResponse> result)
    {
        var state = ToState(result);
        if (state == ServiceState.Ok)
        {
            await SuccessHandlerAsync(methodName, data, httpMethod, request, response, result.Data);
            return;
        }
        if (state == ServiceState.Fail)
        {
            await FailHandlerAsync(methodName, data, httpMethod, request, response, null, result?.Message);
            return;
        }
        if (state == ServiceState.Unauthorized)
        {
            await UnauthorizedHandlerAsync(methodName, data, httpMethod, request, response);
            return;
        }
    }

    #endregion

    #region ToState

    /// <summary>
    /// Convierte un resultado de servicio en un estado de servicio.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de la respuesta del servicio.</typeparam>
    /// <param name="result">El resultado del servicio que se va a convertir.</param>
    /// <returns>El estado del servicio correspondiente al resultado proporcionado.</returns>
    /// <remarks>
    /// Este método verifica si el resultado es nulo y, en caso afirmativo, devuelve el estado de fallo.
    /// Si hay una acción de estado definida, se ejecuta y se devuelve el estado correspondiente.
    /// De lo contrario, se evalúa el código del resultado y se devuelve el estado correspondiente.
    /// </remarks>
    /// <seealso cref="ServiceResult{TResponse}"/>
    /// <seealso cref="ServiceState"/>
    /// <seealso cref="StateCode"/>
    protected virtual ServiceState ToState<TResponse>(ServiceResult<TResponse> result)
    {
        if (result == null)
            return ServiceState.Fail;
        if (OnStateAction != null)
            return OnStateAction(result.Code);
        switch (result.Code)
        {
            case StateCode.Ok:
                return ServiceState.Ok;
            case StateCode.Unauthorized:
                return ServiceState.Unauthorized;
            default:
                return ServiceState.Fail;
        }
    }

    #endregion

    #region SuccessHandlerAsync

    /// <summary>
    /// Maneja la lógica a ejecutar cuando una invocación de servicio es exitosa.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera del servicio.</typeparam>
    /// <param name="methodName">El nombre del método que se invocó.</param>
    /// <param name="data">Los datos que se enviaron en la solicitud.</param>
    /// <param name="httpMethod">El método HTTP utilizado para la solicitud.</param>
    /// <param name="request">El objeto <see cref="HttpRequestMessage"/> que representa la solicitud HTTP.</param>
    /// <param name="response">El objeto <see cref="HttpResponseMessage"/> que representa la respuesta HTTP.</param>
    /// <param name="result">El resultado de la invocación del servicio.</param>
    /// <remarks>
    /// Este método se invoca después de que una llamada al servicio ha sido completada con éxito.
    /// Se registran los detalles de la invocación y se ejecutan las acciones configuradas para manejar el éxito.
    /// </remarks>
    /// <seealso cref="ServiceInvocationArgument"/>
    protected virtual async Task SuccessHandlerAsync<TResponse>(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage request, HttpResponseMessage response, TResponse result)
    {
        Log.LogTrace("Llamada al servicio exitosa, AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        if (Options?.ServiceInvocation?.OnSuccess != null)
            await Options.ServiceInvocation.OnSuccess(new ServiceInvocationArgument(AppId, methodName, httpMethod, data, request, response, result));
        if (OnSuccessAction == null)
            return;
        await OnSuccessAction(request, response, result);
    }

    #endregion

    #region FailHandlerAsync

    /// <summary>
    /// Maneja los errores que ocurren durante la invocación de un servicio.
    /// Registra advertencias o errores dependiendo de la presencia de una excepción y ejecuta acciones de fallo definidas por el usuario.
    /// </summary>
    /// <param name="methodName">El nombre del método que se intentó invocar.</param>
    /// <param name="data">Los datos que se pasaron a la invocación del servicio.</param>
    /// <param name="httpMethod">El método HTTP utilizado para la invocación.</param>
    /// <param name="request">El objeto <see cref="HttpRequestMessage"/> que representa la solicitud HTTP.</param>
    /// <param name="response">El objeto <see cref="HttpResponseMessage"/> que representa la respuesta HTTP.</param>
    /// <param name="exception">La excepción que se lanzó durante la invocación, si existe.</param>
    /// <param name="message">Un mensaje adicional que describe el fallo.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar un manejo de errores personalizado.
    /// </remarks>
    /// <exception cref="InvocationException">Se lanza si ocurre una excepción durante la invocación del servicio.</exception>
    /// <exception cref="Warning">Se lanza si no hay excepción pero se requiere advertir sobre el fallo.</exception>
    /// <seealso cref="ServiceInvocationArgument"/>
    protected virtual async Task FailHandlerAsync(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage request, HttpResponseMessage response, Exception exception, string message)
    {
        if (exception == null)
            Log.LogWarning("Llamada al servicio fallida, AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        else
        {
            Log.LogError(exception, "Llamada al servicio fallida, AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        }
        if (Options?.ServiceInvocation?.OnFail != null)
            await Options.ServiceInvocation.OnFail(new ServiceInvocationArgument(AppId, methodName, httpMethod, data, request, response, null, exception, message));
        if (OnFailAction != null)
        {
            await OnFailAction(request, response, exception);
            return;
        }
        if (exception != null)
            throw new InvocationException(AppId, methodName, exception, response);
        throw new Warning(message);
    }

    #endregion

    #region UnauthorizedHandlerAsync

    /// <summary>
    /// Maneja la situación en la que se detecta un intento no autorizado de acceso a un servicio.
    /// </summary>
    /// <param name="methodName">El nombre del método que se intentó invocar.</param>
    /// <param name="data">Los datos asociados a la invocación del servicio.</param>
    /// <param name="httpMethod">El método HTTP utilizado para la solicitud.</param>
    /// <param name="request">El objeto de solicitud HTTP que se está procesando.</param>
    /// <param name="response">El objeto de respuesta HTTP que se generará.</param>
    /// <returns>Una tarea asincrónica que representa la operación en curso.</returns>
    /// <remarks>
    /// Este método registra una advertencia y ejecuta acciones específicas si se configura un manejador para situaciones no autorizadas.
    /// Si no se proporciona un manejador, se lanzará una excepción de advertencia con un mensaje de no autorizado.
    /// </remarks>
    /// <exception cref="Warning">Se lanza una excepción de tipo Warning si no se maneja la situación no autorizada.</exception>
    protected virtual async Task UnauthorizedHandlerAsync(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage request, HttpResponseMessage response)
    {
        Log.LogWarning("Llamada a un servicio no autorizado, AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        if (Options?.ServiceInvocation?.OnUnauthorized != null)
            await Options.ServiceInvocation.OnUnauthorized(new ServiceInvocationArgument(AppId, methodName, httpMethod, data, request, response));
        if (OnUnauthorizedAction != null)
        {
            await OnUnauthorizedAction(request, response);
            return;
        }
        throw new Warning(R.UnauthorizedMessage, code: StateCode.Unauthorized);
    }

    #endregion

    #region CompleteHandlerAsync

    /// <summary>
    /// Maneja la finalización de una invocación de servicio.
    /// </summary>
    /// <param name="methodName">El nombre del método que se invocó.</param>
    /// <param name="data">Los datos asociados a la invocación del servicio.</param>
    /// <param name="httpMethod">El método HTTP utilizado para la invocación.</param>
    /// <param name="request">El objeto <see cref="HttpRequestMessage"/> que representa la solicitud HTTP.</param>
    /// <param name="response">El objeto <see cref="HttpResponseMessage"/> que representa la respuesta HTTP.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método se invoca al completar una llamada a un servicio. 
    /// Si se ha configurado un manejador de finalización en las opciones, se ejecutará.
    /// Además, si se ha definido una acción de finalización personalizada, también se ejecutará.
    /// </remarks>
    /// <seealso cref="ServiceInvocationArgument"/>
    protected virtual async Task CompleteHandlerAsync(string methodName, object data, HttpMethod httpMethod, HttpRequestMessage request, HttpResponseMessage response)
    {
        Log.LogTrace("Llamada al servicio completada, AppId:{AppId},MethodName:{MethodName}", AppId, methodName);
        if (Options?.ServiceInvocation?.OnComplete != null)
            await Options.ServiceInvocation.OnComplete(new ServiceInvocationArgument(AppId, methodName, httpMethod, data, request, response));
        if (OnCompleteAction == null)
            return;
        await OnCompleteAction(request, response);
    }

    #endregion
}