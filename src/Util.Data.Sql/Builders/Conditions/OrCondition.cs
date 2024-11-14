namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición lógica OR para ser utilizada en consultas SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y permite combinar múltiples condiciones
/// utilizando la lógica OR. Es útil para construir consultas SQL dinámicamente.
/// </remarks>
public class OrCondition : ISqlCondition {
    private readonly ISqlCondition _condition1;
    private readonly ISqlCondition _condition2;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrCondition"/>.
    /// </summary>
    /// <param name="condition1">La primera condición SQL que se evaluará. No puede ser nula.</param>
    /// <param name="condition2">La segunda condición SQL que se evaluará. Puede ser nula.</param>
    /// <remarks>
    /// Si <paramref name="condition1"/> es una instancia de <see cref="NullCondition"/>, se asignará como nula.
    /// De igual manera, si <paramref name="condition2"/> es una instancia de <see cref="NullCondition"/>, se asignará como nula.
    /// </remarks>
    public OrCondition( ISqlCondition condition1, ISqlCondition condition2 = null ) {
        _condition1 = condition1;
        _condition2 = condition2;
        if ( condition1 != null && condition1 == NullCondition.Instance )
            _condition1 = null;
        if( condition2 != null && condition2 == NullCondition.Instance )
            _condition2 = null;
    }

    /// <summary>
    /// Agrega condiciones a un objeto <see cref="StringBuilder"/> dado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le agregarán las condiciones.</param>
    /// <remarks>
    /// Este método verifica el estado de dos condiciones (_condition1 y _condition2).
    /// Si ambas son nulas, no realiza ninguna acción. Si ambas están presentes, se llama al método
    /// <see cref="AppendAllCondition"/> para agregar todas las condiciones. Si solo una de las condiciones
    /// está presente, se agrega la condición correspondiente utilizando el método <see cref="AppendCondition"/>.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if( _condition1 == null && _condition2 == null )
            return;
        if ( _condition1 != null && _condition2 != null ) {
            AppendAllCondition( builder );
            return;
        }
        AppendCondition( builder, _condition1 );
        AppendCondition( builder, _condition2 );
    }

    /// <summary>
    /// Agrega condiciones a un objeto StringBuilder.
    /// </summary>
    /// <param name="builder">El StringBuilder al que se le agregarán las condiciones.</param>
    /// <remarks>
    /// Si el StringBuilder está vacío, se llama al método Or y se retorna. 
    /// De lo contrario, se agrega " And " al StringBuilder antes de llamar al método Or.
    /// </remarks>
    private void AppendAllCondition( StringBuilder builder ) {
        if( builder.Length == 0 ) {
            Or( builder );
            return;
        }
        builder.Append( " And " );
        Or( builder );
    }

    /// <summary>
    /// Combina dos condiciones utilizando el operador lógico "Or".
    /// </summary>
    /// <param name="builder">El objeto StringBuilder donde se construirá la representación de la condición.</param>
    private void Or( StringBuilder builder ) {
        builder.Append( "(" );
        _condition1.AppendTo( builder );
        builder.Append( " Or " );
        _condition2.AppendTo( builder );
        builder.Append( ")" );
    }

    /// <summary>
    /// Agrega una condición SQL a un objeto StringBuilder.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le agregará la condición.</param>
    /// <param name="condition">La condición SQL que se desea agregar. Si es nula, no se realiza ninguna acción.</param>
    /// <remarks>
    /// Si el StringBuilder está vacío, la condición se agrega directamente. 
    /// Si ya contiene texto, se inserta un paréntesis al inicio y se agrega " Or " antes de la condición.
    /// Finalmente, se cierra el paréntesis al final.
    /// </remarks>
    private void AppendCondition( StringBuilder builder,ISqlCondition condition ) {
        if ( condition == null )
            return;
        if( builder.Length == 0 ) {
            condition.AppendTo( builder );
            return;
        }
        builder.Insert( 0, '(' );
        builder.Append( " Or " );
        condition.AppendTo( builder );
        builder.Append( ")" );
    }
}