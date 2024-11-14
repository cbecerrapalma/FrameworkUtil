namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Middleware para manejar los encabezados de eventos en la nube.
/// </summary>
/// <remarks>
/// Este middleware se encarga de interceptar las solicitudes y procesar los encabezados relacionados con eventos en la nube.
/// </remarks>
public class CloudEventHeadersMiddleware {
    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CloudEventHeadersMiddleware"/>.
    /// </summary>
    /// <param name="next">El siguiente delegado de solicitud en la cadena de middleware.</param>
    public CloudEventHeadersMiddleware( RequestDelegate next ) {
        _next = next;
    }

    /// <summary>
    /// Invoca el siguiente middleware en la cadena de procesamiento de solicitudes HTTP.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP que contiene información sobre la solicitud y la respuesta.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay un siguiente middleware en la cadena. Si no hay, simplemente retorna.
    /// Si el contexto HTTP es nulo, también se llama al siguiente middleware sin realizar ninguna operación adicional.
    /// Si el contexto es válido, se importan los encabezados antes de continuar con el siguiente middleware.
    /// </remarks>
    public async Task InvokeAsync( HttpContext httpContext ) {
        if ( _next == null )
            return;
        if ( httpContext == null ) {
            await _next( httpContext );
            return;
        }
        await ImportHeaders( httpContext );
        await _next( httpContext );
    }

    /// <summary>
    /// Importa los encabezados de una solicitud HTTP si el tipo de contenido es "application/cloudevents+json".
    /// </summary>
    /// <param name="httpContext">El contexto HTTP que contiene la solicitud y respuesta.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método verifica el tipo de contenido de la solicitud y, si coincide con el tipo esperado,
    /// habilita el almacenamiento en búfer de la solicitud para poder leer el cuerpo varias veces.
    /// Luego, deserializa el cuerpo de la solicitud en un objeto JSON y extrae los encabezados,
    /// que se añaden a la colección de encabezados de la solicitud HTTP.
    /// </remarks>
    protected virtual async Task ImportHeaders(HttpContext httpContext)
    {
        if (httpContext.Request.ContentType != "application/cloudevents+json")
            return;
        httpContext.Request.EnableBuffering();
        var jsonElement = await JsonSerializer.DeserializeAsync<JsonElement>(httpContext.Request.Body);
        httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        if (jsonElement.TryGetProperty("headers", out var headers) == false)
            return;
        var result = headers.Deserialize<Dictionary<string, string>>();
        foreach (var item in result)
            httpContext.Request.Headers[item.Key] = item.Value;
    }
}