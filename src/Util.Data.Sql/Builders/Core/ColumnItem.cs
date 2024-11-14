namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento de columna en una estructura de datos.
/// </summary>
public class ColumnItem {
    private readonly IDialect _dialect;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ColumnItem"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de la base de datos que se utilizará.</param>
    /// <param name="name">El nombre de la columna que se resolverá.</param>
    public ColumnItem( IDialect dialect, string name ) {
        _dialect = dialect;
        Resolve( name );
    }

    /// <summary>
    /// Resuelve un nombre y asigna los valores correspondientes a las propiedades de la clase.
    /// </summary>
    /// <param name="name">El nombre que se va a resolver.</param>
    private void Resolve( string name ) {
        var item = new NameItem( name );
        TableAlias = item.Prefix;
        Name = item.Name;
        ColumnAlias = item.Alias;
    }

    /// <summary>
    /// Obtiene o establece el alias de la tabla.
    /// </summary>
    /// <remarks>
    /// Este campo se utiliza para referirse a la tabla en consultas SQL o en la construcción de 
    /// expresiones de consulta, permitiendo que el alias sea utilizado en lugar del nombre completo 
    /// de la tabla.
    /// </remarks>
    /// <value>
    /// Un string que representa el alias de la tabla.
    /// </value>
    public string TableAlias { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre.
    /// </summary>
    /// <value>
    /// El nombre como una cadena de caracteres.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Obtiene o establece el alias de la columna.
    /// </summary>
    /// <value>
    /// Un <see cref="string"/> que representa el alias de la columna.
    /// </value>
    public string ColumnAlias { get; set; }

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
    /// Agrega información a un objeto <see cref="StringBuilder"/> 
    /// mediante la adición de un alias de tabla, una columna y un alias de columna.
    /// </summary>
    /// <param name="builder">El objeto <see cref="StringBuilder"/> al que se le agregarán los datos.</param>
    public void AppendTo( StringBuilder builder ) {
        AppendTableAlias( builder );
        AppendColumn( builder );
        AppendColumnAlias( builder );
    }

    /// <summary>
    /// Agrega un alias de tabla al <see cref="StringBuilder"/> especificado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá el alias de tabla.</param>
    /// <remarks>
    /// Si el alias de tabla está vacío, no se realiza ninguna acción.
    /// </remarks>
    private void AppendTableAlias( StringBuilder builder ) {
        if ( TableAlias.IsEmpty() )
            return;
        builder.AppendFormat( "{0}.", _dialect.GetSafeName( TableAlias ) );
    }

    /// <summary>
    /// Agrega el nombre de la columna al objeto StringBuilder proporcionado.
    /// </summary>
    /// <param name="builder">El objeto StringBuilder al que se le añadirá el nombre de la columna.</param>
    /// <remarks>
    /// Este método verifica si el nombre de la columna está vacío antes de intentar agregarlo al StringBuilder.
    /// Si el nombre está vacío, el método no realiza ninguna acción.
    /// </remarks>
    private void AppendColumn( StringBuilder builder ) {
        if( Name.IsEmpty() )
            return;
        builder.AppendFormat( _dialect.GetSafeName( Name ) );
    }

    /// <summary>
    /// Agrega un alias de columna al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le añadirá el alias de columna.</param>
    /// <remarks>
    /// Si el alias de columna está vacío, no se realiza ninguna acción.
    /// </remarks>
    private void AppendColumnAlias( StringBuilder builder ) {
        if( ColumnAlias.IsEmpty() )
            return;
        builder.AppendFormat( " As {0}", _dialect.GetSafeName( ColumnAlias ) );
    }
}