namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para la clase <see cref="Dictionary{TKey, TValue}"/>.
/// </summary>
/// <remarks>
/// Esta clase está diseñada para facilitar la manipulación de diccionarios en C#,
/// añadiendo funcionalidades adicionales que no están disponibles en la implementación
/// estándar de los diccionarios.
/// </remarks>
public static class DictionaryExtensions {
    /// <summary>
    /// Obtiene el valor asociado a la clave especificada en el diccionario.
    /// </summary>
    /// <typeparam name="TKey">El tipo de las claves en el diccionario.</typeparam>
    /// <typeparam name="TValue">El tipo de los valores en el diccionario.</typeparam>
    /// <param name="source">El diccionario del cual se obtendrá el valor.</param>
    /// <param name="key">La clave cuyo valor se desea obtener.</param>
    /// <returns>
    /// El valor asociado a la clave especificada si existe; de lo contrario, el valor predeterminado de <typeparamref name="TValue"/>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IDictionary{TKey, TValue}"/>.
    /// Si el diccionario es nulo, se devuelve el valor predeterminado de <typeparamref name="TValue"/>.
    /// </remarks>
    public static TValue GetValue<TKey, TValue>( this IDictionary<TKey, TValue> source, TKey key ) {
        if ( source == null )
            return default;
        return source.TryGetValue( key, out var obj ) ? obj : default;
    }
}