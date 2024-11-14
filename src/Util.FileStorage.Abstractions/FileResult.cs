namespace Util.FileStorage;

/// <summary>
/// Representa el resultado de una operación de archivo.
/// </summary>
public class FileResult {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileResult"/>.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a procesar. No puede estar vacía.</param>
    /// <param name="size">El tamaño del archivo en bytes. Puede ser nulo.</param>
    /// <param name="originalFileName">El nombre original del archivo. Este parámetro es opcional.</param>
    /// <param name="bucket">El nombre del bucket asociado al archivo. Este parámetro es opcional.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="filePath"/> está vacío.</exception>
    /// <remarks>
    /// Esta clase se utiliza para representar un archivo y sus propiedades, incluyendo su ruta, tamaño, 
    /// nombre original y el bucket al que pertenece. El tamaño se convierte en una instancia de <see cref="FileSize"/>.
    /// </remarks>
    public FileResult( string filePath, long? size, string originalFileName = null, string bucket = null ) {
        if( filePath.IsEmpty() )
            throw new ArgumentNullException( nameof( filePath ) );
        FilePath = filePath;
        Size = new FileSize( size.SafeValue() );
        FileName = System.IO.Path.GetFileName( filePath );
        Extension = System.IO.Path.GetExtension( FileName )?.TrimStart( '.' );
        OriginalFileName = originalFileName;
        Bucket = bucket;
    }

    /// <summary>
    /// Obtiene la ruta del archivo.
    /// </summary>
    /// <value>
    /// La ruta del archivo como una cadena.
    /// </value>
    public string FilePath { get; }
    /// <summary>
    /// Obtiene el nombre del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso de solo lectura al nombre del archivo.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el nombre del archivo.
    /// </value>
    public string FileName { get; }
    /// <summary>
    /// Obtiene la extensión del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve la extensión del archivo como una cadena de texto,
    /// incluyendo el punto inicial (por ejemplo, ".txt", ".jpg").
    /// </remarks>
    /// <value>
    /// Una cadena que representa la extensión del archivo.
    /// </value>
    public string Extension { get; }
    /// <summary>
    /// Obtiene el nombre original del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al nombre del archivo tal como fue 
    /// proporcionado originalmente, sin modificaciones ni cambios en el formato.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre original del archivo.
    /// </value>
    public string OriginalFileName { get; }
    /// <summary>
    /// Obtiene el tamaño del archivo.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="FileSize"/> que representa el tamaño del archivo.
    /// </value>
    public FileSize Size { get; }
    /// <summary>
    /// Obtiene el nombre del bucket.
    /// </summary>
    /// <remarks>
    /// Este propiedad es de solo lectura y devuelve el nombre del bucket asociado.
    /// </remarks>
    /// <returns>
    /// Un string que representa el nombre del bucket.
    /// </returns>
    public string Bucket { get; }
}