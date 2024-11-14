using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Representa una consulta SQL específica para la base de datos Oracle.
/// Esta clase hereda de <see cref="OracleSqlQueryBase"/> y proporciona
/// funcionalidades adicionales para la ejecución de consultas SQL.
/// </summary>
public class OracleSqlQuery : OracleSqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleSqlQuery"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración específicas para la consulta SQL de Oracle.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará. Si es <c>null</c>, se utilizará la base de datos predeterminada.</param>
    public OracleSqlQuery( IServiceProvider serviceProvider, SqlOptions<OracleSqlQuery> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}