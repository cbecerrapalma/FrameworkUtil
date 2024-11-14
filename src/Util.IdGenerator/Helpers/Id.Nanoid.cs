namespace Util.Helpers;

/// <summary>
/// Clase estática que proporciona métodos relacionados con la gestión de identificadores.
/// </summary>
public static partial class Id {
    /// <summary>
    /// Crea un nuevo NanoID con una longitud predeterminada de 21 caracteres.
    /// </summary>
    /// <returns>
    /// Un string que representa el NanoID generado.
    /// </returns>
    public static string CreateNanoid() {
        return CreateNanoid( 21 );
    }

    /// <summary>
    /// Crea un nuevo NanoID de forma asíncrona con una longitud predeterminada.
    /// </summary>
    /// <returns>
    /// Una tarea que representa la operación asíncrona, con un resultado que contiene el NanoID generado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una longitud predeterminada de 21 caracteres para el NanoID.
    /// </remarks>
    public static async Task<string> CreateNanoidAsync() {
        return await CreateNanoidAsync( 21 );
    }

    /// <summary>
    /// Crea un NanoID de la longitud especificada.
    /// </summary>
    /// <param name="size">El tamaño del NanoID a generar.</param>
    /// <returns>Un string que representa el NanoID generado.</returns>
    public static string CreateNanoid( int size ) {
        return CreateNanoid( null, size );
    }

    /// <summary>
    /// Crea un Nanoid de forma asíncrona con el tamaño especificado.
    /// </summary>
    /// <param name="size">El tamaño del Nanoid que se desea generar.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona. El valor de la tarea es un string que contiene el Nanoid generado.
    /// </returns>
    /// <remarks>
    /// Este método es una sobrecarga que llama a otro método de creación de Nanoid con un argumento adicional que se establece en null.
    /// </remarks>
    public static async Task<string> CreateNanoidAsync( int size ) {
        return await CreateNanoidAsync( null, size );
    }

    /// <summary>
    /// Crea un NanoID utilizando un alfabeto específico y una longitud predeterminada.
    /// </summary>
    /// <param name="alphabet">Una cadena que representa el alfabeto a utilizar para generar el NanoID.</param>
    /// <returns>
    /// Un NanoID generado como una cadena de caracteres de la longitud predeterminada.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una longitud predeterminada de 21 caracteres para el NanoID.
    /// </remarks>
    /// <seealso cref="CreateNanoid(string, int)"/>
    public static string CreateNanoid( string alphabet ) {
        return CreateNanoid( alphabet, 21 );
    }

    /// <summary>
    /// Crea un identificador único utilizando un alfabeto especificado y una longitud predeterminada.
    /// </summary>
    /// <param name="alphabet">Una cadena que representa el conjunto de caracteres que se utilizarán para generar el identificador.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica. El resultado de la tarea es un identificador único en forma de cadena.
    /// </returns>
    /// <remarks>
    /// Este método utiliza una longitud predeterminada de 21 caracteres para el identificador generado.
    /// </remarks>
    /// <seealso cref="CreateNanoidAsync(string, int)"/>
    public static async Task<string> CreateNanoidAsync( string alphabet ) {
        return await CreateNanoidAsync( alphabet, 21 );
    }

    /// <summary>
    /// Crea un identificador único utilizando un alfabeto y un tamaño especificados.
    /// </summary>
    /// <param name="alphabet">Una cadena que representa el alfabeto a utilizar para generar el identificador. Si está vacío o es nulo, se utilizará un alfabeto predeterminado.</param>
    /// <param name="size">Un entero que representa el tamaño del identificador a generar.</param>
    /// <returns>
    /// Un identificador único como una cadena. Si ya existe un identificador previamente generado, se devolverá ese valor.
    /// </returns>
    /// <remarks>
    /// Este método verifica primero si ya se ha generado un identificador previamente. Si es así, devuelve ese valor. 
    /// Si no, genera un nuevo identificador utilizando el alfabeto y el tamaño especificados. 
    /// Si el alfabeto está vacío o es nulo, se utilizará un método de generación de identificadores predeterminado.
    /// </remarks>
    public static string CreateNanoid( string alphabet, int size ) {
        if( string.IsNullOrEmpty( _id.Value ) == false )
            return _id.Value;
        if( string.IsNullOrEmpty( alphabet ) )
            return Nanoid.Generate( size: size );
        return Nanoid.Generate( alphabet, size );
    }

    /// <summary>
    /// Crea un Nano ID de forma asíncrona utilizando un alfabeto y un tamaño especificados.
    /// </summary>
    /// <param name="alphabet">El alfabeto a utilizar para generar el Nano ID. Si es nulo o vacío, se utilizará el alfabeto predeterminado.</param>
    /// <param name="size">El tamaño del Nano ID a generar.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un resultado que es el Nano ID generado.</returns>
    /// <remarks>
    /// Si el valor de _id es diferente de null o vacío, se devolverá ese valor sin generar un nuevo Nano ID.
    /// Si el alfabeto proporcionado es nulo o vacío, se generará un Nano ID utilizando el tamaño especificado y el alfabeto predeterminado.
    /// </remarks>
    public static async Task<string> CreateNanoidAsync( string alphabet, int size ) {
        if( string.IsNullOrEmpty( _id.Value ) == false )
            return _id.Value;
        if( string.IsNullOrEmpty( alphabet ) )
            return await Nanoid.GenerateAsync( size: size );
        return await Nanoid.GenerateAsync( alphabet, size );
    }

    /// <summary>
    /// Crea un identificador único utilizando el algoritmo Nanoid.
    /// </summary>
    /// <param name="random">Una instancia de <see cref="System.Random"/> utilizada para generar números aleatorios.</param>
    /// <param name="alphabet">Una cadena que representa el conjunto de caracteres que se pueden utilizar en el identificador. Por defecto incluye caracteres alfanuméricos y algunos símbolos.</param>
    /// <param name="size">El tamaño del identificador a generar. Por defecto es 21.</param>
    /// <returns>
    /// Un identificador único en forma de cadena.
    /// </returns>
    /// <remarks>
    /// Si ya existe un identificador almacenado en <c>_id.Value</c>, se devolverá ese valor en lugar de generar uno nuevo.
    /// </remarks>
    /// <seealso cref="Nanoid.Generate(System.Random, string, int)"/>
    public static string CreateNanoid( System.Random random, string alphabet = "_-0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", int size = 21 ) {
        if( string.IsNullOrEmpty( _id.Value ) == false )
            return _id.Value;
        return Nanoid.Generate( random, alphabet, size );
    }
}