namespace Util.FileStorage; 

/// <summary>
/// Interfaz que define un procesador para nombres de cubos.
/// </summary>
public interface IBucketNameProcessor {
    /// <summary>
    /// Procesa el nombre de un bucket y devuelve un objeto de tipo <see cref="ProcessedName"/>.
    /// </summary>
    /// <param name="bucketName">El nombre del bucket que se va a procesar.</param>
    /// <returns>Un objeto <see cref="ProcessedName"/> que contiene la información procesada del bucket.</returns>
    /// <remarks>
    /// Este método toma un nombre de bucket como entrada, realiza las operaciones necesarias para procesarlo
    /// y devuelve un objeto que representa el resultado del procesamiento.
    /// </remarks>
    /// <seealso cref="ProcessedName"/>
    ProcessedName Process( string bucketName );
}