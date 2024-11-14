using Util.Exceptions;
using Util.Validation;

namespace Util.Domain.Services; 

/// <summary>
/// Clase base abstracta que representa un parámetro en el sistema de validación.
/// Implementa la interfaz <see cref="IValidation"/> para proporcionar funcionalidad de validación.
/// </summary>
public abstract class ParameterBase : IValidation {
    /// <summary>
    /// Valida el objeto actual utilizando las anotaciones de datos.
    /// </summary>
    /// <returns>
    /// Una colección de resultados de validación que indica si la validación fue exitosa.
    /// </returns>
    /// <exception cref="Warning">
    /// Se lanza una excepción de tipo <see cref="Warning"/> si la validación falla,
    /// conteniendo el mensaje de error de la primera validación fallida.
    /// </exception>
    public virtual ValidationResultCollection Validate() {
        var result = DataAnnotationValidation.Validate( this );
        if( result.IsValid )
            return ValidationResultCollection.Success;
        throw new Warning( result.First().ErrorMessage );
    }
}