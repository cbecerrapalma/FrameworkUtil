using Util.Ui.Builders;

namespace Util.Ui.Renders; 

/// <summary>
/// Clase base abstracta para la renderización.
/// </summary>
/// <remarks>
/// Esta clase proporciona una interfaz común para las clases que implementan
/// diferentes tipos de renderización. Debe ser heredada por clases que
/// implementen la lógica específica de renderización.
/// </remarks>
public abstract class RenderBase : IRender {
    private TagBuilder _builder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="RenderBase"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor está protegido y solo puede ser llamado desde clases derivadas.
    /// </remarks>
    protected RenderBase() { }

    /// <summary>
    /// Obtiene una instancia de <see cref="TagBuilder"/>. 
    /// Si la instancia aún no ha sido creada, se inicializa utilizando el método <see cref="GetTagBuilder"/>.
    /// </summary>
    /// <value>
    /// Una instancia de <see cref="TagBuilder"/>.
    /// </value>
    protected TagBuilder Builder => _builder ??= GetTagBuilder();

    /// <summary>
    /// Obtiene una instancia de <see cref="TagBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Este método es abstracto y debe ser implementado por las clases derivadas.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="TagBuilder"/> que representa una etiqueta.
    /// </returns>
    protected abstract TagBuilder GetTagBuilder();

    /// <summary>
    /// Escribe el contenido en el escritor de texto especificado utilizando el codificador HTML proporcionado.
    /// </summary>
    /// <param name="writer">El escritor de texto donde se escribirá el contenido.</param>
    /// <param name="encoder">El codificador HTML que se utilizará para codificar el contenido.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada para proporcionar una implementación personalizada.
    /// </remarks>
    public virtual void WriteTo( TextWriter writer, HtmlEncoder encoder ) {
        Builder.WriteTo( writer, encoder );
    }

    /// <inheritdoc />
    /// <summary>
    /// Define un método para clonar una instancia de un objeto que implementa esta interfaz.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="IHtmlContent"/> que es una copia de la instancia actual.
    /// </returns>
    /// <remarks>
    /// Este método permite crear una copia independiente del contenido HTML, lo que puede ser útil
    /// en situaciones donde se necesita modificar el contenido sin afectar la instancia original.
    /// </remarks>
    public abstract IHtmlContent Clone();
}