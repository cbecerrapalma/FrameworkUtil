namespace Util.Microservices.Dapr.StateManagements.Queries; 

/// <summary>
/// Representa un elemento de ordenación en una colección.
/// </summary>
public class OrderByItem {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OrderByItem"/>.
    /// </summary>
    /// <param name="order">Una cadena que representa el orden, que puede incluir "Asc" o "Desc".</param>
    /// <remarks>
    /// Este constructor procesa la cadena de orden para extraer la clave y determinar el tipo de ordenamiento.
    /// Si la cadena termina con " Asc", se elimina y se considera un orden ascendente.
    /// Si termina con " Desc", se elimina y se considera un orden descendente.
    /// </remarks>
    public OrderByItem( string order ) {
        Key = order.SafeString().Trim();
        if ( Key.EndsWith( " Asc", StringComparison.OrdinalIgnoreCase ) ) {
            Key = Key.Substring( 0, Key.Length - 3 ).Trim();
            return;
        }
        if ( Key.EndsWith( " Desc", StringComparison.OrdinalIgnoreCase ) ) {
            Order = "DESC";
            Key = Key.Substring( 0, Key.Length - 4 ).Trim();
        }
    }

    /// <summary>
    /// Representa una clave en formato JSON.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para almacenar una clave que será serializada a JSON.
    /// </remarks>
    /// <returns>
    /// Devuelve la clave como una cadena de texto.
    /// </returns>
    [JsonPropertyName("key")]
    public string Key { get; }

    /// <summary>
    /// Representa la propiedad que almacena el pedido en formato JSON.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para deserializar datos JSON relacionados con un pedido.
    /// </remarks>
    /// <returns>
    /// Devuelve una cadena que representa el pedido.
    /// </returns>
    [JsonPropertyName("order")]
    public string Order { get; }
}