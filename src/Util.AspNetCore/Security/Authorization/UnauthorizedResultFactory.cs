using Util.Applications;

namespace Util.Security.Authorization;

/// <summary>
/// Clase que implementa la interfaz <see cref="IUnauthorizedResultFactory"/>.
/// Proporciona métodos para crear resultados no autorizados.
/// </summary>
public class UnauthorizedResultFactory : IUnauthorizedResultFactory {
    /// <inheritdoc />
    /// <summary>
    /// Obtiene el código de estado HTTP.
    /// </summary>
    /// <remarks>
    /// Este código de estado representa una respuesta exitosa.
    /// </remarks>
    /// <returns>
    /// Un entero que representa el código de estado HTTP.
    /// </returns>
    public virtual int HttpStatusCode => 200;

    /// <inheritdoc />
    /// <summary>
    /// Crea un resultado basado en el contexto HTTP proporcionado.
    /// </summary>
    /// <param name="context">El contexto HTTP que se utiliza para crear el resultado.</param>
    /// <returns>
    /// Un objeto anónimo que contiene un código de estado que indica que la solicitud no está autorizada.
    /// </returns>
    /// <remarks>
    /// Este método es virtual y puede ser sobreescrito en clases derivadas para proporcionar una implementación diferente.
    /// </remarks>
    public virtual object CreateResult( HttpContext context ) {
        return new { Code = StateCode.Unauthorized };
    }
}