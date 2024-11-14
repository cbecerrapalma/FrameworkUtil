using Util.Helpers;

namespace Util.SystemTextJson; 

/// <summary>
/// Convierte objetos de tipo <see cref="DateTime"/> a su representación en formato JSON 
/// y viceversa.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="JsonConverter{T}"/> y proporciona la funcionalidad 
/// necesaria para serializar y deserializar objetos <see cref="DateTime"/> 
/// en un formato específico, permitiendo una mayor flexibilidad en el manejo de fechas 
/// en aplicaciones que utilizan JSON.
/// </remarks>
public class DateTimeJsonConverter : JsonConverter<DateTime> {
    private readonly string _format;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DateTimeJsonConverter"/> 
    /// con un formato de fecha y hora predeterminado.
    /// </summary>
    /// <remarks>
    /// Este constructor utiliza el formato "yyyy-MM-dd HH:mm:ss" para la conversión 
    /// de objetos <see cref="DateTime"/> a JSON y viceversa.
    /// </remarks>
    public DateTimeJsonConverter() : this( "yyyy-MM-dd HH:mm:ss" ) {
    }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DateTimeJsonConverter"/>.
    /// </summary>
    /// <param name="format">El formato de fecha y hora que se utilizará para la conversión.</param>
    public DateTimeJsonConverter( string format ) {
        _format = format;
    }

    /// <summary>
    /// Lee un valor de fecha y hora desde un lector JSON y lo convierte a la hora local.
    /// </summary>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor de fecha y hora.</param>
    /// <param name="typeToConvert">El tipo al que se está convirtiendo el valor leído.</param>
    /// <param name="options">Las opciones de serialización que se utilizan durante la lectura.</param>
    /// <returns>Un objeto <see cref="DateTime"/> que representa la fecha y hora en la hora local.</returns>
    /// <remarks>
    /// Si el token actual del lector es una cadena, se intenta convertir esa cadena a un objeto <see cref="DateTime"/>.
    /// Si el token actual es un valor de fecha y hora, se convierte directamente a la hora local.
    /// Si no se puede leer un valor de fecha y hora válido, se devuelve <see cref="DateTime.MinValue"/>.
    /// </remarks>
    public override DateTime Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if ( reader.TokenType == JsonTokenType.String )
            return Time.ToLocalTime( Util.Helpers.Convert.ToDateTime( reader.GetString() ) );
        if ( reader.TryGetDateTime( out var date ) )
            return Time.ToLocalTime( date );
        return DateTime.MinValue;
    }

    /// <summary>
    /// Escribe un valor de tipo <see cref="DateTime"/> en formato de cadena en el escritor JSON especificado.
    /// </summary>
    /// <param name="writer">El escritor JSON donde se escribirá el valor.</param>
    /// <param name="value">El valor de tipo <see cref="DateTime"/> que se va a escribir.</param>
    /// <param name="options">Las opciones de serialización JSON que se utilizarán.</param>
    /// <remarks>
    /// Este método convierte el valor de <see cref="DateTime"/> a la hora local y lo formatea
    /// utilizando el formato especificado por la variable <c>_format</c> antes de escribirlo
    /// como una cadena en el escritor JSON.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options ) {
        var date = Time.ToLocalTime( value ).ToString( _format );
        writer.WriteStringValue( date );
    }
}