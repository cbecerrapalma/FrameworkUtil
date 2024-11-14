using System.Text.Json;
using Util.Domain.Extending;
using Util.Helpers;

namespace Util.Domain;

/// <summary>
/// Proporciona métodos de extensión para trabajar con diccionarios de propiedades adicionales.
/// </summary>
public static class ExtraPropertyDictionaryExtensions {
    /// <summary>
    /// Obtiene el valor de una propiedad del diccionario de propiedades adicionales.
    /// </summary>
    /// <typeparam name="TProperty">El tipo del valor de la propiedad que se desea obtener.</typeparam>
    /// <param name="source">El diccionario de propiedades adicionales desde el cual se obtendrá la propiedad.</param>
    /// <param name="name">El nombre de la propiedad que se desea obtener.</param>
    /// <returns>
    /// El valor de la propiedad especificada si se encuentra; de lo contrario, el valor predeterminado de <typeparamref name="TProperty"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="source"/> es null.</exception>
    /// <remarks>
    /// Este método intenta recuperar un valor del diccionario utilizando el nombre proporcionado. 
    /// Si el valor es un <see cref="JsonElement"/>, se convierte a un objeto del tipo especificado. 
    /// Si el valor no se encuentra, se devuelve el valor predeterminado del tipo especificado.
    /// </remarks>
    /// <seealso cref="ExtraPropertyDictionary"/>
    /// <seealso cref="Util.Helpers.Json"/>
    /// <seealso cref="Util.Helpers.Convert"/>
    public static TProperty GetProperty<TProperty>( this ExtraPropertyDictionary source, string name ) {
        source.CheckNull( nameof( source ) );
        if ( source.TryGetValue( name, out var value ) == false )
            return default;
        if ( value is JsonElement element )
            return Util.Helpers.Json.ToObject<TProperty>( Util.Helpers.Json.ToJson( element ) );
        return Util.Helpers.Convert.To<TProperty>( value );
    }

    /// <summary>
    /// Establece una propiedad en el diccionario de propiedades adicionales.
    /// </summary>
    /// <param name="source">El diccionario de propiedades adicionales en el que se establecerá la propiedad.</param>
    /// <param name="name">El nombre de la propiedad que se desea establecer.</param>
    /// <param name="value">El valor de la propiedad que se desea establecer. Si es <c>null</c>, la propiedad se eliminará.</param>
    /// <returns>El diccionario de propiedades adicionales actualizado.</returns>
    /// <remarks>
    /// Este método verifica si el diccionario de origen es nulo y lanza una excepción si es así.
    /// Luego, elimina cualquier propiedad existente con el mismo nombre antes de establecer la nueva propiedad.
    /// Si el valor proporcionado es <c>null</c>, la propiedad se elimina del diccionario.
    /// </remarks>
    public static ExtraPropertyDictionary SetProperty( this ExtraPropertyDictionary source, string name, object value ) {
        source.CheckNull( nameof( source ) );
        source.RemoveProperty( name );
        if ( value == null )
            return source;
        source[name] = GetPropertyValue( source, value );
        return source;
    }

    /// <summary>
    /// Obtiene el valor de una propiedad de un diccionario de propiedades adicionales,
    /// aplicando transformaciones específicas según el tipo de valor.
    /// </summary>
    /// <param name="source">El diccionario de propiedades adicionales desde el cual se obtiene el valor.</param>
    /// <param name="value">El valor de la propiedad que se va a procesar.</param>
    /// <returns>
    /// El valor procesado según su tipo. Si el valor es una cadena y el diccionario
    /// indica que se debe recortar, se devuelve la cadena recortada. Si el valor es
    /// un <see cref="DateTime"/>, se normaliza utilizando el método <see cref="Time.Normalize(DateTime)"/>.
    /// En caso contrario, se devuelve el valor original sin modificaciones.
    /// </returns>
    private static object GetPropertyValue( ExtraPropertyDictionary source, object value ) {
        if ( value is string && source.IsTrimString )
            return value.SafeString();
        if ( value is DateTime dateValue )
            return Time.Normalize( dateValue );
        return value;
    }

    /// <summary>
    /// Elimina una propiedad del diccionario de propiedades adicionales si existe.
    /// </summary>
    /// <param name="source">El diccionario de propiedades adicionales del cual se eliminará la propiedad.</param>
    /// <param name="name">El nombre de la propiedad que se desea eliminar.</param>
    /// <returns>
    /// El diccionario de propiedades adicionales actualizado después de la eliminación.
    /// </returns>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="source"/> es nulo.</exception>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="ExtraPropertyDictionary"/> que permite eliminar una propiedad 
    /// especificada por su nombre. Si la propiedad no existe en el diccionario, no se realiza ninguna acción.
    /// </remarks>
    public static ExtraPropertyDictionary RemoveProperty( this ExtraPropertyDictionary source, string name ) {
        source.CheckNull( nameof( source ) );
        if ( source.ContainsKey( name ) )
            source.Remove( name );
        return source;
    }
}