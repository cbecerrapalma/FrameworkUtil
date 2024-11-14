namespace Util.FileStorage; 

/// <summary>
/// Define un contrato para procesar nombres de archivos.
/// </summary>
public interface IFileNameProcessor {
    /// <summary>
    /// Procesa un archivo y devuelve un objeto <see cref="ProcessedName"/> que representa el resultado del procesamiento.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a procesar.</param>
    /// <returns>Un objeto <see cref="ProcessedName"/> que contiene la información procesada del archivo.</returns>
    /// <remarks>
    /// Este método puede lanzar excepciones si el archivo no se encuentra o si ocurre un error durante el procesamiento.
    /// Asegúrese de manejar adecuadamente las excepciones al llamar a este método.
    /// </remarks>
    /// <seealso cref="ProcessedName"/>
    ProcessedName Process( string fileName );
}