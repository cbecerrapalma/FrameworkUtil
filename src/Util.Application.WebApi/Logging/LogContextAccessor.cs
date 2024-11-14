using Util.Helpers;
using Util.Logging;

namespace Util.Applications.Logging; 

/// <summary>
/// Proporciona acceso al contexto de registro.
/// </summary>
/// <remarks>
/// Esta clase implementa la interfaz <see cref="ILogContextAccessor"/> 
/// para permitir la obtención y configuración del contexto de registro 
/// en la aplicación.
/// </remarks>
public class LogContextAccessor : ILogContextAccessor {
    public const string LogContextKey = "Util.Logging.LogContext";

    /// <summary>
    /// Obtiene o establece el contexto de registro (LogContext).
    /// </summary>
    /// <value>
    /// El contexto de registro actual, o <c>null</c> si no está establecido.
    /// </value>
    /// <remarks>
    /// Este contexto se almacena en el objeto <see cref="HttpContext.Items"/> utilizando una clave específica.
    /// </remarks>
    public LogContext Context {
        get => Util.Helpers.Convert.To<LogContext>( Web.HttpContext?.Items[LogContextKey] );
        set => Web.HttpContext.Items[LogContextKey] = value;
    }
}