using Util.Helpers;

namespace Util.SystemTextJson; 

/// <summary>
/// Convertidor de JSON para objetos <see cref="DateTime?"/> que maneja fechas en formato UTC.
/// </summary>
/// <remarks>
/// Este convertidor se utiliza para serializar y deserializar valores de fecha y hora que pueden ser nulos.
/// Asegura que las fechas se manejen correctamente en formato UTC durante el proceso de conversión.
/// </remarks>
public class UtcNullableDateTimeJsonConverter : JsonConverter<DateTime?> {
    private readonly string _format;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UtcNullableDateTimeJsonConverter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor utiliza un formato de fecha y hora predeterminado "yyyy-MM-ddTHH:mm:sszzzz".
    /// </remarks>
    public UtcNullableDateTimeJsonConverter() : this( "yyyy-MM-ddTHH:mm:sszzzz" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UtcNullableDateTimeJsonConverter"/>.
    /// </summary>
    /// <param name="format">El formato de fecha y hora que se utilizará para la conversión.</param>
    public UtcNullableDateTimeJsonConverter( string format ) {
        _format = format;
    }

    /// <summary>
    /// Lee un valor de fecha y hora desde un lector JSON.
    /// </summary>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor.</param>
    /// <param name="typeToConvert">El tipo al que se está convirtiendo el valor leído.</param>
    /// <param name="options">Las opciones de serialización JSON que se aplican durante la lectura.</param>
    /// <returns>La fecha y hora convertida a la hora local, o <c>null</c> si no se puede convertir.</returns>
    /// <remarks>
    /// Este método intenta leer un valor de fecha y hora desde el lector JSON. Si el token actual es una cadena,
    /// se convierte a <see cref="DateTime"/> utilizando un método auxiliar. Si el token es un tipo de fecha y hora
    /// válido, se convierte directamente a la hora local. Si no se puede leer un valor de fecha y hora válido,
    /// se devuelve <see cref="DateTime.MinValue"/>.
    /// </remarks>
    public override DateTime? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if ( reader.TokenType == JsonTokenType.String ) {
            return Time.UtcToLocalTime( Util.Helpers.Convert.ToDateTime( reader.GetString() ) );
        }
        if ( reader.TryGetDateTime( out var date ) ) {
            return Time.UtcToLocalTime( date );
        }
        return DateTime.MinValue;
    }

    /// <summary>
    /// Escribe un valor de tipo <see cref="DateTime?"/> en el escritor JSON especificado.
    /// </summary>
    /// <param name="writer">El escritor JSON donde se escribirá el valor.</param>
    /// <param name="value">El valor de tipo <see cref="DateTime?"/> que se va a escribir. Puede ser nulo.</param>
    /// <param name="options">Las opciones de serialización que se aplicarán al escribir el valor.</param>
    /// <remarks>
    /// Si el valor proporcionado es nulo, se escribirá un valor nulo en el escritor JSON.
    /// En caso contrario, se normaliza el valor de fecha y se escribe como una cadena en el formato especificado.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options ) {
        if ( value == null ) {
            writer.WriteNullValue();
            return;
        }
        var date = Time.Normalize( value.Value ).ToString( _format );
        writer.WriteStringValue( date );
    }
}