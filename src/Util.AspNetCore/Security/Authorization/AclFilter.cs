using Util.Applications;

namespace Util.Security.Authorization; 

/// <summary>
/// Representa un filtro de autorización que implementa controles de acceso basados en listas de control de acceso (ACL).
/// </summary>
/// <remarks>
/// Este filtro se utiliza para restringir el acceso a recursos específicos en función de las políticas de autorización definidas.
/// </remarks>
public class AclFilter : AuthorizeFilter {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AclFilter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor configura el filtro ACL utilizando un proveedor de políticas ACL y un atributo ACL.
    /// </remarks>
    /// <param name="provider">El proveedor de políticas que se utilizará para gestionar las políticas de acceso.</param>
    /// <param name="attributes">Una matriz de atributos que se aplicarán al filtro.</param>
    /// <seealso cref="AclPolicyProvider"/>
    /// <seealso cref="AclAttribute"/>
    public AclFilter()
        : base( new AclPolicyProvider(),new[] { new AclAttribute() } ) {
    }

    /// <summary>
    /// Método que se ejecuta de manera asíncrona para realizar la autorización 
    /// en el contexto del filtro de autorización.
    /// </summary>
    /// <param name="context">El contexto del filtro de autorización que contiene información sobre la solicitud actual.</param>
    /// <remarks>
    /// Este método verifica si la política de autorización es efectiva y, si es así, 
    /// obtiene la política efectiva y evalúa la autenticación y autorización 
    /// del usuario en el contexto de la solicitud.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de autorización.
    /// </returns>
    /// <seealso cref="AuthorizationFilterContext"/>
    /// <seealso cref="IPolicyEvaluator"/>
    public override async Task OnAuthorizationAsync( AuthorizationFilterContext context ) {
        context.CheckNull( nameof( context ) );
        if ( context.IsEffectivePolicy( this ) == false ) {
            return;
        }
        var effectivePolicy = await GetEffectivePolicyAsync( context );
        if ( effectivePolicy == null )
            return;
        var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
        var authenticateResult = await policyEvaluator.AuthenticateAsync( effectivePolicy, context.HttpContext );
        if ( HasAllowAnonymous( context ) ) {
            return;
        }
        var authorizationResult = await policyEvaluator.AuthorizeAsync( effectivePolicy, authenticateResult, context.HttpContext, context );
        SetContextResult( context, authorizationResult );
    }

    /// <summary>
    /// Obtiene la política de autorización efectiva para el contexto dado.
    /// </summary>
    /// <param name="context">El contexto del filtro de autorización que contiene la información sobre la solicitud actual.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de la tarea contiene la política de autorización efectiva.
    /// </returns>
    /// <remarks>
    /// Este método combina políticas de autorización de los filtros aplicados al contexto y las metadatas del endpoint.
    /// Se asegura de que no se dupliquen requisitos de autorización y se maneja la combinación de políticas de manera adecuada.
    /// </remarks>
    /// <seealso cref="AuthorizationFilterContext"/>
    /// <seealso cref="IAuthorizationPolicyProvider"/>
    /// <seealso cref="IAuthorizeData"/>
    /// <seealso cref="AclRequirement"/>
    protected async Task<AuthorizationPolicy> GetEffectivePolicyAsync( AuthorizationFilterContext context ) {
        var builder = new AuthorizationPolicyBuilder( await ComputePolicyAsync() );
        for ( var i = 0; i < context.Filters.Count; i++ ) {
            if ( ReferenceEquals( this, context.Filters[i] ) )
                continue;
            if ( context.Filters[i] is AclFilter authorizeFilter ) {
                builder.Combine( await authorizeFilter.ComputePolicyAsync() );
            }
        }
        var endpoint = context.HttpContext.GetEndpoint();
        if ( endpoint != null ) {
            var policyProvider = PolicyProvider ?? context.HttpContext.RequestServices.GetRequiredService<IAuthorizationPolicyProvider>();
            var endpointAuthorizeData = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();
            var endpointPolicy = await AuthorizationPolicy.CombineAsync( policyProvider, endpointAuthorizeData );
            if ( endpointPolicy != null && endpointPolicy.Requirements.Any( t => t is AclRequirement ) ) {
                builder.Requirements.Remove( builder.Requirements.FirstOrDefault( t => t is AclRequirement ) );
                builder.Combine( endpointPolicy );
            }
        }
        return builder.Build();
    }

    /// <summary>
    /// Calcula de manera asíncrona una política de autorización combinando los datos de autorización.
    /// </summary>
    /// <returns>
    /// Un <see cref="ValueTask{AuthorizationPolicy}"/> que representa la política de autorización combinada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="AuthorizationPolicyProvider"/> para combinar los datos de autorización
    /// proporcionados en <paramref name="AuthorizeData"/> y devolver una política de autorización.
    /// </remarks>
    protected async ValueTask<AuthorizationPolicy> ComputePolicyAsync() 
    { 
        return await AuthorizationPolicy.CombineAsync(PolicyProvider, AuthorizeData); 
    }

    /// <summary>
    /// Determina si el contexto de autorización permite el acceso anónimo.
    /// </summary>
    /// <param name="context">El contexto de filtro de autorización que contiene la información sobre la solicitud actual.</param>
    /// <returns>
    /// Devuelve <c>true</c> si se permite el acceso anónimo; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si hay algún filtro en la colección de filtros que implemente 
    /// <see cref="IAllowAnonymousFilter"/>. También verifica si el endpoint actual tiene 
    /// metadatos que permiten el acceso anónimo.
    /// </remarks>
    protected bool HasAllowAnonymous(AuthorizationFilterContext context) {
        var filters = context.Filters;
        for (var i = 0; i < filters.Count; i++) {
            if (filters[i] is IAllowAnonymousFilter) {
                return true;
            }
        }
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Establece el resultado del contexto de autorización basado en el resultado de la política de autorización.
    /// </summary>
    /// <param name="context">El contexto del filtro de autorización que contiene información sobre la solicitud actual.</param>
    /// <param name="authorizationResult">El resultado de la evaluación de la política de autorización.</param>
    /// <remarks>
    /// Si el resultado de la autorización es exitoso, el método no realiza ninguna acción. 
    /// Si la autorización falla, se establece un resultado JSON en el contexto con un código de estado de no autorizado.
    /// </remarks>
    protected virtual void SetContextResult(AuthorizationFilterContext context, PolicyAuthorizationResult authorizationResult)
    {
        if (authorizationResult.Succeeded)
            return;
        context.Result = new JsonResult(new { Code = StateCode.Unauthorized }) { StatusCode = 200 };
    }
}