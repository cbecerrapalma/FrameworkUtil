namespace Util.FileStorage; 

/// <summary>
/// Clase que representa los argumentos necesarios para obtener un flujo de archivo.
/// Hereda de <see cref="FileStorageArgs"/>.
/// </summary>
public class GetFileStreamArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetFileStreamArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a procesar.</param>
    public GetFileStreamArgs( string fileName ) : base( fileName ) {
    }
}