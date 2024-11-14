using Util.Tenants.Managements;

namespace Util.Tenants.Middlewares;

/// <summary>
/// Middleware que gestiona la lógica de inquilinos (tenants) en la aplicación.
/// </summary>
/// <remarks>
/// Este middleware se encarga de identificar el inquilino basado en la solicitud entrante
/// y de establecer el contexto correspondiente para el resto de la aplicación.
/// </remarks>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TenantMiddleware"/>.
    /// </summary>
    /// <param name="next">El siguiente delegado de solicitud en la cadena de middleware.</param>
    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Procesa la solicitud HTTP y resuelve el inquilino actual.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método invoca el siguiente middleware en la cadena de procesamiento, 
    /// después de resolver el inquilino actual utilizando el <see cref="ITenantResolver"/>.
    /// Si se produce una excepción durante la resolución del inquilino, se registra el error.
    /// </remarks>
    /// <exception cref="Exception">Se lanza si ocurre un error durante la resolución del inquilino.</exception>
    /// <seealso cref="ITenantResolver"/>
    /// <seealso cref="ILogger{T}"/>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (_next == null)
            return;
        if (httpContext == null)
        {
            await _next(httpContext);
            return;
        }
        var log = httpContext.RequestServices.GetService<ILogger<TenantMiddleware>>() ?? NullLogger<TenantMiddleware>.Instance;
        var resolver = httpContext.RequestServices.GetRequiredService<ITenantResolver>();
        try
        {
            var tenantId = await resolver.ResolveAsync(httpContext);
            TenantManager.CurrentTenantId = tenantId;
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            log.LogError(exception, "Se produjo una excepción en el middleware de procesamiento de inquilinos.");
            throw;
        }
    }
}