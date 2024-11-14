namespace Util.FileStorage; 

/// <summary>
/// Clase que representa los argumentos necesarios para guardar un archivo a través de una URL.
/// Hereda de <see cref="FileStorageArgs"/>.
/// </summary>
public class SaveFileByUrlArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SaveFileByUrlArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a guardar.</param>
    /// <param name="url">La URL desde la cual se descargará el archivo.</param>
    public SaveFileByUrlArgs( string fileName, string url ) : base( fileName ) {
        Url = url;
    }

    /// <summary>
    /// Obtiene la URL asociada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve una cadena que representa la URL.
    /// </remarks>
    /// <returns>
    /// Una cadena que contiene la URL.
    /// </returns>
    public string Url { get; }
}