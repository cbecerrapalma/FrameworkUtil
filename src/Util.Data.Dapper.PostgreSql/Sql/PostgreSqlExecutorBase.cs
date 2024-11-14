using Util.Data.Dapper.Sql.Builders;
using Util.Data.Sql;
using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Core;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase base abstracta para la ejecución de comandos SQL en bases de datos PostgreSQL.
/// Hereda de <see cref="SqlExecutorBase"/> y proporciona funcionalidades específicas para PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase está diseñada para ser extendida por clases que implementen la lógica específica de ejecución de comandos SQL
/// en un contexto de base de datos PostgreSQL. Proporciona métodos y propiedades comunes que pueden ser utilizados
/// por las clases derivadas.
/// </remarks>
public abstract class PostgreSqlExecutorBase : SqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PostgreSqlExecutorBase"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración para la conexión a la base de datos.</param>
    /// <param name="database">La instancia de la base de datos que se utilizará para ejecutar las operaciones.</param>
    protected PostgreSqlExecutorBase( IServiceProvider serviceProvider, SqlOptions options, IDatabase database ) : base( serviceProvider, options, database ) {
    }

    /// <summary>
    /// Crea una instancia de un generador de SQL específico para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="ISqlBuilder"/> para construir consultas SQL en PostgreSQL.
    /// </returns>
    protected override ISqlBuilder CreateSqlBuilder() {
        return new PostgreSqlBuilder();
    }

    /// <summary>
    /// Crea una instancia de <see cref="IExistsSqlBuilder"/> para construir consultas SQL de existencia.
    /// </summary>
    /// <param name="sqlBuilder">El constructor SQL que se utilizará para crear la consulta de existencia.</param>
    /// <returns>
    /// Una instancia de <see cref="IExistsSqlBuilder"/> que se puede utilizar para construir consultas de existencia específicas de PostgreSQL.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica para PostgreSQL.
    /// </remarks>
    /// <seealso cref="IExistsSqlBuilder"/>
    /// <seealso cref="PostgreSqlExistsSqlBuilder"/>
    protected override IExistsSqlBuilder CreatExistsSqlBuilder( ISqlBuilder sqlBuilder ) {
        return new PostgreSqlExistsSqlBuilder( sqlBuilder );
    }

    /// <summary>
    /// Crea una instancia de la fábrica de bases de datos específica para PostgreSQL.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IDatabaseFactory"/> que representa la fábrica de bases de datos PostgreSQL.
    /// </returns>
    protected override IDatabaseFactory CreateDatabaseFactory() {
        return new PostgreSqlDatabaseFactory();
    }

    /// <summary>
    /// Obtiene el procedimiento SQL correspondiente al nombre proporcionado.
    /// </summary>
    /// <param name="procedure">El nombre del procedimiento SQL que se desea obtener.</param>
    /// <returns>
    /// Una cadena que representa el procedimiento SQL si existe; 
    /// de lo contrario, devuelve el resultado de un nuevo objeto <see cref="TableItem"/> 
    /// creado con el nombre del procedimiento proporcionado.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el procedimiento existe mediante el método <see cref="IsProcedure"/>.
    /// Si el procedimiento es válido, se reemplaza utilizando el método <see cref="Dialect.ReplaceSql"/>.
    /// En caso contrario, se crea un nuevo objeto <see cref="TableItem"/> y se convierte a resultado.
    /// </remarks>
    protected override string GetProcedure( string procedure ) {
        if ( IsProcedure( procedure ) )
            return Dialect.ReplaceSql( procedure );
        return new TableItem( Dialect, procedure ).ToResult();
    }

    /// <summary>
    /// Determina si la cadena proporcionada representa una llamada a un procedimiento.
    /// </summary>
    /// <param name="procedure">La cadena que se evaluará para determinar si es un procedimiento.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena comienza con "call " (ignorando mayúsculas y minúsculas); de lo contrario, devuelve <c>false</c>.
    /// </returns>
    protected virtual bool IsProcedure( string procedure ) {
        return procedure.Trim().StartsWith( "call ", StringComparison.OrdinalIgnoreCase );
    }

    /// <summary>
    /// Obtiene el tipo de comando para un procedimiento almacenado.
    /// </summary>
    /// <returns>
    /// Devuelve <see cref="CommandType.Text"/> si la consulta SQL es un procedimiento, 
    /// de lo contrario devuelve <see cref="CommandType.StoredProcedure"/>.
    /// </returns>
    protected override CommandType GetProcedureCommandType() {
        if ( IsProcedure( GetSql() ) )
            return CommandType.Text;
        return CommandType.StoredProcedure;
    }
}