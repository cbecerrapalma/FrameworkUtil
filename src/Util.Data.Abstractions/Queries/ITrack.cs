namespace Util.Data.Queries; 

/// <summary>
/// Define una interfaz para rastrear objetos.
/// </summary>
public interface ITrack {
    /// <summary>
    /// Desactiva el seguimiento de cambios en el contexto actual.
    /// </summary>
    /// <remarks>
    /// Este método es útil cuando se desea realizar consultas sobre entidades sin que el contexto
    /// realice un seguimiento de los cambios en dichas entidades, lo que puede mejorar el rendimiento
    /// en escenarios de solo lectura.
    /// </remarks>
    void NoTracking();
}