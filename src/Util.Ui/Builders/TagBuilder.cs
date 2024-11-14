using Microsoft.AspNetCore.Mvc.Rendering;
using Util.Ui.Configs;
using Util.Ui.Extensions;
using Util.Ui.Helpers;

namespace Util.Ui.Builders;

/// <summary>
/// Representa un generador de etiquetas HTML que implementa la interfaz <see cref="IHtmlContent"/>.
/// </summary>
public class TagBuilder : IHtmlContent {
    private readonly Microsoft.AspNetCore.Mvc.Rendering.TagBuilder _tagBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="tagName">El nombre del tag HTML que se va a crear.</param>
    /// <param name="renderMode">El modo de renderizado del tag. Por defecto es <see cref="TagRenderMode.Normal"/>.</param>
    /// <remarks>
    /// Esta clase se utiliza para construir elementos HTML de manera programática.
    /// </remarks>
    public TagBuilder( string tagName, TagRenderMode renderMode = TagRenderMode.Normal ) {
        _tagBuilder = new Microsoft.AspNetCore.Mvc.Rendering.TagBuilder( tagName ) { TagRenderMode = renderMode };
    }

    public static readonly TagBuilder Null = new EmptyTagBuilder();

    /// <summary>
    /// Obtiene el contenido HTML interno del constructor de etiquetas.
    /// </summary>
    /// <value>
    /// Un objeto que representa el contenido HTML interno.
    /// </value>
    /// <remarks>
    /// Esta propiedad permite acceder al contenido HTML que se encuentra dentro de la etiqueta actual.
    /// </remarks>
    public virtual IHtmlContentBuilder InnerHtml => _tagBuilder.InnerHtml;

    /// <summary>
    /// Obtiene un valor que indica si el elemento HTML tiene contenido interno.
    /// </summary>
    /// <value>
    /// <c>true</c> si el elemento HTML tiene contenido interno; de lo contrario, <c>false</c>.
    /// </value>
    /// <remarks>
    /// Esta propiedad es útil para determinar si se debe renderizar contenido adicional dentro del elemento HTML.
    /// </remarks>
    public virtual bool HasInnerHtml => _tagBuilder.HasInnerHtml;

    /// <summary>
    /// Obtiene o establece el objeto <see cref="TagBuilder"/> que se utilizará para construir etiquetas.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite personalizar la construcción de etiquetas en el contexto actual.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TagBuilder"/> que representa el constructor de etiquetas.
    /// </value>
    public TagBuilder PreBuilder { get; set; }

    /// <summary>
    /// Obtiene o establece el objeto <see cref="TagBuilder"/> utilizado para construir etiquetas.
    /// </summary>
    /// <remarks>
    /// Este objeto permite la creación y manipulación de etiquetas HTML en el contexto de la aplicación.
    /// </remarks>
    /// <value>
    /// Un objeto <see cref="TagBuilder"/> que representa el constructor de etiquetas.
    /// </value>
    public TagBuilder PostBuilder { get; set; }

