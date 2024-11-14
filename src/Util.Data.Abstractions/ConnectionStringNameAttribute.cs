namespace Util.Data; 

/// <summary>
/// Atributo que se utiliza para especificar el nombre de la cadena de conexión.
/// </summary>
/// <remarks>
/// Este atributo se puede aplicar a clases o propiedades para indicar el nombre de la cadena de conexión 
/// que debe utilizarse al establecer una conexión con una base de datos.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false)]
public class ConnectionStringNameAttribute : Attribute {
    /// <summary>
    /// Obtiene el nombre asociado a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y devuelve un valor de tipo cadena que representa el nombre.
    /// </remarks>
    /// <returns>
    /// Una cadena que contiene el nombre.
    /// </returns>
    public string Name { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ConnectionStringNameAttribute"/>.
    /// </summary>
    /// <param name="name">El nombre de la cadena de conexión.</param>
    public ConnectionStringNameAttribute( string name ) {
        Name = name;
    }

    /// <summary>
    /// Obtiene el nombre de la clase o tipo especificado por el parámetro genérico.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener el nombre.</typeparam>
    /// <returns>El nombre del tipo especificado como una cadena.</returns>
    /// <remarks>
    /// Este método utiliza reflexión para obtener el nombre del tipo y puede ser útil en situaciones donde se necesita
    /// trabajar con tipos genéricos y se desea obtener su representación en forma de cadena.
    /// </remarks>
    /// <seealso cref="GetName(Type)"/>
    public static string GetName<T>() {
        return GetName( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el nombre de la cadena de conexión asociada a un tipo dado.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el nombre de la cadena de conexión.</param>
    /// <returns>
    /// El nombre de la cadena de conexión si se encuentra un atributo <see cref="ConnectionStringNameAttribute"/> 
    /// asociado al tipo; de lo contrario, devuelve el nombre del tipo. Si el tipo es <c>null</c>, devuelve <c>null</c>.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado tiene un atributo <see cref="ConnectionStringNameAttribute"/> 
    /// y devuelve el nombre especificado en dicho atributo. Si no se encuentra el atributo o si el nombre está vacío, 
    /// se devuelve el nombre del tipo.
    /// </remarks>
    /// <seealso cref="ConnectionStringNameAttribute"/>
    public static string GetName( Type type ) {
        if ( type == null )
            return null;
        var attribute = type.GetCustomAttribute<ConnectionStringNameAttribute>();
        return attribute == null || attribute.Name.IsEmpty() ? type.Name : attribute.Name;
    }
}