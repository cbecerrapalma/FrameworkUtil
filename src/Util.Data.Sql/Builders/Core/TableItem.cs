namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento de una tabla.
/// </summary>
public class TableItem {
    private readonly IDialect _dialect;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TableItem"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de la base de datos que se utilizará.</param>
    /// <param name="name">El nombre de la tabla que se resolverá.</param>
    public TableItem( IDialect dialect, string name ) {
        _dialect = dialect;
        Resolve( name );
    }

    /// <summary>
    /// Resuelve el nombre y asigna los valores correspondientes a las propiedades de la clase.
    /// </summary>
    /// <param name="name">El nombre que se va a resolver y del cual se extraerán el prefijo, el nombre y el alias.</param>
    private void Resolve( string name ) {
        var item = new NameItem( name );
        Schema = item.Prefix;
        Name = item.Name;
        TableAlias = item.Alias;
    }

    /// <summary>
    /// Obtiene o establece el esquema asociado.
    /// </summary>
    /// <remarks>
    /// El esquema puede ser utilizado para definir la estructura de los datos
    /// o para validar la conformidad de los mismos con un formato específico.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el esquema.
    /// </value>
    public string Schema { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de caracteres.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Obtiene o establece el alias de la tabla.
    /// </summary>
    /// <remarks>
    /// Este alias se utiliza para referirse a la tabla en consultas SQL o en operaciones de manipulación de datos.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el alias de la tabla.
    /// </value>
    public string TableAlias { get; set; }

    /// <summary>
    /// Valida si el nombre no es null, vacío o solo contiene espacios en blanco.
    /// </summary>
    /// <returns>
    /// Devuelve true si el nombre es válido; de lo contrario, devuelve false.
    /// </returns>
    public bool Validate() {
        return !string.IsNullOrWhiteSpace( Name );
    }

    /// <summary>
    /// Convierte el objeto actual en una representación de cadena.
    /// </summary>
    /// <returns>
    /// Una cadena que representa el resultado del objeto actual.
    /// </returns>
    public string ToResult() {
        var builder = new StringBuilder();
        AppendTo( builder );
        return builder.ToString();
    }

    /// <summary>
    /// Agrega información a un objeto <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le añadirá la información.</param>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en una clase derivada. 
    /// Llama a los métodos <see cref="AppendSchema"/>, <see cref="AppendTable"/> y <see cref="AppendTableAlias"/> 
    /// para construir la representación en cadena del objeto.
    /// </remarks>
    public virtual void AppendTo( StringBuilder builder ) {
        AppendSchema( builder );
        AppendTable( builder );
        AppendTableAlias( builder );
    }

    /// <summary>
    /// Agrega el esquema al <see cref="StringBuilder"/> proporcionado si no está vacío.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá el esquema.</param>
    /// <remarks>
    /// Este método verifica si el esquema está vacío antes de intentar agregarlo al 
    /// <paramref name="builder"/>. Si el esquema está vacío, no se realiza ninguna acción.
    /// </remarks>
    private void AppendSchema( StringBuilder builder ) {
        if ( Schema.IsEmpty() )
            return;
        builder.AppendFormat( "{0}.", _dialect.GetSafeName( Schema ) );
    }

    /// <summary>
    /// Agrega el nombre de la tabla al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá el nombre de la tabla.</param>
    /// <remarks>
    /// Si el nombre de la tabla está vacío, no se realizará ninguna acción.
    /// </remarks>
    private void AppendTable( StringBuilder builder ) {
        if( Name.IsEmpty() )
            return;
        builder.AppendFormat( _dialect.GetSafeName( Name ) );
    }

    /// <summary>
    /// Agrega un alias de tabla al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá el alias de tabla.</param>
    /// <remarks>
    /// Este método verifica si el alias de la tabla está vacío antes de intentar agregarlo.
    /// Si el alias está vacío, no se realiza ninguna acción.
    /// </remarks>
    private void AppendTableAlias( StringBuilder builder ) {
        if( TableAlias.IsEmpty() )
            return;
        builder.AppendFormat( " {0}", _dialect.GetSafeName( TableAlias ) );
    }
}