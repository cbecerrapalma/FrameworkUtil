namespace Util.Logging; 

/// <summary>
/// Define una interfaz para la creación de instancias de registros de log.
/// </summary>
public interface ILogFactory {
    /// <summary>
    /// Crea una instancia de un registro de log para la categoría especificada.
    /// </summary>
    /// <param name="categoryName">El nombre de la categoría para la cual se desea crear el log.</param>
    /// <returns>Una instancia de <see cref="ILog"/> que se puede utilizar para registrar información en la categoría especificada.</returns>
    /// <remarks>
    /// Este método es útil para organizar los registros de log en diferentes categorías, 
    /// lo que facilita la gestión y el análisis de los registros generados.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog CreateLog( string categoryName );
    /// <summary>
    /// Crea una instancia de un objeto que implementa la interfaz <see cref="ILog"/>.
    /// </summary>
    /// <param name="type">El tipo para el cual se está creando el registro de log.</param>
    /// <returns>Una instancia de <see cref="ILog"/> asociada al tipo especificado.</returns>
    /// <remarks>
    /// Este método es útil para obtener un logger que esté asociado a un tipo específico,
    /// permitiendo así que los mensajes de log sean categorizados según el contexto del tipo.
    /// </remarks>
    /// <seealso cref="ILog"/>
    ILog CreateLog( Type type );
}