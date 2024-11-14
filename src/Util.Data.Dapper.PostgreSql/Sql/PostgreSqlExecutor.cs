using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que representa un ejecutor de comandos para bases de datos PostgreSQL.
/// Hereda de <see cref="PostgreSqlExecutorBase"/> para proporcionar funcionalidad específica.
/// </summary>
public class PostgreSqlExecutor : PostgreSqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlExecutor"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración específicas para el ejecutor de PostgreSQL.</param>
    /// <param name="database">La instancia de la base de datos, opcional. Si no se proporciona, se utilizará la base de datos predeterminada.</param>
    public PostgreSqlExecutor( IServiceProvider serviceProvider, SqlOptions<PostgreSqlExecutor> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}