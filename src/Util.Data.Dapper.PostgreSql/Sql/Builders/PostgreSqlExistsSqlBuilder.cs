using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IExistsSqlBuilder"/> 
/// para construir consultas SQL que verifican la existencia de registros en una base de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona métodos para crear sentencias SQL que determinan si 
/// ciertos registros existen en la base de datos, optimizando así las operaciones 
/// de verificación de existencia.
/// </remarks>
public class PostgreSqlExistsSqlBuilder : IExistsSqlBuilder {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlExistsSqlBuilder"/>.
    /// </summary>
    /// <param name="sqlBuilder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public PostgreSqlExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        _sqlBuilder = sqlBuilder;
    }

    /// <summary>
    /// Obtiene la cadena SQL generada a partir de la inicialización de la selección.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL generada.
    /// </returns>
    public string GetSql() {
        InitSelect();
        return GetResult();
    }

    /// <summary>
    /// Inicializa la selección en el constructor SQL.
    /// </summary>
    /// <remarks>
    /// Este método limpia cualquier selección previa y establece una nueva selección con el valor "1".
    /// </remarks>
    private void InitSelect() {
        _sqlBuilder.ClearSelect();
        _sqlBuilder.AppendSelect( "1" );
    }

    /// <summary>
    /// Genera una cadena que representa una consulta SQL utilizando una estructura de caso.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene la consulta SQL construida.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto StringBuilder para construir la consulta SQL de manera eficiente.
    /// Se espera que el método <see cref="_sqlBuilder.AppendTo"/> se encargue de agregar la parte necesaria de la consulta.
    /// </remarks>
    private string GetResult() {
        var result = new StringBuilder();
        result.AppendLine( "Select Case" );
        result.AppendLine( "  When Exists (" );
        _sqlBuilder.AppendTo( result );
        result.AppendLine( ")" );
        result.AppendLine( "  Then Cast(1 As Bit)" );
        result.AppendLine( "  Else Cast(0 As Bit) " );
        result.Append( "End" );
        return result.ToString();
    }
}