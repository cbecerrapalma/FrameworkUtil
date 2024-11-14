using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición de finalización en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y se utiliza para definir condiciones específicas 
/// que determinan cuándo se considera que una consulta ha alcanzado su estado final.
/// </remarks>
public class EndsCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EndsCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que gestiona los parámetros utilizados en la condición.</param>
    /// <param name="column">El nombre de la columna sobre la cual se aplica la condición.</param>
    /// <param name="value">El valor que se utilizará para evaluar la condición.</param>
    /// <param name="isParameterization">Indica si la condición utiliza parametrización.</param>
    public EndsCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) 
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Obtiene el valor formateado con un símbolo de porcentaje.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el valor formateado como una cadena, precedido por un símbolo de porcentaje.
    /// </returns>
    protected override object GetValue() {
        return $"%{Value}";
    }

    /// <summary>
    /// Agrega una condición de búsqueda a la cadena de consulta.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que contiene la consulta SQL.</param>
    /// <param name="column">El nombre de la columna sobre la que se aplica la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición de búsqueda.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0} Like {1}", column, value);
    }

    /// <summary>
    /// Agrega una condición no parametrizada a un <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que utiliza el valor obtenido de <see cref="GetValue"/> y lo formatea como una cadena
    /// para ser utilizada en la condición.
    /// </remarks>
    protected override void AppendNonParameterizedCondition(StringBuilder builder) 
    {
        AppendCondition(builder, Column, $"'{GetValue()}'");
    }
}