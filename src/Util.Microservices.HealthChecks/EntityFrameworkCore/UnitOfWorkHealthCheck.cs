namespace Util.Microservices.HealthChecks.EntityFrameworkCore;

/// <summary>
/// Clase que implementa un chequeo de salud para una unidad de trabajo.
/// </summary>
/// <typeparam name="TUnitOfWork">El tipo de unidad de trabajo que implementa <see cref="IUnitOfWork"/>.</typeparam>
public class UnitOfWorkHealthCheck<TUnitOfWork> : IHealthCheck where TUnitOfWork : IUnitOfWork {
    private readonly TUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWorkHealthCheck"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo que se utilizará para realizar comprobaciones de salud.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="unitOfWork"/> es <c>null</c>.</exception>
    public UnitOfWorkHealthCheck( TUnitOfWork unitOfWork ) {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException( nameof( unitOfWork ) );
    }

    /// <summary>
    /// Verifica el estado de salud de la aplicación de forma asíncrona.
    /// </summary>
    /// <param name="context">El contexto de verificación de salud que contiene información sobre la verificación actual.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Un objeto <see cref="HealthCheckResult"/> que indica si la aplicación está sana o no.</returns>
    /// <remarks>
    /// Este método intenta establecer una conexión utilizando la unidad de trabajo. Si la conexión es exitosa, 
    /// se devuelve un resultado de salud saludable. Si no se puede conectar, se devuelve un resultado de salud 
    /// que indica un estado de fallo. En caso de que ocurra una excepción durante el proceso, se devuelve 
    /// un resultado de salud no saludable con el mensaje de la excepción.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el <paramref name="context"/> es nulo.</exception>
    public async Task<HealthCheckResult> CheckHealthAsync( HealthCheckContext context, CancellationToken cancellationToken = default ) {
        context.CheckNull( nameof( context ) );
        try {
            var healthy = await _unitOfWork.CanConnectAsync( cancellationToken );
            return healthy ? HealthCheckResult.Healthy() : new HealthCheckResult( context.Registration.FailureStatus );
        }
        catch ( Exception exception ) {
            return HealthCheckResult.Unhealthy( exception.Message, exception );
        }
    }
}