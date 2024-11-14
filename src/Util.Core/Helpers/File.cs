namespace Util.Helpers;

/// <summary>
/// Proporciona métodos estáticos para trabajar con archivos.
/// </summary>
public static class File {

    #region ToBytes

    /// <summary>
    /// Convierte un flujo de datos en un arreglo de bytes.
    /// </summary>
    /// <param name="stream">El flujo de datos que se desea convertir a un arreglo de bytes.</param>
    /// <returns>Un arreglo de bytes que contiene los datos del flujo.</returns>
    /// <remarks>
    /// Este método establece la posición del flujo al inicio antes de leer los datos.
    /// Asegúrese de que el flujo sea accesible y tenga datos disponibles para evitar excepciones.
    /// </remarks>
    public static byte[] ToBytes( Stream stream ) {
        stream.Seek( 0, SeekOrigin.Begin );
        var buffer = new byte[stream.Length];
        stream.Read( buffer, 0, buffer.Length );
        return buffer;
    }

    /// <summary>
    /// Convierte una cadena de texto en un arreglo de bytes utilizando la codificación UTF-8.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea convertir a bytes.</param>
    /// <returns>Un arreglo de bytes que representa la cadena de texto en formato UTF-8.</returns>
    public static byte[] ToBytes( string data ) {
        return ToBytes( data, Encoding.UTF8 );
    }

    /// <summary>
    /// Convierte una cadena de texto en un arreglo de bytes utilizando la codificación especificada.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea convertir a bytes.</param>
    /// <param name="encoding">La codificación que se utilizará para la conversión.</param>
    /// <returns>
    /// Un arreglo de bytes que representa la cadena de texto en la codificación especificada.
    /// Si la cadena de texto es nula o está vacía, se devuelve un arreglo vacío.
    /// </returns>
    /// <remarks>
    /// Este método es útil para convertir datos de texto en un formato que puede ser almacenado o transmitido
    /// de manera más eficiente, como en archivos o redes.
    /// </remarks>
    public static byte[] ToBytes( string data, Encoding encoding ) {
        if ( string.IsNullOrWhiteSpace( data ) )
            return Array.Empty<byte>();
        return encoding.GetBytes( data );
    }

    #endregion

    #region ToBytesAsync

    /// <summary>
    /// Convierte un flujo en un arreglo de bytes de manera asíncrona.
    /// </summary>
    /// <param name="stream">El flujo que se desea convertir a un arreglo de bytes.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un arreglo de bytes que representa el contenido del flujo.</returns>
    /// <remarks>
    /// Este método posiciona el flujo al inicio antes de leer su contenido. 
    /// Asegúrese de que el flujo sea accesible y que su longitud sea mayor que cero.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="stream"/> es null.</exception>
    /// <exception cref="ObjectDisposedException">Se lanza si el flujo ha sido cerrado.</exception>
    /// <exception cref="IOException">Se lanza si ocurre un error de entrada/salida durante la lectura del flujo.</exception>
    public static async Task<byte[]> ToBytesAsync( Stream stream, CancellationToken cancellationToken = default ) {
        stream.Seek( 0, SeekOrigin.Begin );
        var buffer = new byte[stream.Length];
        await stream.ReadAsync( buffer, 0, buffer.Length, cancellationToken );
        return buffer;
    }

    #endregion

    #region ToStream

    /// <summary>
    /// Convierte una cadena de texto en un flujo de datos (Stream) utilizando la codificación UTF-8.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea convertir a un flujo.</param>
    /// <returns>Un objeto <see cref="Stream"/> que representa el flujo de datos de la cadena proporcionada.</returns>
    /// <remarks>
    /// Este método es útil para manipular cadenas como flujos, permitiendo su uso en operaciones que requieren un <see cref="Stream"/>.
    /// </remarks>
    public static Stream ToStream( string data ) {
        return ToStream( data, Encoding.UTF8 );
    }

