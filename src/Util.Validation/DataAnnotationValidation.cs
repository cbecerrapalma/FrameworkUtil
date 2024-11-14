namespace Util.Validation; 

/// <summary>
/// Proporciona métodos estáticos para la validación de datos utilizando anotaciones.
/// </summary>
public static class DataAnnotationValidation {
    /// <summary>
    /// Valida un objeto dado utilizando el sistema de validación de datos.
    /// </summary>
    /// <param name="target">El objeto que se va a validar. No puede ser nulo.</param>
    /// <returns>
    /// Una colección de resultados de validación que contiene los errores de validación encontrados.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="target"/> es nulo.</exception>
    /// <remarks>
    /// Este método utiliza el <see cref="Validator"/> para intentar validar el objeto especificado.
    /// Si el objeto no es válido, los resultados de la validación se añaden a la colección de resultados.
    /// </remarks>
    public static ValidationResultCollection Validate( object target ) {
        if( target == null )
            throw new ArgumentNullException( nameof( target ) );
        var result = new ValidationResultCollection();
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext( target, null, null );
        var isValid = Validator.TryValidateObject( target, context, validationResults, true );
        if ( !isValid )
            result.AddList( validationResults );
        return result;
    }
}