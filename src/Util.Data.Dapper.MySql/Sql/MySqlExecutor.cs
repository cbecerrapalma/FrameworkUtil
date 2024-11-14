using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que representa un ejecutor de comandos MySQL.
/// Hereda de <see cref="MySqlExecutorBase"/> para proporcionar funcionalidades específicas de ejecución de comandos en una base de datos MySQL.
/// </summary>
public class MySqlExecutor : MySqlExecutorBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlExecutor"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    /// <param name="options">Las opciones de configuración específicas para el ejecutor de MySQL.</param>
    /// <param name="database">La instancia de la base de datos a utilizar. Si es <c>null</c>, se utilizará la base de datos predeterminada.</param>
    public MySqlExecutor( IServiceProvider serviceProvider, SqlOptions<MySqlExecutor> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}