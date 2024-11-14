namespace Util.FileStorage;

/// <summary>
/// Representa los parámetros necesarios para una carga directa.
/// </summary>
public class DirectUploadParam {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DirectUploadParam"/>.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a subir.</param>
    /// <param name="url">La URL a la que se enviará el archivo.</param>
    /// <param name="data">Datos adicionales que se pueden enviar junto con el archivo (opcional).</param>
    /// <param name="originalFileName">El nombre original del archivo (opcional).</param>
    /// <param name="bucket">El nombre del bucket donde se almacenará el archivo (opcional).</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="filePath"/> está vacío.</exception>
    /// <remarks>
    /// Esta clase se utiliza para encapsular los parámetros necesarios para una carga directa de un archivo.
    /// </remarks>
    public DirectUploadParam( string filePath, string url, object data = null, string originalFileName = null, string bucket = null ) {
        if( filePath.IsEmpty() )
            throw new ArgumentNullException( nameof( filePath ) );
        FilePath = filePath;
        Url = url;
        Data = data;
        FileName = System.IO.Path.GetFileName( filePath );
        Extension = System.IO.Path.GetExtension( FileName )?.TrimStart( '.' );
        OriginalFileName = originalFileName;
        Bucket = bucket;
    }

    /// <summary>
    /// Obtiene la URL asociada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso de solo lectura a la URL. 
    /// La URL puede ser utilizada para realizar solicitudes o para 
    /// ser mostrada en la interfaz de usuario.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la URL.
    /// </value>
    public string Url { get; }
    /// <summary>
    /// Representa un objeto de datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a un objeto de datos que puede ser de cualquier tipo.
    /// </remarks>
    /// <value>
    /// Un objeto que contiene los datos.
    /// </value>
    public object Data { get; }
    /// <summary>
    /// Obtiene la ruta del archivo.
    /// </summary>
    /// <value>
    /// La ruta del archivo como una cadena de texto.
    /// </value>
    public string FilePath { get; }
    /// <summary>
    /// Obtiene el nombre del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad solo tiene un getter, lo que significa que su valor es de solo lectura.
    /// </remarks>
    /// <returns>
    /// Un string que representa el nombre del archivo.
    /// </returns>
    public string FileName { get; }
    /// <summary>
    /// Obtiene la extensión del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve la extensión del archivo como una cadena de texto.
    /// La extensión incluye el punto inicial, por ejemplo, ".txt".
    /// </remarks>
    /// <value>
    /// Una cadena que representa la extensión del archivo.
    /// </value>
    public string Extension { get; }
    /// <summary>
    /// Obtiene el nombre original del archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al nombre del archivo tal como fue proporcionado originalmente,
    /// sin modificaciones ni cambios en su formato.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el nombre original del archivo.
    /// </returns>
    public string OriginalFileName { get; }
    /// <summary>
    /// Obtiene el nombre del bucket.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el nombre del bucket asociado.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre del bucket.
    /// </value>
    public string Bucket { get; }
}