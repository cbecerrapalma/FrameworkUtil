namespace Util.Logging; 

/// <summary>
/// Representa el contexto de registro para la aplicación.
/// </summary>
/// <remarks>
/// Esta clase se encarga de manejar la información relacionada con el registro de eventos,
/// permitiendo la configuración y el acceso a los datos necesarios para el registro.
/// </remarks>
public class LogContext {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="LogContext"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea un nuevo diccionario para almacenar los datos de contexto de registro.
    /// </remarks>
    public LogContext() {
        Data = new Dictionary<string, object>();
    }

    /// <summary>
    /// Obtiene o establece el temporizador que mide el tiempo transcurrido.
    /// </summary>
    /// <remarks>
    /// Esta propiedad utiliza la clase <see cref="System.Diagnostics.Stopwatch"/> 
    /// para realizar un seguimiento del tiempo. Puede ser utilizada para medir 
    /// el rendimiento de operaciones o para implementar funcionalidades que 
    /// requieran un control preciso del tiempo.
    /// </remarks>
    public Stopwatch Stopwatch { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador de seguimiento.
    /// </summary>
    /// <remarks>
    /// El identificador de seguimiento se utiliza para rastrear solicitudes y operaciones a través de los sistemas.
    /// </remarks>
    /// <value>
    /// Un string que representa el identificador de seguimiento.
    /// </value>
    public string TraceId { get; set; }
    /// <summary>
    /// Obtiene o establece el identificador del usuario.
    /// </summary>
    /// <remarks>
    /// Este identificador se utiliza para diferenciar entre los distintos usuarios en el sistema.
    /// </remarks>
    public string UserId { get; set; }
    /// <summary>
    /// Obtiene o establece el nombre de la aplicación.
    /// </summary>
    /// <value>
    /// Una cadena que representa el nombre de la aplicación.
    /// </value>
    public string Application { get; set; }
    /// <summary>
    /// Obtiene o establece el entorno de la aplicación.
    /// </summary>
    /// <remarks>
    /// Este propiedad puede contener valores como "Desarrollo", "Pruebas" o "Producción".
    /// </remarks>
    /// <value>
    /// Un string que representa el entorno actual de la aplicación.
    /// </value>
    public string Environment { get; set; }
    /// <summary>
    /// Obtiene un diccionario que almacena datos asociados a claves de tipo <see cref="string"/>.
    /// </summary>
    /// <remarks>
    /// Este diccionario permite almacenar y recuperar información de manera dinámica,
    /// utilizando cadenas como claves y objetos como valores. Es útil para situaciones
    /// donde se necesita un almacenamiento flexible de datos.
    /// </remarks>
    /// <returns>
    /// Un <see cref="IDictionary{TKey, TValue}"/> que contiene pares clave-valor,
    /// donde la clave es de tipo <see cref="string"/> y el valor es de tipo <see cref="object"/>.
    /// </returns>
    public IDictionary<string, object> Data { get; }
}