namespace Util.FileStorage; 

/// <summary>
/// Interfaz que define un filtro para nombres de archivos.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para implementar filtros que determinan si un nombre de archivo cumple con ciertos criterios.
/// </remarks>
public interface IFileNameFilter : ISingletonDependency {
    /// <summary>
    /// Filtra el contenido de un archivo especificado por su nombre.
    /// </summary>
    /// <param name="fileName">El nombre del archivo que se va a filtrar.</param>
    /// <returns>Una cadena que representa el contenido filtrado del archivo.</returns>
    /// <remarks>
    /// Este método lee el archivo indicado y aplica un conjunto de reglas de filtrado
    /// para devolver solo la información relevante. Asegúrese de que el archivo
    /// exista y sea accesible antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="System.IO.File"/>
    string Filter( string fileName );
}