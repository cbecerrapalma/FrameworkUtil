using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql.Builders; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IExistsSqlBuilder"/> para construir consultas SQL 
/// específicas de Oracle que verifican la existencia de registros.
/// </summary>
public class OracleExistsSqlBuilder : IExistsSqlBuilder {
    private readonly ISqlBuilder _sqlBuilder;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleExistsSqlBuilder"/>.
    /// </summary>
    /// <param name="sqlBuilder">Una instancia de <see cref="ISqlBuilder"/> que se utilizará para construir la consulta SQL.</param>
    public OracleExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
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
    /// Inicializa la construcción de una consulta SQL para la selección de datos.
    /// </summary>
    /// <remarks>
    /// Este método limpia cualquier selección previa y establece una nueva selección
    /// que consiste en el valor constante "1". Esto puede ser útil para inicializar
    /// una consulta antes de agregar condiciones adicionales.
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
    private string GetResult() {
        var result = new StringBuilder();
        result.AppendLine( "Select Case" );
        result.AppendLine( "  When Exists (" );
        _sqlBuilder.AppendTo( result );
        result.AppendLine( ")" );
        result.AppendLine( "  Then 1" );
        result.AppendLine( "  Else 0 " );
        result.Append( "End" );
        return result.ToString();
    }
}