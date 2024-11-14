namespace Util.Microservices.Dapr.StateManagements.Queries;

/// <summary>
/// Representa una página de estado en la aplicación.
/// </summary>
public class StatePage {
    /// <summary>
    /// Representa el límite de elementos a recuperar.
    /// </summary>
    /// <remarks>
    /// Este valor es opcional y puede ser nulo. Si se establece, se utilizará para limitar la cantidad de elementos
    /// devueltos en una consulta o respuesta.
    /// </remarks>
    /// <value>
    /// Un entero que representa el límite de elementos, o null si no se ha especificado.
    /// </value>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>
    /// Representa un token en formato de cadena.
    /// </summary>
    /// <remarks>
    /// Esta propiedad se utiliza para almacenar un token que puede ser utilizado para la autenticación o autorización.
    /// </remarks>
    /// <value>
    /// Un valor de tipo <see cref="string"/> que representa el token.
    /// </value>
    [JsonPropertyName("token")]
    public string Token { get; set; }
}