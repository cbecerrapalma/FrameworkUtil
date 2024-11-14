namespace Util.FileStorage; 

/// <summary>
/// Representa la información de almacenamiento de un archivo.
/// </summary>
public class FileStorageInfo {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena de texto.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Obtiene o establece la ruta del archivo.
    /// </summary>
    /// <value>
    /// La ruta del archivo como una cadena de texto.
    /// </value>
    public string FilePath { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del archivo.
    /// </summary>
    /// <value>
    /// El nombre del archivo como una cadena de texto.
    /// </value>
    public string FileName { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del archivo sin su extensión.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el nombre de un archivo, excluyendo la parte de la extensión. 
    /// Es útil cuando se necesita trabajar con el nombre del archivo sin considerar su tipo o formato.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre del archivo sin la extensión.
    /// </value>
    public string FileNameWithoutExtension { get; set; }
    /// <summary>
    /// Obtiene o establece la extensión de un archivo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la extensión del archivo, incluyendo el punto inicial (por ejemplo, ".txt", ".jpg").
    /// </remarks>
    /// <value>
    /// Una cadena que representa la extensión del archivo.
    /// </value>
    public string Extension { get; set; }
    /// <summary>
    /// Obtiene o establece el tamaño.
    /// </summary>
    /// <value>El tamaño en formato de tipo <see cref="long"/>.</value>
    public long Size { get; set; }
    /// <summary>
    /// Obtiene o establece la descripción del tamaño.
    /// </summary>
    /// <value>
    /// Una cadena que representa la descripción del tamaño.
    /// </value>
    public string SizeDescription { get; set; }
    /// <summary>
    /// Obtiene o establece la URL asociada.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URL.
    /// </value>
    public string Url { get; set; }
    /// <summary>
    /// Obtiene o establece la URL de la miniatura.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URL de la miniatura.
    /// </value>
    public string ThumbUrl { get; set; }
}