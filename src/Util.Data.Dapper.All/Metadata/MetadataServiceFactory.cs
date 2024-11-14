using Util.Data.Dapper.Sql;
using Util.Data.Metadata;
using Util.Data.Sql;

namespace Util.Data.Dapper.Metadata; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IMetadataServiceFactory"/>.
/// Proporciona métodos para crear instancias de servicios de metadatos.
/// </summary>
public class MetadataServiceFactory : IMetadataServiceFactory {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MetadataServiceFactory"/>.
    /// </summary>
    /// <param name="serviceProvider">El proveedor de servicios que se utilizará para resolver las dependencias.</param>
    public MetadataServiceFactory( IServiceProvider serviceProvider ) {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Crea una instancia de un servicio de metadatos basado en el tipo de base de datos especificado.
    /// </summary>
    /// <param name="dbType">El tipo de base de datos para el cual se debe crear el servicio de metadatos.</param>
    /// <param name="connection">La cadena de conexión utilizada para conectarse a la base de datos.</param>
    /// <returns>Una instancia de <see cref="IMetadataService"/> correspondiente al tipo de base de datos especificado.</returns>
    /// <exception cref="NotImplementedException">Se lanza si el tipo de base de datos no está implementado.</exception>
    public IMetadataService Create( DatabaseType dbType,string connection ) {
        switch ( dbType ) {
            case DatabaseType.SqlServer:
                return CreateSqlServerMetadataService( connection );
            case DatabaseType.PgSql:
                return CreatePgSqlMetadataService( connection );
            case DatabaseType.MySql:
                return CreateMySqlMetadataService( connection );
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// Crea una instancia de <see cref="IMetadataService"/> utilizando una conexión a SQL Server.
    /// </summary>
    /// <param name="connection">La cadena de conexión a la base de datos SQL Server.</param>
    /// <returns>
    /// Una instancia de <see cref="IMetadataService"/> configurada con la conexión proporcionada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="SqlOptions{T}"/> para establecer las opciones de conexión 
    /// y crea un objeto <see cref="SqlServerSqlQuery"/> que se utiliza para interactuar 
    /// con la base de datos SQL Server.
    /// </remarks>
    /// <seealso cref="IMetadataService"/>
    /// <seealso cref="SqlOptions{T}"/>
    /// <seealso cref="SqlServerSqlQuery"/>
    /// <seealso cref="SqlServerMetadataService"/>
    private IMetadataService CreateSqlServerMetadataService( string connection ) {
        var options = new SqlOptions<SqlServerSqlQuery> {
            ConnectionString = connection
        };
        var sqlQuery = new SqlServerSqlQuery( _serviceProvider, options );
        return new SqlServerMetadataService( sqlQuery );
    }

    /// <summary>
    /// Crea una instancia de <see cref="IMetadataService"/> para la base de datos PostgreSQL.
    /// </summary>
    /// <param name="connection">La cadena de conexión a la base de datos PostgreSQL.</param>
    /// <returns>Una instancia de <see cref="IMetadataService"/> configurada para interactuar con la base de datos PostgreSQL.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="SqlOptions{T}"/> para configurar las opciones de conexión y 
    /// <see cref="PostgreSqlQuery"/> para ejecutar consultas en la base de datos.
    /// </remarks>
    /// <seealso cref="IMetadataService"/>
    /// <seealso cref="PostgreSqlQuery"/>
    /// <seealso cref="PostgreSqlMetadataService"/>
    private IMetadataService CreatePgSqlMetadataService( string connection ) {
        var options = new SqlOptions<PostgreSqlQuery> {
            ConnectionString = connection
        };
        var sqlQuery = new PostgreSqlQuery( _serviceProvider, options );
        return new PostgreSqlMetadataService( sqlQuery );
    }

    /// <summary>
    /// Crea una instancia de <see cref="IMetadataService"/> utilizando una conexión a MySQL.
    /// </summary>
    /// <param name="connection">La cadena de conexión a la base de datos MySQL.</param>
    /// <returns>Una instancia de <see cref="IMetadataService"/> configurada para interactuar con MySQL.</returns>
    /// <remarks>
    /// Este método configura las opciones necesarias para realizar consultas a la base de datos MySQL
    /// y devuelve un servicio de metadatos que puede ser utilizado para acceder a la información
    /// almacenada en la base de datos.
    /// </remarks>
    /// <seealso cref="IMetadataService"/>
    /// <seealso cref="MySqlQuery"/>
    /// <seealso cref="MySqlMetadataService"/>
    private IMetadataService CreateMySqlMetadataService( string connection ) {
        var options = new SqlOptions<MySqlQuery> {
            ConnectionString = connection
        };
        var sqlQuery = new MySqlQuery( _serviceProvider, options );
        return new MySqlMetadataService( sqlQuery );
    }
}