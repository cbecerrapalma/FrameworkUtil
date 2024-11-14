namespace Util.Applications.Trees;

/// <summary>
/// Enumera los modos de carga disponibles.
/// </summary>
/// <remarks>
/// Este enumerador se utiliza para especificar el modo en que se debe realizar una operación de carga.
/// </remarks>
public enum LoadMode
{
    /// <summary>
    /// Carga sincrónica, carga de todos los nodos de una vez.
    /// </summary>
    Sync = 0,
    /// <summary>
    /// Carga asíncrona, carga el nodo raíz por primera vez, al hacer clic solo se cargan los nodos directamente inferiores.
    /// </summary>
    Async = 1,
    /// <summary>
    /// El nodo raíz se carga de manera asíncrona, al cargar por primera vez el nodo raíz, se hace clic para cargar todos los nodos secundarios.
    /// </summary>
    RootAsync = 2
}