using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta que representa una consulta SQL específica para SQL Server.
/// Esta clase hereda de <see cref="SqlQueryBase"/> y proporciona funcionalidades
/// específicas para interactuar con bases de datos SQL Server.
/// </summary>
/// <remarks>
/// Esta clase no puede ser instanciada directamente. Debe ser heredada por otras clases
/// que implementen consultas SQL específicas para SQL Server.
/// </remarks>
public abstract class SqlServerSqlQueryBase : SqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="SqlServerSqlQueryBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar las consultas.</param>
    protected SqlServerSqlQueryBase(IServiceProvider serviceProvider, SqlOptions options, IDatabase database) : base(serviceProvider, options, database) { }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un constructor de SQL específico para SQL Server.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="ISqlBuilder"/> para construir consultas SQL.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto de la clase base.
    /// Se utiliza para proporcionar un constructor de SQL que se adapte a las características de SQL Server.
    /// </remarks>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new SqlServerBuilder();
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IExistsSqlBuilder"/> para la implementación de SQL Server.
    /// </summary>
    /// <param name="sqlBuilder">El constructor de SQL que se utilizará para crear la consulta EXISTS.</param>
    /// <returns>
    /// Una instancia de <see cref="IExistsSqlBuilder"/> que se utiliza para construir consultas EXISTS específicas de SQL Server.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación específica para SQL Server y se utiliza para 
    /// generar las consultas EXISTS de manera adecuada según las características de este sistema de gestión de bases de datos.
    /// </remarks>
    /// <seealso cref="IExistsSqlBuilder"/>
    /// <seealso cref="SqlServerExistsSqlBuilder"/>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new SqlServerExistsSqlBuilder( sqlBuilder );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/> para la conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="SqlServerDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto <see cref="CreateDatabaseFactory"/> 
    /// de la clase base, y se utiliza para proporcionar la lógica específica de creación de 
    /// la fábrica de base de datos para SQL Server.
    /// </remarks>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new SqlServerDatabaseFactory();
    }
}