namespace Util.SystemTextJson; 

/// <summary>
/// Clase que representa una fábrica de convertidores JSON para enumeraciones.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="JsonConverterFactory"/> y proporciona funcionalidad
/// para crear convertidores que manejan la serialización y deserialización de tipos de enumeración
/// en formato JSON.
/// </remarks>
public class EnumJsonConverterFactory : JsonConverterFactory {
    /// <summary>
    /// Determina si el tipo especificado se puede convertir.
    /// </summary>
    /// <param name="type">El tipo que se va a verificar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el tipo es un enumerado; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public override bool CanConvert( Type type ) {
        return type.IsEnum;
    }

    /// <summary>
    /// Crea un convertidor JSON para el tipo especificado.
    /// </summary>
    /// <param name="type">El tipo para el cual se desea crear el convertidor.</param>
    /// <param name="options">Las opciones de serialización JSON que se utilizarán.</param>
    /// <returns>Un convertidor JSON que se puede utilizar para serializar o deserializar el tipo especificado.</returns>
    public override JsonConverter CreateConverter( Type type, JsonSerializerOptions options ) {
        return (JsonConverter)Activator.CreateInstance( GetEnumConverterType( type ) );
    }

    /// <summary>
    /// Obtiene el tipo del convertidor de enumeraciones para el tipo de enumeración especificado.
    /// </summary>
    /// <param name="enumType">El tipo de enumeración para el cual se desea obtener el convertidor.</param>
    /// <returns>El tipo del convertidor de enumeraciones correspondiente al tipo de enumeración proporcionado.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para crear un tipo genérico basado en el convertidor de enumeraciones.
    /// Se asume que el tipo proporcionado es una enumeración válida.
    /// </remarks>
    private static Type GetEnumConverterType( Type enumType ) => typeof( EnumJsonConverter<> ).MakeGenericType( enumType );
}