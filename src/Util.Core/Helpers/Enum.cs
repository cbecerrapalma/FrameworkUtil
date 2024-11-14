using Util.Properties;

namespace Util.Helpers; 

/// <summary>
/// Clase estática que proporciona métodos relacionados con enumeraciones.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos que permiten trabajar con enumeraciones de manera más eficiente,
/// facilitando la conversión y manipulación de valores de tipo enum.
/// </remarks>
public static class Enum {
    /// <summary>
    /// Analiza un valor de cadena y lo convierte en un valor del tipo enumerado especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo del enumerado al que se desea convertir el valor.</typeparam>
    /// <param name="member">El objeto que contiene el valor a analizar.</param>
    /// <returns>Un valor del tipo enumerado especificado que representa el valor analizado.</returns>
    /// <exception cref="ArgumentNullException">Se lanza cuando el valor proporcionado es null o una cadena vacía.</exception>
    /// <remarks>
    /// Este método utiliza el método <see cref="System.Enum.Parse(Type, string, bool)"/> 
    /// para realizar la conversión, ignorando mayúsculas y minúsculas.
    /// Si el valor proporcionado es una cadena vacía o solo contiene espacios en blanco,
    /// se devolverá el valor predeterminado del tipo enumerado si este es un tipo genérico.
    /// </remarks>
    public static TEnum Parse<TEnum>( object member ) {
        string value = member.SafeString();
        if( string.IsNullOrWhiteSpace( value ) ) {
            if( typeof( TEnum ).IsGenericType )
                return default( TEnum );
            throw new ArgumentNullException( nameof( member ) );
        }
        return (TEnum)System.Enum.Parse( Common.GetType<TEnum>(), value, true );
    }

    /// <summary>
    /// Obtiene el nombre de un miembro de un enumerador especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo del enumerador del cual se desea obtener el nombre.</typeparam>
    /// <param name="member">El miembro del enumerador cuyo nombre se desea recuperar.</param>
    /// <returns>
    /// El nombre del miembro del enumerador como una cadena.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el tipo de enumerador proporcionado por el parámetro genérico <typeparamref name="TEnum"/> 
    /// y el objeto <paramref name="member"/> para devolver el nombre correspondiente.
    /// </remarks>
    /// <seealso cref="Common.GetType{T}"/>
    public static string GetName<TEnum>( object member ) {
        return GetName( Common.GetType<TEnum>(), member );
    }

    /// <summary>
    /// Obtiene el nombre de un miembro de un tipo dado, que puede ser un valor de enumeración.
    /// </summary>
    /// <param name="type">El tipo del que se desea obtener el nombre del miembro. Debe ser un tipo de enumeración.</param>
    /// <param name="member">El valor del miembro cuyo nombre se desea obtener. Puede ser un objeto de tipo string o un valor de la enumeración.</param>
    /// <returns>
    /// Devuelve el nombre del miembro si se encuentra, de lo contrario, devuelve una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo es nulo o si el miembro es nulo. Si el miembro es de tipo string, se devuelve directamente su valor.
    /// Si el tipo no es una enumeración, se devuelve una cadena vacía. Si el miembro es un valor válido de la enumeración, se devuelve su nombre.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se lanza si el tipo es nulo o el miembro es nulo.
    /// </exception>
    public static string GetName( Type type, object member ) {
        if( type == null )
            return string.Empty;
        if( member == null )
            return string.Empty;
        if( member is string )
            return member.ToString();
        if( type.GetTypeInfo().IsEnum == false )
            return string.Empty;
        return System.Enum.GetName( type, member );
    }

    /// <summary>
    /// Obtiene el valor asociado a un miembro de un tipo enumerado especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo de enumeración del cual se desea obtener el valor.</typeparam>
    /// <param name="member">El miembro del cual se desea obtener el valor. Este puede ser un nombre de miembro o un objeto que representa el miembro.</param>
    /// <returns>
    /// Un valor entero que representa el valor del miembro de la enumeración, o null si el miembro no es válido.
    /// </returns>
    /// <remarks>
    /// Este método utiliza el tipo de enumeración proporcionado por el parámetro genérico <typeparamref name="TEnum"/> 
    /// para buscar el valor correspondiente al miembro especificado.
    /// </remarks>
    /// <seealso cref="Common.GetType{T}"/>
    public static int? GetValue<TEnum>( object member ) {
        return GetValue( Common.GetType<TEnum>(), member );
    }

    /// <summary>
    /// Obtiene un valor de tipo nullable int a partir de un miembro y un tipo de enumeración especificado.
    /// </summary>
    /// <param name="type">El tipo de enumeración del cual se desea obtener el valor.</param>
    /// <param name="member">El objeto que contiene el valor a convertir.</param>
    /// <returns>
    /// Un valor nullable de tipo int que representa el valor del miembro convertido, 
    /// o null si el valor del miembro es una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método intenta convertir el valor del miembro a una enumeración del tipo especificado.
    /// Si el valor es una cadena vacía, se devuelve null.
    /// </remarks>
    /// <exception cref="ArgumentException">Se lanza si el tipo proporcionado no es una enumeración.</exception>
    /// <exception cref="ArgumentNullException">Se lanza si el tipo o el miembro son nulos.</exception>
    /// <seealso cref="System.Enum"/>
    /// <seealso cref="Convert.ToInt32(object)"/>
    public static int? GetValue( Type type, object member ) {
        string value = member.SafeString();
        if ( value.IsEmpty() )
            return null;
        var result = System.Enum.Parse( type, value, true );
        return Convert.To<int?>( result );
    }

