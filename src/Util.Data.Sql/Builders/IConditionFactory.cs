using Util.Data.Queries;

namespace Util.Data.Sql.Builders; 

/// <summary>
/// Define una interfaz para la creación de condiciones.
/// </summary>
public interface IConditionFactory {
    /// <summary>
    /// Crea una condición SQL basada en una columna, un valor y un operador especificado.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la cual se aplicará la condición.</param>
    /// <param name="value">El valor que se comparará con el valor de la columna.</param>
    /// <param name="@operator">El operador que se utilizará en la condición (por ejemplo, igual, mayor que, etc.).</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta. El valor predeterminado es verdadero.</param>
    /// <returns>Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL creada.</returns>
    /// <remarks>
    /// Este método es útil para construir dinámicamente condiciones SQL en consultas.
    /// Asegúrese de que el operador y los tipos de datos sean compatibles con la columna especificada.
    /// </remarks>
    ISqlCondition Create( string column, object value, Operator @operator, bool isParameterization = true );
    /// <summary>
    /// Crea una condición SQL basada en un rango de valores para una columna específica.
    /// </summary>
    /// <param name="column">El nombre de la columna sobre la que se aplicará la condición.</param>
    /// <param name="minValue">El valor mínimo del rango. Puede ser nulo si no se establece un límite inferior.</param>
    /// <param name="maxValue">El valor máximo del rango. Puede ser nulo si no se establece un límite superior.</param>
    /// <param name="boundary">Especifica cómo se deben tratar los límites del rango (incluidos/excluidos).</param>
    /// <param name="isParameterization">Indica si se debe utilizar la parametrización en la consulta SQL. Por defecto es true.</param>
    /// <returns>Una instancia de <see cref="ISqlCondition"/> que representa la condición SQL creada.</returns>
    /// <remarks>
    /// Este método es útil para construir condiciones SQL dinámicamente, permitiendo la creación de consultas más seguras y eficientes.
    /// Asegúrate de que los valores proporcionados sean compatibles con el tipo de datos de la columna especificada.
    /// </remarks>
    ISqlCondition Create( string column, object minValue,object maxValue, Boundary boundary, bool isParameterization = true );
}