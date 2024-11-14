namespace Util.Validation; 

/// <summary>
/// Representa una colección de resultados de validación.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="List{ValidationResult}"/> y proporciona una forma de manejar múltiples resultados de validación.
/// </remarks>
public class ValidationResultCollection : List<ValidationResult> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ValidationResultCollection"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una colección de resultados de validación utilizando una cadena vacía como parámetro.
    /// </remarks>
    public ValidationResultCollection() : this( "" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ValidationResultCollection"/>.
    /// </summary>
    /// <param name="result">El resultado de validación que se añadirá a la colección. Debe ser una cadena no vacía.</param>
    /// <remarks>
    /// Si el parámetro <paramref name="result"/> es nulo o una cadena vacía, no se añadirá ningún resultado a la colección.
    /// </remarks>
    /// <seealso cref="ValidationResult"/>
    public ValidationResultCollection( string result ) {
        if( string.IsNullOrWhiteSpace( result ) )
            return;
        Add( new ValidationResult( result ) );
    }

    public static readonly ValidationResultCollection Success = new();

    /// <summary>
    /// Obtiene un valor que indica si la colección está vacía.
    /// </summary>
    /// <value>
    /// <c>true</c> si la colección no contiene elementos; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsValid => Count == 0;

    /// <summary>
    /// Agrega una lista de resultados de validación.
    /// </summary>
    /// <param name="results">Una colección de objetos <see cref="ValidationResult"/> que se agregarán.</param>
    /// <remarks>
    /// Este método verifica si la colección de resultados es nula antes de intentar agregar cada resultado.
    /// Si la colección es nula, el método no realiza ninguna acción.
    /// </remarks>
    public void AddList( IEnumerable<ValidationResult> results ) {
        if( results == null )
            return;
        foreach( var result in results )
            Add( result );
    }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el mensaje de error si el objeto no es válido; de lo contrario, una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para obtener información sobre el estado de validez del objeto.
    /// Si el objeto es válido, se devuelve una cadena vacía. Si no es válido, se devuelve el mensaje de error correspondiente.
    /// </remarks>
    public override string ToString() {
        if( IsValid )
            return string.Empty;
        return this.First().ErrorMessage;
    }
}