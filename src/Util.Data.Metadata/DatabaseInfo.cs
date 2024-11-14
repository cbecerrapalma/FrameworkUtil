namespace Util.Data.Metadata; 

/// <summary>
/// Representa la información de conexión a una base de datos.
/// </summary>
public class DatabaseInfo {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DatabaseInfo"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de tablas al instanciar un objeto de la clase <see cref="DatabaseInfo"/>.
    /// </remarks>
    public DatabaseInfo() {
        Tables = new List<TableInfo>();
    }

    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de caracteres.
    /// </value>
    public string Name { get; set; }
    /// <summary>
    /// Obtiene la lista de información de las tablas.
    /// </summary>
    /// <value>
    /// Una lista de objetos <see cref="TableInfo"/> que representan la información de las tablas.
    /// </value>
    public List<TableInfo> Tables { get; }
}