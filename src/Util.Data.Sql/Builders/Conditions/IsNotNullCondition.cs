namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor no es nulo.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y se utiliza para construir
/// condiciones SQL que aseguran que un campo específico no tenga un valor nulo.
/// </remarks>
public class IsNotNullCondition : ISqlCondition {
    private readonly string _name;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="IsNotNullCondition"/>.
    /// </summary>
    /// <param name="name">El nombre de la condición que se está creando.</param>
    public IsNotNullCondition( string name ) {
        _name = name;
    }

    /// <summary>
    /// Agrega una cadena al <see cref="StringBuilder"/> especificado si el nombre no está vacío.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le agregará la cadena.</param>
    /// <remarks>
    /// Este método verifica si el campo <c>_name</c> está vacío antes de intentar agregar la cadena. 
    /// Si <c>_name</c> está vacío, no se realiza ninguna acción.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if( _name.IsEmpty() )
            return;
        builder.AppendFormat( "{0} Is Not Null", _name );
    }
}