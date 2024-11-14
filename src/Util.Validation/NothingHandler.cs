namespace Util.Validation; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IValidationHandler"/>.
/// </summary>
/// <remarks>
/// Esta clase se encarga de manejar la validación de manera que no se realice ninguna acción.
/// </remarks>
public class NothingHandler : IValidationHandler {
    /// <summary>
    /// Maneja una colección de resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación que se va a manejar.</param>
    public void Handle( ValidationResultCollection results ) {
    }
}