namespace Util.Localization; 

/// <summary>
/// Representa las opciones de localización para la carga de archivos JSON.
/// Esta clase hereda de <see cref="LocalizationOptions"/> y permite configurar
/// la localización basada en archivos JSON.
/// </summary>
public class JsonLocalizationOptions : LocalizationOptions {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="JsonLocalizationOptions"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para configurar las opciones de localización basadas en archivos JSON.
    /// Por defecto, la ruta de los recursos se establece en "Resources".
    /// </remarks>
    public JsonLocalizationOptions() {
        ResourcesPath = "Resources";
    }

    /// <summary>
    /// Obtiene o establece la ruta de los recursos.
    /// </summary>
    /// <value>
    /// La ruta de los recursos como una cadena de texto.
    /// </value>
    public string ResourcesPath { get; set; }
}