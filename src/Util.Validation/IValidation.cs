namespace Util.Validation; 

/// <summary>
/// Define un contrato para la validación de datos.
/// </summary>
public interface IValidation {
    /// <summary>
    /// Valida un conjunto de datos y devuelve los resultados de la validación.
    /// </summary>
    /// <returns>
    /// Una colección de resultados de validación que contiene información sobre los errores o advertencias encontrados durante el proceso de validación.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para verificar la integridad y validez de los datos antes de proceder con operaciones adicionales.
    /// </remarks>
    /// <seealso cref="ValidationResultCollection"/>
    ValidationResultCollection Validate();
}