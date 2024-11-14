namespace Util.Microservices.Dapr.ServiceInvocations;

/// <summary>
/// Representa un argumento para la invocación de un servicio.
/// </summary>
/// <remarks>
/// Este registro contiene información relevante para la invocación de un servicio,
/// incluyendo el identificador de la aplicación, el nombre del método, el método HTTP,
/// los datos de la solicitud, y los mensajes de solicitud y respuesta.
/// </remarks>
/// <param name="AppId">El identificador de la aplicación que realiza la invocación.</param>
/// <param name="MethodName">El nombre del método que se invoca.</param>
/// <param name="HttpMethod">El método HTTP utilizado para la invocación.</param>
/// <param name="RequestData">Los datos que se envían en la solicitud.</param>
/// <param name="RequestMessage">El mensaje de solicitud HTTP que se envía.</param>
/// <param name="ResponseMessage">El mensaje de respuesta HTTP recibido (opcional).</param>
/// <param name="Result">El resultado de la invocación (opcional).</param>
/// <param name="Exception">Una excepción que puede haber ocurrido durante la invocación (opcional).</param>
/// <param name="Message">Un mensaje adicional relacionado con la invocación (opcional).</param>
public record ServiceInvocationArgument(
    string AppId,
    string MethodName,
    HttpMethod HttpMethod,
    object RequestData,
    HttpRequestMessage RequestMessage,
    HttpResponseMessage ResponseMessage = null,
    object Result = null,
    Exception Exception = null,
    string Message = null
);