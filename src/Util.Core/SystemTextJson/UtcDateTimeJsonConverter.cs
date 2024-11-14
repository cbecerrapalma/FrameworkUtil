using Util.Helpers;

namespace Util.SystemTextJson; 

/// <summary>
/// Convierte objetos <see cref="DateTime"/> a formato UTC y viceversa para su uso en JSON.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="JsonConverter{T}"/> y proporciona una implementación específica
/// para manejar la serialización y deserialización de fechas y horas en formato UTC.
/// </remarks>
public class UtcDateTimeJsonConverter : JsonConverter<DateTime> {
    private readonly string _format;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UtcDateTimeJsonConverter"/> 
    /// utilizando el formato de fecha y hora predeterminado.
    /// </summary>
    /// <remarks>
    /// Este constructor establece el formato de fecha y hora en "yyyy-MM-ddTHH:mm:sszzzz".
    /// </remarks>
    public UtcDateTimeJsonConverter() : this( "yyyy-MM-ddTHH:mm:sszzzz" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UtcDateTimeJsonConverter"/>.
    /// </summary>
    /// <param name="format">El formato de fecha y hora que se utilizará para la conversión.</param>
    public UtcDateTimeJsonConverter( string format ) {
        _format = format;
    }

    /// <summary>
    /// Lee un valor de fecha y hora desde un lector JSON y lo convierte a la hora local.
    /// </summary>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor.</param>
    /// <param name="typeToConvert">El tipo al que se desea convertir el valor leído.</param>
    /// <param name="options">Opciones de serialización JSON que pueden afectar la lectura.</param>
    /// <returns>La fecha y hora convertida a la hora local, o DateTime.MinValue si no se puede leer el valor.</returns>
    /// <remarks>
    /// Este método verifica primero si el token actual es una cadena. Si es así, intenta convertirla a DateTime.
    /// Si no es una cadena, intenta obtener un DateTime directamente del lector. En caso de que ninguna de las 
    /// conversiones sea exitosa, se devuelve DateTime.MinValue.
    /// </remarks>
    public override DateTime Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if ( reader.TokenType == JsonTokenType.String ) {
            return Time.UtcToLocalTime( Util.Helpers.Convert.ToDateTime( reader.GetString() ) );
        }
        if ( reader.TryGetDateTime( out var date ) ) {
            return Time.UtcToLocalTime( date );
        }
        return DateTime.MinValue;
    }

    /// <summary>
    /// Escribe un valor de tipo <see cref="DateTime"/> en formato de cadena en el escritor JSON especificado.
    /// </summary>
    /// <param name="writer">El escritor JSON donde se escribirá el valor de fecha.</param>
    /// <param name="value">El valor de tipo <see cref="DateTime"/> que se va a escribir.</param>
    /// <param name="options">Las opciones de serialización JSON que se aplicarán al escribir el valor.</param>
    /// <remarks>
    /// Este método normaliza el valor de fecha y lo convierte a una cadena utilizando un formato específico antes de escribirlo.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options ) {
        var date = Time.Normalize( value ).ToString( _format );
        writer.WriteStringValue( date );
    }
}