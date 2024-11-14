namespace Util.Templates.Razor; 

/// <summary>
/// Representa un motor de plantillas Razor que implementa la interfaz <see cref="IRazorTemplateEngine"/>.
/// </summary>
public class RazorTemplateEngine : IRazorTemplateEngine {
    private static readonly ConcurrentDictionary<string, ITemplateFilter> _filters = new();
    private static readonly ConcurrentDictionary<string, Assembly> _assemblies = new();
    protected static readonly ConcurrentDictionary<int, IRazorEngineCompiledTemplate> TemplateCache = new();
    private static bool _isAutoLoadAssemblies;
    protected readonly IRazorEngine RazorEngine;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RazorTemplateEngine"/>.
    /// </summary>
    /// <param name="razorEngine">La instancia del motor Razor que se utilizará para procesar las plantillas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="razorEngine"/> es <c>null</c>.</exception>
    public RazorTemplateEngine( IRazorEngine razorEngine ) {
        RazorEngine = razorEngine ?? throw new ArgumentNullException( nameof( razorEngine ) );
        _isAutoLoadAssemblies = true;
    }

    /// <summary>
    /// Limpia la caché de plantillas.
    /// </summary>
    /// <remarks>
    /// Este método está diseñado para eliminar todas las entradas almacenadas en la caché de plantillas,
    /// lo que puede ser útil para liberar memoria o asegurar que se utilicen las versiones más recientes
    /// de las plantillas.
    /// </remarks>
    public static void ClearTemplateCache() {
        TemplateCache.Clear();
    }

    /// <summary>
    /// Agrega un filtro al conjunto de filtros existentes.
    /// </summary>
    /// <param name="filter">El filtro que se desea agregar. No puede ser nulo.</param>
    /// <remarks>
    /// Si el filtro proporcionado es nulo, la operación se ignora y no se realiza ninguna acción.
    /// Si el filtro ya existe, se puede sobrescribir dependiendo de la implementación de <see cref="ConcurrentDictionary{TKey, TValue}"/>.
    /// </remarks>
    /// <seealso cref="ITemplateFilter"/>
    public static void AddFilter( ITemplateFilter filter ) {
        if ( filter == null )
            return;
        _filters.TryAdd( filter.GetType().FullName, filter );
    }

    /// <summary>
    /// Limpia todos los filtros aplicados.
    /// </summary>
    /// <remarks>
    /// Este método elimina todos los elementos de la colección de filtros, 
    /// restableciendo el estado de los filtros a su valor inicial.
    /// </remarks>
    public static void ClearFilters() {
        _filters.Clear();
    }

    /// <summary>
    /// Agrega una referencia de ensamblado a la colección de ensamblados.
    /// </summary>
    /// <param name="assembly">El ensamblado que se va a agregar.</param>
    /// <remarks>
    /// Este método utiliza un diccionario concurrente para almacenar las referencias de ensamblados, 
    /// asegurando que las operaciones de adición sean seguras en entornos multihilo.
    /// </remarks>
    public static void AddAssemblyReference( Assembly assembly ) {
        _assemblies.TryAdd( assembly.FullName, assembly );
    }

    /// <summary>
    /// Agrega una referencia a un ensamblado especificado por su nombre.
    /// </summary>
    /// <param name="assembly">El nombre del ensamblado que se desea cargar y agregar como referencia.</param>
    /// <remarks>
    /// Este método carga el ensamblado utilizando el método <see cref="Assembly.Load(string)"/> 
    /// y luego llama a otro método para agregar la referencia del ensamblado cargado.
    /// </remarks>
    /// <seealso cref="Assembly.Load(string)"/>
    public static void AddAssemblyReference( string assembly ) {
        AddAssemblyReference( Assembly.Load( assembly ) );
    }

    /// <summary>
    /// Desactiva la carga automática de ensamblados.
    /// </summary>
    /// <remarks>
    /// Este método establece la variable interna que controla la carga automática de ensamblados en falso,
    /// lo que significa que los ensamblados no se cargarán automáticamente en el contexto actual.
    /// </remarks>
    public static void DisableAutoLoadAssemblies() {
        _isAutoLoadAssemblies = false;
    }

    /// <summary>
    /// Renderiza una plantilla utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">La plantilla que se va a renderizar.</param>
    /// <param name="data">Un objeto que contiene los datos que se utilizarán en la plantilla. Puede ser nulo.</param>
    /// <returns>Una cadena que representa el resultado de la plantilla renderizada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que llama a otro método de renderizado con un tercer parámetro nulo.
    /// </remarks>
    public string Render( string template, object data = null ) {
        return Render( template, data, null );
    }

    /// <summary>
    /// Renderiza una plantilla utilizando los datos proporcionados y opciones de compilación.
    /// </summary>
    /// <param name="template">La plantilla que se va a renderizar. No puede ser nula o estar vacía.</param>
    /// <param name="data">El objeto que contiene los datos que se utilizarán en la plantilla.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor Razor.</param>
    /// <returns>Una cadena que representa el resultado de la renderización de la plantilla, o null si la plantilla es nula o vacía.</returns>
    /// <remarks>
    /// Este método busca en la caché una plantilla compilada. Si no se encuentra, la plantilla se compila y se almacena en caché.
    /// </remarks>
    public string Render( string template, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if ( string.IsNullOrWhiteSpace( template ) )
            return null;
        var compiledTemplate = GetCompiledTemplateFromCache( template, builderAction );
        return compiledTemplate?.Run( data );
    }

