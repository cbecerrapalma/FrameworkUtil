using AspectCore.DynamicProxy.Parameters;
using Util.Aop;
using Util.Helpers;

namespace Util.Validation; 

/// <summary>
/// Clase que representa un atributo de validación.
/// Hereda de <see cref="ParameterInterceptorBase"/>.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para interceptar y validar parámetros en métodos.
/// </remarks>
public class ValidAttribute : ParameterInterceptorBase {
    /// <summary>
    /// Invoca el siguiente delegado en la cadena de ejecución después de validar el parámetro.
    /// </summary>
    /// <param name="context">El contexto que contiene información sobre el parámetro actual.</param>
    /// <param name="next">El delegado que representa la siguiente acción en la cadena de ejecución.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una validación adicional
    /// antes de continuar con la ejecución del siguiente delegado.
    /// </remarks>
    public override async Task Invoke( ParameterAspectContext context, ParameterAspectDelegate next ) {
        Validate( context.Parameter );
        await next( context );
    }

    /// <summary>
    /// Valida un parámetro dado. Si el tipo del parámetro es una colección genérica, 
    /// se valida la colección. Si no, se intenta validar el valor del parámetro 
    /// si implementa la interfaz <see cref="IValidation"/>.
    /// </summary>
    /// <param name="parameter">El parámetro que se va a validar.</param>
    /// <remarks>
    /// Este método verifica si el tipo del parámetro es una colección genérica 
    /// utilizando la clase <see cref="Reflection"/>. Si es así, se llama al método 
    /// <see cref="ValidateCollection(Parameter)"/> para realizar la validación de la colección.
    /// En caso contrario, se intenta validar el valor del parámetro si es una instancia 
    /// de <see cref="IValidation"/>.
    /// </remarks>
    private void Validate( Parameter parameter ) {
        if ( Reflection.IsGenericCollection( parameter.RawType ) ) {
            ValidateCollection( parameter );
            return;
        }
        IValidation validation = parameter.Value as IValidation;
        validation?.Validate();
    }

    /// <summary>
    /// Valida una colección de objetos que implementan la interfaz <see cref="IValidation"/>.
    /// </summary>
    /// <param name="parameter">El parámetro que contiene la colección a validar.</param>
    /// <remarks>
    /// Este método verifica si el valor del parámetro es una colección de objetos que implementan la interfaz 
    /// <see cref="IValidation"/>. Si es así, se itera sobre cada objeto de la colección y se llama al método 
    /// <see cref="IValidation.Validate"/> para realizar la validación.
    /// </remarks>
    private void ValidateCollection( Parameter parameter ) {
        if ( !( parameter.Value is IEnumerable<IValidation> validations ) )
            return;
        foreach ( var validation in validations )
            validation.Validate();
    }
}