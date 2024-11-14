namespace Util.FileStorage.Local;

/// <summary>
/// Proporciona la configuración para el almacenamiento local.
/// </summary>
/// <remarks>
/// Esta interfaz se utiliza para definir los métodos y propiedades 
/// necesarios para acceder a la configuración del almacenamiento local.
/// </remarks>
public interface ILocalStoreConfigProvider : ITransientDependency {
    /// <summary>
    /// Obtiene la configuración de las opciones de almacenamiento local de manera asíncrona.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de retorno contiene un objeto 
    /// <see cref="LocalStoreOptions"/> que representa las opciones de configuración del almacenamiento local.
    /// </returns>
    /// <remarks>
    /// Este método permite recuperar la configuración necesaria para el funcionamiento del 
    /// almacenamiento local, lo que puede incluir parámetros como la ruta de acceso, 
    /// opciones de seguridad, entre otros.
    /// </remarks>
    Task<LocalStoreOptions> GetConfigAsync();
}