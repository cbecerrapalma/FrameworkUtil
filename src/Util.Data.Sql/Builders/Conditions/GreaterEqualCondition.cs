using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor es mayor o igual a otro.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y proporciona la lógica necesaria
/// para construir una condición SQL que utiliza el operador mayor o igual (>=).
/// </remarks>
public class GreaterEqualCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GreaterEqualCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros de la consulta.</param>
    /// <param name="column">El nombre de la columna que se evaluará en la condición.</param>
    /// <param name="value">El valor con el que se comparará el valor de la columna.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta.</param>
    public GreaterEqualCondition( IParameterManager parameterManager, string column, object value, bool isParameterization )
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Agrega una condición a la consulta en formato de cadena.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que contiene la consulta.</param>
    /// <param name="column">El nombre de la columna a la que se le aplicará la condición.</param>
    /// <param name="value">El valor que se comparará con la columna.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0}>={1}", column, value);
    }

    /// <summary>
    /// Agrega una cláusula SQL al constructor de consultas.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder donde se construirá la consulta SQL.</param>
    /// <param name="column">El nombre de la columna a la que se aplicará la cláusula.</param>
    /// <param name="sqlBuilder">El objeto ISqlBuilder que contiene la lógica para construir la parte de la consulta SQL.</param>
    protected override void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) {
        builder.AppendFormat("{0}>=", column);
        builder.Append("(");
        sqlBuilder.AppendTo(builder);
        builder.Append(")");
    }
}