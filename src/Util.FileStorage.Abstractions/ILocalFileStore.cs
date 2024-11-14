namespace Util.FileStorage; 

/// <summary>
/// Interfaz que define las operaciones para el almacenamiento de archivos locales.
/// </summary>
public interface ILocalFileStore {
    /// <summary>
    /// Verifica de manera asíncrona si un archivo existe en el sistema.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se desea verificar.</param>
    /// <param name="policy">Una política opcional que puede influir en la verificación del archivo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea es <c>true</c> si el archivo existe; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método permite verificar la existencia de un archivo de manera no bloqueante,
    /// lo que es útil en aplicaciones que requieren una alta capacidad de respuesta.
    /// </remarks>
    /// <seealso cref="System.IO.File.Exists(string)"/>
    Task<bool> FileExistsAsync( string fileName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Obtiene un flujo de archivo de manera asíncrona.
    /// </summary>
    /// <param name="fileName">El nombre del archivo del cual se desea obtener el flujo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. El valor predeterminado es <c>default</c>.</param>
    /// <returns>Una tarea que representa la operación asíncrona. El valor de la tarea es un <see cref="Stream"/> que representa el flujo del archivo.</returns>
    /// <remarks>
    /// Este método permite acceder a un archivo de forma asíncrona, lo que es útil para evitar bloqueos en la interfaz de usuario o en otros procesos mientras se realiza la operación de entrada/salida.
    /// </remarks>
    /// <seealso cref="Stream"/>
    Task<Stream> GetFileStreamAsync( string fileName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda un archivo de forma asíncrona en un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos que contiene el contenido del archivo a guardar.</param>
    /// <param name="fileName">El nombre del archivo que se va a guardar.</param>
    /// <param name="policy">Una política opcional que puede ser utilizada para definir reglas de almacenamiento o acceso.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que contiene el resultado del archivo guardado.</returns>
    /// <remarks>
    /// Este método permite guardar archivos en un almacenamiento específico, utilizando un flujo de datos
    /// para obtener el contenido del archivo. Se puede especificar una política para gestionar el acceso
    /// o las reglas de almacenamiento del archivo.
    /// </remarks>
    /// <seealso cref="FileResult"/>
    Task<FileResult> SaveFileAsync( Stream stream, string fileName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Guarda un archivo a partir de una URL especificada.
    /// </summary>
    /// <param name="url">La URL desde la cual se descargará el archivo.</param>
    /// <param name="fileName">El nombre con el que se guardará el archivo en el sistema.</param>
    /// <param name="policy">Una política opcional que puede ser utilizada para la descarga del archivo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asincrónica de guardar el archivo. El resultado es un objeto <see cref="FileResult"/> que representa el archivo guardado.</returns>
    /// <remarks>
    /// Este método permite descargar un archivo de una URL y guardarlo localmente con el nombre especificado.
    /// Si se proporciona una política, esta se aplicará durante la descarga.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="url"/> o <paramref name="fileName"/> son null.</exception>
    /// <exception cref="HttpRequestException">Se lanza si ocurre un error al realizar la solicitud HTTP para descargar el archivo.</exception>
    /// <seealso cref="FileResult"/>
    Task<FileResult> SaveFileByUrlAsync( string url, string fileName, string policy = null, CancellationToken cancellationToken = default );
    /// <summary>
    /// Copia un archivo de forma asíncrona desde la ubicación especificada en <paramref name="sourceFileName"/> 
    /// a la ubicación especificada en <paramref name="destinationFileName"/>.
    /// </summary>
    /// <param name="sourceFileName">La ruta del archivo de origen que se va a copiar.</param>
    /// <param name="destinationFileName">La ruta del archivo de destino donde se copiará el archivo.</param>
    /// <param name="cancellationToken">Un token que puede ser utilizado para cancelar la operación de copia.</param>
    /// <returns>Una tarea que representa la operación de copia asíncrona.</returns>
    /// <remarks>
    /// Este método permite la copia de archivos de manera asíncrona, lo que puede mejorar la 
    /// capacidad de respuesta de la aplicación al evitar bloqueos en el hilo principal. 
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la operación.
    /// </remarks>
    /// <seealso cref="System.IO.File"/>
    /// <seealso cref="System.Threading.CancellationToken"/>
    Task CopyFileAsync( string sourceFileName, string destinationFileName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Mueve un archivo de una ubicación a otra de manera asíncrona.
    /// </summary>
    /// <param name="sourceFileName">La ruta del archivo de origen que se desea mover.</param>
    /// <param name="destinationFileName">La ruta del archivo de destino donde se moverá el archivo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de mover el archivo.</returns>
    /// <remarks>
    /// Este método moverá el archivo especificado en <paramref name="sourceFileName"/> a la ubicación especificada en <paramref name="destinationFileName"/>.
    /// Si el archivo de destino ya existe, se sobrescribirá.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Se lanza si el archivo de origen no se encuentra.</exception>
    /// <exception cref="IOException">Se lanza si ocurre un error de entrada/salida durante el movimiento del archivo.</exception>
    /// <seealso cref="CancellationToken"/>
    Task MoveFileAsync( string sourceFileName, string destinationFileName, CancellationToken cancellationToken = default );
    /// <summary>
    /// Elimina un archivo de forma asíncrona.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se desea eliminar.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona de eliminación del archivo.</returns>
    /// <remarks>
    /// Este método permite eliminar un archivo especificado por su nombre. Si el archivo no existe, no se generará una excepción.
    /// Asegúrese de manejar adecuadamente el token de cancelación para poder cancelar la operación si es necesario.
    /// </remarks>
    /// <seealso cref="System.IO.File.Delete(string)"/>
    Task DeleteFileAsync( string fileName, CancellationToken cancellationToken = default );
}