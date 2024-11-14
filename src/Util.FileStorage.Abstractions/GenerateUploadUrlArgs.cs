namespace Util.FileStorage; 

/// <summary>
/// Clase que representa los argumentos necesarios para generar una URL de carga.
/// Hereda de <see cref="FileStorageArgs"/>.
/// </summary>
public class GenerateUploadUrlArgs : FileStorageArgs {
    private readonly Dictionary<string, string> _headers;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GenerateUploadUrlArgs"/>.
    /// </summary>
    /// <param name="fileName">El nombre del archivo para el cual se generará la URL de carga.</param>
    /// <remarks>
    /// Este constructor llama al constructor base con el nombre del archivo y 
    /// también inicializa un diccionario para almacenar los encabezados relacionados 
    /// con la carga del archivo.
    /// </remarks>
    public GenerateUploadUrlArgs( string fileName ) : base( fileName ) {
        _headers = new Dictionary<string, string>();
    }

    /// <summary>
    /// Establece el tipo de contenido como "application/octet-stream".
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para indicar que el contenido que se está manejando es un flujo de bytes
    /// genérico, lo que puede ser útil para la descarga de archivos o la transferencia de datos binarios.
    /// </remarks>
    public void SetOctetStream() {
        SetContentType( "application/octet-stream" );
    }

    /// <summary>
    /// Establece el tipo de contenido para la respuesta.
    /// </summary>
    /// <param name="contentType">El tipo de contenido que se va a establecer, por ejemplo, "text/html" o "application/json".</param>
    public void SetContentType( string contentType ) {
        AddHeader( "content-type", contentType );
    }

    /// <summary>
    /// Agrega un encabezado con la clave y el valor especificados.
    /// Si la clave ya existe, se elimina el encabezado existente antes de agregar el nuevo.
    /// </summary>
    /// <param name="key">La clave del encabezado que se va a agregar.</param>
    /// <param name="value">El valor del encabezado que se va a agregar.</param>
    public void AddHeader( string key,string value ) {
        if ( _headers.ContainsKey( key ) )
            _headers.Remove( key );
        _headers.Add( key, value );
    }

    /// <summary>
    /// Obtiene los encabezados almacenados.
    /// </summary>
    /// <returns>
    /// Un diccionario que contiene los encabezados, donde la clave es el nombre del encabezado y el valor es su contenido.
    /// </returns>
    public IDictionary<string, string> GetHeaders() {
        return _headers;
    }

    /// <summary>
    /// Obtiene o establece el límite de tamaño.
    /// </summary>
    /// <remarks>
    /// Este límite se utiliza para restringir el tamaño de un recurso específico.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="long"/> que representa el límite de tamaño.
    /// </value>
    public long SizeLimit { get; set; }
}