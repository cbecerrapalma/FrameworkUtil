using Util.Ui.Renders;

namespace Util.Ui.TagHelpers; 

/// <summary>
/// Clase base abstracta para los ayudantes de etiquetas (Tag Helpers).
/// Proporciona funcionalidad común que puede ser utilizada por otros ayudantes de etiquetas.
/// </summary>
public abstract class TagHelperBase : TagHelper {
    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// El identificador como una cadena.
    /// </value>
    public string Id { get; set; }
    /// <summary>
    /// Obtiene o establece el estilo asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir un estilo específico que puede ser utilizado en la presentación de la interfaz de usuario.
    /// </remarks>
    /// <value>
    /// Un string que representa el estilo. Puede ser nulo si no se ha definido un estilo.
    /// </value>
    public string Style { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la clase.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre de la clase.
    /// </value>
    public string Class { get; set; }
    /// <summary>
    /// Obtiene o establece el valor oculto.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar información que no debe ser visible 
    /// o accesible directamente desde la interfaz de usuario.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el valor oculto.
    /// </value>
    public string Hidden { get; set; }

    /// <summary>
    /// Convierte la instancia actual en un objeto <see cref="TagHelperWrapper"/>.
    /// </summary>
    /// <returns>
    /// Un nuevo objeto <see cref="TagHelperWrapper"/> que representa la instancia actual.
    /// </returns>
    public TagHelperWrapper ToWrapper() {
        return new TagHelperWrapper( this );
    }

    /// <summary>
    /// Convierte la instancia actual en un envoltorio de tipo <see cref="TagHelperWrapper{TModel}"/>.
    /// </summary>
    /// <typeparam name="TModel">El tipo del modelo que se utilizará en el envoltorio.</typeparam>
    /// <returns>
    /// Un nuevo objeto <see cref="TagHelperWrapper{TModel}"/> que envuelve la instancia actual.
    /// </returns>
    public TagHelperWrapper<TModel> ToWrapper<TModel>() {
        return new TagHelperWrapper<TModel>( this );
    }

    /// <summary>
    /// Procesa de manera asíncrona el contexto y la salida del TagHelper.
    /// </summary>
    /// <param name="context">El contexto del TagHelper que contiene información sobre la etiqueta HTML.</param>
    /// <param name="output">La salida del TagHelper que se utilizará para generar el contenido HTML.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    /// <remarks>
    /// Este método realiza un procesamiento previo del contexto y la salida, obtiene el contenido hijo
    /// de la salida, genera el contenido a renderizar y lo establece en el elemento posterior de la salida.
    /// La salida se suprime para evitar que se renderice el contenido original.
    /// </remarks>
    public override async Task ProcessAsync( TagHelperContext context, TagHelperOutput output ) {
        ProcessBefore( context, output );
        var content = await output.GetChildContentAsync();
        var render = GetRender( context, output, content );
        output.SuppressOutput();
        output.PostElement.SetHtmlContent( render );
    }

    /// <summary>
    /// Procesa la lógica antes de que se genere la salida del TagHelper.
    /// </summary>
    /// <param name="context">El contexto del TagHelper que proporciona información sobre el elemento HTML que se está procesando.</param>
    /// <param name="output">El objeto que representa la salida del TagHelper y permite modificar el contenido y atributos del elemento HTML.</param>
    /// <remarks>
    /// Este método puede ser sobreescrito en una clase derivada para implementar lógica personalizada antes de la generación de la salida.
    /// </remarks>
    protected virtual void ProcessBefore(TagHelperContext context, TagHelperOutput output) {
    }

    /// <summary>
    /// Obtiene una instancia de <see cref="IRender"/> basada en el contexto del TagHelper, la salida y el contenido proporcionados.
    /// </summary>
    /// <param name="context">El contexto del TagHelper que contiene información sobre el entorno de ejecución.</param>
    /// <param name="output">La salida del TagHelper que se va a modificar o personalizar.</param>
    /// <param name="content">El contenido del TagHelper que se está procesando.</param>
    /// <returns>Una instancia de <see cref="IRender"/> que se utilizará para renderizar el contenido.</returns>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado en una clase derivada.
    /// </remarks>
    protected abstract IRender GetRender( TagHelperContext context, TagHelperOutput output, TagHelperContent content );
}