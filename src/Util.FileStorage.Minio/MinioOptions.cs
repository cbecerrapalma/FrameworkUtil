namespace Util.FileStorage.Minio; 

/// <summary>
/// Representa las opciones de configuración para la conexión a un servidor Minio.
/// </summary>
public class MinioOptions {
    /// <summary>
    /// Obtiene o establece la URL del endpoint.
    /// </summary>
    /// <value>
    /// Una cadena que representa la URL del endpoint.
    /// </value>
    public string Endpoint { get; set; }
    /// <summary>
    /// Obtiene o establece la clave de acceso.
    /// </summary>
    /// <value>
    /// La clave de acceso como una cadena de texto.
    /// </value>
    public string AccessKey { get; set; }
    /// <summary>
    /// Obtiene o establece la clave secreta.
    /// </summary>
    /// <value>
    /// La clave secreta como una cadena de texto.
    /// </value>
    public string SecretKey { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe utilizar SSL (Secure Sockets Layer).
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite habilitar o deshabilitar la seguridad de la conexión mediante SSL. 
    /// Cuando se establece en <c>true</c>, se utilizará una conexión segura; de lo contrario, se utilizará una conexión no segura.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se debe utilizar SSL; de lo contrario, <c>false</c>.
    /// </value>
    public bool UseSSL { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del bucket predeterminado.
    /// </summary>
    /// <remarks>
    /// Este nombre se utiliza para identificar el bucket en el que se almacenan los datos por defecto.
    /// Asegúrese de que el nombre cumpla con las políticas de nomenclatura de buckets de su proveedor de almacenamiento.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre del bucket.
    /// </value>
    public string DefaultBucketName { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre del cliente HTTP.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para identificar de manera única una instancia de cliente HTTP
    /// en el contexto de la aplicación. Puede ser útil para la configuración y el registro.
    /// </remarks>
    public string HttpClientName { get; set; }
    /// <summary>
    /// Obtiene o establece el tiempo de expiración, en segundos, para las URL de carga.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es 3600 segundos (1 hora).
    /// </remarks>
    /// <value>
    /// Un entero que representa el tiempo de expiración de la URL de carga.
    /// </value>
    public int UploadUrlExpiration { get; set; } = 3600;
    /// <summary>
    /// Obtiene o establece el tiempo de expiración en segundos para las URL de descarga.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es 3600 segundos (1 hora). Este valor determina cuánto tiempo
    /// una URL de descarga generada será válida antes de expirar.
    /// </remarks>
    /// <value>
    /// Un entero que representa el tiempo de expiración en segundos.
    /// </value>
    public int DownloadUrlExpiration { get; set; } = 3600;
}