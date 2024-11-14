namespace Util.Microservices;

/// <summary>
/// Representa la clase base para el manejo de estados.
/// Implementa las interfaces <see cref="IDataKey"/>, <see cref="IDataType"/> y <see cref="IETag"/>.
/// </summary>
public class StateBase : IDataKey, IDataType, IETag {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser utilizado para distinguir entre diferentes instancias 
    /// de un objeto o entidad en el sistema.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="string"/> que representa el identificador.
    /// </value>
    public string Id { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece el tipo de dato.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para definir el tipo de dato asociado a un elemento específico.
    /// </remarks>
    /// <value>
    /// Un string que representa el tipo de dato.
    /// </value>
    public string DataType { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene o establece el ETag del recurso.
    /// </summary>
    /// <remarks>
    /// El ETag es un identificador único que se utiliza para gestionar la caché y la concurrencia en las solicitudes HTTP.
    /// </remarks>
    /// <value>
    /// Un string que representa el ETag del recurso.
    /// </value>
    public string ETag { get; set; }
}