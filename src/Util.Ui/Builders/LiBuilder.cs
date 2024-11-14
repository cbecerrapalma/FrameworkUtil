namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor para crear elementos de lista (<li>) en HTML.
/// Hereda de la clase <see cref="TagBuilder"/>.
/// </summary>
/// <remarks>
/// Esta clase permite personalizar y construir elementos de lista con atributos y contenido específicos.
/// </remarks>
public class LiBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LiBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos de lista HTML <li>.
    /// </remarks>
    public LiBuilder() : base( "li" ) {
    }
}