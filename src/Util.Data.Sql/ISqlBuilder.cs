using Util.Data.Sql.Builders;
using Util.Data.Sql.Builders.Operations;

namespace Util.Data.Sql; 

/// <summary>
/// Define una interfaz para construir consultas SQL.
/// </summary>
/// <remarks>
/// Esta interfaz hereda de <see cref="ISqlContent"/> y <see cref="ISqlOperation"/>,
/// lo que permite que las implementaciones manejen tanto el contenido SQL como las operaciones relacionadas.
/// </remarks>
public interface ISqlBuilder : ISqlContent, ISqlOperation {
    /// <summary>
    /// Crea una copia del objeto actual de tipo <see cref="ISqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/> que es una copia del objeto actual.
    /// </returns>
    /// <remarks>
    /// Este método permite duplicar la configuración y el estado del objeto actual,
    /// lo que puede ser útil para crear variaciones de consultas SQL sin modificar el original.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    ISqlBuilder Clone();
    /// <summary>
    /// Crea una nueva instancia de un objeto que implementa la interfaz <see cref="ISqlBuilder"/>.
    /// </summary>
    /// <returns>
    /// Una nueva instancia de <see cref="ISqlBuilder"/>.
    /// </returns>
    /// <remarks>
    /// Este método es útil para construir consultas SQL de manera programática.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    ISqlBuilder New();
    /// <summary>
    /// Limpia el estado actual del generador de consultas SQL.
    /// </summary>
    /// <returns>
    /// Una instancia del generador de consultas SQL actual, permitiendo la encadenación de métodos.
    /// </returns>
    /// <remarks>
    /// Este método restablece todos los parámetros y configuraciones del generador de consultas,
    /// permitiendo comenzar una nueva construcción de consulta desde cero.
    /// </remarks>
    /// <seealso cref="ISqlBuilder"/>
    ISqlBuilder Clear();
    /// <summary>
    /// Obtiene la cadena de consulta SQL.
    /// </summary>
    /// <returns>
    /// Una cadena que representa la consulta SQL.
    /// </returns>
    string GetSql();
}