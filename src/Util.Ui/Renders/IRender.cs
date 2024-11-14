namespace Util.Ui.Renders; 

/// <summary>
/// Define un contrato para la representación de contenido HTML.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="IHtmlContent"/> y permite la implementación
/// de diferentes tipos de contenido que pueden ser renderizados como HTML.
/// </remarks>
public interface IRender : IHtmlContent {
    /// <summary>
    /// Clona la instancia actual de <see cref="IHtmlContent"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IHtmlContent"/> que es una copia de la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método permite crear una duplicación del contenido HTML, lo que puede ser útil
    /// cuando se necesita reutilizar el contenido sin modificar el original.
    /// </remarks>
    /// <seealso cref="IHtmlContent"/>
    IHtmlContent Clone();
}