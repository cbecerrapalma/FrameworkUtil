namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase que resuelve el inquilino a partir de los encabezados de la solicitud.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TenantResolverBase"/> y proporciona
/// una implementación específica para extraer información del inquilino
/// desde los encabezados HTTP.
/// </remarks>
public class HeaderTenantResolver : TenantResolverBase
{
    /// <summary>
    /// Resuelve el identificador del inquilino a partir del contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene la información de la solicitud.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica, que contiene el identificador del inquilino extraído de los encabezados de la solicitud.
    /// </returns>
    /// <remarks>
    /// Este método intenta obtener el valor del encabezado correspondiente al inquilino utilizando la clave obtenida mediante el método <see cref="GetTenantKey(HttpContext)"/>.
    /// Si el encabezado está presente, se registra un mensaje de traza con el identificador del inquilino.
    /// </remarks>
    protected override Task<string> Resolve(HttpContext context)
    {
        var key = GetTenantKey(context);
        context.Request.Headers.TryGetValue(key, out var result);
        var tenantId = result.FirstOrDefault();
        GetLog(context).LogTrace($"Ejecutar el analizador de inquilinos del encabezado de solicitud.,{key}={tenantId}");
        return Task.FromResult(tenantId);
    }
}