using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para la ejecución de comandos SQL en una base de datos Oracle.
/// </summary>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para ejecutar comandos SQL y manejar transacciones
/// en una base de datos Oracle. Debe ser heredada por clases que implementen la lógica específica
/// para la ejecución de consultas y comandos.
/// </remarks>
public abstract class OracleSqlExecutorBase : SqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleSqlExecutorBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará.</param>
    protected OracleSqlExecutorBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <summary>
    /// Crea una instancia de un constructor de consultas SQL específico para Oracle.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL.
    /// </returns>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new OracleSqlBuilder();
    }

    /// <summary>
    /// Crea un constructor de consultas SQL para verificar la existencia de registros.
    /// </summary>
    /// <param name="sqlBuilder">El constructor SQL que se utilizará para crear la consulta.</param>
    /// <returns>Un objeto que implementa <see cref="IExistsSqlBuilder"/> para construir consultas de existencia.</returns>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new OracleExistsSqlBuilder( sqlBuilder );
    }

    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/> específica para Oracle.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="OracleDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new OracleDatabaseFactory();
    }
}