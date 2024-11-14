namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IDatabaseFactory"/> 
/// para crear instancias de conexión a una base de datos MySQL.
/// </summary>
public class MySqlDatabaseFactory : IDatabaseFactory {
    /// <summary>
    /// Crea una instancia de <see cref="IDatabase"/> utilizando la cadena de conexión proporcionada.
    /// </summary>
    /// <param name="connection">La cadena de conexión que se utilizará para establecer la conexión a la base de datos.</param>
    /// <returns>
    /// Una nueva instancia de <see cref="IDatabase"/> que representa la conexión a la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="MySqlConnection"/> para la conexión a una base de datos MySQL.
    /// </remarks>
    /// <seealso cref="IDatabase"/>
    /// <seealso cref="MySqlConnection"/>
    public IDatabase Create( string connection ) {
        return new DefaultDatabase( new MySqlConnection( connection ) );
    }
}