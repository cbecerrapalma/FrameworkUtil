using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor es menor que otro.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y proporciona la funcionalidad específica
/// para evaluar condiciones de tipo "menor que" en consultas SQL.
/// </remarks>
public class LessCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LessCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros.</param>
    /// <param name="column">El nombre de la columna que se está evaluando.</param>
    /// <param name="value">El valor con el que se comparará el valor de la columna.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización para la condición.</param>
    public LessCondition( IParameterManager parameterManager, string column, object value, bool isParameterization )
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Agrega una condición a un objeto StringBuilder en formato de comparación.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le añadirá la condición.</param>
    /// <param name="column">El nombre de la columna que se utilizará en la condición.</param>
    /// <param name="value">El valor que se comparará con la columna.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0}<{1}", column, value);
    }

    /// <summary>
    /// Agrega una representación SQL de una columna al generador de SQL.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que se utilizará para construir la cadena SQL.</param>
    /// <param name="column">El nombre de la columna que se está agregando.</param>
    /// <param name="sqlBuilder">El generador de SQL que proporciona la representación SQL adicional.</param>
    protected override void AppendSqlBuilder(StringBuilder builder, string column, ISqlBuilder sqlBuilder) 
    { 
        builder.AppendFormat("{0}<", column); 
        builder.Append("("); 
        sqlBuilder.AppendTo(builder); 
        builder.Append(")"); 
    }
}