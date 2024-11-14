namespace Util.Data.Sql.Builders.Params; 

/// <summary>
/// Representa un parámetro SQL que se puede utilizar en consultas a bases de datos.
/// </summary>
public class SqlParam {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlParam"/>.
    /// </summary>
    /// <param name="name">El nombre del parámetro SQL.</param>
    /// <param name="value">El valor del parámetro SQL.</param>
    /// <param name="dbType">El tipo de datos del parámetro SQL. Si no se especifica, se utilizará el tipo por defecto.</param>
    /// <param name="direction">La dirección del parámetro (entrada, salida, etc.). Si no se especifica, se utilizará la dirección por defecto.</param>
    /// <param name="size">El tamaño del parámetro. Se utiliza principalmente para tipos de datos como cadenas.</param>
    /// <param name="precision">La precisión del parámetro, aplicable a tipos numéricos.</param>
    /// <param name="scale">La escala del parámetro, aplicable a tipos numéricos.</param>
    /// <remarks>
    /// Esta clase se utiliza para representar un parámetro que se puede pasar a una consulta SQL.
    /// </remarks>
    public SqlParam( string name, object value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null ) {
        Name = name;
        Value = value;
        Direction = direction;
        DbType = dbType;
        Size = size;
        Precision = precision;
        Scale = scale;
    }

    /// <summary>
    /// Obtiene el nombre asociado a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el valor del nombre
    /// que ha sido establecido en el constructor o mediante otro método.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el nombre.
    /// </value>
    public string Name { get; }
    /// <summary>
    /// Obtiene el valor asociado a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un objeto que representa el valor.
    /// </remarks>
    /// <returns>
    /// Un objeto que contiene el valor asociado.
    /// </returns>
    public object Value { get; }
    /// <summary>
    /// Obtiene la dirección del parámetro.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser nula, lo que indica que no se ha establecido una dirección.
    /// </remarks>
    /// <returns>
    /// Un valor de <see cref="ParameterDirection"/> que representa la dirección del parámetro,
    /// o <c>null</c> si no se ha establecido.
    /// </returns>
    public ParameterDirection? Direction { get; }
    /// <summary>
    /// Obtiene el tipo de base de datos asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve un valor de tipo <see cref="DbType"/> que representa el tipo de datos
    /// utilizado en la base de datos. Si no hay un tipo de base de datos asociado, se devolverá null.
    /// </remarks>
    /// <value>
    /// Un valor de <see cref="DbType"/> o null si no se ha establecido un tipo de base de datos.
    /// </value>
    public DbType? DbType { get; }
    /// <summary>
    /// Obtiene el tamaño.
    /// </summary>
    /// <remarks>
    /// Esta propiedad devuelve un valor entero que representa el tamaño. 
    /// Si no se ha establecido un tamaño, el valor será nulo.
    /// </remarks>
    /// <returns>
    /// Un valor entero que representa el tamaño, o <c>null</c> si no se ha establecido.
    /// </returns>
    public int? Size { get; }
    /// <summary>
    /// Obtiene la precisión del valor, que es un byte nullable.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo, lo que indica que no se ha definido una precisión.
    /// </remarks>
    /// <returns>
    /// Un valor de tipo <see cref="byte?"/> que representa la precisión.
    /// </returns>
    public byte? Precision { get; }
    /// <summary>
    /// Obtiene el valor de escala.
    /// </summary>
    /// <remarks>
    /// Este valor puede ser nulo. Si es nulo, indica que no se ha definido una escala.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="byte"/> que representa la escala, o <c>null</c> si no se ha definido.
    /// </value>
    public byte? Scale { get; }
}