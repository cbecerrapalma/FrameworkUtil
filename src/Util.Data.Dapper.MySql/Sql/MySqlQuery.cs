using Util.Data.Sql;

namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Representa una consulta específica para la base de datos MySQL.
/// Esta clase hereda de <see cref="MySqlQueryBase"/> y proporciona funcionalidades
/// adicionales para la ejecución de consultas en MySQL.
/// </summary>
public class MySqlQuery : MySqlQueryBase {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MySqlQuery"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver dependencias.</param>
    /// <param name="options">Las opciones específicas para la consulta de MySQL.</param>
    /// <param name="database">La instancia de la base de datos, si se proporciona; de lo contrario, se utilizará la base de datos predeterminada.</param>
    public MySqlQuery( IServiceProvider serviceProvider, SqlOptions<MySqlQuery> options, IDatabase database = null )
        : base( serviceProvider, options, database ) {
    }
}