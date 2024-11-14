namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase que resuelve el inquilino a partir de la cadena de consulta (query string).
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TenantResolverBase"/> y proporciona una implementación específica
/// para obtener el inquilino desde la cadena de consulta de la URL.
/// </remarks>
public class QueryStringTenantResolver : TenantResolverBase
{
    /// <summary>
    /// Resuelve el identificador del inquilino a partir del contexto HTTP.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene la información de la solicitud actual.</param>
    /// <returns>Una tarea que representa el resultado asincrónico de la operación, que contiene el identificador del inquilino como una cadena.</returns>
    /// <remarks>
    /// Este método extrae el identificador del inquilino de la cadena de consulta de la solicitud HTTP.
    /// Se utiliza una clave específica para acceder al valor correspondiente en la colección de parámetros de la consulta.
    /// Además, se registra un mensaje de traza que indica la ejecución del analizador de inquilinos.
    /// </remarks>
    protected override Task<string> Resolve(HttpContext context)
    {
        var key = GetTenantKey(context);
        var tenantId = context.Request.Query[key].FirstOrDefault();
        GetLog(context).LogTrace($"Ejecutar el analizador de inquilinos de cadena de consulta.,{key}={tenantId}");
        return Task.FromResult(tenantId);
    }
}