namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas para el elemento HTML <del>.
/// </summary>
/// <remarks>
/// La clase <see cref="DelBuilder"/> hereda de <see cref="TagBuilder"/> y proporciona 
/// funcionalidades específicas para construir etiquetas <del> en HTML.
/// </remarks>
public class DelBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DelBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir comandos de tipo "del".
    /// </remarks>
    public DelBuilder() : base( "del" ) {
    }
}