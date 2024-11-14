using Util.Domain.Extending;

namespace Util.Data.EntityFrameworkCore.ValueComparers; 

/// <summary>
/// Compara dos instancias de <see cref="ExtraPropertyDictionary"/> para determinar su igualdad.
/// </summary>
/// <remarks>
/// Esta clase hereda de <see cref="ValueComparer{T}"/> y se utiliza para comparar valores de diccionarios
/// que contienen propiedades adicionales. Es útil en contextos donde se requiere verificar la igualdad
/// de objetos que implementan esta estructura de datos.
/// </remarks>
/// <typeparam name="ExtraPropertyDictionary">El tipo de diccionario que se está comparando.</typeparam>
public class ExtraPropertyDictionaryValueComparer : ValueComparer<ExtraPropertyDictionary> {
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ExtraPropertyDictionaryValueComparer"/>.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para comparar diccionarios de propiedades adicionales.
    /// La comparación se realiza mediante la conversión de los diccionarios a formato JSON y comparando las cadenas resultantes.
    /// Además, se proporciona una función para calcular el código hash de un diccionario de propiedades adicionales.
    /// </remarks>
    /// <param name="extraProperties1">El primer diccionario de propiedades adicionales a comparar.</param>
    /// <param name="extraProperties2">El segundo diccionario de propiedades adicionales a comparar.</param>
    /// <returns>Devuelve verdadero si los diccionarios son iguales, de lo contrario, devuelve falso.</returns>
    /// <seealso cref="ExtraPropertyDictionary"/>
    public ExtraPropertyDictionaryValueComparer()
        : base(
            ( extraProperties1, extraProperties2 ) => GetJson( extraProperties1 ) == GetJson( extraProperties2 ),
            extraProperties => extraProperties.Aggregate( 0, ( key, value ) => HashCode.Combine( key, value.GetHashCode() ) ),
            extraProperties => new ExtraPropertyDictionary( extraProperties ) ) {
    }

    /// <summary>
    /// Convierte un diccionario de propiedades adicionales en una cadena JSON.
    /// </summary>
    /// <param name="extraProperties">El diccionario de propiedades adicionales que se desea convertir a JSON.</param>
    /// <returns>Una cadena que representa el diccionario de propiedades adicionales en formato JSON.</returns>
    private static string GetJson( ExtraPropertyDictionary extraProperties ) {
        var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        return Util.Helpers.Json.ToJson( extraProperties, options );
    }
}