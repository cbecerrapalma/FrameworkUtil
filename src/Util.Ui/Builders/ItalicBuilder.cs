namespace Util.Ui.Builders; 

/// <summary>
/// Clase que representa un generador de etiquetas de texto en cursiva.
/// Hereda de la clase <see cref="TagBuilder"/>.
/// </summary>
public class ItalicBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ItalicBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos de texto en cursiva en un documento.
    /// </remarks>
    /// <param name="tagName">El nombre de la etiqueta HTML que se utilizará, en este caso "i".</param>
    public ItalicBuilder() : base( "i" ) {
    }
}