namespace Util.FileStorage; 

/// <summary>
/// Representa los argumentos necesarios para guardar un archivo.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="FileStorageArgs"/> y proporciona propiedades específicas
/// para el proceso de guardado de archivos.
/// </remarks>
public class SaveFileArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SaveFileArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a guardar.</param>
    /// <param name="stream">El flujo de datos que se va a guardar en el archivo.</param>
    public SaveFileArgs( string fileName, Stream stream ) : base( fileName ) {
        Stream = stream;
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
}