namespace Util.Helpers; 

/// <summary>
/// Representa una clase para manejar operaciones relacionadas con XML.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos y propiedades para manipular y procesar documentos XML.
/// </remarks>
public partial class Xml {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Xml"/> y carga un documento XML.
    /// </summary>
    /// <param name="xml">Una cadena que representa el contenido XML a cargar. Si es null, se utilizará un valor predeterminado.</param>
    /// <exception cref="ArgumentException">Se lanza cuando el contenido XML es inválido y no se puede obtener un elemento raíz.</exception>
    public Xml( string xml = null ) {
        Document = new XmlDocument();
        Document.LoadXml( GetXml( xml ) );
        Root = Document.DocumentElement;
        if( Root == null )
            throw new ArgumentException( nameof( xml ) );
    }

    /// <summary>
    /// Obtiene una cadena XML. Si la cadena proporcionada es nula o está vacía, 
    /// devuelve una cadena XML predeterminada.
    /// </summary>
    /// <param name="xml">La cadena XML a evaluar.</param>
    /// <returns>Una cadena XML válida. Si la entrada es nula o vacía, 
    /// se devuelve "<xml></xml".</returns>
    private string GetXml( string xml ) {
        return string.IsNullOrWhiteSpace( xml ) ? "<xml></xml>" : xml;
    }

    /// <summary>
    /// Obtiene el documento XML asociado.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al objeto <see cref="XmlDocument"/> que representa el documento XML.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="XmlDocument"/> que contiene el documento XML.
    /// </returns>
    public XmlDocument Document { get; }

    /// <summary>
    /// Obtiene el elemento raíz de un documento XML.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso al elemento raíz del XML, permitiendo 
    /// realizar operaciones de lectura sobre la estructura del documento.
    /// </remarks>
    /// <returns>
    /// Un objeto <see cref="XmlElement"/> que representa el elemento raíz.
    /// </returns>
    public XmlElement Root { get; }

    /// <summary>
    /// Agrega un nuevo nodo XML al nodo padre especificado.
    /// </summary>
    /// <param name="name">El nombre del nodo que se va a agregar.</param>
    /// <param name="value">El valor del nodo. Si no se proporciona, se asignará como nulo.</param>
    /// <param name="parent">El nodo padre al que se agregará el nuevo nodo. Si es nulo, se utilizará el nodo raíz.</param>
    /// <returns>El nodo XML agregado.</returns>
    public XmlNode AddNode( string name, object value = null, XmlNode parent = null ) {
        var node = CreateNode( name, value, XmlNodeType.Element );
        GetParent( parent ).AppendChild( node );
        return node;
    }

    /// <summary>
    /// Crea un nodo XML con el nombre y valor especificados.
    /// </summary>
    /// <param name="name">El nombre del nodo XML que se va a crear.</param>
    /// <param name="value">El valor que se asignará al nodo XML. Este valor se convertirá a una cadena segura.</param>
    /// <param name="type">El tipo de nodo XML que se va a crear.</param>
    /// <returns>Un objeto <see cref="XmlNode"/> que representa el nodo XML creado.</returns>
    /// <remarks>
    /// Este método utiliza el método <c>SafeString()</c> para asegurar que el valor proporcionado no sea nulo o vacío antes de asignarlo al nodo.
    /// Si el valor es nulo o vacío, el nodo se creará sin texto interno.
    /// </remarks>
    private XmlNode CreateNode( string name, object value, XmlNodeType type ) {
        var node = Document.CreateNode( type, name, string.Empty );
        if( string.IsNullOrWhiteSpace( value.SafeString() ) == false )
            node.InnerText = value.SafeString();
        return node;
    }

    /// <summary>
    /// Obtiene el nodo padre especificado o el nodo raíz si el nodo padre es nulo.
    /// </summary>
    /// <param name="parent">El nodo padre que se desea obtener. Si es nulo, se devolverá el nodo raíz.</param>
    /// <returns>El nodo padre especificado o el nodo raíz si el nodo padre es nulo.</returns>
    private XmlNode GetParent( XmlNode parent ) {
        if( parent == null )
            return Root;
        return parent;
    }

    /// <summary>
    /// Agrega un nodo CDATA a un nodo padre especificado o al nodo raíz si no se proporciona uno.
    /// </summary>
    /// <param name="value">El valor que se almacenará en el nodo CDATA.</param>
    /// <param name="parent">El nodo padre al que se añadirá el nodo CDATA. Si es null, se utilizará el nodo raíz.</param>
    /// <returns>El nodo CDATA recién creado.</returns>
    /// <remarks>
    /// Este método crea un nuevo nodo CDATA utilizando el valor proporcionado y lo anexa al nodo padre especificado.
    /// Si el nodo padre es null, se obtiene el nodo raíz como padre.
    /// </remarks>
    public XmlNode AddCDataNode( object value, XmlNode parent = null ) {
        var node = CreateNode( CreateId(), value, XmlNodeType.CDATA );
        GetParent( parent ).AppendChild( node );
        return node;
    }

    /// <summary>
    /// Crea un identificador único en forma de cadena.
    /// </summary>
    /// <returns>
    /// Un identificador único generado como una cadena sin guiones.
    /// </returns>
    private string CreateId() {
        return System.Guid.NewGuid().ToString( "N" );
    }

    /// <summary>
    /// Agrega un nodo CDATA a un nodo padre especificado.
    /// </summary>
    /// <param name="value">El valor que se incluirá en el nodo CDATA.</param>
    /// <param name="parentName">El nombre del nodo padre al que se añadirá el nodo CDATA.</param>
    /// <returns>El nodo CDATA que se ha agregado al nodo padre.</returns>
    public XmlNode AddCDataNode( object value, string parentName ) {
        var parent = CreateNode( parentName, null, XmlNodeType.Element );
        Root.AppendChild( parent );
        return AddCDataNode( value, parent );
    }

    /// <summary>
    /// Devuelve una representación en forma de cadena del objeto actual.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el contenido XML del documento asociado.
    /// </returns>
    public override string ToString() {
        return Document.OuterXml;
    }
}