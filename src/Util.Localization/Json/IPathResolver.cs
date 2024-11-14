namespace Util.Localization.Json; 

/// <summary>
/// Define un contrato para resolver rutas en un sistema.
/// </summary>
public interface IPathResolver {
    /// <summary>
    /// Obtiene el espacio de nombres raíz de un ensamblado especificado.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se desea obtener el espacio de nombres raíz.</param>
    /// <returns>El espacio de nombres raíz del ensamblado.</returns>
    /// <remarks>
    /// Este método analiza el ensamblado proporcionado y devuelve el espacio de nombres que se considera
    /// como el raíz, que generalmente es el primer espacio de nombres definido en el ensamblado.
    /// </remarks>
    /// <seealso cref="Assembly"/>
    string GetRootNamespace( Assembly assembly );
    /// <summary>
    /// Obtiene la ruta raíz de los recursos para un ensamblado específico.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se desea obtener la ruta de recursos.</param>
    /// <param name="rootPath">La ruta raíz desde la cual se calculará la ruta de los recursos.</param>
    /// <returns>Una cadena que representa la ruta raíz de los recursos del ensamblado.</returns>
    /// <remarks>
    /// Este método es útil para localizar recursos embebidos o archivos relacionados con el ensamblado
    /// en un directorio específico. Asegúrese de que el ensamblado proporcionado no sea nulo.
    /// </remarks>
    /// <seealso cref="Assembly"/>
    string GetResourcesRootPath( Assembly assembly,string rootPath );
    /// <summary>
    /// Obtiene el nombre base de los recursos de un ensamblado dado y el nombre completo de un tipo.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se desean obtener los recursos.</param>
    /// <param name="typeFullName">El nombre completo del tipo para el cual se busca el nombre base de los recursos.</param>
    /// <returns>El nombre base de los recursos asociado al tipo especificado en el ensamblado dado.</returns>
    /// <remarks>
    /// Este método es útil para localizar recursos específicos que están relacionados con un tipo particular
    /// dentro de un ensamblado, facilitando la gestión de recursos en aplicaciones que utilizan múltiples
    /// tipos y recursos.
    /// </remarks>
    /// <seealso cref="Assembly"/>
    /// <seealso cref="Type"/>
    string GetResourcesBaseName( Assembly assembly,string typeFullName );
    /// <summary>
    /// Obtiene la ruta del recurso JSON basado en la ruta raíz, el nombre base y la cultura especificada.
    /// </summary>
    /// <param name="rootPath">La ruta raíz donde se encuentran los recursos JSON.</param>
    /// <param name="baseName">El nombre base del recurso JSON sin la extensión.</param>
    /// <param name="culture">La cultura que se utilizará para localizar el recurso.</param>
    /// <returns>La ruta completa al recurso JSON correspondiente.</returns>
    /// <remarks>
    /// Este método combina la ruta raíz con el nombre base y la cultura para formar la ruta final.
    /// Se asume que los recursos JSON están organizados en carpetas que corresponden a las culturas.
    /// </remarks>
    /// <seealso cref="System.Globalization.CultureInfo"/>
    string GetJsonResourcePath( string rootPath, string baseName,CultureInfo culture );
}