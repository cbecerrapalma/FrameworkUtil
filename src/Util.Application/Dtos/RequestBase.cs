using Util.Exceptions;
using Util.Helpers;
using Util.Validation;

namespace Util.Applications.Dtos; 

/// <summary>
/// Clase base abstracta para representar una solicitud.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="IRequest"/> y proporciona una base común 
/// para todas las solicitudes en el sistema. Las clases derivadas deben implementar 
/// los métodos y propiedades necesarios para definir el comportamiento específico de 
/// cada tipo de solicitud.
/// </remarks>
public abstract class RequestBase : IRequest {
    /// <summary>
    /// Valida el modelo actual utilizando un validador de objetos.
    /// </summary>
    /// <returns>
    /// Una colección de resultados de validación que indica si la validación fue exitosa.
    /// </returns>
    /// <remarks>
    /// Si el validador de objetos no está disponible, se utiliza la validación de anotaciones de datos.
    /// En caso de que la validación falle, se lanza una excepción de tipo <see cref="Warning"/> 
    /// con el mensaje de error correspondiente.
    /// </remarks>
    /// <exception cref="Warning">
    /// Se lanza cuando la validación falla y se proporciona un mensaje de error.
    /// </exception>
    public virtual ValidationResultCollection Validate() {
        var validator = Ioc.Create<IObjectModelValidator>();
        if ( validator == null ) {
            var result = DataAnnotationValidation.Validate( this );
            if ( result.IsValid )
                return ValidationResultCollection.Success;
            throw new Warning( result.First().ErrorMessage );
        }
        var actionContext = new ActionContext();
        validator.Validate( actionContext, null, string.Empty, this );
        if ( actionContext.ModelState.IsValid )
            return ValidationResultCollection.Success;
        throw new Warning( actionContext.ModelState.Values.First().Errors.First().ErrorMessage );
    }
}