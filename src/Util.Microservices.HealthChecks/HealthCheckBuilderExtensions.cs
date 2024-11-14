using Util.Microservices.HealthChecks.Dapr;
using Util.Microservices.HealthChecks.EntityFrameworkCore;

namespace Util.Microservices.HealthChecks;

/// <summary>
/// Proporciona métodos de extensión para construir verificaciones de salud.
/// </summary>
public static class HealthCheckBuilderExtensions {
    /// <summary>
    /// Agrega un chequeo de salud para Dapr al constructor de chequeos de salud.
    /// </summary>
    /// <param name="builder">El constructor de chequeos de salud al que se le añadirá el chequeo de Dapr.</param>
    /// <param name="name">El nombre del chequeo de salud. Por defecto es "dapr".</param>
    /// <param name="tags">Una colección de etiquetas asociadas al chequeo de salud. Por defecto es null.</param>
    /// <param name="failureStatus">El estado de salud que se considera como fallo. Por defecto es <see cref="HealthStatus.Unhealthy"/>.</param>
    /// <returns>El constructor de chequeos de salud con el chequeo de Dapr agregado.</returns>
    /// <remarks>
    /// Este método permite integrar un chequeo de salud específico para Dapr dentro de un sistema de chequeo de salud más amplio.
    /// Se puede personalizar el nombre, las etiquetas y el estado de fallo según las necesidades del sistema.
    /// </remarks>
    /// <seealso cref="DaprHealthCheck"/>
    public static IHealthChecksBuilder AddDapr( this IHealthChecksBuilder builder, string name = "dapr", IEnumerable<string> tags = null, HealthStatus? failureStatus = null ) {
        builder.CheckNull( nameof( builder ) );
        failureStatus ??= HealthStatus.Unhealthy;
        tags ??= Enumerable.Empty<string>();
        return builder.AddCheck<DaprHealthCheck>( name, failureStatus, tags );
    }

    /// <summary>
    /// Agrega un chequeo de salud para una unidad de trabajo específica.
    /// </summary>
    /// <typeparam name="TUnitOfWork">El tipo de la unidad de trabajo que implementa <see cref="IUnitOfWork"/>.</typeparam>
    /// <param name="builder">El constructor de chequeos de salud al que se le añadirá el chequeo.</param>
    /// <param name="name">El nombre del chequeo de salud. Por defecto es "unitOfWork".</param>
    /// <param name="tags">Una colección opcional de etiquetas para el chequeo de salud.</param>
    /// <param name="failureStatus">El estado de fallo que se utilizará si el chequeo no pasa. Por defecto es <see cref="HealthStatus.Unhealthy"/>.</param>
    /// <returns>El constructor de chequeos de salud con el nuevo chequeo agregado.</returns>
    /// <remarks>
    /// Este método permite registrar un chequeo de salud que verifica el estado de la unidad de trabajo especificada.
    /// Se puede utilizar para asegurar que la unidad de trabajo está operativa y lista para manejar transacciones.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="builder"/> es nulo.</exception>
    /// <seealso cref="IUnitOfWork"/>
    /// <seealso cref="UnitOfWorkHealthCheck{TUnitOfWork}"/>
    public static IHealthChecksBuilder AddUnitOfWork<TUnitOfWork>( this IHealthChecksBuilder builder, string name = "unitOfWork", IEnumerable<string> tags = null, HealthStatus? failureStatus = null )
        where TUnitOfWork: IUnitOfWork {
        builder.CheckNull( nameof( builder ) );
        failureStatus ??= HealthStatus.Unhealthy;
        tags ??= Enumerable.Empty<string>();
        return builder.AddCheck<UnitOfWorkHealthCheck<TUnitOfWork>>( name, failureStatus, tags );
    }

    /// <summary>
    /// Agrega un chequeo de salud basado en una URL al constructor de chequeos de salud.
    /// </summary>
    /// <param name="builder">El constructor de chequeos de salud al que se le añadirá el chequeo de URL.</param>
    /// <param name="url">La URL que se utilizará para el chequeo de salud.</param>
    /// <param name="name">El nombre del chequeo de salud.</param>
    /// <param name="tags">Una colección opcional de etiquetas asociadas con el chequeo de salud.</param>
    /// <param name="failureStatus">El estado de fallo opcional que se utilizará si el chequeo de salud falla. Si no se especifica, se establece en <see cref="HealthStatus.Unhealthy"/>.</param>
    /// <returns>El constructor de chequeos de salud actualizado con el nuevo chequeo de URL.</returns>
    /// <remarks>
    /// Este método permite agregar un chequeo de salud que verifica la disponibilidad de un servicio en una URL específica.
    /// Si la URL no es accesible, se considerará que el estado de salud es el especificado por <paramref name="failureStatus"/>.
    /// </remarks>
    /// <seealso cref="IHealthChecksBuilder"/>
    public static IHealthChecksBuilder AddUrl( this IHealthChecksBuilder builder,string url, string name, IEnumerable<string> tags = null, HealthStatus? failureStatus = null ) {
        builder.CheckNull( nameof( builder ) );
        failureStatus ??= HealthStatus.Unhealthy;
        tags ??= Enumerable.Empty<string>();
        return builder.AddUrlGroup( new Uri( url ), name, failureStatus, tags );
    }
}