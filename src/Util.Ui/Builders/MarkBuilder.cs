namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas para crear elementos <mark> en HTML.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y proporciona funcionalidades específicas
/// para construir etiquetas que resaltan texto en un documento HTML.
/// </remarks>
public class MarkBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MarkBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos de tipo "mark".
    /// </remarks>
    public MarkBuilder() : base( "mark" ) {
    }
}