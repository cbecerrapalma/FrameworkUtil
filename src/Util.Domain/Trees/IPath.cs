namespace Util.Domain.Trees; 

/// <summary>
/// Define una interfaz para representar una ruta.
/// </summary>
public interface IPath {
    /// <summary>
    /// Obtiene la ruta como una cadena de texto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso de solo lectura a la ruta, permitiendo que otros componentes
    /// puedan utilizarla sin la capacidad de modificarla.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la ruta.
    /// </returns>
    string Path { get; }
    /// <summary>
    /// Obtiene el nivel actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el nivel asignado.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el nivel actual.
    /// </returns>
    int Level { get; }
}