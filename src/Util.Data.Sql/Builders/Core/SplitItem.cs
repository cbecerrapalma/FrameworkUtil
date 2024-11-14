namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento que puede ser dividido en partes.
/// </summary>
public class SplitItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SplitItem"/> 
    /// dividiendo una cadena en dos partes utilizando un separador especificado.
    /// </summary>
    /// <param name="value">La cadena que se va a dividir.</param>
    /// <param name="separator">El separador que se utilizará para dividir la cadena. Por defecto es un espacio en blanco.</param>
    /// <remarks>
    /// Si la cadena proporcionada es nula o está vacía, no se realizará ninguna acción.
    /// Si la cadena se divide en una sola parte, solo se asignará a la propiedad <see cref="Left"/>.
    /// Si se divide en dos partes, se asignarán a las propiedades <see cref="Left"/> y <see cref="Right"/> respectivamente.
    /// </remarks>
    public SplitItem( string value,string separator = " " ) {
        if ( string.IsNullOrEmpty( value ) )
            return;
        var list = value.Trim().Split( separator ).Where( item => item.IsEmpty() == false ).ToList();
        if( list.Count == 1 ) {
            Left = list[0];
            return;
        }
        if ( list.Count == 2 ) {
            Left = list[0];
            Right = list[1];
        }
    }

    /// <summary>
    /// Obtiene o establece el valor de la propiedad Left.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el valor de la propiedad Left.
    /// </value>
    public string Left { get; set; }

    /// <summary>
    /// Obtiene o establece el valor de la propiedad Right.
    /// </summary>
    /// <remarks>
    /// Esta propiedad representa un valor que puede ser utilizado para definir la alineación o posición de un elemento 
    /// en relación con su contenedor o contexto.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el valor de la propiedad Right.
    /// </value>
    public string Right { get; set; }
}