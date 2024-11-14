namespace Util.Tenants;

/// <summary>
/// Interfaz que define las operaciones para la gestión de inquilinos en un sistema multitenencia.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITransientDependency"/> lo que indica que su ciclo de vida es transitorio.
/// </remarks>
public interface ISwitchTenantManager : ITransientDependency {
    /// <summary>
    /// Obtiene el identificador del inquilino actual para el cambio de contexto.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador del inquilino.
    /// </returns>
    /// <remarks>
    /// Este método es útil para aplicaciones que requieren cambiar entre diferentes inquilinos 
    /// en un entorno multi-inquilino. Asegúrese de que el contexto del inquilino esté 
    /// correctamente configurado antes de llamar a este método.
    /// </remarks>
    string GetSwitchTenantId();
    /// <summary>
    /// Cambia el contexto del inquilino actual a uno nuevo especificado por el identificador del inquilino.
    /// </summary>
    /// <param name="tenantId">El identificador del inquilino al que se desea cambiar.</param>
    /// <returns>Una tarea que representa la operación asincrónica de cambio de inquilino.</returns>
    /// <remarks>
    /// Este método es útil en aplicaciones multitenencia donde es necesario cambiar el contexto del inquilino
    /// para realizar operaciones específicas para ese inquilino.
    /// </remarks>
    /// <seealso cref="SwitchTenantAsync(string)"/>
    Task SwitchTenantAsync( string tenantId );
    /// <summary>
    /// Reinicia el inquilino de la aplicación de manera asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para restablecer el estado del inquilino actual, 
    /// lo que puede incluir la limpieza de datos temporales, la restauración de 
    /// configuraciones predeterminadas y la liberación de recursos asociados.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de reinicio del inquilino.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el inquilino no puede ser reiniciado debido a un estado 
    /// inválido o si se encuentra en medio de otra operación.
    /// </exception>
    /// <seealso cref="Tenant"/>
    /// <seealso cref="ITenantService"/>
    Task ResetTenantAsync();
}