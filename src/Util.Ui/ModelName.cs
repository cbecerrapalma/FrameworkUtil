namespace Util.Ui;

/// <summary>
/// Clase estática que representa el modelo de datos para <c>ModelName</c>.
/// </summary>
/// <remarks>
/// Esta clase contiene métodos y propiedades estáticas que permiten manipular y acceder a los datos relacionados con <c>ModelName</c>.
/// </remarks>
public static class ModelName {
    private static readonly AsyncLocal<Dictionary<Type, string>> _modelNames = new();

    /// <summary>
    /// Agrega un nombre de modelo a la colección de nombres de modelos.
    /// </summary>
    /// <typeparam name="T">El tipo del modelo que se está agregando.</typeparam>
    /// <param name="name">El nombre del modelo que se desea asociar al tipo especificado.</param>
    /// <remarks>
    /// Este método inicializa la colección de nombres de modelos si aún no ha sido creada,
    /// y luego agrega una entrada que asocia el tipo <typeparamref name="T"/> con el nombre proporcionado.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Se lanza si el parámetro <paramref name="name"/> es nulo.</exception>
    /// <seealso cref="Remove{T}(string)"/>
    public static void Add<T>( string name ) {
        _modelNames.Value ??= new Dictionary<Type, string>();
        _modelNames.Value.Add( typeof( T ), name );
    }

    /// <summary>
    /// Obtiene una representación en forma de cadena del tipo especificado.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener la representación.</typeparam>
    /// <returns>
    /// Una cadena que representa el tipo especificado.
    /// </returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el tipo y generar su representación en forma de cadena.
    /// </remarks>
    /// <seealso cref="Get(Type)"/>
    public static string Get<T>() {
        return Get( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el nombre del modelo asociado a un tipo específico.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el nombre del modelo.</param>
    /// <returns>
    /// El nombre del modelo asociado al tipo especificado, o null si no se encuentra.
    /// </returns>
    public static string Get(Type type) {
        return _modelNames.Value?.GetValue( type );
    }
}