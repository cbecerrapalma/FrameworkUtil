using Util.Data.Queries;

namespace Util.Data; 

/// <summary>
/// Representa una lista paginada de elementos de tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo de los elementos en la lista.</typeparam>
public class PageList<T> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor llama al constructor sobrecargado que acepta un parámetro entero,
    /// inicializando la lista de páginas con un valor predeterminado de 0.
    /// </remarks>
    public PageList() : this( 0 ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList{T}"/>.
    /// </summary>
    /// <param name="data">Una colección opcional de elementos de tipo <typeparamref name="T"/> que se utilizarán para inicializar la lista de páginas.</param>
    public PageList( IEnumerable<T> data = null )
        : this( 0, data ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList{T}"/>.
    /// </summary>
    /// <param name="totalCount">El número total de elementos disponibles.</param>
    /// <param name="data">Una colección opcional de elementos a incluir en la página actual.</param>
    /// <remarks>
    /// Este constructor establece el número de la página actual en 1 y el tamaño de la página en 20.
    /// </remarks>
    public PageList( int totalCount, IEnumerable<T> data = null )
        : this( 1, 20, totalCount, data ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList{T}"/>.
    /// </summary>
    /// <param name="page">El número de la página actual.</param>
    /// <param name="pageSize">El tamaño de la página, es decir, la cantidad de elementos por página.</param>
    /// <param name="totalCount">El número total de elementos disponibles.</param>
    /// <param name="data">Una colección opcional de elementos de tipo <typeparamref name="T"/> que se incluirán en la página.</param>
    public PageList( int page, int pageSize, int totalCount, IEnumerable<T> data = null )
        : this( page, pageSize, totalCount, "", data ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList{T}"/>.
    /// </summary>
    /// <param name="page">El número de la página actual.</param>
    /// <param name="pageSize">El número de elementos por página.</param>
    /// <param name="totalCount">El número total de elementos disponibles.</param>
    /// <param name="order">El criterio de ordenación de los elementos.</param>
    /// <param name="data">Una colección opcional de elementos a incluir en la lista de páginas.</param>
    /// <remarks>
    /// Si <paramref name="data"/> es nulo, se inicializa con una lista vacía.
    /// </remarks>
    public PageList( int page, int pageSize, int totalCount, string order, IEnumerable<T> data = null ) {
        Data = data?.ToList() ?? new List<T>();
        var pager = new Pager( page, pageSize, totalCount );
        Total = pager.Total;
        PageCount = pager.GetPageCount();
        Page = pager.Page;
        PageSize = pager.PageSize;
        Order = order;
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PageList{T}"/>.
    /// </summary>
    /// <param name="pager">Una instancia de <see cref="IPage"/> que contiene información sobre la página actual, el tamaño de la página, el total de elementos y el orden.</param>
    /// <param name="data">Una colección opcional de datos de tipo <typeparamref name="T"/> que se mostrarán en la página.</param>
    public PageList( IPage pager, IEnumerable<T> data = null )
        : this( pager.Page, pager.PageSize, pager.Total, pager.Order, data ) {
    }

    /// <summary>
    /// Obtiene o establece el número de página.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para indicar la página actual en un contexto de paginación.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de la página actual.
    /// </value>
    public int Page { get; set; }

    /// <summary>
    /// Obtiene o establece el tamaño de la página.
    /// </summary>
    /// <remarks>
    /// Este propiedad se utiliza para definir cuántos elementos se mostrarán en cada página 
    /// en una interfaz de paginación. Un valor mayor resultará en menos páginas, 
    /// mientras que un valor menor resultará en más páginas.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de elementos por página.
    /// </value>
    public int PageSize { get; set; }

    /// <summary>
    /// Obtiene o establece el total.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa el valor total que puede ser utilizado para cálculos o visualización.
    /// </remarks>
    /// <value>
    /// Un entero que indica el total.
    /// </value>
    public int Total { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de páginas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa la cantidad total de páginas disponibles en un documento o libro.
    /// </remarks>
    /// <value>
    /// Un entero que indica el número total de páginas.
    /// </value>
    public int PageCount { get; set; }

    /// <summary>
    /// Obtiene o establece el pedido asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa el identificador o la descripción del pedido.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que contiene la información del pedido.
    /// </value>
    public string Order { get; set; }

    /// <summary>
    /// Representa una colección de datos genéricos.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <remarks>
    /// Esta propiedad se inicializa una sola vez y no puede ser modificada después de su creación.
    /// </remarks>
    public List<T> Data { get; init; }

    public T this[int index] {
        get => Data[index];
        set => Data[index] = value;
    }

    /// <summary>
    /// Agrega un elemento al conjunto de datos.
    /// </summary>
    /// <param name="item">El elemento que se va a agregar al conjunto de datos.</param>
    public void Add( T item ) {
        Data.Add( item );
    }

    /// <summary>
    /// Agrega una colección de elementos al final de la lista.
    /// </summary>
    /// <param name="collection">La colección de elementos que se agregarán.</param>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <remarks>
    /// Este método utiliza el método <see cref="List{T}.AddRange"/> para agregar todos los elementos
    /// de la colección proporcionada a la lista existente.
    /// </remarks>
    public void AddRange( IEnumerable<T> collection ) {
        Data.AddRange( collection );
    }

    /// <summary>
    /// Limpia todos los elementos de la colección de datos.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección, dejándola vacía.
    /// </remarks>
    public void Clear() {
        Data.Clear();
    }

    /// <summary>
    /// Convierte una colección de elementos del tipo actual a un tipo especificado utilizando un convertidor.
    /// </summary>
    /// <typeparam name="TResult">El tipo al que se desea convertir cada elemento.</typeparam>
    /// <param name="converter">Función que define cómo convertir un elemento del tipo actual a un elemento del tipo <typeparamref name="TResult"/>.</param>
    /// <returns>Una nueva lista de páginas que contiene los elementos convertidos al tipo <typeparamref name="TResult"/>.</returns>
    /// <remarks>
    /// Este método utiliza la función de conversión proporcionada para transformar cada elemento de la colección original.
    /// </remarks>
    /// <seealso cref="Convert{TResult}(Func{T, TResult})"/>
    public PageList<TResult> Convert<TResult>( Func<T, TResult> converter ) {
        return Convert( Data.Select( converter ) );
    }

    /// <summary>
    /// Convierte una colección de datos en una lista de páginas.
    /// </summary>
    /// <typeparam name="TResult">El tipo de los elementos en la colección de datos.</typeparam>
    /// <param name="data">La colección de datos a convertir en una lista de páginas.</param>
    /// <returns>Una instancia de <see cref="PageList{TResult}"/> que contiene los datos paginados.</returns>
    public PageList<TResult> Convert<TResult>( IEnumerable<TResult> data ) {
        return new( Page, PageSize, Total, Order, data );
    }
}