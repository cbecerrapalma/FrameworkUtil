using Util;
using Util.Properties;
using Util.Validation.Validators;

namespace System.ComponentModel.DataAnnotations; 

/// <summary>
/// Indica que un atributo se puede aplicar a propiedades.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para especificar que el atributo asociado puede ser aplicado a miembros de tipo propiedad.
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class IdCardAttribute : ValidationAttribute {
    /// <summary>
    /// Formatea el mensaje de error para un campo específico.
    /// </summary>
    /// <param name="name">El nombre del campo al que se aplica el mensaje de error.</param>
    /// <returns>El mensaje de error formateado.</returns>
    /// <remarks>
    /// Si tanto <c>ErrorMessage</c> como <c>ErrorMessageResourceName</c> son nulos, se asigna un mensaje de error predeterminado.
    /// </remarks>
    public override string FormatErrorMessage( string name ) {
        if( ErrorMessage == null && ErrorMessageResourceName == null )
            ErrorMessage = R.InvalidIdCard;
        return string.Format( CultureInfo.CurrentCulture, ErrorMessageString );
    }

    /// <summary>
    /// Valida el valor proporcionado en función de un patrón específico para un documento de identidad.
    /// </summary>
    /// <param name="value">El valor que se va a validar, que representa un documento de identidad.</param>
    /// <param name="validationContext">El contexto de validación que proporciona información adicional sobre el proceso de validación.</param>
    /// <returns>
    /// Un objeto <see cref="ValidationResult"/> que indica el resultado de la validación. 
    /// Devuelve <see cref="ValidationResult.Success"/> si el valor es válido o si está vacío; 
    /// de lo contrario, devuelve un nuevo <see cref="ValidationResult"/> con un mensaje de error.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base <see cref="ValidationAttribute.IsValid"/> 
    /// para proporcionar una validación personalizada de documentos de identidad.
    /// </remarks>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) 
    {
        if (value.SafeString().IsEmpty())
            return ValidationResult.Success;
        if (Util.Helpers.Regex.IsMatch(value.SafeString(), ValidatePattern.IdCardPattern))
            return ValidationResult.Success;
        return new ValidationResult(FormatErrorMessage(string.Empty));
    }
}