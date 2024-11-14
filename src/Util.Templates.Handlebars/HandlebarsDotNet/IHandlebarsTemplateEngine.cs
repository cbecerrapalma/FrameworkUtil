namespace Util.Templates.HandlebarsDotNet;

/// <summary>
/// Define un motor de plantillas que utiliza Handlebars para procesar plantillas.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ITemplateEngine"/> y proporciona métodos específicos 
/// para la manipulación y renderización de plantillas Handlebars.
/// </remarks>
public interface IHandlebarsTemplateEngine : ITemplateEngine {
    /// <summary>
    /// Obtiene un motor de plantillas Handlebars con un codificador de texto especificado.
    /// </summary>
    /// <param name="encoder">El codificador de texto que se utilizará para codificar el contenido de la plantilla.</param>
    /// <returns>Un objeto que implementa <see cref="IHandlebarsTemplateEngine"/> configurado con el codificador proporcionado.</returns>
    /// <remarks>
    /// Este método permite personalizar la forma en que se codifican los textos en las plantillas Handlebars,
    /// lo que puede ser útil para asegurar que el contenido se maneje de forma segura y adecuada.
    /// </remarks>
    /// <seealso cref="ITextEncoder"/>
    /// <seealso cref="IHandlebarsTemplateEngine"/>
    IHandlebarsTemplateEngine Encoder( ITextEncoder encoder );
    /// <summary>
    /// Obtiene una instancia de <see cref="IHandlebarsTemplateEngine"/> que se utiliza para codificar HTML.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IHandlebarsTemplateEngine"/> que proporciona funcionalidades de codificación HTML.
    /// </returns>
    /// <remarks>
    /// Este método es útil para asegurar que las cadenas de texto se codifiquen correctamente
    /// antes de ser insertadas en el HTML, previniendo así posibles vulnerabilidades de inyección de código.
    /// </remarks>
    /// <seealso cref="IHandlebarsTemplateEngine"/>
    IHandlebarsTemplateEngine HtmlEncoder();
}