using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Representa una consulta SQL específica para SQL Server.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlServerSqlQueryBase"/> y proporciona funcionalidades específicas
/// para ejecutar consultas en una base de datos SQL Server.
/// </remarks>
public class SqlServerSqlQuery : SqlServerSqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerSqlQuery"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones específicas para la consulta SQL en un servidor SQL.</param>
    /// <param name="database">La base de datos en la que se ejecutará la consulta. Si es <c>null</c>, se utilizará la base de datos predeterminada.</param>
    public SqlServerSqlQuery( IServiceProvider serviceProvider, SqlOptions<SqlServerSqlQuery> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}