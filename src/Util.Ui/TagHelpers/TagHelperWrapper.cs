using Util.Ui.Configs;
using Util.Ui.Expressions;

namespace Util.Ui.TagHelpers; 

/// <summary>
/// Clase que envuelve un modelo de tipo <typeparamref name="TModel"/> para su uso en un TagHelper.
/// </summary>
/// <typeparam name="TModel">El tipo de modelo que se va a envolver.</typeparam>
/// <remarks>
/// Esta clase permite la manipulación y acceso a propiedades del modelo dentro de un TagHelper,
/// facilitando la integración con la vista y la lógica de presentación.
/// </remarks>
public class TagHelperWrapper<TModel> : TagHelperWrapper {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TagHelperWrapper"/>.
    /// </summary>
    /// <param name="tagHelper">La instancia de <see cref="ITagHelper"/> que se va a envolver.</param>
    public TagHelperWrapper( ITagHelper tagHelper ) : base( tagHelper ) {
    }

    /// <summary>
    /// Establece una expresión para un modelo específico.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad que se está expresando.</typeparam>
    /// <param name="propertyExpression">La expresión que representa la propiedad del modelo.</param>
    /// <returns>Una instancia de <see cref="TagHelperWrapper"/> que representa la configuración del tag helper.</returns>
    public TagHelperWrapper SetExpression<TProperty>( Expression<Func<TModel, TProperty>> propertyExpression ) {
        return SetExpression( UiConst.For, propertyExpression );
    }

    /// <summary>
    /// Establece una expresión para un atributo de contexto en el envoltorio de TagHelper.
    /// </summary>
    /// <typeparam name="TProperty">El tipo de la propiedad a la que se hace referencia en la expresión.</typeparam>
    /// <param name="name">El nombre del atributo de contexto que se va a establecer.</param>
    /// <param name="propertyExpression">La expresión que representa la propiedad del modelo.</param>
    /// <returns>El mismo objeto <see cref="TagHelperWrapper"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método utiliza la expresión proporcionada para crear una representación del modelo y la establece
    /// como un atributo de contexto, lo que permite su uso en la vista correspondiente.
    /// </remarks>
    /// <seealso cref="ModelExpressionHelper"/>
    /// <seealso cref="Util.Helpers.Lambda"/>
    public TagHelperWrapper SetExpression<TProperty>( string name, Expression<Func<TModel, TProperty>> propertyExpression ) {
        var modelExpression = ModelExpressionHelper.Create( Util.Helpers.Lambda.GetName( propertyExpression ), propertyExpression );
        SetContextAttribute( name, modelExpression );
        return this;
    }
}

/// <summary>
/// Representa un envoltorio para ayudar en la manipulación de etiquetas.
/// </summary>
public class TagHelperWrapper {
    private readonly TagHelperAttributeList _contextAttributes;
    private readonly TagHelperAttributeList _outputAttributes;
    private readonly IDictionary<object, object> _items;
    private readonly ITagHelper _component;
    private readonly TagHelperContent _content;
    private readonly List<TagHelperWrapper> _children;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TagHelperWrapper"/>.
    /// </summary>
    /// <param name="tagHelper">La instancia de <see cref="ITagHelper"/> que se va a envolver.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="tagHelper"/> es <c>null</c>.</exception>
    public TagHelperWrapper( ITagHelper tagHelper ) {
        _component = tagHelper ?? throw new ArgumentNullException( nameof( tagHelper ) );
        _contextAttributes = new TagHelperAttributeList();
        _outputAttributes = new TagHelperAttributeList();
        _items = new Dictionary<object, object>();
        _content = new DefaultTagHelperContent();
        _children = new List<TagHelperWrapper>();
    }

    /// <summary>
    /// Obtiene el TagHelper asociado.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ITagHelper"/> que representa el TagHelper.
    /// </returns>
    public ITagHelper GetTagHelper() {
        return _component;
    }

    /// <summary>
    /// Establece un atributo en el contexto de la etiqueta.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo que se va a establecer.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. El valor predeterminado es true.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagHelperWrapper"/>.</returns>
    /// <remarks>
    /// Si <paramref name="replaceExisting"/> es false y el atributo ya existe, no se realizará ninguna acción.
    /// </remarks>
    public TagHelperWrapper SetContextAttribute( string name, object value, bool replaceExisting = true ) {
        if ( replaceExisting == false && _contextAttributes.ContainsName( name ) )
            return this;
        _contextAttributes.SetAttribute( new TagHelperAttribute( name, value ) );
        return this;
    }

    /// <summary>
    /// Establece un atributo de salida en el envoltorio del TagHelper.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo que se va a establecer.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. El valor predeterminado es verdadero.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagHelperWrapper"/> para permitir la encadenación de métodos.</returns>
    /// <remarks>
    /// Si <paramref name="replaceExisting"/> es falso y el atributo ya existe, no se realizará ninguna acción.
    /// </remarks>
    public TagHelperWrapper SetOutputAttribute( string name, object value, bool replaceExisting = true ) {
        if ( replaceExisting == false && _outputAttributes.ContainsName( name ) )
            return this;
        _outputAttributes.SetAttribute( new TagHelperAttribute( name, value ) );
        return this;
    }

    /// <summary>
    /// Establece un elemento en el contenedor utilizando una clave y un valor.
    /// </summary>
    /// <param name="key">La clave del elemento que se desea establecer. No puede ser nula.</param>
    /// <param name="value">El valor del elemento que se desea establecer.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagHelperWrapper"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Este método agrega o actualiza un elemento en la colección interna utilizando la clave proporcionada.
    /// Si la clave es nula, el elemento no se establece.
    /// </remarks>
    public TagHelperWrapper SetItem( object key, object value ) {
        if ( key != null )
            _items[key] = value;
        return this;
    }

