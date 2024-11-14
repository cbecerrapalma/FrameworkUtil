using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;

namespace Util.Ui.Razor.Internal;

/// <summary>
/// Clase que implementa la interfaz <see cref="IPartViewPathFinder"/>.
/// Proporciona métodos para encontrar rutas de vista de partes en una aplicación.
/// </summary>
public class PartViewPathFinder : IPartViewPathFinder {
    private readonly ICompositeViewEngine _viewEngine;
    private readonly RazorOptions _options;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PartViewPathFinder"/>.
    /// </summary>
    /// <param name="viewEngine">El motor de vista que se utilizará para buscar vistas.</param>
    /// <param name="options">Las opciones de configuración para Razor.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="viewEngine"/> es <c>null</c>.</exception>
    public PartViewPathFinder( ICompositeViewEngine viewEngine, IOptions<RazorOptions> options ) {
        _viewEngine = viewEngine ?? throw new ArgumentNullException( nameof( viewEngine ) );
        _options = options.Value;
    }

    /// <inheritdoc />
    /// <summary>
    /// Busca la vista especificada en la ruta dada y el nombre de la vista parcial.
    /// </summary>
    /// <param name="viewPath">La ruta de la vista donde se buscará la vista parcial.</param>
    /// <param name="partViewName">El nombre de la vista parcial que se desea encontrar.</param>
    /// <returns>
    /// La ruta de la vista parcial encontrada, o <c>null</c> si no se encuentra ninguna vista.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si los parámetros proporcionados son válidos. 
    /// Si alguno de ellos está vacío, se devuelve <c>null</c>. Luego, intenta obtener la vista 
    /// utilizando el motor de vistas. Si la vista se encuentra con éxito, se devuelve su ruta. 
    /// Si no, se intenta buscar la vista utilizando otro método de búsqueda.
    /// </remarks>
    /// <seealso cref="GetRouteValue(string)"/>
    /// <seealso cref="CreateViewContext(string)"/>
    /// <seealso cref="GetPartViewName(string)"/>
    public string Find( string viewPath, string partViewName ) {
        if ( viewPath.IsEmpty() )
            return null;
        if ( partViewName.IsEmpty() )
            return null;
        var result = _viewEngine.GetView( GetRouteValue( viewPath ), partViewName, false );
        if ( result.Success )
            return result.View.Path;
        var view = _viewEngine.FindView( CreateViewContext( viewPath ), GetPartViewName( partViewName ), isMainPage: false );
        return view.Success ? view.View.Path : null;
    }

    /// <summary>
    /// Obtiene el valor de la ruta a partir de la ruta de la vista proporcionada.
    /// </summary>
    /// <param name="viewPath">La ruta de la vista desde la cual se extraerá el valor de la ruta.</param>
    /// <returns>
    /// Un string que representa el valor de la ruta, 
    /// que se obtiene al eliminar el directorio raíz de Razor y la extensión ".cshtml".
    /// </returns>
    private string GetRouteValue( string viewPath ) {
        return viewPath.RemoveStart( _options.RazorRootDirectory ).RemoveEnd( ".cshtml" );
    }

    /// <summary>
    /// Crea un contexto de vista con la información proporcionada.
    /// </summary>
    /// <param name="viewPath">La ruta de la vista que se está creando.</param>
    /// <returns>
    /// Un objeto <see cref="ViewContext"/> que contiene la información de la vista
    /// y los datos de enrutamiento necesarios para su ejecución.
    /// </returns>
    private ViewContext CreateViewContext( string viewPath ) {
        return new ViewContext {
            ExecutingFilePath = viewPath,
            RouteData = new RouteData {
                Values = { { "page", GetRouteValue( viewPath ) } }
            },
            ActionDescriptor = new PageActionDescriptor {
                RouteValues = { { "page", GetRouteValue( viewPath ) } }
            }
        };
    }

    /// <summary>
    /// Obtiene el nombre de la vista de parte sin la extensión ".cshtml".
    /// </summary>
    /// <param name="partViewName">El nombre de la vista de parte que se desea procesar.</param>
    /// <returns>El nombre de la vista de parte sin la extensión ".cshtml".</returns>
    private string GetPartViewName( string partViewName ) {
        return partViewName.RemoveEnd( ".cshtml" );
    }
}