namespace Util.Domain.Trees; 

/// <summary>
/// Interfaz que define un contrato para clases que implementan un método de ordenamiento basado en un identificador.
/// </summary>
public interface ISortId {
    /// <summary>
    /// Obtiene o establece el identificador de orden.
    /// </summary>
    /// <remarks>
    /// Este identificador es de tipo nullable, lo que significa que puede tener un valor entero o ser nulo.
    /// </remarks>
    /// <value>
    /// Un entero que representa el identificador de orden, o null si no se ha establecido.
    /// </value>
    int? SortId { get; set; }
}