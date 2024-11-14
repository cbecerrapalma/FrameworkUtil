namespace Util.FileStorage.Minio; 

/// <summary>
/// Interfaz que define un proveedor de configuración para Minio.
/// </summary>
/// <remarks>
/// Esta interfaz extiende <see cref="ITransientDependency"/> lo que indica que su implementación
/// debe ser registrada como una dependencia transitoria en el contenedor de inyección de dependencias.
/// </remarks>
public interface IMinioConfigProvider : ITransientDependency {
    /// <summary>
    /// Obtiene la configuración de Minio de forma asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es un objeto <see cref="MinioOptions"/> que contiene la configuración de Minio.
    /// </returns>
    /// <remarks>
    /// Este método permite recuperar la configuración necesaria para interactuar con el servicio de Minio.
    /// Asegúrese de manejar adecuadamente cualquier excepción que pueda surgir durante la operación.
    /// </remarks>
    Task<MinioOptions> GetConfigAsync();
}