using Util.Ui.Extensions;

namespace Util.Ui.Configs; 

/// <summary>
/// Representa la configuración del sistema.
/// </summary>
public class Config {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Config"/>.
    /// Este constructor llama al constructor de la clase base con parámetros nulos.
    /// </summary>
    /// <remarks>
    /// Este constructor es útil para crear un objeto <see cref="Config"/> sin proporcionar valores iniciales.
    /// </remarks>
    public Config() : this( null, null ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Config"/>.
    /// </summary>
    /// <param name="context">El contexto del TagHelper que proporciona información sobre el entorno de ejecución.</param>
    /// <param name="output">El objeto de salida que representa el contenido HTML que se generará.</param>
    /// <remarks>
    /// Este constructor permite crear una instancia de <see cref="Config"/> utilizando el contexto y la salida proporcionados.
    /// </remarks>
    public Config( TagHelperContext context, TagHelperOutput output ) : this( context, output, null ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Config"/>.
    /// </summary>
    /// <param name="context">El contexto del TagHelper que contiene información sobre el estado y los atributos del TagHelper.</param>
    /// <param name="output">El objeto de salida que representa el contenido HTML generado por el TagHelper.</param>
    /// <param name="content">El contenido del TagHelper que se va a procesar.</param>
    /// <remarks>
    /// Si el contexto proporcionado es nulo, se crea un nuevo <see cref="TagHelperContext"/> con una lista de atributos vacía y un nuevo identificador único.
    /// Si el objeto de salida es nulo, se crea un nuevo <see cref="TagHelperOutput"/> con un nombre vacío y una lista de atributos vacía.
    /// </remarks>
    public Config( TagHelperContext context, TagHelperOutput output, TagHelperContent content ) {
        Id = context?.UniqueId;
        context ??= new TagHelperContext( new TagHelperAttributeList(), new Dictionary<object, object>(), Guid.NewGuid().ToString() );
        Output = output ?? new TagHelperOutput( "", new TagHelperAttributeList(), ( useCachedResult, encoder ) => Task.FromResult( content ) );
        Content = content;
        AllAttributes = new TagHelperAttributeList( context.AllAttributes );
        OutputAttributes = new TagHelperAttributeList( Output.Attributes );
        Context = new TagHelperContext( AllAttributes, context.Items, context.UniqueId );
    }

    /// <summary>
    /// Obtiene o establece el identificador.
    /// </summary>
    /// <value>
    /// Un string que representa el identificador.
    /// </value>
    public string Id { get; set; }

    /// <summary>
    /// Obtiene o establece el contexto del TagHelper.
    /// </summary>
    /// <remarks>
    /// El contexto del TagHelper proporciona información sobre el estado actual del procesamiento de la etiqueta,
    /// incluyendo información sobre los atributos y el contenido de la etiqueta.
    /// </remarks>
    protected TagHelperContext Context { get; set; }

    /// <summary>
    /// Obtiene o establece la salida del TagHelper.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para manipular la salida HTML generada por el TagHelper.
    /// </remarks>
    /// <value>
    /// Un objeto de tipo <see cref="TagHelperOutput"/> que representa la salida del TagHelper.
    /// </value>
    protected TagHelperOutput Output { get; set; }

    /// <summary>
    /// Obtiene o establece el contenido del TagHelper.
    /// </summary>
    /// <value>
    /// Un objeto <see cref="TagHelperContent"/> que representa el contenido del TagHelper.
    /// </value>
    public TagHelperContent Content { get; set; }

    /// <summary>
    /// Obtiene o establece la lista de atributos de la etiqueta.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite acceder y modificar todos los atributos asociados a una etiqueta en un contexto de TagHelper.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="TagHelperAttributeList"/> que contiene los atributos de la etiqueta.
    /// </value>
    public TagHelperAttributeList AllAttributes { get; set; }

    /// <summary>
    /// Obtiene o establece la lista de atributos de salida.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar y gestionar los atributos que se generarán en la salida de un Tag Helper.
    /// </remarks>
    /// <value>
    /// Una instancia de <see cref="TagHelperAttributeList"/> que contiene los atributos de salida.
    /// </value>
    public TagHelperAttributeList OutputAttributes { get; set; }

    /// <summary>
    /// Determina si el conjunto de atributos contiene un atributo con el nombre especificado.
    /// </summary>
    /// <param name="name">El nombre del atributo que se desea buscar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el atributo con el nombre especificado se encuentra en el conjunto de atributos; de lo contrario, <c>false</c>.
    /// </returns>
    public bool Contains( string name ) {
        return AllAttributes.ContainsName( name );
    }

    /// <summary>
    /// Obtiene el valor de un atributo dado su nombre.
    /// </summary>
    /// <param name="name">El nombre del atributo cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor del atributo si existe; de lo contrario, una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el atributo existe en la colección de atributos antes de intentar acceder a su valor.
    /// Si el atributo no se encuentra, se devuelve una cadena vacía.
    /// </remarks>
    public string GetValue( string name ) {
        return Contains( name ) ? AllAttributes[name].Value.SafeString() : string.Empty;
    }

    /// <summary>
    /// Obtiene el valor de un atributo especificado por su nombre y lo convierte al tipo indicado.
    /// </summary>
    /// <typeparam name="T">El tipo al que se desea convertir el valor del atributo.</typeparam>
    /// <param name="name">El nombre del atributo cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor del atributo convertido al tipo especificado si el atributo existe; de lo contrario, devuelve el valor predeterminado de tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el atributo existe antes de intentar convertir su valor. 
    /// Si el atributo no se encuentra, se devuelve el valor predeterminado del tipo especificado.
    /// </remarks>
    /// <seealso cref="Contains(string)"/>
    public T GetValue<T>( string name ) {
        return Contains( name ) ? Util.Helpers.Convert.To<T>( AllAttributes[name].Value ) : default;
    }

    /// <summary>
    /// Obtiene el valor de una cadena en formato booleano a partir de un nombre dado.
    /// </summary>
    /// <param name="name">El nombre del valor que se desea obtener.</param>
    /// <returns>
    /// Una cadena que representa el valor booleano en formato de texto, 
    /// donde la primera letra es en minúscula.
    /// </returns>
    public string GetBoolValue( string name ) {
        return Util.Helpers.String.FirstLowerCase( GetValue( name ) );
    }

    /// <summary>
    /// Establece un atributo con el nombre y valor especificados.
    /// </summary>
    /// <param name="name">El nombre del atributo que se va a establecer.</param>
    /// <param name="value">El valor del atributo que se va a establecer.</param>
    /// <param name="replaceExisting">Indica si se debe reemplazar un atributo existente con el mismo nombre. El valor predeterminado es true.</param>
    /// <remarks>
    /// Si <paramref name="replaceExisting"/> es false y el atributo ya existe, no se realizará ninguna acción.
    /// </remarks>
    public void SetAttribute( string name, object value, bool replaceExisting = true ) {
        if ( replaceExisting == false && Contains( name ) )
            return;
        AllAttributes.SetAttribute( new TagHelperAttribute( name, value ) );
    }

    /// <summary>
    /// Elimina un atributo de la colección de atributos.
    /// </summary>
    /// <param name="name">El nombre del atributo que se desea eliminar.</param>
    /// <remarks>
    /// Este método busca y elimina todos los atributos que coinciden con el nombre proporcionado
    /// de la colección de atributos. Si no se encuentra ningún atributo con el nombre especificado,
    /// la colección permanecerá sin cambios.
    /// </remarks>
    public void RemoveAttribute( string name ) {
        AllAttributes.RemoveAll( name );
    }

    /// <summary>
    /// Obtiene un valor de los elementos utilizando una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave utilizada para buscar el valor. Si es null, se utilizará un comportamiento predeterminado.</param>
    /// <returns>El valor del tipo especificado asociado a la clave proporcionada.</returns>
    /// <remarks>
    /// Este método delega la llamada al método <see cref="Context.GetValueFromItems{T}(object)"/> 
    /// para recuperar el valor correspondiente a la clave.
    /// </remarks>
    public T GetValueFromItems<T>( object key = null ) {
        return Context.GetValueFromItems<T>( key );
    }

    /// <summary>
    /// Establece un valor para los elementos en el contexto.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se va a establecer.</typeparam>
    /// <param name="value">El valor que se asignará a los elementos.</param>
    public void SetValueToItems<T>( T value ) {
        Context.SetValueToItems( value );
    }

    /// <summary>
    /// Obtiene un valor de los atributos del contexto utilizando una clave especificada.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se desea obtener.</typeparam>
    /// <param name="key">La clave del atributo cuyo valor se desea recuperar.</param>
    /// <returns>
    /// El valor del atributo correspondiente a la clave especificada, o el valor predeterminado del tipo <typeparamref name="T"/> si el contexto es nulo.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el contexto es nulo antes de intentar recuperar el valor. 
    /// Si el contexto es nulo, se devuelve el valor predeterminado del tipo especificado.
    /// </remarks>
    /// <seealso cref="Context"/>
    public T GetValueFromAttributes<T>( string key ) {
        if ( Context == null )
            return default( T );
        return Context.GetValueFromAttributes<T>( key );
    }

    /// <summary>
    /// Obtiene el valor asociado a la clave especificada desde los atributos.
    /// </summary>
    /// <param name="key">La clave del atributo cuyo valor se desea obtener.</param>
    /// <returns>El valor del atributo asociado a la clave especificada.</returns>
    /// <remarks>
    /// Este método es una sobrecarga que llama a la versión genérica del método 
    /// <see cref="GetValueFromAttributes{T}(string)"/> utilizando el tipo de dato string.
    /// </remarks>
    public string GetValueFromAttributes( string key ) {
        return GetValueFromAttributes<string>( key );
    }

    /// <summary>
    /// Crea una copia de la configuración actual.
    /// </summary>
    /// <returns>
    /// Un nuevo objeto <see cref="Config"/> que es una copia de la configuración actual.
    /// </returns>
    /// <remarks>
    /// Este método clona los atributos y el contenido de la configuración actual,
    /// creando un nuevo contexto y listas de atributos para la nueva instancia.
    /// </remarks>
    public Config Copy() {
        var content = new DefaultTagHelperContent();
        Content?.CopyTo( content );
        return new Config {
            Context = new TagHelperContext( AllAttributes, Context.Items, Util.Helpers.Id.Create() ),
            AllAttributes = new TagHelperAttributeList( AllAttributes ),
            OutputAttributes = new TagHelperAttributeList( OutputAttributes ),
            Content = content
        };
    }
}