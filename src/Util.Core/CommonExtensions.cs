namespace Util; 

/// <summary>
/// Proporciona métodos de extensión comunes para tipos de datos.
/// </summary>
public static class CommonExtensions {
    /// <summary>
    /// Devuelve el valor de un tipo nullable si está presente; de lo contrario, devuelve el valor predeterminado del tipo.
    /// </summary>
    /// <typeparam name="T">El tipo de valor que se está manejando, que debe ser un tipo de valor (struct).</typeparam>
    /// <param name="value">El valor nullable que se va a evaluar.</param>
    /// <returns>El valor de <paramref name="value"/> si no es nulo; de lo contrario, el valor predeterminado de <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Este método es útil para manejar valores que pueden ser nulos sin necesidad de realizar comprobaciones adicionales.
    /// </remarks>
    /// <seealso cref="Nullable{T}"/>
    public static T SafeValue<T>( this T? value ) where T : struct {
        return value ?? default;
    }

    /// <summary>
    /// Obtiene el valor asociado a una instancia de un enumerador.
    /// </summary>
    /// <param name="instance">La instancia del enumerador del cual se desea obtener el valor.</param>
    /// <returns>
    /// Un entero que representa el valor del enumerador, o null si la instancia es null.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la clase <see cref="System.Enum"/>.
    /// Utiliza la clase <see cref="Util.Helpers.Enum"/> para obtener el valor del enumerador.
    /// </remarks>
    public static int? Value( this System.Enum instance ) {
        if( instance == null )
            return null;
        return Util.Helpers.Enum.GetValue( instance.GetType(), instance );
    }

    /// <summary>
    /// Convierte el valor de una instancia de <see cref="System.Enum"/> a un tipo especificado.
    /// </summary>
    /// <typeparam name="TResult">El tipo al que se desea convertir el valor de la enumeración.</typeparam>
    /// <param name="instance">La instancia de <see cref="System.Enum"/> que se va a convertir.</param>
    /// <returns>El valor convertido al tipo especificado. Si la instancia es <c>null</c>, se devuelve el valor predeterminado de <typeparamref name="TResult"/>.</returns>
    /// <remarks>
    /// Este método es una extensión que permite realizar conversiones de manera más sencilla
    /// desde enumeraciones a otros tipos, utilizando un método de conversión auxiliar.
    /// </remarks>
    /// <seealso cref="System.Enum"/>
    /// <seealso cref="Util.Helpers.Convert"/>
    public static TResult Value<TResult>( this System.Enum instance ) {
        if( instance == null )
            return default;
        return Util.Helpers.Convert.To<TResult>( Value( instance ) );
    }

    /// <summary>
    /// Obtiene la descripción de un valor de enumeración.
    /// </summary>
    /// <param name="instance">La instancia de la enumeración de la cual se desea obtener la descripción.</param>
    /// <returns>
    /// Una cadena que representa la descripción del valor de la enumeración. 
    /// Si la instancia es nula, se devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método extiende la funcionalidad de la clase <see cref="System.Enum"/> 
    /// permitiendo obtener descripciones personalizadas definidas en los atributos de la enumeración.
    /// </remarks>
    public static string Description( this System.Enum instance ) {
        if ( instance == null )
            return string.Empty;
        return Util.Helpers.Enum.GetDescription( instance.GetType(), instance );
    }

    /// <summary>
    /// Une los elementos de una colección en una cadena, utilizando un separador y opcionalmente agregando comillas alrededor de cada elemento.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="list">La colección de elementos que se van a unir.</param>
    /// <param name="quotes">Las comillas que se agregarán alrededor de cada elemento. Por defecto es una cadena vacía.</param>
    /// <param name="separator">El separador que se utilizará entre los elementos. Por defecto es una coma.</param>
    /// <returns>
    /// Una cadena que representa todos los elementos de la colección unidos por el separador especificado, 
    /// con comillas alrededor de cada elemento si se especifica.
    /// </returns>
    /// <remarks>
    /// Este método es una extensión para la interfaz <see cref="IEnumerable{T}"/> y permite una forma sencilla de 
    /// concatenar los elementos de una lista en un formato legible.
    /// </remarks>
    /// <seealso cref="Util.Helpers.String.Join(IEnumerable{T}, string, string)"/>
    public static string Join<T>( this IEnumerable<T> list, string quotes = "", string separator = "," ) {
        return Util.Helpers.String.Join( list, quotes, separator );
    }
}