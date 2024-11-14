namespace Util.Localization; 

/// <summary>
/// Representa las opciones de localización para una aplicación.
/// </summary>
public class LocalizationOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LocalizationOptions"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea una lista vacía de culturas que se utilizarán para la localización.
    /// </remarks>
    public LocalizationOptions() {
        Cultures = new List<string>();
    }

    /// <summary>
    /// Obtiene o establece una lista de cadenas que representan las culturas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad puede ser utilizada para almacenar y gestionar múltiples culturas,
    /// permitiendo su uso en diferentes contextos dentro de la aplicación.
    /// </remarks>
    /// <value>
    /// Una lista de cadenas que contiene los códigos de las culturas.
    /// </value>
    public IList<string> Cultures { get; set; }

    /// <summary>
    /// Obtiene o establece el tiempo de expiración en segundos.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado es 28800 segundos, que equivale a 8 horas.
    /// </remarks>
    /// <value>
    /// Un entero que representa el tiempo de expiración en segundos.
    /// </value>
    public int Expiration { get; set; } = 28800;

    /// <summary>
    /// Obtiene o establece un valor que indica si se debe mostrar una advertencia de localización.
    /// </summary>
    /// <value>
    /// <c>true</c> si se debe mostrar la advertencia de localización; de lo contrario, <c>false</c>.
    /// </value>
    public bool IsLocalizeWarning { get; set; }
}