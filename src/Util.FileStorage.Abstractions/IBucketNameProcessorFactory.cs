namespace Util.FileStorage; 

/// <summary>
/// Interfaz para la fábrica de procesadores de nombres de cubos.
/// </summary>
/// <remarks>
/// Esta interfaz define un contrato para la creación de instancias de procesadores de nombres de cubos,
/// permitiendo la inyección de dependencias y la gestión de su ciclo de vida.
/// </remarks>
public interface IBucketNameProcessorFactory : ITransientDependency {
    /// <summary>
    /// Crea un procesador de nombres de cubo basado en la política especificada.
    /// </summary>
    /// <param name="policy">La política que define el comportamiento del procesador.</param>
    /// <returns>Una instancia de <see cref="IBucketNameProcessor"/> que implementa la lógica definida por la política.</returns>
    /// <remarks>
    /// Este método permite la creación de diferentes procesadores de nombres de cubo, 
    /// lo que facilita la personalización del manejo de nombres de cubo en función de las 
    /// políticas definidas por el usuario.
    /// </remarks>
    /// <seealso cref="IBucketNameProcessor"/>
    IBucketNameProcessor CreateProcessor( string policy );
}