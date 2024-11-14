namespace Util.Ui.Builders;

/// <summary>
/// Representa un constructor de etiquetas para contenedores vacíos.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="TagBuilder"/> y se utiliza para crear etiquetas HTML que no contienen contenido.
/// </remarks>
public class EmptyContainerTagBuilder : TagBuilder {
    private readonly HtmlContentBuilder _builder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="EmptyContainerTagBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para construir un contenedor HTML vacío con la etiqueta especificada.
    /// </remarks>
    /// <returns>
    /// Una instancia de <see cref="EmptyContainerTagBuilder"/> que representa un contenedor HTML vacío.
    /// </returns>
    public EmptyContainerTagBuilder() : base( "i" ) {
        _builder = new HtmlContentBuilder();
    }

    /// <summary>
    /// Obtiene el contenido HTML interno.
    /// </summary>
    /// <value>
    /// Siempre devuelve <c>null</c>, indicando que no hay contenido HTML interno disponible.
    /// </value>
    public override IHtmlContentBuilder InnerHtml => null;

    /// <summary>
    /// Obtiene un valor que indica si el objeto tiene contenido HTML interno.
    /// </summary>
    /// <value>
    /// <c>true</c> si el objeto contiene contenido HTML interno; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Este valor se determina verificando si el contador de elementos en el 
    /// constructor de cadenas (_builder) es mayor que cero.
    /// </remarks>
    public override bool HasInnerHtml => _builder.Count > 0;

    /// <summary>
    /// Establece la clase CSS para el objeto actual.
    /// </summary>
    /// <param name="class">La cadena que representa la clase CSS a asignar.</param>
    /// <returns>Devuelve el objeto actual <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public override TagBuilder Class( string @class ) {
        return this;
    }

    /// <summary>
    /// Establece un atributo en el objeto actual de tipo <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo. Por defecto es una cadena vacía.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. Por defecto es <c>false</c>.</param>
    /// <param name="append">Indica si se debe agregar el valor al final del atributo existente en lugar de reemplazarlo. Por defecto es <c>false</c>.</param>
    /// <returns>Devuelve el objeto actual de tipo <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public override TagBuilder Attribute( string name, string value = "", bool replaceExisting = false, bool append = false ) {
        return this;
    }

    /// <summary>
    /// Establece un atributo en el objeto <see cref="TagBuilder"/> si el valor proporcionado no está vacío.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo que se va a establecer.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. El valor predeterminado es <c>false</c>.</param>
    /// <param name="append">Indica si se debe agregar el valor al atributo existente en lugar de reemplazarlo. El valor predeterminado es <c>false</c>.</param>
    /// <returns>
    /// Devuelve el objeto <see cref="TagBuilder"/> actual para permitir la encadenación de métodos.
    /// </returns>
    /// <remarks>
    /// Este método no modifica el objeto si el valor proporcionado está vacío.
    /// </remarks>
    public override TagBuilder AttributeIfNotEmpty( string name, string value, bool replaceExisting = false, bool append = false ) {
        return this;
    }

    /// <summary>
    /// Agrega contenido HTML al constructor de etiquetas.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a agregar.</param>
    /// <returns>El mismo objeto <see cref="TagBuilder"/> para permitir la encadenación de llamadas.</returns>
    public override TagBuilder AppendContent( string content ) {
        _builder.AppendHtml( content );
        return this;
    }

    /// <summary>
    /// Agrega contenido HTML a la instancia actual de <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a agregar.</param>
    /// <returns>La instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public override TagBuilder AppendContent( IHtmlContent content ) {
        _builder.AppendHtml( content );
        return this;
    }

    /// <summary>
    /// Agrega contenido a un <see cref="TagBuilder"/> existente.
    /// </summary>
    /// <param name="tagBuilder">El <see cref="TagBuilder"/> al que se le agregará el contenido.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica de cómo se debe agregar el contenido.
    /// </remarks>
    public override TagBuilder AppendContent( TagBuilder tagBuilder ) {
        AppendContent( (IHtmlContent)tagBuilder );
        return this;
    }

    /// <summary>
    /// Establece el contenido HTML para el objeto <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public override TagBuilder SetContent( string content ) {
        _builder.SetHtmlContent( content );
        return this;
    }

    /// <summary>
    /// Establece el contenido HTML para el objeto <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public override TagBuilder SetContent( IHtmlContent content ) {
        _builder.SetHtmlContent( content );
        return this;
    }

    /// <summary>
    /// Establece el contenido de un <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="tagBuilder">El <see cref="TagBuilder"/> cuyo contenido se va a establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que permite establecer el contenido de un objeto <see cref="TagBuilder"/>.
    /// </remarks>
    public override TagBuilder SetContent( TagBuilder tagBuilder ) {
        SetContent( (IHtmlContent)tagBuilder );
        return this;
    }

    /// <summary>
    /// Configura los parámetros necesarios para la clase derivada.
    /// </summary>
    /// <remarks>
    /// Este método debe ser implementado en las clases que heredan de esta clase base.
    /// Se espera que cada implementación configure adecuadamente los parámetros específicos.
    /// </remarks>
    public override void Config() {
    }

    /// <summary>
    /// Escribe el contenido del objeto en el flujo de texto especificado.
    /// </summary>
    /// <param name="writer">El flujo de texto donde se escribirá el contenido.</param>
    /// <param name="encoder">El codificador HTML que se utilizará para codificar el contenido.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica
    /// que utiliza un codificador HTML nulo por defecto.
    /// </remarks>
    public override void WriteTo( TextWriter writer, HtmlEncoder encoder ) {
        _builder.WriteTo( writer, NullHtmlEncoder.Default );
    }

    /// <summary>
    /// Convierte el objeto actual en una representación de cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un <see cref="StringWriter"/> para escribir el contenido de 
    /// <see cref="_builder"/> y devuelve la representación en forma de cadena.
    /// </remarks>
    public override string ToString() {
        using var writer = new StringWriter();
        _builder.WriteTo( writer, NullHtmlEncoder.Default );
        return writer.ToString();
    }
}