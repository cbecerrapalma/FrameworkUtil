using Util.Data.Metadata;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Representa una fábrica de convertidores de tipos.
/// </summary>
/// <remarks>
/// Esta clase es responsable de crear instancias de convertidores de tipos
/// que pueden convertir entre diferentes tipos de datos.
/// </remarks>
public class TypeConverterFactory : ITypeConverterFactory {
    /// <summary>
    /// Crea un convertidor de tipos basado en el tipo de base de datos especificado.
    /// </summary>
    /// <param name="dbType">El tipo de base de datos para el cual se desea crear el convertidor.</param>
    /// <returns>Un objeto que implementa la interfaz <see cref="ITypeConverter"/> correspondiente al tipo de base de datos.</returns>
    /// <remarks>
    /// Este método soporta los siguientes tipos de base de datos:
    /// <list type="bullet">
    /// <item><description><see cref="DatabaseType.SqlServer"/> - Crea un convertidor para SQL Server.</description></item>
    /// <item><description><see cref="DatabaseType.PgSql"/> - Crea un convertidor para PostgreSQL.</description></item>
    /// <item><description><see cref="DatabaseType.MySql"/> - Crea un convertidor para MySQL.</description></item>
    /// </list>
    /// Si se proporciona un tipo de base de datos no soportado, se lanzará una excepción <see cref="NotImplementedException"/>.
    /// </remarks>
    /// <exception cref="NotImplementedException">
    /// Se lanza cuando el tipo de base de datos proporcionado no está implementado.
    /// </exception>
    public ITypeConverter Create( DatabaseType dbType ) {
        switch ( dbType ) {
            case DatabaseType.SqlServer:
                return new SqlServerTypeConverter();
            case DatabaseType.PgSql:
                return new PostgreSqlTypeConverter();
            case DatabaseType.MySql:
                return new MySqlTypeConverter();
        }
        throw new NotImplementedException();
    }
}