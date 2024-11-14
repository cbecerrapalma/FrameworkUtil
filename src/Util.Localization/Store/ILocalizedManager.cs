namespace Util.Localization.Store;

/// <summary>
/// Define un contrato para la gestión de localización.
/// </summary>
public interface ILocalizedManager {
    /// <summary>
    /// Carga todos los recursos necesarios para la aplicación.
    /// </summary>
    /// <remarks>
    /// Este método se encarga de inicializar y cargar todos los recursos 
    /// que la aplicación necesita para funcionar correctamente. Esto puede incluir 
    /// archivos de configuración, imágenes, sonidos, y otros recursos necesarios.
    /// </remarks>
    void LoadAllResources();
    /// <summary>
    /// Carga todos los recursos para la cultura especificada.
    /// </summary>
    /// <param name="culture">La cadena que representa la cultura para la cual se cargarán los recursos.</param>
    /// <remarks>
    /// Este método se utiliza para inicializar y cargar todos los recursos necesarios 
    /// para la aplicación en función de la cultura proporcionada. Puede incluir 
    /// archivos de configuración, imágenes, textos y otros recursos específicos 
    /// de la cultura.
    /// </remarks>
    /// <seealso cref="UnloadResources"/>
    void LoadAllResources( string culture );
    /// <summary>
    /// Carga todos los recursos de un tipo específico para una cultura dada.
    /// </summary>
    /// <param name="culture">La cultura para la cual se cargarán los recursos, especificada en formato de cadena (por ejemplo, "es-ES").</param>
    /// <param name="type">El tipo de recursos que se deben cargar, especificado en formato de cadena (por ejemplo, "imagenes" o "textos").</param>
    /// <remarks>
    /// Este método es útil para inicializar los recursos necesarios en una aplicación multilingüe,
    /// permitiendo que la interfaz de usuario se adapte a diferentes culturas y tipos de contenido.
    /// Asegúrese de que los recursos estén disponibles en la ubicación esperada antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="UnloadResources(string, string)"/>
    void LoadAllResources( string culture, string type );
    /// <summary>
    /// Carga los recursos según el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo de recurso que se desea cargar.</param>
    /// <remarks>
    /// Este método se encarga de buscar y cargar todos los recursos que coincidan con el tipo
    /// proporcionado. Asegúrese de que el tipo sea válido para evitar errores durante la carga.
    /// </remarks>
    /// <seealso cref="UnloadResourcesByType"/>
    void LoadResourcesByType( string type );
}