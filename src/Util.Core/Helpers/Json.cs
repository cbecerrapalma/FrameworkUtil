using Util.SystemTextJson;

namespace Util.Helpers;

/// <summary>
/// Proporciona métodos estáticos para trabajar con datos en formato JSON.
/// </summary>
public static class Json {
    /// <summary>
    /// Convierte un objeto a su representación en formato JSON.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a convertir a JSON.</typeparam>
    /// <param name="value">El objeto que se desea convertir a JSON.</param>
    /// <param name="options">Opciones que controlan la serialización a JSON.</param>
    /// <returns>
    /// Una cadena que representa el objeto en formato JSON.
    /// </returns>
    /// <remarks>
    /// Este método utiliza las opciones proporcionadas para personalizar el proceso de serialización.
    /// Si las opciones son nulas, se utilizará un método de conversión por defecto.
    /// </remarks>
    /// <seealso cref="JsonOptions"/>
    public static string ToJson<T>( T value, JsonOptions options ) {
        if ( options == null )
            return ToJson( value );
        var jsonSerializerOptions = ToJsonSerializerOptions( options );
        return ToJson( value, jsonSerializerOptions, options.RemoveQuotationMarks, options.ToSingleQuotes, options.IgnoreInterface );
    }

    /// <summary>
    /// Convierte las opciones de configuración de JSON personalizadas en un objeto <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">Las opciones de configuración de JSON que se desean aplicar.</param>
    /// <returns>Un objeto <see cref="JsonSerializerOptions"/> configurado según las opciones proporcionadas.</returns>
    /// <remarks>
    /// Este método permite personalizar el comportamiento del serializador JSON, como la ignorancia de cadenas vacías,
    /// valores nulos y el uso de convenciones de nombres en minúsculas. También agrega convertidores personalizados
    /// para manejar tipos específicos como <see cref="DateTime"/> y <see cref="long"/>.
    /// </remarks>
    /// <seealso cref="JsonSerializerOptions"/>
    /// <seealso cref="JsonOptions"/>
    private static JsonSerializerOptions ToJsonSerializerOptions( JsonOptions options ) {
        var jsonSerializerOptions = new JsonSerializerOptions();
        if ( options.IgnoreEmptyString ) {
            jsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver {
                Modifiers = { IgnoreEmptyString }
            };
        }
        if ( options.IgnoreNullValues )
            jsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        if ( options.IgnoreCase )
            jsonSerializerOptions.PropertyNameCaseInsensitive = true;
        if ( options.ToCamelCase )
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSerializerOptions.Encoder = JavaScriptEncoder.Create( UnicodeRanges.All );
        jsonSerializerOptions.Converters.Add( new DateTimeJsonConverter() );
        jsonSerializerOptions.Converters.Add( new NullableDateTimeJsonConverter() );
        jsonSerializerOptions.Converters.Add( new LongJsonConverter() );
        jsonSerializerOptions.Converters.Add( new NullableLongJsonConverter() );
        return jsonSerializerOptions;
    }

    /// <summary>
    /// Ignora las cadenas vacías en un objeto JSON al serializar.
    /// </summary>
    /// <param name="jsonTypeInfo">El objeto <see cref="JsonTypeInfo"/> que contiene la información del tipo JSON.</param>
    /// <remarks>
    /// Este método verifica si el tipo JSON es un objeto y, si es así, recorre sus propiedades.
    /// Para cada propiedad de tipo cadena, se establece una función de serialización que
    /// determina si la cadena debe ser serializada o no, basándose en si está vacía.
    /// </remarks>
    private static void IgnoreEmptyString( JsonTypeInfo jsonTypeInfo ) {
        if ( jsonTypeInfo.Kind != JsonTypeInfoKind.Object )
            return;
        foreach ( JsonPropertyInfo jsonPropertyInfo in jsonTypeInfo.Properties ) {
            if ( jsonPropertyInfo.PropertyType == typeof( string ) ) {
                jsonPropertyInfo.ShouldSerialize = static ( _, value ) => value.SafeString().IsEmpty() == false;
            }
        }
    }

