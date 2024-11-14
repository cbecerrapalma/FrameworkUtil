namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas para el elemento HTML <kbd>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y se utiliza para crear 
/// representaciones de la etiqueta <kbd> en HTML, que se utiliza para 
/// indicar texto que debe ser introducido por el usuario.
/// </remarks>
public class KbdBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="KbdBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos HTML de tipo "kbd", que representan texto que debe ser ingresado por el usuario.
    /// </remarks>
    public KbdBuilder() : base( "kbd" ) {
    }
}