using Util.Data.Sql.Builders.Params;

namespace Util.Data.Sql.Builders.Conditions; 

/// <summary>
/// Representa una condición que evalúa si un valor no está presente en un conjunto de valores.
/// Esta clase hereda de <see cref="InCondition"/>.
/// </summary>
/// <remarks>
/// La clase <see cref="NotInCondition"/> se utiliza para definir condiciones de exclusión en consultas o filtros,
/// permitiendo verificar que un elemento no pertenezca a un conjunto específico.
/// </remarks>
public class NotInCondition : InCondition {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotInCondition"/>.
    /// </summary>
    /// <param name="parameterManager">El administrador de parámetros utilizado para gestionar los parámetros de la consulta.</param>
    /// <param name="column">El nombre de la columna en la que se aplicará la condición.</param>
    /// <param name="value">El valor que se utilizará en la condición NOT IN.</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta.</param>
    public NotInCondition( IParameterManager parameterManager, string column, object value, bool isParameterization ) 
        :base( parameterManager, column, value, isParameterization ) {
    }

    /// <summary>
    /// Obtiene el operador utilizado en la comparación.
    /// </summary>
    /// <returns>
    /// Un string que representa el operador "Not In".
    /// </returns>
    protected override string GetOperator() {
        return "Not In";
    }
}