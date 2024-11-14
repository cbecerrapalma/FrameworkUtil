namespace Util.Microservices.Dapr.Events; 

/// <summary>
/// Representa un evento de integración que contiene un conteo.
/// </summary>
public class IntegrationEventCount : IDataKey, IETag {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser utilizado para referenciar de manera única un objeto en el sistema.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador.
    /// </value>
    public string Id { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece el ETag asociado con el recurso.
    /// </summary>
    /// <remarks>
    /// El ETag es un identificador único que se utiliza para determinar si el recurso ha cambiado.
    /// Puede ser útil para la gestión de caché y la sincronización de datos.
    /// </remarks>
    /// <value>
    /// Un string que representa el ETag del recurso.
    /// </value>
    public string ETag { get; set; }

    /// <summary>
    /// Obtiene o establece el conteo.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si es nulo, indica que no hay un conteo disponible.
    /// </remarks>
    /// <value>
    /// Un entero que representa el conteo, o null si no está disponible.
    /// </value>
    public int? Count { get; set; }
}