namespace Util; 

/// <summary>
/// Representa las opciones de configuración para el manejo de JSON.
/// </summary>
public class JsonOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JsonOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor establece las opciones predeterminadas para la serialización JSON.
    /// Las propiedades se configuran para ignorar valores nulos, cadenas vacías, 
    /// diferencias entre mayúsculas y minúsculas, y interfaces durante la serialización.
    /// </remarks>
    public JsonOptions() {
        IgnoreNullValues = true;
        IgnoreEmptyString = true;
        IgnoreCase = true;
        IgnoreInterface = true;
    }

    /// <summary>
    /// Obtiene o establece un valor que indica si se deben eliminar las comillas de una cadena.
    /// </summary>
    /// <value>
    /// <c>true</c> si se deben eliminar las comillas; de lo contrario, <c>false</c>.
    /// </value>
    public bool RemoveQuotationMarks { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se deben utilizar comillas simples.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite al usuario elegir entre usar comillas simples o no en la representación de cadenas.
    /// </remarks>
    /// <value>
    /// <c>true</c> si se deben utilizar comillas simples; de lo contrario, <c>false</c>.
    /// </value>
    public bool ToSingleQuotes { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se deben ignorar los valores nulos.
    /// </summary>
    /// <value>
    /// <c>true</c> si se deben ignorar los valores nulos; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Esta propiedad se utiliza para controlar el comportamiento de la serialización o procesamiento de datos,
    /// permitiendo omitir los campos que tienen un valor nulo.
    /// </remarks>
    public bool IgnoreNullValues { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se deben ignorar las cadenas vacías.
    /// </summary>
    /// <value>
    /// <c>true</c> si se deben ignorar las cadenas vacías; de lo contrario, <c>false</c>.
    /// </value>
    public bool IgnoreEmptyString { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si el texto debe ser convertido a formato Camel Case.
    /// </summary>
    /// <remarks>
    /// El formato Camel Case capitaliza la primera letra de cada palabra, excepto la primera.
    /// Esta propiedad es útil para formatear cadenas de texto de manera que sean más legibles 
    /// o para cumplir con ciertos estándares de codificación.
    /// </remarks>
    /// <value>
    /// <c>true</c> si el texto debe ser convertido a Camel Case; de lo contrario, <c>false</c>.
    /// </value>
    public bool ToCamelCase { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar la distinción entre mayúsculas y minúsculas.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe ignorar la distinción entre mayúsculas y minúsculas; de lo contrario, <c>false</c>.
    /// </value>
    public bool IgnoreCase { get; set; }
    /// <summary>
    /// Obtiene o establece un valor que indica si se debe ignorar la interfaz.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe ignorar la interfaz; de lo contrario, <c>false</c>.
    /// </value>
    public bool IgnoreInterface { get; set; }
}