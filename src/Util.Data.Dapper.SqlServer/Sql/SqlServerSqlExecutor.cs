using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que ejecuta comandos SQL en un servidor SQL Server.
/// Hereda de <see cref="SqlServerSqlExecutorBase"/>.
/// </summary>
public class SqlServerSqlExecutor : SqlServerSqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerSqlExecutor"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones específicas para el ejecutor SQL de SQL Server.</param>
    /// <param name="database">La instancia de la base de datos a utilizar. Si es <c>null</c>, se utilizará la base de datos predeterminada.</param>
    public SqlServerSqlExecutor( IServiceProvider serviceProvider, SqlOptions<SqlServerSqlExecutor> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}