    /// <summary>
    /// Convierte una cadena de texto en un flujo de memoria utilizando la codificación especificada.
    /// </summary>
    /// <param name="data">La cadena de texto que se desea convertir en un flujo.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena en bytes.</param>
    /// <returns>
    /// Un <see cref="Stream"/> que representa la cadena convertida. 
    /// Devuelve <see cref="Stream.Null"/> si la cadena está vacía.
    /// </returns>
    /// <remarks>
    /// Este método es útil para manipular cadenas de texto como flujos, lo que puede ser necesario 
    /// en operaciones de entrada/salida donde se requiere un <see cref="Stream"/>.
    /// </remarks>
    public static Stream ToStream( string data, Encoding encoding ) {
        if ( data.IsEmpty() )
            return Stream.Null;
        return new MemoryStream( ToBytes( data, encoding ) );
    }

    #endregion

    #region FileExists

    /// <summary>
    /// Verifica si un archivo existe en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del archivo que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el archivo existe; de lo contrario, <c>false</c>.
    /// </returns>
    public static bool FileExists( string path ) {
        return System.IO.File.Exists( path );
    }

    #endregion

    #region DirectoryExists

    /// <summary>
    /// Verifica si un directorio existe en la ruta especificada.
    /// </summary>
    /// <param name="path">La ruta del directorio que se desea verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el directorio existe; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="System.IO.Directory"/> para comprobar la existencia del directorio.
    /// </remarks>
    public static bool DirectoryExists( string path ) {
        return Directory.Exists( path );
    }

    #endregion

    #region CreateDirectory

    /// <summary>
    /// Crea un directorio en la ruta especificada si no existe.
    /// </summary>
    /// <param name="path">La ruta del archivo para el cual se desea crear el directorio.</param>
    /// <remarks>
    /// Si la ruta proporcionada está vacía, el método no realizará ninguna acción.
    /// Si el directorio ya existe, no se creará nuevamente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="path"/> es nulo.</exception>
    /// <seealso cref="System.IO.Directory"/>
    /// <seealso cref="System.IO.FileInfo"/>
    public static void CreateDirectory( string path ) {
        if ( path.IsEmpty() )
            return;
        var file = new FileInfo( path );
        var directoryPath = file.Directory?.FullName;
        if ( Directory.Exists( directoryPath ) )
            return;
        Directory.CreateDirectory( directoryPath );
    }

    #endregion

    #region ReadToString

    /// <summary>
    /// Lee el contenido de un archivo y lo devuelve como una cadena de texto.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <returns>El contenido del archivo como una cadena de texto.</returns>
    /// <remarks>
    /// Este método utiliza la codificación UTF-8 de forma predeterminada para leer el archivo.
    /// </remarks>
    public static string ReadToString( string filePath ) {
        return ReadToString( filePath, Encoding.UTF8 );
    }

