namespace Util.Data; 

/// <summary>
/// Define una interfaz para la creación de instancias de bases de datos.
/// </summary>
public interface IDatabaseFactory {
    /// <summary>
    /// Crea una instancia de un objeto que implementa la interfaz <see cref="IDatabase"/>.
    /// </summary>
    /// <param name="connection">La cadena de conexión que se utilizará para establecer la conexión con la base de datos.</param>
    /// <returns>Una instancia de <see cref="IDatabase"/> que representa la conexión a la base de datos.</returns>
    /// <remarks>
    /// Este método es responsable de inicializar la conexión a la base de datos utilizando la cadena de conexión proporcionada.
    /// Asegúrese de que la cadena de conexión sea válida antes de llamar a este método.
    /// </remarks>
    /// <seealso cref="IDatabase"/>
    IDatabase Create( string connection );
}