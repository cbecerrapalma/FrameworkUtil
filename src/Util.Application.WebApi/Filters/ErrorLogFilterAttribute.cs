using Util.Aop;
using Util.Exceptions;

namespace Util.Applications.Filters; 

/// <summary>
/// Filtro de excepciones que registra los errores en el registro de errores.
/// </summary>
/// <remarks>
/// Este filtro se utiliza para interceptar excepciones no controladas y registrar la información relevante
/// para el diagnóstico y la solución de problemas.
/// </remarks>
public class ErrorLogFilterAttribute : ExceptionFilterAttribute {
    /// <summary>
    /// Maneja las excepciones que ocurren durante la ejecución de una acción.
    /// </summary>
    /// <param name="context">El contexto de la excepción que contiene información sobre la solicitud y la excepción lanzada.</param>
    /// <remarks>
    /// Este método se invoca automáticamente cuando se produce una excepción en una acción.
    /// Registra la excepción utilizando el servicio de registro correspondiente.
    /// Si la excepción es de tipo <see cref="Warning"/>, se registra como una advertencia.
    /// De lo contrario, se registra como un error.
    /// </remarks>
    /// <seealso cref="Warning"/>
    public override void OnException( ExceptionContext context ) {
        if( context == null )
            return;
        var log = context.HttpContext.RequestServices.GetService<ILogger<ErrorLogFilterAttribute>>();
        var exception = context.Exception.GetRawException();
        if( exception is Warning warning ) {
            log.LogWarning( warning, exception.Message );
            return;
        }
        log.LogError( exception, exception.Message );
    }
}