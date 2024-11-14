namespace Util.Tenants; 

/// <summary>
/// Define un contrato para la resolución de inquilinos.
/// </summary>
public interface ITenantResolver {
    /// <summary>
    /// Resuelve de manera asíncrona una tarea que produce un resultado de tipo <see cref="string"/> 
    /// basado en el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que contiene información sobre la solicitud actual.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene el resultado como una cadena.</returns>
    /// <remarks>
    /// Este método puede ser utilizado para procesar datos de la solicitud HTTP, 
    /// realizar operaciones de acceso a datos o cualquier otra lógica que requiera 
    /// un contexto HTTP.
    /// </remarks>
    /// <seealso cref="HttpContext"/>
    Task<string> ResolveAsync( HttpContext context );
    /// <summary>
    /// Obtiene o establece la prioridad.
    /// </summary>
    /// <value>
    /// Un entero que representa la prioridad. 
    /// </value>
    int Priority { get; set; }
}