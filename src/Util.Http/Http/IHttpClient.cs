namespace Util.Http; 

/// <summary>
/// Define una interfaz para un cliente HTTP que se implementa como una dependencia singleton.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para abstraer las operaciones de cliente HTTP, permitiendo la inyección de dependencias y facilitando las pruebas unitarias.
/// </remarks>
public interface IHttpClient : ISingletonDependency {
    /// <summary>
    /// Obtiene una solicitud HTTP para la URL especificada.
    /// </summary>
    /// <param name="url">La URL desde la cual se realizará la solicitud HTTP.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{T}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método permite realizar una solicitud HTTP a la URL proporcionada y devuelve un objeto que implementa la interfaz <see cref="IHttpRequest{T}"/>.
    /// Asegúrese de que la URL sea válida y accesible para evitar excepciones durante la ejecución.
    /// </remarks>
    IHttpRequest<string> Get(string url);
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada con los parámetros de consulta proporcionados.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <param name="queryString">Un objeto que contiene los parámetros de consulta que se agregarán a la URL.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="IHttpRequest{T}"/> con el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método es útil para obtener datos de un recurso web. Asegúrese de que la URL sea válida y que el servidor pueda manejar la solicitud GET.
    /// </remarks>
    IHttpRequest<string> Get(string url, object queryString);
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada y devuelve el resultado deserializado en el tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera recibir tras la deserialización de la respuesta.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que contiene el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método es genérico y permite especificar cualquier tipo de clase como resultado, siempre que cumpla con las restricciones de tipo.
    /// Asegúrese de que la URL proporcionada sea válida y accesible.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si la URL es nula o vacía.</exception>
    /// <exception cref="HttpRequestException">Se lanza si ocurre un error durante la solicitud HTTP.</exception>
    IHttpRequest<TResult> Get<TResult>(string url) where TResult : class;
    /// <summary>
    /// Realiza una solicitud HTTP GET a la URL especificada y devuelve el resultado deserializado en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud HTTP. Debe ser una clase.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud GET.</param>
    /// <param name="queryString">Un objeto que representa los parámetros de consulta que se agregarán a la URL.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TResult"/> que contiene el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método permite realizar solicitudes GET de manera genérica, facilitando la obtención de datos desde un servicio web.
    /// Asegúrese de que el tipo <typeparamref name="TResult"/> sea compatible con la respuesta esperada del servicio.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Get<TResult>( string url, object queryString ) where TResult : class;
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada y devuelve la respuesta como una cadena.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{string}"/> que representa la respuesta de la solicitud.</returns>
    /// <remarks>
    /// Este método es útil para interactuar con servicios web que requieren el envío de datos mediante el método POST.
    /// Asegúrese de que la URL proporcionada sea válida y que el servicio esté disponible.
    /// </remarks>
    IHttpRequest<string> Post(string url);
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <param name="content">El contenido que se enviará en el cuerpo de la solicitud.</param>
    /// <returns>Un objeto que implementa <see cref="IHttpRequest{T}"/> con el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método se utiliza para enviar datos al servidor y recibir una respuesta.
    /// Asegúrese de que la URL sea válida y que el contenido esté en el formato adecuado.
    /// </remarks>
    IHttpRequest<string> Post(string url, object content);
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada y devuelve el resultado deserializado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera recibir tras la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TResult"/> que representa la respuesta del servidor.</returns>
    /// <remarks>
    /// Este método utiliza el tipo genérico <typeparamref name="TResult"/> para permitir la deserialización de la respuesta en el tipo deseado.
    /// Asegúrese de que el tipo proporcionado sea compatible con el formato de respuesta del servidor.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Post<TResult>( string url ) where TResult : class;
    /// <summary>
    /// Envía una solicitud HTTP POST a la URL especificada y devuelve el resultado deserializado en el tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud POST. Debe ser una clase.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud POST.</param>
    /// <param name="content">El contenido a enviar en el cuerpo de la solicitud.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TResult"/> que representa la respuesta deserializada.</returns>
    /// <remarks>
    /// Este método se utiliza para realizar solicitudes POST a un servicio web y manejar la respuesta de manera tipada.
    /// Asegúrese de que el tipo <typeparamref name="TResult"/> tenga un constructor sin parámetros y propiedades públicas 
    /// que coincidan con la estructura de la respuesta esperada.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Post<TResult>( string url, object content ) where TResult : class;
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada y devuelve una respuesta de tipo cadena.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{string}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método es útil para actualizar recursos en el servidor mediante el método PUT.
    /// Asegúrese de que la URL proporcionada sea válida y accesible.
    /// </remarks>
    IHttpRequest<string> Put(string url);
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <param name="content">El contenido que se enviará en el cuerpo de la solicitud.</param>
    /// <returns>Un objeto que implementa <see cref="IHttpRequest{T}"/> con el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método se utiliza para actualizar recursos en el servidor especificado por la URL.
    /// Asegúrese de que el servidor esté configurado para manejar solicitudes PUT.
    /// </remarks>
    IHttpRequest<string> Put(string url, object content);
    /// <summary>
    /// Realiza una solicitud HTTP PUT a la URL especificada y devuelve el resultado deserializado en el tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera recibir tras la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TResult"/> que representa el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método es genérico y permite especificar el tipo de resultado esperado, lo que facilita el manejo de diferentes tipos de datos.
    /// Asegúrese de que el tipo <typeparamref name="TResult"/> sea una clase, ya que se requiere que sea un tipo de referencia.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Put<TResult>( string url ) where TResult : class;
    /// <summary>
    /// Envía una solicitud HTTP PUT a la URL especificada con el contenido proporcionado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud PUT.</param>
    /// <param name="content">El contenido que se enviará en la solicitud.</param>
    /// <returns>Un objeto de tipo <typeparamref name="TResult"/> que representa la respuesta de la solicitud.</returns>
    /// <remarks>
    /// Este método es genérico y permite especificar el tipo de resultado esperado, 
    /// lo que facilita la deserialización de la respuesta en el tipo deseado.
    /// Asegúrese de que el tipo <typeparamref name="TResult"/> sea una clase.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Put<TResult>( string url, object content ) where TResult : class;
    /// <summary>
    /// Realiza una solicitud HTTP DELETE a la URL especificada.
    /// </summary>
    /// <param name="url">La URL a la que se enviará la solicitud DELETE.</param>
    /// <returns>Un objeto que implementa <see cref="IHttpRequest{T}"/> con el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método se utiliza para eliminar un recurso en el servidor especificado por la URL.
    /// Asegúrese de que la URL sea válida y que el recurso exista antes de realizar la solicitud.
    /// </remarks>
    IHttpRequest<string> Delete(string url);
    /// <summary>
    /// Realiza una solicitud HTTP DELETE a la URL especificada y devuelve el resultado deserializado en el tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud DELETE. Debe ser una clase.</typeparam>
    /// <param name="url">La URL a la que se enviará la solicitud DELETE.</param>
    /// <returns>Un objeto del tipo <typeparamref name="TResult"/> que representa el resultado de la solicitud.</returns>
    /// <remarks>
    /// Este método es útil para eliminar recursos en un servidor a través de una API RESTful.
    /// Asegúrese de que la URL proporcionada sea válida y que el servidor acepte solicitudes DELETE.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Delete<TResult>( string url ) where TResult : class;
}