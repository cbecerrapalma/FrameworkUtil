namespace Util.SystemTextJson; 

/// <summary>
/// Convierte valores de tipo <see cref="long"/> a JSON y viceversa.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="JsonConverter{T}"/> y proporciona una implementación específica
/// para manejar la serialización y deserialización de valores de tipo <see cref="long"/>.
/// </remarks>
public class LongJsonConverter : JsonConverter<long> {
    /// <summary>
    /// Lee un valor de tipo <see cref="long"/> desde un lector de JSON.
    /// </summary>
    /// <param name="reader">Referencia al lector de JSON que se utilizará para leer el valor.</param>
    /// <param name="typeToConvert">El tipo al que se convertirá el valor leído.</param>
    /// <param name="options">Opciones de serialización que se aplicarán durante la lectura.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long"/> leído desde el lector de JSON. 
    /// Si el token actual es una cadena, se convierte a <see cref="long"/>; 
    /// de lo contrario, se intenta obtener un <see cref="long"/> directamente. 
    /// Si no se puede obtener, se devuelve 0.
    /// </returns>
    /// <remarks>
    /// Este método es parte de la implementación de un convertidor de JSON personalizado 
    /// que permite leer valores de tipo <see cref="long"/> desde diferentes representaciones en JSON.
    /// </remarks>
    public override long Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if ( reader.TokenType == JsonTokenType.String )
            return Util.Helpers.Convert.ToLong( reader.GetString() );
        return reader.TryGetInt64( out var value ) ? value : 0;
    }

    /// <summary>
    /// Escribe un valor de tipo <see cref="long"/> en formato JSON utilizando un escritor de JSON.
    /// </summary>
    /// <param name="writer">El escritor de JSON donde se escribirá el valor.</param>
    /// <param name="value">El valor de tipo <see cref="long"/> que se va a escribir.</param>
    /// <param name="options">Las opciones de serialización de JSON que se aplicarán al escribir el valor.</param>
    public override void Write( Utf8JsonWriter writer, long value, JsonSerializerOptions options ) {
        writer.WriteStringValue( value.SafeString() );
    }
}