    /// <summary>
    /// Establece un elemento de tipo especificado en el envoltorio de TagHelper.
    /// </summary>
    /// <typeparam name="T">El tipo del elemento que se va a establecer.</typeparam>
    /// <param name="value">El valor del elemento que se va a establecer.</param>
    /// <returns>Una instancia de <see cref="TagHelperWrapper"/> que representa el estado actualizado.</returns>
    public TagHelperWrapper SetItem<T>( T value ) {
        var key = typeof( T );
        return SetItem( key, value );
    }

    /// <summary>
    /// Agrega contenido HTML al envoltorio de etiquetas.
    /// </summary>
    /// <param name="value">El contenido HTML que se va a agregar. Si es nulo o está vacío, no se realiza ninguna acción.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagHelperWrapper"/> para permitir la encadenación de llamadas.</returns>
    public TagHelperWrapper AppendContent( string value ) {
        if ( string.IsNullOrWhiteSpace( value ) )
            return this;
        _content.AppendHtml( value );
        return this;
    }

    /// <summary>
    /// Agrega contenido HTML a la instancia actual de <see cref="TagHelperWrapper"/>.
    /// </summary>
    /// <param name="value">El contenido HTML que se va a agregar. Puede ser nulo.</param>
    /// <returns>La instancia actual de <see cref="TagHelperWrapper"/> para permitir la encadenación de llamadas.</returns>
    /// <remarks>
    /// Si el parámetro <paramref name="value"/> es nulo, la instancia actual se devuelve sin cambios.
    /// </remarks>
    public TagHelperWrapper AppendContent( IHtmlContent value ) {
        if ( value == null )
            return this;
        _content.AppendHtml( value );
        return this;
    }

    /// <summary>
    /// Agrega un componente hijo a la colección de componentes.
    /// </summary>
    /// <param name="childComponent">El componente hijo que se va a agregar. Si es null, no se realiza ninguna acción.</param>
    /// <returns>Devuelve la instancia actual de <see cref="TagHelperWrapper"/>.</returns>
    public TagHelperWrapper AppendContent( ITagHelper childComponent ) {
        if ( childComponent == null )
            return this;
        _children.Add( new TagHelperWrapper( childComponent ) );
        return this;
    }

    /// <summary>
    /// Agrega un componente hijo a la colección de componentes.
    /// </summary>
    /// <param name="childComponent">El componente hijo que se va a agregar.</param>
    /// <returns>La instancia actual de <see cref="TagHelperWrapper"/> con el componente hijo agregado.</returns>
    /// <remarks>
    /// Si el componente hijo es nulo, se devuelve la instancia actual sin realizar cambios.
    /// </remarks>
    public TagHelperWrapper AppendContent( TagHelperWrapper childComponent ) {
        if ( childComponent == null )
            return this;
        _children.Add( childComponent );
        return this;
    }

    /// <summary>
    /// Obtiene el resultado de la representación del contenido procesado.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido HTML generado.
    /// </returns>
    /// <remarks>
    /// Este método crea un contexto de TagHelper y un TagHelperOutput, 
    /// procesa el contenido de los componentes hijos y devuelve el HTML resultante 
    /// como una cadena.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si ocurre un error durante el procesamiento del componente.
    /// </exception>
    public string GetResult() {
        TagHelperContent content = new DefaultTagHelperContent();
        _content.CopyTo( content );
        var context = new TagHelperContext( _contextAttributes, _items, Guid.NewGuid().ToString() );
        var output = new TagHelperOutput( "", _outputAttributes, ( useCachedResult, encoder ) => {
            foreach ( var child in _children )
                content.AppendHtml( child.GetContent( _items ) );
            return Task.FromResult( content );
        } );
        _component.ProcessAsync( context, output ).GetAwaiter().GetResult();
        var writer = new StringWriter();
        output.WriteTo( writer, HtmlEncoder.Default );
        return writer.ToString();
    }

    /// <summary>
    /// Obtiene el contenido de un TagHelper, copiando el contenido existente y procesando los elementos secundarios.
    /// </summary>
    /// <param name="items">Un diccionario que contiene elementos clave-valor que se utilizarán en el contexto del TagHelper.</param>
    /// <returns>Un objeto <see cref="TagHelperContent"/> que representa el contenido procesado del TagHelper.</returns>
    /// <remarks>
    /// Este método crea un nuevo contexto de TagHelper y un nuevo TagHelperOutput. 
    /// Luego, itera sobre los elementos secundarios y los procesa para generar el contenido final.
    /// </remarks>
    public TagHelperContent GetContent( IDictionary<object, object> items ) {
        TagHelperContent content = new DefaultTagHelperContent();
        _content.CopyTo( content );
        IDictionary<object, object> newItems = items.ToDictionary( t => t.Key, t => t.Value );
        var context = new TagHelperContext( _contextAttributes, newItems, Guid.NewGuid().ToString() );
        var output = new TagHelperOutput( "", _outputAttributes, ( useCachedResult, encoder ) => {
            foreach ( var child in _children )
                content.AppendHtml( child.GetContent( newItems ) );
            return Task.FromResult( content );
        } );
        _component.ProcessAsync( context, output );
        var result = new DefaultTagHelperContent();
        result.AppendHtml( output );
        return result;
    }
}