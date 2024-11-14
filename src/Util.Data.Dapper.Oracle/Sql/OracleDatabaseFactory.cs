namespace Util.Data.Dapper.Sql; 

/// <summary>
/// Clase que implementa la interfaz <see cref="IDatabaseFactory"/> para la creación de instancias de bases de datos Oracle.
/// </summary>
/// <remarks>
/// Esta clase se encarga de gestionar la conexión a la base de datos Oracle y proporciona métodos para crear y administrar instancias de conexión.
/// </remarks>
public class OracleDatabaseFactory : IDatabaseFactory {
    /// <summary>
    /// Crea una instancia de <see cref="IDatabase"/> utilizando la cadena de conexión proporcionada.
    /// </summary>
    /// <param name="connection">La cadena de conexión que se utilizará para establecer la conexión a la base de datos.</param>
    /// <returns>Una instancia de <see cref="IDatabase"/> que representa la conexión a la base de datos.</returns>
    /// <remarks>
    /// Este método utiliza <see cref="OracleConnection"/> para crear la conexión a la base de datos.
    /// Asegúrese de que la cadena de conexión sea válida y que el proveedor de Oracle esté instalado.
    /// </remarks>
    /// <seealso cref="IDatabase"/>
    /// <seealso cref="OracleConnection"/>
    public IDatabase Create( string connection ) {
        var con = new OracleConnection( connection );
        return new DefaultDatabase( con );
    }
}