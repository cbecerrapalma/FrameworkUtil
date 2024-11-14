namespace Util.Reflections; 

/// <summary>
/// Proporciona una implementación de <see cref="IAssemblyFinder"/> 
/// que busca ensamblados en el dominio de aplicación actual.
/// </summary>
public class AppDomainAssemblyFinder : IAssemblyFinder {
    /// <summary>
    /// Obtiene o establece el patrón de omisión de ensamblados.
    /// </summary>
    /// <remarks>
    /// Este patrón se utiliza para especificar qué ensamblados deben ser ignorados
    /// durante ciertas operaciones, como la carga de tipos o la reflexión.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el patrón de omisión de ensamblados.
    /// </value>
    public string AssemblySkipPattern { get; set; }
    private List<Assembly> _assemblies;

    /// <summary>
    /// Busca y carga una lista de ensamblados en el dominio de aplicación actual.
    /// </summary>
    /// <returns>
    /// Una lista de objetos <see cref="Assembly"/> que representan los ensamblados cargados.
    /// </returns>
    /// <remarks>
    /// Si la lista de ensamblados ya ha sido cargada, se devuelve la lista existente.
    /// De lo contrario, se cargan los ensamblados del dominio actual, omitiendo aquellos que cumplen con el criterio de exclusión.
    /// </remarks>
    public List<Assembly> Find() {
        if ( _assemblies != null )
            return _assemblies;
        _assemblies = new List<Assembly>();
        LoadAssemblies();
        foreach( var assembly in AppDomain.CurrentDomain.GetAssemblies() ) {
            if( IsSkip( assembly ) )
                continue;
            _assemblies.Add( assembly );
        }
        return _assemblies;
    }

    /// <summary>
    /// Carga las ensamblados en el dominio de aplicación actual.
    /// </summary>
    /// <remarks>
    /// Este método itera a través de los archivos de ensamblado obtenidos por el método 
    /// <see cref="GetLoadAssemblyFiles"/> y los carga en el dominio de aplicación.
    /// </remarks>
    protected virtual void LoadAssemblies() {
        var currentDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach( string file in GetLoadAssemblyFiles() )
            LoadAssembly( file, currentDomainAssemblies );
    }

    /// <summary>
    /// Obtiene una lista de archivos de ensamblado (.dll) en el directorio base de la aplicación.
    /// </summary>
    /// <returns>
    /// Un arreglo de cadenas que contiene las rutas de los archivos de ensamblado encontrados.
    /// </returns>
    protected virtual string[] GetLoadAssemblyFiles() {
        return Directory.GetFiles(AppContext.BaseDirectory, "*.dll");
    }

    /// <summary>
    /// Carga un ensamblado en el dominio de aplicación actual si no ha sido cargado previamente.
    /// </summary>
    /// <param name="file">La ruta al archivo del ensamblado que se desea cargar.</param>
    /// <param name="currentDomainAssemblies">Un arreglo de ensamblados que ya están cargados en el dominio actual.</param>
    /// <remarks>
    /// Este método intenta cargar un ensamblado a partir de su ruta especificada. Si el ensamblado ya ha sido cargado o si su nombre está en la lista de nombres a omitir, el método no realizará ninguna acción.
    /// </remarks>
    /// <exception cref="BadImageFormatException">
    /// Se lanza si el archivo especificado no es un ensamblado válido.
    /// </exception>
    protected void LoadAssembly( string file, Assembly[] currentDomainAssemblies ) {
        try {
            var assemblyName = AssemblyName.GetAssemblyName( file );
            if( IsSkip( assemblyName.Name ) )
                return;
            if( currentDomainAssemblies.Any( t => t.FullName == assemblyName.FullName ) )
                return;
            AppDomain.CurrentDomain.Load( assemblyName );
        }
        catch( BadImageFormatException ) {
        }
    }

    /// <summary>
    /// Determina si un ensamblado debe ser omitido basado en su nombre.
    /// </summary>
    /// <param name="assemblyName">El nombre del ensamblado que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el ensamblado debe ser omitido; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el nombre del ensamblado comienza con el nombre de la aplicación seguido de 
    /// ".Views" o ".PrecompiledViews". Si no se proporciona un patrón de omisión de ensamblados, 
    /// se devuelve <c>false</c>. Si se proporciona un patrón, se utiliza una expresión regular para determinar
    /// si el ensamblado debe ser omitido.
    /// </remarks>
    protected bool IsSkip(string assemblyName) { 
        var applicationName = Assembly.GetEntryAssembly()?.GetName().Name; 
        if (assemblyName.StartsWith($"{applicationName}.Views")) 
            return true; 
        if (assemblyName.StartsWith($"{applicationName}.PrecompiledViews")) 
            return true; 
        if (string.IsNullOrWhiteSpace(AssemblySkipPattern)) 
            return false; 
        return Regex.IsMatch(assemblyName, AssemblySkipPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled); 
    }

    /// <summary>
    /// Determina si un ensamblado debe ser omitido basado en su nombre completo.
    /// </summary>
    /// <param name="assembly">El ensamblado que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el ensamblado debe ser omitido; de lo contrario, <c>false</c>.
    /// </returns>
    private bool IsSkip( Assembly assembly ) {
        return IsSkip( assembly.FullName );
    }
}