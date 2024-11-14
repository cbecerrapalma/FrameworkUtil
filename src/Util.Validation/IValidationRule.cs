namespace Util.Validation; 

/// <summary>
/// Define una interfaz para las reglas de validación.
/// </summary>
/// <remarks>
/// Esta interfaz debe ser implementada por cualquier clase que desee definir una regla de validación.
/// </remarks>
public interface IValidationRule {
    /// <summary>
    /// Valida el estado actual del objeto y devuelve el resultado de la validación.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="ValidationResult"/> que contiene el resultado de la validación,
    /// incluyendo información sobre si la validación fue exitosa o no, y detalles adicionales
    /// en caso de errores.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado para realizar las validaciones específicas requeridas
    /// por la lógica de negocio del objeto. Se recomienda que las validaciones sean claras y
    /// devuelvan mensajes de error descriptivos para facilitar la identificación de problemas.
    /// </remarks>
    ValidationResult Validate();
}