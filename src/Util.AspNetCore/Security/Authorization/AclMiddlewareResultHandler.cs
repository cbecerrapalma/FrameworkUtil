namespace Util.Security.Authorization; 

/// <summary>
/// Clase que maneja los resultados del middleware de autorización.
/// Implementa la interfaz <see cref="IAuthorizationMiddlewareResultHandler"/>.
/// </summary>
public class AclMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler {
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    /// <summary>
    /// Maneja la autorización de la solicitud HTTP.
    /// </summary>
    /// <param name="next">El siguiente delegado de solicitud que se debe invocar.</param>
    /// <param name="context">El contexto HTTP actual que contiene información sobre la solicitud y la respuesta.</param>
    /// <param name="policy">La política de autorización que se está aplicando.</param>
    /// <param name="authorizeResult">El resultado de la autorización que indica si la autorización fue exitosa o no.</param>
    /// <remarks>
    /// Si la autorización falla, se establece el código de estado de la respuesta HTTP y se envía un resultado no autorizado.
    /// Si la autorización es exitosa, se llama al siguiente manejador en la cadena de procesamiento.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <seealso cref="RequestDelegate"/>
    /// <seealso cref="HttpContext"/>
    /// <seealso cref="AuthorizationPolicy"/>
    /// <seealso cref="PolicyAuthorizationResult"/>
    public async Task HandleAsync( RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult ) {
        if ( authorizeResult.Succeeded == false ) {
            var factory = context.RequestServices.GetRequiredService<IUnauthorizedResultFactory>();
            context.Response.StatusCode = factory.HttpStatusCode;
            await context.Response.WriteAsJsonAsync( factory.CreateResult( context ) );
            return;
        }
        await defaultHandler.HandleAsync( next, context, policy, authorizeResult );
    }
}