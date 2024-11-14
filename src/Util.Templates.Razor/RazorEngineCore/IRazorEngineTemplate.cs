using System.Threading.Tasks;
using Util.Templates.Razor.Helpers;

namespace RazorEngineCore; 

/// <summary>
/// Define una interfaz para la plantilla del motor Razor.
/// </summary>
public interface IRazorEngineTemplate
{
    /// <summary>
    /// Obtiene o establece el modelo dinámico.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar un objeto de cualquier tipo, lo que proporciona flexibilidad 
    /// para trabajar con diferentes tipos de datos en tiempo de ejecución.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo dinámico que representa el modelo.
    /// </value>
    dynamic Model { get; set; }
    /// <summary>
    /// Obtiene o establece el objeto HtmlHelper asociado.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="HtmlHelper"/> que proporciona métodos auxiliares para generar HTML.
    /// </value>
    HtmlHelper Html { get; set; }
    /// <summary>
    /// Escribe un literal en la salida.
    /// </summary>
    /// <param name="literal">El literal que se va a escribir. Si es null, no se escribirá nada.</param>
    /// <remarks>
    /// Este método se utiliza para escribir texto literal en la salida. 
    /// Si el parámetro <paramref name="literal"/> es null, el método no realizará ninguna acción.
    /// </remarks>
    void WriteLiteral(string literal = null);
    /// <summary>
    /// Escribe la representación en cadena del objeto especificado.
    /// </summary>
    /// <param name="obj">El objeto que se va a escribir. Si es null, se utilizará un valor predeterminado.</param>
    /// <remarks>
    /// Este método puede ser utilizado para registrar información o para mostrar datos en la consola.
    /// Si el objeto es null, se puede manejar de manera específica según la implementación.
    /// </remarks>
    void Write(object obj = null);
    /// <summary>
    /// Inicia la escritura de un atributo con el nombre y prefijo especificados.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a escribir.</param>
    /// <param name="prefix">El prefijo del atributo, si corresponde.</param>
    /// <param name="prefixOffset">El desplazamiento del prefijo en la cadena.</param>
    /// <param name="suffix">El sufijo del atributo, si corresponde.</param>
    /// <param name="suffixOffset">El desplazamiento del sufijo en la cadena.</param>
    /// <param name="attributeValuesCount">El número de valores del atributo que se van a escribir.</param>
    /// <remarks>
    /// Este método se utiliza para preparar la escritura de un atributo en un contexto específico,
    /// permitiendo la especificación de prefijos y sufijos para el nombre del atributo.
    /// </remarks>
    void BeginWriteAttribute(string name, string prefix, int prefixOffset, string suffix, int suffixOffset, int attributeValuesCount);
    /// <summary>
    /// Escribe el valor de un atributo con un prefijo y un valor especificados.
    /// </summary>
    /// <param name="prefix">El prefijo del atributo que se va a escribir.</param>
    /// <param name="prefixOffset">La posición de inicio del prefijo en la cadena.</param>
    /// <param name="value">El valor del atributo que se va a escribir.</param>
    /// <param name="valueOffset">La posición de inicio del valor en el objeto.</param>
    /// <param name="valueLength">La longitud del valor que se va a escribir.</param>
    /// <param name="isLiteral">Indica si el valor se debe tratar como literal.</param>
    /// <remarks>
    /// Este método se utiliza para escribir atributos en un formato específico, 
    /// permitiendo especificar offsets y longitudes para el prefijo y el valor. 
    /// El parámetro <c>isLiteral</c> determina si el valor debe ser tratado 
    /// como un literal, lo que puede afectar la forma en que se procesa el 
    /// valor al ser escrito.
    /// </remarks>
    void WriteAttributeValue(string prefix, int prefixOffset, object value, int valueOffset, int valueLength, bool isLiteral);
    /// <summary>
    /// Finaliza la escritura de un atributo en el contexto actual.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para cerrar el atributo que se está escribiendo actualmente.
    /// Debe ser llamado después de haber escrito todos los datos del atributo.
    /// </remarks>
    void EndWriteAttribute();
    /// <summary>
    /// Ejecuta una tarea de forma asíncrona.
    /// </summary>
    /// <remarks>
    /// Este método permite realizar operaciones que pueden llevar tiempo sin bloquear el hilo de ejecución actual.
    /// Asegúrese de manejar adecuadamente las excepciones que puedan surgir durante la ejecución.
    /// </remarks>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    /// <exception cref="System.Exception">
    /// Se produce si ocurre un error durante la ejecución de la tarea.
    /// </exception>
    Task ExecuteAsync();
    /// <summary>
    /// Obtiene el resultado en forma de cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el resultado.
    /// </returns>
    /// <remarks>
    /// Este método puede ser utilizado para obtener información procesada o calculada
    /// que se necesita en forma de texto. Asegúrese de que el resultado esté disponible
    /// antes de llamar a este método.
    /// </remarks>
    string Result();
}