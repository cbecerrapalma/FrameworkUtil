namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas para crear elementos HTML <u>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y proporciona funcionalidades específicas 
/// para construir etiquetas subrayadas en HTML.
/// </remarks>
public class UBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase hereda de la clase base y establece el nombre del elemento como "u".
    /// </remarks>
    public UBuilder() : base( "u" ) {
    }
}