using Util.Applications.Dtos;

namespace Util.Applications.Trees; 

/// <summary>
/// Interfaz que representa un nodo en una estructura de árbol.
/// </summary>
/// <typeparam name="TNode">El tipo del nodo que implementa la interfaz <see cref="ITreeNode{TNode}"/>.</typeparam>
/// <remarks>
/// Esta interfaz permite la creación de nodos que pueden contener otros nodos, 
/// facilitando la construcción de estructuras de árbol genéricas.
/// </remarks>
public interface ITreeNode<TNode> : ITreeNode where TNode : ITreeNode<TNode> {
    /// <summary>
    /// Obtiene o establece la lista de nodos hijos.
    /// </summary>
    /// <value>
    /// Una lista de objetos <typeparamref name="TNode"/> que representan los nodos hijos.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder y modificar la colección de nodos que son hijos del nodo actual.
    /// </remarks>
    /// <typeparam name="TNode">
    /// El tipo de los nodos hijos.
    /// </typeparam>
    List<TNode> Children { get; set; }
}

/// <summary>
/// Representa un nodo en una estructura de árbol.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IDto"/> y define las propiedades y métodos
/// que deben ser implementados por cualquier nodo en una estructura de árbol.
/// </remarks>
public interface ITreeNode : IDto {
    /// <summary>
    /// Obtiene o establece el identificador del padre.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para relacionar el objeto actual con su entidad padre en una jerarquía.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="string"/> que representa el identificador del padre.
    /// </value>
    string ParentId { get; set; }
    /// <summary>
    /// Obtiene o establece la ruta de un recurso.
    /// </summary>
    /// <value>
    /// La ruta como una cadena de texto.
    /// </value>
    string Path { get; set; }
    /// <summary>
    /// Obtiene o establece el nivel.
    /// </summary>
    /// <remarks>
    /// Este campo es de tipo nullable, lo que significa que puede contener un valor entero o ser nulo.
    /// </remarks>
    /// <value>
    /// Un entero que representa el nivel, o null si no se ha establecido.
    /// </value>
    int? Level { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento está expandido.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que el estado de expansión no está definido.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa el estado de expansión. 
    /// Puede ser <c>true</c> si el elemento está expandido, 
    /// <c>false</c> si está colapsado, o <c>null</c> si el estado no está definido.
    /// </value>
    bool? Expanded { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si un elemento está marcado.
    /// </summary>
    /// <remarks>
    /// Este propiedad puede contener un valor nulo, lo que indica que el estado del elemento es indeterminado.
    /// </remarks>
    /// <value>
    /// Un valor booleano que puede ser verdadero, falso o nulo.
    /// </value>
    bool? Checked { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador de orden.
    /// </summary>
    /// <remarks>
    /// Este identificador puede ser nulo, lo que indica que no se ha asignado un valor.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el identificador de orden, o <c>null</c> si no se ha asignado.
    /// </value>
    int? SortId { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento es una hoja.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que significa que no se ha definido si el elemento es una hoja o no.
    /// </remarks>
    /// <value>
    /// Un valor booleano que representa si el elemento es una hoja (true), no es una hoja (false) o es nulo (null).
    /// </value>
    bool? Leaf { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el elemento debe estar oculto.
    /// </summary>
    /// <remarks>
    /// Este valor es nullable, lo que significa que puede ser verdadero, falso o nulo.
    /// Un valor nulo puede indicar que la visibilidad no está definida.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el elemento debe estar oculto; 
    /// <c>false</c> si el elemento debe ser visible; 
    /// <c>null</c> si la visibilidad no está definida.
    /// </value>
    bool? Hide { get; set; }
}