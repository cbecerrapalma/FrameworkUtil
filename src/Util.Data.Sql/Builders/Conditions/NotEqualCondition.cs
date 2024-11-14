using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición de desigualdad en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y se utiliza para construir condiciones
/// que verifican si dos valores no son iguales en una consulta SQL.
/// </remarks>
public class NotEqualCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotEqualCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que gestiona los parámetros utilizados en la condición.</param>
    /// <param name="column">El nombre de la columna a la que se aplica la condición.</param>
    /// <param name="value">El valor que se utilizará en la comparación de desigualdad.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta.</param>
    public NotEqualCondition( IParameterManager parameterManager, string column, object value, bool isParameterization )
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Agrega la representación de este objeto al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se agregará la representación del objeto.</param>
    /// <remarks>
    /// Si el valor es nulo, se agrega una condición que verifica que la columna no sea nula.
    /// En caso contrario, se llama al método base para realizar la operación de append.
    /// </remarks>
    public override void AppendTo( StringBuilder builder ) {
        if( Value == null ) {
            new IsNotNullCondition( Column ).AppendTo( builder );
            return;
        }
        base.AppendTo( builder );
    }

    /// <summary>
    /// Agrega una condición de desigualdad a la consulta en el objeto StringBuilder.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que contiene la consulta SQL.</param>
    /// <param name="column">El nombre de la columna a la que se le aplicará la condición.</param>
    /// <param name="value">El valor con el que se comparará la columna.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0}<>{1}", column, value);
    }

    /// <summary>
    /// Agrega una construcción SQL al objeto StringBuilder especificado.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le añadirá la construcción SQL.</param>
    /// <param name="column">El nombre de la columna que se utilizará en la construcción SQL.</param>
    /// <param name="sqlBuilder">El objeto ISqlBuilder que contiene la lógica para construir la parte de la consulta SQL.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica 
    /// que agrega una condición de desigualdad a la consulta SQL.
    /// </remarks>
    protected override void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) {
        builder.AppendFormat("{0}<>", column);
        builder.Append("(");
        sqlBuilder.AppendTo(builder);
        builder.Append(")");
    }
}