    /// <summary>
    /// Convierte un objeto a su representación en formato JSON.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a convertir.</typeparam>
    /// <param name="value">El objeto que se desea convertir a JSON.</param>
    /// <param name="options">Opciones de serialización JSON que se pueden aplicar. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="removeQuotationMarks">Indica si se deben eliminar las comillas del resultado JSON. El valor predeterminado es <c>false</c>.</param>
    /// <param name="toSingleQuotes">Indica si se deben usar comillas simples en lugar de comillas dobles en el resultado JSON. El valor predeterminado es <c>false</c>.</param>
    /// <returns>Una cadena que representa el objeto en formato JSON.</returns>
    /// <remarks>
    /// Este método es útil para serializar objetos a JSON de manera flexible, permitiendo opciones de configuración como la eliminación de comillas y el uso de comillas simples.
    /// </remarks>
    /// <seealso cref="JsonSerializerOptions"/>
    public static string ToJson<T>( T value, JsonSerializerOptions options = null, bool removeQuotationMarks = false, bool toSingleQuotes = false ) {
        return ToJson( value, options, removeQuotationMarks, toSingleQuotes, true );
    }

    /// <summary>
    /// Convierte un objeto a una representación JSON en forma de cadena.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a convertir a JSON.</typeparam>
    /// <param name="value">El objeto que se desea convertir a JSON. Si es nulo, se devuelve una cadena vacía.</param>
    /// <param name="options">Opciones de serialización que se aplicarán durante la conversión a JSON.</param>
    /// <param name="removeQuotationMarks">Indica si se deben eliminar las comillas dobles del resultado JSON.</param>
    /// <param name="toSingleQuotes">Indica si se deben reemplazar las comillas dobles por comillas simples en el resultado JSON.</param>
    /// <param name="ignoreInterface">Indica si se deben ignorar las interfaces durante la serialización.</param>
    /// <returns>
    /// Una cadena que representa el objeto en formato JSON. 
    /// Si el objeto es nulo, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método permite personalizar la salida JSON mediante opciones de serialización y 
    /// la posibilidad de modificar las comillas en el resultado.
    /// </remarks>
    private static string ToJson<T>( T value, JsonSerializerOptions options, bool removeQuotationMarks, bool toSingleQuotes, bool ignoreInterface ) {
        if ( value == null )
            return string.Empty;
        options = GetToJsonOptions( options );
        var result = Serialize( value, options, ignoreInterface );
        if ( removeQuotationMarks )
            result = result.Replace( "\"", "" );
        if ( toSingleQuotes )
            result = result.Replace( "\"", "'" );
        return result;
    }

    /// <summary>
    /// Obtiene las opciones de serialización JSON. Si se proporcionan opciones, se devuelven las opciones existentes; de lo contrario, se crean nuevas opciones con configuraciones predeterminadas.
    /// </summary>
    /// <param name="options">Las opciones de serialización JSON existentes. Puede ser <c>null</c>.</param>
    /// <returns>Las opciones de serialización JSON que se utilizarán.</returns>
    /// <remarks>
    /// Las opciones predeterminadas incluyen la ignorancia de propiedades con valor <c>null</c>, un codificador que permite todos los rangos Unicode y varios convertidores personalizados para tipos específicos.
    /// </remarks>
    private static JsonSerializerOptions GetToJsonOptions( JsonSerializerOptions options ) {
        if ( options != null )
            return options;
        return new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }

    /// <summary>
    /// Serializa un objeto en formato JSON utilizando las opciones de serialización especificadas.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a serializar.</typeparam>
    /// <param name="value">El objeto que se desea serializar.</param>
    /// <param name="options">Las opciones de serialización que se aplicarán durante el proceso.</param>
    /// <param name="ignoreInterface">Indica si se debe ignorar la interfaz del objeto durante la serialización.</param>
    /// <returns>Una cadena que representa el objeto serializado en formato JSON.</returns>
    /// <remarks>
    /// Si <paramref name="ignoreInterface"/> es verdadero, se convierte el objeto a un tipo de objeto genérico antes de la serialización.
    /// Si el objeto es nulo y <paramref name="ignoreInterface"/> es verdadero, se devolverá una cadena JSON nula.
    /// </remarks>
    /// <seealso cref="JsonSerializer"/>
    private static string Serialize<T>( T value, JsonSerializerOptions options, bool ignoreInterface ) {
        if ( ignoreInterface ) {
            object instance = value;
            if ( instance != null )
                return JsonSerializer.Serialize( instance, options );
        }
        return JsonSerializer.Serialize( value, options );
    }

