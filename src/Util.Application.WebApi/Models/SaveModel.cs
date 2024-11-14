namespace Util.Applications.Models;

/// <summary>
/// Representa un modelo para guardar datos.
/// </summary>
public class SaveModel
{
    /// <summary>
    /// Obtiene o establece la lista de creación.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena una cadena que representa una lista de elementos relacionados con la creación.
    /// </remarks>
    /// <value>
    /// Una cadena que contiene la lista de creación.
    /// </value>
    public string CreationList { get; set; }
    /// <summary>
    /// Obtiene o establece la lista de actualizaciones.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar una cadena que representa 
    /// la lista de actualizaciones en el sistema.
    /// </remarks>
    /// <value>
    /// Una cadena que contiene la lista de actualizaciones.
    /// </value>
    public string UpdateList { get; set; }
    /// <summary>
    /// Obtiene o establece la lista de elementos a eliminar.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar una cadena que representa los elementos que se desean eliminar.
    /// </remarks>
    /// <value>
    /// Una cadena que contiene la lista de elementos a eliminar.
    /// </value>
    public string DeleteList { get; set; }
}