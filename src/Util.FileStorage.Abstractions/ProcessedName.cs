namespace Util.FileStorage; 

/// <summary>
/// Representa un nombre procesado que puede incluir diversas transformaciones o formatos.
/// </summary>
public class ProcessedName {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProcessedName"/>.
    /// </summary>
    /// <param name="name">El nombre que se procesará. No puede estar vacío.</param>
    /// <param name="originalName">El nombre original. Si es <c>null</c>, se asignará el valor de <paramref name="name"/>.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando <paramref name="name"/> está vacío.</exception>
    public ProcessedName( string name, string originalName = null ) {
        if ( name.IsEmpty() )
            throw new ArgumentNullException( name );
        Name = name;
        OriginalName = originalName ?? name;
    }

    /// <summary>
    /// Obtiene el nombre asociado a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el valor del nombre
    /// que se ha establecido en el constructor o mediante otros métodos.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Obtiene el nombre original.
    /// </summary>
    /// <value>
    /// El nombre original como una cadena.
    /// </value>
    public string OriginalName { get; }
}