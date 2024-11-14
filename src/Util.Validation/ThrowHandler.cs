using Util.Exceptions;

namespace Util.Validation; 

/// <summary>
/// Clase que maneja la validación lanzando excepciones cuando se detectan errores.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IValidationHandler"/> y se encarga de
/// gestionar las validaciones de forma que, en caso de error, se lance una excepción
/// correspondiente. Esto permite que el flujo de la aplicación se detenga y se manejen
/// los errores de manera adecuada.
/// </remarks>
public class ThrowHandler : IValidationHandler{
    /// <summary>
    /// Maneja la colección de resultados de validación.
    /// </summary>
    /// <param name="results">La colección de resultados de validación que se va a manejar.</param>
    /// <exception cref="Warning">Se lanza una excepción de tipo <see cref="Warning"/> si la colección de resultados no es válida.</exception>
    /// <remarks>
    /// Este método verifica si la colección de resultados de validación es válida. 
    /// Si es válida, no realiza ninguna acción. Si no es válida, lanza una excepción 
    /// con el mensaje de error del primer resultado de la colección.
    /// </remarks>
    public void Handle( ValidationResultCollection results ) {
        if ( results.IsValid )
            return;
        throw new Warning( results.First().ErrorMessage );
    }
}