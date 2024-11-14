namespace Util.Data; 

/// <summary>
/// Representa una implementación por defecto de la interfaz <see cref="IDatabase"/>.
/// </summary>
/// <remarks>
/// Esta clase proporciona la funcionalidad básica para interactuar con una base de datos.
/// Puede ser extendida o modificada según las necesidades específicas de la aplicación.
/// </remarks>
public class DefaultDatabase : IDatabase {
    private readonly IDbConnection _connection;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DefaultDatabase"/>.
    /// </summary>
    /// <param name="connection">La conexión a la base de datos que se utilizará.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="connection"/> es <c>null</c>.</exception>
    public DefaultDatabase( IDbConnection connection ) {
        _connection = connection ?? throw new ArgumentNullException( nameof(connection) );
    }

    /// <summary>
    /// Obtiene la conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Una instancia de <see cref="IDbConnection"/> que representa la conexión a la base de datos.
    /// </returns>
    public IDbConnection GetConnection() {
        return _connection;
    }
}