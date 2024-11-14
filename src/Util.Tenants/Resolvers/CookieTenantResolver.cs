namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase que resuelve el inquilino (tenant) a partir de la información almacenada en las cookies.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TenantResolverBase"/> y proporciona una implementación específica
/// para obtener el inquilino a través de cookies, permitiendo la gestión de múltiples inquilinos
/// en una aplicación.
/// </remarks>
public class CookieTenantResolver : TenantResolverBase
{
    /// <summary>
    /// Resuelve el identificador del inquilino a partir de las cookies del contexto HTTP.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene la información de la solicitud actual.</param>
    /// <returns>
    /// Una tarea que representa el resultado asíncrono de la operación, que contiene el identificador del inquilino extraído de las cookies.
    /// </returns>
    /// <remarks>
    /// Este método busca una clave específica en las cookies del contexto HTTP y registra la operación de resolución del inquilino.
    /// Si la clave no se encuentra, el valor devuelto será nulo.
    /// </remarks>
    /// <seealso cref="GetTenantKey(HttpContext)"/>
    /// <seealso cref="GetLog(HttpContext)"/>
    protected override Task<string> Resolve(HttpContext context)
    {
        var key = GetTenantKey(context);
        var tenantId = context.Request.Cookies[key];
        GetLog(context).LogTrace($"Ejecutar el analizador de inquilinos de cookies.,{key}={tenantId}");
        return Task.FromResult(tenantId);
    }
}