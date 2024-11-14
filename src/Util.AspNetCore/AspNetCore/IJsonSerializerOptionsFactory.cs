namespace Util.AspNetCore; 

/// <summary>
/// Interfaz para la fábrica de opciones de serialización JSON.
/// </summary>
/// <remarks>
/// Esta interfaz define un contrato para la creación de opciones de serialización JSON,
/// permitiendo la configuración de parámetros específicos para la serialización y deserialización
/// de objetos en formato JSON.
/// </remarks>
public interface IJsonSerializerOptionsFactory : ISingletonDependency {
    /// <summary>
    /// Crea y configura una instancia de <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> configurado con las opciones necesarias para la serialización y deserialización de JSON.
    /// </returns>
    /// <remarks>
    /// Este método establece las configuraciones predeterminadas que se utilizarán al trabajar con JSON, 
    /// como la inclusión de propiedades nulas, la configuración de nombres de propiedades, 
    /// y otras opciones relevantes para la serialización.
    /// </remarks>
    /// <seealso cref="JsonSerializer"/>
    /// <seealso cref="JsonSerializerOptions"/>
    JsonSerializerOptions CreateOptions();
}