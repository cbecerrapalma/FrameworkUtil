namespace Util.FileStorage; 

/// <summary>
/// Interfaz que define un contrato para el almacenamiento de archivos.
/// Esta interfaz hereda de <see cref="ILocalFileStore"/> y puede incluir métodos adicionales
/// para la gestión de archivos en un sistema de almacenamiento.
/// </summary>
public interface IFileStore : ILocalFileStore {
    /// <summary>
    /// Asynchronously obtiene una lista de nombres de buckets.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una lista de nombres de buckets como resultado.</returns>
    /// <remarks>
    /// Este método permite recuperar los nombres de los buckets de forma asíncrona, lo que es útil para evitar bloquear el hilo de ejecución.
    /// Asegúrese de manejar adecuadamente el token de cancelación para permitir la cancelación de la operación si es necesario.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<List<string>> GetBucketNamesAsync( CancellationToken cancellationToken = default );
    /// <summary>
    /// Verifica si un bucket existe en el almacenamiento.
    /// </summary>
    /// <param name="bucketName">El nombre del bucket que se desea verificar.</param>
    /// <param name="policy">La política opcional que se aplicará al bucket. Si no se proporciona, se utilizará la política predeterminada.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es <c>true</c> si el bucket existe; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite a los usuarios verificar la existencia de un bucket en el sistema de almacenamiento.
    /// Se puede proporcionar una política específica para la verificación, aunque no es obligatorio.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<bool> BucketExistsAsync( string bucketName,string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Crea un nuevo bucket en el almacenamiento.
    /// </summary>
    /// <param name="bucketName">El nombre del bucket que se va a crear.</param>
    /// <param name="policy">La política opcional que se aplicará al bucket. Si no se proporciona, se utilizará la política predeterminada.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de creación del bucket.</returns>
    /// <remarks>
    /// Este método permite crear un bucket en el sistema de almacenamiento especificado. 
    /// Asegúrese de que el nombre del bucket sea único y cumpla con las normas de nomenclatura.
    /// </remarks>
    /// <seealso cref="DeleteBucketAsync(string, CancellationToken)"/>
    /// <seealso cref="ListBucketsAsync(CancellationToken)"/>
    Task CreateBucketAsync( string bucketName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un bucket especificado por su nombre.
    /// </summary>
    /// <param name="bucketName">El nombre del bucket que se desea eliminar.</param>
    /// <param name="policy">Una política opcional que se aplicará al eliminar el bucket. Si no se proporciona, se utilizará la política predeterminada.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación del bucket.</returns>
    /// <remarks>
    /// Este método permite eliminar un bucket de forma asíncrona. Asegúrese de que el bucket esté vacío antes de intentar eliminarlo,
    /// ya que algunos servicios pueden requerir que no haya objetos en el bucket para permitir su eliminación.
    /// </remarks>
    /// <seealso cref="CreateBucketAsync(string)"/>
    /// <seealso cref="ListBucketsAsync()"/>
    Task DeleteBucketAsync( string bucketName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Verifica de manera asíncrona si un archivo existe en la ubicación especificada.
    /// </summary>
    /// <param name="args">Los argumentos que contienen la información necesaria para verificar la existencia del archivo.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea es <c>true</c> si el archivo existe; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite comprobar la existencia de un archivo sin bloquear el hilo de ejecución actual.
    /// Asegúrese de manejar adecuadamente el <paramref name="cancellationToken"/> para permitir la cancelación de la operación si es necesario.
    /// </remarks>
    /// <seealso cref="FileExistsArgs"/>
    Task<bool> FileExistsAsync( FileExistsArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un flujo de archivo de manera asíncrona.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para obtener el flujo del archivo.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Un <see cref="Task{Stream}"/> que representa la operación asíncrona, con un flujo de archivo como resultado.</returns>
    /// <remarks>
    /// Este método permite la recuperación de un archivo en forma de flujo, lo que facilita la lectura o manipulación del contenido del archivo sin necesidad de cargarlo completamente en memoria.
    /// </remarks>
    /// <seealso cref="GetFileStreamArgs"/>
    Task<Stream> GetFileStreamAsync( GetFileStreamArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda un archivo de forma asíncrona.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para guardar el archivo.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto <see cref="FileResult"/> que representa el resultado de la operación de guardado.</returns>
    /// <remarks>
    /// Este método permite guardar un archivo en el sistema de archivos. 
    /// Se debe proporcionar un objeto <paramref name="args"/> que contenga la información necesaria, 
    /// como la ruta de destino y el contenido del archivo.
    /// </remarks>
    /// <seealso cref="SaveFileArgs"/>
    /// <seealso cref="FileResult"/>
    Task<FileResult> SaveFileAsync( SaveFileArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda un archivo a partir de una URL proporcionada.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para guardar el archivo, incluyendo la URL y la ruta de destino.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto <see cref="FileResult"/> que representa el resultado de la operación de guardado.</returns>
    /// <remarks>
    /// Este método realiza una operación asíncrona para descargar el archivo desde la URL especificada
    /// y guardarlo en la ubicación indicada. Es importante manejar adecuadamente el token de cancelación
    /// para permitir la interrupción de la operación si es necesario.
    /// </remarks>
    Task<FileResult> SaveFileByUrlAsync( SaveFileByUrlArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Copia un archivo de una ubicación de almacenamiento a otra de manera asíncrona.
    /// </summary>
    /// <param name="sourceArgs">Los argumentos que especifican la ubicación del archivo de origen.</param>
    /// <param name="destinationArgs">Los argumentos que especifican la ubicación del archivo de destino.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de copia asíncrona.</returns>
    /// <remarks>
    /// Este método permite copiar archivos de manera eficiente, utilizando el patrón asíncrono para no bloquear el hilo de ejecución.
    /// Asegúrese de que las rutas de origen y destino sean válidas y que se tengan los permisos necesarios para realizar la operación.
    /// </remarks>
    /// <seealso cref="FileStorageArgs"/>
    /// <seealso cref="CancellationToken"/>
    Task CopyFileAsync( FileStorageArgs sourceArgs, FileStorageArgs destinationArgs, CancellationToken cancellationToken = default );
    /// <summary>
    /// Mueve un archivo de una ubicación a otra de manera asíncrona.
    /// </summary>
    /// <param name="sourceArgs">Los argumentos que especifican la ubicación del archivo de origen.</param>
    /// <param name="destinationArgs">Los argumentos que especifican la ubicación del archivo de destino.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de mover el archivo.</returns>
    /// <remarks>
    /// Este método permite mover archivos entre diferentes ubicaciones de almacenamiento.
    /// Asegúrese de que las rutas de origen y destino sean válidas y que se tengan los permisos necesarios.
    /// </remarks>
    /// <seealso cref="FileStorageArgs"/>
    Task MoveFileAsync( FileStorageArgs sourceArgs, FileStorageArgs destinationArgs, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un archivo de manera asíncrona.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para eliminar el archivo.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación del archivo.</returns>
    /// <remarks>
    /// Este método permite eliminar un archivo especificado en los argumentos.
    /// Asegúrese de que el archivo existe antes de llamar a este método para evitar excepciones.
    /// </remarks>
    /// <seealso cref="DeleteFileArgs"/>
    Task DeleteFileAsync( DeleteFileArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL de descarga para un archivo específico.
    /// </summary>
    /// <param name="fileName">El nombre del archivo para el cual se generará la URL de descarga.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene la URL de descarga generada como una cadena.</returns>
    /// <remarks>
    /// Este método es asíncrono y puede ser cancelado mediante el <paramref name="cancellationToken"/>.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan ocurrir durante la generación de la URL.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task<string> GenerateDownloadUrlAsync( string fileName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL de descarga de forma asíncrona.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para generar la URL de descarga.</param>
    /// <param name="cancellationToken">Un token de cancelación opcional para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con una cadena que contiene la URL de descarga generada.</returns>
    /// <remarks>
    /// Este método permite obtener una URL de descarga que puede ser utilizada para acceder a un recurso específico.
    /// Asegúrese de proporcionar los argumentos correctos para que la URL se genere correctamente.
    /// </remarks>
    /// <seealso cref="GenerateDownloadUrlArgs"/>
    Task<string> GenerateDownloadUrlAsync( GenerateDownloadUrlArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL temporal para la descarga de un archivo especificado.
    /// </summary>
    /// <param name="fileName">El nombre del archivo para el cual se generará la URL de descarga.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene la URL temporal para la descarga del archivo.</returns>
    /// <remarks>
    /// Esta función es útil para crear enlaces de descarga que expiran después de un cierto tiempo, 
    /// proporcionando una capa adicional de seguridad al acceso a los archivos.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="fileName"/> es nulo o vacío.</exception>
    /// <seealso cref="CancellationToken"/>
    Task<string> GenerateTempDownloadUrlAsync( string fileName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL temporal para la descarga de un recurso.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para generar la URL temporal.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica, con una cadena que contiene la URL temporal generada.</returns>
    /// <remarks>
    /// Este método permite a los usuarios obtener una URL que puede ser utilizada para descargar un recurso específico.
    /// La URL generada es válida por un tiempo limitado y puede incluir restricciones de acceso.
    /// </remarks>
    /// <seealso cref="GenerateTempDownloadUrlArgs"/>
    Task<string> GenerateTempDownloadUrlAsync( GenerateTempDownloadUrlArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL para la carga de un archivo de forma asíncrona.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a cargar.</param>
    /// <param name="policy">Una política opcional que puede ser utilizada para definir restricciones adicionales sobre la carga.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado de tipo <see cref="DirectUploadParam"/> que contiene los parámetros de carga.</returns>
    /// <remarks>
    /// Este método permite a los usuarios generar una URL que puede ser utilizada para cargar un archivo directamente a un servidor.
    /// Se recomienda manejar adecuadamente el token de cancelación para evitar cargas innecesarias si la operación es cancelada.
    /// </remarks>
    /// <seealso cref="DirectUploadParam"/>
    Task<DirectUploadParam> GenerateUploadUrlAsync( string fileName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Genera una URL para la carga directa de archivos.
    /// </summary>
    /// <param name="args">Los argumentos necesarios para generar la URL de carga.</param>
    /// <param name="cancellationToken">Un token para la cancelación de la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea contiene un objeto <see cref="DirectUploadParam"/> que incluye la URL generada.</returns>
    /// <remarks>
    /// Este método permite a los usuarios obtener una URL que puede ser utilizada para cargar archivos directamente a un servidor,
    /// evitando la necesidad de pasar por el backend de la aplicación.
    /// </remarks>
    /// <seealso cref="DirectUploadParam"/>
    /// <seealso cref="GenerateUploadUrlArgs"/>
    Task<DirectUploadParam> GenerateUploadUrlAsync( GenerateUploadUrlArgs args, CancellationToken cancellationToken = default );
    /// <summary>
    /// Realiza una operación asíncrona para limpiar los recursos o datos.
    /// </summary>
    /// <param name="cancellationToken">
    /// Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es un valor booleano que indica si la limpieza se realizó con éxito.
    /// </returns>
    /// <remarks>
    /// Este método puede ser llamado en un contexto asíncrono y se recomienda manejar la cancelación adecuadamente.
    /// </remarks>
    /// <seealso cref="CancellationToken"/>
    Task ClearAsync( CancellationToken cancellationToken = default );
}