    /// <summary>
    /// Convierte un objeto en una representación JSON de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a serializar.</typeparam>
    /// <param name="value">El objeto que se va a convertir a JSON. Si es null, se devuelve una cadena vacía.</param>
    /// <param name="options">Opciones de serialización JSON. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token de cancelación que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El resultado de la tarea es una cadena que contiene la representación JSON del objeto.
    /// </returns>
    /// <remarks>
    /// Este método utiliza un flujo de memoria para serializar el objeto y luego lee el contenido del flujo para devolverlo como una cadena.
    /// Si el objeto proporcionado es null, se devolverá una cadena vacía.
    /// </remarks>
    /// <seealso cref="JsonSerializer"/>
    public static async Task<string> ToJsonAsync<T>( T value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default ) {
        if ( value == null )
            return string.Empty;
        options = GetToJsonOptions( options );
        await using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync( stream, value, typeof( T ), options, cancellationToken );
        stream.Position = 0;
        using var reader = new StreamReader( stream );
        return await reader.ReadToEndAsync( cancellationToken );
    }

    /// <summary>
    /// Convierte una cadena JSON en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se desea convertir el JSON.</typeparam>
    /// <param name="json">La cadena JSON que se desea convertir.</param>
    /// <param name="options">Opciones de configuración para la deserialización.</param>
    /// <returns>Un objeto del tipo especificado, o el valor predeterminado si la cadena JSON es nula o vacía.</returns>
    /// <remarks>
    /// Este método verifica si la cadena JSON está vacía o es nula, en cuyo caso devuelve el valor predeterminado del tipo T.
    /// Si las opciones son nulas, se llama a una sobrecarga del método que no utiliza opciones.
    /// </remarks>
    /// <seealso cref="ToObject{T}(string)"/>
    /// <seealso cref="ToJsonSerializerOptions(JsonOptions)"/>
    public static T ToObject<T>( string json, JsonOptions options ) {
        if ( string.IsNullOrWhiteSpace( json ) )
            return default;
        if ( options == null )
            return ToObject<T>( json );
        return ToObject<T>( json, ToJsonSerializerOptions( options ) );
    }

    /// <summary>
    /// Convierte una cadena JSON en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se desea convertir el JSON.</typeparam>
    /// <param name="json">La cadena JSON que se desea deserializar.</param>
    /// <param name="options">Opciones de serialización personalizadas. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <returns>
    /// Un objeto del tipo especificado, o el valor predeterminado del tipo si la cadena JSON es nula o está vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="JsonSerializer"/> para deserializar el JSON.
    /// Si la cadena JSON es nula o está vacía, se devolverá el valor predeterminado del tipo T.
    /// </remarks>
    /// <seealso cref="JsonSerializer"/>
    public static T ToObject<T>( string json, JsonSerializerOptions options = null ) {
        if ( string.IsNullOrWhiteSpace( json ) )
            return default;
        options = GetToObjectOptions( options );
        return JsonSerializer.Deserialize<T>( json, options );
    }

    /// <summary>
    /// Convierte una cadena JSON en un objeto del tipo especificado.
    /// </summary>
    /// <param name="json">La cadena JSON que se desea convertir.</param>
    /// <param name="returnType">El tipo de objeto al que se desea convertir la cadena JSON.</param>
    /// <param name="options">Opciones de serialización JSON que se pueden personalizar. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <returns>Un objeto del tipo especificado que representa los datos de la cadena JSON, o el valor predeterminado si la cadena JSON está vacía o es nula.</returns>
    /// <remarks>
    /// Este método utiliza el <see cref="JsonSerializer"/> para deserializar la cadena JSON.
    /// Asegúrese de que la cadena JSON sea válida y que el tipo de retorno sea compatible con los datos JSON.
    /// </remarks>
    public static object ToObject( string json, Type returnType, JsonSerializerOptions options = null ) {
        if ( string.IsNullOrWhiteSpace( json ) )
            return default;
        options = GetToObjectOptions( options );
        return JsonSerializer.Deserialize( json, returnType, options );
    }

