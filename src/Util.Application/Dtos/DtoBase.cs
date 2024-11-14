namespace Util.Applications.Dtos; 

/// <summary>
/// Clase base abstracta para Data Transfer Objects (DTO).
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="RequestBase"/> y 
/// implementa la interfaz <see cref="IDto"/>. 
/// Se utiliza como base para la creación de DTOs específicos 
/// en la aplicación.
/// </remarks>
public abstract class DtoBase : RequestBase, IDto {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena de texto.
    /// </value>
    public string Id { get; set; }
}