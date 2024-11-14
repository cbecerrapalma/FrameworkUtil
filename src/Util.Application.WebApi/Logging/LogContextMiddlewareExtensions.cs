namespace Util.Applications.Logging; 

/// <summary>
/// Proporciona métodos de extensión para agregar el middleware de contexto de registro.
/// </summary>
public static class LogContextMiddlewareExtensions {
    /// <summary>
    /// Extensión para configurar el middleware de contexto de registro.
    /// </summary>
    /// <param name="builder">El constructor de la aplicación que se está configurando.</param>
    /// <returns>El mismo constructor de la aplicación para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método añade el middleware <see cref="LogContextMiddleware"/> al pipeline de la aplicación.
    /// </remarks>
    public static IApplicationBuilder UseLogContext( this IApplicationBuilder builder ) {
        return builder.UseMiddleware<LogContextMiddleware>();
    }
}