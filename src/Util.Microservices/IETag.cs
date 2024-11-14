namespace Util.Microservices;

/// <summary>
/// Define una interfaz para la gestión de etiquetas ETag.
/// </summary>
public interface IETag {
    /// <summary>
    /// Obtiene o establece el ETag asociado con el recurso.
    /// </summary>
    /// <remarks>
    /// El ETag es un identificador único que se utiliza para gestionar la concurrencia y la caché de recursos.
    /// Puede ser útil para determinar si un recurso ha cambiado desde la última vez que fue recuperado.
    /// </remarks>
    /// <value>
    /// Un string que representa el ETag del recurso.
    /// </value>
    string ETag { get; set; }
}