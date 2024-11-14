namespace Util.Domain.Events;

/// <summary>
/// Representa los diferentes tipos de cambios que pueden ocurrir en una entidad.
/// </summary>
public enum EntityChangeType
{
    /// <summary>
    /// Nuevo
    /// </summary>
    Created,
    /// <summary>
    /// Modificar
    /// </summary>
    Updated,
    /// <summary>
    /// Eliminar
    /// </summary>
    Deleted
}