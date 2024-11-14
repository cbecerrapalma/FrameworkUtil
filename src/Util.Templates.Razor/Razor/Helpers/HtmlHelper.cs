namespace Util.Templates.Razor.Helpers; 

/// <summary>
/// Proporciona métodos de ayuda para generar contenido HTML.
/// </summary>
public class HtmlHelper {
    private readonly ITemplateEngine _engine;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HtmlHelper"/>.
    /// </summary>
    /// <param name="engine">La instancia de <see cref="ITemplateEngine"/> que se utilizará para procesar plantillas.</param>
    public HtmlHelper( ITemplateEngine engine ) {
        _engine = engine;
    }

    /// <summary>
    /// Renderiza una vista parcial utilizando el nombre de la vista y un modelo proporcionado.
    /// </summary>
    /// <param name="partialViewName">El nombre de la vista parcial que se desea renderizar.</param>
    /// <param name="model">El modelo que se pasará a la vista parcial.</param>
    /// <returns>
    /// Una cadena que representa el resultado de la renderización de la vista parcial.
    /// </returns>
    public string Partial( string partialViewName, object model ) {
        var path = Util.Helpers.Common.GetPhysicalPath( partialViewName );
        return _engine.RenderByPath( path, model );
    }

    /// <summary>
    /// Renderiza una vista parcial de forma asíncrona.
    /// </summary>
    /// <param name="partialViewName">El nombre de la vista parcial que se va a renderizar.</param>
    /// <param name="model">El modelo que se pasará a la vista parcial.</param>
    /// <returns>
    /// Una cadena que representa el resultado de la vista parcial renderizada.
    /// </returns>
    public string PartialAsync( string partialViewName, object model ) {
        return Partial( partialViewName, model );
    }
}