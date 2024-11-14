namespace Util.Applications.Trees; 

/// <summary>
/// Representa el resultado de una tabla de árbol que contiene nodos de tipo <typeparamref name="TNode"/>.
/// </summary>
/// <typeparam name="TNode">El tipo de los nodos que implementan la interfaz <see cref="ITreeNode"/>.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="TreeTableResultBase{TNode,TNode,List{TNode}}"/> y proporciona
/// funcionalidades específicas para manejar resultados de tablas de árbol.
/// </remarks>
public class TreeTableResult<TNode> : TreeTableResultBase<TNode,TNode, List<TNode>> where TNode : class,ITreeNode {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeTableResult"/>.
    /// </summary>
    /// <param name="data">Una colección de nodos de tipo <typeparamref name="TNode"/> que se utilizarán para construir el resultado del árbol.</param>
    /// <param name="async">Un valor que indica si la operación debe realizarse de manera asíncrona. El valor predeterminado es <c>false</c>.</param>
    /// <param name="allExpand">Un valor que indica si todos los nodos deben estar expandidos. El valor predeterminado es <c>false</c>.</param>
    /// <remarks>
    /// Esta clase hereda de otra clase que no se especifica en el contexto. Asegúrese de que la clase base maneje correctamente los parámetros proporcionados.
    /// </remarks>
    /// <typeparam name="TNode">El tipo de los nodos que se utilizarán en la colección <paramref name="data"/>.</typeparam>
    /// <seealso cref="TreeTableResult"/>
    public TreeTableResult( IEnumerable<TNode> data, bool async = false, bool allExpand = false ) : base(data,async,allExpand){
    }

    /// <summary>
    /// Convierte una lista de nodos en el resultado final.
    /// </summary>
    /// <param name="nodes">Una lista de nodos de tipo <typeparamref name="TNode"/> que se va a convertir.</param>
    /// <returns>Devuelve la lista de nodos sin modificaciones.</returns>
    /// <typeparam name="TNode">El tipo de los nodos en la lista.</typeparam>
    protected override List<TNode> ToResult(List<TNode> nodes) {
        return nodes;
    }
}