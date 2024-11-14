using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para la ejecución de comandos SQL en un servidor SQL.
/// </summary>
/// <remarks>
/// Esta clase proporciona una implementación común para la ejecución de comandos SQL,
/// y debe ser heredada por clases concretas que implementen la lógica específica
/// para la interacción con un servidor SQL.
/// </remarks>
public abstract class SqlServerSqlExecutorBase : SqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerSqlExecutorBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar comandos SQL.</param>
    protected SqlServerSqlExecutorBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <summary>
    /// Crea una instancia de un constructor de SQL específico para SQL Server.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL.
    /// </returns>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new SqlServerBuilder();
    }

    /// <summary>
    /// Crea una instancia de <see cref="IExistsSqlBuilder"/> específica para SQL Server.
    /// </summary>
    /// <param name="sqlBuilder">El constructor de SQL que se utilizará para crear la instancia de <see cref="IExistsSqlBuilder"/>.</param>
    /// <returns>Una nueva instancia de <see cref="IExistsSqlBuilder"/> que utiliza el constructor de SQL proporcionado.</returns>
    /// <remarks>
    /// Este método es una anulación del método base y está diseñado para proporcionar una implementación específica para SQL Server.
    /// </remarks>
    /// <seealso cref="IExistsSqlBuilder"/>
    /// <seealso cref="SqlServerExistsSqlBuilder"/>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new SqlServerExistsSqlBuilder( sqlBuilder );
    }

    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="SqlServerDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new SqlServerDatabaseFactory();
    }
}