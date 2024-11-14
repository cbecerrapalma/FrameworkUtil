using Util.Helpers;

namespace Util.SystemTextJson; 

/// <summary>
/// Convierte valores de tipo <see cref="DateTime?"/> a JSON y viceversa.
/// </summary>
/// <remarks>
/// Esta clase se utiliza para manejar la serialización y deserialización de fechas que pueden ser nulas.
/// Si el valor es nulo, se serializa como <c>null</c>, de lo contrario, se serializa como una cadena en formato ISO 8601.
/// </remarks>
public class NullableDateTimeJsonConverter : JsonConverter<DateTime?> {
    private readonly string _format;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NullableDateTimeJsonConverter"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor utiliza un formato de fecha y hora predeterminado "yyyy-MM-dd HH:mm:ss".
    /// </remarks>
    public NullableDateTimeJsonConverter() : this( "yyyy-MM-dd HH:mm:ss" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NullableDateTimeJsonConverter"/>.
    /// </summary>
    /// <param name="format">El formato de fecha y hora que se utilizará para la conversión.</param>
    public NullableDateTimeJsonConverter( string format ) {
        _format = format;
    }

    /// <summary>
    /// Lee un valor de fecha y hora desde un lector JSON.
    /// </summary>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor.</param>
    /// <param name="typeToConvert">El tipo al que se convertirá el valor leído.</param>
    /// <param name="options">Opciones de serialización JSON que se aplican durante la lectura.</param>
    /// <returns>
    /// Un objeto <see cref="DateTime?"/> que representa la fecha y hora leída, 
    /// o <c>null</c> si no se puede convertir el valor. 
    /// Si no se puede leer un valor válido, se devuelve <see cref="DateTime.MinValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método intenta leer un valor de tipo cadena y lo convierte a un objeto <see cref="DateTime"/> 
    /// utilizando un método auxiliar. Si el token actual es un tipo de cadena, se convierte directamente. 
    /// Si el token puede ser interpretado como una fecha y hora, se utiliza el método <c>TryGetDateTime</c> 
    /// para obtener el valor. En caso de que no se pueda leer un valor válido, se devuelve <see cref="DateTime.MinValue"/>.
    /// </remarks>
    public override DateTime? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if ( reader.TokenType == JsonTokenType.String ) {
            return Time.ToLocalTime( Util.Helpers.Convert.ToDateTime( reader.GetString() ) );
        }
        if ( reader.TryGetDateTime( out var date ) ) {
            return Time.ToLocalTime( date );
        }
        return DateTime.MinValue;
    }

    /// <summary>
    /// Escribe un valor de fecha y hora en formato JSON utilizando el escritor especificado.
    /// </summary>
    /// <param name="writer">El escritor JSON en el que se escribirá el valor de fecha y hora.</param>
    /// <param name="value">El valor de fecha y hora a escribir. Puede ser nulo.</param>
    /// <param name="options">Las opciones de serialización JSON que se aplicarán.</param>
    /// <remarks>
    /// Si el valor proporcionado es nulo, se escribirá un valor nulo en el JSON. 
    /// Si el valor no es nulo, se convertirá a la hora local y se formateará según el formato especificado.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options ) {
        if ( value == null ) {
            writer.WriteNullValue();
            return;
        }
        var date = Time.ToLocalTime( value.Value ).ToString( _format );
        writer.WriteStringValue( date );
    }
}