namespace Util.FileStorage.Signatures;

/// <summary>
/// Clase que implementa la interfaz <see cref="IFileExtensionInspector"/>.
/// Proporciona funcionalidades para inspeccionar extensiones de archivos.
/// </summary>
public class FileExtensionInspector : IFileExtensionInspector {
    private readonly FileFormatInspector _inspector;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="FileExtensionInspector"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se encarga de inspeccionar las extensiones de archivo utilizando
    /// una instancia de <see cref="FileFormatInspector"/> para determinar el formato
    /// de los archivos.
    /// </remarks>
    public FileExtensionInspector() {
        _inspector = new FileFormatInspector();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la extensión del archivo a partir de un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos del cual se determinará la extensión del archivo. No puede ser nulo.</param>
    /// <returns>
    /// La extensión del archivo como una cadena, o null si el flujo es nulo o si no se puede determinar la extensión.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un inspector para determinar el formato del archivo basado en el contenido del flujo.
    /// </remarks>
    /// <seealso cref="Stream"/>
    public string GetExtension( Stream stream ) {
        if ( stream == null )
            return null;
        var result = _inspector.DetermineFileFormat( stream );
        return result?.Extension;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el flujo proporcionado representa una imagen.
    /// </summary>
    /// <param name="stream">El flujo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo representa una imagen; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un inspector para determinar el formato del archivo basado en el flujo.
    /// Si el flujo es nulo, se devuelve <c>false</c> inmediatamente.
    /// </remarks>
    /// <seealso cref="Stream"/>
    public bool IsImage( Stream stream ) {
        if( stream == null )
            return false;
        var result = _inspector.DetermineFileFormat( stream );
        return result is Image;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el flujo de datos proporcionado corresponde a un archivo PDF.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo de datos representa un archivo PDF; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un inspector para determinar el formato del archivo a partir del flujo de datos.
    /// Si el flujo es nulo, se considera que no es un PDF.
    /// </remarks>
    /// <seealso cref="Stream"/>
    public bool IsPdf( Stream stream ) {
        if( stream == null )
            return false;
        var result = _inspector.DetermineFileFormat( stream );
        return result is Pdf;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el flujo de datos proporcionado corresponde a un archivo de oficina.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo representa un archivo de oficina, de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un inspector para determinar el formato del archivo a partir del flujo.
    /// Se considera que un archivo es de oficina si su formato es de tipo <c>OfficeOpenXml</c> o <c>CompoundFileBinary</c>.
    /// </remarks>
    /// <seealso cref="OfficeOpenXml"/>
    /// <seealso cref="CompoundFileBinary"/>
    public bool IsOffice( Stream stream ) {
        if( stream == null )
            return false;
        var result = _inspector.DetermineFileFormat( stream );
        return result is OfficeOpenXml or CompoundFileBinary;
    }

    /// <inheritdoc />
    /// <summary>
    /// Determina si el flujo proporcionado corresponde a un archivo de video.
    /// </summary>
    /// <param name="stream">El flujo que se va a evaluar para determinar si es un video.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo es un archivo de video; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un inspector para determinar el formato del archivo basado en el flujo.
    /// Si el flujo es nulo, se considera que no es un video.
    /// </remarks>
    /// <seealso cref="Stream"/>
    public bool IsVideo( Stream stream ) {
        if( stream == null )
            return false;
        var result = _inspector.DetermineFileFormat( stream );
        return result is Isobmff;
    }
}