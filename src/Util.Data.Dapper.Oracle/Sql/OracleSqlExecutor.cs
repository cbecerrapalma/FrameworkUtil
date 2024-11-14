using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que representa un ejecutor de comandos SQL para bases de datos Oracle.
/// Hereda de <see cref="OracleSqlExecutorBase"/> para proporcionar funcionalidad específica.
/// </summary>
public class OracleSqlExecutor : OracleSqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OracleSqlExecutor"/>.
    /// </summary>
    /// <param name="serviceProvider">Proporciona acceso a los servicios requeridos por el ejecutor SQL.</param>
    /// <param name="options">Opciones específicas para la configuración del ejecutor SQL.</param>
    /// <param name="database">Instancia opcional de <see cref="IDatabase"/> que representa la base de datos a utilizar. Si es null, se utilizará la base de datos predeterminada.</param>
    public OracleSqlExecutor( IServiceProvider serviceProvider, SqlOptions<OracleSqlExecutor> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}