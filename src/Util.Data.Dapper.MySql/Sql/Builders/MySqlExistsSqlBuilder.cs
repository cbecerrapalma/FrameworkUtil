using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IExistsSqlBuilder"/> para construir consultas SQL
/// que verifican la existencia de registros en una base de datos MySQL.
/// </summary>
public class MySqlExistsSqlBuilder : IExistsSqlBuilder {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlExistsSqlBuilder"/>.
    /// </summary>
    /// <param name="sqlBuilder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public MySqlExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        _sqlBuilder = sqlBuilder;
    }

    /// <summary>
    /// Obtiene una cadena SQL generada a partir de la inicialización de la selección.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL generada.
    /// </returns>
    public string GetSql() {
        InitSelect();
        return GetResult();
    }

    /// <summary>
    /// Inicializa la selección de la consulta SQL.
    /// </summary>
    /// <remarks>
    /// Este método limpia cualquier selección previa y establece una nueva selección que consiste en el valor "1".
    /// </remarks>
    private void InitSelect() {
        _sqlBuilder.ClearSelect();
        _sqlBuilder.AppendSelect( "1" );
    }

    /// <summary>
    /// Genera una cadena que representa una consulta SQL utilizando una estructura de caso.
    /// </summary>
    /// <returns>
    /// Una cadena que contiene la consulta SQL generada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un objeto StringBuilder para construir la consulta SQL.
    /// La consulta comienza con "Select Case", seguido de una verificación de existencia,
    /// y finaliza con "End". El contenido de la verificación se construye a partir
    /// del método <see cref="_sqlBuilder.AppendTo"/>.
    /// </remarks>
    private string GetResult() {
        var result = new StringBuilder();
        result.AppendLine( "Select Case" );
        result.AppendLine( "  When Exists (" );
        _sqlBuilder.AppendTo( result );
        result.AppendLine( ")" );
        result.AppendLine( "  Then true" );
        result.AppendLine( "  Else false " );
        result.Append( "End" );
        return result.ToString();
    }
}