    /// <summary>
    /// Obtiene las opciones de deserialización de JSON.
    /// </summary>
    /// <param name="options">Opciones de serialización JSON proporcionadas. Si es <c>null</c>, se crearán opciones predeterminadas.</param>
    /// <returns>
    /// Un objeto <see cref="JsonSerializerOptions"/> que contiene las opciones de deserialización.
    /// </returns>
    /// <remarks>
    /// Si se proporcionan opciones, se devolverán tal cual. Si no se proporcionan, se crearán nuevas opciones con configuraciones predeterminadas,
    /// incluyendo la insensibilidad a mayúsculas y minúsculas en los nombres de propiedades, el manejo de números que permiten la lectura desde cadenas,
    /// y la inclusión de varios convertidores personalizados para tipos específicos.
    /// </remarks>
    private static JsonSerializerOptions GetToObjectOptions( JsonSerializerOptions options ) {
        if ( options != null )
            return options;
        return new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }

    /// <summary>
    /// Convierte un arreglo de bytes que representa un JSON en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo de objeto al que se desea convertir el JSON.</typeparam>
    /// <param name="json">El arreglo de bytes que contiene el JSON a deserializar.</param>
    /// <param name="options">Opciones de configuración para la deserialización. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <returns>Un objeto del tipo especificado, o el valor predeterminado si el arreglo de bytes es null.</returns>
    /// <remarks>
    /// Este método utiliza el <see cref="JsonSerializer"/> para deserializar el JSON.
    /// Asegúrese de que el JSON sea válido y coincida con la estructura del tipo T.
    /// </remarks>
    /// <seealso cref="JsonSerializer"/>
    public static T ToObject<T>( byte[] json, JsonSerializerOptions options = null ) {
        if ( json == null )
            return default;
        options = GetToObjectOptions( options );
        return JsonSerializer.Deserialize<T>( json, options );
    }

    /// <summary>
    /// Deserializa un flujo JSON en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se desea deserializar el flujo JSON.</typeparam>
    /// <param name="json">El flujo que contiene el JSON a deserializar.</param>
    /// <param name="options">Opciones de configuración para el deserializador JSON. Si es null, se utilizarán las opciones predeterminadas.</param>
    /// <returns>
    /// Un objeto del tipo especificado que representa el contenido del flujo JSON. 
    /// Si el flujo es null, se devuelve el valor predeterminado del tipo T.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el <see cref="JsonSerializer"/> para realizar la deserialización. 
    /// Asegúrese de que el flujo JSON esté en un formato válido para evitar excepciones durante la deserialización.
    /// </remarks>
    public static T ToObject<T>( Stream json, JsonSerializerOptions options = null ) {
        if ( json == null )
            return default;
        options = GetToObjectOptions( options );
        return JsonSerializer.Deserialize<T>( json, options );
    }

    /// <summary>
    /// Convierte una cadena JSON en un objeto de tipo especificado de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se desea convertir el JSON.</typeparam>
    /// <param name="json">La cadena JSON que se desea convertir.</param>
    /// <param name="options">Opciones de serialización que se utilizarán durante la conversión. Puede ser <c>null</c>.</param>
    /// <param name="encoding">La codificación que se utilizará para convertir la cadena JSON en bytes. Si es <c>null</c>, se utilizará <see cref="Encoding.UTF8"/> por defecto.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto del tipo especificado, o <c>default</c> si la cadena JSON está vacía o es nula.</returns>
    /// <remarks>
    /// Este método es útil para deserializar cadenas JSON en objetos de manera eficiente y permite la personalización de la codificación y las opciones de serialización.
    /// </remarks>
    /// <seealso cref="ToObjectAsync{T}(Stream, JsonSerializerOptions, CancellationToken)"/>
    public static async Task<T> ToObjectAsync<T>( string json, JsonSerializerOptions options = null, Encoding encoding = null, CancellationToken cancellationToken = default ) {
        if ( string.IsNullOrWhiteSpace( json ) )
            return default;
        encoding ??= Encoding.UTF8;
        byte[] bytes = encoding.GetBytes( json );
        await using var stream = new MemoryStream( bytes );
        return await ToObjectAsync<T>( stream, options, cancellationToken );
    }

