using Util.Helpers;

namespace Util.Localization.Json; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IPathResolver"/> 
/// para resolver rutas de archivos y directorios.
/// </summary>
public class PathResolver : IPathResolver {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene el espacio de nombres raíz de un ensamblado especificado.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se desea obtener el espacio de nombres raíz.</param>
    /// <returns>
    /// El espacio de nombres raíz del ensamblado. Si no se encuentra el atributo <see cref="RootNamespaceAttribute"/> en el ensamblado,
    /// se devuelve el nombre del ensamblado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="assembly"/> es nulo.</exception>
    /// <remarks>
    /// Este método verifica si el ensamblado proporcionado es nulo y lanza una excepción si es el caso. 
    /// Luego, intenta obtener el atributo <see cref="RootNamespaceAttribute"/> del ensamblado. 
    /// Si el atributo no está presente, se devuelve el nombre del ensamblado como espacio de nombres raíz.
    /// </remarks>
    /// <seealso cref="RootNamespaceAttribute"/>
    public string GetRootNamespace( Assembly assembly ) {
        assembly.CheckNull( nameof(assembly) );
        var attribute = assembly.GetCustomAttribute<RootNamespaceAttribute>();
        return attribute == null ? assembly.GetName().Name : attribute.RootNamespace;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la ruta raíz de los recursos a partir de un ensamblado y una ruta raíz proporcionada.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se extraerá la ubicación de los recursos. Puede ser nulo.</param>
    /// <param name="rootPath">La ruta raíz que se utilizará si no se encuentra una ubicación de recursos en el ensamblado.</param>
    /// <returns>
    /// La ruta de ubicación de los recursos si se encuentra en el ensamblado; de lo contrario, devuelve la ruta raíz proporcionada.
    /// </returns>
    /// <remarks>
    /// Este método busca un atributo de tipo <see cref="ResourceLocationAttribute"/> en el ensamblado especificado.
    /// Si el atributo está presente, se devuelve su valor; de lo contrario, se devuelve el <paramref name="rootPath"/>.
    /// </remarks>
    public string GetResourcesRootPath( Assembly assembly, string rootPath ) {
        if ( assembly == null )
            return rootPath;
        var attribute = assembly.GetCustomAttribute<ResourceLocationAttribute>();
        return attribute == null ? rootPath : attribute.ResourceLocation;
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el nombre base de los recursos a partir de un ensamblado y el nombre completo de un tipo.
    /// </summary>
    /// <param name="assembly">El ensamblado del cual se extraerá el espacio de nombres raíz.</param>
    /// <param name="typeFullName">El nombre completo del tipo del cual se desea obtener el nombre base de los recursos.</param>
    /// <returns>
    /// Un string que representa el nombre base de los recursos, sin el espacio de nombres raíz.
    /// </returns>
    /// <remarks>
    /// Este método elimina el espacio de nombres raíz del nombre completo del tipo para obtener el nombre base de los recursos.
    /// </remarks>
    /// <seealso cref="GetRootNamespace(Assembly)"/>
    public string GetResourcesBaseName( Assembly assembly, string typeFullName ) {
        var rootNamespace = GetRootNamespace( assembly );
        return typeFullName.RemoveStart( $"{rootNamespace}." );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene la ruta del recurso JSON basado en el nombre base y la cultura especificada.
    /// </summary>
    /// <param name="rootPath">La ruta raíz donde se encuentra el recurso.</param>
    /// <param name="baseName">El nombre base del recurso. Si está vacío, se usará solo la cultura.</param>
    /// <param name="culture">La cultura que se utilizará para determinar el nombre del archivo JSON.</param>
    /// <returns>
    /// La ruta completa al archivo JSON correspondiente.
    /// </returns>
    /// <remarks>
    /// Si el <paramref name="baseName"/> está vacío, se generará una ruta que solo incluye la cultura.
    /// De lo contrario, se generará una ruta que incluye tanto el <paramref name="baseName"/> como la cultura.
    /// </remarks>
    /// <seealso cref="FixInnerClassPath(string)"/>
    public string GetJsonResourcePath( string rootPath, string baseName, CultureInfo culture ) {
        if( baseName.IsEmpty() )
            return Path.Combine( Common.ApplicationBaseDirectory, rootPath, $"{culture.Name}.json" );
        baseName = FixInnerClassPath( baseName );
        return Path.Combine( Common.ApplicationBaseDirectory, rootPath, $"{baseName}.{culture.Name}.json" );
    }

    /// <summary>
    /// Reemplaza el separador de clase interna en la ruta especificada por un punto.
    /// </summary>
    /// <param name="path">La ruta de la clase que puede contener un separador de clase interna.</param>
    /// <returns>
    /// La ruta con el separador de clase interna reemplazado por un punto si estaba presente; de lo contrario, la ruta original.
    /// </returns>
    private string FixInnerClassPath( string path ) {
        const char innerClassSeparator = '+';
        return path.Contains( innerClassSeparator ) ? path.Replace( innerClassSeparator, '.' ) : path;
    }
}