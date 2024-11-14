using Microsoft.AspNetCore.Http;
using Util.Logging;

namespace Util.Applications.Logging; 

/// <summary>
/// Middleware que se encarga de gestionar el contexto de registro (logging) 
/// en la aplicación, permitiendo la captura y el manejo de información de 
/// registro durante el ciclo de vida de las solicitudes HTTP.
/// </summary>
public class LogContextMiddleware {
    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LogContextMiddleware"/>.
    /// </summary>
    /// <param name="next">El siguiente delegado de solicitud en la cadena de middleware.</param>
    public LogContextMiddleware( RequestDelegate next ) {
        _next = next;
    }

    /// <summary>
    /// Invoca el middleware y establece el contexto de registro para la solicitud HTTP actual.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método extrae el identificador de correlación de los encabezados de la solicitud,
    /// y si no está presente, utiliza el identificador de traza del contexto. Luego, obtiene
    /// la sesión del usuario y el entorno de la aplicación para crear un contexto de registro.
    /// Finalmente, almacena el contexto de registro en el contexto de la solicitud y llama al siguiente
    /// middleware en la cadena.
    /// </remarks>
    public async Task Invoke( HttpContext context ) {
        var traceId = context.Request.Headers["x-correlation-id"].SafeString();
        if ( traceId.IsEmpty() )
            traceId = context.TraceIdentifier;
        var session = context.RequestServices.GetService<Util.Sessions.ISession>();
        var environment = context.RequestServices.GetService<IWebHostEnvironment>();
        var logContext = new LogContext {
            Stopwatch = Stopwatch.StartNew(), 
            TraceId = traceId, 
            UserId = session?.UserId,
            Application = environment?.ApplicationName,
            Environment = environment?.EnvironmentName
        };
        context.Items[LogContextAccessor.LogContextKey] = logContext;
        await _next( context );
    }
}