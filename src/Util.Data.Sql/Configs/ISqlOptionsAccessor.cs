namespace Util.Data.Sql.Configs; 

/// <summary>
/// Interfaz que define las opciones de configuración para el acceso a bases de datos SQL.
/// </summary>
public interface ISqlOptionsAccessor {
    /// <summary>
    /// Obtiene las opciones de configuración para la conexión a la base de datos.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="SqlOptions"/> que contiene las opciones de configuración.
    /// </returns>
    /// <remarks>
    /// Este método se utiliza para recuperar las opciones necesarias para establecer una conexión
    /// con la base de datos, incluyendo parámetros como el servidor, la base de datos, el usuario,
    /// y la contraseña.
    /// </remarks>
    SqlOptions GetOptions();
}