namespace Util.Ui.Razor.Internal;

/// <summary>
/// Define un contrato para encontrar rutas de vista de partes.
/// </summary>
public interface IPartViewPathFinder {
    /// <summary>
    /// Busca una vista parcial en la ruta especificada.
    /// </summary>
    /// <param name="viewPath">La ruta donde se debe buscar la vista parcial.</param>
    /// <param name="partViewName">El nombre de la vista parcial que se desea encontrar.</param>
    /// <returns>La ruta completa de la vista parcial si se encuentra; de lo contrario, una cadena vacía.</returns>
    /// <remarks>
    /// Este método realiza una búsqueda en la ruta proporcionada y devuelve la ruta completa 
    /// de la vista parcial si existe. Si no se encuentra, se devuelve una cadena vacía.
    /// </remarks>
    /// <seealso cref="System.String"/>
    string Find( string viewPath, string partViewName );
}