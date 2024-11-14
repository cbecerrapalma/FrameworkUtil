using System.Text;
using System.Threading.Tasks;
using Util.Templates.Razor.Helpers;

namespace RazorEngineCore; 

/// <summary>
/// Clase base abstracta para la implementación de plantillas Razor.
/// </summary>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica necesaria para crear plantillas Razor personalizadas.
/// Debe ser heredada por clases que implementen la lógica específica de la plantilla.
/// </remarks>
public abstract class RazorEngineTemplateBase : IRazorEngineTemplate
{
    private readonly StringBuilder stringBuilder = new StringBuilder();

    private string attributeSuffix = null;

    /// <summary>
    /// Obtiene o establece el modelo dinámico asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite almacenar un objeto de cualquier tipo, 
    /// lo que proporciona flexibilidad para trabajar con diferentes 
    /// estructuras de datos sin necesidad de definir un tipo específico.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo dinámico que representa el modelo.
    /// </value>
    public dynamic Model { get; set; }

    /// <summary>
    /// Obtiene o establece el helper HTML asociado.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="HtmlHelper"/> que proporciona métodos para generar HTML en las vistas.
    /// </value>
    public HtmlHelper Html { get; set; }

    /// <summary>
    /// Escribe un literal en el objeto actual.
    /// </summary>
    /// <param name="literal">El literal que se va a escribir. Si es nulo, no se realiza ninguna acción.</param>
    public virtual void WriteLiteral(string literal = null)
    {
        this.stringBuilder.Append(literal);
    }

    /// <summary>
    /// Escribe el objeto especificado en el StringBuilder.
    /// </summary>
    /// <param name="obj">El objeto que se va a escribir. Si es nulo, no se agrega nada al StringBuilder.</param>
    public virtual void Write(object obj = null)
    {
        this.stringBuilder.Append(obj);
    }

    /// <summary>
    /// Inicia la escritura de un atributo con el nombre y el prefijo especificados.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a escribir.</param>
    /// <param name="prefix">El prefijo que se añadirá al atributo.</param>
    /// <param name="prefixOffset">El desplazamiento del prefijo en la cadena.</param>
    /// <param name="suffix">El sufijo que se añadirá al atributo.</param>
    /// <param name="suffixOffset">El desplazamiento del sufijo en la cadena.</param>
    /// <param name="attributeValuesCount">El número de valores del atributo.</param>
    /// <remarks>
    /// Este método configura el sufijo del atributo y comienza a construir la cadena del atributo
    /// utilizando el prefijo proporcionado. 
    /// </remarks>
    public virtual void BeginWriteAttribute(string name, string prefix, int prefixOffset, string suffix, int suffixOffset, int attributeValuesCount)
    {
        this.attributeSuffix = suffix;
        this.stringBuilder.Append(prefix);
    }

    /// <summary>
    /// Escribe el valor de un atributo en un formato específico.
    /// </summary>
    /// <param name="prefix">El prefijo que se añadirá al valor del atributo.</param>
    /// <param name="prefixOffset">La posición de inicio del prefijo en el contexto actual.</param>
    /// <param name="value">El valor del atributo que se va a escribir.</param>
    /// <param name="valueOffset">La posición de inicio del valor en el contexto actual.</param>
    /// <param name="valueLength">La longitud del valor que se va a escribir.</param>
    /// <param name="isLiteral">Indica si el valor se debe tratar como un literal.</param>
    /// <remarks>
    /// Este método agrega el prefijo y el valor del atributo a un StringBuilder interno.
    /// El método es virtual, lo que permite que las clases derivadas lo sobrescriban para proporcionar
    /// una implementación personalizada si es necesario.
    /// </remarks>
    public virtual void WriteAttributeValue(string prefix, int prefixOffset, object value, int valueOffset, int valueLength, bool isLiteral)
    {
        this.stringBuilder.Append(prefix);
        this.stringBuilder.Append(value);
    }

    /// <summary>
    /// Finaliza la escritura de un atributo, agregando el sufijo del atributo al 
    /// <see cref="StringBuilder"/> y restableciendo el sufijo a null.
    /// </summary>
    /// <remarks>
    /// Este método debe ser llamado después de haber escrito un atributo para 
    /// asegurar que el sufijo correspondiente se añada correctamente. 
    /// No se debe invocar este método si no se ha iniciado previamente la 
    /// escritura de un atributo.
    /// </remarks>
    public virtual void EndWriteAttribute()
    {
        this.stringBuilder.Append(this.attributeSuffix);
        this.attributeSuffix = null;
    }

    /// <summary>
    /// Ejecuta de manera asíncrona la tarea definida en la clase derivada.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. 
    /// El valor devuelto es una tarea completada sin resultado.
    /// </returns>
    public virtual Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Obtiene el resultado como una cadena de texto.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido actual del objeto <see cref="StringBuilder"/>.
    /// </returns>
    public virtual string Result()
    {
        return this.stringBuilder.ToString();
    }
}