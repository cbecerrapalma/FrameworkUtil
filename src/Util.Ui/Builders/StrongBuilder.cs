namespace Util.Ui.Builders; 

/// <summary>
/// Representa un generador de etiquetas HTML para la etiqueta <strong>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y proporciona funcionalidades específicas
/// para construir etiquetas <strong> en HTML.
/// </remarks>
public class StrongBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StrongBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos HTML de tipo "strong", 
    /// que se emplean para resaltar texto de manera fuerte.
    /// </remarks>
    public StrongBuilder() : base( "strong" ) {
    }
}