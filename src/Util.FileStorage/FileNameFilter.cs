namespace Util.FileStorage; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IFileNameFilter"/> para filtrar nombres de archivos.
/// </summary>
/// <remarks>
/// Esta clase permite aplicar criterios de filtrado a los nombres de archivos, facilitando la selección de archivos
/// que cumplen con ciertas condiciones definidas por el usuario.
/// </remarks>
public class FileNameFilter : IFileNameFilter {
    /// <inheritdoc />
    /// <summary>
    /// Filtra el nombre de un archivo y devuelve el mismo nombre sin modificaciones.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a filtrar.</param>
    /// <returns>El nombre del archivo filtrado, que en este caso es el mismo que se recibe como parámetro.</returns>
    /// <remarks>
    /// Este método actualmente no aplica ningún filtro al nombre del archivo.
    /// </remarks>
    public string Filter( string fileName ) {
        return fileName;
    }
}