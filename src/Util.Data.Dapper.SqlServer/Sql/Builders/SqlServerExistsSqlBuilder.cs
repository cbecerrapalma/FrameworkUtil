using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IExistsSqlBuilder"/> 
/// para construir consultas SQL que verifican la existencia de registros en una base de datos SQL Server.
/// </summary>
public class SqlServerExistsSqlBuilder : IExistsSqlBuilder {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerExistsSqlBuilder"/>.
    /// </summary>
    /// <param name="sqlBuilder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public SqlServerExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
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
    /// Inicializa la construcción de una consulta SQL para la selección.
    /// </summary>
    /// <remarks>
    /// Este método limpia cualquier selección previa y agrega una nueva selección que consiste en el valor "1".
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
    /// Este método utiliza un objeto <see cref="StringBuilder"/> para construir la consulta SQL.
    /// La consulta incluye una cláusula "Select Case" que evalúa si existe un resultado basado en la
    /// construcción SQL proporcionada por el método <see cref="_sqlBuilder.AppendTo"/>.
    /// Si existe, se devuelve 1 como un valor booleano (Bit), de lo contrario, se devuelve 0.
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