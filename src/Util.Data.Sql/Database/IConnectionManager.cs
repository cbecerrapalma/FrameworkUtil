namespace Util.Data.Sql.Database; 

/// <summary>
/// Define una interfaz para gestionar conexiones.
/// </summary>
public interface IConnectionManager {
    /// <summary>
    /// Establece la conexión a la base de datos utilizando el objeto de conexión proporcionado.
    /// </summary>
    /// <param name="connection">El objeto de conexión a la base de datos que se va a establecer.</param>
    /// <remarks>
    /// Este método permite configurar la conexión a la base de datos que será utilizada por la aplicación.
    /// Asegúrese de que el objeto de conexión esté correctamente inicializado y configurado antes de llamar a este método.
    /// </remarks>
    void SetConnection(IDbConnection connection);
    /// <summary>
    /// Obtiene una conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa la interfaz <see cref="IDbConnection"/> que representa la conexión a la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método es responsable de establecer y devolver una conexión activa a la base de datos.
    /// Asegúrese de cerrar la conexión después de su uso para liberar los recursos.
    /// </remarks>
    IDbConnection GetConnection();
}