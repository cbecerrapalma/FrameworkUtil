namespace Util.Data;

/// <summary>
/// Representa una colección de cadenas de conexión, que se almacena como un diccionario 
/// donde la clave es el nombre de la conexión y el valor es la cadena de conexión correspondiente.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="Dictionary{TKey, TValue}"/> y permite gestionar 
/// múltiples cadenas de conexión de manera eficiente.
/// </remarks>
public class ConnectionStringCollection : Dictionary<string, string> {
    public const string DefaultName = "Default";

    /// <summary>
    /// Obtiene o establece el valor predeterminado asociado con el nombre de propiedad <c>DefaultName</c>.
    /// </summary>
    /// <value>
    /// El valor predeterminado como una cadena.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder y modificar el valor predeterminado de manera sencilla.
    /// Al establecer un nuevo valor, se actualiza automáticamente el almacenamiento interno.
    /// </remarks>
    public string Default {
        get => this.GetValue( DefaultName );
        set => this[DefaultName] = value;
    }

    /// <summary>
    /// Obtiene la cadena de conexión asociada a un nombre específico.
    /// </summary>
    /// <param name="name">El nombre de la cadena de conexión que se desea obtener.</param>
    /// <returns>
    /// La cadena de conexión correspondiente al nombre proporcionado si existe; 
    /// de lo contrario, devuelve la cadena de conexión predeterminada.
    /// </returns>
    public string GetConnectionString( string name ) {
        return ContainsKey( name ) ? this.GetValue( name ) : Default;
    }
}