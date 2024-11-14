namespace Util.Microservices.HealthChecks.Dapr;

/// <summary>
/// Clase que implementa la verificación de salud para Dapr.
/// </summary>
/// <remarks>
/// Esta clase se encarga de realizar chequeos de salud para asegurar que el sistema Dapr está funcionando correctamente.
/// </remarks>
public class DaprHealthCheck : IHealthCheck {
    private readonly DaprClient _client;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DaprHealthCheck"/>.
    /// </summary>
    /// <param name="client">El cliente Dapr que se utilizará para las comprobaciones de salud.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando el parámetro <paramref name="client"/> es <c>null</c>.</exception>
    public DaprHealthCheck( DaprClient client ) {
        _client = client ?? throw new ArgumentNullException( nameof( client ) );
    }

    /// <summary>
    /// Verifica el estado de salud del sistema de manera asíncrona.
    /// </summary>
    /// <param name="context">El contexto de la verificación de salud que contiene información sobre la verificación actual.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un objeto <see cref="HealthCheckResult"/> que indica si el sistema está sano o no.
    /// </returns>
    /// <remarks>
    /// Este método llama a un cliente para verificar el estado de salud y devuelve un resultado basado en la respuesta.
    /// Si la verificación es exitosa, se devuelve un resultado saludable; de lo contrario, se devuelve un resultado con el estado de fallo especificado.
    /// En caso de que ocurra una excepción durante la verificación, se devuelve un resultado no saludable con el mensaje de la excepción.
    /// </remarks>
    public async Task<HealthCheckResult> CheckHealthAsync( HealthCheckContext context, CancellationToken cancellationToken = default ) {
        try {
            var healthy = await _client.CheckHealthAsync( cancellationToken );
            return healthy ? HealthCheckResult.Healthy() : new HealthCheckResult( context.Registration.FailureStatus );
        }
        catch ( Exception exception ) {
            return HealthCheckResult.Unhealthy( exception.Message, exception );
        }
    }
}