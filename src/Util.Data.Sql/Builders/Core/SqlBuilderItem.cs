namespace Util.Data.Sql.Builders.Core; 

/// <summary>
/// Representa un elemento de construcción de una consulta SQL.
/// </summary>
public class SqlBuilderItem {
    private readonly IDialect _dialect;
    private readonly ISqlBuilder _builder;
    private readonly string _alias;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlBuilderItem"/>.
    /// </summary>
    /// <param name="dialect">El dialecto de SQL que se utilizará para construir las consultas.</param>
    /// <param name="builder">El constructor de SQL que se utilizará para generar las instrucciones SQL.</param>
    /// <param name="alias">Un alias opcional para el elemento SQL. Puede ser nulo.</param>
    public SqlBuilderItem( IDialect dialect, ISqlBuilder builder, string alias = null ) {
        _dialect = dialect;
        _builder = builder;
        _alias = alias;
    }

    /// <summary>
    /// Agrega una representación de este objeto al <see cref="StringBuilder"/> proporcionado.
    /// </summary>
    /// <param name="builder">El <see cref="StringBuilder"/> al que se le agregará la representación.</param>
    /// <remarks>
    /// Este método envuelve la representación del objeto entre paréntesis y, si el alias no está vacío,
    /// lo agrega al final de la representación utilizando el formato " As {alias}".
    /// </remarks>
    /// <seealso cref="StringBuilder"/>
    /// <seealso cref="_dialect"/>
    /// <seealso cref="_alias"/>
    public void AppendTo( StringBuilder builder ) {
        builder.Append( "(" );
        _builder.AppendTo( builder );
        builder.Append( ")" );
        if ( _alias.IsEmpty() )
            return;
        builder.AppendFormat( " As {0}", _dialect.GetSafeName( _alias ) );
    }
}