    /// <summary>
    /// Lee el contenido de un archivo y lo devuelve como una cadena de texto.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <param name="encoding">La codificación que se utilizará para leer el archivo.</param>
    /// <returns>El contenido del archivo como una cadena. Si el archivo no existe, se devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StreamReader"/> para leer el archivo especificado.
    /// Asegúrese de que la ruta del archivo sea correcta y que el archivo sea accesible.
    /// </remarks>
    public static string ReadToString( string filePath, Encoding encoding ) {
        if ( System.IO.File.Exists( filePath ) == false )
            return string.Empty;
        using var reader = new StreamReader( filePath, encoding );
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Lee el contenido de un flujo y lo convierte en una cadena.
    /// </summary>
    /// <param name="stream">El flujo desde el cual se leerá el contenido. No puede ser nulo.</param>
    /// <param name="encoding">La codificación a utilizar para leer el flujo. Si es nulo, se utilizará UTF-8 por defecto.</param>
    /// <param name="bufferSize">El tamaño del búfer a utilizar al leer el flujo. El valor predeterminado es 2048 bytes.</param>
    /// <param name="isCloseStream">Indica si el flujo debe cerrarse después de la lectura. El valor predeterminado es verdadero.</param>
    /// <returns>Una cadena que contiene el contenido leído del flujo. Devuelve una cadena vacía si el flujo es nulo o no se puede leer.</returns>
    /// <remarks>
    /// Este método permite leer el contenido de un flujo de manera eficiente utilizando un búfer. 
    /// Si el flujo es buscable, se restablecerá a la posición inicial después de la lectura.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanzará si el flujo es nulo.</exception>
    public static string ReadToString( Stream stream, Encoding encoding = null, int bufferSize = 1024 * 2, bool isCloseStream = true ) {
        if ( stream == null )
            return string.Empty;
        encoding ??= Encoding.UTF8;
        if ( stream.CanRead == false )
            return string.Empty;
        using var reader = new StreamReader( stream, encoding, true, bufferSize, !isCloseStream );
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        var result = reader.ReadToEnd();
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        return result;
    }

    #endregion

    #region ReadToStringAsync

    /// <summary>
    /// Lee el contenido de un archivo de texto de forma asíncrona y lo devuelve como una cadena.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es el contenido del archivo como una cadena.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la codificación UTF-8 de forma predeterminada para leer el archivo.
    /// </remarks>
    /// <seealso cref="ReadToStringAsync(string, Encoding)"/>
    public static async Task<string> ReadToStringAsync( string filePath ) {
        return await ReadToStringAsync( filePath, Encoding.UTF8 );
    }

    /// <summary>
    /// Lee el contenido de un archivo de forma asíncrona y lo devuelve como una cadena.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <param name="encoding">La codificación que se utilizará para leer el archivo.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de retorno contiene el contenido del archivo como una cadena. 
    /// Si el archivo no existe, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StreamReader"/> para leer el archivo de manera eficiente.
    /// Asegúrese de que la ruta del archivo sea válida y que el archivo sea accesible.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si hay un error de entrada/salida al intentar leer el archivo.
    /// </exception>
    public static async Task<string> ReadToStringAsync( string filePath, Encoding encoding ) {
        if ( System.IO.File.Exists( filePath ) == false )
            return string.Empty;
        using var reader = new StreamReader( filePath, encoding );
        return await reader.ReadToEndAsync();
    }

    #endregion

    #region ReadToStream

    /// <summary>
    /// Lee un archivo desde la ruta especificada y lo devuelve como un flujo de datos.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se desea leer.</param>
    /// <returns>Un flujo de datos que representa el archivo, o null si ocurre un error al abrir el archivo.</returns>
    /// <remarks>
    /// Este método utiliza un bloque try-catch para manejar excepciones que pueden ocurrir al intentar abrir el archivo.
    /// Si el archivo no se puede abrir, el método devolverá null.
    /// </remarks>
    /// <exception cref="System.IO.FileNotFoundException">
    /// Se lanzará si el archivo especificado no se encuentra en la ruta dada.
    /// </exception>
    /// <exception cref="System.UnauthorizedAccessException">
    /// Se lanzará si el acceso al archivo está denegado.
    /// </exception>
    public static Stream ReadToStream( string filePath ) {
        try {
            return new FileStream( filePath, FileMode.Open );
        }
        catch {
            return null;
        }
    }

    #endregion

    #region ReadToBytes

    /// <summary>
    /// Lee el contenido de un archivo y lo convierte en un arreglo de bytes.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <returns>
    /// Un arreglo de bytes que contiene el contenido del archivo, 
    /// o null si el archivo no existe.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="BinaryReader"/> para leer el archivo 
    /// en modo binario. Asegúrese de que la ruta del archivo sea válida 
    /// y que el archivo exista antes de llamar a este método.
    /// </remarks>
    public static byte[] ReadToBytes( string filePath ) {
        if ( !System.IO.File.Exists( filePath ) )
            return null;
        var fileInfo = new FileInfo( filePath );
        using var reader = new BinaryReader( fileInfo.Open( FileMode.Open ) );
        return reader.ReadBytes( (int)fileInfo.Length );
    }

    /// <summary>
    /// Lee todos los bytes de un flujo y los devuelve como un arreglo de bytes.
    /// </summary>
    /// <param name="stream">El flujo desde el cual se leerán los bytes. Este parámetro no puede ser nulo y debe ser un flujo que se pueda leer.</param>
    /// <returns>
    /// Un arreglo de bytes que contiene los datos leídos del flujo. 
    /// Devuelve null si el flujo es nulo o no se puede leer.
    /// </returns>
    /// <remarks>
    /// Si el flujo puede buscar, se reiniciará a la posición inicial después de la lectura.
    /// </remarks>
    public static byte[] ReadToBytes( Stream stream ) {
        if ( stream == null )
            return null;
        if ( stream.CanRead == false )
            return null;
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        var buffer = new byte[stream.Length];
        stream.Read( buffer, 0, buffer.Length );
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        return buffer;
    }

    #endregion

    #region ReadToBytesAsync

    /// <summary>
    /// Lee todos los bytes de un flujo de forma asíncrona y los devuelve como un arreglo de bytes.
    /// </summary>
    /// <param name="stream">El flujo desde el cual se leerán los bytes. No puede ser nulo y debe ser legible.</param>
    /// <param name="cancellationToken">Token opcional para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un arreglo de bytes que contiene los datos leídos del flujo. 
    /// Devuelve <c>null</c> si el flujo es nulo o no es legible.
    /// </returns>
    /// <remarks>
    /// Si el flujo puede buscar, se restablecerá a la posición inicial después de la lectura.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el flujo es nulo.</exception>
    /// <exception cref="ObjectDisposedException">Se lanza si el flujo ha sido cerrado.</exception>
    /// <seealso cref="Stream"/>
    public static async Task<byte[]> ReadToBytesAsync( Stream stream, CancellationToken cancellationToken = default ) {
        if ( stream == null )
            return null;
        if ( stream.CanRead == false )
            return null;
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        var buffer = new byte[stream.Length];
        await stream.ReadAsync( buffer, 0, buffer.Length, cancellationToken );
        if ( stream.CanSeek )
            stream.Seek( 0, SeekOrigin.Begin );
        return buffer;
    }

    #endregion

    #region ReadToMemoryStreamAsync

    /// <summary>
    /// Lee el contenido de un archivo y lo copia en un <see cref="MemoryStream"/> de forma asíncrona.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se va a leer.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Un <see cref="MemoryStream"/> que contiene el contenido del archivo, o <c>null</c> si el archivo no existe o si ocurre un error durante la lectura.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el archivo existe antes de intentar abrirlo. Si el archivo no existe, se devuelve <c>null</c>.
    /// En caso de que ocurra una excepción durante la lectura del archivo, también se devuelve <c>null</c>.
    /// </remarks>
    /// <seealso cref="MemoryStream"/>
    /// <seealso cref="FileStream"/>
    public static async Task<MemoryStream> ReadToMemoryStreamAsync( string filePath, CancellationToken cancellationToken = default ) {
        try {
            if ( FileExists( filePath ) == false )
                return null;
            var memoryStream = new MemoryStream();
            await using var stream = new FileStream( filePath, FileMode.Open );
            await stream.CopyToAsync( memoryStream, cancellationToken ).ConfigureAwait( false );
            return memoryStream;
        }
        catch {
            return null;
        }
    }

    #endregion

    #region Write

    /// <summary>
    /// Escribe el contenido especificado en un archivo en la ruta dada.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="content">El contenido que se escribirá en el archivo.</param>
    /// <remarks>
    /// Si el contenido está vacío, la operación de escritura no se llevará a cabo.
    /// </remarks>
    public static void Write( string filePath, string content ) {
        if ( content.IsEmpty() )
            return;
        Write( filePath, Convert.ToBytes( content ) );
    }

    /// <summary>
    /// Escribe el contenido de un flujo en un archivo especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="content">El flujo que contiene los datos a escribir en el archivo.</param>
    /// <remarks>
    /// Si el flujo de contenido es nulo, el método no realizará ninguna acción.
    /// El flujo se cierra automáticamente al finalizar la escritura.
    /// </remarks>
    public static void Write( string filePath, Stream content ) {
        if ( content == null )
            return;
        using ( content ) {
            var bytes = ToBytes( content );
            Write( filePath, bytes );
        }
    }

    /// <summary>
    /// Escribe un arreglo de bytes en un archivo especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="content">El arreglo de bytes que se escribirá en el archivo.</param>
    /// <remarks>
    /// Este método verifica si la ruta del archivo es nula o está en blanco, así como si el contenido es nulo.
    /// Si alguna de estas condiciones se cumple, el método no realiza ninguna acción.
    /// Antes de escribir el contenido, se asegura de que el directorio del archivo exista, llamando a <see cref="CreateDirectory(string)"/>.
    /// </remarks>
    /// <seealso cref="CreateDirectory(string)"/>
    public static void Write( string filePath, byte[] content ) {
        if ( string.IsNullOrWhiteSpace( filePath ) )
            return;
        if ( content == null )
            return;
        CreateDirectory( filePath );
        System.IO.File.WriteAllBytes( filePath, content );
    }

    #endregion

    #region WriteAsync

    /// <summary>
    /// Escribe de manera asíncrona el contenido en un archivo especificado.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="content">El contenido que se escribirá en el archivo.</param>
    /// <param name="cancellationToken">Token de cancelación opcional para cancelar la operación.</param>
    /// <remarks>
    /// Si el contenido está vacío, la operación no realizará ninguna acción.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de escritura en el archivo.
    /// </returns>
    /// <seealso cref="WriteAsync(string, byte[], CancellationToken)"/>
    public static async Task WriteAsync( string filePath, string content, CancellationToken cancellationToken = default ) {
        if ( content.IsEmpty() )
            return;
        await WriteAsync( filePath, Convert.ToBytes( content ), cancellationToken );
    }

    /// <summary>
    /// Escribe de manera asíncrona el contenido de un flujo en un archivo especificado.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido.</param>
    /// <param name="content">El flujo que contiene los datos que se escribirán en el archivo.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación asíncrona de escritura.</returns>
    /// <remarks>
    /// Este método verifica si el flujo de contenido es nulo y, en caso afirmativo, no realiza ninguna acción.
    /// Si el flujo no es nulo, se convierte a un arreglo de bytes y se escribe en el archivo especificado.
    /// </remarks>
    /// <seealso cref="ToBytesAsync(Stream, CancellationToken)"/>
    /// <seealso cref="WriteAsync(string, byte[], CancellationToken)"/>
    public static async Task WriteAsync( string filePath, Stream content, CancellationToken cancellationToken = default ) {
        if ( content == null )
            return;
        await using ( content ) {
            var bytes = await ToBytesAsync( content, cancellationToken );
            await WriteAsync( filePath, bytes, cancellationToken );
        }
    }

    /// <summary>
    /// Escribe de manera asíncrona un arreglo de bytes en un archivo especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo donde se escribirá el contenido. No puede ser nula o estar vacía.</param>
    /// <param name="content">El arreglo de bytes que se escribirá en el archivo. No puede ser nulo.</param>
    /// <param name="cancellationToken">Un token de cancelación que puede ser utilizado para cancelar la operación asíncrona. Por defecto es <see cref="CancellationToken.None"/>.</param>
    /// <returns>Una tarea que representa la operación asíncrona de escritura en el archivo.</returns>
    /// <remarks>
    /// Este método crea el directorio necesario para la ruta del archivo si no existe.
    /// Si la ruta del archivo o el contenido son inválidos (nulos o vacíos), el método no realiza ninguna acción.
    /// </remarks>
    /// <seealso cref="System.IO.File.WriteAllBytesAsync(string, byte[], CancellationToken)"/>
    public static async Task WriteAsync( string filePath, byte[] content, CancellationToken cancellationToken = default ) {
        if ( string.IsNullOrWhiteSpace( filePath ) )
            return;
        if ( content == null )
            return;
        CreateDirectory( filePath );
        await System.IO.File.WriteAllBytesAsync( filePath, content, cancellationToken );
    }

    #endregion

    #region Delete

    /// <summary>
    /// Elimina los archivos especificados en la colección de rutas de archivo.
    /// </summary>
    /// <param name="filePaths">Una colección de rutas de archivo que se van a eliminar.</param>
    /// <remarks>
    /// Este método itera sobre cada ruta de archivo en la colección y llama al método 
    /// <see cref="Delete(string)"/> para eliminar cada archivo individualmente.
    /// </remarks>
    /// <seealso cref="Delete(string)"/>
    public static void Delete( IEnumerable<string> filePaths ) {
        foreach ( var filePath in filePaths )
            Delete( filePath );
    }

    /// <summary>
    /// Elimina un archivo del sistema de archivos especificado por la ruta.
    /// </summary>
    /// <param name="filePath">La ruta del archivo que se desea eliminar.</param>
    /// <remarks>
    /// Este método verifica si la ruta del archivo es nula, vacía o solo contiene espacios en blanco.
    /// Si la ruta es válida y el archivo existe, se procederá a eliminarlo.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si ocurre un error de entrada/salida al intentar eliminar el archivo.
    /// </exception>
    /// <exception cref="System.UnauthorizedAccessException">
    /// Se produce si el proceso no tiene permiso para eliminar el archivo.
    /// </exception>
    public static void Delete( string filePath ) {
        if ( string.IsNullOrWhiteSpace( filePath ) )
            return;
        if ( System.IO.File.Exists( filePath ) )
            System.IO.File.Delete( filePath );
    }

    #endregion

    #region GetAllFiles

    /// <summary>
    /// Obtiene una lista de todos los archivos en un directorio especificado y sus subdirectorios que coinciden con un patrón de búsqueda.
    /// </summary>
    /// <param name="path">La ruta del directorio donde se buscarán los archivos.</param>
    /// <param name="searchPattern">El patrón de búsqueda que se utilizará para filtrar los archivos. Por ejemplo, "*.txt" para buscar archivos de texto.</param>
    /// <returns>Una lista de objetos <see cref="FileInfo"/> que representan los archivos encontrados.</returns>
    /// <remarks>
    /// Este método busca de manera recursiva en todos los subdirectorios del directorio especificado.
    /// Si no se encuentran archivos que coincidan con el patrón, se devolverá una lista vacía.
    /// </remarks>
    public static List<FileInfo> GetAllFiles( string path, string searchPattern ) {
        return Directory.GetFiles( path, searchPattern, SearchOption.AllDirectories )
            .Select( filePath => new FileInfo( filePath ) ).ToList();
    }

    #endregion

    #region Copy

    /// <summary>
    /// Copia un archivo de una ubicación a otra.
    /// </summary>
    /// <param name="sourceFilePath">La ruta del archivo de origen que se desea copiar.</param>
    /// <param name="destinationFilePath">La ruta donde se desea copiar el archivo.</param>
    /// <param name="overwrite">Indica si se debe sobrescribir el archivo de destino si ya existe. El valor predeterminado es <c>false</c>.</param>
    /// <remarks>
    /// Este método verifica si las rutas de origen y destino son válidas y si el archivo de origen existe antes de realizar la copia.
    /// Si el archivo de destino ya existe y el parámetro <paramref name="overwrite"/> es <c>false</c>, la copia no se realizará.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si hay un error de entrada/salida durante la operación de copia.
    /// </exception>
    /// <seealso cref="System.IO.File.Copy(string, string, bool)"/>
    public static void Copy( string sourceFilePath, string destinationFilePath, bool overwrite = false ) {
        if ( sourceFilePath.IsEmpty() || destinationFilePath.IsEmpty() )
            return;
        if ( FileExists( sourceFilePath ) == false )
            return;
        CreateDirectory( destinationFilePath );
        System.IO.File.Copy( sourceFilePath, destinationFilePath, overwrite );
    }

    #endregion

    #region Move

    /// <summary>
    /// Mueve un archivo de una ubicación a otra.
    /// </summary>
    /// <param name="sourceFilePath">La ruta del archivo de origen que se desea mover.</param>
    /// <param name="destinationFilePath">La ruta de destino donde se moverá el archivo.</param>
    /// <param name="overwrite">Indica si se debe sobrescribir el archivo de destino si ya existe. El valor predeterminado es false.</param>
    /// <remarks>
    /// Este método verifica si las rutas de origen y destino son válidas y si el archivo de origen existe antes de intentar moverlo.
    /// Si el archivo de destino ya existe y el parámetro <paramref name="overwrite"/> es true, se sobrescribirá.
    /// </remarks>
    /// <exception cref="System.IO.IOException">
    /// Se produce si el archivo de origen no se puede mover o si hay un problema con el sistema de archivos.
    /// </exception>
    /// <seealso cref="System.IO.File"/>
    public static void Move( string sourceFilePath, string destinationFilePath, bool overwrite = false ) {
        if ( sourceFilePath.IsEmpty() || destinationFilePath.IsEmpty() )
            return;
        if ( FileExists( sourceFilePath ) == false )
            return;
        CreateDirectory( destinationFilePath );
        System.IO.File.Move( sourceFilePath, destinationFilePath, overwrite );
    }

    #endregion
}