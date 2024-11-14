namespace Util.Microservices.Dapr.Events;

/// <summary>
/// Representa el resultado de una operación de 
public class PubsubResult : JsonResult {
    /// <summary>
    /// Obtiene el estado actual.
    /// </summary>
    /// <remarks>
    /// Esta propiedad proporciona acceso de solo lectura al estado.
    /// </remarks>
    /// <value>
    /// Un <see cref="string"/> que representa el estado.
    /// </value>
    public string Status { get; }

    /// <summary>
    /// Obtiene el mensaje asociado.
    /// </summary>
    /// <value>
    /// Un string que representa el mensaje.
    /// </value>
    public string Message { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PubsubResult"/>.
    /// </summary>
    /// <param name="status">El estado del resultado de la operación.</param>
    /// <param name="message">Un mensaje opcional que proporciona información adicional sobre el resultado.</param>
    /// <remarks>
    /// Este constructor establece el estado y el mensaje del resultado, y configura las opciones de serialización.
    /// </remarks>
    public PubsubResult( string status, string message = null ) : base( null ) {
        Status = status;
        Message = message;
        SerializerSettings = GetOptions();
    }

    /// <summary>
    /// Obtiene las opciones de configuración para la serialización JSON.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> configurado con la política de nombres de propiedades en formato camel case.
    /// </returns>
    private JsonSerializerOptions GetOptions() {
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Ejecuta el resultado de la acción de manera asíncrona.
    /// </summary>
    /// <param name="context">El contexto de la acción que contiene información sobre la solicitud actual.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// </returns>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación personalizada
    /// que establece un valor con el estado antes de llamar al método base.
    /// </remarks>
    public override Task ExecuteResultAsync( ActionContext context ) {
        Value = new { Status };
        return base.ExecuteResultAsync( context );
    }

    public static readonly PubsubResult Success = new( "SUCCESS" );

    /// <summary>
    /// Crea un resultado de fallo para una operación de Pub/Sub.
    /// </summary>
    /// <param name="message">El mensaje que describe el motivo del fallo.</param>
    /// <returns>Un objeto <see cref="PubsubResult"/> que indica un estado de reintento con el mensaje proporcionado.</returns>
    public static PubsubResult Fail( string message ) {
        return new PubsubResult( "RETRY", message );
    }

    /// <summary>
    /// Crea un resultado de 
    public static PubsubResult Drop( string message ) {
        return new PubsubResult( "DROP", message );
    }
}