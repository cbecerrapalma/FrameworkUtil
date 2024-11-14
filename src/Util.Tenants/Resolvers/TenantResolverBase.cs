namespace Util.Tenants.Resolvers;

/// <summary>
/// Clase base abstracta que proporciona la funcionalidad para resolver inquilinos.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ITenantResolver"/> y define la estructura básica
/// que deben seguir las clases derivadas para resolver inquilinos en un contexto determinado.
/// </remarks>
public abstract class TenantResolverBase : ITenantResolver {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece la prioridad.
    /// </summary>
    /// <remarks>
    /// La propiedad <c>Priority</c> se utiliza para determinar la importancia de un elemento en comparación con otros.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa la prioridad, donde un número mayor indica una mayor prioridad.
    /// </value>
    public int Priority { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// Resuelve de manera asíncrona el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que se va a resolver. No puede ser nulo.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene una cadena resultante
    /// de la resolución del contexto, o null si el contexto es nulo.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contexto es nulo antes de proceder a la resolución.
    /// Si el contexto es nulo, se devuelve null de inmediato.
    /// </remarks>
    public async Task<string> ResolveAsync( HttpContext context ) {
        if ( context == null )
            return null;
        return await Resolve( context );
    }

    /// <summary>
    /// Método abstracto que se encarga de resolver una solicitud HTTP y devolver un resultado en forma de cadena.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que representa la respuesta.</returns>
    /// <remarks>
    /// Este método debe ser implementado por las clases derivadas para proporcionar la lógica específica de resolución.
    /// </remarks>
    protected abstract Task<string> Resolve(HttpContext context);

    /// <summary>
    /// Obtiene la clave del inquilino (tenant) a partir del contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene los servicios requeridos.</param>
    /// <returns>La clave del inquilino como una cadena de texto.</returns>
    /// <remarks>
    /// Este método utiliza el servicio de opciones para acceder a la configuración del inquilino.
    /// Asegúrese de que el servicio de opciones esté registrado en el contenedor de servicios.
    /// </remarks>
    protected string GetTenantKey(HttpContext context) {
        var options = context.RequestServices.GetRequiredService<IOptions<TenantOptions>>();
        return options.Value.TenantKey;
    }

    /// <summary>
    /// Obtiene una instancia de <see cref="ILogger{ITenantResolver}"/> para el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP actual.</param>
    /// <returns>
    /// Una instancia de <see cref="ILogger{ITenantResolver}"/>. Si no se encuentra una instancia, se devuelve <see cref="NullLogger{ITenantResolver}.Instance"/>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el contenedor de servicios para resolver el logger asociado a <see cref="ITenantResolver"/>.
    /// Si el servicio no está registrado, se proporciona un logger nulo para evitar excepciones.
    /// </remarks>
    protected ILogger<ITenantResolver> GetLog(HttpContext context)
    {
        return context.RequestServices.GetService<ILogger<ITenantResolver>>() ?? NullLogger<ITenantResolver>.Instance;
    }
}