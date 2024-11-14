namespace Util.Microservices; 

/// <summary>
/// Atributo que se utiliza para marcar clases o métodos que están relacionados con el sistema de 
public class PubsubNameAttribute : Attribute {
    /// <summary>
    /// Obtiene el nombre asociado a la instancia.
    /// </summary>
    /// <remarks>
    /// Esta propiedad es de solo lectura y se utiliza para acceder al nombre sin permitir su modificación.
    /// </remarks>
    /// <value>
    /// Un string que representa el nombre.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="PubsubNameAttribute"/>.
    /// </summary>
    /// <param name="name">El nombre que se asignará al atributo.</param>
    public PubsubNameAttribute( string name ) {
        Name = name;
    }

    /// <summary>
    /// Obtiene el nombre del tipo especificado por el parámetro genérico.
    /// </summary>
    /// <typeparam name="T">El tipo del cual se desea obtener el nombre.</typeparam>
    /// <returns>El nombre del tipo como una cadena.</returns>
    /// <remarks>
    /// Este método utiliza la reflexión para obtener el nombre del tipo.
    /// </remarks>
    /// <seealso cref="System.Type"/>
    public static string GetName<T>() {
        return GetName( typeof( T ) );
    }

    /// <summary>
    /// Obtiene el nombre asociado a un tipo a través del atributo <see cref="PubsubNameAttribute"/>.
    /// </summary>
    /// <param name="type">El tipo del cual se desea obtener el nombre.</param>
    /// <returns>
    /// Devuelve el nombre definido en el atributo <see cref="PubsubNameAttribute"/> si está presente y no está vacío; 
    /// de lo contrario, devuelve "pubsub". Si el tipo es nulo, devuelve null.
    /// </returns>
    /// <remarks>
    /// Este método verifica si el tipo proporcionado tiene un atributo <see cref="PubsubNameAttribute"/> 
    /// y devuelve el nombre especificado en dicho atributo. Si el atributo no está presente o su nombre está vacío, 
    /// se utiliza un valor por defecto.
    /// </remarks>
    /// <seealso cref="PubsubNameAttribute"/>
    public static string GetName( Type type ) {
        if ( type == null )
            return null;
        var attribute = type.GetCustomAttribute<PubsubNameAttribute>();
        return attribute == null || attribute.Name.IsEmpty() ? "pubsub" : attribute.Name;
    }
}