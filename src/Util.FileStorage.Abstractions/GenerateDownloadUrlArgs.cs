namespace Util.FileStorage; 

/// <summary>
/// Clase que representa los argumentos necesarios para generar una URL de descarga.
/// Hereda de <see cref="FileStorageArgs"/>.
/// </summary>
public class GenerateDownloadUrlArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GenerateDownloadUrlArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo para el cual se generará la URL de descarga.</param>
    public GenerateDownloadUrlArgs( string fileName ) : base( fileName ) {
    }

    /// <summary>
    /// Obtiene o establece el tipo de contenido de la respuesta.
    /// </summary>
    /// <remarks>
    /// Este propiedad se utiliza para definir el tipo de contenido que se enviará en la respuesta,
    /// lo cual es importante para que el cliente sepa cómo interpretar los datos recibidos.
    /// </remarks>
    /// <value>
    /// Un string que representa el tipo de contenido, como "application/json" o "text/html".
    /// </value>
    public string ResponseContentType { get; set; }
}