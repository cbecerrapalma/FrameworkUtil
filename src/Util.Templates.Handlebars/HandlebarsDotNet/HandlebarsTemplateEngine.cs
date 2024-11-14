namespace Util.Templates.HandlebarsDotNet;

/// <summary>
/// Representa un motor de plantillas Handlebars.
/// </summary>
/// <remarks>
/// Este motor permite la generación de contenido dinámico a partir de plantillas Handlebars,
/// facilitando la separación de la lógica de presentación del resto de la aplicación.
/// </remarks>
public class HandlebarsTemplateEngine : IHandlebarsTemplateEngine {
    protected static readonly ConcurrentDictionary<int, HandlebarsTemplate<object, object>> TemplateCache = new();
    private ITextEncoder _encoder;

    /// <summary>
    /// Limpia la caché de plantillas.
    /// </summary>
    /// <remarks>
    /// Este método está diseñado para eliminar todos los elementos almacenados en la caché de plantillas.
    /// Se debe utilizar con precaución, ya que puede afectar el rendimiento si se llama con frecuencia.
    /// </remarks>
    public static void ClearCache() {
        TemplateCache.Clear();
    }

    /// <summary>
    /// Establece el codificador de texto para la instancia actual del motor de plantillas Handlebars.
    /// </summary>
    /// <param name="encoder">El codificador de texto que se utilizará para codificar las plantillas.</param>
    /// <returns>La instancia actual del motor de plantillas Handlebars.</returns>
    public IHandlebarsTemplateEngine Encoder( ITextEncoder encoder ) {
        _encoder = encoder;
        return this;
    }

    /// <summary>
    /// Crea una instancia de un motor de plantillas de Handlebars que utiliza un codificador HTML.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="IHandlebarsTemplateEngine"/> que utiliza el codificador HTML.
    /// </returns>
    public IHandlebarsTemplateEngine HtmlEncoder() {
        return Encoder( new HtmlEncoder() );
    }

    /// <summary>
    /// Renderiza una plantilla utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">La plantilla que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar la plantilla. Puede ser nulo.</param>
    /// <returns>
    /// Una cadena que representa el resultado de la plantilla renderizada, o null si la plantilla está vacía o en blanco.
    /// </returns>
    public virtual string Render( string template, object data = null ) {
        if ( string.IsNullOrWhiteSpace( template ) )
            return null;
        var compiledTemplate = GetCompiledTemplateFromCache( template );
        return compiledTemplate?.Invoke( data );
    }

    /// <summary>
    /// Obtiene una plantilla compilada desde la caché o la compila si no está disponible.
    /// </summary>
    /// <param name="template">La cadena que representa la plantilla a compilar.</param>
    /// <returns>
    /// Una instancia de <see cref="HandlebarsTemplate{TModel, TContext}"/> que representa la plantilla compilada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un mecanismo de caché para almacenar las plantillas compiladas, 
    /// lo que mejora el rendimiento al evitar la recompilación de plantillas que ya han sido procesadas.
    /// </remarks>
    /// <seealso cref="Handlebars"/>
    protected virtual HandlebarsTemplate<object, object> GetCompiledTemplateFromCache( string template ) {
        return TemplateCache.GetOrAdd( template.GetHashCode(), _ => {
            var handlebars = Handlebars.Create();
            handlebars.Configuration.TextEncoder = _encoder;
            return handlebars.Compile( template );
        } );
    }

    /// <summary>
    /// Renderiza un template utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">El template que se va a renderizar.</param>
    /// <param name="data">Los datos que se utilizarán para completar el template. Puede ser nulo.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona de renderizado, 
    /// con un resultado que es una cadena que contiene el resultado del renderizado.
    /// </returns>
    public virtual Task<string> RenderAsync( string template, object data = null ) {
        return Task.FromResult( Render( template, data ) );
    }
}