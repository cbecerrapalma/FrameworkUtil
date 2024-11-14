using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para la ejecución de comandos SQL en una base de datos MySQL.
/// Hereda de <see cref="SqlExecutorBase"/> y proporciona funcionalidades específicas para MySQL.
/// </summary>
public abstract class MySqlExecutorBase : SqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlExecutorBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar comandos.</param>
    protected MySqlExecutorBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <summary>
    /// Crea una instancia de un constructor de SQL específico.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// </returns>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new MySqlBuilder();
    }

    /// <summary>
    /// Crea una instancia de un generador de SQL para verificar la existencia de registros.
    /// </summary>
    /// <param name="sqlBuilder">El generador de SQL que se utilizará para construir la consulta.</param>
    /// <returns>Una instancia de <see cref="IExistsSqlBuilder"/> que permite construir consultas de existencia.</returns>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new MySqlExistsSqlBuilder( sqlBuilder );
    }

    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="MySqlDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new MySqlDatabaseFactory();
    }
}