namespace Util.Applications.Trees; 

/// <summary>
/// Clase base abstracta que representa el resultado de una tabla de árbol.
/// </summary>
/// <typeparam name="TSourceNode">El tipo de nodo fuente.</typeparam>
/// <typeparam name="TDestinationNode">El tipo de nodo de destino.</typeparam>
/// <typeparam name="TResult">El tipo de resultado que se generará a partir de los nodos.</typeparam>
public abstract class TreeTableResultBase<TSourceNode, TDestinationNode, TResult>
    where TSourceNode : ITreeNode
    where TDestinationNode : class
    where TResult : class {
    private readonly List<TSourceNode> _data;
    private readonly List<TDestinationNode> _result;
    private readonly bool _async;
    private readonly bool _allExpand;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TreeTableResultBase"/>.
    /// </summary>
    /// <param name="data">Una colección de nodos de origen que se utilizarán para construir el resultado.</param>
    /// <param name="async">Indica si la operación debe realizarse de manera asíncrona. El valor predeterminado es <c>false</c>.</param>
    /// <param name="allExpand">Indica si todos los nodos deben estar expandidos. El valor predeterminado es <c>false</c>.</param>
    /// <remarks>
    /// Este constructor verifica que la colección de datos no sea nula y la convierte en una lista.
    /// También inicializa las propiedades internas necesarias para el funcionamiento de la clase.
    /// </remarks>
    protected TreeTableResultBase( IEnumerable<TSourceNode> data, bool async = false, bool allExpand = false ) {
        data.CheckNull( nameof( data ) );
        _data = data.ToList();
        _async = async;
        _allExpand = allExpand;
        _result = new List<TDestinationNode>();
    }

    /// <summary>
    /// Obtiene la lista de nodos de datos.
    /// </summary>
    /// <returns>
    /// Una lista de objetos de tipo <typeparamref name="TSourceNode"/> que representa los nodos de datos.
    /// </returns>
    protected List<TSourceNode> GetData() {
        return _data;
    }

    /// <summary>
    /// Obtiene el resultado procesado a partir de los nodos raíz.
    /// </summary>
    /// <returns>
    /// Un objeto de tipo <typeparamref name="TResult"/> que representa el resultado final.
    /// Si no hay datos disponibles, se devuelve el resultado correspondiente a <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si los datos están disponibles. Si no lo están, 
    /// se devuelve un resultado nulo. En caso contrario, se itera sobre los nodos raíz 
    /// obtenidos y se añaden al resultado antes de devolverlo.
    /// </remarks>
    public TResult GetResult() {
        if ( _data == null )
            return ToResult( null );
        foreach ( var root in GetRootNodes() )
            AddNode( root );
        return ToResult( _result );
    }

    /// <summary>
    /// Obtiene una lista de nodos raíz ordenados por su identificador de orden.
    /// </summary>
    /// <returns>
    /// Una lista de nodos de tipo <typeparamref name="TSourceNode"/> que son considerados raíz,
    /// ordenados según su propiedad <c>SortId</c>.
    /// </returns>
    /// <remarks>
    /// Este método filtra los nodos de la colección interna <c>_data</c> utilizando el método <c>IsRoot</c>
    /// para determinar cuáles son nodos raíz. Luego, ordena los nodos filtrados por su identificador de orden
    /// y devuelve la lista resultante.
    /// </remarks>
    /// <typeparam name="TSourceNode">
    /// El tipo de los nodos que se están manejando.
    /// </typeparam>
    protected virtual List<TSourceNode> GetRootNodes() {
        return _data.Where( IsRoot ).OrderBy( t => t.SortId ).ToList();
    }

    /// <summary>
    /// Determina si el nodo especificado es un nodo raíz.
    /// </summary>
    /// <param name="dto">El nodo fuente que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nodo es un nodo raíz; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Un nodo se considera raíz si no tiene un identificador de padre o si su nivel es el mínimo
    /// entre todos los nodos en la colección de datos.
    /// </remarks>
    protected virtual bool IsRoot(TSourceNode dto) {
        if (_data.Any(t => t.ParentId.IsEmpty()))
            return dto.ParentId.IsEmpty();
        return dto.Level == _data.Min(t => t.Level);
    }

    /// <summary>
    /// Agrega un nodo a la estructura de destino.
    /// </summary>
    /// <param name="node">El nodo de origen que se va a agregar.</param>
    /// <remarks>
    /// Este método inicializa el nodo de origen, lo convierte en un nodo de destino y lo agrega a la estructura de destino.
    /// Luego, recorre todos los nodos hijos del nodo de origen y los agrega recursivamente.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el nodo proporcionado es nulo.</exception>
    /// <seealso cref="InitNode(TSourceNode)"/>
    /// <seealso cref="ToDestinationNode(TSourceNode)"/>
    /// <seealso cref="AddDestinationNode(TDestinationNode)"/>
    /// <seealso cref="GetChildren(TSourceNode)"/>
    protected virtual void AddNode( TSourceNode node ) {
        if ( node == null )
            return;
        InitNode( node );
        var destinationNode = ToDestinationNode( node );
        AddDestinationNode( destinationNode );
        var children = GetChildren( node );
        foreach ( var child in children )
            AddNode( child );
    }

    /// <summary>
    /// Inicializa un nodo de tipo <typeparamref name="TSourceNode"/>.
    /// </summary>
    /// <param name="node">El nodo que se va a inicializar.</param>
    /// <remarks>
    /// Este método llama a otros métodos para inicializar las propiedades del nodo,
    /// como <see cref="InitLeaf(TSourceNode)"/> y <see cref="InitExpanded(TSourceNode)"/>.
    /// </remarks>
    /// <typeparam name="TSourceNode">El tipo del nodo que se está inicializando.</typeparam>
    protected void InitNode(TSourceNode node) 
    { 
        InitLeaf(node); 
        InitExpanded(node); 
    }

    /// <summary>
    /// Inicializa el estado de un nodo como hoja.
    /// </summary>
    /// <param name="node">El nodo fuente que se va a inicializar.</param>
    /// <remarks>
    /// Este método establece la propiedad Leaf del nodo en falso inicialmente.
    /// Si la operación es asíncrona, el método retorna sin realizar más cambios.
    /// Si el nodo es considerado una hoja, se establece la propiedad Leaf en verdadero.
    /// </remarks>
    /// <typeparam name="TSourceNode">El tipo del nodo fuente que se está inicializando.</typeparam>
    protected virtual void InitLeaf(TSourceNode node) {
        node.Leaf = false;
        if (_async)
            return;
        if (IsLeaf(node))
            node.Leaf = true;
    }

    /// <summary>
    /// Determina si un nodo dado es una hoja en la estructura de datos.
    /// </summary>
    /// <param name="node">El nodo que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nodo es una hoja; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Un nodo se considera una hoja si no tiene hijos o si todos sus hijos están ocultos.
    /// </remarks>
    protected virtual bool IsLeaf( TSourceNode node ) {
        if ( _data.All( t => t.ParentId != node.Id ) )
            return true;
        var children = _data.FindAll( t => t.ParentId == node.Id );
        if ( children.All( t => t.Hide == true ) )
            return true;
        return false;
    }

    /// <summary>
    /// Inicializa el estado expandido de un nodo fuente.
    /// </summary>
    /// <param name="node">El nodo fuente que se va a inicializar.</param>
    /// <remarks>
    /// Este método verifica varias condiciones antes de expandir el nodo. 
    /// Si la propiedad <c>_allExpand</c> es falsa, no se realiza ninguna acción. 
    /// Si la propiedad <c>_async</c> es falsa, el nodo se expande directamente. 
    /// Si todos los elementos de datos tienen un nivel igual a 1, el método no realiza ninguna acción. 
    /// Finalmente, si el nodo no es una hoja, se expande el nodo.
    /// </remarks>
    protected virtual void InitExpanded(TSourceNode node) {
        if (_allExpand == false)
            return;
        if (_async == false) {
            node.Expanded = true;
            return;
        }
        if (_data.All(t => t.Level == 1))
            return;
        if (node.Leaf == false)
            node.Expanded = true;
    }

    /// <summary>
    /// Agrega un nodo de destino a la colección de resultados.
    /// </summary>
    /// <param name="node">El nodo de destino que se va a agregar.</param>
    protected void AddDestinationNode(TDestinationNode node) 
    { 
        _result.Add(node); 
    }

    /// <summary>
    /// Obtiene una lista de nodos hijos para un nodo específico.
    /// </summary>
    /// <param name="node">El nodo del cual se desean obtener los nodos hijos.</param>
    /// <returns>Una lista de nodos hijos que tienen como padre el nodo especificado.</returns>
    /// <remarks>
    /// Este método filtra la colección de datos para encontrar todos los nodos que tienen el mismo Id de padre que el nodo proporcionado,
    /// y los ordena según el campo SortId antes de devolver la lista.
    /// </remarks>
    /// <typeparam name="TSourceNode">El tipo de nodo que se está manejando.</typeparam>
    protected List<TSourceNode> GetChildren(TSourceNode node) {
        return _data.Where(t => t.ParentId == node.Id).OrderBy(t => t.SortId).ToList();
    }

    /// <summary>
    /// Convierte un nodo de origen en un nodo de destino.
    /// </summary>
    /// <param name="node">El nodo de origen que se va a convertir.</param>
    /// <returns>El nodo de destino resultante de la conversión.</returns>
    /// <typeparam name="TSourceNode">El tipo del nodo de origen.</typeparam>
    /// <typeparam name="TDestinationNode">El tipo del nodo de destino.</typeparam>
    /// <remarks>
    /// Este método utiliza un mapeo para transformar el nodo de origen en el nodo de destino.
    /// Asegúrese de que el tipo de nodo de origen sea compatible con el tipo de nodo de destino.
    /// </remarks>
    /// <seealso cref="MapTo{TDestinationNode}"/>
    protected virtual TDestinationNode ToDestinationNode(TSourceNode node) {
        return node.MapTo<TDestinationNode>();
    }

    /// <summary>
    /// Convierte una lista de nodos de destino en un resultado de tipo específico.
    /// </summary>
    /// <param name="nodes">Una lista de nodos de destino que se utilizarán para generar el resultado.</param>
    /// <returns>Un resultado de tipo <typeparamref name="TResult"/> que representa la conversión de los nodos.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// La implementación debe definir cómo se realiza la conversión de los nodos a un resultado.
    /// </remarks>
    /// <typeparam name="TResult">El tipo de resultado que se generará a partir de los nodos.</typeparam>
    protected abstract TResult ToResult( List<TDestinationNode> nodes );
}