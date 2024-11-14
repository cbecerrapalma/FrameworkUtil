namespace Util.Data.Metadata; 

/// <summary>
/// Define un contrato para los servicios de metadatos.
/// </summary>
/// <remarks>
/// Esta interfaz proporciona métodos para gestionar y acceder a metadatos 
/// en diferentes contextos. Las implementaciones de esta interfaz deben 
/// proporcionar la lógica específica para manejar los metadatos.
/// </remarks>
public interface IMetadataService {
    /// <summary>
    /// Obtiene información sobre la base de datos de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, que contiene un objeto <see cref="DatabaseInfo"/> 
    /// con la información de la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método permite recuperar detalles sobre la base de datos, como su nombre, versión, 
    /// y otros metadatos relevantes. Es recomendable manejar excepciones que puedan surgir 
    /// durante la operación, especialmente en entornos de producción.
    /// </remarks>
    Task<DatabaseInfo> GetDatabaseInfoAsync();
}