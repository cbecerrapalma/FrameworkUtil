using Util.Domain.Properties;

namespace Util.Domain.Compare; 

/// <summary>
/// Representa una clase que permite cambiar el valor de una variable.
/// </summary>
public class ChangeValue {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ChangeValue"/>.
    /// </summary>
    /// <param name="propertyName">El nombre de la propiedad que ha cambiado.</param>
    /// <param name="description">Una descripción del cambio realizado.</param>
    /// <param name="originalValue">El valor original de la propiedad antes del cambio.</param>
    /// <param name="newValue">El nuevo valor de la propiedad después del cambio.</param>
    public ChangeValue( string propertyName, string description, string originalValue, string newValue ) {
        PropertyName = propertyName;
        Description = description;
        OriginalValue = originalValue;
        NewValue = newValue;
    }

    /// <summary>
    /// Obtiene el nombre de la propiedad.
    /// </summary>
    /// <value>
    /// Un string que representa el nombre de la propiedad.
    /// </value>
    public string PropertyName { get; }
    /// <summary>
    /// Obtiene la descripción del objeto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve una cadena que representa la descripción.
    /// </remarks>
    /// <value>
    /// Una cadena que contiene la descripción.
    /// </value>
    public string Description { get; }
    /// <summary>
    /// Obtiene el valor original.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y proporciona acceso al valor original almacenado.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el valor original.
    /// </value>
    public string OriginalValue { get; }
    /// <summary>
    /// Obtiene el nuevo valor.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve el valor actual.
    /// </remarks>
    /// <returns>El nuevo valor como una cadena.</returns>
    public string NewValue { get; }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el objeto actual, formateada con el nombre de la propiedad, 
    /// la descripción, el valor original y el nuevo valor.
    /// </returns>
    public override string ToString() {
        return string.Format( DomainResource.ChangeValueToString, PropertyName, Description, OriginalValue, NewValue );
    }
}