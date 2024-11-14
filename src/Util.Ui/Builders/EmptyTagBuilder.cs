namespace Util.Ui.Builders; 

/// <summary>
/// Representa un constructor de etiquetas vacías que hereda de la clase <see cref="TagBuilder"/>.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para crear etiquetas HTML que no contienen contenido, 
/// como etiquetas de imagen o de línea de separación.
/// </remarks>
public class EmptyTagBuilder : TagBuilder {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EmptyTagBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir etiquetas HTML vacías con el nombre de etiqueta "i".
    /// </remarks>
    public EmptyTagBuilder() : base( "i" ) {
    }

    /// <summary>
    /// Escribe la representación en texto del objeto en el flujo de salida especificado.
    /// </summary>
    /// <param name="writer">El flujo de salida donde se escribirá la representación del objeto.</param>
    /// <param name="encoder">El codificador HTML que se utilizará para codificar el contenido antes de escribirlo.</param>
    /// <remarks>
    /// Este método se puede sobrescribir para proporcionar una implementación personalizada de cómo se debe 
    /// escribir el objeto en el flujo de salida. Asegúrese de utilizar el codificador proporcionado para 
    /// evitar problemas de seguridad relacionados con la inyección de HTML.
    /// </remarks>
    public override void WriteTo( TextWriter writer, HtmlEncoder encoder ) {
    }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena vacía.
    /// </returns>
    public override string ToString() {
        return string.Empty;
    }
}