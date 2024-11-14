namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición lógica AND en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ISqlCondition"/> y permite combinar múltiples condiciones
/// utilizando la lógica AND. Se puede utilizar para construir consultas SQL más complejas.
/// </remarks>
public class AndCondition : ISqlCondition {
    private readonly ISqlCondition _condition1;
    private readonly ISqlCondition _condition2;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="AndCondition"/>.
    /// </summary>
    /// <param name="condition1">La primera condición SQL que se evaluará.</param>
    /// <param name="condition2">La segunda condición SQL que se evaluará. Si es <c>null</c>, solo se utilizará <paramref name="condition1"/>.</param>
    public AndCondition( ISqlCondition condition1, ISqlCondition condition2 = null ) {
        _condition1 = condition1;
        _condition2 = condition2;
    }

    /// <summary>
    /// Agrega condiciones a un objeto StringBuilder si al menos una de las condiciones es diferente de null.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se agregarán las condiciones.</param>
    /// <remarks>
    /// Este método verifica si ambas condiciones (_condition1 y _condition2) son nulas. 
    /// Si ambas son nulas, el método no realiza ninguna acción. 
    /// Si al menos una de las condiciones es válida, se llama al método And para agregar cada condición al StringBuilder.
    /// </remarks>
    public void AppendTo( StringBuilder builder ) {
        if( _condition1 == null && _condition2 == null )
            return;
        And( builder, _condition1 );
        And( builder, _condition2 );
    }

    /// <summary>
    /// Agrega una condición SQL al objeto StringBuilder especificado, 
    /// precedida por " And " si ya hay contenido en el builder.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le añadirá la condición.</param>
    /// <param name="condition">La condición SQL que se desea agregar. 
    /// Si es null o una instancia de <see cref="NullCondition"/>, no se realizará ninguna acción.</param>
    private void And( StringBuilder builder,ISqlCondition condition ) {
        if ( condition == null || condition == NullCondition.Instance )
            return;
        if ( builder.Length == 0 ) {
            condition.AppendTo( builder );
            return;
        }
        builder.Append( " And " );
        condition.AppendTo( builder );
    }
}