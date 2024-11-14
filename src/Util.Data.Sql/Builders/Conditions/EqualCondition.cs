using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición de igualdad en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y se utiliza para definir condiciones que comparan dos valores para verificar si son iguales.
/// </remarks>
public class EqualCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EqualCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros de la consulta.</param>
    /// <param name="column">El nombre de la columna con la que se comparará el valor.</param>
    /// <param name="value">El valor que se utilizará en la comparación.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta.</param>
    public EqualCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) 
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Agrega la representación de este objeto al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se agregará la representación del objeto.</param>
    /// <remarks>
    /// Si el valor es nulo, se agrega una condición de nulidad para la columna correspondiente.
    /// De lo contrario, se llama a la implementación base para agregar la representación del objeto.
    /// </remarks>
    public override void AppendTo( StringBuilder builder ) {
        if ( Value == null ) {
            new IsNullCondition( Column ).AppendTo( builder );
            return;
        }
        base.AppendTo( builder );
    }

    /// <summary>
    /// Agrega una condición a un objeto <see cref="StringBuilder"/> en formato de comparación.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregará la condición.</param>
    /// <param name="column">El nombre de la columna que se utilizará en la condición.</param>
    /// <param name="value">El valor que se comparará con la columna especificada.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0}={1}", column, value);
    }

    /// <summary>
    /// Agrega una representación SQL de una columna y su valor asociado al 
    /// <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> donde se construirá la consulta SQL.</param>
    /// <param name="column">El nombre de la columna que se está procesando.</param>
    /// <param name="sqlBuilder">Una instancia de <see cref="ISqlBuilder"/> que se utiliza para construir la parte SQL del valor.</param>
    /// <remarks>
    /// Este método sobrescribe el comportamiento base para personalizar la forma en que se 
    /// construye la consulta SQL, asegurando que el formato sea correcto y que se incluya 
    /// la representación adecuada del valor en función de la lógica del <see cref="ISqlBuilder"/>.
    /// </remarks>
    protected override void AppendSqlBuilder( StringBuilder builder, string column, ISqlBuilder sqlBuilder ) {
        builder.AppendFormat( "{0}=", column );
        builder.Append( "(" );
        sqlBuilder.AppendTo( builder );
        builder.Append( ")" );
    }
}