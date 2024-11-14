using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para consultas SQL específicas de PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlQueryBase"/> y proporciona la funcionalidad
/// necesaria para ejecutar consultas en una base de datos PostgreSQL.
/// </remarks>
public abstract class PostgreSqlQueryBase : SqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlQueryBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar consultas.</param>
    protected PostgreSqlQueryBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un constructor SQL específico para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL en PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para proporcionar un constructor SQL que se adapte a las necesidades de la base de datos PostgreSQL.
    /// </remarks>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new PostgreSqlBuilder();
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de un constructor de SQL para verificar la existencia de registros.
    /// </summary>
    /// <param name="sqlBuilder">El constructor de SQL que se utilizará para crear la consulta.</param>
    /// <returns>
    /// Un objeto que implementa <see cref="IExistsSqlBuilder"/> para construir consultas de existencia en PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método está diseñado para ser sobreescrito en clases derivadas y proporciona una implementación específica
    /// para el sistema de base de datos PostgreSQL.
    /// </remarks>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new PostgreSqlExistsSqlBuilder( sqlBuilder );
    }

    /// <inheritdoc />
    /// <summary>
    /// Crea una instancia de la fábrica de bases de datos específica para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="PostgreSqlDatabaseFactory"/> que implementa <see cref="IDatabaseFactory"/>.
    /// </returns>
    /// <remarks>
    /// Este método anula el método base para proporcionar una implementación específica de la fábrica de bases de datos
    /// que se utilizará para interactuar con una base de datos PostgreSQL.
    /// </remarks>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new PostgreSqlDatabaseFactory();
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el procedimiento correspondiente a una cadena de texto dada.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento que se desea obtener.</param>
    /// <returns>
    /// Una cadena que representa el procedimiento, ya sea reemplazado por SQL o convertido a un resultado de tabla.
    /// </returns>
    /// <remarks>
    /// Este método verifica si la cadena de entrada es un procedimiento válido.
    /// Si es así, se reemplaza utilizando el dialecto SQL correspondiente.
    /// De lo contrario, se crea un nuevo objeto <see cref="TableItem"/> y se convierte a un resultado.
    /// </remarks>
    protected override string GetProcedure( string procedure ) {
        if ( IsProcedure( procedure ) )
            return Dialect.ReplaceSql( procedure );
        return new TableItem( Dialect, procedure ).ToResult();
    }

    /// <summary>
    /// Determina si la cadena proporcionada representa un procedimiento.
    /// </summary>
    /// <param name="procedure">La cadena que se va a evaluar como procedimiento.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena comienza con "call " (ignorando mayúsculas y minúsculas); de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected virtual bool IsProcedure( string procedure ) {
        return procedure.Trim().StartsWith( "call ", StringComparison.OrdinalIgnoreCase );
    }

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el tipo de comando para el procedimiento almacenado.
    /// </summary>
    /// <returns>
    /// Devuelve <see cref="CommandType.Text"/> si la consulta SQL es un procedimiento; de lo contrario, devuelve <see cref="CommandType.StoredProcedure"/>.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para determinar el tipo de comando que se debe utilizar al ejecutar una consulta SQL.
    /// Se basa en la evaluación de si la consulta SQL proporcionada es un procedimiento almacenado o no.
    /// </remarks>
    protected override CommandType GetProcedureCommandType() {
        if ( IsProcedure( GetSql() ) )
            return CommandType.Text;
        return CommandType.StoredProcedure;
    }
}