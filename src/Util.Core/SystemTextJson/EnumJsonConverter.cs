using System.Runtime.CompilerServices;

namespace Util.SystemTextJson;

/// <summary>
/// Convierte un valor de enumeración a su representación JSON y viceversa.
/// </summary>
/// <typeparam name="T">El tipo de enumeración que se va a convertir.</typeparam>
/// <remarks>
/// Esta clase hereda de <see cref="JsonConverter{T}"/> y proporciona la funcionalidad necesaria
/// para serializar y deserializar valores de enumeración en formato JSON.
/// </remarks>
public class EnumJsonConverter<T> : JsonConverter<T> where T : struct, Enum {
    /// <summary>
    /// Lee un valor JSON y lo convierte en un valor de enumeración del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de enumeración que se va a leer.</typeparam>
    /// <param name="reader">El lector JSON que se utiliza para leer el valor.</param>
    /// <param name="type">El tipo de la enumeración que se va a convertir.</param>
    /// <param name="options">Las opciones de serialización que se utilizan durante la conversión.</param>
    /// <returns>
    /// Devuelve el valor de enumeración correspondiente si la lectura es exitosa; de lo contrario, devuelve el valor predeterminado de tipo <typeparamref name="T"/>.
    /// </returns>
    /// <remarks>
    /// Este método intenta primero leer un valor de punto flotante y, si tiene éxito, lo convierte en el valor de enumeración correspondiente.
    /// Si la lectura del valor de punto flotante falla, intenta leer un valor entero y realiza la conversión correspondiente.
    /// Si ambas lecturas fallan, se devuelve el valor predeterminado.
    /// </remarks>
    /// <seealso cref="Utf8JsonReader"/>
    /// <seealso cref="JsonSerializerOptions"/>
    public override T Read( ref Utf8JsonReader reader, System.Type type, JsonSerializerOptions options ) {
        var isSuccess = reader.TryGetSingle( out var floatValue );
        if ( isSuccess )
            return (T)System.Enum.Parse( type, floatValue.ToString( CultureInfo.InvariantCulture ), true );
        isSuccess = reader.TryGetInt32( out var intValue );
        if ( isSuccess )
            return (T)System.Enum.Parse( type, intValue.ToString( CultureInfo.InvariantCulture ), true );
        return default;
    }


    /// <summary>
    /// Escribe el valor especificado en el escritor JSON utilizando las opciones de serialización proporcionadas.
    /// </summary>
    /// <param name="writer">El escritor JSON donde se escribirá el valor.</param>
    /// <param name="value">El valor a escribir, que debe ser del tipo especificado por <typeparamref name="T"/>.</param>
    /// <param name="options">Las opciones de serialización que se utilizarán al escribir el valor.</param>
    /// <typeparam name="T">El tipo del valor que se está escribiendo. Debe ser un tipo compatible con la serialización JSON.</typeparam>
    /// <remarks>
    /// Este método sobrescribe el método base para proporcionar una implementación específica de escritura
    /// para tipos de datos como <see cref="Byte"/> e <see cref="Int32"/>. 
    /// Se utiliza <see cref="Unsafe.As{TFrom,TTo}(ref TFrom)"/> para realizar conversiones de tipo de manera segura.
    /// </remarks>
    public override void Write( Utf8JsonWriter writer, T value, JsonSerializerOptions options ) {
        var code = Type.GetTypeCode( typeof(T) );
        switch ( code ) {
            case TypeCode.Byte:
                writer.WriteNumberValue( Unsafe.As<T, byte>( ref value ) );
                break;
            case TypeCode.Int32:
                writer.WriteNumberValue( Unsafe.As<T, int>( ref value ) );
                break;
        }
    }
}