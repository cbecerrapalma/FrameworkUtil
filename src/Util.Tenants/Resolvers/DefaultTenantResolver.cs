namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase que implementa la interfaz <see cref="ITenantResolver"/> 
/// para resolver el inquilino predeterminado en una aplicación multiinquilino.
/// </summary>
public class DefaultTenantResolver : ITenantResolver {
    private readonly TenantOptions _options;
    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece la prioridad.
    /// </summary>
    /// <remarks>
    /// La propiedad <c>Priority</c> permite definir un nivel de importancia 
    /// que puede ser utilizado para ordenar o clasificar elementos en una 
    /// colección o sistema.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa la prioridad. Los valores más bajos 
    /// indican una mayor prioridad.
    /// </value>
    public int Priority { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultTenantResolver"/>.
    /// </summary>
    /// <param name="options">Las opciones de inquilino que se utilizarán para la configuración.</param>
    /// <remarks>
    /// Si <paramref name="options"/> es nulo, se utilizarán las opciones de inquilino nulas por defecto.
    /// </remarks>
    public DefaultTenantResolver( IOptions<TenantOptions> options ) {
        _options = options?.Value ?? TenantOptions.Null;
    }

    /// <inheritdoc />
    /// <summary>
    /// Resuelve el identificador del inquilino (tenant) de la sesión actual.
    /// </summary>
    /// <param name="context">El contexto HTTP actual que contiene información sobre la solicitud y la respuesta.</param>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del inquilino si se encuentra, 
    /// o <c>null</c> si no se puede resolver el identificador o si el contexto es nulo.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contexto es nulo y si las opciones están habilitadas. 
    /// Si hay una sesión autenticada, se devuelve el identificador del inquilino de la sesión. 
    /// Si no, se itera sobre los resolutores configurados en orden de prioridad para intentar resolver el identificador del inquilino.
    /// </remarks>
    /// <seealso cref="Util.Sessions.ISession"/>
    public async Task<string> ResolveAsync( HttpContext context ) {
        if ( context == null )
            return null;
        if ( _options.IsEnabled == false )
            return null;
        var session = context.RequestServices.GetService<Util.Sessions.ISession>();
        if ( session is { IsAuthenticated: true } )
            return session.TenantId;
        if ( _options.Resolvers == null )
            return null;
        foreach ( var resolver in _options.Resolvers.OrderByDescending( t => t.Priority ) ) {
            var result = await resolver.ResolveAsync( context );
            if ( result.IsEmpty() == false )
                return result;
        }
        return null;
    }
}