namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IDatabaseFactory"/> para la creación de instancias de base de datos SQL Server.
/// </summary>
/// <remarks>
/// Esta clase se encarga de manejar la conexión y el contexto de la base de datos SQL Server,
/// proporcionando métodos para crear y gestionar la conexión a la base de datos.
/// </remarks>
public class SqlServerDatabaseFactory : IDatabaseFactory {
    /// <summary>
    /// Crea una instancia de <see cref="IDatabase"/> utilizando la cadena de conexión proporcionada.
    /// </summary>
    /// <param name="connection">La cadena de conexión que se utilizará para establecer la conexión a la base de datos.</param>
    /// <returns>Una nueva instancia de <see cref="IDatabase"/> configurada con la conexión especificada.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="SqlConnection"/> para conectarse a la base de datos.
    /// Asegúrese de que la cadena de conexión sea válida y que el servidor de base de datos esté accesible.
    /// </remarks>
    public IDatabase Create( string connection ) {
        return new DefaultDatabase( new SqlConnection( connection ) );
    }
}