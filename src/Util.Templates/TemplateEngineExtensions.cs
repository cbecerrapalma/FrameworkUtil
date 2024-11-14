namespace Util.Templates; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="TemplateEngine"/>.
/// </summary>
public static class TemplateEngineExtensions {
    /// <summary>
    /// Renderiza una plantilla a partir de un archivo especificado en la ruta.
    /// </summary>
    /// <param name="templateEngine">La instancia de <see cref="ITemplateEngine"/> que se utilizará para renderizar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo de plantilla que se desea renderizar.</param>
    /// <param name="data">Un objeto opcional que contiene los datos que se pasarán a la plantilla durante el proceso de renderizado.</param>
    /// <returns>
    /// Una cadena que representa el resultado del renderizado de la plantilla.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Se lanza cuando <paramref name="templateEngine"/> es <c>null</c>.
    /// </exception>
    /// <remarks>
    /// Este método lee el contenido del archivo de plantilla especificado por <paramref name="filePath"/> 
    /// y utiliza el motor de plantillas proporcionado para renderizarlo con los datos opcionales.
    /// </remarks>
    public static string RenderByPath( this ITemplateEngine templateEngine, string filePath, object data = null ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var template = Util.Helpers.File.ReadToString( filePath );
        return templateEngine.Render( template, data );
    }

    /// <summary>
    /// Renderiza una plantilla a partir de un archivo especificado y opcionalmente utiliza un objeto de datos.
    /// </summary>
    /// <param name="templateEngine">La instancia de <see cref="ITemplateEngine"/> que se utilizará para renderizar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo de plantilla que se va a renderizar.</param>
    /// <param name="data">Un objeto opcional que contiene los datos que se utilizarán durante la renderización de la plantilla.</param>
    /// <returns>Una tarea que representa la operación asincrónica de renderización, que contiene el resultado de la plantilla renderizada como una cadena.</returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es <c>null</c>.</exception>
    /// <remarks>
    /// Este método permite cargar una plantilla desde un archivo y renderizarla utilizando un motor de plantillas.
    /// Es útil para generar contenido dinámico basado en plantillas almacenadas en el sistema de archivos.
    /// </remarks>
    /// <seealso cref="ITemplateEngine"/>
    public static async Task<string> RenderByPathAsync( this ITemplateEngine templateEngine, string filePath, object data = null ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var template = await Util.Helpers.File.ReadToStringAsync( filePath );
        return await templateEngine.RenderAsync( template, data );
    }

    /// <summary>
    /// Guarda el resultado de renderizar una plantilla utilizando un motor de plantillas.
    /// </summary>
    /// <param name="templateEngine">El motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="template">La plantilla que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para rellenar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado renderizado.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad del motor de plantillas, permitiendo guardar el resultado
    /// de la renderización directamente en un archivo especificado por <paramref name="filePath"/>.
    /// </remarks>
    public static void Save( this ITemplateEngine templateEngine, string template, object data,string filePath ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = templateEngine.Render( template, data );
        Util.Helpers.File.Write( filePath, result );
    }

    /// <summary>
    /// Guarda el resultado del renderizado de una plantilla en un archivo.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="template">La plantilla que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado del renderizado.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <returns>Una tarea que representa la operación asincrónica de guardar el archivo.</returns>
    /// <remarks>
    /// Este método utiliza el motor de plantillas para renderizar el contenido basado en la plantilla y los datos proporcionados,
    /// y luego guarda el resultado en el archivo especificado por <paramref name="filePath"/>.
    /// </remarks>
    public static async Task SaveAsync( this ITemplateEngine templateEngine, string template, object data, string filePath ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = await templateEngine.RenderAsync( template, data );
        await Util.Helpers.File.WriteAsync( filePath, result );
    }

    /// <summary>
    /// Guarda el resultado de la renderización de una plantilla en un archivo especificado.
    /// </summary>
    /// <param name="templateEngine">La instancia del motor de plantillas que se utilizará para renderizar la plantilla.</param>
    /// <param name="templatePath">La ruta de la plantilla que se va a renderizar.</param>
    /// <param name="data">El objeto que contiene los datos que se utilizarán en la renderización de la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado de la renderización.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es null.</exception>
    /// <remarks>
    /// Este método extiende la funcionalidad del motor de plantillas, permitiendo guardar el resultado de la renderización directamente en un archivo.
    /// Asegúrese de que la ruta del archivo sea válida y que tenga permisos de escritura.
    /// </remarks>
    public static void SaveByPath( this ITemplateEngine templateEngine, string templatePath, object data, string filePath ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = templateEngine.RenderByPath( templatePath, data );
        Util.Helpers.File.Write( filePath, result );
    }

    /// <summary>
    /// Guarda el resultado del renderizado de una plantilla en un archivo especificado.
    /// </summary>
    /// <param name="templateEngine">La instancia de <see cref="ITemplateEngine"/> utilizada para renderizar la plantilla.</param>
    /// <param name="templatePath">La ruta de la plantilla que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para el renderizado de la plantilla.</param>
    /// <param name="filePath">La ruta del archivo donde se guardará el resultado del renderizado.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="templateEngine"/> es <c>null</c>.</exception>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    /// <remarks>
    /// Este método es útil para generar archivos a partir de plantillas dinámicas utilizando datos proporcionados.
    /// </remarks>
    /// <seealso cref="ITemplateEngine"/>
    public static async Task SaveByPathAsync( this ITemplateEngine templateEngine, string templatePath, object data, string filePath ) {
        if( templateEngine == null )
            throw new ArgumentNullException( nameof( templateEngine ) );
        var result = await templateEngine.RenderByPathAsync( templatePath, data );
        await Util.Helpers.File.WriteAsync( filePath, result );
    }
}