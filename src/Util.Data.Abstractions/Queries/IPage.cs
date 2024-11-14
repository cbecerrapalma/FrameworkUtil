namespace Util.Data.Queries; 

/// <summary>
/// Define una interfaz para representar una página en una aplicación.
/// </summary>
public interface IPage {
    /// <summary>
    /// Obtiene o establece el número de página.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar la página actual en un contexto de paginación.
    /// </remarks>
    int Page { get; set; }
    /// <summary>
    /// Obtiene o establece el tamaño de la página.
    /// </summary>
    /// <remarks>
    /// El tamaño de la página se utiliza para definir cuántos elementos se mostrarán en cada página 
    /// de una lista o conjunto de datos. Este valor es crucial para la paginación en interfaces de usuario 
    /// y en la gestión de grandes volúmenes de datos.
    /// </remarks>
    /// <value>
    /// Un entero que representa el número de elementos por página.
    /// </value>
    int PageSize { get; set; }
    /// <summary>
    /// Obtiene o establece el valor total.
    /// </summary>
    /// <value>
    /// Un entero que representa el total.
    /// </value>
    int Total { get; set; }
    /// <summary>
    /// Obtiene el número total de páginas.
    /// </summary>
    /// <returns>Un entero que representa la cantidad total de páginas.</returns>
    /// <remarks>
    /// Este método es útil para determinar cuántas páginas se deben mostrar en una interfaz de usuario
    /// o para realizar cálculos relacionados con la paginación de datos.
    /// </remarks>
    int GetPageCount();
    /// <summary>
    /// Obtiene el número de elementos que se deben omitir.
    /// </summary>
    /// <returns>
    /// Un entero que representa la cantidad de elementos a omitir.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para implementar paginación o para saltar un número específico de elementos en una colección.
    /// </remarks>
    int GetSkipCount();
    /// <summary>
    /// Representa el pedido asociado a un objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad almacena la información del pedido en forma de cadena.
    /// </remarks>
    /// <value>
    /// Una cadena que representa el pedido.
    /// </value>
    string Order { get; set; }
    /// <summary>
    /// Obtiene el número de inicio.
    /// </summary>
    /// <returns>
    /// Un número entero que representa el número de inicio.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para inicializar procesos que requieren un número de inicio específico.
    /// </remarks>
    int GetStartNumber();
    /// <summary>
    /// Obtiene el número final.
    /// </summary>
    /// <returns>
    /// Un entero que representa el número final.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para recuperar el número final que puede ser utilizado en cálculos posteriores.
    /// </remarks>
    int GetEndNumber();
}