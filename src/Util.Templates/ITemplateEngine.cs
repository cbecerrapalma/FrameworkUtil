namespace Util.Templates; 

/// <summary>
/// Interfaz que define un motor de plantillas.
/// </summary>
/// <remarks>
/// Esta interfaz proporciona métodos para procesar y renderizar plantillas.
/// Los implementadores de esta interfaz deben proporcionar la lógica necesaria
/// para manejar diferentes tipos de plantillas y datos.
/// </remarks>
public interface ITemplateEngine {
    /// <summary>
    /// Renderiza una plantilla utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">La plantilla que se va a renderizar.</param>
    /// <param name="data">Un objeto que contiene los datos que se utilizarán para completar la plantilla. Este parámetro es opcional y su valor predeterminado es null.</param>
    /// <returns>Una cadena que representa el resultado de la plantilla renderizada.</returns>
    /// <remarks>
    /// Este método toma una plantilla en forma de cadena y un objeto de datos, 
    /// y devuelve una cadena resultante de la combinación de ambos. 
    /// Si no se proporcionan datos, la plantilla se renderizará sin información adicional.
    /// </remarks>
    string Render( string template, object data = null );
    /// <summary>
    /// Renderiza un template utilizando los datos proporcionados.
    /// </summary>
    /// <param name="template">El template que se va a renderizar.</param>
    /// <param name="data">Un objeto que contiene los datos que se utilizarán en el template. Puede ser nulo.</param>
    /// <returns>Una tarea que representa la operación asincrónica de renderizado. El resultado es una cadena que contiene el contenido renderizado.</returns>
    /// <remarks>
    /// Este método permite la generación dinámica de contenido basado en un template y datos específicos.
    /// Asegúrese de que el template esté correctamente formateado para evitar errores durante el renderizado.
    /// </remarks>
    /// <seealso cref="Task{TResult}"/>
    Task<string> RenderAsync( string template, object data = null );
}