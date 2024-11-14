namespace Util.Data.Sql; 

/// <summary>
/// Representa las opciones de configuración para una conexión SQL específica con un tipo genérico.
/// </summary>
/// <typeparam name="T">El tipo de entidad que se utilizará con las opciones SQL.</typeparam>
/// <remarks>
/// Esta clase permite definir opciones personalizadas para la conexión SQL, 
/// proporcionando un tipo específico para la entidad que se manipulará.
/// </remarks>
public class SqlOptions<T> : SqlOptions where T: class {
}

/// <summary>
/// Representa las opciones de configuración para la conexión a una base de datos SQL.
/// </summary>
public class SqlOptions {
    /// <summary>
    /// Obtiene o establece la cadena de conexión utilizada para conectarse a la base de datos.
    /// </summary>
    /// <remarks>
    /// Esta propiedad permite definir la cadena de conexión que se utilizará para establecer la conexión con la base de datos.
    /// Asegúrese de que la cadena de conexión sea válida y contenga todos los parámetros necesarios para la conexión.
    /// </remarks>
    /// <value>
    /// Una cadena que representa la cadena de conexión.
    /// </value>
    public string ConnectionString { get; set; }
    /// <summary>
    /// Obtiene o establece la conexión a la base de datos.
    /// </summary>
    /// <value>
    /// Un objeto que implementa la interfaz <see cref="IDbConnection"/> que representa la conexión a la base de datos.
    /// </value>
    public IDbConnection Connection { get; set; }
    /// <summary>
    /// Representa la categoría de registro utilizada para la salida de logs.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se inicializa con el valor por defecto "Util.Data.Sql".
    /// </remarks>
    /// <value>
    /// Una cadena que representa la categoría de registro.
    /// </value>
    public string LogCategory { get; set; } = "Util.Data.Sql";
}