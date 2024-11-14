namespace Util.FileStorage; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IFileNameProcessor"/> 
/// para procesar nombres de archivos.
/// </summary>
public class FileNameProcessor : IFileNameProcessor {
    /// <inheritdoc />
    /// <summary>
    /// Procesa el nombre de un archivo y devuelve un objeto <see cref="ProcessedName"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a procesar.</param>
    /// <returns>
    /// Un objeto <see cref="ProcessedName"/> que representa el nombre procesado del archivo.
    /// </returns>
    /// <seealso cref="ProcessedName"/>
    public ProcessedName Process( string fileName ) {
        return new ProcessedName( fileName );
    }
}