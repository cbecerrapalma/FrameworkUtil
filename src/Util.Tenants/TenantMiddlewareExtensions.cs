using Util.Tenants.Middlewares;

namespace Util.Tenants;

/// <summary>
/// Proporciona métodos de extensión para configurar el middleware de inquilinos.
/// </summary>
public static class TenantMiddlewareExtensions {
    /// <summary>
    /// Extiende la funcionalidad de <see cref="IApplicationBuilder"/> para agregar el middleware de inquilino.
    /// </summary>
    /// <param name="builder">El <see cref="IApplicationBuilder"/> en el que se agrega el middleware.</param>
    /// <returns>El mismo <see cref="IApplicationBuilder"/> con el middleware de inquilino agregado.</returns>
    /// <remarks>
    /// Este método permite configurar el middleware de inquilino en la tubería de procesamiento de solicitudes.
    /// </remarks>
    /// <seealso cref="TenantMiddleware"/>
    public static IApplicationBuilder UseTenant( this IApplicationBuilder builder ) {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}