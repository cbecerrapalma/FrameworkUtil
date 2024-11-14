namespace Util.Ui.Razor.Internal;

/// <summary>
/// Interfaz que define un contrato para resolver rutas de vista de partes.
/// </summary>
public interface IPartViewPathResolver {
    /// <summary>
    /// Resuelve el contenido de un archivo en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del archivo que se va a resolver.</param>
    /// <param name="content">El contenido que se va a procesar.</param>
    /// <returns>Una lista de cadenas que representa el resultado de la resolución.</returns>
    /// <remarks>
    /// Este método toma una ruta de archivo y un contenido, 
    /// y devuelve una lista de cadenas que puede contener 
    /// información procesada o extraída del contenido 
    /// según la lógica implementada.
    /// </remarks>
    /// <seealso cref="System.IO.File"/>
    /// <seealso cref="System.Collections.Generic.List{T}"/>
    List<string> Resolve( string path, string content );
}