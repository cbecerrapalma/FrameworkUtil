namespace Util.Data.Queries; 

/// <summary>
/// Representa un paginador que implementa la interfaz <see cref="IPage"/>.
/// </summary>
/// <remarks>
/// Esta clase se encarga de manejar la lógica de paginación, permitiendo navegar entre diferentes páginas de datos.
/// </remarks>
public class Pager : IPage {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Pager"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el número de página inicial en 1.
    /// </remarks>
    public Pager()
        : this( 1 ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Pager"/>.
    /// </summary>
    /// <param name="page">El número de la página actual.</param>
    /// <param name="pageSize">El número de elementos por página.</param>
    /// <param name="order">El orden en que se deben mostrar los elementos.</param>
    /// <remarks>
    /// Este constructor llama a otro constructor de la clase <see cref="Pager"/> 
    /// con un valor predeterminado de 0 para el parámetro adicional.
    /// </remarks>
    public Pager( int page, int pageSize, string order )
        : this( page, pageSize, 0, order ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Pager"/>.
    /// </summary>
    /// <param name="page">El número de la página actual.</param>
    /// <param name="pageSize">El número de elementos por página. El valor predeterminado es 20.</param>
    /// <param name="total">El número total de elementos disponibles. El valor predeterminado es 0.</param>
    /// <param name="order">Una cadena que representa el orden en que se deben mostrar los elementos. El valor predeterminado es una cadena vacía.</param>
    public Pager( int page, int pageSize = 20, int total = 0, string order = "" ) {
        Page = page;
        PageSize = pageSize;
        Total = total;
        Order = order;
    }

    private int _pageIndex;
    /// <summary>
    /// Obtiene o establece el índice de la página actual.
    /// </summary>
    /// <remarks>
    /// Si el índice de la página es menor o igual a cero, se establece automáticamente en 1.
    /// </remarks>
    /// <value>
    /// El índice de la página actual.
    /// </value>
    public int Page {
        get {
            if( _pageIndex <= 0 )
                _pageIndex = 1;
            return _pageIndex;
        }
        set => _pageIndex = value;
    }

    /// <summary>
    /// Obtiene o establece el tamaño de la página.
    /// </summary>
    /// <remarks>
    /// Este valor determina cuántos elementos se mostrarán en cada página.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de elementos por página.
    /// </value>
    public int PageSize { get; set; }

    /// <summary>
    /// Obtiene o establece el total.
    /// </summary>
    /// <value>
    /// Un entero que representa el total.
    /// </value>
    public int Total { get; set; }

    /// <summary>
    /// Calcula la cantidad total de páginas necesarias para mostrar un conjunto de datos.
    /// </summary>
    /// <returns>
    /// Devuelve el número total de páginas, que se calcula dividiendo el total de elementos 
    /// por el tamaño de la página. Si el total de elementos es divisible por el tamaño de la 
    /// página, se devuelve el cociente; de lo contrario, se devuelve el cociente más uno.
    /// </returns>
    public int GetPageCount() {
        if ( ( Total % PageSize ) == 0 )
            return Total / PageSize;
        return ( Total / PageSize ) + 1;
    }

    /// <summary>
    /// Calcula el número de elementos que se deben omitir en una paginación.
    /// </summary>
    /// <returns>
    /// El número de elementos a omitir, calculado como el tamaño de la página multiplicado por el número de la página menos uno.
    /// </returns>
    public int GetSkipCount() {
        return PageSize * ( Page - 1 );
    }

    /// <summary>
    /// Obtiene o establece el pedido asociado.
    /// </summary>
    /// <value>
    /// Una cadena que representa el pedido.
    /// </value>
    public string Order { get; set; }

    /// <summary>
    /// Calcula el número de inicio basado en la página actual y el tamaño de página.
    /// </summary>
    /// <returns>
    /// Un entero que representa el número de inicio para la paginación.
    /// </returns>
    public int GetStartNumber() {
        return ( Page - 1 ) * PageSize + 1;
    }
    /// <summary>
    /// Calcula el número final de elementos en una página dada.
    /// </summary>
    /// <returns>
    /// El número final de elementos en la página actual, calculado como 
    /// el producto de la página actual y el tamaño de la página.
    /// </returns>
    public int GetEndNumber() {
        return Page * PageSize;
    }
}