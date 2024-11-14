namespace Util.FileStorage; 

/// <summary>
/// Representa un tamaño de archivo en bytes, permitiendo la conversión entre diferentes unidades de medida.
/// </summary>
public readonly struct FileSize {
    private readonly long _size;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileSize"/>.
    /// </summary>
    /// <param name="size">El tamaño del archivo en la unidad especificada.</param>
    /// <param name="unit">La unidad de medida del tamaño del archivo. Por defecto es <see cref="FileSizeUnit.Byte"/>.</param>
    public FileSize( long size, FileSizeUnit unit = FileSizeUnit.Byte ) {
        _size = GetSize( size, unit );
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileSize"/>.
    /// </summary>
    /// <param name="size">El tamaño del archivo en la unidad especificada.</param>
    /// <param name="unit">La unidad de medida del tamaño del archivo. Por defecto es <see cref="FileSizeUnit.Byte"/>.</param>
    /// <remarks>
    /// Este constructor convierte el tamaño proporcionado a bytes utilizando la unidad especificada.
    /// </remarks>
    public FileSize( double size, FileSizeUnit unit = FileSizeUnit.Byte ) {
        _size = Util.Helpers.Convert.ToLong( GetSize( size, unit ) );
    }

    /// <summary>
    /// Convierte un tamaño de archivo a bytes según la unidad especificada.
    /// </summary>
    /// <param name="size">El tamaño a convertir.</param>
    /// <param name="unit">La unidad del tamaño a convertir. Puede ser K (kilobytes), M (megabytes), G (gigabytes) o bytes.</param>
    /// <returns>
    /// El tamaño convertido a bytes.
    /// </returns>
    /// <remarks>
    /// Este método multiplica el tamaño proporcionado por el factor correspondiente a la unidad especificada.
    /// Por ejemplo, si la unidad es K, el tamaño se multiplica por 1024; si es M, por 1024^2; y si es G, por 1024^3.
    /// Si la unidad no es reconocida, se devuelve el tamaño original sin cambios.
    /// </remarks>
    private static long GetSize( long size, FileSizeUnit unit ) {
        switch ( unit ) {
            case FileSizeUnit.K:
                return size * 1024;
            case FileSizeUnit.M:
                return size * 1024 * 1024;
            case FileSizeUnit.G:
                return size * 1024 * 1024 * 1024;
            default:
                return size;
        }
    }

    /// <summary>
    /// Convierte un tamaño de archivo a bytes según la unidad especificada.
    /// </summary>
    /// <param name="size">El tamaño a convertir.</param>
    /// <param name="unit">La unidad del tamaño a convertir, que puede ser kilobytes (K), megabytes (M) o gigabytes (G).</param>
    /// <returns>El tamaño convertido a bytes.</returns>
    /// <remarks>
    /// Este método multiplica el tamaño proporcionado por el factor correspondiente a la unidad especificada.
    /// Si la unidad no es reconocida, se devuelve el tamaño original sin cambios.
    /// </remarks>
    private static double GetSize( double size, FileSizeUnit unit ) {
        switch( unit ) {
            case FileSizeUnit.K:
                return size * 1024;
            case FileSizeUnit.M:
                return size * 1024 * 1024;
            case FileSizeUnit.G:
                return size * 1024 * 1024 * 1024;
            default:
                return size;
        }
    }

    /// <summary>
    /// Obtiene el tamaño actual.
    /// </summary>
    /// <value>
    /// El tamaño actual como un valor de tipo <see cref="long"/>.
    /// </value>
    public long Size => _size;

    /// <summary>
    /// Obtiene el tamaño de un objeto.
    /// </summary>
    /// <returns>
    /// Un entero que representa el tamaño del objeto.
    /// </returns>
    public int GetSize() {
        return Util.Helpers.Convert.ToInt( Size );
    }

    /// <summary>
    /// Obtiene el tamaño en kilobytes (KB) a partir del tamaño en bytes.
    /// </summary>
    /// <returns>
    /// El tamaño en kilobytes, representado como un valor de tipo <see cref="double"/>.
    /// </returns>
    public double GetSizeByK() {
        return Util.Helpers.Convert.ToDouble( _size / 1024.0, 2 );
    }

    /// <summary>
    /// Obtiene el tamaño en megabytes (MB) a partir de un tamaño en bytes.
    /// </summary>
    /// <returns>
    /// El tamaño en megabytes como un valor de tipo <see cref="double"/>.
    /// </returns>
    public double GetSizeByM() {
        return Util.Helpers.Convert.ToDouble( _size / 1024.0 / 1024.0, 2 );
    }

    /// <summary>
    /// Obtiene el tamaño en gigabytes (GB).
    /// </summary>
    /// <returns>
    /// El tamaño convertido a gigabytes como un valor de tipo <see cref="double"/>.
    /// </returns>
    /// <remarks>
    /// Este método divide el tamaño almacenado en la variable privada <c>_size</c> 
    /// por 1024 tres veces para convertirlo de bytes a gigabytes. 
    /// Se utiliza la función <c>Convert.ToDouble</c> para asegurar que el resultado 
    /// sea un número de punto flotante con dos decimales.
    /// </remarks>
    public double GetSizeByG() {
        return Util.Helpers.Convert.ToDouble( _size / 1024.0 / 1024.0 / 1024.0, 2 );
    }

    /// <summary>
    /// Devuelve una representación en forma de cadena del tamaño del archivo,
    /// formateada según las unidades de medida apropiadas (bytes, kilobytes, megabytes, gigabytes).
    /// </summary>
    /// <returns>
    /// Una cadena que representa el tamaño del archivo en la unidad correspondiente.
    /// </returns>
    /// <remarks>
    /// Este método verifica el tamaño del archivo y lo convierte a la unidad más adecuada:
    /// - Gigabytes si el tamaño es mayor o igual a 1 GB.
    /// - Megabytes si el tamaño es mayor o igual a 1 MB.
    /// - Kilobytes si el tamaño es mayor o igual a 1 KB.
    /// - Bytes en caso contrario.
    /// </remarks>
    public override string ToString() {
        if ( _size >= 1024 * 1024 * 1024 )
            return $"{GetSizeByG()} {FileSizeUnit.G.Description()}";
        if ( _size >= 1024 * 1024 )
            return $"{GetSizeByM()} {FileSizeUnit.M.Description()}";
        if ( _size >= 1024 )
            return $"{GetSizeByK()} {FileSizeUnit.K.Description()}";
        return $"{_size} {FileSizeUnit.Byte.Description()}";
    }
}