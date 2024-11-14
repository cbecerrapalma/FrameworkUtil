namespace Util.FileStorage; 

/// <summary>
/// Representa los argumentos necesarios para verificar la existencia de un archivo en el almacenamiento.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="FileStorageArgs"/> y se utiliza para proporcionar información adicional
/// relacionada con la verificación de la existencia de un archivo.
/// </remarks>
public class FileExistsArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileExistsArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a verificar.</param>
    public FileExistsArgs( string fileName ) : base( fileName ) {
    }
}