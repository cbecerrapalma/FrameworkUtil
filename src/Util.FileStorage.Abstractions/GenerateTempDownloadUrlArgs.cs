namespace Util.FileStorage; 

/// <summary>
/// Clase que representa los argumentos necesarios para generar una URL de descarga temporal.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="FileStorageArgs"/> y se utiliza para especificar los parámetros
/// requeridos al generar una URL que permite la descarga temporal de un archivo almacenado.
/// </remarks>
public class GenerateTempDownloadUrlArgs : FileStorageArgs {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GenerateTempDownloadUrlArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo para el cual se generará la URL de descarga temporal.</param>
    public GenerateTempDownloadUrlArgs( string fileName ) : base( fileName ) {
    }

    /// <summary>
    /// Obtiene o establece la fecha de expiración.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo, lo que indica que no hay una fecha de expiración definida.
    /// </remarks>
    /// <value>
    /// Un entero que representa la fecha de expiración, o <c>null</c> si no está definida.
    /// </value>
    public int? Expiration { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de contenido de la respuesta.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el tipo de contenido de la respuesta, 
    /// como "text/html", "application/json", etc.
    /// </value>
    public string ResponseContentType { get; set; }
}