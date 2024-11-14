namespace Util.Ui.Razor; 

/// <summary>
/// Clase que implementa la convención de modelo de ruta para páginas.
/// </summary>
/// <remarks>
/// Esta clase permite personalizar la forma en que se generan las rutas para las páginas en una aplicación ASP.NET.
/// </remarks>
public class PageRouteConvention : IPageRouteModelConvention {
    /// <summary>
    /// Aplica un prefijo a las plantillas de ruta de los selectores en el modelo de ruta de la página.
    /// </summary>
    /// <param name="model">El modelo de ruta de la página que contiene los selectores a modificar.</param>
    /// <remarks>
    /// Este método itera sobre cada selector en el modelo de ruta, verifica si la plantilla de ruta es válida 
    /// y no es igual a "Error". Si la plantilla es válida, se le añade el prefijo "view/".
    /// </remarks>
    public void Apply( PageRouteModel model ) {
        foreach( var selector in model.Selectors.ToList() ) {
            var template = selector.AttributeRouteModel?.Template;
            if( string.IsNullOrWhiteSpace( template ) )
                continue;
            if( template == "Error" )
                continue;
            selector.AttributeRouteModel.Template = $"view/{template}";
        }
    }
}