    /// <summary>
    /// Agrega una clase CSS al <see cref="_tagBuilder"/> si la cadena proporcionada no está vacía o es nula.
    /// </summary>
    /// <param name="class">La clase CSS que se desea agregar al <see cref="_tagBuilder"/>.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public virtual TagBuilder Class( string @class ) {
        if ( string.IsNullOrWhiteSpace( @class ) == false )
            _tagBuilder.AddCssClass( @class );
        return this;
    }

    /// <summary>
    /// Agrega o actualiza un atributo en el TagBuilder.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a agregar o actualizar.</param>
    /// <param name="value">El valor del atributo. Por defecto es una cadena vacía.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar el valor existente del atributo. Por defecto es false.</param>
    /// <param name="append">Indica si se debe agregar el nuevo valor al valor existente del atributo. Por defecto es false.</param>
    /// <returns>Devuelve la instancia actual de TagBuilder para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si el atributo no existe, se agrega con el valor proporcionado. 
    /// Si el atributo ya existe y <paramref name="replaceExisting"/> es true, se reemplaza el valor existente. 
    /// Si <paramref name="append"/> es true, se añade el nuevo valor al final del valor existente, separado por un punto y coma.
    /// </remarks>
    public virtual TagBuilder Attribute( string name, string value = "", bool replaceExisting = false, bool append = false ) {
        if ( _tagBuilder.Attributes.ContainsKey( name ) == false ) {
            _tagBuilder.MergeAttribute( name, value );
            return this;
        }
        if ( replaceExisting ) {
            _tagBuilder.MergeAttribute( name, value, true );
            return this;
        }
        if ( append == false )
            return this;
        var newValue = $"{_tagBuilder.Attributes[name]};{value}";
        _tagBuilder.MergeAttribute( name, newValue, true );
        return this;
    }

    /// <summary>
    /// Agrega un atributo al objeto actual si la condición especificada es verdadera.
    /// </summary>
    /// <param name="name">El nombre del atributo que se agregará.</param>
    /// <param name="condition">Una condición que determina si se debe agregar el atributo.</param>
    /// <returns>El objeto actual <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    public virtual TagBuilder AttributeIf( string name, bool condition ) {
        if ( condition == false )
            return this;
        Attribute( name );
        return this;
    }

    /// <summary>
    /// Agrega un atributo a la instancia actual si se cumple la condición especificada.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a agregar.</param>
    /// <param name="value">El valor del atributo que se va a agregar.</param>
    /// <param name="condition">Una condición que determina si se debe agregar el atributo.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre.</param>
    /// <param name="append">Indica si se debe agregar el valor al final del atributo existente.</param>
    /// <returns>La instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si la condición es falsa, el método no realiza ninguna acción y simplemente devuelve la instancia actual.
    /// </remarks>
    public virtual TagBuilder AttributeIf( string name, string value, bool condition, bool replaceExisting = false, bool append = false ) {
        if ( condition == false )
            return this;
        Attribute( name, value, replaceExisting, append );
        return this;
    }

    /// <summary>
    /// Establece un atributo en el objeto actual si el valor proporcionado no está vacío.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo que se va a establecer.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. El valor predeterminado es false.</param>
    /// <param name="append">Indica si se debe agregar el valor al atributo existente en lugar de reemplazarlo. El valor predeterminado es false.</param>
    /// <returns>El objeto actual <see cref="TagBuilder"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Este método verifica si el valor proporcionado no es nulo ni una cadena vacía antes de intentar establecer el atributo.
    /// Si el valor es válido y el atributo ya existe, se comportará según el parámetro <paramref name="replaceExisting"/>.
    /// Si <paramref name="append"/> es true, el valor se agregará al atributo existente.
    /// </remarks>
    public virtual TagBuilder AttributeIfNotEmpty( string name, string value, bool replaceExisting = false, bool append = false ) {
        AttributeIf( name, value, !string.IsNullOrWhiteSpace( value ), replaceExisting, append );
        return this;
    }

    /// <summary>
    /// Agrega contenido HTML al <see cref="TagBuilder"/> actual.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a agregar.</param>
    /// <returns>El <see cref="TagBuilder"/> actual con el contenido agregado.</returns>
    /// <remarks>
    /// Este método permite añadir contenido HTML de forma encadenada, 
    /// lo que facilita la construcción de elementos HTML de manera fluida.
    /// </remarks>
    public virtual TagBuilder AppendContent( string content ) {
        _tagBuilder.InnerHtml.AppendHtml( content );
        return this;
    }

    /// <summary>
    /// Agrega contenido HTML a la instancia actual de <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a agregar.</param>
    /// <returns>La instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método permite agregar contenido HTML de manera segura, utilizando la clase <see cref="IHtmlContent"/>.
    /// </remarks>
    /// <seealso cref="IHtmlContent"/>
    public virtual TagBuilder AppendContent( IHtmlContent content ) {
        _tagBuilder.InnerHtml.AppendHtml( content );
        return this;
    }

    /// <summary>
    /// Agrega contenido a un <see cref="TagBuilder"/> existente.
    /// </summary>
    /// <param name="tagBuilder">El <see cref="TagBuilder"/> al que se le agregará el contenido.</param>
    /// <returns>
    /// Devuelve la instancia actual de <see cref="TagBuilder"/> para permitir la encadenación de llamadas.
    /// </returns>
    /// <remarks>
    /// Este método permite modificar el contenido de un <see cref="TagBuilder"/> existente
    /// al agregarle contenido desde otro <see cref="TagBuilder"/>.
    /// </remarks>
    public virtual TagBuilder AppendContent( TagBuilder tagBuilder ) {
        AppendContent( (IHtmlContent)tagBuilder );
        return this;
    }

    /// <summary>
    /// Establece el contenido HTML interno del TagBuilder.
    /// </summary>
    /// <param name="content">El contenido HTML que se establecerá.</param>
    /// <returns>Devuelve la instancia actual de TagBuilder para permitir la encadenación de métodos.</returns>
    public virtual TagBuilder SetContent( string content ) {
        _tagBuilder.InnerHtml.SetHtmlContent( content );
        return this;
    }

    /// <summary>
    /// Establece el contenido HTML de un TagBuilder.
    /// </summary>
    /// <param name="content">El contenido HTML que se va a establecer.</param>
    /// <returns>El objeto TagBuilder actual con el contenido actualizado.</returns>
    public virtual TagBuilder SetContent( IHtmlContent content ) {
        _tagBuilder.InnerHtml.SetHtmlContent( content );
        return this;
    }

    /// <summary>
    /// Establece el contenido de un <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="tagBuilder">El <see cref="TagBuilder"/> que contiene el contenido a establecer.</param>
    /// <returns>El <see cref="TagBuilder"/> actual con el contenido establecido.</returns>
    /// <remarks>
    /// Este método permite configurar el contenido de un objeto <see cref="TagBuilder"/> 
    /// utilizando otro <see cref="TagBuilder"/> como fuente.
    /// </remarks>
    public virtual TagBuilder SetContent( TagBuilder tagBuilder ) {
        SetContent( (IHtmlContent)tagBuilder );
        return this;
    }

    /// <summary>
    /// Escribe el contenido en el flujo de salida especificado utilizando el codificador HTML proporcionado.
    /// </summary>
    /// <param name="writer">El escritor de texto que se utilizará para la salida.</param>
    /// <param name="encoder">El codificador HTML que se utilizará para codificar el contenido.</param>
    /// <remarks>
    /// Este método verifica si existen constructores previos y posteriores. Si están presentes, 
    /// se escriben en el flujo de salida antes y después del contenido principal.
    /// </remarks>
    public virtual void WriteTo( TextWriter writer, HtmlEncoder encoder ) {
        if ( PreBuilder != null )
            PreBuilder.WriteTo( writer, NullHtmlEncoder.Default );
        _tagBuilder.WriteTo( writer, NullHtmlEncoder.Default );
        if ( PostBuilder != null )
            PostBuilder.WriteTo( writer, NullHtmlEncoder.Default );
    }

    /// <summary>
    /// Convierte el objeto actual en una representación de cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el objeto actual, generada a partir del contenido del 
    /// <see cref="_tagBuilder"/> utilizando un codificador HTML nulo por defecto.
    /// </returns>
    public override string ToString() {
        using var writer = new StringWriter();
        _tagBuilder.WriteTo( writer, NullHtmlEncoder.Default );
        return writer.ToString();
    }

    /// <summary>
    /// Configura los parámetros necesarios para la clase.
    /// </summary>
    /// <remarks>
    /// Este método puede ser sobreescrito por las clases derivadas para proporcionar una configuración específica.
    /// </remarks>
    public virtual void Config() {
    }

    /// <summary>
    /// Configura la base de la configuración proporcionada.
    /// </summary>
    /// <param name="config">La instancia de configuración que se va a configurar.</param>
    /// <remarks>
    /// Este método llama a otros métodos de configuración específicos para aplicar estilos, clases, 
    /// atributos ocultos y atributos de salida a la configuración.
    /// </remarks>
    public virtual void ConfigBase( Config config ) {
        ConfigStyle( config );
        ConfigClass( config );
        ConfigHidden( config );
        ConfigOutputAttributes( config );
    }

    /// <summary>
    /// Configura el estilo utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para establecer el estilo.</param>
    protected virtual void ConfigStyle(Config config) 
    { 
        this.Style(config); 
    }

    /// <summary>
    /// Configura la clase utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que se utilizará para configurar la clase.</param>
    protected virtual void ConfigClass(Config config) 
    {
        this.Class(config);
    }

    /// <summary>
    /// Configura la propiedad oculta utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La instancia de configuración que se utilizará para establecer la propiedad oculta.</param>
    protected virtual void ConfigHidden(Config config) {
        this.Hidden(config);
    }

    /// <summary>
    /// Configura los atributos de salida utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="config">La configuración que contiene los atributos de salida a aplicar.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobrescrito en clases derivadas para proporcionar una implementación personalizada.
    /// </remarks>
    protected virtual void ConfigOutputAttributes(Config config) 
    {
        this.Attributes(config.OutputAttributes);
    }

    /// <summary>
    /// Aplica un estilo a la configuración especificada.
    /// </summary>
    /// <param name="config">La instancia de configuración a la que se le aplicará el estilo.</param>
    /// <param name="name">El nombre del estilo que se desea aplicar.</param>
    /// <param name="value">El valor del estilo que se desea establecer.</param>
    protected virtual void Style(Config config, string name, string value) {
        config.SetAttribute(UiConst.Style, $"{name}:{SizeHelper.GetValue(value)}");
    }
}