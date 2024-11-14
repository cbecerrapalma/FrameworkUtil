namespace Util.Applications.Trees; 

/// <summary>
/// Clase base abstracta que representa el resultado de un árbol, 
/// proporcionando una estructura para manejar la conversión de nodos 
/// de origen a nodos de destino y el resultado asociado.
/// </summary>
/// <typeparam name="TSourceNode">El tipo de nodo de origen.</typeparam>
/// <typeparam name="TDestinationNode">El tipo de nodo de destino.</typeparam>
/// <typeparam name="TResult">El tipo de resultado que se generará.</typeparam>
/// <remarks>
/// Esta clase se utiliza como base para implementar resultados específicos 
/// de árboles que requieren la conversión y manipulación de nodos 
/// en una estructura jerárquica.
/// </remarks>
public abstract class TreeResultBase<TSourceNode, TDestinationNode, TResult> : TreeTableResultBase<TSourceNode, TDestinationNode, TResult>
    where TSourceNode : ITreeNode<TSourceNode>
    where TDestinationNode : class
    where TResult : class {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeResultBase"/>.
    /// </summary>
    /// <param name="data">Una colección de nodos de origen que se utilizarán para construir el árbol.</param>
    /// <param name="async">Indica si la operación debe realizarse de manera asíncrona. El valor predeterminado es <c>false</c>.</param>
    /// <param name="allExpand">Indica si todos los nodos deben estar expandidos por defecto. El valor predeterminado es <c>false</c>.</param>
    protected TreeResultBase(IEnumerable<TSourceNode> data, bool async = false, bool allExpand = false) : base(data, async, allExpand) { }

    /// <summary>
    /// Agrega un nodo a la estructura de destino, inicializando el nodo raíz y sus hijos.
    /// </summary>
    /// <param name="root">El nodo fuente que se va a agregar. No puede ser nulo.</param>
    /// <remarks>
    /// Este método inicializa el nodo raíz utilizando <see cref="InitNode"/> y luego agrega sus hijos
    /// mediante <see cref="AddChildren"/>. Finalmente, convierte el nodo fuente en un nodo de destino
    /// y lo agrega a la estructura de destino utilizando <see cref="AddDestinationNode"/>.
    /// </remarks>
    protected override void AddNode(TSourceNode root) {
        if (root == null)
            return;
        InitNode(root);
        AddChildren(root);
        var destinationNode = ToDestinationNode(root);
        AddDestinationNode(destinationNode);
    }

    /// <summary>
    /// Agrega los nodos hijos a un nodo fuente dado.
    /// </summary>
    /// <param name="node">El nodo fuente al que se le agregarán los nodos hijos.</param>
    /// <remarks>
    /// Este método es recursivo y se asegura de que todos los nodos hijos se inicialicen
    /// antes de ser asignados al nodo fuente. Si el nodo fuente es nulo, el método no realiza ninguna acción.
    /// </remarks>
    /// <typeparam name="TSourceNode">El tipo de nodo fuente que se está procesando.</typeparam>
    /// <seealso cref="GetChildren(TSourceNode)"/>
    /// <seealso cref="InitNode(TSourceNode)"/>
    protected virtual void AddChildren( TSourceNode node ) {
        if ( node == null )
            return;
        var children = GetChildren( node );
        children.ForEach( InitNode );
        node.Children = children;
        foreach ( var child in node.Children )
            AddChildren( child );
    }
}