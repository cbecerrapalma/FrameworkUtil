using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición SQL que verifica si un valor está contenido en un conjunto de valores.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y se utiliza para construir condiciones SQL
/// que utilizan la cláusula "IN" para determinar si un valor específico se encuentra dentro de un conjunto.
/// </remarks>
public class ContainsCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ContainsCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que gestiona los parámetros de la consulta.</param>
    /// <param name="column">El nombre de la columna en la que se aplicará la condición.</param>
    /// <param name="value">El valor que se utilizará para la condición de contención.</param>
    /// <param name="isParameterization">Indica si la condición debe ser parametrizada.</param>
    public ContainsCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) 
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Obtiene el valor formateado con signos de porcentaje.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el valor formateado como una cadena, 
    /// rodeado por signos de porcentaje.
    /// </returns>
    protected override object GetValue() {
        return $"%{Value}%";
    }

    /// <summary>
    /// Agrega una condición a la consulta en formato de texto.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder que contiene la consulta en construcción.</param>
    /// <param name="column">El nombre de la columna a la que se aplica la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición, que se espera que sea compatible con el formato de la consulta.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0} Like {1}", column, value);
    }

    /// <summary>
    /// Agrega una condición no parametrizada al constructor de la consulta.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> que se utiliza para construir la consulta.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que añade una condición basada en el valor de la columna y el valor obtenido.
    /// </remarks>
    protected override void AppendNonParameterizedCondition(StringBuilder builder) 
    { 
        AppendCondition(builder, Column, $"'{GetValue()}'"); 
    }
}