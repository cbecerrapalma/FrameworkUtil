namespace Util.Tenants;

/// <summary>
/// Define un contrato para resolver inquilinos en un dominio.
/// </summary>
/// <remarks>
/// Esta interfaz es utilizada para obtener información sobre el inquilino actual en un contexto de múltiples inquilinos.
/// </remarks>
public interface IDomainTenantResolver : ITransientDependency {
    /// <summary>
    /// Resuelve el identificador del inquilino (tenant) a partir del nombre de host proporcionado.
    /// </summary>
    /// <param name="host">El nombre de host del que se desea obtener el identificador del inquilino.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene el identificador del inquilino como una cadena.</returns>
    /// <remarks>
    /// Este método es útil para aplicaciones que necesitan determinar el inquilino correspondiente en función del contexto del host.
    /// Asegúrese de que el nombre de host sea válido y esté correctamente formateado antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="ResolveTenantIdAsync(string)"/>
    Task<string> ResolveTenantIdAsync( string host );
}