using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición que se evalúa en una consulta SQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlConditionBase"/> y permite definir condiciones específicas
/// que pueden ser utilizadas en la construcción de consultas SQL.
/// </remarks>
public class StartsCondition : SqlConditionBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="StartsCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros que se utilizará para gestionar los parámetros.</param>
    /// <param name="column">El nombre de la columna que se evaluará en la condición.</param>
    /// <param name="value">El valor que se comparará con el contenido de la columna.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la condición.</param>
    public StartsCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) 
        : base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Obtiene el valor formateado como un porcentaje.
    /// </summary>
    /// <returns>
    /// Un objeto que representa el valor formateado como un porcentaje, 
    /// concatenando el valor actual con el símbolo de porcentaje ("%").
    protected override object GetValue() {
        return $"{Value}%";
    }

    /// <summary>
    /// Agrega una condición de búsqueda al objeto <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <param name="column">El nombre de la columna en la que se aplicará la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición de búsqueda.</param>
    protected override void AppendCondition(StringBuilder builder, string column, object value) {
        builder.AppendFormat("{0} Like {1}", column, value);
    }

    /// <summary>
    /// Agrega una condición no parametrizada al objeto <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá la condición.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que añade una condición basada en el valor de la columna y el valor obtenido.
    /// </remarks>
    protected override void AppendNonParameterizedCondition(StringBuilder builder)
    {
        AppendCondition(builder, Column, $"'{GetValue()}'");
    }
}