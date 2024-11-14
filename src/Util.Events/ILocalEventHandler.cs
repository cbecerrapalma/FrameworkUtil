namespace Util.Events; 

/// <summary>
/// Define un manejador de eventos local que hereda de la interfaz <see cref="IEventHandler"/>.
/// </summary>
/// <remarks>
/// Esta interfaz puede ser implementada por clases que manejan eventos específicos dentro de un contexto local,
/// permitiendo una mayor modularidad y separación de responsabilidades en la gestión de eventos.
/// </remarks>
public interface ILocalEventHandler : IEventHandler  {
    /// <summary>
    /// Representa el orden de un elemento.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un valor entero que indica el orden.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa el orden del elemento.
    /// </value>
    int Order { get; }
}