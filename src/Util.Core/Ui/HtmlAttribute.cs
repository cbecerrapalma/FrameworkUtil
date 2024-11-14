namespace Util.Ui; 

/// <summary>
/// Representa un atributo HTML que se puede aplicar a elementos.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class HtmlAttribute : Attribute {
    /// <summary>
    /// Obtiene o establece la ruta como una cadena.
    /// </summary>
    /// <value>
    /// La ruta que se desea establecer o recuperar.
    /// </value>
    public string Path { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar un elemento.
    /// </summary>
    /// <value>
    /// <c>true</c> si el elemento debe ser ignorado; de lo contrario, <c>false</c>.
    /// </value>
    public bool Ignore { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HtmlAttribute"/>.
    /// </summary>
    /// <param name="path">La ruta asociada al atributo HTML. Si no se proporciona, se establece en <c>null</c>.</param>
    public HtmlAttribute( string path = null ) {
        Path = path;
    }
}