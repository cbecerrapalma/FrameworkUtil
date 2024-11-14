namespace Util.Helpers;

/// <summary>
/// Representa una clase que proporciona métodos para generar números aleatorios.
/// </summary>
public class Random {
    private readonly System.Random _random;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Random"/>.
    /// </summary>
    /// <remarks>
    /// Este constructor crea un generador de números aleatorios utilizando la clase <see cref="System.Random"/>.
    /// </remarks>
    public Random() {
        _random = new System.Random();
    }

    /// <summary>
    /// Genera un número entero aleatorio que es menor que el valor especificado.
    /// </summary>
    /// <param name="max">El valor máximo (exclusivo) que puede generar el número aleatorio.</param>
    /// <returns>
    /// Un número entero aleatorio menor que <paramref name="max"/>.
    /// </returns>
    public int Next( int max ) {
        return _random.Next( max );
    }

    /// <summary>
    /// Genera un número entero aleatorio dentro de un rango especificado.
    /// </summary>
    /// <param name="min">El límite inferior del rango, inclusivo.</param>
    /// <param name="max">El límite superior del rango, exclusivo.</param>
    /// <returns>Un número entero aleatorio que se encuentra entre <paramref name="min"/> y <paramref name="max"/>.</returns>
    /// <remarks>
    /// Este método utiliza una instancia de la clase <see cref="_random"/> para generar el número aleatorio.
    /// Asegúrese de que <paramref name="min"/> sea menor que <paramref name="max"/> para evitar excepciones.
    /// </remarks>
    public int Next( int min, int max ) {
        return _random.Next( min, max );
    }

    /// <summary>
    /// Obtiene un valor aleatorio de una colección enumerable.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="array">La colección de la que se obtendrá un valor aleatorio.</param>
    /// <returns>Un valor aleatorio de la colección si no es nula; de lo contrario, el valor predeterminado de <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// Si la colección proporcionada es nula, se devolverá el valor predeterminado del tipo <typeparamref name="T"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Se lanza si la colección está vacía.</exception>
    public static T GetValue<T>( IEnumerable<T> array ) {
        if ( array == null )
            return default;
        var list = array.ToList();
        var index = System.Random.Shared.Next( 0, list.Count );
        return list[index];
    }

    /// <summary>
    /// Obtiene un número específico de elementos aleatorios de una colección.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="array">La colección de la que se extraerán los elementos. Puede ser nula.</param>
    /// <param name="count">El número de elementos aleatorios a obtener.</param>
    /// <returns>
    /// Una lista que contiene hasta <paramref name="count"/> elementos aleatorios de <paramref name="array"/>.
    /// Si <paramref name="array"/> es nula, se devuelve una lista vacía.
    /// </returns>
    /// <remarks>
    /// Este método selecciona elementos aleatorios sin reemplazo, lo que significa que un elemento no puede ser seleccionado más de una vez.
    /// Si <paramref name="count"/> es mayor que el número de elementos en <paramref name="array"/>, se devolverán todos los elementos disponibles.
    /// </remarks>
    /// <seealso cref="System.Collections.Generic.List{T}"/>
    /// <seealso cref="System.Random"/>
    public static List<T> GetValues<T>( IEnumerable<T> array, int count ) {
        var result = new List<T>();
        if ( array == null )
            return result;
        var list = array.ToList();
        while ( list.Count > 0 && result.Count < count ) {
            var index = System.Random.Shared.Next( 0, list.Count );
            var item = list[index];
            result.Add( item );
            list.Remove( item );
        }
        return result;
    }

    /// <summary>
    /// Ordena aleatoriamente los elementos de una colección.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="array">La colección de elementos a ordenar aleatoriamente.</param>
    /// <returns>Una lista que contiene los elementos de la colección en un orden aleatorio, o null si la colección es null.</returns>
    /// <remarks>
    /// Este método utiliza un algoritmo de intercambio para mezclar los elementos de la colección.
    /// Se generan dos índices aleatorios y se intercambian los elementos en esos índices repetidamente.
    /// </remarks>
    /// <seealso cref="System.Collections.Generic.List{T}"/>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}"/>
    public static List<T> Sort<T>( IEnumerable<T> array ) {
        if ( array == null )
            return null;
        var list = array.ToList();
        for ( int i = 0; i < list.Count; i++ ) {
            int index1 = System.Random.Shared.Next( 0, list.Count );
            int index2 = System.Random.Shared.Next( 0, list.Count );
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }
        return list;
    }
}