    /// <summary>
    /// Obtiene una plantilla compilada desde la caché, o la compila si no está disponible.
    /// </summary>
    /// <param name="template">La cadena que representa la plantilla que se desea compilar.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación de la plantilla.</param>
    /// <returns>
    /// Una instancia de <see cref="IRazorEngineCompiledTemplate"/> que representa la plantilla compilada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un mecanismo de caché para almacenar las plantillas compiladas, 
    /// lo que mejora el rendimiento al evitar recompilaciones innecesarias.
    /// Si la plantilla ya está en caché, se devuelve directamente; 
    /// de lo contrario, se filtra y se compila la plantilla antes de almacenarla en la caché.
    /// </remarks>
    private IRazorEngineCompiledTemplate GetCompiledTemplateFromCache( string template, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        return TemplateCache.GetOrAdd( template.GetHashCode(), t => {
            template = FilterTemplate( template );
            return GetCompiledTemplate( template, builderAction );
        } );
    }

    /// <summary>
    /// Filtra una plantilla de texto utilizando una serie de filtros predefinidos.
    /// </summary>
    /// <param name="template">La plantilla de texto que se desea filtrar.</param>
    /// <returns>
    /// La plantilla de texto filtrada, después de aplicar todos los filtros.
    /// </returns>
    private string FilterTemplate( string template ) {
        foreach ( var filter in _filters.Values )
            template = filter.Filter( template );
        return template;
    }

    /// <summary>
    /// Compila una plantilla Razor utilizando el motor Razor especificado.
    /// </summary>
    /// <param name="template">La cadena que representa la plantilla Razor a compilar.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor Razor.</param>
    /// <returns>
    /// Un objeto que representa la plantilla compilada.
    /// </returns>
    /// <remarks>
    /// Este método permite personalizar la compilación de la plantilla mediante el uso de un delegado que recibe un 
    /// <see cref="IRazorEngineCompilationOptionsBuilder"/>. Se pueden cargar ensamblados adicionales o configurar 
    /// otras opciones según sea necesario.
    /// </remarks>
    /// <seealso cref="IRazorEngineCompiledTemplate"/>
    /// <seealso cref="IRazorEngineCompilationOptionsBuilder"/>
    private IRazorEngineCompiledTemplate GetCompiledTemplate( string template, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        return RazorEngine.Compile( template, builder => {
            LoadAssemblies( builder );
            builderAction?.Invoke( builder );
        } );
    }

    /// <summary>
    /// Carga las ensambladuras en el compilador de Razor si la opción de carga automática está habilitada.
    /// </summary>
    /// <param name="builder">El constructor de opciones de compilación de Razor que se utilizará para agregar referencias de ensamblado.</param>
    /// <remarks>
    /// Este método verifica si la carga automática de ensambladuras está habilitada mediante la variable 
    /// <c>_isAutoLoadAssemblies</c>. Si no está habilitada, el método termina sin realizar ninguna acción.
    /// Si está habilitada, se itera sobre las ensambladuras almacenadas en <c>_assemblies</c> y se agregan 
    /// a las referencias del compilador.
    /// </remarks>
    private void LoadAssemblies( IRazorEngineCompilationOptionsBuilder builder ) {
        if ( _isAutoLoadAssemblies == false )
            return;
        foreach ( var assembly in _assemblies.Values ) {
            builder.AddAssemblyReference( assembly );
        }
    }

    /// <summary>
    /// Renderiza un template de manera asíncrona utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">El template que se va a renderizar.</param>
    /// <param name="data">Un objeto que contiene los datos que se utilizarán en el template. Puede ser nulo.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es una cadena que contiene el resultado del renderizado.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que permite llamar a <see cref="RenderAsync(string, object, object)"/> sin proporcionar un tercer parámetro.
    /// </remarks>
    public virtual async Task<string> RenderAsync( string template, object data = null ) {
        return await RenderAsync( template, data, null );
    }

    /// <summary>
    /// Renderiza un template de Razor de manera asíncrona utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">El template de Razor que se desea renderizar.</param>
    /// <param name="data">El objeto que contiene los datos que se utilizarán en el template.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de Razor.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una cadena que contiene el resultado del rendering del template, o null si el template es nulo o vacío, o si no se pudo compilar el template.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un caché para almacenar templates compilados, lo que puede mejorar el rendimiento al evitar recompilaciones innecesarias.
    /// </remarks>
    /// <seealso cref="IRazorEngineCompilationOptionsBuilder"/>
    public async Task<string> RenderAsync( string template, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if ( string.IsNullOrWhiteSpace( template ) )
            return null;
        var compiledTemplate = GetCompiledTemplateFromCache( template, builderAction );
        if ( compiledTemplate == null )
            return null;
        return await compiledTemplate.RunAsync( data );
    }
}