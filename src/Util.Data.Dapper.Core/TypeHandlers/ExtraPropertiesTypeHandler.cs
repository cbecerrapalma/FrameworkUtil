using Util.Domain.Extending;
using Util.SystemTextJson;

namespace Util.Data.Dapper.TypeHandlers; 

/// <summary>
/// Clase que maneja la conversión de un diccionario de propiedades adicionales 
/// para su uso con Dapper y bases de datos SQL.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="SqlMapper.TypeHandler{T}"/> y permite 
/// la serialización y deserialización de un objeto <see cref="ExtraPropertyDictionary"/> 
/// a un formato que puede ser almacenado en una base de datos.
/// </remarks>
public class ExtraPropertiesTypeHandler : SqlMapper.TypeHandler<ExtraPropertyDictionary> {
    /// <summary>
    /// Establece el valor de un parámetro de base de datos utilizando un diccionario de propiedades adicionales.
    /// </summary>
    /// <param name="parameter">El parámetro de base de datos al que se le asignará el valor.</param>
    /// <param name="value">El diccionario de propiedades adicionales que se convertirá a JSON.</param>
    /// <remarks>
    /// Este método verifica si el parámetro o el diccionario de propiedades son nulos antes de intentar establecer el valor.
    /// Si alguno de ellos es nulo, el método no realiza ninguna acción.
    /// </remarks>
    public override void SetValue( IDbDataParameter parameter, ExtraPropertyDictionary value ) {
        if ( parameter == null )
            return;
        if ( value == null )
            return;
        parameter.Value = PropertiesToJson( value );
    }

    /// <summary>
    /// Convierte un diccionario de propiedades adicionales en una cadena JSON.
    /// </summary>
    /// <param name="extraProperties">El diccionario de propiedades adicionales que se desea convertir a JSON.</param>
    /// <returns>Una cadena que representa el diccionario de propiedades adicionales en formato JSON.</returns>
    /// <remarks>
    /// Este método utiliza opciones de serialización que ignoran las propiedades con valores nulos y 
    /// emplea convertidores personalizados para manejar fechas y horas en formato UTC.
    /// </remarks>
    /// <seealso cref="ExtraPropertyDictionary"/>
    /// <seealso cref="JsonSerializerOptions"/>
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
    /// Analiza el valor proporcionado y lo convierte en un diccionario de propiedades adicionales.
    /// </summary>
    /// <param name="value">El objeto que se va a analizar, que se espera que contenga una representación JSON.</param>
    /// <returns>Un <see cref="ExtraPropertyDictionary"/> que representa las propiedades analizadas del objeto JSON.</returns>
    /// <remarks>
    /// Este método sobrescribe la implementación base para proporcionar una conversión específica de un objeto JSON a un diccionario de propiedades.
    /// </remarks>
    public override ExtraPropertyDictionary Parse( object value ) {
        return JsonToProperties( value.SafeString() );
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
    /// Este método utiliza opciones de deserialización que permiten ignorar la diferencia entre mayúsculas y minúsculas en los nombres de las propiedades.
    /// Si la deserialización falla, se devuelve un nuevo <see cref="ExtraPropertyDictionary"/> vacío.
    /// </remarks>
    private static ExtraPropertyDictionary JsonToProperties( string json ) {
        if ( json.IsEmpty() || json == "{}" )
            return new ExtraPropertyDictionary();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return Util.Helpers.Json.ToObject<ExtraPropertyDictionary>( json, options ) ?? new ExtraPropertyDictionary();
    }
}