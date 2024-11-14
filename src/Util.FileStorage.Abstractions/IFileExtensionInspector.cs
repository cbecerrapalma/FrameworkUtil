namespace Util.FileStorage; 

/// <summary>
/// Interfaz que define un inspector de extensiones de archivo.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISingletonDependency"/> lo que indica que debe ser implementada como un servicio singleton.
/// </remarks>
public interface IFileExtensionInspector : ISingletonDependency {
    /// <summary>
    /// Obtiene la extensión del archivo a partir de un flujo de datos.
    /// </summary>
    /// <param name="stream">El flujo de datos del cual se extraerá la extensión del archivo.</param>
    /// <returns>La extensión del archivo como una cadena de texto. Si no se puede determinar la extensión, se devuelve una cadena vacía.</returns>
    /// <remarks>
    /// Este método analiza el flujo de datos para intentar determinar la extensión del archivo asociado.
    /// Es importante asegurarse de que el flujo esté posicionado correctamente antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="System.IO.Stream"/>
    string GetExtension( Stream stream );
    /// <summary>
    /// Determina si el flujo proporcionado representa una imagen válida.
    /// </summary>
    /// <param name="stream">El flujo que se va a evaluar para verificar si es una imagen.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo representa una imagen válida; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método intenta leer los datos del flujo y verificar si corresponden a un formato de imagen conocido.
    /// Se recomienda que el flujo esté posicionado al inicio antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="System.IO.Stream"/>
    bool IsImage( Stream stream );
    /// <summary>
    /// Determina si el flujo proporcionado corresponde a un archivo PDF.
    /// </summary>
    /// <param name="stream">El flujo que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo es un archivo PDF; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método examina los primeros bytes del flujo para identificar la cabecera de un archivo PDF,
    /// que comienza con el encabezado "%PDF-". Si el flujo no contiene suficientes datos o no coincide
    /// con el formato esperado, se devolverá <c>false</c>.
    /// </remarks>
    /// <seealso cref="System.IO.Stream"/>
    bool IsPdf( Stream stream );
    /// <summary>
    /// Determina si el flujo de datos proporcionado corresponde a un archivo de Microsoft Office.
    /// </summary>
    /// <param name="stream">El flujo de datos que se va a evaluar.</param>
    /// <returns>Devuelve <c>true</c> si el flujo representa un archivo de Office; de lo contrario, <c>false</c>.</returns>
    /// <remarks>
    /// Este método verifica la cabecera del flujo para identificar si corresponde a uno de los formatos de archivo de Microsoft Office,
    /// como Word, Excel o PowerPoint. Es importante asegurarse de que el flujo esté en una posición válida antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="System.IO.Stream"/>
    bool IsOffice( Stream stream );
    /// <summary>
    /// Determina si el flujo proporcionado representa un video.
    /// </summary>
    /// <param name="stream">El flujo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el flujo es un video; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método examina el contenido del flujo para identificar si corresponde a un formato de video conocido.
    /// Se pueden implementar diferentes técnicas de detección, como la verificación de encabezados o la lectura de metadatos.
    /// </remarks>
    /// <seealso cref="Stream"/>
    bool IsVideo( Stream stream );
}