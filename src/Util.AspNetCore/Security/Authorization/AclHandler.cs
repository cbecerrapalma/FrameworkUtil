namespace Util.Security.Authorization;

/// <summary>
/// Clase que maneja la autorización basada en requisitos de ACL (Control de Acceso).
/// Hereda de <see cref="AuthorizationHandler{TRequirement}"/> para implementar la lógica de autorización.
/// </summary>
public class AclHandler : AuthorizationHandler<AclRequirement>
{
    /// <summary>
    /// Maneja la autorización de requisitos específicos mediante el contexto de autorización.
    /// </summary>
    /// <param name="context">El contexto de autorización que contiene información sobre la solicitud actual.</param>
    /// <param name="requirement">El requisito de autorización que se está evaluando.</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método verifica si el contexto y el requisito son válidos, y luego determina si el acceso debe ser concedido o denegado
    /// en función de las opciones de ACL y los permisos del usuario.
    /// </remarks>
    /// <seealso cref="AuthorizationHandlerContext"/>
    /// <seealso cref="AclRequirement"/>
    /// <seealso cref="IPermissionManager"/>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AclRequirement requirement)
    {
        if (context == null)
            return;
        if (requirement == null)
            return;
        var httpContext = GetHttpContext(context.Resource);
        if (httpContext == null)
            return;
        var options = GetAclOptions(httpContext);
        if (options.AllowAnonymous)
        {
            context.Succeed(requirement);
            return;
        }
        if (httpContext.GetIdentity().IsAuthenticated == false)
            return;
        if (options.IgnoreAcl)
        {
            context.Succeed(requirement);
            return;
        }
        if (requirement.Ignore)
        {
            context.Succeed(requirement);
            return;
        }
        var permissionManager = httpContext.RequestServices.GetService<IPermissionManager>();
        if (permissionManager == null)
        {
            context.Fail(new AuthorizationFailureReason(this, "No se ha implementado IPermissionManager."));
            return;
        }
        var uri = GetResourceUri(requirement, httpContext, options);
        var result = await permissionManager.HasPermissionAsync(uri);
        if (result)
        {
            context.Succeed(requirement);
            return;
        }
        context.Fail(new AuthorizationFailureReason(this, $"No tienes permiso para acceder al recurso {uri}."));
    }

    /// <summary>
    /// Obtiene el contexto HTTP a partir del recurso proporcionado.
    /// </summary>
    /// <param name="resource">El recurso del cual se extraerá el contexto HTTP. Puede ser de tipo <see cref="DefaultHttpContext"/> o un objeto que contenga una propiedad <c>HttpContext</c>.</param>
    /// <returns>El contexto HTTP correspondiente al recurso proporcionado.</returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual HttpContext GetHttpContext(dynamic resource)
    {
        if (resource is DefaultHttpContext httpContext)
            return httpContext;
        return resource.HttpContext;
    }

    /// <summary>
    /// Obtiene las opciones de control de acceso (ACL) para el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="httpContext">El contexto HTTP del cual se obtendrán las opciones ACL.</param>
    /// <returns>
    /// Un objeto <see cref="AclOptions"/> que contiene las opciones de control de acceso.
    /// Si no se encuentran opciones configuradas, se devuelve una nueva instancia de <see cref="AclOptions"/> por defecto.
    /// </returns>
    /// <remarks>
    /// Este método busca en los servicios del contexto HTTP para obtener las opciones de ACL.
    /// Si no se encuentra ninguna configuración, se proporciona un objeto por defecto.
    /// </remarks>
    protected virtual AclOptions GetAclOptions(HttpContext httpContext)
    {
        var result = httpContext.RequestServices.GetService<IOptions<AclOptions>>();
        return result == null ? new AclOptions() : result.Value;
    }

    /// <summary>
    /// Obtiene la URI del recurso basado en los requisitos de acceso y las opciones proporcionadas.
    /// </summary>
    /// <param name="requirement">El requisito de acceso que contiene la URI del recurso.</param>
    /// <param name="httpContext">El contexto HTTP actual que contiene información sobre la solicitud.</param>
    /// <param name="options">Las opciones de acceso que pueden incluir una URI de recurso predeterminada.</param>
    /// <returns>
    /// La URI del recurso, que puede ser la URI del requisito, la URI de las opciones, 
    /// o una URI generada a partir de la ruta y el método de la solicitud HTTP.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si el requisito de acceso tiene una URI definida. 
    /// Si no es así, verifica las opciones proporcionadas. 
    /// Si ninguna de las dos tiene una URI válida, genera una URI basada en la ruta de la solicitud 
    /// y el método HTTP.
    /// </remarks>
    protected virtual string GetResourceUri(AclRequirement requirement, HttpContext httpContext, AclOptions options)
    {
        if (requirement.Uri.IsEmpty() == false)
            return requirement.Uri;
        if (options.ResourceUri.IsEmpty() == false)
            return options.ResourceUri;
        return GetResourceUri(httpContext.Request.Path, httpContext.Request.Method);
    }

    /// <summary>
    /// Obtiene la URI de un recurso basado en la ruta y el método HTTP proporcionados.
    /// </summary>
    /// <param name="path">La ruta del recurso.</param>
    /// <param name="httpMethod">El método HTTP asociado (por ejemplo, GET, POST).</param>
    /// <returns>
    /// Una cadena que representa la URI del recurso en formato lowercase, o null si la ruta está vacía.
    /// </returns>
    /// <remarks>
    /// Este método combina la ruta y el método HTTP en una sola cadena, separándolos con un símbolo de hash (#).
    /// La cadena resultante se convierte a minúsculas utilizando la cultura invariante.
    /// </remarks>
    protected virtual string GetResourceUri(string path, string httpMethod)
    {
        if (path.IsEmpty())
            return null;
        var result = $"{path}#{httpMethod}";
        return result.ToLower(CultureInfo.InvariantCulture);
    }
}