using Util.Ui.Builders;
using Util.Ui.Configs;

namespace Util.Ui.Extensions; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="TagBuilder"/>.
/// </summary>
public static class TagBuilderExtensions {
    /// <summary>
    /// Agrega un conjunto de atributos a un objeto <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="TagBuilder"/> al que se le agregarán los atributos.</param>
    /// <param name="attributes">La lista de atributos que se agregarán al <paramref name="builder"/>.</param>
    /// <returns>El objeto <see cref="TagBuilder"/> modificado con los atributos agregados.</returns>
    /// <remarks>
    /// Este método itera sobre cada atributo en la lista de atributos y los agrega al <see cref="TagBuilder"/>.
    /// Se utiliza el método <see cref="SafeString"/> para garantizar que el valor del atributo sea seguro.
    /// </remarks>
    public static TagBuilder Attributes( this TagBuilder builder, TagHelperAttributeList attributes ) {
        foreach ( var attribute in attributes )
            builder.Attribute( attribute.Name, attribute.Value.SafeString() );
        return builder;
    }

    /// <summary>
    /// Aplica estilos al <see cref="TagBuilder"/> utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="builder">El <see cref="TagBuilder"/> al que se le aplicarán los estilos.</param>
    /// <param name="config">La configuración que contiene los valores de estilo.</param>
    /// <returns>El <see cref="TagBuilder"/> modificado con los estilos aplicados.</returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de <see cref="TagBuilder"/> para permitir la 
    /// aplicación de estilos de manera fluida utilizando un objeto de configuración.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    /// <seealso cref="Config"/>
    public static TagBuilder Style( this TagBuilder builder, Config config ) {
        builder.AttributeIfNotEmpty( UiConst.Style, config.GetValue( UiConst.Style ) );
        return builder;
    }

    /// <summary>
    /// Establece la clase de un objeto <see cref="TagBuilder"/> utilizando la configuración proporcionada.
    /// </summary>
    /// <param name="builder">El objeto <see cref="TagBuilder"/> al que se le asignará la clase.</param>
    /// <param name="config">La configuración que contiene el valor de la clase a establecer.</param>
    /// <returns>El objeto <see cref="TagBuilder"/> con la clase actualizada.</returns>
    /// <remarks>
    /// Este método es una extensión para el tipo <see cref="TagBuilder"/> y permite 
    /// asignar una clase de manera más sencilla utilizando un objeto de configuración.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    /// <seealso cref="Config"/>
    public static TagBuilder Class( this TagBuilder builder, Config config ) {
        return builder.Class( config.GetValue( UiConst.Class ) );
    }

    /// <summary>
    /// Establece un atributo "hidden" en el objeto <see cref="TagBuilder"/> si el valor correspondiente en la configuración no está vacío.
    /// </summary>
    /// <param name="builder">El objeto <see cref="TagBuilder"/> al que se le añadirá el atributo.</param>
    /// <param name="config">La configuración que contiene el valor para el atributo "hidden".</param>
    /// <returns>
    /// El objeto <see cref="TagBuilder"/> modificado con el atributo "hidden" si corresponde.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="TagBuilder"/> que permite agregar un atributo de forma condicional.
    /// </remarks>
    /// <seealso cref="TagBuilder"/>
    /// <seealso cref="Config"/>
    public static TagBuilder Hidden( this TagBuilder builder, Config config ) {
        builder.AttributeIfNotEmpty( "[hidden]", config.GetValue( UiConst.Hidden ) );
        return builder;
    }
}