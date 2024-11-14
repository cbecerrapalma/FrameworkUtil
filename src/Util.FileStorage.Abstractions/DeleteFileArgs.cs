namespace Util.FileStorage; 

/// <summary>
/// Representa los argumentos necesarios para eliminar un archivo en el sistema de almacenamiento.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="FileStorageArgs"/> y proporciona propiedades específicas
/// para la operación de eliminación de archivos.
/// </remarks>
public class DeleteFileArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DeleteFileArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a eliminar.</param>
    public DeleteFileArgs( string fileName ) : base( fileName ) {
    }
}