namespace Util.Http;

/// <summary>
/// Representa un conjunto de datos relacionados con un archivo.
/// </summary>
public class FileData {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileData"/>.
    /// </summary>
    /// <param name="stream">El flujo de datos asociado al archivo.</param>
    /// <param name="fileName">El nombre del archivo.</param>
    /// <param name="name">El nombre descriptivo del archivo.</param>
    public FileData( Stream stream,string fileName,string name ) {
        Stream = stream;
        FileName = fileName;
        Name = name;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileData"/>.
    /// </summary>
    /// <param name="filePath">La ruta del archivo.</param>
    /// <param name="name">El nombre del archivo.</param>
    public FileData( string filePath, string name ) {
        FilePath = filePath;
        Name = name;
    }

    /// <summary>
    /// Obtiene el flujo asociado.
    /// </summary>
    /// <remarks>
    /// Este flujo puede ser utilizado para realizar operaciones de lectura o escritura de datos.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="Stream"/> que representa el flujo.
    /// </returns>
    public Stream Stream { get; }
    /// <summary>
    /// Obtiene la ruta del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso de solo lectura a la ruta del archivo 
    /// asociado con la instancia actual. La ruta se almacena como una cadena 
    /// y puede ser utilizada para realizar operaciones de archivo, como 
    /// abrir, leer o escribir en el archivo.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta del archivo.
    /// </value>
    public string FilePath { get; }
    /// <summary>
    /// Obtiene el nombre del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad solo tiene un getter, lo que significa que su valor se establece en el momento de la creación del objeto 
    /// y no puede ser modificado posteriormente.
    /// </remarks>
    /// <value>
    /// El nombre del archivo como una cadena de texto.
    /// </value>
    public string FileName { get; }
    /// <summary>
    /// Obtiene el nombre asociado a la instancia.
    /// </summary>
    /// <value>
    /// Un string que representa el nombre.
    /// </value>
    public string Name { get; }
}