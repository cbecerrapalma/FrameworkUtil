using Util.Domain.Extending;
using Util.SystemTextJson;

namespace Util.Data.EntityFrameworkCore.ValueConverters; 

/// <summary>
/// Convierte un diccionario de propiedades adicionales en una cadena y viceversa.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ValueConverter{TSource, TDestination}"/> y proporciona la lógica necesaria 
/// para transformar un <see cref="ExtraPropertyDictionary"/> en un <see cref="string"/> y viceversa.
/// </remarks>
/// <typeparam name="ExtraPropertyDictionary">
/// Tipo de diccionario que contiene las propiedades adicionales.
/// </typeparam>
/// <seealso cref="ValueConverter{TSource, TDestination}"/>
public class ExtraPropertiesValueConverter : ValueConverter<ExtraPropertyDictionary, string> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExtraPropertiesValueConverter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor configura el convertidor para transformar propiedades adicionales en formato JSON
    /// y viceversa, utilizando las funciones <see cref="PropertiesToJson"/> y <see cref="JsonToProperties"/>.
    /// </remarks>
    /// <param name="extraProperties">Las propiedades adicionales que se convertirán a JSON.</param>
    /// <returns>Un objeto que representa el convertidor de propiedades adicionales.</returns>
    public ExtraPropertiesValueConverter()
        : base( extraProperties => PropertiesToJson( extraProperties ), json => JsonToProperties( json ) ) {
    }

    /// <summary>
    /// Convierte un diccionario de propiedades adicionales en una representación JSON.
    /// </summary>
    /// <param name="extraProperties">El diccionario de propiedades adicionales que se desea convertir a JSON.</param>
    /// <returns>Una cadena que representa el diccionario de propiedades adicionales en formato JSON.</returns>
    /// <remarks>
    /// Este método utiliza opciones de serialización que ignoran los valores nulos y codifican caracteres Unicode.
    /// Además, se incluyen convertidores personalizados para manejar fechas y horas en formato UTC.
    /// </remarks>
    /// <seealso cref="ExtraPropertyDictionary"/>
    /// <seealso cref="JsonSerializerOptions"/>
    /// <seealso cref="UtcDateTimeJsonConverter"/>
    /// <seealso cref="UtcNullableDateTimeJsonConverter"/>
    private static string PropertiesToJson( ExtraPropertyDictionary extraProperties ) {
        var options = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            Converters = {
                new UtcDateTimeJsonConverter(),
                new UtcNullableDateTimeJsonConverter()
            }
        };
        return Util.Helpers.Json.ToJson( extraProperties, options );
    }

    /// <summary>
    /// Convierte una cadena JSON en un diccionario de propiedades adicionales.
    /// </summary>
    /// <param name="json">La cadena JSON que se desea convertir.</param>
    /// <returns>
    /// Un <see cref="ExtraPropertyDictionary"/> que contiene las propiedades del JSON.
    /// Si el JSON está vacío o es un objeto vacío, se devuelve un nuevo <see cref="ExtraPropertyDictionary"/> vacío.
    /// </returns>
    /// <remarks>
    /// Este método utiliza opciones de deserialización que permiten que los nombres de las propiedades
    /// no sean sensibles a mayúsculas y minúsculas.
    /// </remarks>
    /// <seealso cref="ExtraPropertyDictionary"/>
    private static ExtraPropertyDictionary JsonToProperties( string json ) {
        if( json.IsEmpty() || json == "{}" )
            return new ExtraPropertyDictionary();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return Util.Helpers.Json.ToObject<ExtraPropertyDictionary>( json, options ) ?? new ExtraPropertyDictionary();
    }
}