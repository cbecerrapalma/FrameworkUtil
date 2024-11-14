namespace Util.Ui.Builders; 

/// <summary>
/// Representa un generador de código que hereda de la clase <see cref="TagBuilder"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para construir y manipular estructuras de código de manera programática.
/// </remarks>
public class CodeBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CodeBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir código de manera estructurada. 
    /// Hereda de la clase base y establece un identificador específico.
    /// </remarks>
    public CodeBuilder() : base( "code" ) {
    }
}