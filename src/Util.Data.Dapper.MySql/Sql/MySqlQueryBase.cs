using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para consultas SQL específicas de MySQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlQueryBase"/> y proporciona 
/// funcionalidades específicas para la ejecución de consultas en 
/// bases de datos MySQL.
/// </remarks>
public abstract class MySqlQueryBase : SqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlQueryBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar las consultas.</param>
    protected MySqlQueryBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un generador de SQL específico para MySQL.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto <see cref="CreateSqlBuilder"/> 
    /// de la clase base. Se utiliza para proporcionar un generador de SQL adecuado para 
    /// la base de datos MySQL.
    /// </remarks>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new MySqlBuilder();
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un constructor de SQL para verificar la existencia de registros.
    /// </summary>
    /// <param name="sqlBuilder">El constructor de SQL que se utilizará para construir la consulta.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IExistsSqlBuilder"/> que permite construir consultas para verificar la existencia de registros.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación de un método base que permite personalizar la creación del constructor de SQL
    /// para la verificación de existencia, en este caso, utilizando <see cref="MySqlExistsSqlBuilder"/>.
    /// </remarks>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new MySqlExistsSqlBuilder( sqlBuilder );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="MySqlDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto <see cref="CreateDatabaseFactory"/> 
    /// de la clase base, y se utiliza para proporcionar una fábrica de base de datos específica.
    /// </remarks>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new MySqlDatabaseFactory();
    }
}