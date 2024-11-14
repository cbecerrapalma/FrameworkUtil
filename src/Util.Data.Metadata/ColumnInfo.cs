namespace Util.Data.Metadata; 

/// <summary>
/// Representa la información de una columna en una estructura de datos.
/// </summary>
public class ColumnInfo {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena de caracteres.
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
    /// Obtiene o establece el comentario asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el comentario.
    /// </value>
    public string Comment { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si la propiedad es una clave primaria.
    /// </summary>
    /// <value>
    /// <c>true</c> si la propiedad es una clave primaria; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsPrimaryKey { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el campo es de auto-incremento.
    /// </summary>
    /// <value>
    /// <c>true</c> si el campo es de auto-incremento; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsAutoIncrement { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el objeto puede ser nulo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es útil para determinar si se permite que el valor asociado sea nulo.
    /// </remarks>
    /// <value>
    /// Un valor booleano que puede ser verdadero, falso o nulo.
    /// </value>
    public bool? IsNullable { get; set; }
    /// <summary>
    /// Obtiene o establece el tipo de dato.
    /// </summary>
    /// <value>
    /// Una cadena que representa el tipo de dato.
    /// </value>
    public string DataType { get; set; }
    /// <summary>
    /// Obtiene o establece la longitud.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa la longitud de un objeto o entidad, 
    /// permitiendo su acceso y modificación.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="long"/> que indica la longitud.
    /// </value>
    public long Length { get; set; }
    /// <summary>
    /// Obtiene o establece la precisión de un valor.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo, lo que indica que la precisión no está definida.
    /// </remarks>
    /// <value>
    /// Un valor entero que representa la precisión, o null si no se ha definido.
    /// </value>
    public int? Precision { get; set; }
    /// <summary>
    /// Obtiene o establece el valor de la escala.
    /// </summary>
    /// <remarks>
    /// Este campo puede ser nulo. Si no se establece un valor, se considerará que la escala no está definida.
    /// </remarks>
    /// <value>
    /// Un entero que representa la escala, o <c>null</c> si no se ha definido.
    /// </value>
    public int? Scale { get; set; }
}