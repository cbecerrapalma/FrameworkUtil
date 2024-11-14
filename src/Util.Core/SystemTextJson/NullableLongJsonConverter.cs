namespace Util.SystemTextJson;

/// <summary>
/// Convertidor de JSON para valores de tipo <see cref="long?"/> (nullable long).
/// </summary>
/// <remarks>
/// Este convertidor permite serializar y deserializar valores nulos y no nulos de tipo <see cref="long"/> 
/// al formato JSON, manejando adecuadamente los casos en que el valor es nulo.
/// </remarks>
public class NullableLongJsonConverter : JsonConverter<long?> {
    /// <summary>
    /// Lee un valor de tipo <see cref="long?"/> desde un <see cref="Utf8JsonReader"/>.
    /// </summary>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor.</param>
    /// <param name="typeToConvert">El tipo al que se convertirá el valor leído.</param>
    /// <param name="options">Las opciones de serialización JSON que se aplicarán durante la lectura.</param>
    /// <returns>
    /// Un valor de tipo <see cref="long?"/> que representa el valor leído, o <c>null</c> si no se puede convertir.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el token actual es una cadena y, de ser así, intenta convertirlo a un valor <see cref="long"/>.
    /// Si el token no es una cadena, intenta obtener un valor <see cref="long"/> directamente.
    /// </remarks>
    public override long? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
        if( reader.TokenType == JsonTokenType.String )
            return Util.Helpers.Convert.ToLongOrNull( reader.GetString() );
        return reader.TryGetInt64( out var value ) ? value : null;
    }

    /// <summary>
    /// Escribe un valor de tipo <see cref="long?"/> en el formato JSON utilizando el escritor proporcionado.
    /// </summary>
    /// <param name="writer">El escritor JSON en el que se escribirá el valor.</param>
    /// <param name="value">El valor de tipo <see cref="long?"/> que se va a escribir. Puede ser nulo.</param>
    /// <param name="options">Opciones de serialización que se aplicarán al escribir el valor.</param>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una representación de cadena del valor.
    /// Si el valor es nulo, se escribirá como una cadena vacía.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, long? value, JsonSerializerOptions options ) {
        writer.WriteStringValue( value.SafeString() );
    }
}