using Util.AspNetCore;
using Util.SystemTextJson;

namespace Util.Applications;

/// <summary>
/// Clase que implementa la interfaz <see cref="IJsonSerializerOptionsFactory"/>.
/// Proporciona opciones de configuración para la serialización y deserialización de JSON.
/// </summary>
public class JsonSerializerOptionsFactory : IJsonSerializerOptionsFactory {
    /// <summary>
    /// Crea y configura una instancia de <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>
    /// Devuelve un objeto <see cref="JsonSerializerOptions"/> configurado con las políticas de 
    /// nomenclatura, codificación y condiciones de ignorar propiedades.
    /// </returns>
    /// <remarks>
    /// Este método establece la política de nombres de propiedades en formato camelCase, 
    /// utiliza un codificador que permite todos los rangos Unicode, y especifica que las 
    /// propiedades que tienen un valor nulo deben ser ignoradas durante la serialización.
    /// Además, se añaden convertidores personalizados para manejar tipos específicos como 
    /// <see cref="DateTime"/>, <see cref="Nullable{DateTime}"/>, <see cref="long"/> y 
    /// <see cref="Nullable{long}"/>.
    /// </remarks>
    public JsonSerializerOptions CreateOptions() {
        return new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }
}