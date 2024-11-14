namespace Util.Microservices.Dapr; 

/// <summary>
/// Clase auxiliar para interactuar con Dapr.
/// </summary>
public static class DaprHelper {
    public const string DaprAppId = "Dapr_AppId";

    /// <summary>
    /// Establece el identificador de la aplicación en las variables de entorno.
    /// </summary>
    /// <param name="appId">El identificador de la aplicación que se va a establecer.</param>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Environment"/> para establecer una variable de entorno
    /// con el nombre definido por <c>DaprAppId</c> y el valor proporcionado por el parámetro <c>appId</c>.
    /// </remarks>
    public static void SetAppId( string appId ) {
        Environment.SetEnvironmentVariable( DaprAppId, appId );
    }

    /// <summary>
    /// Obtiene el identificador de la aplicación desde las variables de entorno.
    /// </summary>
    /// <returns>
    /// Un <see cref="string"/> que representa el identificador de la aplicación, 
    /// o <c>null</c> si la variable de entorno no está definida.
    /// </returns>
    /// <remarks>
    /// Este método busca la variable de entorno definida por la constante <c>DaprAppId</c>.
    /// Asegúrese de que la variable de entorno esté configurada antes de llamar a este método.
    /// </remarks>
    public static string GetAppId() {
        return Environment.GetEnvironmentVariable( DaprAppId );
    }
}