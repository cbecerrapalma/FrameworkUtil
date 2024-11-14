namespace Util.Domain.Compare; 

/// <summary>
/// Representa una colección de objetos <see cref="ChangeValue"/>.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="List{ChangeValue}"/> y proporciona funcionalidades adicionales
/// específicas para manejar una colección de cambios de valor.
/// </remarks>
public class ChangeValueCollection : List<ChangeValue> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChangeValueCollection"/>.
    /// </summary>
    /// <param name="typeName">El nombre del tipo asociado a la colección.</param>
    /// <param name="typeDescription">Una descripción del tipo asociado a la colección.</param>
    /// <param name="id">El identificador único de la colección.</param>
    public ChangeValueCollection( string typeName = null,string typeDescription = null, string id = null ) {
        TypeName = typeName;
        TypeDescription = typeDescription;
        Id = id;
    }

    /// <summary>
    /// Obtiene el nombre del tipo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el nombre del tipo asociado.
    /// </remarks>
    /// <returns>
    /// Un <see cref="string"/> que representa el nombre del tipo.
    /// </returns>
    public string TypeName { get; }

    /// <summary>
    /// Obtiene la descripción del tipo.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona una representación en forma de cadena del tipo.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa la descripción del tipo.
    /// </returns>
    public string TypeDescription { get; }

    /// <summary>
    /// Obtiene el identificador de la entidad.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para acceder al identificador único de la entidad.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador de la entidad.
    /// </value>
    public string Id { get; }

    /// <summary>
    /// Agrega un nuevo cambio de valor a la colección.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que ha cambiado.</param>
    /// <param name="description">Una descripción del cambio realizado.</param>
    /// <param name="originalValue">El valor original de la propiedad antes del cambio.</param>
    /// <param name="newValue">El nuevo valor de la propiedad después del cambio.</param>
    /// <remarks>
    /// Este método no realiza ninguna acción si el nombre de la propiedad es nulo o está vacío.
    /// </remarks>
    public void Add( string propertyName, string description, string originalValue, string newValue ) {
        if( string.IsNullOrWhiteSpace( propertyName ) )
            return;
        Add( new ChangeValue( propertyName, description, originalValue, newValue ) );
    }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el objeto actual, incluyendo el nombre del tipo, la descripción,
    /// el identificador (si no está vacío) y los elementos contenidos en el objeto.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método <see cref="object.ToString"/> para proporcionar una
    /// representación más significativa del objeto, incluyendo detalles sobre su estado.
    /// </remarks>
    public override string ToString() {
        var result = new StringBuilder();
        result.Append( $"{TypeName}({TypeDescription})," );
        if ( Id.IsEmpty() == false )
            result.Append( $"Id: {Id}," );
        foreach ( var item in this )
            result.Append( $"{item}," );
        return result.ToString().TrimEnd( ',' );
    }
}