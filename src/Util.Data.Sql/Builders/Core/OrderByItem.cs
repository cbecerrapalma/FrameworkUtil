namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento de ordenación para una colección de elementos.
/// </summary>
public class OrderByItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrderByItem"/>.
    /// </summary>
    /// <param name="order">Una cadena que representa el nombre de la columna y el orden (ascendente o descendente).</param>
    /// <remarks>
    /// Este constructor procesa la cadena de entrada para determinar el nombre de la columna y si el orden es ascendente o descendente.
    /// Si la cadena termina con " Asc", se considera que el orden es ascendente. Si termina con " Desc", se establece la propiedad <see cref="Desc"/> en verdadero.
    /// </remarks>
    public OrderByItem( string order ) {
        Column = order.SafeString().Trim();
        if ( Column.EndsWith( " Asc",StringComparison.OrdinalIgnoreCase ) ) {
            Column = Column.Substring( 0, Column.Length - 3 ).Trim();
            return;
        }
        if( Column.EndsWith( " Desc", StringComparison.OrdinalIgnoreCase ) ) {
            Desc = true;
            Column = Column.Substring( 0, Column.Length - 4 ).Trim();
        }
    }

    /// <summary>
    /// Obtiene el nombre de la columna.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el nombre de la columna 
    /// asociada a la instancia actual. 
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre de la columna.
    /// </value>
    public string Column { get; }

    /// <summary>
    /// Obtiene un valor que indica si el orden es descendente.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para determinar el tipo de ordenación que se aplicará a una colección de elementos.
    /// </remarks>
    /// <returns>
    /// Devuelve <c>true</c> si el orden es descendente; de lo contrario, <c>false</c>.
    /// </returns>
    public bool Desc { get; }
}