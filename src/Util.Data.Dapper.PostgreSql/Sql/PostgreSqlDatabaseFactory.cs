namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IDatabaseFactory"/> para la creación de instancias de bases de datos PostgreSQL.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar la conexión a una base de datos PostgreSQL, proporcionando métodos para abrir y cerrar conexiones.
/// </remarks>
public class PostgreSqlDatabaseFactory : IDatabaseFactory {
    /// <summary>
    /// Crea una instancia de <see cref="IDatabase"/> utilizando la cadena de conexión proporcionada.
    /// </summary>
    /// <param name="connection">La cadena de conexión que se utilizará para establecer la conexión a la base de datos.</param>
    /// <returns>Una nueva instancia de <see cref="IDatabase"/> que representa la conexión a la base de datos.</returns>
    public IDatabase Create( string connection ) {
        return new DefaultDatabase( new NpgsqlConnection( connection ) );
    }
}