namespace Util.Templates.Razor; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="TemplateEngine"/>.
/// </summary>
public static class TemplateEngineExtensions {
    /// <summary>
    /// Limpia la caché de plantillas del motor de plantillas especificado.
    /// </summary>
    /// <param name="templateEngine">El motor de plantillas del cual se desea limpiar la caché.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    public static void ClearTemplateCache( this ITemplateEngine templateEngine ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        RazorTemplateEngine.ClearTemplateCache();
    }

    /// <summary>
    /// Renderiza una plantilla utilizando el motor de plantillas especificado.
    /// </summary>
    /// <param name="templateEngine">El motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="template">La plantilla que se desea renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar la plantilla.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <returns>
    /// Una cadena que representa el resultado de la renderización de la plantilla, o null si el motor de plantillas no es del tipo esperado.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad del motor de plantillas, permitiendo una renderización más flexible y configurada.
    /// </remarks>
    public static string Render( this ITemplateEngine templateEngine, string template, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        if( !( templateEngine is RazorTemplateEngine engine ) )
            return null;
        return engine.Render( template, data, builderAction );
    }

    /// <summary>
    /// Renderiza un template de forma asíncrona utilizando un motor de plantillas Razor.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar el template.</param>
    /// <param name="template">El template que se desea renderizar.</param>
    /// <param name="data">Los datos que se pasarán al template para su renderización.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor Razor.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de renderización, que contiene el resultado como una cadena.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="templateEngine"/> es null.
    /// </exception>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="ITemplateEngine"/> al proporcionar una implementación específica para el motor Razor.
    /// </remarks>
    /// <seealso cref="ITemplateEngine"/>
    /// <seealso cref="RazorTemplateEngine"/>
    public static async Task<string> RenderAsync( this ITemplateEngine templateEngine, string template, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        if( !( templateEngine is RazorTemplateEngine engine ) )
            return null;
        return await engine.RenderAsync( template, data, builderAction );
    }

    /// <summary>
    /// Renderiza una plantilla a partir de la ruta especificada utilizando el motor de plantillas proporcionado.
    /// </summary>
    /// <param name="templateEngine">El motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo de plantilla que se desea renderizar.</param>
    /// <param name="data">Los datos que se pasarán a la plantilla durante el proceso de renderizado.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <returns>
    /// Una cadena que representa el resultado de la plantilla renderizada.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si <paramref name="templateEngine"/> es <c>null</c>.
    /// </exception>
    /// <remarks>
    /// Este método lee el contenido del archivo de plantilla especificado en <paramref name="filePath"/>
    /// y lo renderiza utilizando el motor de plantillas proporcionado y los datos especificados.
    /// </remarks>
    public static string RenderByPath( this ITemplateEngine templateEngine, string filePath, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var template = Util.Helpers.File.ReadToString( filePath );
        return templateEngine.Render( template, data, builderAction );
    }

    /// <summary>
    /// Renderiza una plantilla a partir de un archivo especificado y datos proporcionados.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar.</param>
    /// <param name="filePath">La ruta del archivo de plantilla que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar la plantilla.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El valor de retorno contiene el resultado de la renderización de la plantilla como una cadena.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método lee el contenido del archivo de plantilla de forma asincrónica y lo renderiza utilizando el motor de plantillas especificado.
    /// </remarks>
    public static async Task<string> RenderByPathAsync( this ITemplateEngine templateEngine, string filePath, object data, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var template = await Util.Helpers.File.ReadToStringAsync( filePath );
        return await templateEngine.RenderAsync( template, data, builderAction );
    }

    /// <summary>
    /// Guarda el resultado de la renderización de una plantilla en un archivo especificado.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="template">La plantilla que se desea renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado de la renderización.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad del motor de plantillas, permitiendo guardar el resultado de la renderización directamente en un archivo.
    /// Asegúrese de que la ruta del archivo sea válida y que tenga permisos de escritura.
    /// </remarks>
    public static void Save( this ITemplateEngine templateEngine, string template, object data, string filePath, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = templateEngine.Render( template, data, builderAction );
        Util.Helpers.File.Write( filePath, result );
    }

    /// <summary>
    /// Guarda de manera asíncrona el resultado de la renderización de una plantilla utilizando un motor de plantillas.
    /// </summary>
    /// <param name="templateEngine">El motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="template">La plantilla que se desea renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para la renderización de la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado de la renderización.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <returns>Una tarea que representa la operación asíncrona de guardar el archivo.</returns>
    /// <remarks>
    /// Este método permite renderizar una plantilla con los datos proporcionados y guardar el resultado en un archivo especificado.
    /// </remarks>
    public static async Task SaveAsync( this ITemplateEngine templateEngine, string template, object data, string filePath, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = await templateEngine.RenderAsync( template, data, builderAction );
        await Util.Helpers.File.WriteAsync( filePath, result );
    }

    /// <summary>
    /// Guarda el resultado de la renderización de una plantilla en un archivo especificado.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="templatePath">La ruta de la plantilla que se desea renderizar.</param>
    /// <param name="data">El objeto que contiene los datos que se pasarán a la plantilla durante la renderización.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado de la renderización.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método utiliza el motor de plantillas para renderizar el contenido de la plantilla especificada
    /// y luego guarda el resultado en el archivo indicado. Asegúrese de que la ruta del archivo de destino
    /// sea válida y que tenga permisos de escritura.
    /// </remarks>
    public static void SaveByPath( this ITemplateEngine templateEngine, string templatePath, object data, string filePath, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = templateEngine.RenderByPath( templatePath, data, builderAction );
        Util.Helpers.File.Write( filePath, result );
    }

    /// <summary>
    /// Guarda el resultado de la renderización de una plantilla en un archivo especificado.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="templatePath">La ruta de la plantilla que se desea renderizar.</param>
    /// <param name="data">El objeto que contiene los datos que se pasarán a la plantilla durante la renderización.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado de la renderización.</param>
    /// <param name="builderAction">Una acción que permite configurar las opciones de compilación del motor de plantillas.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <returns>Una tarea que representa la operación asincrónica de guardar el archivo.</returns>
    /// <remarks>
    /// Este método es útil para generar archivos a partir de plantillas dinámicas, permitiendo la personalización de la salida
    /// según los datos proporcionados. Asegúrese de que la ruta de la plantilla y la ruta del archivo sean válidas.
    /// </remarks>
    public static async Task SaveByPathAsync( this ITemplateEngine templateEngine, string templatePath, object data, string filePath, Action<IRazorEngineCompilationOptionsBuilder> builderAction ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = await templateEngine.RenderByPathAsync( templatePath, data, builderAction );
        await Util.Helpers.File.WriteAsync( filePath, result );
    }
}