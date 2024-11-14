namespace Util.Http; 

/// <summary>
/// Define una interfaz para representar una solicitud HTTP.
/// </summary>
public interface IHttpRequest {
}

/// <summary>
/// Interfaz que representa una solicitud HTTP que devuelve un resultado de tipo específico.
/// </summary>
/// <typeparam name="TResult">El tipo de resultado que se espera de la solicitud HTTP. Debe ser una clase.</typeparam>
/// <remarks>
/// Esta interfaz hereda de <see cref="IHttpRequest"/> y se utiliza para definir solicitudes HTTP que 
/// requieren un tipo de resultado específico, permitiendo así una mayor flexibilidad y tipado seguro 
/// en las operaciones que manejan respuestas HTTP.
/// </remarks>
public interface IHttpRequest<TResult> : IHttpRequest where TResult : class {
    /// <summary>
    /// Establece un tiempo de espera para la solicitud HTTP.
    /// </summary>
    /// <param name="timeout">El tiempo de espera que se aplicará a la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el tiempo de espera configurado.</returns>
    /// <remarks>
    /// Este método permite especificar un límite de tiempo para la ejecución de la solicitud HTTP. 
    /// Si la solicitud no se completa dentro del tiempo especificado, se generará una excepción.
    /// </remarks>
    IHttpRequest<TResult> Timeout( TimeSpan timeout );
    /// <summary>
    /// Obtiene una instancia de <see cref="IHttpRequest{TResult}"/> asociada al nombre especificado.
    /// </summary>
    /// <param name="name">El nombre que se utilizará para identificar la solicitud HTTP.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para realizar la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método permite crear solicitudes HTTP personalizadas basadas en el nombre proporcionado.
    /// Asegúrese de que el nombre sea válido y esté registrado en el sistema.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> HttpClientName( string name );
    /// <summary>
    /// Establece la dirección base para las solicitudes HTTP.
    /// </summary>
    /// <param name="baseAddress">La dirección base que se utilizará para las solicitudes.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la dirección base configurada.</returns>
    /// <remarks>
    /// Esta función permite definir una URL base que se usará como prefijo para todas las solicitudes que se realicen a través de esta instancia.
    /// Asegúrese de que la dirección base sea una URL válida.
    /// </remarks>
    IHttpRequest<TResult> BaseAddress(string baseAddress);
    /// <summary>
    /// Establece la codificación para la solicitud HTTP.
    /// </summary>
    /// <param name="encoding">La cadena que representa la codificación que se desea aplicar a la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la codificación especificada.</returns>
    /// <remarks>
    /// Este método permite configurar la codificación de la solicitud, lo que puede ser útil
    /// para asegurar que los datos se envían en el formato correcto, especialmente cuando se
    /// manejan caracteres especiales o se trabaja con diferentes lenguajes.
    /// </remarks>
    IHttpRequest<TResult> Encoding( string encoding );
    /// <summary>
    /// Establece la codificación que se utilizará para el contenido de la solicitud HTTP.
    /// </summary>
    /// <param name="encoding">La codificación que se aplicará al contenido de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la codificación especificada.</returns>
    /// <remarks>
    /// Utilizar esta función permite definir cómo se debe interpretar el contenido de la solicitud
    /// al enviarlo al servidor. Es importante seleccionar la codificación adecuada para asegurar
    /// que los datos se envíen correctamente.
    /// </remarks>
    IHttpRequest<TResult> Encoding( Encoding encoding );
    /// <summary>
    /// Configura el token de portador (Bearer) para la solicitud HTTP.
    /// </summary>
    /// <param name="token">El token de portador que se utilizará para la autenticación en la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> configurada con el token de portador.</returns>
    /// <remarks>
    /// Este método es útil para autenticar solicitudes a servicios que requieren un token de portador en el encabezado de autorización.
    /// </remarks>
    IHttpRequest<TResult> BearerToken(string token);
    /// <summary>
    /// Establece el tipo de contenido para la solicitud HTTP.
    /// </summary>
    /// <param name="contentType">El tipo de contenido que se va a establecer para la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el tipo de contenido actualizado.</returns>
    /// <remarks>
    /// Este método permite especificar el tipo de contenido que se enviará en la solicitud HTTP,
    /// lo que es útil para indicar el formato de los datos que se están enviando al servidor.
    /// </remarks>
    IHttpRequest<TResult> ContentType( HttpContentType contentType );
    /// <summary>
    /// Establece el tipo de contenido para la solicitud HTTP.
    /// </summary>
    /// <param name="contentType">El tipo de contenido que se va a establecer, como "application/json" o "text/xml".</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el tipo de contenido configurado.</returns>
    /// <remarks>
    /// Este método permite especificar el tipo de contenido que se enviará en la solicitud HTTP.
    /// Asegúrese de que el tipo de contenido sea compatible con el servidor que recibe la solicitud.
    /// </remarks>
    IHttpRequest<TResult> ContentType( string contentType );
    /// <summary>
    /// Representa una operación que completa una solicitud HTTP y devuelve un resultado de tipo especificado.
    /// </summary>
    /// <param name="option">La opción que determina cómo se completa la solicitud HTTP.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP completada.</returns>
    /// <remarks>
    /// Este método permite especificar diferentes opciones de finalización para la solicitud HTTP,
    /// lo que puede afectar el comportamiento de la operación y el resultado devuelto.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> HttpCompletion( HttpCompletionOption option );
    /// <summary>
    /// Crea una solicitud HTTP que utiliza un certificado para la autenticación.
    /// </summary>
    /// <param name="path">La ruta del certificado que se utilizará para la autenticación.</param>
    /// <param name="password">La contraseña del certificado, si es necesario.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> configurada con el certificado.</returns>
    /// <remarks>
    /// Este método es útil para situaciones en las que se requiere autenticación basada en certificados,
    /// como en servicios web seguros o APIs que requieren un nivel adicional de seguridad.
    /// Asegúrese de que la ruta del certificado sea válida y que la contraseña proporcionada sea correcta.
    /// </remarks>
    IHttpRequest<TResult> Certificate(string path, string password);
    /// <summary>
    /// Configura la solicitud para ignorar los errores de validación del certificado SSL.
    /// </summary>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> con la configuración de ignorar SSL aplicada.</returns>
    /// <remarks>
    /// Esta función es útil en entornos de desarrollo o pruebas donde se utilizan certificados SSL autofirmados.
    /// Sin embargo, se debe tener cuidado al usar esta opción en producción, ya que puede comprometer la seguridad.
    /// </remarks>
    IHttpRequest<TResult> IgnoreSsl();
    /// <summary>
    /// Interfaz que representa una solicitud HTTP que puede serializar y deserializar datos.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado que se espera de la solicitud HTTP.</typeparam>
    IHttpRequest<TResult> JsonSerializerOptions( JsonSerializerOptions options );
    /// <summary>
    /// Establece un encabezado en la solicitud HTTP.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a establecer.</param>
    /// <param name="value">El valor del encabezado que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el encabezado establecido.</returns>
    /// <remarks>
    /// Este método permite agregar o modificar un encabezado en la solicitud HTTP actual.
    /// Asegúrese de que la clave y el valor no sean nulos o vacíos para evitar excepciones.
    /// </remarks>
    IHttpRequest<TResult> Header(string key, string value);
    /// <summary>
    /// Establece los encabezados para la solicitud HTTP.
    /// </summary>
    /// <param name="headers">Un diccionario que contiene los encabezados a establecer, donde la clave es el nombre del encabezado y el valor es su contenido.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con los encabezados establecidos.</returns>
    /// <remarks>
    /// Este método permite agregar o modificar encabezados en la solicitud HTTP antes de enviarla.
    /// Asegúrese de que los nombres de los encabezados sean válidos y que los valores estén correctamente formateados.
    /// </remarks>
    IHttpRequest<TResult> Header( IDictionary<string, string> headers );
    /// <summary>
    /// Agrega un parámetro de consulta a la solicitud HTTP.
    /// </summary>
    /// <param name="key">La clave del parámetro de consulta que se va a agregar.</param>
    /// <param name="value">El valor del parámetro de consulta que se va a agregar.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el parámetro de consulta agregado.</returns>
    /// <remarks>
    /// Este método permite construir una solicitud HTTP con parámetros de consulta específicos,
    /// lo que puede ser útil para realizar solicitudes a API que requieren parámetros en la URL.
    /// </remarks>
    IHttpRequest<TResult> QueryString(string key, string value);
    /// <summary>
    /// Establece los parámetros de consulta para la solicitud HTTP.
    /// </summary>
    /// <param name="queryString">Un diccionario que contiene los pares clave-valor que representan los parámetros de consulta.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP con los parámetros de consulta aplicados.</returns>
    /// <remarks>
    /// Este método permite agregar parámetros de consulta a la solicitud HTTP, lo que puede ser útil para filtrar o modificar la información que se envía al servidor.
    /// </remarks>
    IHttpRequest<TResult> QueryString(IDictionary<string, string> queryString);
    /// <summary>
    /// Interfaz que define un método para manejar solicitudes HTTP con un resultado genérico.
    /// </summary>
    /// <typeparam name="TResult">El tipo de resultado esperado de la solicitud HTTP.</typeparam>
    IHttpRequest<TResult> QueryString( object queryString );
    /// <summary>
    /// Configura el uso de cookies en la solicitud HTTP.
    /// </summary>
    /// <param name="isUseCookies">Indica si se deben utilizar cookies en la solicitud. 
    /// Si es <c>true</c>, se habilitarán las cookies; de lo contrario, se deshabilitarán.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la configuración de cookies aplicada.</returns>
    /// <remarks>
    /// Este método permite al usuario decidir si las cookies deben ser enviadas con la solicitud HTTP.
    /// Es útil en situaciones donde se requiere mantener el estado de la sesión o para la autenticación.
    /// </remarks>
    IHttpRequest<TResult> UseCookies(bool isUseCookies);
    /// <summary>
    /// Establece una cookie en la solicitud HTTP.
    /// </summary>
    /// <param name="key">La clave de la cookie que se va a establecer.</param>
    /// <param name="value">El valor de la cookie que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con la cookie establecida.</returns>
    /// <remarks>
    /// Esta función permite agregar una cookie a la solicitud HTTP actual,
    /// lo que puede ser útil para mantener el estado de la sesión o para
    /// enviar información adicional al servidor.
    /// </remarks>
    IHttpRequest<TResult> Cookie(string key, string value);
    /// <summary>
    /// Establece las cookies que se enviarán con la solicitud HTTP.
    /// </summary>
    /// <param name="cookies">Un diccionario que contiene las cookies a enviar, donde la clave es el nombre de la cookie y el valor es el valor de la cookie.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que permite encadenar otras configuraciones de la solicitud.</returns>
    /// <remarks>
    /// Este método permite agregar cookies personalizadas a la solicitud HTTP, lo que puede ser útil para la autenticación o para mantener el estado de la sesión.
    /// </remarks>
    IHttpRequest<TResult> Cookie(IDictionary<string, string> cookies);
    /// <summary>
    /// Establece el contenido de la solicitud HTTP utilizando una clave y un valor especificados.
    /// </summary>
    /// <param name="key">La clave que se utilizará para identificar el contenido en la solicitud.</param>
    /// <param name="value">El valor que se asociará a la clave en la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el contenido actualizado.</returns>
    /// <remarks>
    /// Este método permite agregar o modificar el contenido de la solicitud HTTP.
    /// Asegúrese de que la clave y el valor sean compatibles con el formato esperado por el servidor.
    /// </remarks>
    IHttpRequest<TResult> Content(string key, object value);
    /// <summary>
    /// Establece el contenido de la solicitud HTTP utilizando un diccionario de parámetros.
    /// </summary>
    /// <param name="parameters">Un diccionario que contiene los parámetros que se enviarán como contenido de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP con el contenido establecido.</returns>
    /// <remarks>
    /// Este método permite configurar el contenido de la solicitud de manera flexible, 
    /// permitiendo que se envíen múltiples parámetros en forma de clave-valor.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> Content( IDictionary<string, object> parameters );
    /// <summary>
    /// Establece el contenido de la solicitud HTTP.
    /// </summary>
    /// <param name="value">El valor que se establecerá como contenido de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el contenido establecido.</returns>
    /// <remarks>
    /// Este método permite definir el cuerpo de la solicitud HTTP que se enviará.
    /// Asegúrese de que el valor proporcionado sea compatible con el tipo de contenido esperado.
    /// </remarks>
    IHttpRequest<TResult> Content(object value);
    /// <summary>
    /// Establece el contenido de la solicitud HTTP como un objeto JSON.
    /// </summary>
    /// <param name="value">El objeto que se convertirá a JSON y se establecerá como contenido de la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el contenido JSON establecido.</returns>
    /// <remarks>
    /// Este método serializa el objeto proporcionado a formato JSON utilizando un serializador adecuado
    /// y lo asigna como el cuerpo de la solicitud HTTP.
    /// </remarks>
    IHttpRequest<TResult> JsonContent( object value );
    /// <summary>
    /// Establece el contenido XML de la solicitud HTTP.
    /// </summary>
    /// <param name="value">El contenido XML que se establecerá en la solicitud.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> con el contenido XML configurado.</returns>
    /// <remarks>
    /// Este método permite enviar datos en formato XML en el cuerpo de la solicitud HTTP.
    /// Asegúrese de que el contenido XML sea válido antes de llamar a este método.
    /// </remarks>
    IHttpRequest<TResult> XmlContent(string value);
    /// <summary>
    /// Envía el contenido de un archivo como parte de una solicitud HTTP.
    /// </summary>
    /// <param name="file">El flujo de datos del archivo que se desea enviar.</param>
    /// <param name="fileName">El nombre del archivo que se enviará.</param>
    /// <param name="name">El nombre del campo en la solicitud que contendrá el archivo. Por defecto es "file".</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método permite enviar archivos a través de una solicitud HTTP, lo que es útil para operaciones como la carga de archivos.
    /// Asegúrese de que el flujo de archivo esté abierto y sea accesible antes de llamar a este método.
    /// </remarks>
    IHttpRequest<TResult> FileContent( Stream file,string fileName, string name = "file" );
    /// <summary>
    /// Representa una solicitud HTTP para obtener el contenido de un archivo.
    /// </summary>
    /// <param name="filePath">La ruta del archivo del cual se desea obtener el contenido.</param>
    /// <param name="name">El nombre del archivo que se utilizará en la solicitud. Por defecto es "file".</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP.</returns>
    /// <remarks>
    /// Este método permite enviar una solicitud para obtener el contenido de un archivo específico
    /// en la ruta proporcionada. El nombre del archivo se puede personalizar si es necesario.
    /// </remarks>
    IHttpRequest<TResult> FileContent(string filePath, string name = "file");
    /// <summary>
    /// Obtiene un objeto <see cref="IHttpRequest{TResult}"/> que representa una solicitud HTTP para manejar parámetros de archivo.
    /// </summary>
    /// <param name="isQuote">Indica si se deben incluir comillas en los parámetros del archivo. El valor predeterminado es <c>true</c>.</param>
    /// <returns>Un objeto <see cref="IHttpRequest{TResult}"/> configurado para manejar los parámetros de archivo.</returns>
    /// <remarks>
    /// Este método es útil para construir solicitudes HTTP que requieren el manejo específico de archivos,
    /// permitiendo la opción de incluir comillas en los parámetros según sea necesario.
    /// </remarks>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> FileParameterQuotes( bool isQuote = true );
    /// <summary>
    /// Establece una acción que se ejecutará antes de enviar una solicitud HTTP.
    /// </summary>
    /// <param name="action">Una función que recibe un <see cref="HttpRequestMessage"/> y devuelve un valor booleano que indica si se debe continuar con el envío de la solicitud.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Esta función permite modificar o validar la solicitud antes de que se envíe.
    /// Si la función devuelve <c>false</c>, el envío de la solicitud se cancelará.
    /// </remarks>
    /// <seealso cref="HttpRequestMessage"/>
    IHttpRequest<TResult> OnSendBefore( Func<HttpRequestMessage,bool> action );
    /// <summary>
    /// Registra una acción que se ejecutará después de que se envíe una solicitud HTTP.
    /// </summary>
    /// <param name="action">Una función que toma un <see cref="HttpResponseMessage"/> y devuelve una tarea que produce un resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir el encadenamiento de métodos.</returns>
    /// <remarks>
    /// Este método permite la ejecución de lógica adicional después de que se recibe la respuesta de la solicitud HTTP.
    /// Es útil para manejar la respuesta, realizar transformaciones o registrar información.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que se espera de la función proporcionada.</typeparam>
    /// <seealso cref="HttpResponseMessage"/>
    IHttpRequest<TResult> OnSendAfter( Func<HttpResponseMessage, Task<TResult>> action );
    /// <summary>
    /// Realiza la conversión de la cadena de entrada a un tipo de resultado especificado.
    /// </summary>
    /// <param name="action">Función que define cómo se debe convertir la cadena de entrada en un resultado de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que representa la solicitud HTTP con el resultado convertido.</returns>
    /// <typeparam name="TResult">El tipo de resultado al que se convertirá la cadena de entrada.</typeparam>
    /// <remarks>
    /// Este método permite aplicar una función de conversión a la cadena de entrada, facilitando la manipulación de los datos
    /// recibidos en la solicitud HTTP. Es útil en escenarios donde se necesita transformar datos de texto en tipos más complejos.
    /// </remarks>
    IHttpRequest<TResult> OnConvert( Func<string, TResult> action );
    /// <summary>
    /// Registra una acción que se ejecutará cuando la solicitud HTTP se complete con éxito.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al recibir un resultado exitoso de tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite manejar la respuesta exitosa de una solicitud HTTP de manera asíncrona,
    /// facilitando la implementación de lógica específica al recibir un resultado positivo.
    /// </remarks>
    /// <typeparam name="TResult">El tipo del resultado esperado de la solicitud HTTP.</typeparam>
    IHttpRequest<TResult> OnSuccess(Action<TResult> action);
    /// <summary>
    /// Registra una acción que se ejecutará cuando la solicitud se complete con éxito.
    /// </summary>
    /// <param name="action">La acción asíncrona que se ejecutará al recibir una respuesta exitosa.</param>
    /// <returns>La instancia actual de <see cref="IHttpRequest{TResult}"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método permite definir un comportamiento específico que se debe llevar a cabo cuando la solicitud HTTP se completa correctamente,
    /// proporcionando el resultado de tipo <typeparamref name="TResult"/> como parámetro a la acción.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que se espera de la solicitud HTTP.</typeparam>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> OnSuccess( Func<TResult,Task> action );
    /// <summary>
    /// Registra una acción que se ejecutará cuando la solicitud HTTP falle.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al fallar la solicitud. 
    /// Recibe como parámetros un objeto <see cref="HttpResponseMessage"/> que representa la respuesta 
    /// del servidor y un objeto adicional que puede contener información adicional sobre el error.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> para permitir el encadenamiento de llamadas.</returns>
    /// <remarks>
    /// Este método permite manejar errores de manera personalizada, facilitando la implementación 
    /// de lógica específica en caso de fallos en la solicitud HTTP.
    /// </remarks>
    IHttpRequest<TResult> OnFail( Action<HttpResponseMessage,object> action );
    /// <summary>
    /// Registra una acción que se ejecutará cuando se complete la solicitud HTTP.
    /// </summary>
    /// <param name="action">La acción que se ejecutará al completarse la solicitud. 
    /// Esta acción recibe un <see cref="HttpResponseMessage"/> y un objeto adicional como parámetros.</param>
    /// <returns>Una instancia de <see cref="IHttpRequest{TResult}"/> que permite continuar la configuración de la solicitud.</returns>
    /// <remarks>
    /// Esta función permite manejar la respuesta de la solicitud HTTP de manera asíncrona,
    /// facilitando la implementación de lógica de procesamiento posterior a la recepción de la respuesta.
    /// </remarks>
    /// <seealso cref="HttpResponseMessage"/>
    /// <seealso cref="IHttpRequest{TResult}"/>
    IHttpRequest<TResult> OnComplete( Action<HttpResponseMessage, object> action );
    /// <summary>
    /// Obtiene el resultado de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, que contiene el resultado de tipo <typeparamref name="TResult"/>.</returns>
    /// <remarks>
    /// Este método permite obtener el resultado de una operación que se está ejecutando de manera asíncrona.
    /// Si se solicita la cancelación a través del <paramref name="cancellationToken"/>, la operación puede ser cancelada.
    /// </remarks>
    /// <typeparam name="TResult">El tipo del resultado que se espera obtener al completar la operación.</typeparam>
    /// <seealso cref="CancellationToken"/>
    Task<TResult> GetResultAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un flujo de datos de forma asíncrona.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es un arreglo de bytes que contiene los datos del flujo.</returns>
    /// <remarks>
    /// Este método permite obtener un flujo de datos de manera no bloqueante, lo que es útil en aplicaciones que requieren una alta capacidad de respuesta.
    /// Asegúrese de manejar adecuadamente el token de cancelación para evitar operaciones innecesarias si se solicita la cancelación.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<byte[]> GetStreamAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Escribe de manera asíncrona en un archivo especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <remarks>
    /// Este método permite realizar operaciones de escritura en un archivo sin bloquear el hilo de ejecución.
    /// Asegúrese de que la ruta del archivo sea válida y que tenga los permisos necesarios para escribir en él.
    /// </remarks>
    /// <seealso cref="ReadAsync(string, CancellationToken)"/>
    Task WriteAsync( string filePath, CancellationToken cancellationToken = default );
}