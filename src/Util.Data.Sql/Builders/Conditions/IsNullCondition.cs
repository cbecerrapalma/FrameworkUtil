namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor es nulo.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir
/// condiciones SQL que comprueban la nulidad de un campo o expresión.
/// </remarks>
public class IsNullCondition : ISqlCondition {
    private readonly string _name;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IsNullCondition"/>.
    /// </summary>
    /// <param name="name">El nombre que se asignará a la condición.</param>
    public IsNullCondition( string name ) {
        _name = name;
    }

    /// <summary>
    /// Agrega una cadena al <see cref="StringBuilder"/> especificado si el nombre no está vacío.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la cadena.</param>
    /// <remarks>
    /// Si el campo <c>_name</c> está vacío, no se realiza ninguna acción.
    /// De lo contrario, se formatea una cadena que indica que el nombre es nulo y se añade al <c>builder</c>.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if ( _name.IsEmpty() )
            return;
        builder.AppendFormat( "{0} Is Null", _name );
    }
}