using Util.Dependency;

namespace Util.Sessions;

/// <summary>
/// Clase que representa una operación de entrada/salida con un código específico.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para manejar operaciones de entrada/salida y puede incluir
/// métodos para leer y escribir datos, así como para manejar errores relacionados
/// con estas operaciones.
/// </remarks>
/// <param name="codigo">El código de operación, que puede ser un valor negativo o positivo.</param>
/// <seealso cref="OtraClase"/> 
[Ioc(-9)]
public class NullSession : ISession {
    public static readonly ISession Instance = new NullSession();

    /// <summary>
    /// Obtiene el proveedor de servicios asociado.
    /// </summary>
    /// <remarks>
    /// Este miembro devuelve siempre <c>null</c>. 
    /// Se debe implementar la lógica para proporcionar un servicio válido.
    /// </remarks>
    /// <returns>
    /// Un objeto que implementa <see cref="IServiceProvider"/> o <c>null</c>.
    /// </returns>
    public IServiceProvider ServiceProvider => null;

    /// <summary>
    /// Indica si el usuario está autenticado.
    /// </summary>
    /// <value>
    /// Devuelve siempre <c>false</c>, lo que significa que el usuario no está autenticado.
    /// </value>
    public bool IsAuthenticated => false;

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del usuario.
    /// </summary>
    /// <remarks>
    /// Este identificador se devuelve como una cadena vacía.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el identificador del usuario.
    /// </returns>
    public string UserId => string.Empty;

    /// <inheritdoc />
    /// <summary>
    /// Obtiene el identificador del inquilino.
    /// </summary>
    /// <remarks>
    /// Este identificador se devuelve como una cadena vacía.
    /// </remarks>
    /// <returns>
    /// Una cadena que representa el identificador del inquilino.
    /// </returns>
    public string TenantId => string.Empty;
}