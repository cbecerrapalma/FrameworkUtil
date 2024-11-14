namespace Util.Validation; 

/// <summary>
/// Define un manejador de validación que puede ser implementado por clases que 
/// deseen proporcionar lógica de validación específica.
/// </summary>
public interface IValidationHandler {
    /// <summary>
    /// Maneja una colección de resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación que se va a manejar.</param>
    /// <remarks>
    /// Este método procesa los resultados de validación, permitiendo realizar acciones específicas
    /// en función de los resultados obtenidos. Es importante asegurarse de que la colección no sea nula
    /// antes de llamar a este método.
    /// </remarks>
    void Handle(ValidationResultCollection results);
}