    /// <summary>
    /// Deserializa un flujo JSON en un objeto del tipo especificado de manera asíncrona.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a deserializar.</typeparam>
    /// <param name="json">El flujo de datos que contiene el JSON a deserializar.</param>
    /// <param name="options">Opciones de configuración para el deserializador JSON. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto del tipo especificado deserializado del flujo JSON, o <c>default</c> si el flujo es <c>null</c>.</returns>
    /// <remarks>
    /// Este método utiliza el <see cref="JsonSerializer"/> para realizar la deserialización.
    /// Asegúrese de que el flujo JSON sea válido y que coincida con la estructura del tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <exception cref="JsonException">Se produce si el JSON no se puede deserializar en el tipo especificado.</exception>
    public static async Task<T> ToObjectAsync<T>( Stream json, JsonSerializerOptions options = null, CancellationToken cancellationToken = default ) {
        if ( json == null )
            return default;
        options = GetToObjectOptions( options );
        return await JsonSerializer.DeserializeAsync<T>( json, options, cancellationToken );
    }

    /// <summary>
    /// Convierte un arreglo de bytes que representa un JSON en un objeto del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto al que se desea convertir el JSON.</typeparam>
    /// <param name="json">El arreglo de bytes que contiene el JSON a convertir.</param>
    /// <param name="options">Opciones de serialización que se aplicarán durante la conversión. Puede ser null.</param>
    /// <param name="cancellationToken">Token que puede ser utilizado para cancelar la operación asíncrona.</param>
    /// <returns>Un objeto del tipo especificado, o el valor predeterminado si el arreglo de bytes es null.</returns>
    /// <remarks>
    /// Este método es asíncrono y utiliza un flujo de memoria para realizar la conversión.
    /// Si el parámetro <paramref name="json"/> es null, se devolverá el valor predeterminado del tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <seealso cref="ToObjectAsync{T}(Stream, JsonSerializerOptions, CancellationToken)"/>
    public static async Task<T> ToObjectAsync<T>( byte[] json, JsonSerializerOptions options = null, CancellationToken cancellationToken = default ) {
        if ( json == null )
            return default;
        await using var stream = new MemoryStream( json );
        return await ToObjectAsync<T>( stream, options, cancellationToken );
    }

    /// <summary>
    /// Serializa un objeto de tipo <typeparamref name="T"/> a un arreglo de bytes utilizando opciones de serialización JSON.
    /// </summary>
    /// <typeparam name="T">El tipo del objeto que se va a serializar.</typeparam>
    /// <param name="value">El objeto que se desea convertir a un arreglo de bytes.</param>
    /// <param name="options">Opciones de serialización JSON que se aplicarán durante la conversión. Si es <c>null</c>, se utilizarán las opciones predeterminadas.</param>
    /// <returns>
    /// Un arreglo de bytes que representa el objeto serializado en formato JSON.
    /// </returns>
    /// <remarks>
    /// Este método utiliza <see cref="JsonSerializer.SerializeToUtf8Bytes"/> para realizar la serialización.
    /// </remarks>
    public static byte[] ToBytes<T>( T value, JsonSerializerOptions options = null ) {
        options = GetToBytesOptions( options );
        return JsonSerializer.SerializeToUtf8Bytes( value, options );
    }

    /// <summary>
    /// Obtiene las opciones de serialización a bytes para JSON.
    /// </summary>
    /// <param name="options">Opciones de serialización JSON existentes. Si es <c>null</c>, se crearán nuevas opciones predeterminadas.</param>
    /// <returns>Las opciones de serialización JSON, ya sea las proporcionadas o las nuevas opciones predeterminadas.</returns>
    /// <remarks>
    /// Si se proporcionan opciones, se devolverán tal cual. Si no se proporcionan, se crearán nuevas opciones con un codificador que permite todos los rangos Unicode,
    /// y se agregarán convertidores personalizados para manejar tipos de datos específicos como <c>DateTime</c> y <c>long</c>.
    /// </remarks>
    /// <seealso cref="JsonSerializerOptions"/>
    /// <seealso cref="JavaScriptEncoder"/>
    /// <seealso cref="DateTimeJsonConverter"/>
    /// <seealso cref="NullableDateTimeJsonConverter"/>
    /// <seealso cref="LongJsonConverter"/>
    /// <seealso cref="NullableLongJsonConverter"/>
    private static JsonSerializerOptions GetToBytesOptions( JsonSerializerOptions options ) {
        if ( options != null )
            return options;
        return new JsonSerializerOptions {
            Encoder = JavaScriptEncoder.Create( UnicodeRanges.All ),
            Converters = {
                new DateTimeJsonConverter(),
                new NullableDateTimeJsonConverter(),
                new LongJsonConverter(),
                new NullableLongJsonConverter()
            }
        };
    }
}