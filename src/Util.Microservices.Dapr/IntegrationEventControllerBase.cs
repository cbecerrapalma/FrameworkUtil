using Util.Microservices.Dapr.Events;
using Util.Microservices.Dapr.Events.Filters;

namespace Util.Microservices.Dapr;

/// <summary>
/// Controlador de API para manejar las operaciones relacionadas con los recursos.
/// </summary>
/// <remarks>
/// Este controlador proporciona métodos para realizar operaciones CRUD sobre los recursos.
/// </remarks>
[ApiController]
[SubscriptionFilter]
[SubscriptionExceptionHandler( Order = 1 )]
[SubscriptionErrorLogFilter( Order = 2 )]
[Route( "api/[controller]" )]
public abstract class IntegrationEventControllerBase : ControllerBase {
    /// <summary>
    /// Obtiene la sesión de usuario actual.
    /// </summary>
    /// <value>
    /// La instancia de la sesión de usuario.
    /// </value>
    /// <remarks>
    /// Esta propiedad es virtual, lo que permite que las clases derivadas puedan 
    /// sobreescribir su comportamiento si es necesario.
    /// </remarks>
    /// <seealso cref="Util.Sessions.ISession"/>
    protected virtual Util.Sessions.ISession Session => Util.Sessions.UserSession.Instance;

    /// <summary>
    /// Devuelve un resultado exitoso de la operación.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="IActionResult"/> que representa el resultado exitoso.
    /// </returns>
    protected IActionResult Success() 
    { 
        return PubsubResult.Success; 
    }

    /// <summary>
    /// Devuelve un resultado de fallo con un mensaje específico.
    /// </summary>
    /// <param name="message">El mensaje que describe el motivo del fallo.</param>
    /// <returns>Un objeto <see cref="IActionResult"/> que representa el resultado de fallo.</returns>
    protected IActionResult Fail(string message) 
    { 
        return PubsubResult.Fail(message); 
    }

    /// <summary>
    /// Maneja la acción de eliminar un mensaje del sistema de 
    protected IActionResult Drop( string message ) {
        return PubsubResult.Drop( message );
    }
}