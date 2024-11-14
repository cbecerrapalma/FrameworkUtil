#nullable enable
namespace Util; 

/// <summary>
/// Proporciona métodos de extensión para la validación de objetos.
/// </summary>
public static class ValidationExtensions {
    /// <summary>
    /// Verifica si el objeto proporcionado es nulo y lanza una excepción si lo es.
    /// </summary>
    /// <param name="obj">El objeto que se va a verificar.</param>
    /// <param name="parameterName">El nombre del parámetro que se utilizará en la excepción si el objeto es nulo.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando el objeto es nulo.</exception>
    public static void CheckNull( this object obj, string parameterName ) {
        if( obj == null )
            throw new ArgumentNullException( parameterName );
    }

    /// <summary>
    /// Determina si una cadena es vacía o contiene solo espacios en blanco.
    /// </summary>
    /// <param name="value">La cadena que se va a evaluar. Puede ser nula.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la cadena es nula, vacía o contiene solo espacios en blanco; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool IsEmpty( [NotNullWhen( false )] this string? value ) {
        return string.IsNullOrWhiteSpace( value );
    }

    /// <summary>
    /// Determina si el valor del identificador único global (GUID) es vacío.
    /// </summary>
    /// <param name="value">El GUID que se va a evaluar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el GUID es igual a <see cref="Guid.Empty"/>; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    public static bool IsEmpty( this Guid value ) {
        return value == Guid.Empty;
    }

    /// <summary>
    /// Determina si un valor de tipo <see cref="Guid?"/> es nulo o igual a <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="value">El valor de tipo <see cref="Guid?"/> que se va a evaluar.</param>
    /// <returns>
    /// <c>true</c> si el valor es nulo o igual a <see cref="Guid.Empty"/>; de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para el tipo nullable <see cref="Guid?"/> y permite verificar de manera sencilla
    /// si un GUID es considerado vacío.
    /// </remarks>
    public static bool IsEmpty( [NotNullWhen( false )] this Guid? value ) {
        if( value == null )
            return true;
        return value == Guid.Empty;
    }

    /// <summary>
    /// Determina si una colección de tipo <typeparamref name="T"/> está vacía o es nula.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="value">La colección a evaluar, que puede ser nula.</param>
    /// <returns>
    /// Devuelve <c>true</c> si la colección es nula o no contiene elementos; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IEnumerable{T}"/> y permite verificar fácilmente
    /// si una colección está vacía o no, evitando excepciones al intentar acceder a una colección nula.
    /// </remarks>
    public static bool IsEmpty<T>( this IEnumerable<T>? value ) {
        if( value == null )
            return true;
        return !value.Any();
    }

    /// <summary>
    /// Determina si el valor especificado es el valor predeterminado para su tipo.
    /// </summary>
    /// <typeparam name="T">El tipo del valor a evaluar.</typeparam>
    /// <param name="value">El valor que se va a comprobar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el valor es igual al valor predeterminado de su tipo; de lo contrario, devuelve <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el comparador de igualdad predeterminado para el tipo <typeparamref name="T"/> 
    /// para comparar el valor proporcionado con el valor predeterminado de ese tipo.
    /// </remarks>
    /// <seealso cref="EqualityComparer{T}"/>
    public static bool IsDefault<T>( this T value ) {
        return EqualityComparer<T>.Default.Equals( value, default );
    }
}