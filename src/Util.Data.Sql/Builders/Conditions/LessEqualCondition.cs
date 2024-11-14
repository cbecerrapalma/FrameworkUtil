using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor es menor o igual a otro.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y se utiliza para construir
/// condiciones SQL en consultas, específicamente para la comparación de valores.
/// </remarks>
public class LessEqualCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LessEqualCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros de la condición.</param>
    /// <param name="column">El nombre de la columna a la que se aplicará la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición de menor o igual.</param>
    /// <param name="isParameterization">Indica si la condición debe ser parametrizada.</param>
    public LessEqualCondition( IParameterManager parameterManager, string column, object value, bool isParameterization )
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Agrega una condición a la cadena de consulta que indica que el valor de la columna debe ser menor o igual al valor especificado.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que contiene la consulta SQL en construcción.</param>
    /// <param name="column">El nombre de la columna a la que se aplica la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) 
    { 
        builder.AppendFormat("{0}<={1}", column, value); 
    }

    /// <summary>
    /// Agrega una cláusula SQL al constructor de cadenas especificado.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le agregará la cláusula SQL.</param>
    /// <param name="column">El nombre de la columna a la que se aplicará la cláusula.</param>
    /// <param name="sqlBuilder">El objeto que contiene la lógica para construir la parte de la consulta SQL.</param>
    protected override void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) {
        builder.AppendFormat("{0}<=", column);
        builder.Append("(");
        sqlBuilder.AppendTo(builder);
        builder.Append(")");
    }
}