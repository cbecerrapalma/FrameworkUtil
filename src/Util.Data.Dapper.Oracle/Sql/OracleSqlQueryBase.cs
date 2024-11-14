using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para consultas SQL específicas de Oracle.
/// </summary>
/// <remarks>
/// Esta clase proporciona una estructura básica para construir consultas SQL 
/// que se ejecutarán en una base de datos Oracle. Hereda de <see cref="SqlQueryBase"/> 
/// y puede ser extendida para implementar consultas específicas.
/// </remarks>
public abstract class OracleSqlQueryBase : SqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleSqlQueryBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración para la consulta SQL.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar las consultas.</param>
    protected OracleSqlQueryBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un generador de SQL específico para Oracle.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL específicas de Oracle.
    /// </returns>
    /// <remarks>
    /// Este método se sobrescribe para proporcionar una implementación específica de <see cref="ISqlBuilder"/> 
    /// que es adecuada para trabajar con bases de datos Oracle.
    /// </remarks>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new OracleSqlBuilder();
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IExistsSqlBuilder"/> para la construcción de consultas SQL 
    /// que verifican la existencia de registros en una base de datos Oracle.
    /// </summary>
    /// <param name="sqlBuilder">El constructor SQL que se utilizará para crear la consulta.</param>
    /// <returns>Una instancia de <see cref="IExistsSqlBuilder"/> configurada para Oracle.</returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto de la clase base, y se utiliza 
    /// para proporcionar una lógica específica para la creación de un constructor de SQL que 
    /// se adapte a la base de datos Oracle.
    /// </remarks>
    /// <seealso cref="IExistsSqlBuilder"/>
    /// <seealso cref="OracleExistsSqlBuilder"/>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new OracleExistsSqlBuilder( sqlBuilder );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de <see cref="IDatabaseFactory"/> específica para Oracle.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="OracleDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una implementación del método abstracto <see cref="CreateDatabaseFactory"/> 
    /// de la clase base, y se utiliza para proporcionar la lógica necesaria para crear una 
    /// fábrica de bases de datos específica para Oracle.
    /// </remarks>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new OracleDatabaseFactory();
    }
}