using Util.Applications.Dtos;

namespace Util.Applications.Trees; 

/// <summary>
/// Clase base abstracta para representar un árbol de nodos.
/// </summary>
/// <typeparam name="TNode">El tipo de nodo que implementa la interfaz <see cref="ITreeNode{TNode}"/>.</typeparam>
/// <remarks>
/// Esta clase proporciona una estructura base para los árboles, permitiendo la creación de nodos que pueden contener otros nodos.
/// </remarks>
public abstract class TreeDtoBase<TNode> : TreeDtoBase,ITreeNode<TNode> where TNode : ITreeNode<TNode> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeDtoBase"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece la propiedad <see cref="Children"/> como una nueva lista de nodos.
    /// </remarks>
    protected TreeDtoBase() {
        Children = new List<TNode>();
    }

    /// <summary>
    /// Obtiene o establece la lista de nodos hijos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad contiene todos los nodos que son hijos del nodo actual.
    /// </remarks>
    /// <typeparam name="TNode">El tipo de los nodos hijos.</typeparam>
    /// <returns>Una lista de nodos hijos del tipo especificado.</returns>
    public List<TNode> Children { get; set; }
}

/// <summary>
/// Clase base abstracta que representa un nodo de árbol.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="DtoBase"/> y 
/// implementa la interfaz <see cref="ITreeNode"/>. 
/// Se utiliza como base para la creación de estructuras de árbol 
/// en aplicaciones que requieren una jerarquía de datos.
/// </remarks>
public abstract class TreeDtoBase : DtoBase, ITreeNode {
    /// <summary>
    /// Obtiene o establece el identificador del elemento padre.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador del elemento padre.
    /// </value>
    public string ParentId { get; set; }
    /// <summary>
    /// Representa el nombre del padre.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el nombre del padre en un contexto específico.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre del padre.
    /// </value>
    [Display( Name = "util.parentName" )]
    public string ParentName { get; set; }
    /// <summary>
    /// Obtiene o establece la ruta como una cadena de texto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar la ubicación de un archivo o directorio en el sistema de archivos.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la ruta. 
    /// </value>
    public string Path { get; set; }
    /// <summary>
    /// Obtiene o establece el nivel.
    /// </summary>
    /// <remarks>
    /// Este campo puede contener un valor nulo, lo que indica que el nivel no ha sido definido.
    /// </remarks>
    /// <value>
    /// Un entero que representa el nivel, o null si no se ha establecido.
    /// </value>
    public int? Level { get; set; }
    /// <summary>
    /// Representa la propiedad que indica si la funcionalidad está habilitada.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es opcional y puede ser nula. Si no se establece, el valor predeterminado es verdadero.
    /// </remarks>
    /// <value>
    /// Un valor booleano que indica si la funcionalidad está habilitada. Puede ser verdadero, falso o nulo.
    /// </value>
    [Display( Name = "util.enabled" )]
    public bool? Enabled { get; set; } = true;
    /// <summary>
    /// Representa el identificador de orden para un elemento.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo, lo que indica que no se ha asignado un orden específico.
    /// </remarks>
    /// <value>
    /// Un entero que representa el identificador de orden, o null si no se ha asignado.
    /// </value>
    [Display( Name = "util.sortId" )]
    public int? SortId { get; set; }
    /// <summary>
    /// Representa el estado expandido de un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad indica si el elemento está expandido o no.
    /// Puede ser nula, lo que indica que el estado no está definido.
    /// </remarks>
    /// <value>
    /// Un valor booleano que puede ser verdadero, falso o nulo.
    /// </value>
    [Display( Name = "util.expanded" )]
    public bool? Expanded { get; set; }
    /// <summary>
    /// Representa el ícono asociado a un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar el nombre del ícono que se mostrará en la interfaz de usuario.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el nombre del ícono.
    /// </value>
    [Display( Name = "util.icon" )]
    public string Icon { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el checkbox debe estar deshabilitado.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que significa que no se ha especificado un estado.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado del checkbox. 
    /// Puede ser <c>true</c> para deshabilitar el checkbox, <c>false</c> para habilitarlo, 
    /// o <c>null</c> si no se ha definido un estado.
    /// </value>
    public bool? DisableCheckbox { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento es seleccionable.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que la propiedad no está definida.
    /// Si es verdadero, el elemento es seleccionable; si es falso, no lo es.
    /// Por defecto, esta propiedad se inicializa en verdadero.
    /// </remarks>
    /// <value>
    /// Un valor booleano que puede ser verdadero, falso o nulo.
    /// </value>
    public bool? Selectable { get; set; } = true;
    /// <summary>
    /// Obtiene o establece el estado de verificación.
    /// </summary>
    /// <remarks>
    /// Este propiedad puede contener un valor booleano que indica si el elemento está marcado (true),
    /// desmarcado (false) o no definido (null).
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de verificación, o null si no está definido.
    /// </value>
    public bool? Checked { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si un elemento está seleccionado.
    /// </summary>
    /// <remarks>
    /// Este propiedad puede contener un valor booleano que indica el estado de selección,
    /// o puede ser nula si no se ha definido un estado de selección.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de selección, o null si no está definido.
    /// </value>
    public bool? Selected { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el nodo es una hoja.
    /// </summary>
    /// <remarks>
    /// Un nodo se considera una hoja si no tiene nodos secundarios. 
    /// Este valor puede ser nulo, lo que indica que el estado del nodo no está definido.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de la hoja, 
    /// donde <c>true</c> indica que es una hoja, <c>false</c> indica que no lo es, 
    /// y <c>null</c> indica que el estado no está definido.
    /// </value>
    public bool? Leaf { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento debe estar oculto.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que significa que no se ha especificado un estado de visibilidad.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de visibilidad del elemento. 
    /// Puede ser verdadero (true) si el elemento debe estar oculto, 
    /// falso (false) si debe ser visible, o nulo (null) si no se ha definido.
    /// </value>
    public bool? Hide { get; set; }
    /// <summary>
    /// Define un método abstracto que debe ser implementado por las clases derivadas
    /// para obtener un texto específico.
    /// </summary>
    /// <returns>
    /// Una cadena de texto que representa el contenido específico de la implementación.
    /// </returns>
    /// <remarks>
    /// Este método es parte de una clase abstracta y no puede ser utilizado directamente
    /// sin una implementación concreta en una clase derivada.
    /// </remarks>
    public abstract string GetText();
}