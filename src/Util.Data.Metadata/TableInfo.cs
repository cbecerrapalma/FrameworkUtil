namespace Util.Data.Metadata; 

/// <summary>
/// Representa la información de una tabla en una base de datos.
/// </summary>
public class TableInfo {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TableInfo"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de columnas que se puede utilizar para almacenar información sobre las columnas de una tabla.
    /// </remarks>
    public TableInfo() {
        Columns = new List<ColumnInfo>();
    }

    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Obtiene o establece el esquema asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir el esquema que se utilizará en la operación actual.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el esquema.
    /// </value>
    public string Schema { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de caracteres.
    /// </value>
    public string Name { get; set; }
    /// <summary>
    /// Obtiene o establece el comentario asociado.
    /// </summary>
    /// <value>
    /// Una cadena que representa el comentario.
    /// </value>
    public string Comment { get; set; }
    /// <summary>
    /// Obtiene la lista de información de columnas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso a una colección de objetos <see cref="ColumnInfo"/> 
    /// que representan la información de las columnas en un conjunto de datos.
    /// </remarks>
    /// <returns>
    /// Una lista de <see cref="ColumnInfo"/> que contiene la información de las columnas.
    /// </returns>
    public List<ColumnInfo> Columns { get; }
}