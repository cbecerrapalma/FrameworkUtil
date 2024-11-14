namespace Util.Microservices;

/// <summary>
/// Define una interfaz para la invocación de servicios.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IServiceInvocationBase{T}"/> y <see cref="ITransientDependency"/>.
/// Se utiliza para definir comportamientos específicos de invocación de servicios en el contexto de una aplicación.
/// </remarks>
/// <typeparam name="T">El tipo de servicio que se invocará.</typeparam>
public interface IServiceInvocation : IServiceInvocationBase<IServiceInvocation>,ITransientDependency {
    /// <summary>
    /// Intercepta la invocación de un servicio antes de que se realice la solicitud.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y devuelve un valor booleano que indica si se debe proceder con la invocación.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> que permite encadenar otras configuraciones o acciones.</returns>
    /// <remarks>
    /// Este método permite a los desarrolladores agregar lógica personalizada que se ejecutará antes de que se realice la solicitud HTTP.
    /// Puede ser útil para validar condiciones, modificar la solicitud o registrar información.
    /// </remarks>
    /// <seealso cref="IServiceInvocation"/>
    IServiceInvocation OnBefore( Func<HttpRequestMessage, bool> action );
    /// <summary>
    /// Interfaz que define un método para manejar el resultado de una invocación de servicio.
    /// </summary>
    IServiceInvocation OnResult( Func<HttpResponseMessage, JsonSerializerOptions, CancellationToken, Task<object>> action );
    /// <summary>
    /// Interfaz que representa una invocación de servicio.
    /// </summary>
    IServiceInvocation OnAfter( Action<HttpResponseMessage> action );
    /// <summary>
    /// Define un método que se invoca cuando una operación se completa con éxito.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera de la operación exitosa.</typeparam>
    /// <param name="action">Una función que se ejecuta al completarse la operación, la cual recibe un <see cref="HttpRequestMessage"/>, un <see cref="HttpResponseMessage"/> y el resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> que permite encadenar más configuraciones o acciones.</returns>
    /// <remarks>
    /// Este método es útil para manejar la lógica que debe ejecutarse después de recibir una respuesta exitosa de una solicitud HTTP.
    /// </remarks>
    /// <seealso cref="IServiceInvocation"/>
    IServiceInvocation OnSuccess<TResult>( Func<HttpRequestMessage, HttpResponseMessage, TResult, Task> action );
    /// <summary>
    /// Define un método para manejar fallos durante la invocación de servicios.
    /// </summary>
    /// <param name="action">Una función que se ejecutará en caso de fallo, la cual recibe un <see cref="HttpRequestMessage"/>, un <see cref="HttpResponseMessage"/> y una <see cref="Exception"/> como parámetros, y devuelve una tarea asíncrona.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> que permite encadenar otras configuraciones.</returns>
    /// <remarks>
    /// Este método permite personalizar el comportamiento en caso de que ocurra un error durante la invocación de un servicio.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    /// <seealso cref="HttpResponseMessage"/>
    /// <seealso cref="Exception"/>
    IServiceInvocation OnFail( Func<HttpRequestMessage, HttpResponseMessage, Exception, Task> action );
    /// <summary>
    /// Define un método que se invoca cuando se recibe una respuesta no autorizada.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/> y devuelve una tarea asíncrona.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> que permite encadenar otras configuraciones.</returns>
    /// <remarks>
    /// Este método permite manejar situaciones en las que se recibe un código de estado HTTP 401 (No autorizado).
    /// Se puede utilizar para implementar lógica personalizada, como redirigir al usuario a una página de inicio de sesión
    /// o mostrar un mensaje de error.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    /// <seealso cref="HttpResponseMessage"/>
    IServiceInvocation OnUnauthorized( Func<HttpRequestMessage, HttpResponseMessage, Task> action );
    /// <summary>
    /// Define un método que se invoca cuando se completa una invocación de servicio.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpRequestMessage"/> y un <see cref="HttpResponseMessage"/>, y devuelve una tarea que representa la operación asincrónica.</param>
    /// <returns>Una instancia de <see cref="IServiceInvocation"/> que representa la invocación del servicio.</returns>
    /// <remarks>
    /// Este método permite registrar una acción que se ejecutará al finalizar la invocación del servicio,
    /// lo que permite realizar operaciones adicionales como el manejo de respuestas o la gestión de errores.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    /// <seealso cref="HttpResponseMessage"/>
    /// <seealso cref="IServiceInvocation"/>
    IServiceInvocation OnComplete( Func<HttpRequestMessage, HttpResponseMessage, Task> action );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado por su nombre.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite la invocación dinámica de métodos en tiempo de ejecución. 
    /// Asegúrese de que el método especificado sea accesible y que su firma sea compatible 
    /// con los parámetros esperados.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="methodName"/> es null.</exception>
    /// <exception cref="InvalidOperationException">Se lanza si el método no se puede encontrar o invocar.</exception>
    /// <seealso cref="CancellationToken"/>
    Task InvokeAsync( string methodName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado utilizando el método HTTP indicado.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es de tipo <c>Task</c>.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones a métodos de forma asíncrona, facilitando la integración con servicios web o APIs.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la invocación.
    /// </remarks>
    /// <seealso cref="HttpMethod"/>
    Task InvokeAsync( string methodName, HttpMethod httpMethod, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado por su nombre.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se pasarán al método como argumento.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método permite la invocación dinámica de métodos en tiempo de ejecución,
    /// lo que puede ser útil en escenarios donde los métodos a invocar no son conocidos
    /// en tiempo de compilación.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="methodName"/> es null.</exception>
    /// <exception cref="InvalidOperationException">Se lanza si el método especificado no se puede encontrar o invocar.</exception>
    Task InvokeAsync( string methodName,object data, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca un método de forma asíncrona utilizando el nombre del método, los datos a enviar y el método HTTP especificado.
    /// </summary>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación (por ejemplo, GET, POST).</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea puede contener la respuesta del método invocado.</returns>
    /// <remarks>
    /// Este método permite la invocación de métodos de manera asíncrona, facilitando la comunicación con servicios web o APIs.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la invocación.
    /// </remarks>
    /// <seealso cref="Task"/>
    /// <seealso cref="HttpMethod"/>
    Task InvokeAsync( string methodName, object data, HttpMethod httpMethod, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado por su nombre y devuelve una respuesta de tipo <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera recibir al invocar el método.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <see cref="CancellationToken.None"/>.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/>.</returns>
    /// <remarks>
    /// Este método permite invocar métodos de forma dinámica y manejar la respuesta de manera asíncrona.
    /// Asegúrese de que el nombre del método proporcionado sea correcto y que el método sea accesible.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<TResponse> InvokeAsync<TResponse>( string methodName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado utilizando el método HTTP indicado.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera recibir tras la invocación del método.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación (por ejemplo, GET, POST).</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/> que contiene la respuesta del método invocado.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones a servicios web o APIs de manera asíncrona, facilitando la integración con sistemas externos.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la invocación.
    /// </remarks>
    /// <seealso cref="Task{T}"/>
    Task<TResponse> InvokeAsync<TResponse>( string methodName, HttpMethod httpMethod, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca un método de forma asíncrona y devuelve una tarea que representa la operación.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de respuesta que se espera recibir tras la invocación del método.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="cancellationToken">Un token que puede utilizarse para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/>.</returns>
    /// <remarks>
    /// Este método permite la invocación de métodos de manera asíncrona, facilitando la ejecución de operaciones que pueden tardar en completarse sin bloquear el hilo de ejecución actual.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<TResponse> InvokeAsync<TResponse>( string methodName, object data, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca de manera asíncrona un método especificado y devuelve una respuesta de tipo <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">El tipo de la respuesta esperada del método invocado.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se enviarán al método invocado.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene la respuesta del tipo <typeparamref name="TResponse"/>.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones a métodos de forma asíncrona, facilitando la integración con servicios web o APIs.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la invocación.
    /// </remarks>
    /// <seealso cref="Task{TResponse}"/>
    Task<TResponse> InvokeAsync<TResponse>( string methodName, object data, HttpMethod httpMethod, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca un método de forma asíncrona y devuelve una respuesta.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se envían al método.</typeparam>
    /// <typeparam name="TResponse">El tipo de datos que se reciben como respuesta del método.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se envían al método.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <typeparamref name="TResponse"/>.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones a métodos de forma asíncrona, lo que es útil para operaciones que pueden tardar tiempo en completarse,
    /// permitiendo que la aplicación siga respondiendo mientras se espera la respuesta.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<TResponse> InvokeAsync<TRequest,TResponse>( string methodName, TRequest data, CancellationToken cancellationToken = default );
    /// <summary>
    /// Invoca un método de forma asíncrona y devuelve una respuesta de tipo TResponse.
    /// </summary>
    /// <typeparam name="TRequest">El tipo de datos que se envían en la solicitud.</typeparam>
    /// <typeparam name="TResponse">El tipo de datos que se reciben en la respuesta.</typeparam>
    /// <param name="methodName">El nombre del método que se desea invocar.</param>
    /// <param name="data">Los datos que se envían como parte de la solicitud.</param>
    /// <param name="httpMethod">El método HTTP que se utilizará para la invocación (por ejemplo, GET, POST).</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo TResponse.</returns>
    /// <remarks>
    /// Este método permite realizar invocaciones a servicios web o API de manera asíncrona,
    /// facilitando la integración y el manejo de solicitudes y respuestas.
    /// </remarks>
    /// <seealso cref="HttpMethod"/>
    Task<TResponse> InvokeAsync<TRequest, TResponse>( string methodName, TRequest data, HttpMethod httpMethod, CancellationToken cancellationToken = default );
}