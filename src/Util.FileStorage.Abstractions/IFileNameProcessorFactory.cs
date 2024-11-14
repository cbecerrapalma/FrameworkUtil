namespace Util.FileStorage; 

/// <summary>
/// Interfaz para la fábrica de procesadores de nombres de archivo.
/// </summary>
/// <remarks>
/// Esta interfaz define un contrato para las fábricas que crean instancias de procesadores de nombres de archivo.
/// </remarks>
public interface IFileNameProcessorFactory : ITransientDependency {
    /// <summary>
    /// Crea un procesador de nombres de archivo basado en la política especificada.
    /// </summary>
    /// <param name="policy">La política que determina el tipo de procesador a crear.</param>
    /// <returns>Una instancia de <see cref="IFileNameProcessor"/> que implementa la política dada.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="policy"/> es null.</exception>
    /// <remarks>
    /// Este método permite la creación dinámica de procesadores de nombres de archivo,
    /// facilitando la extensión y personalización del comportamiento del procesamiento
    /// de nombres de archivo según diferentes políticas.
    /// </remarks>
    IFileNameProcessor CreateProcessor( string policy );
}