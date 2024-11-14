namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas para elementos HTML de tipo <c>&lt;ul&gt;</c>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y proporciona funcionalidades específicas
/// para crear listas desordenadas en HTML.
/// </remarks>
public class UlBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UlBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir un elemento HTML de lista desordenada (<ul>).
    /// </remarks>
    public UlBuilder() : base( "ul" ) {
    }
}