    /// <summary>
    /// Obtiene la descripción de un miembro de un enumerador especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo del enumerador del cual se desea obtener la descripción.</typeparam>
    /// <param name="member">El miembro del enumerador del cual se quiere obtener la descripción.</param>
    /// <returns>
    /// La descripción del miembro del enumerador, si está disponible; de lo contrario, una cadena vacía.
    /// </returns>
    /// <remarks>
    /// Este método utiliza reflexión para recuperar la descripción asociada al miembro del enumerador.
    /// Asegúrese de que el miembro proporcionado sea un valor válido del enumerador especificado.
    /// </remarks>
    /// <seealso cref="Reflection.GetDescription{TEnum}(string)"/>
    /// <seealso cref="GetName{TEnum}(object)"/>
    public static string GetDescription<TEnum>( object member ) {
        return Reflection.GetDescription<TEnum>( GetName<TEnum>( member ) );
    }

    /// <summary>
    /// Obtiene la descripción de un miembro de un tipo específico.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener la descripción del miembro.</param>
    /// <param name="member">El miembro del cual se desea obtener la descripción.</param>
    /// <returns>
    /// Una cadena que representa la descripción del miembro especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la clase <see cref="Reflection"/> para obtener la descripción
    /// a partir del nombre del miembro obtenido mediante el método <see cref="GetName(Type, object)"/>.
    /// </remarks>
    public static string GetDescription( Type type, object member ) {
        return Reflection.GetDescription( type, GetName( type, member ) );
    }

    /// <summary>
    /// Obtiene una lista de elementos de tipo <see cref="Item"/> basada en el tipo de enumeración especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo de enumeración del cual se obtendrán los elementos.</typeparam>
    /// <returns>
    /// Una lista de elementos de tipo <see cref="Item"/> que corresponden a los valores de la enumeración especificada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener los valores de la enumeración y convertirlos en una lista de elementos.
    /// Asegúrese de que el tipo de enumeración proporcionado tenga un mapeo adecuado a los elementos de tipo <see cref="Item"/>.
    /// </remarks>
    /// <seealso cref="Item"/>
    public static List<Item> GetItems<TEnum>() {
        return GetItems( typeof( TEnum ) );
    }

    /// <summary>
    /// Obtiene una lista de elementos a partir de un tipo especificado.
    /// </summary>
    /// <param name="type">El tipo del cual se obtendrán los elementos. Este tipo debe ser un enumerador.</param>
    /// <returns>
    /// Una lista de objetos <see cref="Item"/> que representan los elementos del enumerador especificado,
    /// ordenados por su identificador de orden (SortId).
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el tipo especificado no es un enumerador.
    /// </exception>
    /// <remarks>
    /// Este método utiliza el método <see cref="Common.GetType(Type)"/> para obtener el tipo correcto,
    /// y luego itera sobre los campos del enumerador para agregar cada uno a la lista de resultados.
    /// </remarks>
    public static List<Item> GetItems( Type type ) {
        type = Common.GetType( type );
        if( type.IsEnum == false )
            throw new InvalidOperationException( string.Format( R.TypeNotEnum,type ) );
        var result = new List<Item>();
        foreach( var field in type.GetFields() )
            AddItem( type, result, field );
        return result.OrderBy( t => t.SortId ).ToList();
    }

    /// <summary>
    /// Agrega un elemento a la colección de resultados si el campo proporcionado es de tipo enumeración.
    /// </summary>
    /// <param name="type">El tipo del que se está extrayendo el valor del campo.</param>
    /// <param name="result">La colección donde se agregará el nuevo elemento.</param>
    /// <param name="field">El campo que se está evaluando para determinar si es de tipo enumeración.</param>
    private static void AddItem( Type type, ICollection<Item> result, FieldInfo field ) {
        if( !field.FieldType.IsEnum )
            return;
        var value = GetValue( type, field.Name );
        var description = Reflection.GetDescription( field );
        result.Add( new Item( description, value, value ) );
    }

    /// <summary>
    /// Obtiene una lista de los nombres de los miembros de un enumerador especificado.
    /// </summary>
    /// <typeparam name="TEnum">El tipo del enumerador del cual se desean obtener los nombres.</typeparam>
    /// <returns>
    /// Una lista de cadenas que contiene los nombres de los miembros del enumerador especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para acceder a los nombres de los miembros del enumerador.
    /// Asegúrese de que el tipo proporcionado sea un enumerador.
    /// </remarks>
    /// <seealso cref="Enum.GetNames(Type)"/>
    public static List<string> GetNames<TEnum>() {
        return GetNames( typeof( TEnum ) );
    }

    /// <summary>
    /// Obtiene una lista de los nombres de los campos de un tipo enumerado.
    /// </summary>
    /// <param name="type">El tipo del cual se desean obtener los nombres de los campos. Debe ser un tipo enumerado.</param>
    /// <returns>
    /// Una lista de cadenas que contiene los nombres de los campos del tipo enumerado especificado.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza si el tipo proporcionado no es un tipo enumerado.
    /// </exception>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener los campos del tipo especificado. 
    /// Solo se agregarán a la lista aquellos campos que también sean de tipo enumerado.
    /// </remarks>
    /// <seealso cref="Common.GetType(Type)"/>
    public static List<string> GetNames( Type type ) {
        type = Common.GetType( type );
        if( type.IsEnum == false )
            throw new InvalidOperationException( string.Format( R.TypeNotEnum, type ) );
        var result = new List<string>();
        foreach ( var field in type.GetFields() ) {
            if( !field.FieldType.IsEnum )
                continue;
            result.Add( field.Name );
        }
        return result;
    }
}