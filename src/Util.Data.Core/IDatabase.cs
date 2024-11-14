using Util.Aop;

namespace Util.Data; 

/// <summary>
/// Clase que representa un ejemplo de uso de documentación XML en C#.
/// </summary>
/// <remarks>
/// Esta clase es un ejemplo para mostrar cómo se puede documentar el código utilizando etiquetas XML estándar.
/// </remarks>
[Ignore]
public interface IDatabase {
    /// <summary>
    /// Obtiene una conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto que implementa <see cref="IDbConnection"/> que representa la conexión a la base de datos.
    /// </returns>
    /// <remarks>
    /// Este método debe ser implementado para proporcionar una conexión válida a la base de datos 
    /// que se utilizará para realizar operaciones de acceso a datos.
    /// </remarks>
    IDbConnection GetConnection();
}