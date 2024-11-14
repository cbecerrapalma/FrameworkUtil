using Util.Data.Queries;

namespace Util.Data.Trees; 

/// <summary>
/// Representa un parámetro de consulta específico para estructuras de árbol.
/// Hereda de <see cref="QueryParameter"/> y implementa <see cref="ITreeQueryParameter"/>.
/// </summary>
public class TreeQueryParameter : QueryParameter, ITreeQueryParameter {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeQueryParameter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el valor predeterminado de la propiedad <see cref="Order"/> a "SortId".
    /// </remarks>
    public TreeQueryParameter() {
        Order = "SortId";
    }

    /// <summary>
    /// Obtiene o establece el identificador del elemento padre.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para relacionar el objeto actual con su elemento padre en una jerarquía.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador del elemento padre.
    /// </value>
    public string ParentId { get; set; }

    /// <summary>
    /// Obtiene o establece el nivel.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede contener un valor entero o ser nula. 
    /// Se utiliza para representar el nivel de un objeto en un contexto determinado.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el nivel, o null si no se ha establecido.
    /// </value>
    public int? Level { get; set; }

    /// <summary>
    /// Obtiene o establece la ruta como una cadena.
    /// </summary>
    /// <value>
    /// La ruta del tipo <see cref="string"/> que representa la ubicación deseada.
    /// </value>
    public string Path { get; set; }

    /// <summary>
    /// Representa si una funcionalidad está habilitada o no.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser nula, lo que indica que el estado de habilitación no está definido.
    /// </remarks>
    /// <value>
    /// Un valor booleano que indica si la funcionalidad está habilitada. 
    /// Puede ser verdadero (true), falso (false) o nulo (null).
    /// </value>
    [Display( Name = "util.enabled" )]
    public bool? Enabled { get; set; }
}