namespace Util.FileStorage.Local;

/// <summary>
/// Representa las opciones de configuración para el almacenamiento local.
/// </summary>
public class LocalStoreOptions {
    /// <summary>
    /// Obtiene o establece la ruta raíz donde se almacenan los archivos subidos.
    /// </summary>
    /// <remarks>
    /// El valor predeterminado de esta propiedad es "upload".
    /// </remarks>
    public string RootPath { get; set; } = "upload";
}