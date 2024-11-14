using Util.Aop;

namespace Util.Microservices.Dapr.Events.Filters;

/// <summary>
/// Atributo que maneja excepciones específicas para suscripciones.
/// </summary>
/// <remarks>
/// Este atributo se utiliza para interceptar excepciones que ocurren durante el procesamiento de solicitudes 
/// relacionadas con suscripciones y permite manejar errores de manera centralizada.
/// </remarks>
public class SubscriptionExceptionHandlerAttribute : ExceptionFilterAttribute {
    /// <summary>
    /// Maneja las excepciones que ocurren durante el procesamiento de una solicitud.
    /// </summary>
    /// <param name="context">El contexto de la excepción que contiene información sobre el error ocurrido.</param>
    /// <remarks>
    /// Este método se invoca cuando se produce una excepción en el flujo de trabajo. 
    /// Dependiendo del tipo de excepción, se establece un resultado específico en el contexto.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el contexto es nulo.</exception>
    /// <seealso cref="ExceptionContext"/>
    public override void OnException( ExceptionContext context ) {
        context.ExceptionHandled = true;
        var exception = context.Exception.GetRawException();
        if ( exception == null )
            return;
        if ( exception is ConcurrencyException concurrencyException ) {
            context.Result = PubsubResult.Fail( concurrencyException.Message );
            return;
        }
        if ( exception is Warning warning ) {
            context.Result = PubsubResult.Drop( warning.Message );
            return;
        }
        context.Result = PubsubResult.Fail( Warning.GetMessage( exception ) );
    }
}