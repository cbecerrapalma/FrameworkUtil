namespace Util.FileStorage;

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="FileStore"/>.
/// </summary>
public static class FileStoreExtensions
{
    /// <summary>
    /// Guarda un archivo de forma asíncrona en un almacenamiento local.
    /// </summary>
    /// <param name="fileStore">La instancia de <see cref="ILocalFileStore"/> donde se guardará el archivo.</param>
    /// <param name="fileInfo">Información del archivo que se desea guardar.</param>
    /// <param name="policy">Política opcional que se aplicará al guardar el archivo.</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Un <see cref="FileResult"/> que representa el resultado de la operación de guardado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="fileStore"/> es nulo.</exception>
    /// <exception cref="FileNotFoundException">Se lanza si la ruta del archivo no es correcta o el archivo no existe.</exception>
    /// <remarks>
    /// Este método verifica si el archivo existe en la ruta especificada antes de intentar guardarlo.
    /// Si el archivo no se encuentra, se lanzará una excepción.
    /// </remarks>
    public static async Task<FileResult> SaveFileAsync(this ILocalFileStore fileStore, FileInfo fileInfo, string policy = null, CancellationToken cancellationToken = default)
    {
        fileStore.CheckNull(nameof(fileStore));
        var path = fileInfo.FullName;
        if (System.IO.File.Exists(path) == false)
            throw new FileNotFoundException("Guardar archivo fallido, la ruta del archivo no es correcta.", path);
        var fileName = System.IO.Path.GetFileName(path);
        await using var stream = Util.Helpers.File.ReadToStream(path);
        return await fileStore.SaveFileAsync(stream, fileName, policy, cancellationToken);
    }

    /// <summary>
    /// Guarda un archivo de forma asíncrona en el almacenamiento local.
    /// </summary>
    /// <param name="fileStore">La instancia de almacenamiento de archivos local donde se guardará el archivo.</param>
    /// <param name="bytes">Un arreglo de bytes que representa el contenido del archivo a guardar.</param>
    /// <param name="fileName">El nombre con el que se guardará el archivo.</param>
    /// <param name="policy">Una política opcional que puede aplicarse al guardar el archivo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación.</param>
    /// <returns>
    /// Un objeto <see cref="FileResult"/> que representa el resultado de la operación de guardado.
    /// </returns>
    /// <remarks>
    /// Este método crea un flujo de memoria a partir del arreglo de bytes y lo utiliza para guardar el archivo
    /// en el almacenamiento local. Asegúrese de que el arreglo de bytes no sea nulo y que el nombre del archivo
    /// sea válido.
    /// </remarks>
    public static async Task<FileResult> SaveFileAsync(this ILocalFileStore fileStore, byte[] bytes, string fileName, string policy = null, CancellationToken cancellationToken = default)
    {
        fileStore.CheckNull(nameof(fileStore));
        await using var stream = new MemoryStream(bytes);
        return await fileStore.SaveFileAsync(stream, fileName, policy, cancellationToken);
    }

    /// <summary>
    /// Obtiene los bytes de un archivo de forma asíncrona desde un almacenamiento de archivos local.
    /// </summary>
    /// <param name="fileStore">La instancia de <see cref="ILocalFileStore"/> desde la cual se obtendrá el archivo.</param>
    /// <param name="fileName">El nombre del archivo que se desea obtener.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="Task{Byte[]}"/> que representa la operación asíncrona, que contiene un arreglo de bytes del archivo solicitado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el método <see cref="ILocalFileStore.GetFileStreamAsync(string, CancellationToken)"/> 
    /// para obtener un flujo del archivo y luego lo convierte en un arreglo de bytes.
    /// </remarks>
    /// <seealso cref="ILocalFileStore"/>
    /// <seealso cref="Util.Helpers.File.ToBytesAsync(Stream, CancellationToken)"/>
    public static async Task<byte[]> GetFileBytesAsync(this ILocalFileStore fileStore, string fileName, CancellationToken cancellationToken = default)
    {
        var stream = await fileStore.GetFileStreamAsync(fileName, cancellationToken);
        await using (stream)
        {
            return await Util.Helpers.File.ToBytesAsync(stream, cancellationToken);
        }
    }
}