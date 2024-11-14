using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Representa una consulta específica para la base de datos PostgreSQL.
/// Esta clase hereda de <see cref="PostgreSqlQueryBase"/> y proporciona
/// funcionalidades adicionales para la ejecución de consultas.
/// </summary>
public class PostgreSqlQuery : PostgreSqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlQuery"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para la inyección de dependencias.</param>
    /// <param name="options">Las opciones específicas para la consulta de PostgreSQL.</param>
    /// <param name="database">La instancia de la base de datos, si se proporciona; de lo contrario, se utilizará la base de datos predeterminada.</param>
    public PostgreSqlQuery( IServiceProvider serviceProvider, SqlOptions<PostgreSqlQuery> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}