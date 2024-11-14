using Util.Data.Queries;

namespace Util.Data.Trees; 

/// <summary>
/// Interfaz que representa los parámetros de consulta para un árbol.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IPage"/> y se utiliza para definir los parámetros
/// específicos necesarios para realizar consultas sobre estructuras de árbol.
/// </remarks>
public interface ITreeQueryParameter : IPage {
    /// <summary>
    /// Obtiene o establece el identificador del elemento padre.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser utilizado para establecer relaciones jerárquicas entre elementos.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el identificador del elemento padre.
    /// </value>
    string ParentId { get; set; }
    /// <summary>
    /// Obtiene o establece el nivel.
    /// </summary>
    /// <remarks>
    /// Este propiedad puede contener un valor entero o ser nula.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el nivel, o null si no se ha establecido.
    /// </value>
    int? Level { get; set; }
    /// <summary>
    /// Obtiene o establece la ruta del archivo o directorio.
    /// </summary>
    /// <value>
    /// La ruta como una cadena de texto.
    /// </value>
    string Path { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que el estado de habilitación no está definido.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de habilitación. 
    /// Puede ser <c>true</c> si está habilitado, <c>false</c> si está deshabilitado, 
    /// o <c>null</c> si el estado no está definido.
    /// </value>
    bool? Enabled { get; set; }
}