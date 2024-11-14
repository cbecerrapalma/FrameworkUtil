using Util.Applications.Trees;

namespace Util.Applications; 

/// <summary>
/// Contiene métodos de extensión para la clase <see cref="TreeNode"/>.
/// </summary>
public static class TreeNodeExtensions {
    /// <summary>
    /// Obtiene una lista de identificadores de los nodos padres a partir de la ruta del nodo actual.
    /// </summary>
    /// <param name="node">El nodo del cual se extraerán los identificadores de los nodos padres.</param>
    /// <param name="excludeSelf">Indica si se debe excluir el identificador del nodo actual de la lista resultante. Por defecto es <c>true</c>.</param>
    /// <returns>
    /// Una lista de cadenas que contiene los identificadores de los nodos padres. 
    /// Si el nodo es nulo o su ruta está vacía, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// La ruta del nodo se espera que sea una cadena de identificadores separados por comas. 
    /// Se eliminarán los identificadores vacíos y se aplicará un filtro para excluir el identificador del nodo actual si se especifica.
    /// </remarks>
    public static List<string> GetParentIdsFromPath( this ITreeNode node, bool excludeSelf = true ) {
        if ( node == null || node.Path.IsEmpty() )
            return new List<string>();
        var result = node.Path.Split( ',' ).Where( id => !string.IsNullOrWhiteSpace( id ) && id != "," ).ToList();
        if ( excludeSelf )
            result = result.Where( id => id.SafeString().ToLower() != node.Id.SafeString().ToLower() ).ToList();
        return result;
    }

    /// <summary>
    /// Obtiene una lista de identificadores de padres que faltan en una colección de nodos.
    /// </summary>
    /// <typeparam name="TNode">El tipo de nodo que implementa la interfaz <see cref="ITreeNode"/>.</typeparam>
    /// <param name="nodes">Una colección de nodos de tipo <typeparamref name="TNode"/>.</param>
    /// <returns>Una lista de cadenas que representan los identificadores de padres que faltan.</returns>
    /// <remarks>
    /// Este método verifica cada nodo en la colección proporcionada y extrae los identificadores de padres
    /// desde la ruta de cada nodo. Luego, compara estos identificadores con los identificadores únicos de
    /// los nodos en la colección y devuelve aquellos que no están presentes.
    /// </remarks>
    /// <seealso cref="ITreeNode"/>
    public static List<string> GetMissingParentIds<TNode>( this IEnumerable<TNode> nodes ) where TNode : class, ITreeNode {
        var result = new List<string>();
        if ( nodes == null )
            return result;
        var list = nodes.ToList();
        list.ForEach( entity => {
            if ( entity == null )
                return;
            result.AddRange( entity.GetParentIdsFromPath().Select( t => t.SafeString() ) );
        } );
        var ids = list.DistinctBy( t => t.Id ).Select( t => t?.Id.SafeString() );
        return result.Except( ids ).ToList();
    }
}